using System;

namespace DCEAccessLib
{
	/// <summary>
	/// Summary description for XmlReports.
	/// </summary>
	public class XmlReports
	{
		/// <summary>
		/// ��������� ����� - ��������� ������� ��� ��������� ������� �� ������ xslt �������
		/// </summary>
      public XmlReports()
		{
		}
      /// <summary>
      /// ��������� ���� �� �������� ������
      /// </summary>
      /// <param name="resname"></param>
      /// <returns></returns>
      public static string LoadXmlFromResource(string resname)
      {
         System.Reflection.Assembly ass= System.Reflection.Assembly.GetEntryAssembly();
         System.IO.Stream stream = ass.GetManifestResourceStream(resname);
         if (stream != null)
         {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes,0,(int)stream.Length);
            stream.Close();
            return System.Text.Encoding.GetEncoding("windows-1251").GetString(bytes,0,bytes.Length);
         }
         return "";
      }

      /// <summary>
      /// ��������� ���������� xml ������ � ����
      /// </summary>
      /// <param name="xml"></param>
      /// <param name="filename"></param>
      /// <returns></returns>
      public static bool WriteXml(string xml, string filename)
      {
         xml = "\uFEFF" + xml;
         System.Text.UnicodeEncoding enc = new System.Text.UnicodeEncoding();
         byte [] bytes = new byte[enc.GetByteCount(xml)];
         enc.GetBytes(xml,0,xml.Length,bytes,0);
         System.IO.FileStream wrxml = null;
         try
         {
            wrxml = System.IO.File.Create(filename);
         }
         catch (System.IO.IOException)
         {
            System.Windows.Forms.MessageBox.Show("���������� ������� ����: "+filename, "�������� �����");
         }
         if (wrxml != null)
         {
            try
            {
               wrxml.Write(bytes,0,bytes.Length);
            }
            finally
            {
               wrxml.Close();
            }
         }
         return true;
      }

      /// <summary>
      /// ������������� ����� �� ����������� xml ������ �� ������ �������,
      /// ����������� � ������� ������ � ������ xslResName
      /// � ��������� � htmlFile
      /// </summary>
      /// <param name="xml">xml ������</param>
      /// <param name="xslResName">��� ������� � ��������</param>
      /// <param name="htmlFile">��� ��������������� html �����</param>
      public static void ProduceReport(string xml, string xslResName, string htmlFile)
      {
         xml ="<!--?xml-stylesheet type=\"text/xsl\" href=\""+xslResName+"\"?-->"+  "<xml><StatDate>"+DateTime.Now.ToString(DCEAccessLib.Settings.DateFormat)+"</StatDate>"
            + xml + "</xml>";

#if DEBUG
         WriteXml(xml,"output.xml");
#endif

         string xsl = LoadXmlFromResource(xslResName);
         string output = XmlTransform(xml,xsl);

         if (WriteXml(output,htmlFile))
         {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = htmlFile;
            p.StartInfo.UseShellExecute = true;
            p.Start();
         }
      }
      /// <summary>
      /// ������������� ����� �� ����������� xml ������ �� ������ �������,
      /// ����������� � ������� ������ � ������ xslResName
      /// � ��������� � "output.html"
      /// </summary>
      /// <param name="xml">xml ������</param>
      /// <param name="xslResName">��� ������� � ��������</param>
      public static void ProduceReport(string xml, string xslResName)
      {
         ProduceReport(xml, xslResName, "output.html");
      }
      /// <summary>
      /// �������� html ��� �� ������ xml ������ � xsl �������
      /// </summary>
      /// <param name="xml">xml ������</param>
      /// <param name="xsl">xsl ������</param>
      /// <returns></returns>
      public static string XmlTransform(string xml, string xsl)
      {
         System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
         doc.LoadXml(xml);

         System.Xml.Xsl.XslTransform tr = new System.Xml.Xsl.XslTransform();
         System.IO.StringWriter writer = new System.IO.StringWriter();
         
         System.Xml.XmlDocument xsldoc = new System.Xml.XmlDocument();
         xsldoc.LoadXml(xsl);
         tr.Load(xsldoc);
         tr.Transform(doc,null,writer);

         return writer.ToString();
      }
   }
}
