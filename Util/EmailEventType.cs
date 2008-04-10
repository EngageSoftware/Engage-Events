using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Engage.Events.Util
{
    /// <summary>
    /// Summary description for RoutingEventType.
    /// </summary>
    [Serializable]
    public class EmailEventType: Engage.Routing.RoutingEventType
    {

        public static EmailEventType Invitation = new EmailEventType("Invitation");
        public static EmailEventType Reminder = new EmailEventType("Reminder");
        public static EmailEventType Recap = new EmailEventType("Recap");

        public EmailEventType(string description)
            : base(description)
        {
        }

    }
}
