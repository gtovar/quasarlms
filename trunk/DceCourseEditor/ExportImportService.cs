using System;
using System.Data;
using System.Windows.Forms;
using DCEAccessLib;
using System.Xml;
using System.IO;
using System.Text;

namespace DCECourseEditor
{
   /// <summary>
	/// �����, �������������� ������� � ������ ������
	/// </summary>
	public class ExportImportService
	{
      /// <summary>
      /// ��������������� �����
      /// </summary>
      public void ExportCourse(string oldCourseId, string CourseName)
      {
         // ������ ������ �������� �����
         SaveFileDialog d = new SaveFileDialog();

         d.Filter = "Courses files (xml)|*.xml|All files|*.*";
         d.InitialDirectory = DCEUser.CourseRootPath;
         d.Title = "������� �����";
         
         if (d.ShowDialog() == DialogResult.OK)
         {
            string newCourseId = Guid.NewGuid().ToString();

            try
            {
               string Version = DCEAccessLib.DCEWebAccess.WebAccess.GetString(
                  "select Version from dbo.Courses where id = '" + oldCourseId + "'");
            
               // �������� ����� ��������������� �����
               DCEAccessLib.DCEWebAccess.execSQL(
                  "exec MakeCourseCopy '" + 
                  newCourseId + "', '" + oldCourseId + "', '" + Version + "', ''");
               
               // ������� ����� ������ ���������� �������������� ����
               DataSet exportDS = new DataSet(CourseName);
                  
               // ���������� �����
               exportDS.Merge(
                  DCEAccessLib.DCEWebAccess.GetdataSet(
                  "select * from dbo.Courses where id = '" + newCourseId + "'", 
                  "Courses"), 
                  false, 
                  MissingSchemaAction.Add);
            
               // ���������� �������� �����
               MergeContent(exportDS, "Courses", "Name");
               MergeContent(exportDS, "Courses", "Author");
               MergeContent(exportDS, "Courses", "DescriptionShort");
               MergeContent(exportDS, "Courses", "DescriptionLong");
               MergeContent(exportDS, "Courses", "Requirements");
               MergeContent(exportDS, "Courses", "Keywords");
               MergeContent(exportDS, "Courses", "Additions");

               // ���������� ��� �����
               exportDS.Merge(
                  DCEAccessLib.DCEWebAccess.GetdataSet(
                  "select * from dbo.Themes where dbo.CourseOfTheme(id) = '" + newCourseId + "' order by TOrder", 
                  "Themes"), 
                  false, 
                  MissingSchemaAction.Add);
            
               // ���������� �������� ���
               MergeContent(exportDS, "Themes", "Name");
               MergeContent(exportDS, "Themes", "Content");

               // ���������� ������ �����
               exportDS.Merge(
                  DCEAccessLib.DCEWebAccess.GetdataSet(
                  "select * from dbo.Tests where dbo.CourseOfTest(id) = '" + newCourseId + "'", 
                  "Tests"), 
                  false, 
                  MissingSchemaAction.Add);

               // ���������� �������� �����
               exportDS.Merge(
                  DCEAccessLib.DCEWebAccess.GetdataSet(
                  "select * from dbo.TestQuestions where dbo.CourseOfTestQuestion(id) = '" + newCourseId + "' order by QOrder", 
                  "TestQuestions"), 
                  false, 
                  MissingSchemaAction.Add);
            
               // ���������� �������� ��������
               MergeContent(exportDS, "TestQuestions", "Content");
               MergeContent(exportDS, "TestQuestions", "ShortHint");
               MergeContent(exportDS, "TestQuestions", "LongHint");
               MergeContent(exportDS, "TestQuestions", "Answer");

               // ���������� �������� �����
               exportDS.Merge(
                  DCEAccessLib.DCEWebAccess.GetdataSet(
                  "select t.id, t.Name, t.Text from dbo.VTerms t, dbo.Vocabulary v " + 
                  "where v.Term = t.id and v.Course = '" + newCourseId + "'", 
                  "Terms"), 
                  false, 
                  MissingSchemaAction.Add);

               // ���������� �������� ��������
               MergeContent(exportDS, "Terms", "Name", "TermsContent");
               MergeContent(exportDS, "Terms", "Text", "TermsContent");

               // ��������� xml � xsd DataSet � ����
               exportDS.WriteXml(d.FileName, XmlWriteMode.WriteSchema);
            }
            catch(Exception e)
            {
               throw e;
            }
            finally
            {
               // ������� ����� ����� �� ���� ������
               DCEAccessLib.DCEWebAccess.execSQL(
                  "delete from dbo.Courses where id = '" + newCourseId + "'");
            }
         }
      }
      
      private void MergeContent(DataSet ds, string table, string field)
      {
         MergeContent(ds, table, field, "Content");
      }
      
      private void MergeContent(DataSet ds, string table, string field, string contenttable)
      {
         DataSet content = new DataSet();

         DataView dv = new DataView(ds.Tables[table]);
         
         foreach (DataRowView row in dv)
         {
            string id = row[field].ToString();

            content.Merge( 
               DCEAccessLib.DCEWebAccess.GetdataSet(
                  "select * from dbo.Content where eid = '" + id + "' order by COrder",
                  contenttable)
               );
         }

         ds.Merge(content, false, MissingSchemaAction.Add);
      }
      
      /// <summary>
      /// �������������� �����
      /// </summary>
      public void ImportCourse(out string courseid, string domain)
      {
         courseid = null;

         OpenFileDialog d = new OpenFileDialog();

         d.Filter = "Courses files (xml)|*.xml|All files|*.*";
         d.InitialDirectory = DCEUser.CourseRootPath;
         d.Title = "������ �����";

         // ������� ����, ���������� ���������������� ����
         if (d.ShowDialog() == DialogResult.OK)
         {
            DataSet importDS = new DataSet();
         
            importDS.ReadXml(d.FileName, XmlReadMode.ReadSchema);
         
            ImportForm form = new ImportForm(domain);
            form.CourseName = importDS.DataSetName;

            // ����� ����� � ������� �����
            if (form.ShowDialog() == DialogResult.OK)
            {
               DataView dv = new DataView(importDS.Tables["Courses"]);
               
               dv[0].BeginEdit();
               dv[0]["DiskFolder"] = form.DiskFolder;
               
               if (form.DomainId == null || form.DomainId == "")
                  dv[0]["Area"] = DBNull.Value;
               else
                  dv[0]["Area"] = form.DomainId;

               if (form.TypeId == null || form.TypeId == "")
                  dv[0]["Type"] = DBNull.Value;
               else
                  dv[0]["Type"] = form.TypeId;

               dv[0].EndEdit();

               courseid = dv[0]["id"].ToString();

               // �������� �� ������������� �������������� ����� � ����
               string basecourseid = DCEAccessLib.DCEWebAccess.WebAccess.GetString(
                  "select id from dbo.Courses where id = '" + courseid + "'");

               if (basecourseid != "")
                  throw new DCEException("����� ���� ��� ���������� � ����", DCEException.ExceptionLevel.InvalidAction);
                  
               // ���������� ������ ������ ��������� � ������
               try
               {
                  DCEAccessLib.DCEWebAccess.SaveDataSet(
                     "select * from dbo.Courses", "Courses", importDS);

                  DCEAccessLib.DCEWebAccess.SaveDataSet(
                     "select * from dbo.Themes", "Themes", importDS);

                  DCEAccessLib.DCEWebAccess.SaveDataSet(
                     "select * from dbo.Tests", "Tests", importDS);

                  DCEAccessLib.DCEWebAccess.SaveDataSet(
                     "select * from dbo.TestQuestions", "TestQuestions", importDS);

                  DCEAccessLib.DCEWebAccess.SaveDataSet(
                     "select * from dbo.Content", "Content", importDS);

                  // �������� ������������� ������ ����� ��������� � ������������ ������

                  if (importDS.Tables["Terms"].Rows.Count != 0)
                  {
                     DataSet dsVocabulary = importDS.Copy();
                     DataView dvVocabulary = new DataView(dsVocabulary.Tables["Terms"]);

                     DataView dvterms = new DataView(importDS.Tables["Terms"]);
                     DataView dvcontent = new DataView(importDS.Tables["TermsContent"]);

                     for (int i=dvterms.Count-1; i>=0; i--)
                     {
                        // ������� ������ �� ������� ������� ��� � ����

                        string TermId = DCEAccessLib.DCEWebAccess.WebAccess.GetString(
                           "select id from VTerms where id = '" + dvterms[i]["id"].ToString() + "'");
               
                        if (TermId != "")
                        {
                           for(int j=dvcontent.Count-1; j>=0; j--)
                           {
                              if ( dvcontent[j]["eid"].ToString() == dvterms[i]["Name"].ToString() ||
                                 dvcontent[j]["eid"].ToString() == dvterms[i]["Text"].ToString() )
                              {
                                 dvcontent[j].Delete();
                              }
                           }
                  
                           dvterms[i].Delete();
                           continue;
                        }
                     }
            
                     //dvcontent.Table.AcceptChanges();
                     //dvterms.Table.AcceptChanges();
            
                     // ���������� ������ ����� ��������� � ������������� ������

                     DCEAccessLib.DCEWebAccess.SaveDataSet(
                        "select * from dbo.VTerms", "Terms", importDS);
            
                     foreach(DataRowView row in dvVocabulary)
                     {
                        DCEAccessLib.DCEWebAccess.execSQL(
                           "insert into dbo.Vocabulary values ( newid(), '" + 
                           courseid + "', '" + row["id"] + "')");
                     }

                     DCEAccessLib.DCEWebAccess.SaveDataSet(
                        "select * from dbo.Content", "TermsContent", importDS);
                  }
               }
               catch(Exception e)
               {
                  DCEAccessLib.DCEWebAccess.execSQL(
                     "delete from dbo.Courses where id = '" + courseid + "'");
                     
                  throw e;
               }
            }
         }            
      }
      
      private XmlNamespaceManager nsmgr;
      private XmlNode root;
      string manifestbase;
      string manifestbasepath = "";
      string coursefolder = null;
      bool leafresource;

      /// <summary>
      /// �������������� SCORM �����
      /// </summary>
      public void ImportSCORM(out string courseid, string domain)
      {
         courseid = null;
         
         OpenFileDialog d = new OpenFileDialog();

         d.Filter = "SCORM imsmanifest files(xml)|*.xml";
         d.InitialDirectory = DCEUser.CourseRootPath;
         d.Title = "������ SCORM �����";

         if (d.ShowDialog() == DialogResult.OK)
         {
            XmlDocument imsmanifest = new XmlDocument();
            imsmanifest.Load(d.FileName);

            manifestbase = System.IO.Path.GetDirectoryName(d.FileName) + "\\";

            root = imsmanifest.DocumentElement;

            if (root == null || root.Name.ToLower() != "manifest")
               throw new DCEException("�������� xml", DCEException.ExceptionLevel.InvalidAction);
            
            leafresource = false;
            string version = "";

            XmlNode n = root.Attributes.GetNamedItem("version");
            
            if (n != null)
            {
               leafresource = ( (version = n.InnerText) == "1.3" );
            }

            XmlNode basepath = root.Attributes.GetNamedItem("xml:base");
               
            if (basepath != null)
            {
               manifestbasepath += basepath.InnerText; // ��������� ������� ����
               
               if (basepath.InnerText[basepath.InnerText.Length-1] != '/')
                  manifestbasepath += "/";
            }

            // TODO : �������� metadata � ����� �������� shemaversion
               
            nsmgr = new XmlNamespaceManager( imsmanifest.NameTable );

            XmlNode node = root.Attributes.GetNamedItem("xmlns");
            if (node == null)
            {
               //   throw new DCEException("�� ������� ����������� ������������ ����", DCEException.ExceptionLevel.InvalidAction);
               nsmgr.AddNamespace(String.Empty, "");
               nsmgr.AddNamespace("defns", "");
            }
            else
            {
               nsmgr.AddNamespace(String.Empty, node.InnerText);
               nsmgr.AddNamespace("defns", node.InnerText);
            }

            XmlNode organizations = root.SelectSingleNode("defns:organizations", nsmgr);
            if (organizations == null)
               throw new DCEException("�� ������� �� ������ �����", DCEException.ExceptionLevel.InvalidAction);

            // TODO : ����������� ����������� �� ��������
            XmlNode defnode = organizations.Attributes.GetNamedItem("default");
            
            if (defnode == null)
               throw new DCEException("�� ������� �� ������ �����", DCEException.ExceptionLevel.InvalidAction);

            string deforgident = defnode.InnerText;

            XmlNodeList organization = organizations.SelectNodes("defns:organization", nsmgr);

            if (organization == null || organization.Count == 0)
               throw new DCEException("�� ������� �� ������ �����", DCEException.ExceptionLevel.InvalidAction);
               
            foreach ( XmlNode course1 in organization )
            {
               XmlNode course = course1;
               // ��������� ������ ����� �� ���������
               if (course.Attributes.GetNamedItem("identifier").InnerText == deforgident)
               {
                  XmlNodeList items = course.SelectNodes("defns:item", this.nsmgr);
                     
                  if (items == null || items.Count == 0)
                     throw new DCEException("�� ������� �� ������ �����", DCEException.ExceptionLevel.InvalidAction);
                     
                  if(items.Count == 1)
                     course = items[0];

                  // �������� �����
                  string title = GetInnerTextAlt(course.SelectSingleNode("defns:title", nsmgr), deforgident);

                  ImportForm form = new ImportForm(domain);
                  form.CourseName = title;
                     
                  if (form.ShowDialog() == DialogResult.OK)
                  {
                     courseid = System.Guid.NewGuid().ToString();
                        
                     coursefolder = DCEUser.CourseRootPath + form.DiskFolder;

                     string nameid = Guid.NewGuid().ToString();

                     try
                     {
                        DCEAccessLib.DCEWebAccess.execSQL(
            
                           "INSERT INTO Courses (id, Version, Code, Name, DiskFolder, Area, Type) " + 
                           "VALUES ('" + courseid + "','','', '" + nameid + "', '" + 
                           form.DiskFolder + "'," + 
                           (domain == null ? "Null" : "'" + domain + "', ") + 
                           (form.TypeId.ToString() == "" ? "Null" : "'" + form.TypeId + "'") + ") " + 
            
                           "INSERT INTO Content (id, eid, datastr, lang, COrder) " + 
                           "VALUES (newid(), '" + nameid + "', '" + title + "', 1, dbo.GetMaxCOrder() ) " + 

                           "INSERT INTO Content (id, eid, datastr, lang, COrder) " + 
                           "VALUES (newid(), '" + nameid + "', '" + title + "', 2, dbo.GetMaxCOrder() ) "
            
                           );

                        // ��������� ���� items
                        ProcessAllItems(course, courseid, null);
                     }
                     catch (Exception e)
                     {
                        DCEAccessLib.DCEWebAccess.execSQL(
                           "delete from Courses where id = " + "'" + courseid + "'");

                        throw e;
                     }

                     ProcessAllResources();

                     if (version == "1.1")
                        MessageBox.Show("SCORM 1.1 ! ���������� ����� ����� ���������� � ������!");
                  }
               }
            }
         }
      }
      
      protected string GetInnerTextAlt(XmlNode node, string altstring)
      {
         if (node != null)
            return node.InnerText;
         else
            return altstring;
      }

      protected void ProcessAllItems(XmlNode course, string parentid, string contentid)
      {
         XmlNodeList items = course.SelectNodes("defns:item", nsmgr);
         
         foreach( XmlNode item in items)
         {
            XmlNode resref = item.Attributes.GetNamedItem("identifierref");

            if (leafresource)
            {
               // version = 1.3
               
               if (resref != null)
               {
                  // ���� ������
               
                  if (contentid != null)
                  {
                     // �������� - ����

                     // �������� � ���� ��������
                     ProcessResource(resref.InnerText, contentid);
                  }
                  else
                  {
                     // �������� - ����

                     // ������� ���� � �������� � ��� ��������
                  
                     string materialid = System.Guid.NewGuid().ToString();
                  
                     ProcessTheme(
                        System.Guid.NewGuid().ToString(), 
                        materialid, 
                        parentid, 
                        GetInnerTextAlt(item.SelectSingleNode("defns:title", nsmgr), "no title"));
                  
                     ProcessResource(resref.InnerText, materialid);
                  }
               }
               else
               {
                  // ���� �������

                  // ������� ���� � ���������� ��� �������
               
                  string themeid = System.Guid.NewGuid().ToString();
                  string materialid = System.Guid.NewGuid().ToString();

                  ProcessTheme(
                     themeid, 
                     materialid, 
                     parentid, 
                     GetInnerTextAlt(item.SelectSingleNode("defns:title", nsmgr), "no title"));

                  ProcessAllItems(item, themeid, materialid);
               }
            }
            else
            {
               // version = 1.0 - 1.2
               
               string materialid = System.Guid.NewGuid().ToString();
               string themeid = System.Guid.NewGuid().ToString();
                  
               ProcessTheme(
                  themeid, 
                  materialid, 
                  parentid, 
                  GetInnerTextAlt(item.SelectSingleNode("defns:title", nsmgr), "no title"));
                  
               if (resref != null)
                  ProcessResource(resref.InnerText, materialid);

               ProcessAllItems(item, themeid, materialid);
            }
         }
      }

      protected void ProcessTheme(string themeid, string contentid, string parentid, string themename)
      {
         string nameid = Guid.NewGuid().ToString();

         DCEAccessLib.DCEWebAccess.execSQL(
            
            "INSERT INTO Themes (id, Name, Parent, Content) " + 
            "VALUES ('" + themeid + "', '" + nameid + "', '" + parentid + "', '" + contentid + "') " + 
            
            "INSERT INTO Content (id, eid, datastr, lang, COrder) " + 
            "VALUES (newid(), '" + nameid + "', '" + themename + "', 1, dbo.GetMaxCOrder() ) " + 
            
            "INSERT INTO Content (id, eid, datastr, lang, COrder) " + 
            "VALUES (newid(), '" + nameid + "', '" + themename + "', 2, dbo.GetMaxCOrder() ) "
            
            );
      }

      protected void ProcessResource(string resid, string contentid)
      {
         XmlNode resources = root.SelectSingleNode("defns:resources", nsmgr);
                     
         string currentbase = this.manifestbasepath;
         
         XmlNode resbasepath = resources.Attributes.GetNamedItem("base", "xml");
         
         if (resbasepath != null)
            currentbase += resbasepath.InnerText; // ��������� ������� ����

         XmlNodeList resource = resources.SelectNodes("defns:resource", nsmgr);
         
         foreach( XmlNode item in resource)
         {
            string resourceid = item.Attributes.GetNamedItem("identifier").InnerText;

            if (resourceid == resid)
            {
               XmlNode resourcebasepath = item.Attributes.GetNamedItem("base", "xml");
                        
               if (resourcebasepath != null)
                  currentbase += resourcebasepath.InnerText; // ��������� ������� ����
         
               XmlNode reshref = item.Attributes.GetNamedItem("href"); // (optional)
               
               if (reshref != null)
               {
                  // ���������� ���������

                  string urlr = "RU/" + currentbase + reshref.InnerText;
                  string urle = "EN/" + currentbase + reshref.InnerText;
                  
                  DCEAccessLib.DCEWebAccess.execSQL(
                     
                     "INSERT INTO Content (id, eid, datastr, lang, COrder) " + 
                     "VALUES (newid(), '" + contentid + "', '" + urlr + "', 1, dbo.GetMaxCOrder() ) " + 
                     
                     "INSERT INTO Content (id, eid, datastr, lang, COrder) " + 
                     "VALUES (newid(), '" + contentid + "', '" + urle + "', 2, dbo.GetMaxCOrder() ) "

                     );
               }

               break;
            }
         }
      }
      protected void ProcessAllResources()
      {
         XmlNode resources = root.SelectSingleNode("defns:resources", nsmgr);
                     
         string currentbase = this.manifestbasepath;
         
         XmlNode resbasepath = resources.Attributes.GetNamedItem("xml:base");
         
         if (resbasepath != null)
         {
            currentbase += resbasepath.InnerText; // ��������� ������� ����
            
            if (resbasepath.InnerText[resbasepath.InnerText.Length-1] != '/')
               currentbase += "/";
         }

         XmlNodeList resource = resources.SelectNodes("defns:resource", nsmgr);
         
         string _currentbase = currentbase;
         
         foreach( XmlNode item in resource)
         {
            currentbase = _currentbase;
            
            XmlNode resourcebasepath = item.Attributes.GetNamedItem("xml:base");
                     
            if (resourcebasepath != null)
            {
               currentbase += resourcebasepath.InnerText; // ��������� ������� ����
               
               if (resourcebasepath.InnerText[resourcebasepath.InnerText.Length-1] != '/')
                  currentbase += "/";
            }
      
            XmlNodeList resfiles = item.SelectNodes("defns:file", nsmgr);
         
            foreach(XmlNode file in resfiles)
            {
               string resfilename = currentbase + file.Attributes.GetNamedItem("href").InnerText;

               string f1 = manifestbase + resfilename;
               string f2r = coursefolder + "RU/" + resfilename;
               string f2e = coursefolder + "EN/" + resfilename;

               try
               {
                  if (!Directory.Exists(Path.GetDirectoryName(f2r)))
                  Directory.CreateDirectory(Path.GetDirectoryName(f2r));

                  File.Copy(f1, f2r, true);
                  File.SetAttributes(f2r, FileAttributes.Normal);

                  if (!Directory.Exists(Path.GetDirectoryName(f2e)))
                  Directory.CreateDirectory(Path.GetDirectoryName(f2e));

                  File.Copy(f1, f2e, true);
                  File.SetAttributes(f2e, FileAttributes.Normal);
               }
               catch (Exception)
               {

               }
            }
         }
      }
      
      protected void InvolveContent(string contentid, string st, string datafield, int type, ref string query)
      {
         //st.Replace("'", "''");
         string st1 = st.Replace("'", "''");
         
//         for (int i = 0; i<st.Length; i++)
//         {
//            if (st[i].ToString() == "'" )
//               st1 += "'";
//            
//            st1 += st[i];
//         }

         query += "\ninsert into Content (eid, " + datafield + ", lang, type) values ( " + 
            "'" + contentid + "', '" + 
            st1 + "', " + 
            "1," + 
            type + ")\n";
         
         query += "\ninsert into Content (eid, " + datafield + ", lang, type) values ( " + 
            "'" + contentid + "', '" + 
            st1 + "', " + 
            "2," + 
            type + ")\n";
      }
      
      public void ImportQuestions(string testid)
      {
         OpenFileDialog dialog = new OpenFileDialog();
         dialog.Title = "������ ������ ��������";
         dialog.Filter = "CSV(Comma delimited)(*.csv)|*.csv";
         dialog.InitialDirectory = DCEUser.CourseRootPath;
         if (dialog.ShowDialog() == DialogResult.OK)
         {
            StreamReader file = null;

            FileStream filestream = new FileStream(dialog.FileName, FileMode.Open);
               
            if (filestream == null)
               throw new DCEException("������ ��� �������� ����� " + dialog.FileName, DCEException.ExceptionLevel.InvalidAction);

            try
            {
               file = new StreamReader( filestream, Encoding.Default);
               
               // ������ �����������
               file.ReadLine();

               while (file.Peek() > -1) 
               {
                  string line = "";
                  int next = (char)file.Read();
                  while (next > -1)
                  {
                     if ((char)next == '\r')
                     {
                        next = (char)file.Read();
                        if (next == -1 || (char)next == '\n')
                           break;
                     }
                     line += (char)next;
                     next = file.Read();
                  }


                  string[] tokens = line/*file.ReadLine()*/.Split(';');
                  
                  // ������               0
                  // ���                  1
                  // ��. ���������        2
                  // ����. ���������      3
                  // �����                4
                  // ��� ������           5
                  // ����� 6 , ������������ 7, � �.�.

                  if (tokens.Length < 6)
                     new DCEException("������������ ������ ��������� �������", DCEException.ExceptionLevel.InvalidAction);

                  string contentid = Guid.NewGuid().ToString();
                  string shorthint = Guid.NewGuid().ToString();
                  string longhint = Guid.NewGuid().ToString();
                  string answer = Guid.NewGuid().ToString();
                  string questiontype = "";
                  string contenttype = "";
                  int icontenttype = 0;

                  #region ���������� questiontype
                  switch (tokens[1].ToLower())
                  {
                     case "text":
                        questiontype = ((int)ContentType._html).ToString();
                        contenttype = "tdata";
                        icontenttype = ((int)ContentType._html);
                        break;
                     case "url":
                        questiontype = ((int)ContentType._url).ToString();
                        contenttype = "datastr";
                        icontenttype = ((int)ContentType._url);
                        break;
                     case "object":
                        questiontype = ((int)ContentType._object).ToString();
                        contenttype = "datastr";
                        icontenttype = ((int)ContentType._object);
                        break;
                  }
                  #endregion

                  string query = "\ninsert into TestQuestions values ( " + 
                     "newid(), '" + 
                     testid + "', '" + 
                     contentid + "', '" + 
                     shorthint + "', '" + 
                     longhint + "', " + 
                     tokens[4] + ", '" + 
                     answer + "', " +
                     questiontype + ", 0, Null)\n";

                  InvolveContent(contentid, tokens[0], contenttype, icontenttype,  ref query);
                  InvolveContent(shorthint, tokens[2], "datastr", (int)ContentType._string, ref query);
                  InvolveContent(longhint, tokens[3], "datastr", (int)ContentType._string, ref query);

                  string xml;

                  #region ���������� Xml 
                  xml = "<Question type = ";
                  bool ter = false;
                  switch (tokens[5].ToLower())
                  {
                     case "single":
                        xml += "\"single\">";
                        break;
                     case "multiple":
                        xml += "\"multiple\">";
                        break;
                     case "textbox":
                        ter = true;
                        xml += "\"textbox\">";
                        break;
                  }

                  for (int i=6; i<tokens.Length; i+=2)
                  {
                     string right = "false";
                     if (tokens[i+1] != null && tokens[i+1] == "1")
                        right = "true";

                     if (ter)
                     {
                        xml += "<Answer right = \"true\" " +
                           " casesensitive = \"" + right + "\">" 
                           + tokens[i] + "</Answer>"; 
                        
                        break;
                     }
                     else
                     {
                        xml += "<Answer right = \"" + right + "\">" 
                           + tokens[i] + "</Answer>"; 
                     }
                  }
                  xml += "</Question>";
                  #endregion 
                  
                  InvolveContent(answer, xml, "tdata", (int)ContentType._xml, ref query);

                  DCEWebAccess.execSQL(query);
               }
            }
            catch (Exception)
            {
            }
            finally
            {
               file.Close();
            }
         }
      }
   }
}
