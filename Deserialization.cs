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
    public class Deserialization
    {
        public enum ValueType
        {
            Field,
            DictEntry,
            ListItem
        }

        Color backGreen = Color.FromArgb(232, 255, 229);
        Color backRed = Color.FromArgb(255, 229, 229);

        public Dictionary<MetroTextBox, dynamic> objects = new Dictionary<MetroTextBox, dynamic>();

        public Dictionary<MetroTextBox, bool> changed = new Dictionary<MetroTextBox, bool>();

        public Dictionary<MetroTextBox, Tuple<ValueType, dynamic>> findInfo = new Dictionary<MetroTextBox, Tuple<ValueType, dynamic>>();

        public dynamic currentObject;

        MainForm main;
        public int currentY = 0;

        public Deserialization(MainForm main)
        {
            this.main = main;
        }

        public void ResetForNewDeserialization()
        {
            currentY = 0;
            main.objectView.Controls.Clear();
            main.fieldNameTooltip.RemoveAll();
        }

        public void DisplayObject(dynamic obj)
        {
            currentObject = obj;
            ResetForNewDeserialization();
            if (obj is IEnumerable) // Is enumerable
            {
                if (obj is IDictionary) // Has two values
                {
                    foreach (dynamic keyValue in obj) // Iterate as Dictionary
                    {
                        CreateDictEntryUI(keyValue);
                    }
                }
                else // Has only one value
                {
                    int i = 0;
                    foreach (dynamic value in obj) // Iterate as list
                    {
                        CreateListEntryUI(value, i);
                        i++;
                    }
                }
            }
            else // Is object
            {
                foreach (FieldInfo field in obj.GetType().GetFields())
                {
                    CreateFieldUI(field, field.GetValue(obj));
                }
            }
        }

        BinaryFormatter formatter = new BinaryFormatter();

        public void Save(string path)
        {
            foreach(KeyValuePair<MetroTextBox, bool> keyValue in changed)
            {
                if (keyValue.Value)
                {
                    var find = findInfo[keyValue.Key];
                    switch (find.Item1)
                    {
                        case ValueType.Field:
                            FieldInfo info = (FieldInfo)find.Item2;
                            info.SetValue(currentObject, objects[keyValue.Key]);
                            break;
                        
                        case ValueType.DictEntry:
                            currentObject[find.Item2] = objects[keyValue.Key];
                            break;

                        case ValueType.ListItem:
                            currentObject[find.Item2] = objects[keyValue.Key];
                            break;
                    }
                }
            }

            FileStream file = File.OpenWrite(path);

            formatter.Serialize(file, currentObject);

            file.Close();
            
        }

        public Tuple<MetroLabel, MetroTextBox, MetroButton> CreateFieldUI(FieldInfo field, dynamic value)
        {
            bool isSimple = Utils.IsSimple(field.FieldType);
            Tuple<MetroLabel, MetroTextBox, MetroButton> tuple = CreateGenericUI(!isSimple);
            MetroLabel label = tuple.Item1;
            MetroTextBox textBox = tuple.Item2;
            MetroButton moreDataBtn = tuple.Item3;

            textBox.Text = value.ToString();
            label.Text = field.Name;

            main.fieldNameTooltip.SetToolTip(label, field.Name);

            main.objectView.Controls.Add(label);
            main.objectView.Controls.Add(textBox);

            textBox.TextChanged += delegate
            {
                bool sucess;
                objects[textBox] = Utils.TryParse(objects[textBox], textBox.Text, out sucess);
                textBox.Style = sucess ? MetroFramework.MetroColorStyle.Lime : MetroFramework.MetroColorStyle.Red;
                textBox.BackColor = sucess ? backGreen : backRed;
                changed[textBox] = sucess;
            };

            changed.Add(textBox, false);
            objects.Add(textBox, value);
            findInfo.Add(textBox, new Tuple<ValueType, object>(ValueType.Field, field));


            if (!isSimple)
            {
                main.objectView.Controls.Add(moreDataBtn);
                moreDataBtn.Click += delegate
                {
                    DisplayObject(value);
                };
            }

            return new Tuple<MetroLabel, MetroTextBox, MetroButton>(label, textBox, moreDataBtn);
        }

        public Tuple<MetroLabel, MetroTextBox, MetroButton> CreateDictEntryUI(dynamic keyValue)
        {
            bool isSimple = Utils.IsSimple(keyValue.Value.GetType());
            Tuple<MetroLabel, MetroTextBox, MetroButton> tuple = CreateGenericUI(!isSimple);
            MetroLabel label = tuple.Item1;
            MetroTextBox textBox = tuple.Item2;
            MetroButton moreDataBtn = tuple.Item3;

            textBox.Text = keyValue.Value.ToString();
            label.Text = keyValue.Key;

            main.fieldNameTooltip.SetToolTip(label, keyValue.Key);

            main.objectView.Controls.Add(label);
            main.objectView.Controls.Add(textBox);

            textBox.TextChanged += delegate
            {
                bool sucess;
                objects[textBox] = Utils.TryParse(objects[textBox], textBox.Text, out sucess);
                textBox.Style = sucess ? MetroFramework.MetroColorStyle.Lime : MetroFramework.MetroColorStyle.Red;
                textBox.BackColor = sucess ? backGreen : backRed;
                changed[textBox] = sucess;
            };

            changed.Add(textBox, false);
            objects.Add(textBox, keyValue.Value);
            findInfo.Add(textBox, new Tuple<ValueType, object>(ValueType.DictEntry, keyValue.Key));

            if (!isSimple)
            {
                main.objectView.Controls.Add(moreDataBtn);
                moreDataBtn.Click += delegate
                {
                    DisplayObject(keyValue.Value);
                };
            }

            return new Tuple<MetroLabel, MetroTextBox, MetroButton>(label, textBox, moreDataBtn);
        }

        public Tuple<MetroLabel, MetroTextBox, MetroButton> CreateListEntryUI(dynamic value, int index)
        {
            bool isSimple = Utils.IsSimple(value.GetType());
            Tuple<MetroLabel, MetroTextBox, MetroButton> tuple = CreateGenericUI(!isSimple);
            MetroLabel label = tuple.Item1;
            MetroTextBox textBox = tuple.Item2;
            MetroButton moreDataBtn = tuple.Item3;

            textBox.Text = value.ToString();
            label.Text = index.ToString();

            main.fieldNameTooltip.SetToolTip(label, index.ToString());

            main.objectView.Controls.Add(label);
            main.objectView.Controls.Add(textBox);

            textBox.TextChanged += delegate
            {
                bool sucess;
                objects[textBox] = Utils.TryParse(objects[textBox], textBox.Text, out sucess);
                textBox.Style = sucess ? MetroFramework.MetroColorStyle.Lime : MetroFramework.MetroColorStyle.Red;
                textBox.BackColor = sucess ? backGreen : backRed;
                changed[textBox] = sucess;
            };

            changed.Add(textBox, false);
            objects.Add(textBox, value);
            findInfo.Add(textBox, new Tuple<ValueType, object>(ValueType.ListItem, index));

            if (!isSimple)
            {
                main.objectView.Controls.Add(moreDataBtn);
                moreDataBtn.Click += delegate
                {
                    DisplayObject(value);
                };
            }

            return new Tuple<MetroLabel, MetroTextBox, MetroButton>(label, textBox, moreDataBtn);
        }

        public Tuple<MetroLabel, MetroTextBox, MetroButton> CreateGenericUI(bool moreData)
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

            MetroButton moreDataBtn = null;

            if (moreData)
            {
                textBox.ReadOnly = true;

                moreDataBtn = new MetroButton();
                moreDataBtn.Location = new Point(200, 5 + currentY * 25);
                moreDataBtn.Name = "moreDataBtn";
                moreDataBtn.Size = new Size(23, 23);
                moreDataBtn.TabIndex = 0;
                moreDataBtn.Text = "+";
                moreDataBtn.Highlight = true;
                //moreDataBtn.Font = new Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            currentY++;

            return new Tuple<MetroLabel, MetroTextBox, MetroButton>(label, textBox, moreDataBtn);
        }
    }
}
