using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutomatedTest
{
    public partial class TypeUnderTest : Form
    {
        public TypeUnderTest()
        {
            InitializeComponent();
        }
        private DialogResult m_typeState;
        public DialogResult TypeState
        {
            get
            {
                return m_typeState;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_typeState = DialogResult.OK;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_typeState = DialogResult.Cancel;
            this.Hide(); 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int i = 0; i < chckListType.Items.Count; i++)
                    chckListType.SetItemChecked(i, true);
            }
            else
            {
                for (int i = 0; i < chckListType.Items.Count; i++)
                    chckListType.SetItemChecked(i, false);
            } 
        }
    }
}
