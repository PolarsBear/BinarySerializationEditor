
namespace BinarySerializationEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.styleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.chooseFileBtn = new MetroFramework.Controls.MetroButton();
            this.chooseFile = new System.Windows.Forms.OpenFileDialog();
            this.openDLL = new System.Windows.Forms.OpenFileDialog();
            this.loadDLLBtn = new MetroFramework.Controls.MetroButton();
            this.objectView = new System.Windows.Forms.Panel();
            this.fieldNameTooltip = new MetroFramework.Components.MetroToolTip();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.styleManager)).BeginInit();
            this.SuspendLayout();
            // 
            // styleManager
            // 
            this.styleManager.Owner = this;
            this.styleManager.Style = MetroFramework.MetroColorStyle.Yellow;
            this.styleManager.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // chooseFileBtn
            // 
            this.chooseFileBtn.Highlight = true;
            this.chooseFileBtn.Location = new System.Drawing.Point(308, 79);
            this.chooseFileBtn.Name = "chooseFileBtn";
            this.chooseFileBtn.Size = new System.Drawing.Size(105, 35);
            this.chooseFileBtn.Style = MetroFramework.MetroColorStyle.Yellow;
            this.chooseFileBtn.TabIndex = 1;
            this.chooseFileBtn.Text = "Choose File";
            this.chooseFileBtn.Click += new System.EventHandler(this.chooseFileBtn_Click);
            // 
            // chooseFile
            // 
            this.chooseFile.InitialDirectory = "C:\\Users\\larzi\\AppData\\LocalLow\\Deli Interactive\\We Need to Go Deeper\\76561198188" +
    "617354";
            this.chooseFile.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // openDLL
            // 
            this.openDLL.InitialDirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\WeNeedtoGoDeeper\\WeNeedToGoDeeper_D" +
    "ata\\Managed";
            this.openDLL.FileOk += new System.ComponentModel.CancelEventHandler(this.openDLL_FileOk);
            // 
            // loadDLLBtn
            // 
            this.loadDLLBtn.Highlight = true;
            this.loadDLLBtn.Location = new System.Drawing.Point(308, 130);
            this.loadDLLBtn.Name = "loadDLLBtn";
            this.loadDLLBtn.Size = new System.Drawing.Size(105, 35);
            this.loadDLLBtn.Style = MetroFramework.MetroColorStyle.Yellow;
            this.loadDLLBtn.TabIndex = 3;
            this.loadDLLBtn.Text = "Load DLL";
            this.loadDLLBtn.Click += new System.EventHandler(this.loadDLLBtn_Click);
            // 
            // objectView
            // 
            this.objectView.AutoScroll = true;
            this.objectView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.objectView.Location = new System.Drawing.Point(23, 79);
            this.objectView.Name = "objectView";
            this.objectView.Size = new System.Drawing.Size(245, 332);
            this.objectView.TabIndex = 4;
            // 
            // metroButton1
            // 
            this.metroButton1.Highlight = true;
            this.metroButton1.Location = new System.Drawing.Point(308, 360);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(105, 35);
            this.metroButton1.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroButton1.TabIndex = 5;
            this.metroButton1.Text = "Save";
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // saveDialog
            // 
            this.saveDialog.AddExtension = false;
            this.saveDialog.Title = "Save Modified Data";
            this.saveDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveDialog_FileOk);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 434);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.objectView);
            this.Controls.Add(this.loadDLLBtn);
            this.Controls.Add(this.chooseFileBtn);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "BinarySerializationEditor";
            this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            ((System.ComponentModel.ISupportInitialize)(this.styleManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroStyleManager styleManager;
        private MetroFramework.Controls.MetroButton chooseFileBtn;
        private System.Windows.Forms.OpenFileDialog chooseFile;
        private System.Windows.Forms.OpenFileDialog openDLL;
        private MetroFramework.Controls.MetroButton loadDLLBtn;
        public MetroFramework.Components.MetroToolTip fieldNameTooltip;
        public System.Windows.Forms.Panel objectView;
        private MetroFramework.Controls.MetroButton metroButton1;
        private System.Windows.Forms.SaveFileDialog saveDialog;
    }
}

