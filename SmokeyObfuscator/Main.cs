using dnlib.DotNet.Writer;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using SmokeyObfuscator.Protections;

namespace SmokeyObfuscator
{
    public partial class Main : Form
    {
        private static string rawFileLocation;
        public Main()
        {
            InitializeComponent();
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length == 1)
            {
                string droppedFile = files[0];
                if (Path.GetExtension(droppedFile).Equals(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    rawFileLocation = droppedFile;
                    textBox1.Text = $"File Selected: {rawFileLocation}";
                }
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "SmokeyObfuscator";
                openFileDialog.Filter = "Executable (*.exe)|*.exe";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.FileNames.Length == 1)
                    {
                        rawFileLocation = openFileDialog.FileName;
                        textBox1.Text = $"File Selected: {rawFileLocation}";
                    }
                }
                openFileDialog.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ModuleDefMD module = ModuleDefMD.Load(rawFileLocation);
            NumberChanger.Process(module);
            Strings.Execute(module);
            ProxyInts.Execute(module);
            HideMethods.Execute(module);
            SaveFile(module, rawFileLocation);
        }

        private void SaveFile(ModuleDefMD module, string path)
        {
            string AssemblyPath = Path.GetDirectoryName(module.Location);
            if (!AssemblyPath.EndsWith("\\")) AssemblyPath += "\\";
            string savePath = AssemblyPath + Path.GetFileNameWithoutExtension(module.Location) + "_Obfuscated" + Path.GetExtension(module.Location);
            var opts = new ModuleWriterOptions(module);
            opts.Logger = DummyLogger.NoThrowInstance;
            module.Write(savePath, opts);
        }
    }
}
