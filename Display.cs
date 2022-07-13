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

        public static Color backGreen = Color.FromArgb(225, 255, 210);
        public static Color backRed = Color.FromArgb(255, 229, 229);
        public static Color backWhite = Color.FromKnownColor(KnownColor.Window);


        // Seems i'm gonna have to be *dramatic pause* ORGANIZED *lightining strike*


        public SerializationElement topElement; // First element in hierarchy
        public SerializationElement currentElement; // Selected element

        public dynamic origin; // Raw deserialized object

        MainForm main; // Form for editing
        BinaryFormatter formatter = new BinaryFormatter(); // Formatter for (de)serializing

        public int currentY = 0; // Determines y position of future GUI elements


        public Control.ControlCollection viewControls;

        public static Display instance;
        

        public Display(MainForm main, string path)
        {
            instance = this;

            this.main = main;
            FileStream file = File.OpenRead(path);

            origin = formatter.Deserialize(file);

            file.Close();

            topElement = new SerializationElement("Top", origin, SerializationElement.OriginType.TopLevel, null, null);
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
                viewControls.Add(gui.comboBox);
                viewControls.Add(gui.expandBtn);
            }
            main.backButton.Visible = currentElement != topElement;
        }

        public class ElementGUI
        {
            public MetroLabel label;
            public MetroTextBox textBox;
            public ComboBox comboBox;
            public MetroButton expandBtn;

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

                if (element.value is bool)
                {
                    comboBox = GenericComboBox();
                    comboBox.Location = new Point(90, 5 + display.currentY * 25);

                    comboBox.Items.Add(true);
                    comboBox.Items.Add(false);

                    comboBox.SelectedItem = element.value;

                    comboBox.SelectedIndexChanged += delegate
                    {
                        element.value = comboBox.SelectedItem;
                        Console.WriteLine($"{element.value} == {element.originalValue}");
                        comboBox.BackColor = (element.value == element.originalValue) ? backWhite : backGreen;
                    };
                }
                else if (element.classification == SerializationElement.Classification.Enum)
                {
                    comboBox = GenericComboBox();
                    comboBox.Location = new Point(90, 5 + display.currentY * 25);
                    string[] enumNames = element.value.GetType().GetEnumNames();
                    Array enumValues = element.value.GetType().GetEnumValues();
                    for (int i = 0; i < enumValues.Length; i++)
                    {
                        var member = new EnumMember(enumNames[i], enumValues.GetValue(i));
                        comboBox.Items.Add(member);
                        if (member.value == element.value)
                        {
                            comboBox.SelectedItem = member;
                        }
                    }
                    

                    comboBox.SelectedIndexChanged += delegate
                    {
                        element.value = ((EnumMember)comboBox.SelectedItem).value;
                        Console.WriteLine($"{element.value} == {element.originalValue}");
                        comboBox.BackColor = (element.value == element.originalValue) ? backWhite : backGreen;
                    };
                }
                else
                {

                    textBox = new MetroTextBox();
                    textBox.Location = new Point(90, 5 + display.currentY * 25);
                    textBox.Name = "valueTextBox";
                    textBox.Size = new Size(106, 23);
                    textBox.TabIndex = 1;
                    textBox.ReadOnly = false;
                    textBox.CustomBackground = true;
                    textBox.BackColor = Color.FromKnownColor(KnownColor.Window);

                    textBox.Text = Convert.ToString(element.value);

                    textBox.TextChanged += delegate
                    {
                        element.valueStr = textBox.Text;
                        element.StringEdited();

                        ChangedValue();
                    };
                }

                if (element.classification != SerializationElement.Classification.Primitive && element.classification != SerializationElement.Classification.Enum)
                {
                    if (textBox != null)
                        textBox.ReadOnly = true;

                    expandBtn = new MetroButton();
                    expandBtn.Location = new Point(200, 5 + display.currentY * 25);
                    expandBtn.Name = "expandBtn";
                    expandBtn.Size = new Size(23, 23);
                    expandBtn.TabIndex = 0;
                    expandBtn.Text = "+";
                    expandBtn.Highlight = true;
                    expandBtn.Style = MetroFramework.MetroColorStyle.Yellow;

                    expandBtn.Click += delegate
                    {
                        display.currentElement = element;
                        display.DisplayElement();
                    };
                }

                display.currentY++;

                ChangedValue();
            }

            public void ChangedValue()
            {
                if (element.classification == SerializationElement.Classification.Primitive || element.classification == SerializationElement.Classification.Enum)
                {
                    if (comboBox != null)
                        comboBox.BackColor = (element.value == element.originalValue) ? backWhite : backGreen;
                    if (textBox != null)
                    {
                        if (!element.strToObjValid)
                        {
                            textBox.BackColor = backRed;
                        }
                        else
                        {
                            textBox.BackColor = (element.value == element.originalValue) ? backWhite : backGreen;
                        }
                    }
                }
                else
                {
                    textBox.BackColor = (element.value == element.originalValue) ? backWhite : backGreen;
                }
            }

            public static ComboBox GenericComboBox()
            {
                ComboBox comboBox = new ComboBox();
                
                comboBox.Name = "valueComboBox";
                comboBox.Size = new Size(106, 23);
                comboBox.FormattingEnabled = true;
                comboBox.ItemHeight = 23;
                comboBox.BackColor = Color.FromKnownColor(KnownColor.Window);
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.FlatStyle = FlatStyle.Popup;

                return comboBox;
            }
        }
       
    }
}
