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
        Deserialization deserialization = null;
        public MainForm()
        {
            InitializeComponent();
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
                formatter = new BinaryFormatter();
                dynamic result = formatter.Deserialize(stream);
                deserialization = new Deserialization(this, result);
                deserialization.DisplayObject(result);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"{exception.Message}\nDid you forget to load a DLL?\n(For Unity games, load AssemblyCSharp.dll)", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine(exception.StackTrace);
            }
            stream.Close();
        }

        private void openDLL_FileOk(object sender, CancelEventArgs e)
        {
            string path = Path.GetDirectoryName(openDLL.FileName);
            foreach (string f in Directory.GetFiles(path))
            {
                // Load all dlls in folder, to make sure no dependencies are lost (dunno if this is even useful. oh well)
                if (f.EndsWith(".dll") && !f.ToLower().Contains("firstpass")) // Get only dlls, and ignore unity's weird firstpass dll
                {
                    AppDomain.CurrentDomain.AssemblyResolve += new AssemblyResolver(Path.GetFileNameWithoutExtension(f) , f).AssemblyResolve;
                }
            }
        }

        private void loadDLLBtn_Click(object sender, EventArgs e)
        {
            openDLL.ShowDialog(this);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (deserialization == null)
            {
                MessageBox.Show(this, "You need to deserialize a binary file first!", "No Loaded Data");
                return;
            }
            saveDialog.ShowDialog(this);
        }

        private void saveDialog_FileOk(object sender, CancelEventArgs e)
        {
            deserialization.Save(saveDialog.FileName);
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
