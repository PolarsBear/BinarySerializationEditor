using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MetroFramework.Controls;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.IO;

namespace BinarySerializationEditor
{
    public class Display
    {
        public enum ValueType
        {
            Field,
            DictEntry,
            ListItem
        }

        Color backGreen = Color.FromArgb(232, 255, 229);
        Color backRed = Color.FromArgb(255, 229, 229);


        // Seems i'm gonna have to be *dramatic pause* ORGANIZED *lightining strike*


        public SerializationElement topElement; // First element in hierarchy
        public SerializationElement currentElement; // Selected element

        public dynamic origin; // Raw deserialized object

        MainForm main; // Form for editing
        BinaryFormatter formatter = new BinaryFormatter(); // Formatter for (de)serializing

        public static int currentY = 0; // Determines y position of future GUI elements
        

        public Display(MainForm main, string path)
        {
            this.main = main;
            FileStream file = File.OpenRead(path);

            origin = formatter.Deserialize(file);

            file.Close();

            topElement = new SerializationElement("Top", origin, SerializationElement.OriginType.TopLevel, null);
            currentElement = topElement;

            DisplayElement(currentElement);
        }

        public void ResetForNewDisplay()
        {
            currentY = 0;
            main.objectView.Controls.Clear();
            main.fieldNameTooltip.RemoveAll();
        }

        public void DisplayElement(SerializationElement element)
        {
            currentElement = element;
            ResetForNewDisplay();


        }

        public class ElementGUI
        {
            MetroLabel label;
            MetroTextBox textBox;
            MetroButton expandBtn;

            public ElementGUI(SerializationElement element)
            {
                label = new MetroLabel();
                label.AutoSize = false;
                label.Location = new Point(5, 5 + currentY * 25);
                label.Name = "fieldLabel";
                label.Size = new Size(81, 23);
                label.TabIndex = 0;

                textBox = new MetroTextBox();
                textBox.Location = new Point(90, 5 + currentY * 25);
                textBox.Name = "valueTextBox";
                textBox.Size = new Size(106, 23);
                textBox.TabIndex = 1;
                textBox.ReadOnly = false;
                textBox.UseStyleColors = true;
                textBox.CustomBackground = true;
                textBox.BackColor = Color.FromKnownColor(KnownColor.Window);

                if (element.classification != SerializationElement.Classification.Primitive)
                {
                    textBox.ReadOnly = true;

                    expandBtn = new MetroButton();
                    expandBtn.Location = new Point(200, 5 + currentY * 25);
                    expandBtn.Name = "expandBtn";
                    expandBtn.Size = new Size(23, 23);
                    expandBtn.TabIndex = 0;
                    expandBtn.Text = "+";
                    expandBtn.Highlight = true;
                }
            }
        }
        


        public Tuple<MetroLabel, MetroTextBox, MetroButton> CreateGenericGUI(bool moreData)
        {
            MetroLabel label = new MetroLabel();
            label.AutoSize = false;
            label.Location = new Point(5, 5 + currentY * 25);
            label.Name = "fieldLabel";
            label.Size = new Size(81, 23);
            label.TabIndex = 0;

            MetroTextBox textBox = new MetroTextBox();
            textBox.Location = new Point(90, 5 + currentY * 25);
            textBox.Name = "valueTextBox";
            textBox.Size = new Size(106, 23);
            textBox.TabIndex = 1;
            textBox.ReadOnly = false;
            textBox.UseStyleColors = true;
            textBox.CustomBackground = true;
            textBox.BackColor = Color.FromKnownColor(KnownColor.Window);

            MetroButton expandBtn = null;

            if (moreData)
            {
                textBox.ReadOnly = true;

                expandBtn = new MetroButton();
                expandBtn.Location = new Point(200, 5 + currentY * 25);
                expandBtn.Name = "expandBtn";
                expandBtn.Size = new Size(23, 23);
                expandBtn.TabIndex = 0;
                expandBtn.Text = "+";
                expandBtn.Highlight = true;
            }

            currentY++;

            return new Tuple<MetroLabel, MetroTextBox, MetroButton>(label, textBox, expandBtn);
        }
    }
}
