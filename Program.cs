using System;
using System.IO;

namespace Discord_Splitter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DiscordSplitter";
            Console.WriteLine("DiscordSplitter V3.0");
            string[] folders = { "Input", "Output", "MergeInput", "MergeOutput" };
            foreach (var folder in folders)
            {
                Console.WriteLine($"Missing Directory Created: {folder}");
                Directory.CreateDirectory(folder);
            }

            bool programActive = true;
            bool fileSplitter = true;
            double fileSplitterValue = 9.5;
            ConvertionEngine engine = new();
            while (programActive)
            {
                Console.WriteLine(" (A) Convert/Merge Files. (B) Options. (C) Exit. ");
                Console.WriteLine($"FileSplitter ({fileSplitter}) ({fileSplitterValue} MB).");
                string choice = (Console.ReadLine() ?? "").Trim().ToLower();

                switch (choice)
                {
                    case "a":
                        Console.WriteLine(" (A) Convert Files. (B) Merge Files.");
                        choice = (Console.ReadLine() ?? "").Trim().ToLower();
                        switch (choice)
                        {
                            case "a":
                                engine.StartEngine(fileSplitter, fileSplitterValue);
                                break;
                            case "b":
                                engine.MergeFiles();
                                break;
                        }
                            break;

                    case "b":
                        Console.WriteLine($"FileSplitter ({fileSplitter}) ({fileSplitterValue} MB).");
                        Console.WriteLine(" (A) Enable/Disable FileSplitter. (B) Set value in MB.");
                        choice = (Console.ReadLine() ?? "").Trim().ToLower();
                        switch (choice)
                        {
                            case "a":
                                fileSplitter = !fileSplitter;
                                Console.WriteLine($"FileSplitter is now {fileSplitter}");
                                break;

                            case "b":
                                Console.WriteLine("Write value in MB, Example: 8 (Will set it to 8 MB, Minimum is 1 MB)");
                                choice = (Console.ReadLine() ?? "9.5");
                                if (double.TryParse(choice, out fileSplitterValue))
                                {
                                    fileSplitterValue = (fileSplitterValue >= 1.0) ? fileSplitterValue : 1.0;
                                    Console.WriteLine($"You've chosen {fileSplitterValue} MB");
                                }
                                else
                                {
                                    Console.WriteLine("Wrong value picked, Defaulting to 9.5 MB");
                                    fileSplitterValue = 9.5;
                                }
                                break;
                        }
                        break;

                    case "c":
                        programActive = false;
                        break;
                }

            }

        }
    }
}