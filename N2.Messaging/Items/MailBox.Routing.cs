﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace N2.Messaging
{
	using N2.Web;
	using N2.Collections;

	public class MailBoxTemplateAttribute : TemplateAttribute
	{
		public MailBoxTemplateAttribute(MailBox.ActionEnum actionUrlSegment, string actionTemplateUrl)
			: base(actionUrlSegment.ToString(), actionTemplateUrl)
		{
		}
	}

	[Template("~/Messaging/UI/Views/MailBox.aspx")]
	[MailBoxTemplate(ActionEnum.Create, "~/Messaging/UI/Views/NewMessage.aspx")]
	[MailBoxTemplate(ActionEnum.Forward, "~/Messaging/UI/Views/NewMessage.aspx")]
	[MailBoxTemplate(ActionEnum.Reply, "~/Messaging/UI/Views/NewMessage.aspx")]
    [MailBoxTemplate(ActionEnum.DrCreate, "~/Messaging/UI/Views/NewMessage.aspx")]
	partial class MailBox
	{
		#region MVC implementation

		public enum ActionEnum
		{
			List,
			Reply,
			Forward,
			Create,
            DrCreate,
            Delete,
            Restore,
            Destroy
		}

		public int msgID { get; set; }

		public override PathData FindPath(string remainingUrl)
		{
			var _matches = Routes.Match(new Uri(BaseUri, remainingUrl));

			if (_matches.Any()) {

				var _match = _matches.First();
				var _action = (ActionEnum)_match.Data;
				string _folder = _match.BoundVariables["folder"] ?? C.Folders.Inbox;
				string _filter = string.Empty;
				
				switch (_action) {
					case ActionEnum.List:
						_filter = _match.BoundVariables["filter"];
						break;
					case ActionEnum.Delete:
                        {
                            int _id = int.Parse(_match.BoundVariables["id"]);
                            var _original = Context.Persister.Get(_id);
                            Context.Persister.Move(_original, MessageStore.RecycleBin);
                        }
                        break;
                    case ActionEnum.Restore:
                        {
                            int _id = int.Parse(_match.BoundVariables["id"]);
                            var _original = Context.Persister.Get(_id);
                            Context.Persister.Move(_original, MessageStore);
                        }
                        break;
                    case ActionEnum.Destroy:
                        {
                            int _id = int.Parse(_match.BoundVariables["id"]);
                            var _original = Context.Persister.Get(_id);
                            Context.Persister.Delete(_original);
                        }
                        break;
					}
				
				return new PathData(
					this,
					string.Concat(
						"~/Messaging/UI/Views/",
						new[] {
							ActionEnum.Create,
                            ActionEnum.DrCreate,
							ActionEnum.Reply,
							ActionEnum.Forward
						}.Contains(_action)
							? "NewMessage"
							: "MailBox",
							".aspx"),
					_action.ToString(),
					_match.BoundVariables["id"] != null ?
                          string.Concat(_folder, "/" + _match.BoundVariables["id"])
                        : string.Concat(
							_folder,
							string.IsNullOrEmpty(_filter)
								? string.Empty
								: "/" + _filter));
			}

			return base.FindPath(remainingUrl);
		}
		
		#endregion MVC implementation

		#region Uri Routing

		readonly static Uri BaseUri = new Uri("http://localhost/");

		static UriTemplateTable s_routes;
		static UriTemplateTable Routes { get { return s_routes ?? (s_routes = GetRoutes()); } }

		static UriTemplateTable GetRoutes()
		{
			var _uris =
				from _pair in new Dictionary<string, ActionEnum> {
					{ "folder/{folder}/filter/{filter}", ActionEnum.List },
					{ "folder/{folder}", ActionEnum.List },
					//{ "delete/{id}", ActionEnum.Delete },
                    { "folder/{folder}/delete/{id}", ActionEnum.Delete },
					//{ "restore/{id}", ActionEnum.Restore },
                    { "folder/{folder}/restore/{id}", ActionEnum.Restore },
					//{ "destroy/{id}", ActionEnum.Destroy },
                    { "folder/{folder}/destroy/{id}", ActionEnum.Destroy },
				}
				select
					new KeyValuePair<UriTemplate, object>(
						new UriTemplate(_pair.Key),
						_pair.Value);
			
			return new UriTemplateTable(BaseUri, _uris);
		}

		#endregion Uri routing

		#region N2 menu support

		class FakeMailBox : MailBox
		{
		}

		ItemList GetFakeChildren()
		{
			if (this is FakeMailBox) {
				return new ItemList();
			}
            
			var _result = new ItemList(new[] {
				new FakeMailBox { Title = MailBoxResource.mnuInboxTitle , Name = "folder/" + C.Folders.Inbox, Parent = this},
				new FakeMailBox { Title = MailBoxResource.mnuDraftsTitle , Name = "folder/" + C.Folders.Drafts, Parent = this},
				new FakeMailBox { Title = MailBoxResource.mnuRecBinTitle , Name = "folder/" + C.Folders.RecyleBin, Parent = this},
				new FakeMailBox { Title = MailBoxResource.mnuSendTitle , Name = "folder/" + C.Folders.Outbox, Parent = this},
			});

			foreach (var _child in _result) {
				((Web.IUrlParserDependency)_child).SetUrlParser(N2.Context.UrlParser);
			}

			return _result;
		}

		public override ItemList GetChildren(params ItemFilter[] filters)
		{
			if (filters.Length == 1 && filters[0] is NavigationFilter) {
				return this.GetFakeChildren();
			}
			return base.GetChildren(filters);
		}

		public override ItemList GetChildren()
		{
			return this.GetFakeChildren();
		}
		
		#endregion N2 menu support
	}
}
