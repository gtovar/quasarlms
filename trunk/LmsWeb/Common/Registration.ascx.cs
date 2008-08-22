using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace DCE.Common
{

	/// <summary>
	/// ����������� ������ ��������
	/// </summary>
	public partial  class Registration : DCE.BaseWebControl
	{
		protected void createUserWizard_CreatedUser(object sender, EventArgs e)
		{
			string _login = this.createUserWizard.UserName;
			
			System.Web.UI.Control _template = this.createUserWizard.CreateUserStep.ContentTemplateContainer;
			TextBox _tbLastName = _template.FindControl("tbLastName") as TextBox;
			TextBox _tbFirstName = _template.FindControl("tbFirstName") as TextBox;
			TextBox _tbMidName = _template.FindControl("tbMidName") as TextBox;

			Profile.LastName = _tbLastName.Text;
			Profile.FirstName = _tbFirstName.Text;
			Profile.Patronymic = _tbMidName.Text;
			
			Profile.Save();
		}
	}
}
