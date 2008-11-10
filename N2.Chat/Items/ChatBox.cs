﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using N2.Details;
using N2.Integrity;
using N2.Templates;
using N2.Templates.Items;

namespace N2.Templates.Chat.Items
{
    [Definition("ChatBox", "ChatBox", "Общение онлайн.", "", 160)]
    [WithEditableTitle("Title", 10, Required = false)]
    [AllowedZones(Zones.RecursiveAbove, Zones.RecursiveBelow, Zones.SiteTop, Zones.Content)]
    [RestrictParents(typeof(IStructuralPage))]
    public class ChatBox : SidebarItem
    {
        #region N2.Properties

        public override string TemplateUrl
        {
            get
            {
                return "~/Chat/UI/Parts/ChatBox.ascx";
            }
        }

        public override string IconUrl
        {
            get
            {
                return "~/Chat/UI/Images/chat.png";
            }
        }

        #endregion
    }
}