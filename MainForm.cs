using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using MetroFramework.Controls;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace BinarySerializationEditor
{
    public partial class MainForm : MetroForm
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Deserialization deserialization;
        public MainForm()
        {
            InitializeComponent();
            deserialization = new Deserialization(this);
        }
        private void chooseFileBtn_Click(object sender, EventArgs e)
        {
            chooseFile.ShowDialog(this);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Stream stream = chooseFile.OpenFile();
            try
            {
                dynamic result = formatter.Deserialize(stream);
                Console.WriteLine(result);

                deserialization.ResetForNewDeserialization();

                foreach(dynamic keyValue in result)
                {
                    Console.WriteLine(keyValue);
                    deserialization.CreateDictEntryUI(keyValue);
                }
                foreach (dynamic keyValue in result)
                {
                    Console.WriteLine(keyValue);
                    deserialization.CreateDictEntryUI(keyValue);
                }
                foreach (dynamic keyValue in result)
                {
                    Console.WriteLine(keyValue);
                    deserialization.CreateDictEntryUI(keyValue);
                }
                foreach (dynamic keyValue in result)
                {
                    Console.WriteLine(keyValue);
                    deserialization.CreateDictEntryUI(keyValue);
                }
            }
            catch (System.Runtime.Serialization.SerializationException exception)
            {
                //[TODO] Proper error display system
                // Why couldn't they add more info in SerializationException? It doesn't specify what the issue is... And expects programmers to do string parsing gymnastics just to extract simple data
                string typeName = exception.Message.Replace("Unable to load type ", "").Replace(" required for deserialization.", ""); // I'll improve this later, too lazy rn
                Console.WriteLine(typeName);
                MessageBox.Show(this, $"The type {typeName} is missing.\nDid you forget to load a DLL?\n(For Unity games, load AssemblyCSharp.dll)", "Required type not found!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            stream.Close();
        }

        private void openDLL_FileOk(object sender, CancelEventArgs e)
        {
            string path = Path.GetDirectoryName(openDLL.FileName);
            foreach (string f in Directory.GetFiles(path))
            {
                // Load all dlls in folder, to make sure no dependencies are lost ()
                if (f.EndsWith(".dll") && !f.ToLower().Contains("firstpass")) // Get only dlls, and ignore unity's weird firstpass dll
                {
                    Console.WriteLine(f);
                    AppDomain.CurrentDomain.AssemblyResolve += new AssemblyResolver(Path.GetFileNameWithoutExtension(f) , f).AssemblyResolve;
                }
            }
        }

        private void loadDLLBtn_Click(object sender, EventArgs e)
        {
            openDLL.ShowDialog(this);
        }
    }
}
