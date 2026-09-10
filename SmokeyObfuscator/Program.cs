using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using SmokeyObfuscator.Protections;

namespace SmokeyObfuscator
{
    internal static class Program
    {
        private static readonly string[] SupportedExtensions = new[] { ".exe", ".dll" };

        private static int Main(string[] args)
        {
            try
            {
                var options = ParseArguments(args);
                if (options.ShowHelp)
                {
                    ShowHelp();
                    return 0;
                }

                if (string.IsNullOrWhiteSpace(options.TargetPath))
                {
                    Console.Error.WriteLine("A target file or directory is required. Use -h for help.");
                    return 1;
                }

                if (options.IsDirectory)
                {
                    ObfuscateDirectory(options.TargetPath, options.OutputDirectory);
                }
                else
                {
                    ObfuscateFile(options.TargetPath, options.OutputDirectory);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("SmokeyObfuscator failed: " + ex.Message);
                return 1;
            }
        }

        private static CommandOptions ParseArguments(string[] args)
        {
            var options = new CommandOptions();

            for (int i = 0; i < args.Length; i++)
            {
                string current = args[i];

                switch (current)
                {
                    case "-h":
                    case "--help":
                        options.ShowHelp = true;
                        break;
                    case "-d":
                        options.IsDirectory = true;
                        if (i + 1 >= args.Length)
                            throw new ArgumentException("Directory path required after -d.");
                        options.TargetPath = args[++i];
                        break;
                    case "-f":
                        options.IsDirectory = false;
                        if (i + 1 >= args.Length)
                            throw new ArgumentException("File path required after -f.");
                        options.TargetPath = args[++i];
                        break;
                    case "-o":
                        if (i + 1 >= args.Length)
                            throw new ArgumentException("Output path required after -o.");
                        options.OutputDirectory = args[++i];
                        break;
                    default:
                        if (string.IsNullOrWhiteSpace(options.TargetPath))
                        {
                            options.TargetPath = current;
                            options.IsDirectory = Directory.Exists(current);
                        }
                        else
                        {
                            throw new ArgumentException("Unexpected argument: " + current);
                        }
                        break;
                }
            }

            return options;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("SmokeyObfuscator");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  SmokeyObfuscator.exe -d <directory> [-o <output-directory>] ");
            Console.WriteLine("  SmokeyObfuscator.exe -f <file> [-o <output-directory>] ");
            Console.WriteLine("  SmokeyObfuscator.exe -h");
            Console.WriteLine();
            Console.WriteLine("Notes:");
            Console.WriteLine("  -d obfuscates every supported executable in a directory.");
            Console.WriteLine("  -f obfuscates a single .exe or .dll file.");
            Console.WriteLine("  -o writes the transformed files into a separate output directory.");
        }

        private static void ObfuscateDirectory(string directory, string outputDirectory = null)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("Directory not found: " + directory);

            var files = Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                .Where(path => SupportedExtensions.Contains(Path.GetExtension(path).ToLowerInvariant()))
                .ToList();

            if (files.Count == 0)
            {
                Console.WriteLine("No supported .exe/.dll files were found in: " + directory);
                return;
            }

            foreach (string file in files)
            {
                ObfuscateFile(file, outputDirectory);
            }
        }

        private static void ObfuscateFile(string filePath, string outputDirectory = null)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found: " + filePath);

            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            if (!SupportedExtensions.Contains(extension))
                throw new InvalidOperationException("Unsupported file type: " + filePath);

            string destinationPath = ResolveOutputPath(filePath, outputDirectory);
            string directory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            byte[] fileBytes = File.ReadAllBytes(filePath);
            ModuleDefMD module;
            using (var stream = new MemoryStream(fileBytes))
            {
                module = ModuleDefMD.Load(stream);
            }

            ObfuscationPipeline.Execute(module);
            SaveFile(module, destinationPath);
            Console.WriteLine("Obfuscated: " + destinationPath);
        }

        private static string ResolveOutputPath(string sourcePath, string outputDirectory)
        {
            if (string.IsNullOrWhiteSpace(outputDirectory))
                return sourcePath;

            string fileName = Path.GetFileName(sourcePath);
            return Path.Combine(outputDirectory, fileName);
        }

        private static void SaveFile(ModuleDefMD module, string path)
        {
            var options = new ModuleWriterOptions(module)
            {
                Logger = DummyLogger.NoThrowInstance
            };

            string tempPath = path + ".tmp";
            module.Write(tempPath, options);
            File.Copy(tempPath, path, true);
            File.Delete(tempPath);
        }

        private class CommandOptions
        {
            public bool ShowHelp { get; set; }
            public bool IsDirectory { get; set; }
            public string TargetPath { get; set; }
            public string OutputDirectory { get; set; }
        }
    }
}
