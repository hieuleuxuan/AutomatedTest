using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace AutomatedTest
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }
        private string AssemblyNameToTest;
        private void GetAssemblyName()
        {
            openFileDialog1.Title = "DLL under test";
            openFileDialog1.Filter = "DLL files (*.dll)|*.dll|Executable files (*.exe)|*.exe|All files (*.*)|*.*";


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AssemblyNameToTest = openFileDialog1.FileName;
            }
            else
            {
                AssemblyNameToTest = "";
            }
        }
        private void GetTypesOfAssemblyUnderTest()
        {
            if (AssemblyNameToTest.Length <= 0)
            {
                return;
            }
            TypeUnderTest typeDUT = new TypeUnderTest();
            try
            {
                Assembly asm = Assembly.LoadFrom(AssemblyNameToTest);
                Type[] tys = asm.GetTypes();
                foreach (Type ty in tys)
                {
                    typeDUT.chckListType.Items.Add(ty.Name);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            typeDUT.lblTypeAvailable.Text = "Thư mục chứa các lớp để kiểm thử:\n" + AssemblyNameToTest;
            typeDUT.ShowDialog();

            PassSelectedTypesUT(typeDUT);
        }

        private string m_typesDUT;
        private void PassSelectedTypesUT(TypeUnderTest typeDUT)
        {
            if (typeDUT.TypeState == DialogResult.OK)
            {
                m_typesDUT = "";
                for (int i = 0; i < typeDUT.chckListType.Items.Count; i++)
                {
                    if (typeDUT.chckListType.GetItemChecked(i))
                        m_typesDUT = m_typesDUT + typeDUT.chckListType.GetItemText(typeDUT.chckListType.Items[i]) + " ";
                }
            }
            else
            {
                m_typesDUT = "";
            }
        }

        private Assembly DUTAsm;
        private string xlsDataStoreFilename = @"C:/temp/SoftTestDataStore.xls"; //textbox2.text
        private int Tongsodong;
        private void Khoitaophuongthuckiemthu()
        {
            
            int i = 2;
            if (m_typesDUT.Length > 1)
            {
                //mở sheet trong Excel
                TaoSheet();

                //load file dll
                try
                {
                    DUTAsm = Assembly.LoadFrom(AssemblyNameToTest);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

                Type[] types = null;
                types = DUTAsm.GetTypes();

                foreach (Type t in types)
                {
                    if (m_typesDUT.IndexOf(t.Name) > -1)
                    {
                        ConstructorInfo[] cis = t.GetConstructors();
                        foreach (ConstructorInfo ci in cis)
                        {
                            LayCacCell(xSheet, i, 1, t.Name, null);
                            LayCacCell(xSheet, i, 2, t.Name, null);
                            ParameterInfo[] ps = ci.GetParameters();
                            foreach (ParameterInfo p in ps)
                            {
                                LayCacCell(xSheet, i, p.Position + 3, p.Name, p);
                            }
                            i++;
                        }

                        MethodInfo[] ms = t.GetMethods();

                        foreach (MethodInfo m in ms)
                        {
                            LayCacCell(xSheet, i, 1, t.Name, null);
                            LayCacCell(xSheet, i, 2, m.Name, null);
                            ParameterInfo[] ps = m.GetParameters();
                            foreach (ParameterInfo p in ps)
                            {
                                LayCacCell(xSheet, i, p.Position + 3, p.Name, p);
                            }
                            if (m.ReturnType.ToString() == "System.Void")
                            {
                            }
                            else
                            {
                                ThemGiaTriMongDoi(xSheet, i, m);
                            }
                            i++;
                        }
                    }
                }

                try
                {
                    xBook.SaveAs(xlsDataStoreFilename, -4143, "", "", false, false, 0, "", 0, "", "", "");
                }
                catch (Exception err) { MessageBox.Show(err.Message); }
            }
            Tongsodong = i;
        }

        Excel.Application xApp;
        Excel.Workbook xBook;
        Excel.Worksheet xSheet;
        private void TaoSheet()
        {
            xApp = new Excel.Application();
            xBook = xApp.Workbooks.Add(1);
            xSheet = (Excel.Worksheet)xBook.ActiveSheet;
            xSheet.Cells.set_Item(1, 1, "TÊN LỚP");
            xSheet.Cells.set_Item(1, 2, "TÊN PHƯƠNG THỨC");
            xSheet.Cells.set_Item(1, 3, "CÁC THAM SỐ ĐẦU VÀO");
            Excel.Range range;
            range = xSheet.get_Range("A1", "Z1");
            range.Interior.ColorIndex = 8;
            range.Columns.AutoFit();
            range.Font.Bold = true;
            xApp.Visible = true;
        }

        private void ThemGiaTriMongDoi(Excel.Worksheet xs, int shtRow, MethodInfo mi)
        {
            Excel.Range range = null;
            int parCount = 0;
            try
            {
                foreach (ParameterInfo pi in mi.GetParameters())
                {
                    parCount++;
                }
                string ColChar = TestUtility.ConvCHAR(parCount + 3);

                range = xs.get_Range(ColChar + shtRow, ColChar + shtRow);
                range.AddComment("giá trị mong đợi " + mi.ReturnType.ToString());
                range.Interior.ColorIndex = 43;
                range.Font.ColorIndex = 3;
                range.Font.Bold = true;
            }
            catch { }
        }

        private void LayCacCell(Excel.Worksheet xs, int shtRow, int shtCol, string setText, ParameterInfo p)
        {
            string ColChar = TestUtility.ConvCHAR(shtCol);
            Excel.Range range = null;
            //Giá trị mong đợi trả giá trị trả về của tham số đưa vào
            range = xs.get_Range(ColChar + shtRow, ColChar + shtRow);

            if (null != p)
            {
                range.Value2 = TestUtility.SysToCSPro(p.ParameterType.ToString()) + " " + setText;
            }
            else
                range.Value2 = setText;

            if (p != null)
            {
                DuaVaoThamSoTuDong(p, range);
            }

            try
            {
                if (p == null)
                    range.Font.ColorIndex = 3;
                else if (p != null)
                    range.AddComment(TestUtility.SysToCSPro(p.ParameterType.ToString()) + " " + setText);
                if (p != null)
                    if (p.ParameterType.ToString().IndexOf("&") > 0)
                    {
                        if (p.IsOut)
                            range.Interior.ColorIndex = 8; //set color to a out parameter cell
                        else
                            range.Interior.ColorIndex = 6; //set color to a ref parameter cell
                    }
            }
            catch { }
            
            string formulaStr = null;
            string[] tempShort = null;
            bool defaultEnumIsSet = false;

            if (p != null)
            {
                if (p.ParameterType.IsEnum)
                {
                    FieldInfo[] enumMembers = p.ParameterType.GetFields();
                    foreach (FieldInfo fs in enumMembers)
                    {
                        tempShort = fs.ToString().Trim().Split(' ');
                        if (tempShort[tempShort.Length - 1] != "value__")
                        {
                            formulaStr = formulaStr + tempShort[tempShort.Length - 1] + ",";
                            if (!defaultEnumIsSet)
                            {
                                defaultEnumIsSet = true;
                                range.Value2 = tempShort[tempShort.Length - 1];
                            }
                        }
                    }
                    defaultEnumIsSet = false;
                    formulaStr = formulaStr + "xxxx";
                    formulaStr = formulaStr.Replace(" ", ".");
                    formulaStr = formulaStr.Replace(",xxxx", "");

                    try
                    {
                        range.Validation.Delete();
                        range.Validation.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop, Excel.XlFormatConditionOperator.xlBetween, formulaStr, "");
                    }
                    catch { }
                }
            }
        }

        private void DuaVaoThamSoTuDong(ParameterInfo p, Excel.Range range)
        {
            Random thamso = new Random(); 

            if (p.ParameterType.ToString().IndexOf("Boolean") > 0)
                range.Value2 = "true";
            if (p.ParameterType.ToString().IndexOf("Boolean") > 0 && p.ParameterType.ToString().IndexOf("&") > 0)
                range.Value2 = "false";

            if (CacKieuSo(TestUtility.SysToCSPro(p.ParameterType.ToString())))
            {
                if (p.ParameterType.ToString().IndexOf("Double") > 0 || p.ParameterType.ToString().IndexOf("Decimal") > 0)
                    range.Value2 = (double)thamso.Next() / 1000000;
                else
                    range.Value2 = (int)thamso.Next() / 1000000;
            }
        }

        private bool CacKieuSo(string typeStr)
        {
            if (typeStr.StartsWith("int") ||
                typeStr.StartsWith("double") ||
                typeStr.StartsWith("long") ||
                typeStr.StartsWith("short") ||
                typeStr.StartsWith("uint") ||
                typeStr.StartsWith("float") ||
                typeStr.StartsWith("decimal") ||
                typeStr.StartsWith("ulong") ||
                typeStr.StartsWith("ushort"))
                return true;

            return false;
        }

        //Start coding XML documentation
        private void ThaoTacDulieuXML()
        {
            XmlDocument xmldoc = MoFileXML();

            //add code to read XML data into Excel data store

            for (int RowC = 2; RowC < Tongsodong; RowC++)
            {
                //Get the Column with data;
                int ColC = 0;
                Excel.Range colRange = xSheet.get_Range(TestUtility.ConvCHAR(1) + RowC, TestUtility.ConvCHAR(1) + RowC);
                while (colRange.Value2 != null || colRange.Comment != null)
                {
                    ColC++;
                    colRange = xSheet.get_Range(TestUtility.ConvCHAR(ColC) + RowC, TestUtility.ConvCHAR(ColC) + RowC);
                }
                if (ColC > 3)
                {
                    //parameter starts from column 3
                    for (int ColCou = 3; ColCou < ColC; ColCou++)
                    {
                        DocDuLieuXMLTrongNguon(xmldoc, RowC, ColCou);
                    }
                }
            }

            try
            {
                xBook.Save();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private XmlDocument MoFileXML()
        {
            string[] TenDuongDanThuMuc = AssemblyNameToTest.Split('\\');
            string ThuMucGocxml = AssemblyNameToTest.Replace(TenDuongDanThuMuc[TenDuongDanThuMuc.Length - 1], "");
            DirectoryInfo ThuMucxml = null;
            try
            {
                ThuMucxml = new DirectoryInfo(ThuMucGocxml);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n Nguyên nhân có thể: Tập tin XML không phải trong cùng thư mục với việc kiểm thử, hoặc thuộc tính không được thiết lập để xây dựng một đầu ra XML được đặt tên", "Không thể tìm thấy tập tin XML");
                return null;
            }
            FileInfo[] TatCaFile = ThuMucxml.GetFiles();
            FileInfo xmlFile = null;
            foreach (FileInfo xmlF in TatCaFile)
            {
                if (xmlF.Extension == ".xml")
                {
                    xmlFile = xmlF;
                    break;
                }
            }

            if (xmlFile != null)
            {
                XmlReader Docfilexml = new XmlTextReader(File.OpenRead(xmlFile.FullName));
                XmlDocument Tailieuxml = new XmlDocument();
                Tailieuxml.Load(Docfilexml);
                Docfilexml.Close();

                return Tailieuxml;
            }

            return null;
        }

        private void DocDuLieuXMLTrongNguon(XmlDocument xmlDoc, int RowC, int ColC)
        {
            Excel.Range xmlRange = xSheet.get_Range(TestUtility.ConvCHAR(ColC) + RowC, TestUtility.ConvCHAR(ColC) + RowC);//LocateRangeForXML(RowC, ColC);

            XmlNodeList memNames = xmlDoc.GetElementsByTagName("member");

            for (int memCount = 0; memCount < memNames.Count; memCount++)
            {
                XmlAttributeCollection memAtt = memNames[memCount].Attributes;

                string classMem = (string)xSheet.get_Range(TestUtility.ConvCHAR(1) + RowC, TestUtility.ConvCHAR(1) + RowC).Value2;//LocateRangeForXML(RowC, 1).Value2;
                string methodMem = (string)xSheet.get_Range(TestUtility.ConvCHAR(2) + RowC, TestUtility.ConvCHAR(2) + RowC).Value2;//LocateRangeForXML(RowC, 2).Value2;

                if (memAtt[0].Value.IndexOf(classMem + "." + methodMem) > 0 || memAtt[0].Value.IndexOf(classMem + ".#ctor(") > 0)//("LowLevelObj.SimpleMath") > 0)
                {
                    XmlNodeList paramNodes = memNames[memCount].ChildNodes;

                    for (int paramCount = 0; paramCount < paramNodes.Count; paramCount++)
                    {
                        if (paramNodes[paramCount].Name == "param")// || paramNodes[2].Name == "returns")
                        {
                            XmlAttributeCollection paramAtt = paramNodes[paramCount].Attributes;

                            string[] commentStr = null;
                            try
                            {
                                commentStr = xmlRange.Comment.Shape.AlternativeText.Split(' ');
                            }
                            catch { }
                            if (commentStr != null)
                            {
                                if (paramAtt[0].Value == commentStr[commentStr.Length - 1])// "x")
                                {

                                    DuaDuLieuXMLLenExcel(paramNodes[paramCount].InnerXml, xmlRange);
                                    break;
                                }
                            }
                        }

                        if (paramNodes[paramCount].Name == "returns")
                        {
                            if (xmlRange.Comment.Shape.AlternativeText.IndexOf("Expect to return a") >= 0)
                            {

                                DuaDuLieuXMLLenExcel(paramNodes[paramCount].InnerXml, xmlRange);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void DuaDuLieuXMLLenExcel(string TuVanBanXML, Excel.Range PhamViXML)
        {
            string DenCell = TuVanBanXML;//[paramCount].InnerXml;
            int DuLieuDauTien = DenCell.IndexOf("e.g.,");
            //int dataLen = GotoCell.Length - startData;
            try
            {
                DenCell = DenCell.Substring(DuLieuDauTien);
            }
            catch { }
            DenCell = DenCell.Replace("e.g.,", "").Trim();
            PhamViXML.Value2 = DenCell;
        }

        //Start coding chapter 7
        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
        private void DongExcelSheet()
        {
            xBook.Close(false, Missing.Value, false);
            xApp.Quit();
            xSheet = null;
            xBook = null;
            xApp = null;
        }

        Excel.Range range = null;
        Type typ = null;
        Type[] types = null;
        CodeMemberMethod cm = null;
        CodeDomProvider codeProvider = null;
        ICodeGenerator cg = null;
        string nameSpace = null;
        CodeCompileUnit TestUnit;
        CodeNamespace cnamespace;
        string clsName;
        CodeTypeDeclaration co;
        CodePropertyReferenceExpression pState = null;
        CodeExpression[] pCode = null;
        CodeFieldReferenceExpression cLeft = null;
        CodeFieldReferenceExpression cRight = null;

        private void TaoMaKiemThu(Assembly DUT, TextWriter t)
        {
            BatDauCodeDom(DUT);
            ThemNamespaces(DUT);
            PhuongThucLopDauTien();
            TaoMaSheet(cm);
            ThemMaExcelDauTien(cm);
            MaCuaCacPhuongThuc(cm);

            //Thông tin bắt đầu từ dòng 2 của Excel sheet
            int i = 2;

            range = xSheet.get_Range(TestUtility.ConvCHAR(1) + i, TestUtility.ConvCHAR(1) + i);
            string previousType = range.Value2.ToString();
            KhoiTaoCacKieuKiemThu(i, ref previousType);

            int totalRows = DemCacDongTrongSheet(xSheet);

            MethodInfo mi = null;
            ConstructorInfo ci = null; //khởi tạo
            Type[] AmbiguousOverLoad = null;
            string wholeCellText = null;
            string[] typeText = null;
            string strPar = null;
            string[] arrayPar = null;
            Excel.Range rng;
            Excel.Range rngCstr;

            for (i = 2; i < totalRows; i++)//while (range.Value2.ToString() != "")
            {
                int j = 3;

                CacKieuMoi(ref i, ref j, ref previousType);

                rng = xSheet.get_Range(TestUtility.ConvCHAR(j) + i, TestUtility.ConvCHAR(j) + i);
                rngCstr = xSheet.get_Range(TestUtility.ConvCHAR(1) + i, TestUtility.ConvCHAR(1) + i);

                if (range.Value2.ToString() == rngCstr.Value2.ToString())//kiểm tra xem nó được khởi tạo
                {
                    ThemKhoiMaKiemThu(ref i, ref j, ref rng, ref wholeCellText, ref typeText, ref strPar, ref arrayPar, ref AmbiguousOverLoad, ref ci);
                }
                else
                {
                    ThemMaPhuongThucKiemThu(ref i, ref j, ref mi, ref rng, ref wholeCellText, ref typeText, ref strPar, ref arrayPar, ref AmbiguousOverLoad);

                    ParameterInfo[] ps = mi.GetParameters();
                    string parStr = "";

                    ThuThapCacThamSochoKiemThu(ref i, ref j, ref rng, ref ps, ref parStr);

                    pState = new CodePropertyReferenceExpression(null, "obj" + typ.Name);
                    pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, parStr) };
                    CodeStatement[] trySt = null;
                    CodeExpression[] passPar = new CodeExpression[] { new CodeFieldReferenceExpression(null, "xResult, shtRow, mName") };

                    AddInvokeTestMethodCallCode(ref i, ref parStr, ref mi, ref trySt, ref passPar);

                    //add log result code
                    int colPosition = 0;
                    LogParmResultAndReturnValue(ref i, colPosition, ps, ref mi);
                }
                range = xSheet.get_Range(TestUtility.ConvCHAR(1) + i, TestUtility.ConvCHAR(1) + i);
            }
            AddOtherCodesPartII(cm, clsName);

            if (objReturnStub == null)//Thêm cho việc kiểm thử COM
                objReturnStub = new CodeSnippetExpression("null"); //thêm cho việc kiểm thử COM
            cm.Statements.Add(new CodeMethodReturnStatement(objReturnStub));
            objReturnStub = null; 
            AddUpClassesMethods();
            cg.GenerateCodeFromNamespace(cnamespace, t, null);
            DocFileCSPROJ(clsName, DUTAsm);
        }
        private void BatDauCodeDom(Assembly DUT)
        {
            TestUnit = new CodeCompileUnit();

            types = DUT.GetTypes();
            codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
            foreach (Type ty in types)
            {
                nameSpace = ty.Namespace + "Test";
            }
            cnamespace = new CodeNamespace(nameSpace);

            clsName = "Test" + DUTAsm.FullName.Substring(0, DUTAsm.FullName.IndexOf(", "));

            cg = codeProvider.CreateGenerator();
        }

        private void ThemNamespaces(Assembly DUT)
        {
            TestUnit.Namespaces.Add(cnamespace);
            cnamespace.Imports.Add(new CodeNamespaceImport("System"));
            cnamespace.Imports.Add(new CodeNamespaceImport("System.IO"));
            cnamespace.Imports.Add(new CodeNamespaceImport("Excel=Microsoft.Office.Interop.Excel"));
            cnamespace.Imports.Add(new CodeNamespaceImport("System.Reflection"));
            AssemblyName[] asms = null;
            asms = DUT.GetReferencedAssemblies();
            foreach (AssemblyName asm in asms)
            {
                if (asm.Name.ToString() != "mscorlib")
                {
                    if (asm.Name.IndexOf(".") > 0 && asm.Name != "")
                        cnamespace.Imports.Add(new CodeNamespaceImport(asm.Name));
                }
            }
        }

        private void PhuongThucLopDauTien()
        {
            //add class name
            co = new CodeTypeDeclaration(clsName);
            cnamespace.Types.Add(co);
            co.Attributes = (System.CodeDom.MemberAttributes)TypeAttributes.Public;

            //add main mehtod
            cm = new CodeMemberMethod();
            cm.Name = "StartTest";//"Main";
            cm.ReturnType = new CodeTypeReference(typeof(object));//added for integration
            cm.Attributes = MemberAttributes.Public | MemberAttributes.Final;
        }
        private void TaoMaSheet(CodeMemberMethod cm)
        {
            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Application), "xApp = new Excel.Application()"));
            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Workbook), "xBook = xApp.Workbooks.Open(fileName, 0, false, 1, \"\", \"\", true, 1, 0, true, 1, 0, 0, 0, 0)"));
            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Worksheet), "xSheet = (Excel.Worksheet)xBook.ActiveSheet"));
            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Workbook), "xBook2 = xApp.Workbooks.Add(1)"));
            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Worksheet), "xResult = (Excel.Worksheet)xBook2.ActiveSheet"));
        }

        private void ThemMaExcelDauTien(CodeMemberMethod cm)
        {
            CodePropertyReferenceExpression pState = null;
            CodeExpression[] pCode = null;

            pState =
                new CodePropertyReferenceExpression(null, "xResult");
            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "1, 1, \"PHƯƠNG THỨC KIỂM THỬ\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(pState, "Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "1, 2, \"KẾT QUẢ\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(pState, "Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "1, 3, \"NGUYÊN NHÂN LỖI\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(pState, "Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "1, 4, \"KẾT QUẢ TRẢ VỀ\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(pState, "Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "1, 5, \"KẾT QUẢ MONG ĐỢI\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(pState, "Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "1, 6, \"CÁC THAM SỐ VÀ GIÁ TRỊ\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(pState, "Cells.set_Item", pCode));
        }
        private void MaCuaCacPhuongThuc(CodeMemberMethod cm)
        {
            CodePropertyReferenceExpression pState = null;
            CodeExpression[] pCode = null;
            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "xApp.Visible") };
            cLeft = new CodeFieldReferenceExpression(null, "xApp.Visible");
            cRight = new CodeFieldReferenceExpression(null, " true");//sysbooltrue.ToString().ToLower());  
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Range), "range"));
            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Range), "rangeCurr"));
            cLeft = new CodeFieldReferenceExpression(null, "range");
            cRight = new CodeFieldReferenceExpression(null, "xResult.get_Range(\"A1\", \"H1\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            cLeft = new CodeFieldReferenceExpression(null, "range.Interior.ColorIndex");
            cRight = new CodeFieldReferenceExpression(null, "8");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            cLeft = new CodeFieldReferenceExpression(null, "range.Font.Bold");
            cRight = new CodeFieldReferenceExpression(null, " true");//sysbooltrue.ToString().ToLower());
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));


            pState = new CodePropertyReferenceExpression(null, "range");
            cm.Statements.Add(new CodeMethodInvokeExpression(pState, "Columns.AutoFit"));

            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(int), "shtRow = 0"));
            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(string), "mName = null"));
            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(int), "tempArrayIndex = 0"));
        }

        //initiate an object for the StartTest method to return an object
        //in order to use a bottom up approach for integration testing.
        CodeSnippetExpression objReturnStub;//added for integration
        private void KhoiTaoCacKieuKiemThu(int i, ref string KieuTruoc)
        {
            foreach (Type ty in types)
            {
                if (ty.Name.ToString() == KieuTruoc)
                {
                    typ = ty;
                }
            }
            string nameSPLen = typ.Namespace;
            if (nameSPLen != null || typ.Namespace != null)
                cnamespace.Imports.Add(new CodeNamespaceImport(typ.Namespace));
            if (typ.IsClass)
            {
                cm.Statements.Add(new CodeParameterDeclarationExpression(typ.Name, "obj" + typ.Name + " = null"));//modified for constructor
                objReturnStub = new CodeSnippetExpression("obj" + typ.Name);//added for integration
            }
            else if (typ.IsInterface)
            {
                cm.Statements.Add(new CodeParameterDeclarationExpression(typ.Name, "obj" + typ.Name + " = null"));
                foreach (Type ty in types)
                {
                    if (ty.IsClass)
                    {
                        cm.Statements.Add(new CodeParameterDeclarationExpression(ty.Name, "obj_" + i + ty.Name + " = new " + ty.Name + "()"));
                        cLeft = new CodeFieldReferenceExpression(null, "obj" + typ.Name);
                        cRight = new CodeFieldReferenceExpression(null, "(" + typ.Name + ")obj_" + i + ty.Name);
                        cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
                    }
                }
            }

        }
        private int DemCacDongTrongSheet(Excel.Worksheet xSheet)
        {
            Excel.Range range = null;
            int i = 1;
            range = xSheet.get_Range("A" + i, "A" + i);
            while (range.Value2 != null)
            {
                i++;
                range = xSheet.get_Range("A" + i, "A" + i);
            }
            return i;
        }

        private void CacKieuMoi(ref int i, ref int j, ref string KieuTruoc)
        {
            range = xSheet.get_Range(TestUtility.ConvCHAR(1) + i, TestUtility.ConvCHAR(1) + i);
            string currentType = range.Value2.ToString();
            if (KieuTruoc != currentType)
            {
                foreach (Type ty in types)
                {
                    if (ty.Name.ToString() == currentType)
                    {
                        typ = ty;
                    }
                }
                KieuTruoc = currentType;
                //Sửa đổi để thêm khởi tạo
                cm.Statements.Add(new CodeParameterDeclarationExpression(typ.Name, "obj" + typ.Name + " = null"));
            }
            range = xSheet.get_Range(TestUtility.ConvCHAR(2) + i, TestUtility.ConvCHAR(2) + i);

            cLeft = new CodeFieldReferenceExpression(null, "shtRow");
            cRight = new CodeFieldReferenceExpression(null, "" + i);
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

        }
        private void ThemKhoiMaKiemThu(ref int i, ref int j, ref Excel.Range rng, ref string wholeCellText, ref string[] typeText, ref string strPar, ref string[] arrayPar, ref Type[] AmbiguousOverLoad, ref ConstructorInfo ci)
        {
            cLeft = new CodeFieldReferenceExpression(null, "mName");
            cRight = new CodeFieldReferenceExpression(null, "\"" + typ.Name + "\"");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            strPar = ""; //Start a new variable
            while (rng.Value2 != null)
            {
                wholeCellText = rng.Comment.Text("", 1, 0);
                typeText = wholeCellText.Split(' ');
                strPar = strPar + typeText[0] + ",";
                j++;
                rng = xSheet.get_Range(TestUtility.ConvCHAR(j) + i, TestUtility.ConvCHAR(j) + i);
            }
            if (strPar != null)
            {
                strPar = strPar.Replace("Expect,", "");
                arrayPar = strPar.Split(',');
                AmbiguousOverLoad = new Type[arrayPar.Length - 1];
                for (int parPos = 0; parPos < arrayPar.Length - 1; parPos++)//enumerate parameters
                {
                    TestUtility.ConvertStringToType(arrayPar[parPos], ref AmbiguousOverLoad[parPos]);
                }
                ci = typ.GetConstructor(AmbiguousOverLoad);
            }
            else//if (strPar == "")
                ci = typ.GetConstructor(new Type[0]);

            if (ci != null)
            {
                ParameterInfo[] pars = ci.GetParameters();
                foreach (ParameterInfo p in pars)
                {
                    ThemMaChoCacThamSo(p, cm, i);
                }

                ThemCacKhoiMa(typ, ci, cm, i);
            }
            pState = new CodePropertyReferenceExpression(null, "xResult");
            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow, 4, \"Test Constructor\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(pState, "Cells.set_Item", pCode));

        }
        private void ThemMaChoCacThamSo(ParameterInfo p, CodeMemberMethod cm, int i)
        {
            Excel.Range rng = xSheet.get_Range(TestUtility.ConvCHAR(p.Position + 3) + i, TestUtility.ConvCHAR(p.Position + 3) + i); //added for pass parameter by object					

            CodeFieldReferenceExpression cLeft = new CodeFieldReferenceExpression(null, "range");
            CodeFieldReferenceExpression cRight = new CodeFieldReferenceExpression(null, "xSheet.get_Range(\"" + TestUtility.ConvCHAR(p.Position + 3) + i + "\", \"" + TestUtility.ConvCHAR(p.Position + 3) + i + "\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //int errorCode = int.Parse(range.Value2.ToString());
            cLeft = new CodeFieldReferenceExpression(null, TestUtility.SysToCSPro(p.ParameterType.ToString()) + " " + p.Name + "_" + i);
            //cRight=new CodeFieldReferenceExpression(null,  "new System.Text.StringBuilder()");
            if (p.ParameterType.ToString().StartsWith("System.String") || p.ParameterType.ToString().StartsWith("System.Object"))
            {
                cRight = new CodeFieldReferenceExpression(null, "range.Value2.ToString()");
            }
            else if (p.ParameterType.ToString().IndexOf("[]") > 0)
            {
                //cRight=new CodeFieldReferenceExpression(null, "{" + TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]","")) + ".Parse(range.Value2.ToString())}"); //commented to read array
                if (TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]", "")) == "string")
                {
                    cRight = new CodeFieldReferenceExpression(null, "range.Value2.ToString().Split(',')");
                }
                else
                {
                    cRight = new CodeFieldReferenceExpression(null, " null");
                    //cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

                    //cLeft=new CodeFieldReferenceExpression(null, "tempArray");
                    cRight = new CodeFieldReferenceExpression(null, "new " + TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]", "")) + "[range.Value2.ToString().Split(',').Length]");
                    cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
                    //foreach (string z in x.Split(',')){a[i]=int.Parse(z);i++;};
                    cLeft = new CodeFieldReferenceExpression(null, "foreach (string z in range.Value2.ToString().Split(',')){" + p.Name + "_" + i + "[tempArrayIndex]");
                    cRight = new CodeFieldReferenceExpression(null, TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]", "")) + ".Parse(z); tempArrayIndex++;}");
                    //cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
                }
            }
            else if (p.ParameterType.ToString().IndexOf("Text.StringBuilder") > 0)
            {
                cRight = new CodeFieldReferenceExpression(null, "new System.Text.StringBuilder()");
            }
            else if (rng.Value2.ToString() == "new")
            {
                cRight = new CodeFieldReferenceExpression(null, "new " + p.ParameterType.ToString() + "()");
            }
            else
            {
                cRight = new CodeFieldReferenceExpression(null, TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]", "")) + ".Parse(range.Value2.ToString())");
            }
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            if (p.ParameterType.ToString().IndexOf("[]") > 0)
            {
                cLeft = new CodeFieldReferenceExpression(null, "tempArrayIndex");
                cRight = new CodeFieldReferenceExpression(null, "0");
                cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            }
        }
        private void ThemCacKhoiMa(Type typ, ConstructorInfo ci, CodeMemberMethod cm, int i)
        {
            string pCodeStr = "";

            CodeStatement[] trySt;
            CodeExpression[] passPar = new CodeExpression[] { new CodeFieldReferenceExpression(null, "xResult, shtRow, mName") };

            ParameterInfo[] pis = ci.GetParameters();
            foreach (ParameterInfo pi in pis)
            {
                pCodeStr += pi.Name + "_" + i + ", ";
            }
            if (pCodeStr.IndexOf(",") > 0)
            {
                pCodeStr += "xxxx";
                pCodeStr = pCodeStr.Replace(", xxxx", "");
            }
            CodeExpression[] pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, pCodeStr) };

            trySt = new CodeStatement[] {new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "obj" + typ.Name + " = new " + typ.Name, pCode)),
											new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "TestPass", passPar)),
			};

            //TestFail(xResult, shtRow, mName, err.Message);
            passPar = new CodeExpression[] { new CodeFieldReferenceExpression(null, "xResult, shtRow, mName, err.Message") };
            CodeCatchClause catchCl = new CodeCatchClause("err", new CodeTypeReference(typeof(Exception)), new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "TestFail", passPar)));

            CodeCatchClause[] catchSt = new CodeCatchClause[]{ catchCl
															 };
            cm.Statements.Add(new CodeTryCatchFinallyStatement(trySt, catchSt));
        }

        private void ThemMaPhuongThucKiemThu(ref int i, ref int j, ref MethodInfo mi, ref Excel.Range rng, ref string wholeCellText, ref string[] typeText, ref string strPar, ref string[] arrayPar, ref Type[] AmbiguousOverLoad)
        {
            try
            {
                mi = typ.GetMethod(range.Value2.ToString());
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                strPar = ""; //Start a new variable
                while (rng.Value2 != null)
                {
                    //wholeCellText = rng.Value2.ToString();
                    wholeCellText = rng.Comment.Text("", 1, 0);
                    typeText = wholeCellText.Split(' ');
                    strPar = strPar + typeText[0] + ",";
                    j++;
                    rng = xSheet.get_Range(TestUtility.ConvCHAR(j) + i, TestUtility.ConvCHAR(j) + i);
                }

                if (strPar == "Expect,")
                    strPar = null;

                if (strPar != null)
                {
                    strPar = strPar.Replace("Expect,", "");
                    arrayPar = strPar.Split(',');
                    AmbiguousOverLoad = new Type[arrayPar.Length - 1];
                    for (int parPos = 0; parPos < arrayPar.Length - 1; parPos++)//enumerate parameters
                    {
                        TestUtility.ConvertStringToType(arrayPar[parPos], ref AmbiguousOverLoad[parPos]);
                    }
                    mi = typ.GetMethod(range.Value2.ToString(), AmbiguousOverLoad);
                }
                else//if (strPar == "")
                    mi = typ.GetMethod(range.Value2.ToString(), new Type[0]);
            }
            cLeft = new CodeFieldReferenceExpression(null, "mName");
            cRight = new CodeFieldReferenceExpression(null, "\"" + mi.Name + "\"");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
        }

        private void ThuThapCacThamSochoKiemThu(ref int i, ref int j, ref Excel.Range rng, ref ParameterInfo[] ps, ref string parStr)
        {
            foreach (ParameterInfo p in ps)
            {
                rng = xSheet.get_Range(TestUtility.ConvCHAR(p.Position + 3) + i, TestUtility.ConvCHAR(p.Position + 3) + i); //added for pass parameter by object					
                cLeft = new CodeFieldReferenceExpression(null, "range");
                cRight = new CodeFieldReferenceExpression(null, "xSheet.get_Range(\"" + TestUtility.ConvCHAR(p.Position + 3) + i + "\", \"" + TestUtility.ConvCHAR(p.Position + 3) + i + "\")");
                cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
                cLeft = new CodeFieldReferenceExpression(null, TestUtility.SysToCSPro(p.ParameterType.ToString()) + " " + p.Name + "_" + i);

                if (rng.Value2.ToString().ToUpper().StartsWith("WINREG"))
                {
                    NeedWinReg = true;
                    if (p.ParameterType == typeof(string) || p.ParameterType.ToString() == "System.String&")
                        cRight = new CodeFieldReferenceExpression(null, "(string)GetWinRegValue(range.Value2.ToString())");
                    else if (p.ParameterType == typeof(bool) || p.ParameterType.ToString() == "System.Boolean&")
                        cRight = new CodeFieldReferenceExpression(null, "(bool)GetWinRegValue(range.Value2.ToString())");
                    else if (p.ParameterType == typeof(object) || p.ParameterType.ToString() == "System.Object&")
                        cRight = new CodeFieldReferenceExpression(null, "GetWinRegValue(range.Value2.ToString())");
                    else
                        cRight = new CodeFieldReferenceExpression(null, "(int)GetWinRegValue(range.Value2.ToString())");
                }
                else if (p.ParameterType.ToString().StartsWith("System.String") || p.ParameterType.ToString().StartsWith("System.Object"))
                {
                    cRight = new CodeFieldReferenceExpression(null, "range.Value2.ToString()");
                }
                else if (p.ParameterType.IsEnum)
                {
                    cRight = new CodeFieldReferenceExpression(null, p.ParameterType.ToString() + "." + rng.Value2);
                }
                else if (p.ParameterType.ToString().IndexOf("[]") > 0)
                {
                    if (TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]", "")) == "string")
                    {
                        cRight = new CodeFieldReferenceExpression(null, "range.Value2.ToString().Split(',')");
                    }
                    else
                    {
                        cRight = new CodeFieldReferenceExpression(null, " null");
                        cRight = new CodeFieldReferenceExpression(null, "new " + TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]", "")) + "[range.Value2.ToString().Split(',').Length]");
                        cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
                        cLeft = new CodeFieldReferenceExpression(null, "foreach (string z in range.Value2.ToString().Split(',')){" + p.Name + "_" + i + "[tempArrayIndex]");
                        cRight = new CodeFieldReferenceExpression(null, TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]", "")) + ".Parse(z); tempArrayIndex++;}");
                    }
                }
                else if (p.ParameterType.ToString().IndexOf("Text.StringBuilder") > 0)
                {
                    cRight = new CodeFieldReferenceExpression(null, "new System.Text.StringBuilder()");
                }
                else if (rng.Value2.ToString() == "new")
                {
                    TaoThongSoDoiTuong(ref i, p);
                }
                else
                {
                    cRight = new CodeFieldReferenceExpression(null, TestUtility.SysToCSPro(p.ParameterType.ToString().Replace("[]", "")) + ".Parse(range.Value2.ToString())");
                }

                AddStubDecision(rng);
                if (p.ParameterType.ToString().IndexOf("[]") > 0)
                {
                    cLeft = new CodeFieldReferenceExpression(null, "tempArrayIndex");
                    cRight = new CodeFieldReferenceExpression(null, "0");
                    cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
                }

                if (p.ParameterType.ToString().IndexOf("Text.StringBuilder") > 0)
                {
                    pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "range.Value2.ToString()") };
                    cm.Statements.Add(new CodeMethodInvokeExpression(null, p.Name.ToString() + "_" + i + ".Append", pCode));
                }

                if (p.Position != 0)
                {
                    if (p.ParameterType.ToString().IndexOf("&") > 0)
                    {
                        if (!p.IsOut)
                            parStr = parStr + ", ref " + p.Name + "_" + i;
                        else
                            parStr = parStr + ", out " + p.Name + "_" + i;
                    }
                    else
                        parStr = parStr + ", " + p.Name + "_" + i;
                }
                else if (p.ParameterType.ToString().IndexOf("&") > 0)
                {
                    if (!p.IsOut)
                        parStr = parStr + "ref " + p.Name + "_" + i;
                    else
                        parStr = parStr + "out " + p.Name + "_" + i;
                }
                else
                    parStr = parStr + p.Name + "_" + i;
            }
        }
        private void AddInvokeTestMethodCallCode(ref int i, ref string parStr, ref MethodInfo mi, ref CodeStatement[] trySt, ref CodeExpression[] passPar)
        {
            if (mi.ReturnType.ToString() == "System.Void")
            {
                string propertySETname = "xxx" + mi.Name.ToString();
                if (propertySETname.Trim().StartsWith("xxxset_"))
                {
                    trySt = new CodeStatement[] {new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "obj" + typ.Name + "." + mi.Name.Replace("set_", "") + " = ", pCode)),
													new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "TestPass", passPar)),
					};
                }
                else
                {
                    trySt = new CodeStatement[] {new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "obj" + typ.Name + "." + mi.Name, pCode)),
													new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "TestPass", passPar)),
					};
                }
            }
            else
            {
                string propertyname = "xxx" + mi.Name.ToString();
                if (propertyname.Trim().StartsWith("xxxget_"))
                {
                    pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "" + i + ", " + 4 + ", obj" + typ.Name + "." + mi.Name.Replace("get_", "")) };
                    trySt = new CodeStatement[] {new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode)),
													new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "TestPass", passPar)),
					};
                }
                else
                {

                    pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "" + i + ", " + 4 + ", obj" + typ.Name + "." + mi.Name + "(" + parStr + ")"+".ToString()") };
                    trySt = new CodeStatement[] {new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode)),
													new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "TestPass", passPar)),
					};
                }
            }
            //TestFail(xResult, shtRow, mName, err.Message);
            passPar = new CodeExpression[] { new CodeFieldReferenceExpression(null, "xResult, shtRow, mName, err.Message") };
            CodeCatchClause catchCl = new CodeCatchClause("err", new CodeTypeReference(typeof(Exception)), new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "TestFail", passPar)));
            CodeCatchClause[] catchSt = new CodeCatchClause[]{ catchCl
															 };
            cm.Statements.Add(new CodeTryCatchFinallyStatement(trySt, catchSt));
        }
        private void LogParmResultAndReturnValue(ref int i, int colPosition, ParameterInfo[] ps, ref MethodInfo mi)
        {
            foreach (ParameterInfo p in ps)
            {
                colPosition = p.Position + 6;
                pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow" + ", " + colPosition + ", \"" + p.Name + " = \" + " + p.Name + "_" + i) };
                cm.Statements.Add(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode));
            }
            //added to transfer expected return to the result sheet
            if (mi.ReturnType.ToString() != "System.Void")
            {
                if (colPosition > 0)
                    colPosition -= 6;
                else
                    colPosition -= 1;

                AddMethodCodes(cm, colPosition, i);
            }
        }
        private void AddMethodCodes(CodeMemberMethod cm, int colPosition, int i)
        {
            CodeExpression[] pCode = null;
            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;

            cLeft = new CodeFieldReferenceExpression(null, "range");
            cRight = new CodeFieldReferenceExpression(null, "xSheet.get_Range(\"" + TestUtility.ConvCHAR(colPosition + 4) + i + "\", \"" + TestUtility.ConvCHAR(colPosition + 4) + i + "\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //if (range.Value2 != null)
            //	xResult.Cells.set_Item(shtRow, 5, range.Value2.ToString());
            //			pCode = new CodeExpression[]{new CodeFieldReferenceExpression(null, "shtRow" + ", " + 5 + ", range.Value2.ToString()")};
            //			cm.Statements.Add(new CodeMethodInvokeExpression(null, "if (range.Value2 != null) xResult.Cells.set_Item", pCode));
            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow, 5, range.Value2.ToString()") };
            CodeConditionStatement ifState = new CodeConditionStatement(new CodeSnippetExpression("range.Value2 != null"), new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode)));
            cm.Statements.Add(ifState);



            cLeft = new CodeFieldReferenceExpression(null, "rangeCurr");
            cRight = new CodeFieldReferenceExpression(null, "xResult.get_Range(\"" + TestUtility.ConvCHAR(4) + i + "\", \"" + TestUtility.ConvCHAR(4) + i + "\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //if (range.Value2.ToString() != range.Value2.ToString() && range.Value2 != null)
            //	range.Interior.ColorIndex = 3;
            cLeft = new CodeFieldReferenceExpression(null, "if (rangeCurr.Value2 != null) if (range.Value2 != null && rangeCurr.Value2.ToString() != range.Value2.ToString()) rangeCurr.Interior.ColorIndex");
            cRight = new CodeFieldReferenceExpression(null, "3");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            cLeft = new CodeFieldReferenceExpression(null, "if (rangeCurr.Value2 != null) if (range.Value2 != null && rangeCurr.Value2.ToString() == range.Value2.ToString()) rangeCurr.Interior.ColorIndex");
            cRight = new CodeFieldReferenceExpression(null, "4");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

        }
        string XlsReportFilename = @"C:/Temp"; //textbox10text
        string tempTestProjDir = @"C:/Temp";//textbox8text
        private void AddOtherCodesPartII(CodeMemberMethod cm, string clsName)
        {
            CodeExpression[] pCode = null;
            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;

            cLeft = new CodeFieldReferenceExpression(null, "string datetime");
            cRight = new CodeFieldReferenceExpression(null, "DateTime.Now.Date.ToShortDateString() + DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //datetime = datetime.Replace("/", "");
            cLeft = new CodeFieldReferenceExpression(null, "datetime");
            cRight = new CodeFieldReferenceExpression(null, "datetime.Replace(\"/\", \"\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //datetime = datetime.Replace(":", "");
            cLeft = new CodeFieldReferenceExpression(null, "datetime");
            cRight = new CodeFieldReferenceExpression(null, "datetime.Replace(\":\", \"\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //string resultFile =System.IO.Path.Combine(Environment.CurrentDirectory, "xyz" + x.GetDate().Replace("/","") + x.GetTime().Replace(":","") + ".xls");
            cLeft = new CodeFieldReferenceExpression(null, "string resultFile");
            cRight = new CodeFieldReferenceExpression(null, "System.IO.Path.Combine(\"" + XlsReportFilename + "\", \"" + clsName + "\" + datetime + \".xls\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //			xBook2.SaveAs(resultFile, -4143, "", "", false, false, 0, "", 0, "", "", "");
            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "resultFile, -4143, \"\", \"\", false, false, 0, \"\", 0, \"\", \"\", \"\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xBook2.SaveAs", pCode));
            //			xBook.Close(null, null, null);
            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, " false, null, false") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xBook.Close", pCode));
            //			xBook2.Close(null, null, null);
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xBook2.Close", pCode));
            //			xApp.Quit();
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xApp.Quit"));
            //			xSheet = null;
            cLeft = new CodeFieldReferenceExpression(null, "xSheet");
            cRight = new CodeFieldReferenceExpression(null, " null");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //			xResult = null;
            cLeft = new CodeFieldReferenceExpression(null, "xResult");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //			xBook = null;
            cLeft = new CodeFieldReferenceExpression(null, "xBook");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //			xBook2 = null;
            cLeft = new CodeFieldReferenceExpression(null, "xBook2");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //			xApp = null;
            cLeft = new CodeFieldReferenceExpression(null, "xApp");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
        }

        private void AddUpClassesMethods()
        {
            co.Members.Add(cm);

            CreateTestPassMethod(cm, "TestPass");
            CreateTestFailMethod(cm, "TestFail");

            //add a file name field
            AddFilenameField(co);
            //add a constructor
            AddCstorCodes(co);
            //add a class to start the Main method
            CodeTypeDeclaration eco = new CodeTypeDeclaration("Start" + clsName);
            eco.TypeAttributes = TypeAttributes.Public;
            cnamespace.Types.Add(eco);

            //add the Main method

            AddMainMethod(eco, clsName);
            //eco.Members.Add(cm);
        }
        private void AddFilenameField(CodeTypeDeclaration co)
        {
            CodeMemberField cf = new CodeMemberField();
            cf.Name = "fileName = \"" + xlsDataStoreFilename + "\"";
            cf.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            cf.Type = new CodeTypeReference(typeof(string));
            co.Members.Add(cf);
        }
        private void AddCstorCodes(CodeTypeDeclaration co)
        {
            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;

            
            CodeConstructor cc = new CodeConstructor();
            cc.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            co.Members.Add(cc);

            //overload a constructor to load a data store file
            cc = new CodeConstructor();
            cc.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            cc.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "fileName"));
            cLeft = new CodeFieldReferenceExpression(null, "this.fileName");
            cRight = new CodeFieldReferenceExpression(null, "fileName");
            cc.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            co.Members.Add(cc);
        }

        private void AddMainMethod(CodeTypeDeclaration eco, string clsName)
        {
            CodeEntryPointMethod entryCM = new CodeEntryPointMethod();//  CodeMemberMethod();
            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;

            //entryCM.Name = "Main";
            //cm.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            //entryCM.Parameters.Add(new CodeParameterDeclarationExpression("System.String[]", "agrs"));
            //cm.ReturnType = new CodeTypeReference(typeof(void));

            //string rootDir = Environment.CurrentDirectory;
            cLeft = new CodeFieldReferenceExpression(null, "string rootDir");
            cRight = new CodeFieldReferenceExpression(null, "Environment.CurrentDirectory");
            entryCM.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //tested //string [] fileArray = null;
            entryCM.Statements.Add(new CodeParameterDeclarationExpression(new CodeTypeReference("System.String", 1), "fileArray = null"));
            //StreamReader sr = File.OpenText("fileArray.txt");
            cLeft = new CodeFieldReferenceExpression(null, "StreamReader sr");
            cRight = new CodeFieldReferenceExpression(null, "File.OpenText(System.IO.Path.Combine(\"" + tempTestProjDir + "/" + dirName + "/Bin/Debug" + "\", \"fileArray.txt\"))");
            entryCM.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //string input = null
            cLeft = new CodeFieldReferenceExpression(null, "string input");
            cRight = new CodeFieldReferenceExpression(null, " null");
            entryCM.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            //string fStr = null;
            cLeft = new CodeFieldReferenceExpression(null, "string fStr");
            cRight = new CodeFieldReferenceExpression(null, " null");
            entryCM.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //tested //while((input = sr.ReadLine()) != null) fStr = fStr + input + ",";
            cLeft = new CodeFieldReferenceExpression(null, "while ((input = sr.ReadLine()) != null) fStr");
            cRight = new CodeFieldReferenceExpression(null, "fStr + input + \",\"");
            entryCM.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //Tested //sr.Close();
            entryCM.Statements.Add(new CodeMethodInvokeExpression(null, "sr.Close"));
            //fStr = fStr.Replace("/", "\\")
            cLeft = new CodeFieldReferenceExpression(null, "fStr");
            cRight = new CodeFieldReferenceExpression(null, "fStr.Replace(\"/\", \"\\\\\")");
            entryCM.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //fileArray = fStr.Split(',');
            cLeft = new CodeFieldReferenceExpression(null, "fileArray");
            cRight = new CodeFieldReferenceExpression(null, "fStr.Split(\',\')");
            entryCM.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //TestDotNetClassLib test = null;
            cLeft = new CodeFieldReferenceExpression(null, clsName + " test");
            cRight = new CodeFieldReferenceExpression(null, " null");
            entryCM.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            //for (int i = 0; i < fileArray.Length; i++)
            //{
            //	test =new TestDotNetClassLib(fileArra[i]);
            //	test.StartTest();
            //}
            cLeft = new CodeFieldReferenceExpression(null, "test");
            cRight = new CodeFieldReferenceExpression(null, "new " + clsName + "(System.IO.Path.Combine(rootDir, fileArray[i]))");
            CodeIterationStatement loop = new CodeIterationStatement(
                new CodeVariableDeclarationStatement("System.Int32", "i", new CodePrimitiveExpression(0)),
                new CodeBinaryOperatorExpression(
                new CodeFieldReferenceExpression(null, "i"),
                CodeBinaryOperatorType.LessThan,
                new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(null, "fileArray"), "Length - 1")
                ),
                new CodeAssignStatement(
                new CodeFieldReferenceExpression(null, "i"),
                new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(null, "i"),
                CodeBinaryOperatorType.Add,
                new CodePrimitiveExpression(1))
                ),
                new CodeStatement[]
	{
		new CodeAssignStatement(cLeft, cRight),
		new CodeExpressionStatement(new CodeMethodInvokeExpression(null, "test.StartTest"))
	}
                );
            entryCM.Statements.Add(loop);
            eco.Members.Add(entryCM);
            
        }
       
       
        private void TaoThongSoDoiTuong(ref int i, ParameterInfo p)
        {
            if (chckManualStub.Checked)
            {
                Excel.Range stubRang = xSheet.get_Range(TestUtility.ConvCHAR(p.Position + 3) + i, TestUtility.ConvCHAR(p.Position + 3) + i);
                string[] stubType = stubRang.Comment.Text("", 1, 0).Split(' ');
                OBJStub(stubType[0], p.Name, i);

                if (stubForm.txtDllToStub.Text.Length > 0)//added for integration
                {
                    stubForm.MakeNewObjOfStub(stubForm.txtDllToStub.Text, i, p);
                }
                if (stubForm.m_RealObject != "" || stubForm.m_RealObject != null)
                {
                    cm.Statements.Add(new CodeSnippetStatement(stubForm.m_RealObject));// CodeMethodInvokeExpression(pState, stubForm.txtConstructor.Text));
                    stubForm.m_RealObject = "";
                    stubForm.txtDllToStub.Text = "";
                }

                if (stubForm.txtConstructor.Text.Trim() != "")
                {
                    cm.Statements.Add(new CodeSnippetStatement(stubForm.txtConstructor.Text));// CodeMethodInvokeExpression(pState, stubForm.txtConstructor.Text));
                }
                if (stubForm.txtMethod.Text.Trim() != "")
                {
                    cm.Statements.Add(new CodeSnippetStatement(stubForm.txtMethod.Text));// CodeMethodInvokeExpression(pState, stubForm.txtConstructor.Text));
                }
            }
            else
            {
                cRight = new CodeFieldReferenceExpression(null, "new " + p.ParameterType.ToString() + "()");
            }

        }

        private StubForm stubForm = new StubForm();
        private void OBJStub(string typeStr, string pName, int i)
        {
            ThucHienBangTay(typeStr);

            string PathStr = null;

            ChuyenDenFileKiemThu(out PathStr, typeStr);

            try
            {
                Assembly asm = Assembly.LoadFrom(PathStr);
                Type type = asm.GetType(typeStr);// + "." + nameSPStr[nameSPStr.Length-1]);
                string list1Str = "";
                ConstructorInfo[] cis = type.GetConstructors();
                foreach (ConstructorInfo ci in cis)
                {
                    list1Str += type.Name + "(";
                    ParameterInfo[] ps = ci.GetParameters();
                    foreach (ParameterInfo p in ps)
                    {
                        list1Str += p.ParameterType + " " + p.Name + ", ";
                    }
                    if (list1Str.IndexOf(",") > 0)
                    {
                        list1Str += "Xtra";
                        list1Str = list1Str.Replace(", Xtra", ")");
                    }
                    if (list1Str.EndsWith("("))
                        list1Str += ")";
                    list1Str = type.Name.ToString() + " " + pName + "_" + i + " = new " + list1Str + ";";
                    stubForm.lstConstructors.Items.Add(list1Str);
                    list1Str = "";
                }
                MethodInfo[] mis = type.GetMethods();
                foreach (MethodInfo mi in mis)
                {
                    list1Str += mi.Name + "(";
                    ParameterInfo[] ps = mi.GetParameters();
                    foreach (ParameterInfo p in ps)
                    {
                        list1Str += p.ParameterType + " " + p.Name + ", ";
                    }
                    if (list1Str.EndsWith(", "))
                    {
                        list1Str += "Xtra";
                        list1Str = list1Str.Replace(", Xtra", ")");
                    }
                    if (list1Str.Trim().EndsWith("("))
                        list1Str += ")";
                    if (list1Str.Trim().StartsWith("set_"))
                    {
                        list1Str = list1Str.Replace("set_", "");
                        list1Str = list1Str.Replace("(", " = ");
                        list1Str = list1Str.Replace(")", "");
                    }
                    list1Str = pName + "_" + i + "." + list1Str + ";";
                    stubForm.lstMethods.Items.Add(list1Str);
                    list1Str = "";
                }
                stubForm.ShowDialog(this);
            }
            catch
            {
                stubForm.txtDllToStub.Text = PathStr;

                if (!stubForm.TestClassFound)
                {
                    stubForm.txtDllToStub.Text = "";
                    MessageBox.Show("The assembly you selected is neither a test script object nor an already referenced instance. Pleae click OK to write C# code in the right text areas to make the Automated Software Test generate a precise test project.\n\nOr, if you are not sure, please restart the Automated Software Test. Make sure the Manual Stub checkbox is not checked before you create script.", "Wrong Stub Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stubForm.ShowDialog();
                }
                stubForm.TestClassFound = false;
            }
        }

        private void ThucHienBangTay(string typeStr)
        {
            stubForm.Text = typeStr + " Stub";
            stubForm.txtConstructor.Text = "";
            stubForm.txtMethod.Text = "";
            stubForm.lstConstructors.Items.Clear();
            stubForm.lstMethods.Items.Clear();
            stubForm.ControlBox = false;
        }
        private void ChuyenDenFileKiemThu(out string PathStr, string typeStr)
        {
            PathStr = null;
            openFileDialog1.Title = "Locate the " + typeStr;
            openFileDialog1.Filter = "DLL Files(*.dll)|*.dll|All Files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                PathStr = openFileDialog1.FileName;
        }
        private void AddStubDecision(Excel.Range rng)
        {
            if (!(rng.Value2.ToString() == "new" && chckManualStub.Checked))
            {
                cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            }
        }
       
        private void CreateTestPassMethod(CodeMemberMethod cm, string MethodName)
        {
            cm = new CodeMemberMethod();

            CodeExpression[] pCode = null;
            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;

            cm.Name = MethodName;//"TestPass";
            cm.ReturnType = new CodeTypeReference(typeof(void));

            cm.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            cm.Parameters.Add(new CodeParameterDeclarationExpression("Excel.Worksheet", "xResult"));
            cm.Parameters.Add(new CodeParameterDeclarationExpression("System.Int32", "shtRow"));
            cm.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "mName"));

            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Range), "range"));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow, 1, mName") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow, 2, \"PASS\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow, 3, \"NO ERROR\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode));

            cLeft = new CodeFieldReferenceExpression(null, "range");
            cRight = new CodeFieldReferenceExpression(null, "xResult.get_Range(\"B\" + shtRow, \"B\" + shtRow)");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            cLeft = new CodeFieldReferenceExpression(null, "range.Interior.ColorIndex");
            cRight = new CodeFieldReferenceExpression(null, "10");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            co.Members.Add(cm);
        }
        private void CreateTestFailMethod(CodeMemberMethod cm, string MethodName)
        {
            CodeExpression[] pCode = null;
            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;
            cm = new CodeMemberMethod();

            cm.Name = MethodName;//"TestFail";
            cm.ReturnType = new CodeTypeReference(typeof(void));
            cm.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            cm.Parameters.Add(new CodeParameterDeclarationExpression("Excel.Worksheet", "xResult"));
            cm.Parameters.Add(new CodeParameterDeclarationExpression("System.Int32", "shtRow"));
            cm.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "mName"));
            cm.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "errMsg"));

            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Excel.Range), "range"));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow, 1, mName") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow, 2, \"FAIL\"") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode));

            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "shtRow, 3, errMsg") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "xResult.Cells.set_Item", pCode));

            cLeft = new CodeFieldReferenceExpression(null, "range");
            cRight = new CodeFieldReferenceExpression(null, "xResult.get_Range(\"B\" + shtRow, \"B\" + shtRow)");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            cLeft = new CodeFieldReferenceExpression(null, "range.Interior.ColorIndex");
            cRight = new CodeFieldReferenceExpression(null, "3");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));
            co.Members.Add(cm);
        }

        
        string dirName;
        string AssemblyNameUT;
        string testResultStr;
        string TestScriptCSFilename;
        string CSProjFilename;
        string AsmInfoFilename;
        string AppIconFilename;
        private void InitConstStrings()
        {
            try
            {
                dirName = openFileDialog1.FileName;
                dirName = dirName.Replace("\\", "/");
                string[] DirName = dirName.Split('/');
                dirName = DirName[DirName.Length - 1];
                dirName = dirName.Replace(".dll", "");
            }
            catch (Exception err)
            {
                MessageBox.Show("Select a DLL file\n" + err.Message, "XOA Software Tester");
            }

            if (txtTargetProj.Text == "textBox8")
            {
                txtTargetProj.Text = "C:/Temp";//textbox8text=tempTestProjDir
            }
            tempTestProjDir = txtTargetProj.Text.Replace("\\", "/");

            AssemblyNameUT = openFileDialog1.FileName; //textbox1
            xlsDataStoreFilename = tempTestProjDir + "/" + dirName + "/Bin/Debug/test" + dirName + "Data.xls"; //textbox2
            testResultStr = tempTestProjDir + "/" + dirName + "/TestResult.xls";//textbox3
            //TestScriptCSFilename = tempTestProjDir + "/" + dirName + "/test" + dirName + ".cs";//textbox4
            TestScriptCSFilename = tempTestProjDir + "/" + dirName + "/OATLS1TESTRep.cs";
            CSProjFilename = tempTestProjDir + "/" + dirName + "/Test" + dirName + ".csproj";//textbox5
            AsmInfoFilename = tempTestProjDir + "/" + dirName + "/AssemblyInfo.cs";//textbox6
            AppIconFilename = tempTestProjDir + "/" + dirName + "/App.ico";//textbox7
            if (txtCurrDir.Text == "Current Directory") //textbox10
                txtCurrDir.Text = txtTargetProj.Text;
            XlsReportFilename = txtCurrDir.Text.Replace("\\", "/");
        }
        private void TaoThuMucKiemThu() 
        {
            DirectoryInfo dir = new DirectoryInfo(tempTestProjDir);
            try
            {
                if (dirName != null)
                    dir.CreateSubdirectory(dirName);
            }
            catch (IOException err) { MessageBox.Show(err.Message); }
            dir = new DirectoryInfo(tempTestProjDir + "/" + dirName);
            try
            {
                dir.CreateSubdirectory("Bin");
            }
            catch (IOException err) { MessageBox.Show(err.Message); }
            dir = new DirectoryInfo(tempTestProjDir + "/" + dirName + "/Bin");
            try
            {
                dir.CreateSubdirectory("Debug");
            }
            catch (IOException err) { MessageBox.Show(err.Message); }
        }

        private void TaoNguonThuThapDuLieu(string Content)
        {
            FileInfo f = new FileInfo(System.IO.Path.Combine(tempTestProjDir + "/" + dirName + "/Bin/Debug", "fileArray.txt"));
            StreamWriter sw = f.CreateText();
            sw.Write(Content);
            sw.Close();
        }

        string rootDir = Environment.CurrentDirectory;
        private void ThaoTacCopyFile(string LoaiFile)
        {
            FileInfo f = null;
            if (LoaiFile == "ico")
            {
                f = new FileInfo(rootDir + "/testApp.ico");
                try
                {
                    FileInfo AppF = f.CopyTo(AppIconFilename, true);
                }
                catch (Exception err) { MessageBox.Show(err.Message); }
            }
            else
            {
                f = new FileInfo(rootDir + "/TestAssemblyInfo.cs");
                try
                {
                    FileInfo AppF = f.CopyTo(AsmInfoFilename, true);
                }
                catch (Exception err) { MessageBox.Show(err.Message); }
            }
        }

        //add references tự động
        //public bool AddReference(EnvDTE.Project project, string reference)
        //{
        //    VSProject proj = m_Project.Object as VSProject;
        //    System.Diagnostics.Debug.Assert(proj != null); // This project is not a VSProject
        //    if (proj == null)
        //        return false;
        //    try
        //    {
        //        proj.References.Add(reference);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = String.Format("Could not add {0}. \n Exception: {1}", reference, ex.Message);
        //        System.Diagnostics.Trace.WriteLine(message);
        //        return false;
        //    }

        //    return true;
        //}

        
        private void Copytatca(string sourceDir, string targetDir)
        {
            foreach(var file in Directory.GetFiles(sourceDir))
    
        File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

           foreach(var directory in Directory.GetDirectories(sourceDir))
        Copytatca(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }

        private void DocFileCSPROJ(string clsName, Assembly asm)
        {
            StreamReader sr = File.OpenText(rootDir + "/OATestProj.csproj");
            string input = null;
            string output = null;
            string refStr = "";
            string relPath = TestScriptCSFilename;//textBox4.Text;
            string[] RelPath = relPath.Split('/');
            relPath = RelPath[RelPath.Length - 1];


            //dirName = dirName.Replace("\\", "/");
            //    string[] DirName = dirName.Split('/');
            //    dirName = DirName[DirName.Length - 1];
            //    dirName = dirName.Replace(".dll", "");


            //string filedll=tempTestProjDir+"/"+dirName+"/Bin/Debug"+dirName+".dll";
            


            while (null != (input = sr.ReadLine()))
            {
                //input = sr.ReadLine();
                if (input.IndexOf("OATLStest") > 0)
                {
                    input = input.Replace("OATLStest", clsName);
                }
                if (input.IndexOf("</References>") > 0)
                {
                    AssemblyName[] asmRefs = asm.GetReferencedAssemblies();

                    foreach (AssemblyName aRef in asmRefs)
                    {
                        if (aRef.FullName.IndexOf("mscorlib") < 0)
                        {
                            refStr = refStr + "<Reference\n";
                            refStr = refStr + "Name = \"" + aRef.Name + "\"\n";
                            refStr = refStr + "AssemblyName = \"" + aRef.Name + "\"\n";
                            refStr = refStr + "HintPath = \"" + aRef.CodeBase + "\"\n" + "/>\n";
                        }
                    }
                    refStr = refStr + "<Reference\n" +
                        "Name = \"" + asm.FullName.Substring(0, asm.FullName.IndexOf(", ")) + "\"\n" +
                        "AssemblyName = \"" + asm.FullName.Substring(0, asm.FullName.IndexOf(", ")) + "\"\n" +
                        "HintPath = \"" + asm.CodeBase + "\"\n" +
                        "/>\n";
                    

                    refStr = refStr + "<Reference\n" +
                        "Name = \"" + dirName + "\"\n" +
                        "AssemblyName = \"" + dirName + "\"\n" +
                        "HintPath = \"" + tempTestProjDir + "/" + dirName + "/Bin/Debug" + dirName + ".dll" + "\"\n" +
                        "/>\n";

                    if (stubForm.m_CreateReference != "") //added for integration
                    {
                        refStr += stubForm.m_CreateReference;
                        stubForm.m_CreateReference = "";
                    }

                    refStr = refStr.Replace("file:///", "");
                    refStr = refStr.Replace("/", "\\");
                    input = refStr + input + "\n";
                }

                if (input.IndexOf("RelPath = \"OATLS1TESTRep.cs\"") > 0)
                {
                    input = input.Replace("OATLS1TESTRep.cs", relPath);
                }
                output = output + input + "\n";
            }
            FileInfo f = new FileInfo(CSProjFilename);
            StreamWriter write = f.CreateText();
            write.Write(output);
            write.Close();
            sr.Close();
        }

        private void BatDauDOTNETIDE(string fileName, string args)
        {
            try
            {
                Process shell = new Process();
                shell.StartInfo.FileName = fileName;
                shell.StartInfo.Arguments = args;
                shell.Start();
            }
            catch (Exception er)
            { MessageBox.Show(er.Message); }
        }
       
        private void TestForm_Load(object sender, EventArgs e)
        {
            try
            {
                string DevenvPath = Environment.ExpandEnvironmentVariables(
                    @"%VSCOMNTOOLS%devenv.exe").Replace(@"Tools", "IDE").Replace("\"", "");

                if (!File.Exists(DevenvPath)) //dùng cho visual studio 2008
                {
                    DevenvPath = Environment.ExpandEnvironmentVariables(
                @"%VS90COMNTOOLS%devenv.exe").Replace(@"Tools", "IDE");
                }

                txtDotNETLocation.Text = DevenvPath;
            }
            catch
            {
            }

            btnStart.Focus(); 


        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //mở file dll hoặc file exe.
            GetAssemblyName();
            //tạo các thư mục để chứa các file .cs, .csproj..trong ổ C
            InitConstStrings();
            //Tạo thư mục kiểm thử
            TaoThuMucKiemThu();
            //
            GetTypesOfAssemblyUnderTest();
            //khởi tạo phương thức kiểm thử
            Khoitaophuongthuckiemthu();

            if (chckXMLDataDoc.Checked)
            {
                ThaoTacDulieuXML();
                btnCreateScript_Click(sender, e);
            }

            //btnCreateScript.Focus();
        }

        private void btnCreateScript_Click(object sender, EventArgs e)
        {
            btnStart.Focus();
            if (dirName == null || xApp == null)
            {
                MessageBox.Show("Click the Start button to collect test information first.");
                return;
            }
            //Create a TextWriter object to save the script
            TextWriter t = null;

            t = new StreamWriter(new FileStream(TestScriptCSFilename, FileMode.Create));
            //start generating script
            try
            {
                TaoMaKiemThu(DUTAsm, t);
            }
            catch (Exception err)
            { MessageBox.Show(err.Message); }
            finally
            {
                t.Close();
            }

            DongExcelSheet();
            ThaoTacCopyFile("ico");
            ThaoTacCopyFile("cs");
            TaoNguonThuThapDuLieu(xlsDataStoreFilename);
            txtDataStore.Text = xlsDataStoreFilename;
            BatDauDOTNETIDE(txtDotNETLocation.Text, CSProjFilename);
            coppyfile();
            //coppydll();
            string nguon = @"D:/thao" + "/" + dirName + "/" + dirName + "/Bin/Debug";
            string targetPath = tempTestProjDir + "/" + dirName + "/Bin/Debug";
            //tatcafilecopy(nguon, targetPath);
            Copytatca(nguon, targetPath);
        }



        private void coppyfile()
        {
            string fileName = "Interop.Microsoft.Office.Interop.Excel.dll";
            string sourcePath = @"D:\thao\de tai\AutomatedTest\AutomatedTest\bin\Debug";
            string targetPath = tempTestProjDir + "/" + dirName + "/Bin/Debug";

            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);
            File.Copy(sourceFile, destFile);
        }



        //thao tac regedit
        bool NeedWinReg;
        private void MakeIfStatementForAccessRegKey(CodeMemberMethod cm, string strCondition, string strLeft, string strRight)
        {
            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;
            cLeft = new CodeFieldReferenceExpression(null, strLeft);
            cRight = new CodeFieldReferenceExpression(null, strRight);
            CodeConditionStatement ifState = new CodeConditionStatement(new CodeSnippetExpression(strCondition), new CodeAssignStatement(cLeft, cRight));
            cm.Statements.Add(ifState);
        }

        private void CreateAccessRegKeyMethod(CodeMemberMethod cm, string MethodName)
        {
            cm = new CodeMemberMethod();

            cm.Name = MethodName;
            cm.ReturnType = new CodeTypeReference(typeof(void));
            cm.Attributes = MemberAttributes.Private | MemberAttributes.Final;

            cm.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "locHive"));
            cm.Parameters.Add(new CodeParameterDeclarationExpression("ref RegistryKey", "key"));


            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;

            cLeft = new CodeFieldReferenceExpression(null, "locHive");
            cRight = new CodeFieldReferenceExpression(null, "locHive.Trim()");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            MakeIfStatementForAccessRegKey(cm, "locHive == \"HKEY_CURRENT_CONFIG\"", "key", "Registry.CurrentConfig");
            MakeIfStatementForAccessRegKey(cm, "locHive == \"HKEY_LOCAL_MACHINE\"", "key", "Registry.LocalMachine");
            MakeIfStatementForAccessRegKey(cm, "locHive == \"HKEY_CURRENT_USER\"", "key", "Registry.CurrentUser");
            MakeIfStatementForAccessRegKey(cm, "locHive == \"HKEY_CLASSES_ROOT\"", "key", "Registry.ClassesRoot");
            MakeIfStatementForAccessRegKey(cm, "locHive == \"HKEY_USERS\"", "key", "Registry.Users");

            co.Members.Add(cm);
        }

        private void CreateGetWinRegValueMethod(CodeMemberMethod cm, string MethodName)
        {
            cm = new CodeMemberMethod();

            cm.Name = MethodName;
            cm.ReturnType = new CodeTypeReference(typeof(object));
            cm.Attributes = MemberAttributes.Private | MemberAttributes.Final;

            cm.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "WinRegKey"));

            CodeFieldReferenceExpression cLeft = null;
            CodeFieldReferenceExpression cRight = null;

            cLeft = new CodeFieldReferenceExpression(null, "WinRegKey");
            cRight = new CodeFieldReferenceExpression(null, "WinRegKey.ToUpper().Replace(\"WINREG:\", \"\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(Microsoft.Win32.RegistryKey), "key = null"));

            cm.Statements.Add(new CodeParameterDeclarationExpression(typeof(object), "WinRegVal = \"\""));

            cLeft = new CodeFieldReferenceExpression(null, "string[] HiveSubs");
            cRight = new CodeFieldReferenceExpression(null, "WinRegKey.Split('\\\\')");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));

            cLeft = new CodeFieldReferenceExpression(null, "WinRegKey");
            cRight = new CodeFieldReferenceExpression(null, "WinRegKey.Replace(HiveSubs[0]+@\"\\\", \"\").Replace(@\"\\\"+HiveSubs[HiveSubs.Length-1], \"\")");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));


            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, "HiveSubs[0], ref key") };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "AccessRegKey", pCode));

            cLeft = new CodeFieldReferenceExpression(null, "key");
            cRight = new CodeFieldReferenceExpression(null, "key.OpenSubKey(WinRegKey)");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));


            cLeft = new CodeFieldReferenceExpression(null, "WinRegVal");
            cRight = new CodeFieldReferenceExpression(null, "key.GetValue(HiveSubs[HiveSubs.Length-1])");
            cm.Statements.Add(new CodeAssignStatement(cLeft, cRight));


            pCode = new CodeExpression[] { new CodeFieldReferenceExpression(null, null) };
            cm.Statements.Add(new CodeMethodInvokeExpression(null, "key.Close", pCode));


            cm.Statements.Add(new CodeMethodReturnStatement(new CodeSnippetExpression("WinRegVal")));
            co.Members.Add(cm);
        }






        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAddDataStore_Click(object sender, EventArgs e)
        {
            if (dirName == null)
            {
                MessageBox.Show("Please click Start button, then Create Script button to generate a test script first.");
                return;
            }
            openFileDialog1.Title = "Add more data store";
            openFileDialog1.Filter = "Excel Files (*.xls)|*.xls";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string fn in openFileDialog1.FileNames)
                {
                    txtDataStore.Text += "\n" + fn;
                }
            }
        }

        private void btnSaveDataStore_Click(object sender, EventArgs e)
        {
            if (txtDataStore.Text.Trim() == "")
                return;
            TaoNguonThuThapDuLieu(txtDataStore.Text);
        }
    }
}
