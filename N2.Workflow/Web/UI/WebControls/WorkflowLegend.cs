﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace N2.Web.UI.WebControls
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:WorkflowLegend runat=server></{0}:WorkflowLegend>")]
	public class WorkflowLegend : WebControl
	{
		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string Text
		{
			get
			{
				String s = (String)ViewState["Text"];
				return ((s == null) ? String.Empty : s);
			}

			set
			{
				ViewState["Text"] = value;
			}
		}

		protected override void RenderContents(HtmlTextWriter output)
		{
			output.Write(Text);
		}
	}
}
