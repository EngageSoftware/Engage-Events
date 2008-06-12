using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Engage.Dnn.Events
{
    public class Setting
    {
        
        public static readonly Setting PrinterFriendly = new Setting("pnlPrinterFriendly",  "Hide/Show the printer friendly link on the module");
        public static readonly Setting EmailAFriend = new Setting("pnlEmailAFriend", "Hide/Show the Email a Friend link on the module");
        public static readonly Setting PrivacyPolicyUrl = new Setting("upnlRating", "Specify the URL for your Privacy Policy.");
        public static readonly Setting UnsubscribeUrl = new Setting("unsubscribeUrl","Specify the URL for unsubscribing.");
        public static readonly Setting OpenLinkUrl = new Setting("openLinkUrl", "Specify the URL for your Open Link to track opens.");
        public static readonly Setting DisplayType = new Setting("DisplayType", "The display that is used for unauthenticated users.");
        public static readonly Setting ReplacementMessage = new Setting("replacementMessage", "You can include an entire section of replaceable text in your message using this setting.");
        public static readonly Setting SkinSelection = new Setting("SkinSelection",  "The skin used for Calendar Display.");
       
        private Setting( string propertyName, string description)
        {
            this._propertyName = propertyName;
            this._description = description;
        }

        #region Public Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyName = string.Empty;
        public string PropertyName
        {
            [DebuggerStepThrough]
            get { return _propertyName; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyValue = string.Empty;
        public string PropertyValue
        {
            [DebuggerStepThrough]
            get { return _propertyValue; }
            [DebuggerStepThrough]
            set { _propertyValue = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _description = string.Empty;
        public string Description
        {
            [DebuggerStepThrough]
            get { return _description; }
            [DebuggerStepThrough]
            set { _description = value; }
        }

        #endregion

        public static List<Setting> GetList(Type ct)
        {
            if (ct == null) throw new ArgumentNullException("ct");

            List<Setting> settings = new List<Setting>();

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    Object o = f.GetValue(type);
                    if (o is Setting)
                    {
                        settings.Add((Setting)o);
                    }
                }

                type = type.BaseType; //check the super type 
            }

            return settings;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(128);

            builder.Append("Property Name: ");
            builder.Append(_propertyName);
            builder.Append(" Property Value: ");
            builder.Append(_propertyValue);

            return builder.ToString();
        }

    }
}
