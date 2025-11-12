using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Splitter
{
    internal class ConvertionEngine
    {
        private readonly string inputFolder = "Input";
        private readonly string outputFolder = "Output";
        private readonly string mergeinputFolder = "MergeInput";
        private readonly string mergeoutputFolder = "MergeOutput";
        public void StartEngine(bool splitter = true, double splitterValueMB = 9.5)
        {
            long splitterValue = (long)(splitterValueMB * 1024 * 1024);

            foreach (var file in Directory.EnumerateFiles(inputFolder, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(inputFolder, file);
                string fileName = Path.GetFileName(file);
                string fileExtension = Path.GetExtension(file);

                string outputPath = Path.Combine(outputFolder, $"{relativePath}.bin");
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");

                long fileSize = new FileInfo(file).Length;

                if (splitter == false || fileSize <= splitterValue)
                {
                    using (FileStream source = File.OpenRead(file))
                    using (FileStream destination = File.Create(outputPath))
                    {
                        byte[] buffer = new byte[1024 * 1024];
                        int bytesRead;
                        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            destination.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                else
                {
                    string basefolderName = Path.GetFileNameWithoutExtension(file);
                    string basefolderPath = Path.Combine(outputFolder, basefolderName);
                    Directory.CreateDirectory(basefolderPath);

                    using FileStream inputStream = File.OpenRead(file);
                    byte[] buffer = new byte[splitterValue];
                    int partIndex = 0;
                    int bytesRead;

                    while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string partFileName = $"{fileName}_part{partIndex:D3}.bin";
                        string parthPath = Path.Combine(basefolderPath, partFileName);

                        using FileStream partStream = File.Create(parthPath);
                        partStream.Write(buffer, 0, bytesRead);
                        partIndex++;
                    }
                }

            }
        }

        public void MergeFiles()
        {
            foreach (var dir in Directory.EnumerateDirectories(mergeinputFolder))
            {
                string[] partFiles = Directory.GetFiles(dir, "*_part*.bin", SearchOption.TopDirectoryOnly);
                if (partFiles.Length == 0)
                {
                    continue;
                }

                Array.Sort(partFiles);

                string firstPartName = Path.GetFileName(partFiles[0]);
                string originalName = firstPartName.Split("_part")[0];

                string outputPath = Path.Combine(mergeoutputFolder, originalName);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");

                using (FileStream outputStream = File.Create(outputPath))
                {
                    foreach (var part in partFiles)
                    {
                        using FileStream inputStream = File.OpenRead(part);
                        inputStream.CopyTo(outputStream);

                    }
                }

            }
            foreach (var file in Directory.EnumerateFiles(mergeinputFolder, "*.bin", SearchOption.AllDirectories))
            {
                string name = Path.GetFileName(file);
                if (name.Contains("_part"))
                {
                    continue;
                }

                string restoredName = Path.GetFileNameWithoutExtension(file);
                string outputPath = Path.Combine(mergeoutputFolder, restoredName);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");

                File.Copy(file, outputPath, true);
            }
        }
    }
}
