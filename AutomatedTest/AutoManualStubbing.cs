using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace AutomatedTest
{
    public partial class StubForm : Form
    {
        public StubForm()
        {
            InitializeComponent();
        }
        private bool m_TestClassFound;
		public bool TestClassFound
		{

			get 
			{
				Assembly stubAssm = Assembly.LoadFrom(txtDllToStub.Text);
			
				string typeName = "";

				foreach (Type type in stubAssm.GetTypes())
				{
					if (type.ToString().StartsWith("Test") || type.ToString().IndexOf("Test.Test")>-1)
					{
						typeName = type.ToString();
						m_TestClassFound = true;
						break;
					}
				}
				return m_TestClassFound;
			}
			set {m_TestClassFound = value;}
		}

		public string m_RealObject;
		public string m_CreateReference;
		public void MakeNewObjOfStub(string DllToStub, int shtRow, ParameterInfo p)
		{
			string[] interpolNames = DllToStub.Split('\\');
			string assmName = interpolNames[interpolNames.Length-1].Replace(".dll", "");
			Assembly stubAssm = Assembly.LoadFrom(DllToStub);
			
			string typeName = "";

			foreach (Type type in stubAssm.GetTypes())
			{
				if (type.ToString().StartsWith("Test") || type.ToString().IndexOf("Test.Test")>-1)
				{
					typeName = type.ToString();
					m_TestClassFound = true;
					break;
				}
			}

			m_RealObject   = "Assembly appDomain_" + shtRow;
			m_RealObject   += " = Assembly.LoadFrom(@\"" + DllToStub + "\");\n";
	
			m_RealObject   += "\t\tType TestClass_" + shtRow + " = appDomain_";
			m_RealObject   += shtRow + ".GetType(\"" + typeName + "\");\n";

			m_RealObject   += "\t\tobject objInst_" + shtRow;
			m_RealObject   += " = Activator.CreateInstance(TestClass_";
			m_RealObject   += shtRow + ");\n";

			m_RealObject   += "\t\tMethodInfo mi_" + shtRow + " = TestClass_";
			m_RealObject   += shtRow +".GetMethod(\"StartTest\");\n";

			m_RealObject   += "\t\t" + p.ParameterType.ToString() + " ";
			m_RealObject   += p.Name + "_" + shtRow + " =(";
			m_RealObject   += p.ParameterType.ToString() +") mi_" + shtRow;
			m_RealObject   += ".Invoke(objInst_" + shtRow + ", null);\n";


			m_CreateReference = "<Reference\n";
			m_CreateReference += "Name = \"" + assmName + "\"\n";
			m_CreateReference += "AssemblyName = \"" + assmName + "\"\n";
			m_CreateReference += "HintPath = \"" + DllToStub + "\"\n";
			m_CreateReference += "/>\n";
			m_CreateReference += GetDependencies(stubAssm);

		}

		private string GetDependencies(Assembly assmName)
		{
			string refStr="";
			foreach (AssemblyName aRef in assmName.GetReferencedAssemblies())
			{
				if (aRef.FullName.IndexOf("mscorlib") < 0) 
				{
					refStr = refStr + "<Reference\n";
					refStr = refStr + "Name = \"" + aRef.Name + "\"\n";
					refStr = refStr + "AssemblyName = \"" + aRef.Name + "\"\n";
					refStr = refStr + "HintPath = \"" + aRef.CodeBase + "\"\n" + "/>\n";
				}
			}
			return refStr;
		}

        private void btnAddConstructor_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtConstructor.Text != "")
                {
                    string reasignCstor = lstConstructors.SelectedItem.ToString();
                    reasignCstor = reasignCstor.Substring(reasignCstor.IndexOf(" "));
                    txtConstructor.Text += "\n\n" + reasignCstor;
                }
                else
                    txtConstructor.Text = lstConstructors.SelectedItem.ToString();

                lstConstructors.Items.RemoveAt(lstConstructors.SelectedIndex);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\nSelecte a constructor to add.", "Error Reminder");
            }
        }

        private void btnRemoveConstructor_Click(object sender, EventArgs e)
        {
            try
            {
                lstConstructors.Items.Add(txtConstructor.SelectedText.Replace("\n", ""));
                txtConstructor.Text = txtConstructor.Text.Replace(txtConstructor.SelectedText, "").Trim();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\nSelete a constructor to Remove.", "Error Reminder");
            }
        }

        private void btnAddMethod_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMethod.Text != "")
                    txtMethod.Text += "\n\n" + lstMethods.SelectedItem.ToString();
                else
                    txtMethod.Text = lstMethods.SelectedItem.ToString();
                lstMethods.Items.RemoveAt(lstMethods.SelectedIndex);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\nSelecte a method to add.", "Error Reminder");
            }
        }

        private void btnRemovemethod_Click(object sender, EventArgs e)
        {
            try
            {
                lstMethods.Items.Add(txtMethod.SelectedText.Replace("\n", ""));
                txtMethod.Text = txtMethod.Text.Replace(txtMethod.SelectedText, "").Trim();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\nSelete a method to Remove.", "Error Reminder");
            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select a test script dll";
            openFileDialog1.Filter = "Script Dlls (*.dll)|*.dll|All Files(*.*)|*.*";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDllToStub.Text = openFileDialog1.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
	}
    }
