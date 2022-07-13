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

        public int currentY = 0; // Determines y position of future GUI elements


        public Control.ControlCollection viewControls;
        

        public Display(MainForm main, string path)
        {
            this.main = main;
            FileStream file = File.OpenRead(path);

            origin = formatter.Deserialize(file);

            file.Close();

            topElement = new SerializationElement("Top", origin, SerializationElement.OriginType.TopLevel, null);
            currentElement = topElement;

            viewControls = main.objectView.Controls;

            DisplayElement();
        }

        public void ResetForNewDisplay()
        {
            currentY = 0;
            viewControls.Clear();
            main.fieldNameTooltip.RemoveAll();
        }

        public void DisplayElement()
        {
            ResetForNewDisplay();

            foreach (KeyValuePair<string, SerializationElement> kvp in currentElement.children)
            {
                var gui = new ElementGUI(kvp.Value, this);

                viewControls.Add(gui.label);
                viewControls.Add(gui.textBox);
                if (gui.hasBtn)
                    viewControls.Add(gui.expandBtn);
            }
        }

        public class ElementGUI
        {
            public MetroLabel label;
            public MetroTextBox textBox;
            public MetroButton expandBtn;

            public bool hasBtn = false;

            public SerializationElement element;

            public ElementGUI(SerializationElement element, Display display)
            {
                this.element = element;

                label = new MetroLabel();
                label.AutoSize = false;
                label.Location = new Point(5, 5 + display.currentY * 25);
                label.Name = "fieldLabel";
                label.Size = new Size(81, 23);
                label.TabIndex = 0;

                label.Text = element.name;

                textBox = new MetroTextBox();
                textBox.Location = new Point(90, 5 + display.currentY * 25);
                textBox.Name = "valueTextBox";
                textBox.Size = new Size(106, 23);
                textBox.TabIndex = 1;
                textBox.ReadOnly = false;
                textBox.UseStyleColors = true;
                textBox.CustomBackground = true;
                textBox.BackColor = Color.FromKnownColor(KnownColor.Window);

                textBox.Text = Convert.ToString(element.value);

                if (element.classification != SerializationElement.Classification.Primitive)
                {
                    textBox.ReadOnly = true;

                    expandBtn = new MetroButton();
                    expandBtn.Location = new Point(200, 5 + display.currentY * 25);
                    expandBtn.Name = "expandBtn";
                    expandBtn.Size = new Size(23, 23);
                    expandBtn.TabIndex = 0;
                    expandBtn.Text = "+";
                    expandBtn.Highlight = true;

                    hasBtn = true;

                    expandBtn.Click += delegate
                    {
                        display.currentElement = element;
                        display.DisplayElement();
                    };
                }

                display.currentY++;
            }
        }
       
    }
}
