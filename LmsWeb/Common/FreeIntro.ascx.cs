namespace DCE.Common
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// Welcome �������� ���������� �����
	/// </summary>
	public partial  class FreeIntro : DCE.BaseTrainingControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			DCE.Service.LoadXmlDoc(this.Page, doc, "Welcome.xml");
			
			DataTable tableCourses = null;

			if (DCE.Service.courseId.HasValue) {

				tableCourses = DceAccessLib.DAL.CourseController.SelectFreeCourse(DCE.Service.courseId.Value);

				if (tableCourses.Rows.Count == 1) {
					DCE.Service.CourseLanguage = tableCourses.Rows[0]["CourseLanguage"].ToString();
					this.Session["courseName"] = tableCourses.Rows[0]["Name"].ToString();
					System.Xml.XmlDocument tdoc = new System.Xml.XmlDocument();
					tdoc.LoadXml(dsCourses.GetXml());

					doc.DocumentElement.InnerXml += tdoc.SelectSingleNode("dataSet/Courses").InnerXml;

					System.Xml.XmlNode startNode = doc.SelectSingleNode("xml/Start");
					startNode.InnerXml += "<href>" + this.ResolveUrl(Resources.PageUrl.PAGE_TRAINING + "?cId=" + DCE.Service.courseId.Value) + "</href>";
				} else {
					this.Response.Redirect(Resources.PageUrl.PAGE_MAIN + "?index="
					+ this.Request["index"] + "&cset=FreeIntro");
				}
			}

			System.Xml.Xsl.XslTransform trans = new System.Xml.Xsl.XslTransform();
			trans.Load(this.Page.MapPath(@"~\xsl\FreeIntro.xslt"));
			
			this.Xml1.Document = doc;
			this.Xml1.TransformArgumentList = new System.Xml.Xsl.XsltArgumentList();
			this.Xml1.TransformArgumentList.AddParam("LangPath", "", 
			DCE.Service.GetLanguagePath(this.Page));
			this.Xml1.Transform = trans;
		}
	}
}
