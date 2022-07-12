using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MetroFramework.Controls;
using System.Windows.Forms;

namespace BinarySerializationEditor
{
    public class Deserialization
    {
        MainForm main;
        public int currentY = 0;

        public Deserialization(MainForm main)
        {
            this.main = main;
        }

        public void ResetForNewDeserialization()
        {
            currentY = 0;
            //main.objectView.Controls.Clear();
        }

        public Tuple<MetroLabel, MetroTextBox, MetroButton> CreateFieldUI(dynamic obj)
        {
            // I'll make this later
            return null;
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
            if (!isSimple)
            {
                main.objectView.Controls.Add(moreDataBtn);
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

            MetroButton moreDataBtn = null;

            if (moreData)
            {
                moreDataBtn = new MetroButton();
                moreDataBtn.Location = new Point(200, 5 + currentY * 25);
                moreDataBtn.Name = "moreDataBtn";
                moreDataBtn.Size = new Size(23, 23);
                moreDataBtn.TabIndex = 0;
                moreDataBtn.Text = "+";
                //moreDataBtn.Font = new Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            currentY++;

            return new Tuple<MetroLabel, MetroTextBox, MetroButton>(label, textBox, moreDataBtn);
        }
    }
}
