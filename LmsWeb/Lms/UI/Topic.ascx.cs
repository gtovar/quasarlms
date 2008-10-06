using System;
using System.Collections.Generic;
using System.Linq;
using N2.Lms.Items;
using N2.Details;
using N2.Resources;
using N2.Web.UI;
using N2.Templates.Web.UI;
using N2.Templates.Items;

/// <summary>
/// �������� ������� ���������� �� ����
/// </summary>
public partial class Topic : TemplateUserControl<AbstractContentPage, N2.Lms.Items.Topic>
{
	protected override void OnLoad(EventArgs e)
	{
		Register.JQuery(this.Page);
		Register.StyleSheet(this.Page, "~/Lms/UI/Js/jQuery.tabs.css");
		Register.JavaScript(this.Page, "~/Lms/UI/Js/jQuery.tabs.js");
		Register.JavaScript(
			this.Page,
			"~/Lms/UI/Js/n2lms.js",
			N2.Resources.ScriptPosition.Header,
			N2.Resources.ScriptOptions.Include);

		base.OnLoad(e);
	}
}