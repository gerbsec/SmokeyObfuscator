using System;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using SmokeyObfuscator.Protections;

namespace SmokeyObfuscator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Application started.");

            if (args.Length == 0 || args[0] == "-h")
            {
                Console.WriteLine("No arguments provided or help requested.");
                ShowHelp();
                return;
            }
                
            switch (args[0])
            {
                case "-d":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Directory path required.");
                        return;
                    }
                    Console.WriteLine($"Obfuscating directory: {args[1]}");
                    ObfuscateDirectory(args[1]);
                    break;
                case "-f":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("File path required.");
                        return;
                    }
                    Console.WriteLine($"Obfuscating file: {args[1]}");
                    ObfuscateFile(args[1]);
                    break;
                default:
                    Console.WriteLine("Invalid argument. Use -h for help.");
                    break;
            }
        }


        static void ShowHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  -d <directory>  Obfuscate all executables in the specified directory.");
            Console.WriteLine("  -f <file>       Obfuscate a single executable.");
            Console.WriteLine("  -h              Show this help message.");
        }

        static void ObfuscateDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.exe");
            foreach (string file in files)
            {
                ObfuscateFile(file);
            }
        }

        static void ObfuscateFile(string filePath)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                ModuleDefMD module;
                using (var ms = new MemoryStream(fileBytes))
                {
                    module = ModuleDefMD.Load(ms);
                }

                NumberChanger.Process(module);
                Strings.Execute(module);
                ProxyInts.Execute(module);
                HideMethods.Execute(module);

                SaveFile(module, filePath);
                Console.WriteLine($"Obfuscated: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {filePath}: {ex.Message}");
            }
        }

        static void SaveFile(ModuleDefMD module, string path)
        {
            var opts = new ModuleWriterOptions(module)
            {
                Logger = DummyLogger.NoThrowInstance
            };
            string tempPath = path + ".tmp";
            module.Write(tempPath, opts);
            File.Copy(tempPath, path, true);
            File.Delete(tempPath);
        }
    }
}
