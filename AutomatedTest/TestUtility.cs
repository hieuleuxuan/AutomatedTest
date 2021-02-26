using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatedTest
{
    class TestUtility
    {
        public TestUtility()
        {
        }
        public static string ConvCHAR(int pPosition)
        {
            string kytudau = "";
            if (pPosition > 26)
            {
                pPosition -= 26;
                kytudau = "A";
            }
            byte aByte = byte.Parse((pPosition + 64).ToString());
            byte[] bytes1 = { aByte, 0x42, 0x43 };
            byte[] bytes2 = { 0x98, 0xe3 };
            char[] chars = new char[30];

            Decoder d = Encoding.UTF8.GetDecoder();
            int charLen = d.GetChars(bytes1, 0, bytes1.Length, chars, 0);
            // The value of charLen should be 2 now.
            charLen += d.GetChars(bytes2, 0, bytes2.Length, chars, charLen);
            foreach (char c in chars)
            {
                Console.Write("U+" + ((ushort)c).ToString() + "  ");

                return kytudau + c.ToString();
            }
            return "Need a entry";
        }

        public static string SysToCSPro(string s)
        {
            string x = "";
            if (s.StartsWith("System.Boolean"))
                x = "bool";
            else if (s.StartsWith("System.Int16"))
                x = "short";
            else if (s.StartsWith("System.SByte"))
                x = "sbyte";
            else if (s.StartsWith("System.Byte"))
                x = "byte";
            else if (s.StartsWith("System.UI16"))
                x = "ushort";
            else if (s.StartsWith("System.Int32"))
                x = "int";
            else if (s.StartsWith("System.Int64"))
                x = "long";
            else if (s.StartsWith("System.Char"))
                x = "char";
            else if (s.StartsWith("System.Single"))
                x = "float";
            else if (s.StartsWith("System.Double"))
                x = "double";
            else if (s.StartsWith("System.Decimal"))
                x = "decimal";
            else if (s.StartsWith("System.String"))
                x = "string";
            else if (s.StartsWith("System.Object"))
                x = "object";
            else if (s.StartsWith("System.UInt32"))
                x = "uint";
            else if (s.StartsWith("System.UInt64"))
                x = "ulong";
            else
                x = s;

            if (s.EndsWith("[]"))
                x = x + "[]";
            return x;
        }
        public static void ConvertStringToType(string parName, ref Type type)
        {
            if (parName == "string")
                type = typeof(string);
            else if (parName == "int")
                type = typeof(int);
            else if (parName == "True" || parName == "False" || parName == "bool")
                type = typeof(bool);
            else if (parName == "double")
                type = typeof(double);
            else if (parName == "float")
                type = typeof(float);
            else if (parName == "object")
                type = typeof(object);
            else if (parName == "byte")
                type = typeof(byte);
            else if (parName == "sbyte")
                type = typeof(sbyte);
            else if (parName == "short")
                type = typeof(short);
            else if (parName == "ushort")
                type = typeof(ushort);
            else if (parName == "long")
                type = typeof(long);
            else if (parName == "uint")
                type = typeof(uint);
            else if (parName == "ulong")
                type = typeof(ulong);
            else if (parName == "char")
                type = typeof(char);
            else if (parName == "decimal")
                type = typeof(decimal);
            else if (parName == "bool")
                type = typeof(bool);
            else if (parName == "System.Text.StringBuilder")
                type = typeof(System.Text.StringBuilder);
            else if (parName == "System.IFormatProvider")
                type = typeof(System.IFormatProvider);
            else if (parName == "System.Array")
                type = typeof(System.Array);
            else if (parName == "System.AppDomain")
                type = typeof(System.AppDomain);
            else if (parName == "System.CharEnumerator")
                type = typeof(System.CharEnumerator);
            else if (parName == "System.Type")
                type = typeof(System.Type);
            else if (parName == "System.Runtime.Serialization.SerializationInfo")
                type = typeof(System.Runtime.Serialization.SerializationInfo);
            else if (parName == "VBIDE.CodePane")
                type = typeof(VBIDE.CodePane);
            else if (parName == "VBIDE.VBProject")
                type = typeof(VBIDE.VBProject);
            else if (parName == "VBIDE.vbext_WindowType")
                type = typeof(VBIDE.vbext_WindowType);
            else if (parName == "VBIDE.AddIn")
                type = typeof(VBIDE.AddIn);
            else if (parName == "VBIDE.Window")
                type = typeof(VBIDE.Window);
            else if (parName == "VBIDE.VBComponent")
                type = typeof(VBIDE.VBComponent);
            else if (parName == "VBIDE.Reference")
                type = typeof(VBIDE.Reference);
            else if (parName == "VBIDE._dispReferences_Events_ItemAddedEventHandler")
                type = typeof(VBIDE._dispReferences_Events_ItemAddedEventHandler);
            else if (parName == "VBIDE._dispReferences_Events_ItemRemovedEventHandler")
                type = typeof(VBIDE._dispReferences_Events_ItemRemovedEventHandler);
            else if (parName == "VBIDE._dispCommandBarControlEvents_ClickEventHandler")
                type = typeof(VBIDE._dispCommandBarControlEvents_ClickEventHandler);
            else if (parName == "VBIDE.VBComponent")
                type = typeof(VBIDE.VBComponent);
            else if (parName == "TESTTYPE")
                type = typeof(int);//Type.GetType("TESTTYPE");
        }
    }
}
