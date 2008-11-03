﻿using N2.Edit.Trash;
using N2.Persistence;

namespace N2.Messaging
{
    using N2.Integrity;
	using N2.Details;
	using N2.Templates.Items;
	using N2.Definitions;
	
	[Definition("Почта", "MailBox")]
    [NotThrowable, NotVersionable]
    [RestrictParents(typeof(IStructuralPage))]
	[ItemAuthorizedRoles("Everyone")]
    public partial class MailBox : AbstractContentPage
	{
		public MailBox()
		{
			this.Title = "Почта";
			this.Folder = C.Folders.Inbox;
		}

		#region System properties
		
		public override string TemplateUrl {
            get { return string.Concat(
					"~/Messaging/UI/Views/",
                    (this.Action != ActionEnum.List && 
                     this.Action != ActionEnum.Delete && 
                     this.Action != ActionEnum.Restore &&
                     this.Action != ActionEnum.Destroy ? "NewMessage" : "MailBox"),
					".aspx");
			}
		}

		public override string IconUrl {
			get {
				this.Validate();
				return string.Concat(
					"~/Messaging/UI/Images/",
					this.IsValid ? "email.png" : "email_error.png");
			}
		}

		#endregion System properties

		#region Lms properties
		
		[EditableLink("Message Store", 1,
			HelpTitle = "Select an item, which contains all messages.",
			Required = true)]
        public MessageStore MessageStore
        {
            get { return this.GetDetail("MessageStore") as MessageStore; }
            set { this.SetDetail<MessageStore>("MessageStore", value); }
        }

		public Message EditedItem { get; protected set; }

		#endregion Lms properties
	}
}
