//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using Engage.Dnn.Events.Util;
using Engage.Events;

namespace Engage.Dnn.Events
{
    /// <summary>
    /// Summary description for ModuleBase.
    /// </summary>
    public class ModuleBase : PortalModuleBase
    {
        private bool allowTitleUpdate = true;
        private bool logBreadCrumb = true;
        private bool useCache = true;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }            
        }

        public bool IsSetup
        {
            get
            {
                string s = HostSettings.GetHostSetting(Engage.Dnn.Events.Util.Utility.ModuleConfigured + PortalId);
                return !String.IsNullOrEmpty(s);
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (Request.IsAuthenticated == false)
                {
                    return false;
                }
                else
                {
                    return PortalSecurity.IsInRole(HostSettings.GetHostSetting(Engage.Dnn.Events.Util.Utility.AdminRole + PortalId));
                }
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return (Request.IsAuthenticated == true);
            }
        }

        public bool IsHostMailConfigured
        {
            get
            {
                string s = HostSettings.GetHostSetting("SMTPServer");
                return Engage.Utility.HasValue(s);
            }
        }

        protected int EventId
        {
            get
            {
                int id = -1;
                //Get the currentpage index from the url parameter
                if (Request.QueryString["eventId"] != null)
                {
                    id = Convert.ToInt32(Request.QueryString["eventId"]);
                }

                return id;
            }
        }

        //public bool IsCommentsEnabled
        //{
        //    get
        //    {
        //        return IsCommentsEnabledForPortal(PortalId);
        //    }
        //}

        //public static bool IsCommentsEnabledForPortal(int portalId)
        //{
        //    string s = HostSettings.GetHostSetting(Utility.PublishComment + portalId.ToString(CultureInfo.InvariantCulture));
        //    if (Utility.HasValue(s))
        //    {
        //        return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        //    }
        //    return false;
        //}


        //public bool AreCommentsModerated
        //{
        //    get
        //    {
        //        return AreCommentsModeratedForPortal(PortalId);
        //    }
        //}

        //public static bool AreCommentsModeratedForPortal(int portalId)
        //{
        //    string s = HostSettings.GetHostSetting(Utility.PublishCommentApproval + portalId.ToString(CultureInfo.InvariantCulture));
        //    if (Utility.HasValue(s))
        //    {
        //        return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        //    }
        //    return true;
        //}

        //public bool AutoApproveComments
        //{
        //    get
        //    {
        //        return AutoApproveCommentsForPortal(PortalId);
        //    }
        //}

        //public static bool AutoApproveCommentsForPortal(int portalId)
        //{
        //    string s = HostSettings.GetHostSetting(Utility.PublishCommentAutoApprove + portalId.ToString(CultureInfo.InvariantCulture));
        //    if (Utility.HasValue(s))
        //    {
        //        return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        //    }
        //    return false;
        //}

        //public bool AreRatingsEnabled
        //{
        //    get
        //    {
        //        return AreRatingsEnabledForPortal(PortalId);
        //    }
        //}

        //public static bool AreRatingsEnabledForPortal(int portalId)
        //{
        //    string s = HostSettings.GetHostSetting(Utility.PublishRating + portalId.ToString(CultureInfo.InvariantCulture));
        //    if (Utility.HasValue(s))
        //    {
        //        return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        //    }
        //    return false;
        //}

        //public bool AllowAnonymousRatings
        //{
        //    get
        //    {
        //        return AllowAnonymousRatingsForPortal(PortalId);
        //    }
        //}

        //public static bool AllowAnonymousRatingsForPortal(int portalId)
        //{
        //    string s = HostSettings.GetHostSetting(Utility.PublishRatingAnonymous + portalId.ToString(CultureInfo.InvariantCulture));
        //    if (Utility.HasValue(s))
        //    {
        //        return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        //    }
        //    return false;
        //}

     
        //public bool EnablePublishFriendlyUrls
        //{
        //    get
        //    {
        //        return EnablePublishFriendlyUrlsForPortal(PortalId);
        //    }
        //}

        //public static bool EnablePublishFriendlyUrlsForPortal(int portalId)
        //{
        //    string s = HostSettings.GetHostSetting(Utility.PublishEnablePublishFriendlyUrls + portalId.ToString(CultureInfo.InvariantCulture));
        //    if (Utility.HasValue(s))
        //    {
        //        return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        //    }
        //    return true;
        //}

        //public bool IsAdmin
        //{
        //    get
        //    {
        //        if (Request.IsAuthenticated == false)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + PortalId));
        //        }
        //    }
        //}

        //public bool IsConfigured
        //{
        //    get
        //    {
        //        return this.Settings.Contains("DisplayType");
        //    }
        //}

        //public static bool IsUserAdmin(int portalId)
        //{
        //    if (HttpContext.Current.Request.IsAuthenticated == false)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + portalId));
        //    }
        //}

        //public bool IsAuthor
        //{
        //    get
        //    {
        //        if (Request.IsAuthenticated == false)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAuthorRole + PortalId));
        //        }
        //    }
        //}

        //protected void AddBreadCrumb(string pageName)
        //{
        //    BreadCrumb.Add(pageName, GetItemLinkUrl(ItemId));
        //}

        public bool UseCache
        {
            get {
                return this.useCache && CacheTime > 0;
            }
            set { this.useCache = value; }
        }

        public bool AllowTitleUpdate
        {
            get
            {
                object o = Settings["AllowTitleUpdate"];
                if (o == null || !bool.TryParse(o.ToString(), out this.allowTitleUpdate))
                {
                    this.allowTitleUpdate = true;
                }
                return this.allowTitleUpdate;
            }
            set
            {
                this.allowTitleUpdate = value;
            }
        }

        //This is the cachetime used by Publish modules
        public int CacheTime
        {
            get
            {
                object o = Settings["CacheTime"];
                if (o != null)
                {
                    return Convert.ToInt32(o.ToString());
                }
                else if (GetDefaultCacheSetting(PortalId) > 0)
                {
                    return GetDefaultCacheSetting(PortalId);
                }
                return 0;
            }
        }


        public static int GetDefaultCacheSetting(int portalId)
        {
            string s = HostSettings.GetHostSetting(Engage.Dnn.Events.Util.Utility.CacheTime + portalId);
            if (Engage.Utility.HasValue(s))
            {
                return Convert.ToInt32(s);
            }
            else
            {
                return 0;
            }
        }

        protected void SendICalendarToClient(string content, string name)
        {
            HttpContext.Current.Response.ClearContent();

            //Stream The ICalendar 
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.ANSICodePage);
            HttpContext.Current.Response.BufferOutput = true;
            HttpContext.Current.Response.ContentType = "text/calendar";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.AppendHeader("Content-Class", "urn:content-classes:calendarmessage");
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(name) + ".ics");
            
            //Engage.Logging.FileNotifier fn = new Engage.Logging.FileNotifier();
            //fn.Notify("Ical", "", content);
                        
            HttpContext.Current.Response.Write(content);

            HttpContext.Current.Response.Flush();


        }

        
        public static string ApplicationUrl
        {
            get
            {
                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Request.ApplicationPath;
                }
            }
        }

        public bool LogBreadCrumb
        {
            get { return this.logBreadCrumb; }
            set { this.logBreadCrumb = value; }
        }

        public string BuildLinkUrl(string qsParameters)
        {
            return DotNetNuke.Common.Globals.NavigateURL(TabId, "", qsParameters);
        }

        public static string DesktopModuleFolderName
        {
            get
            {
                return Engage.Dnn.Events.Util.Utility.DesktopModuleFolderName;
            }
        }

        //public static string GetRssLinkUrl(object itemId, int maxDisplayItems, int itemTypeId, int portalId, string displayType)
        //{
        //    StringBuilder url = new StringBuilder(128);

        //    url.Append(ApplicationUrl);
        //    url.Append(DesktopModuleFolderName);
        //    url.Append("eprss.aspx?itemId=");
        //    url.Append(itemId);
        //    url.Append("&numberOfItems=");
        //    url.Append(maxDisplayItems);
        //    url.Append("&itemtypeid=");
        //    url.Append(itemTypeId);
        //    url.Append("&portalid=");
        //    url.Append(portalId);
        //    url.Append("&DisplayType=");
        //    url.Append(displayType);

        //    return url.ToString();
        //}

        //public static string GetPrintFriendlyLinkUrl(object itemId, int portalId)
        //{
        //    return ApplicationUrl + DesktopModuleFolderName + "printerfriendly.aspx?itemId=" + itemId + "&PortalId=" + portalId.ToString(CultureInfo.InvariantCulture);
        //}

        protected string GetEditUrl(string eventId)
        {
            return EditUrl("eventId", eventId, "Edit");
        }

        protected bool HasInviteUrl(object invitationUrl)
        {
            return (invitationUrl.ToString().Length > 0);
        }

        //So far, we are only using the DataGrid and a Repeater. This method pulls it out accordingly
        //to prevent the same code in subclasses over and over.hk
        protected int GetId(object sender)
        {
            LinkButton button = (LinkButton)sender;

            RepeaterItem repeater = button.NamingContainer as RepeaterItem;
            if (repeater != null)
            {
                Label l = (Label)repeater.FindControl("lblId");
                return Convert.ToInt32(l.Text);
            }
            else
            {
                DataGridItem gridItem = button.NamingContainer as DataGridItem;
                if (gridItem != null)
                {
                    return Convert.ToInt32(gridItem.Cells[0].Text);
                }
                else
                {
                    return -1;
                }
            }
        }

        #region Event Specific Navigation Handlers


        protected void lbEditEvent_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit&eventId=" + eventId.ToString());

            Response.Redirect(href, true);
        }

        protected void lbDeleteEvent_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            Event.Delete(eventId);
        }

        //this can be overridden because a Datagrid is used instead of a Repeater so GetId(..) won't work.
        protected void lbEditEmail_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EmailEdit&eventid=" + eventId.ToString());

            Response.Redirect(href, true);
        }

        protected void lbViewRsvp_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=RsvpDetail&eventid=" + eventId.ToString());

            Response.Redirect(href, true);
        }

        protected void lbRsvp_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);

            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Register&eventid=" + eventId.ToString());
            Response.Redirect(href, true);
        }
        protected void lnkAddToCalendar_OnClick(object sender, EventArgs e)
        {
            //LinkButton button = (LinkButton)sender;
            //DataGridItem item = (DataGridItem)button.NamingContainer;

            //int eventId = Convert.ToInt32(item.Cells[0].Text);
            int eventId = GetId(sender);
            Event ee = Event.Load(eventId);

            //Stream The vCalendar 
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.ANSICodePage);
            HttpContext.Current.Response.ContentType = "text/x-iCalendar";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "filename=" + HttpUtility.UrlEncode(ee.Title) + ".vcs");
            HttpContext.Current.Response.Write(ee.ToICal("hkenuam@engagesoftware.com"));
        }

        #endregion
    }

}

