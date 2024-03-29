﻿namespace N2.Lms.Items
{
	using System;
	using N2.Details;
	using N2.Integrity;

	[Definition]
	[AllowedChildren(typeof(TestQuestion))]
	[RestrictParents(typeof(TopicContainer), typeof(Topic))]
	[WithEditableTitle]
	public partial class Test: ContentItem//, ISurvey
	{
		#region System properties

		public override string IconUrl { get { return "~/Lms/UI/Img/page_script.gif"; } }
		public override bool IsPage { get { return false; } }
		public override string TemplateUrl { get { return
			this.DisplayMultiplePages
				? "~/Lms/UI/Parts/TestMultiPages.ascx"
				: "~/Lms/UI/Parts/Test.ascx"; } }
		
		#endregion System properties

		#region Lms properties

		[EditableCheckBox("Expanded view", 09)]
		public bool DisplayMultiplePages {
			get { return this.GetDetail<bool>("DisplayMultiplePages", false); }
			set { this.SetDetail<bool>("DisplayMultiplePages", value); }
		}

		[EditableCheckBox("Enable instant check", 10,
			HelpTitle = "Allows trainee to check correctness immediatlly upon answering a question")]
		public bool InstantCheckEnabled {
			get { return this.GetDetail<bool>("InstantCheckEnabled", true); }
			set { this.SetDetail<bool>("InstantCheckEnabled", value); }
		}

		[EditableEnum("Test type", 11, typeof(TestTypeEnum))]
		public int TestType {
			get { return (int)(TestTypeEnum)Enum.Parse(typeof(TestTypeEnum), this.GetDetail<string>("Type", "Test"), true); }
			set { this.SetDetail<string>("Type", ((TestTypeEnum)value).ToString()); }
		}

		[EditableTextBox(
			"Duration", 13,
			//DefaultValue = "600",
			Required = true,
			Validate = true,
			ValidationExpression = @"(\d+)")]
		public int Duration
		{
			get { return this.GetDetail<int>("Duration", 600); }
			set { this.SetDetail<int>("Duration", value); }
		}

		[EditableTextBox(
			"Points", 15,
			DefaultValue = "0",
			Required = true,
			Validate = true,
			ValidationExpression = @"(\d+)")]
		public int Points
		{
			get { return this.GetDetail<int>("Points", 600); }
			set { this.SetDetail<int>("Points", value); }
		}

		[EditableCheckBox("Split", 17)]
		public bool Split {
			get { return this.GetDetail<bool>("Split", false); }
			set { this.SetDetail<bool>("Split", value); }
		}

		[EditableCheckBox("Finish automatically", 19)]
		public bool AutoFinish
		{
			get { return this.GetDetail<bool>("AutoFinish", false); }
			set { this.SetDetail<bool>("AutoFinish", value); }
		}

		[EditableCheckBox("Allow changing language", 21)]
		public bool AllowChangingLanguage
		{
			get { return this.GetDetail<bool>("CanSwitchLang", false); }
			set { this.SetDetail<bool>("CanSwitchLang", value); }
		}

		[EditableCheckBox("Show topics", 23)]
		public bool ShowTopics
		{
			get { return this.GetDetail<bool>("ShowThemes", false); }
			set { this.SetDetail<bool>("ShowThemes", value); }
		}

		[EditableEnum("Hint type", 25, typeof(HintTypeEnum))]
		public HintTypeEnum HintType
		{
			get { return (HintTypeEnum)Enum.Parse(typeof(HintTypeEnum), this.GetDetail<string>("Hint", "None"), true); }
			set { this.SetDetail<string>("Hint", value.ToString()); }
		}

		#endregion Lms properties

		public enum TestTypeEnum
		{
			Test = 1,
			Practice,
			Questionaire,
			Global
		}

		public enum HintTypeEnum
		{
			None = 1,
			Single,
			Both
		}
	}
}
