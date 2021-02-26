namespace AutomatedTest
{
    partial class StubForm
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
            this.txtDllToStub = new System.Windows.Forms.TextBox();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnRemovemethod = new System.Windows.Forms.Button();
            this.btnAddMethod = new System.Windows.Forms.Button();
            this.btnRemoveConstructor = new System.Windows.Forms.Button();
            this.btnAddConstructor = new System.Windows.Forms.Button();
            this.txtMethod = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.txtConstructor = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstMethods = new System.Windows.Forms.ListBox();
            this.lstConstructors = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtDllToStub
            // 
            this.txtDllToStub.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDllToStub.Location = new System.Drawing.Point(15, 322);
            this.txtDllToStub.Name = "txtDllToStub";
            this.txtDllToStub.Size = new System.Drawing.Size(389, 29);
            this.txtDllToStub.TabIndex = 44;
            this.txtDllToStub.Text = "\"\"";
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(423, 319);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(75, 37);
            this.btnBrowser.TabIndex = 43;
            this.btnBrowser.Text = "Browser";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(606, 314);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 37);
            this.btnOK.TabIndex = 42;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnRemovemethod
            // 
            this.btnRemovemethod.Location = new System.Drawing.Point(329, 271);
            this.btnRemovemethod.Name = "btnRemovemethod";
            this.btnRemovemethod.Size = new System.Drawing.Size(75, 37);
            this.btnRemovemethod.TabIndex = 41;
            this.btnRemovemethod.Text = "<";
            this.btnRemovemethod.UseVisualStyleBackColor = true;
            this.btnRemovemethod.Click += new System.EventHandler(this.btnRemovemethod_Click);
            // 
            // btnAddMethod
            // 
            this.btnAddMethod.Location = new System.Drawing.Point(329, 209);
            this.btnAddMethod.Name = "btnAddMethod";
            this.btnAddMethod.Size = new System.Drawing.Size(75, 37);
            this.btnAddMethod.TabIndex = 40;
            this.btnAddMethod.Text = ">";
            this.btnAddMethod.UseVisualStyleBackColor = true;
            this.btnAddMethod.Click += new System.EventHandler(this.btnAddMethod_Click);
            // 
            // btnRemoveConstructor
            // 
            this.btnRemoveConstructor.Location = new System.Drawing.Point(329, 107);
            this.btnRemoveConstructor.Name = "btnRemoveConstructor";
            this.btnRemoveConstructor.Size = new System.Drawing.Size(75, 37);
            this.btnRemoveConstructor.TabIndex = 39;
            this.btnRemoveConstructor.Text = "<";
            this.btnRemoveConstructor.UseVisualStyleBackColor = true;
            this.btnRemoveConstructor.Click += new System.EventHandler(this.btnRemoveConstructor_Click);
            // 
            // btnAddConstructor
            // 
            this.btnAddConstructor.Location = new System.Drawing.Point(329, 45);
            this.btnAddConstructor.Name = "btnAddConstructor";
            this.btnAddConstructor.Size = new System.Drawing.Size(75, 37);
            this.btnAddConstructor.TabIndex = 38;
            this.btnAddConstructor.Text = ">";
            this.btnAddConstructor.UseVisualStyleBackColor = true;
            this.btnAddConstructor.Click += new System.EventHandler(this.btnAddConstructor_Click);
            // 
            // txtMethod
            // 
            this.txtMethod.Location = new System.Drawing.Point(410, 200);
            this.txtMethod.Name = "txtMethod";
            this.txtMethod.Size = new System.Drawing.Size(293, 108);
            this.txtMethod.TabIndex = 37;
            this.txtMethod.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(489, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Testing Methods\r\n";
            // 
            // txtConstructor
            // 
            this.txtConstructor.Location = new System.Drawing.Point(410, 36);
            this.txtConstructor.Name = "txtConstructor";
            this.txtConstructor.Size = new System.Drawing.Size(293, 108);
            this.txtConstructor.TabIndex = 35;
            this.txtConstructor.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 34;
            this.label3.Text = "Available Methods";
            // 
            // lstMethods
            // 
            this.lstMethods.FormattingEnabled = true;
            this.lstMethods.Location = new System.Drawing.Point(15, 200);
            this.lstMethods.Name = "lstMethods";
            this.lstMethods.Size = new System.Drawing.Size(297, 108);
            this.lstMethods.TabIndex = 33;
            // 
            // lstConstructors
            // 
            this.lstConstructors.FormattingEnabled = true;
            this.lstConstructors.Location = new System.Drawing.Point(15, 36);
            this.lstConstructors.Name = "lstConstructors";
            this.lstConstructors.Size = new System.Drawing.Size(297, 108);
            this.lstConstructors.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(493, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Testing Constructor(s):\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Available Constructor(s):\r\n";
            // 
            // StubForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 368);
            this.Controls.Add(this.txtDllToStub);
            this.Controls.Add(this.btnBrowser);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRemovemethod);
            this.Controls.Add(this.btnAddMethod);
            this.Controls.Add(this.btnRemoveConstructor);
            this.Controls.Add(this.btnAddConstructor);
            this.Controls.Add(this.txtMethod);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtConstructor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstMethods);
            this.Controls.Add(this.lstConstructors);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "StubForm";
            this.Text = "AutoManualStubbing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtDllToStub;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnRemovemethod;
        private System.Windows.Forms.Button btnAddMethod;
        private System.Windows.Forms.Button btnRemoveConstructor;
        private System.Windows.Forms.Button btnAddConstructor;
        public System.Windows.Forms.RichTextBox txtMethod;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.RichTextBox txtConstructor;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ListBox lstMethods;
        public System.Windows.Forms.ListBox lstConstructors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}