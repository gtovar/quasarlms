﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.ACalendar;
using N2.Resources;
using N2.Templates.Web.UI;

public partial class ACalendar_UI_ACalendar : TemplatePage<N2.ACalendar.ACalendar>, ICallbackEventHandler
{

	public string sCallBack = string.Empty;
	protected HyperLink linkExcel;

	protected bool IsEditable {
		get {
			return N2.Context.Current.SecurityManager.IsAdmin(this.User);
		}
	}

	protected string MonthName(bool full)
	{
		return
			"["
			+ string.Join(
				",",
				Enumerable
					.Range(1, 12)
					.Select(i => "'" + GetMonthName(i, full) + "'")
					.ToArray())
			+ "]";
	}

	protected string CurrentYear()
	{
		if (DateTime.Now.Month < 7) return (DateTime.Now.Year - 1).ToString();
		return DateTime.Now.Year.ToString();
	}

	protected string GetMonthName(int m, bool bFull)
	{
		return
			bFull
				? DateTimeFormatInfo.CurrentInfo.GetMonthName(m)
				: DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(m);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		sCallBack = Page.ClientScript.GetCallbackEventReference(this,
			"message", "TikeInformation", "context");
	}


	// Обработчик события обратного вызова на серверной стороне
	void ICallbackEventHandler.RaiseCallbackEvent(string e)
	{
		var result = e;

		var ser = new DataContractJsonSerializer(typeof(JsonAEvent));
		MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(result));

		var _event = ser.ReadObject(ms) as JsonAEvent;

		foreach (AEvent _e in AEvents)
			if (_e.DateStart == _event.dateStart) {
				if (_event.act == "у") {
					del(_e);
					return;
				}

				if (Convert.ToDateTime(_e.DateEnd) > Convert.ToDateTime(_event.dateEnd)) //существующее событие дольше
               {

					_e.DateStart = oneDayMore(_event.dateEnd);
					this.Engine.Persister.Save(_e);

				} else {
					del(_e);
				}

			}
		save(_event);
	}

	protected string oneDayMore(string inputDate)
	{
		DateTime dt = Convert.ToDateTime(inputDate);
		dt.AddDays(1);
		return dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();
	}

	//Выдача результата
	string ICallbackEventHandler.GetCallbackResult()
	{
		return "";
	}

	protected AEvent[] AEvents {
		get {
			return this.CurrentItem.AEvents.ToArray();
			//where string.Equals(child.To, Profile.UserName, StringComparison.OrdinalIgnoreCase)
		}
		set {
			//this.CurrentItem.Events = value;
		}
	}

	protected string data_in {
		get {
			string _current = "";
			foreach (AEvent _e in AEvents) {
				_current += "{\"act\": \"" + _e.Act + "\", \"dateStart\":\"" + _e.DateStart + "\",\"dateEnd\":\"" + _e.DateEnd + "\"},";// _e.Title.Remove(2);
			}
			if (_current.Length > 1) _current = _current.Remove(_current.Length - 1);
			return _current;
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		Register.JQuery(this.Page);
		Register.StyleSheet(this.Page, "~/Lms/UI/Js/jCal.small.css");
		Register.JavaScript(this.Page, "~/Lms/UI/Js/jQuery.animate.clip.js");
		Register.JavaScript(this.Page, "~/Lms/UI/Js/jQuery.color.js");
		Register.JavaScript(this.Page, "~/Lms/UI/Js/jCal.js");
		Register.JavaScript(this.Page, "~/Lms/UI/Js/json2.js");
		//Register.JavaScript(this.Page, "~/Lms/UI/Js/jQuery.intellisense.js");

		base.OnLoad(e);
	}

	//protected void btnExcel_Click(object sender, EventArgs e)
	//{
	//    var  cals =  (this.CurrentItem.Parent).Children.ToArray() ;
	//    string strURL = "~/Reporting/ReportFiles/"; 
	//     string path = Server.MapPath("../../Reporting/ReportFiles/");
	//   //strURL += ExcelExport.ExportToFile(cals, path);
	//    //Response.Redirect( ExcelExport.ExportToFile(cals, path))
	//    this.linkExcel.NavigateUrl = strURL;
	//    this.linkExcel.Text = "Академические календари";
	//}

	protected void save(JsonAEvent _e)
	{
		//((N2.ACalendar.ACalendar)this.CurrentItem).Text = this.SelectUser.SelectedUser;
		switch (_e.act) {
			case "а":
				AEventAK ev = this.Engine.Definitions.CreateInstance<AEventAK>(this.CurrentItem);
				ev.Act = _e.act;
				ev.DateStart = _e.dateStart;
				ev.DateEnd = _e.dateEnd;
				this.Engine.Persister.Save(ev);
				break;
			case "п":
				AEventPV pv = this.Engine.Definitions.CreateInstance<AEventPV>(this.CurrentItem);
				pv.Act = _e.act;
				pv.DateStart = _e.dateStart;
				pv.DateEnd = _e.dateEnd;
				this.Engine.Persister.Save(pv);
				break;
			case "э":
				AEventES es = this.Engine.Definitions.CreateInstance<AEventES>(this.CurrentItem);
				es.Act = _e.act;
				es.DateStart = _e.dateStart;
				es.DateEnd = _e.dateEnd;
				this.Engine.Persister.Save(es);
				break;
			case "к":
				AEventKO ko = this.Engine.Definitions.CreateInstance<AEventKO>(this.CurrentItem);
				ko.Act = _e.act;
				ko.DateStart = _e.dateStart;
				ko.DateEnd = _e.dateEnd;
				this.Engine.Persister.Save(ko);
				break;
			case "в":
				AEventVS vs = this.Engine.Definitions.CreateInstance<AEventVS>(this.CurrentItem);
				vs.Act = _e.act;
				vs.DateStart = _e.dateStart;
				vs.DateEnd = _e.dateEnd;
				this.Engine.Persister.Save(vs);
				break;
			default:
				//Console.WriteLine("Учеба однако:(");
				break;
		}
	}
	
	protected void del(AEvent _e)
	{
		this.Engine.Persister.Delete(_e);
	}

	protected string ConvertDateString(string inputDate)
	{
		string[] s = inputDate.Split(new Char[] { '/' });
		if (s.Length < 3) return inputDate;
		s[2] = (Convert.ToInt32(s[2])).ToString();
		return s[2] + "." + s[1] + "." + s[0];
	}

	protected void btnSave_Click(object sender, EventArgs e)
	{
		string _current = data_in;
		_current = "[" + _current + "]";

		int _c = this.CurrentItem.AEvents.Count();
		while (this.CurrentItem.AEvents.Count() > 0) {
			AEvent _ce = this.CurrentItem.AEvents.First();
			this.Engine.Persister.Delete(_ce);
		}
		DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JsonAEvent[]));
		MemoryStream ms = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(_current));

		var events = ser.ReadObject(ms) as JsonAEvent[];
		foreach (JsonAEvent _e in events) if (_e.act != null) save(_e);

	}
	
	public class JsonAEvent {
		public string act;
		public string dateStart;
		public string dateEnd;
	}
}
