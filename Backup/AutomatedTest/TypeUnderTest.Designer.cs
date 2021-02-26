namespace AutomatedTest
{
    partial class TypeUnderTest
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
            this.chckListType = new System.Windows.Forms.CheckedListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblTypeAvailable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chckListType
            // 
            this.chckListType.FormattingEnabled = true;
            this.chckListType.Location = new System.Drawing.Point(12, 44);
            this.chckListType.Name = "chckListType";
            this.chckListType.Size = new System.Drawing.Size(447, 139);
            this.chckListType.TabIndex = 14;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(338, 202);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Thoát";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(239, 202);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "Chấp nhận";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(32, 208);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(81, 17);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "Chọn tất cả";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblTypeAvailable
            // 
            this.lblTypeAvailable.AutoSize = true;
            this.lblTypeAvailable.Location = new System.Drawing.Point(12, 18);
            this.lblTypeAvailable.Name = "lblTypeAvailable";
            this.lblTypeAvailable.Size = new System.Drawing.Size(177, 13);
            this.lblTypeAvailable.TabIndex = 10;
            this.lblTypeAvailable.Text = "Các kiểu trong assembly được chọn";
            this.lblTypeAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TypeUnderTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 242);
            this.Controls.Add(this.chckListType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lblTypeAvailable);
            this.Name = "TypeUnderTest";
            this.Text = "Các kiểu để kiểm thử";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckedListBox chckListType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.Label lblTypeAvailable;
    }
}