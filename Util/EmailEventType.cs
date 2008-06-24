using System;

namespace Engage.Events.Util
{
    /// <summary>
    /// Summary description for RoutingEventType.
    /// </summary>
    [Serializable]
    public class EmailEventType: Routing.RoutingEventType
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
