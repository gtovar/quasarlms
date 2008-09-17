	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Xml;
	using System.Web.UI.HtmlControls;
	using System.Linq;
	using N2.Lms.Items;
	
	/// <summary>
	/// ����������� ���������� � �����
	/// </summary>
	public partial  class CourseIntro : N2.Web.UI.ContentUserControl<Course>
	{
		protected override void OnInit(EventArgs e)
		{
			if (null != this.CurrentItem) {
				this.Session["courseName"] = this.CurrentItem.Title;

				this.CurrentItem["MetaKeywrods"] = this.CurrentItem.Keywords;
				this.CurrentItem["MetaDescription"] = this.CurrentItem.Text;
				/*
				var _metaApplier = new N2.Templates.SEO.TitleAndMetaTagApplyer(
					this.Page, this.CurrentItem);*/
			}

			base.OnInit(e);
		}

		Course GetCourse()
		{
			return this.CurrentItem as Course;
		}
	}
