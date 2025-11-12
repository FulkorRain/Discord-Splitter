using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Splitter
{
    internal class ConvertionEngine
    {
        private readonly string inputFolder = "Input";
        private readonly string outputFolder = "Output";
        public void StartEngine(bool splitter = true, double splitterValueMB = 9.5)
        {
            long splitterValue = (long)(splitterValueMB * 1024 * 1024);

            foreach (var file in Directory.EnumerateFiles(inputFolder, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(inputFolder, file);
                string fileName = Path.GetFileName(file);
                string fileExtension = Path.GetExtension(file);

                string outputPath = Path.Combine(outputFolder, $"{relativePath}.bin");
                long fileSize = new FileInfo(file).Length;

                if (splitter == false || fileSize <= splitterValue)
                {
                    byte[] bytes = File.ReadAllBytes(file);
                    File.WriteAllBytes(outputPath, bytes);
                }
                else
                {
                    // TODO: Splitter.
                }

            }
        }
    }
}
