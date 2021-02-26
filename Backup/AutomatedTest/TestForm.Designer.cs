namespace AutomatedTest
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveDataStore = new System.Windows.Forms.Button();
            this.btnAddDataStore = new System.Windows.Forms.Button();
            this.txtDataStore = new System.Windows.Forms.RichTextBox();
            this.txtDotNETLocation = new System.Windows.Forms.TextBox();
            this.txtCurrDir = new System.Windows.Forms.TextBox();
            this.txtTargetProj = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chckManualStub = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCreateScript = new System.Windows.Forms.Button();
            this.chckXMLDataDoc = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveDataStore);
            this.panel1.Controls.Add(this.btnAddDataStore);
            this.panel1.Controls.Add(this.txtDataStore);
            this.panel1.Location = new System.Drawing.Point(14, 204);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(491, 135);
            this.panel1.TabIndex = 24;
            // 
            // btnSaveDataStore
            // 
            this.btnSaveDataStore.Location = new System.Drawing.Point(19, 74);
            this.btnSaveDataStore.Name = "btnSaveDataStore";
            this.btnSaveDataStore.Size = new System.Drawing.Size(108, 35);
            this.btnSaveDataStore.TabIndex = 2;
            this.btnSaveDataStore.Text = "Ghi dữ liệu";
            this.btnSaveDataStore.UseVisualStyleBackColor = true;
            this.btnSaveDataStore.Click += new System.EventHandler(this.btnSaveDataStore_Click);
            // 
            // btnAddDataStore
            // 
            this.btnAddDataStore.Location = new System.Drawing.Point(19, 26);
            this.btnAddDataStore.Name = "btnAddDataStore";
            this.btnAddDataStore.Size = new System.Drawing.Size(108, 33);
            this.btnAddDataStore.TabIndex = 1;
            this.btnAddDataStore.Text = "Thêm dữ liệu";
            this.btnAddDataStore.UseVisualStyleBackColor = true;
            this.btnAddDataStore.Click += new System.EventHandler(this.btnAddDataStore_Click);
            // 
            // txtDataStore
            // 
            this.txtDataStore.Location = new System.Drawing.Point(143, 14);
            this.txtDataStore.Name = "txtDataStore";
            this.txtDataStore.Size = new System.Drawing.Size(328, 107);
            this.txtDataStore.TabIndex = 0;
            this.txtDataStore.Text = "";
            // 
            // txtDotNETLocation
            // 
            this.txtDotNETLocation.Location = new System.Drawing.Point(157, 86);
            this.txtDotNETLocation.Name = "txtDotNETLocation";
            this.txtDotNETLocation.Size = new System.Drawing.Size(348, 20);
            this.txtDotNETLocation.TabIndex = 23;
            // 
            // txtCurrDir
            // 
            this.txtCurrDir.Location = new System.Drawing.Point(157, 48);
            this.txtCurrDir.Name = "txtCurrDir";
            this.txtCurrDir.Size = new System.Drawing.Size(348, 20);
            this.txtCurrDir.TabIndex = 22;
            this.txtCurrDir.Text = "C:\\Temp";
            // 
            // txtTargetProj
            // 
            this.txtTargetProj.Location = new System.Drawing.Point(157, 12);
            this.txtTargetProj.Name = "txtTargetProj";
            this.txtTargetProj.Size = new System.Drawing.Size(348, 20);
            this.txtTargetProj.TabIndex = 21;
            this.txtTargetProj.Text = "C:\\Temp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Vị trí .NET IDE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Thư mục chứa kết quả\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Thư mục chứa dự án";
            // 
            // chckManualStub
            // 
            this.chckManualStub.AutoSize = true;
            this.chckManualStub.Location = new System.Drawing.Point(286, 181);
            this.chckManualStub.Name = "chckManualStub";
            this.chckManualStub.Size = new System.Drawing.Size(118, 17);
            this.chckManualStub.TabIndex = 17;
            this.chckManualStub.Text = "Thực hiện bằng tay";
            this.chckManualStub.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(342, 125);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 32);
            this.btnExit.TabIndex = 16;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click_1);
            // 
            // btnCreateScript
            // 
            this.btnCreateScript.Location = new System.Drawing.Point(203, 125);
            this.btnCreateScript.Name = "btnCreateScript";
            this.btnCreateScript.Size = new System.Drawing.Size(86, 32);
            this.btnCreateScript.TabIndex = 15;
            this.btnCreateScript.Text = "Tạo kịch bản";
            this.btnCreateScript.UseVisualStyleBackColor = true;
            this.btnCreateScript.Click += new System.EventHandler(this.btnCreateScript_Click);
            // 
            // chckXMLDataDoc
            // 
            this.chckXMLDataDoc.AutoSize = true;
            this.chckXMLDataDoc.Location = new System.Drawing.Point(122, 181);
            this.chckXMLDataDoc.Name = "chckXMLDataDoc";
            this.chckXMLDataDoc.Size = new System.Drawing.Size(125, 17);
            this.chckXMLDataDoc.TabIndex = 14;
            this.chckXMLDataDoc.Text = "Sử dụng dữ liệu XML";
            this.chckXMLDataDoc.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(92, 125);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 32);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "Kiểm thử";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 351);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtDotNETLocation);
            this.Controls.Add(this.txtCurrDir);
            this.Controls.Add(this.txtTargetProj);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chckManualStub);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCreateScript);
            this.Controls.Add(this.chckXMLDataDoc);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestForm";
            this.Text = "AutomatedSoftwareTest";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSaveDataStore;
        private System.Windows.Forms.Button btnAddDataStore;
        private System.Windows.Forms.RichTextBox txtDataStore;
        private System.Windows.Forms.TextBox txtDotNETLocation;
        private System.Windows.Forms.TextBox txtCurrDir;
        private System.Windows.Forms.TextBox txtTargetProj;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chckManualStub;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCreateScript;
        private System.Windows.Forms.CheckBox chckXMLDataDoc;
        private System.Windows.Forms.Button btnStart;
    }
}

