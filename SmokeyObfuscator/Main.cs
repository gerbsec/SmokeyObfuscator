using dnlib.DotNet.Writer;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SmokeyObfuscator.Protections;

namespace SmokeyObfuscator
{
    public partial class Main : Form
    {
        private string selectedDirectory;

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select Directory with Binaries to Obfuscate";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedDirectory = folderDialog.SelectedPath;
                    textBox1.Text = $"Selected Directory: {selectedDirectory}";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedDirectory))
            {
                MessageBox.Show("Please select a directory first.");
                return;
            }

            List<string> skippedFiles = new List<string>();

            try
            {
                // Get all .exe files from the selected directory
                string[] files = Directory.GetFiles(selectedDirectory, "*.exe");

                foreach (string file in files)
                {
                    try
                    {
                        // Load file into memory
                        byte[] fileBytes = File.ReadAllBytes(file);

                        // Load module from memory
                        ModuleDefMD module;
                        using (var ms = new MemoryStream(fileBytes))
                        {
                            module = ModuleDefMD.Load(ms);
                        }

                        // Perform obfuscation
                        NumberChanger.Process(module);
                        Strings.Execute(module);
                        ProxyInts.Execute(module);
                        HideMethods.Execute(module);

                        // Save the obfuscated file back to disk
                        SaveFile(module, file);
                    }
                    catch (Exception ex)
                    {
                        // Add the file to the list of skipped files
                        skippedFiles.Add(file);
                        Console.WriteLine($"Error processing {file}: {ex.Message}");
                    }
                }

                // Display results
                if (skippedFiles.Count > 0)
                {
                    MessageBox.Show($"Obfuscation completed with errors. Skipped files:\n{string.Join("\n", skippedFiles)}");
                }
                else
                {
                    MessageBox.Show("Obfuscation completed for all files in the directory.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void SaveFile(ModuleDefMD module, string path)
        {
            var opts = new ModuleWriterOptions(module);
            opts.Logger = DummyLogger.NoThrowInstance;

            // Save to a temporary file first
            string tempPath = path + ".tmp";
            module.Write(tempPath, opts);

            // Overwrite the original file with the temporary file
            File.Copy(tempPath, path, true);
            File.Delete(tempPath);
        }
    }
}
