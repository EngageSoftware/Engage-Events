// <copyright file="ModuleBase.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Localization;
    using Utility=Engage.Utility;

    /// <summary>
    /// This class extends the framework version in order for developers to add on any specific methods/behavior.
    /// </summary>
    public class ModuleBase : Framework.ModuleBase, IActionable
    {
        /// <summary>
        /// This method looks at the query string and the currently logged in user (if any) and checks for
        /// an existing RSVP(registration) for the user.
        /// </summary>
        internal static bool IsRegistered
        {
            get
            {
                bool registered = false;
                if (EventId > 0 && IsLoggedIn)
                {
                    Engage.Events.Rsvp rsvp = Engage.Events.Rsvp.Load(EventId, DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo().Email);
                    registered = (rsvp != null);
                }
                return registered;
            }
        }

        /// <summary>
        /// Gets the register URL.
        /// </summary>
        /// <value>The register URL.</value>
        protected string RegisterUrl
        {
            get
            {
                return
                    this.BuildLinkUrl(
                        "&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Register&eventid="
                        + EventId.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets the RSVP URL.
        /// </summary>
        /// <value>The RSVP URL.</value>
        protected string RsvpUrl
        {
            get
            {
                return
                    this.BuildLinkUrl(
                        "&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Rsvp&eventid="
                        + EventId.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        protected static int EventId
        {
            get
            {
                int id = -1;
                if (HttpContext.Current.Request.QueryString["eventId"] != null)
                {
                    id = Convert.ToInt32(HttpContext.Current.Request.QueryString["eventId"]);
                }

                return id;
            }
        }

        protected override bool IsConfigured
        {
            get { return Utility.HasValue(Dnn.Utility.GetStringSetting(this.Settings, Framework.Setting.DisplayTemplate.PropertyName)); }
        }

        #region IActionable Members

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();

                //Add OnLine Help Action
                string helpUrl = this.GetOnLineHelp(this.ModuleConfiguration.HelpUrl, this.ModuleConfiguration);
                if (helpUrl != null)
                {
                    ModuleAction helpAction = new ModuleAction(this.GetNextActionID());
                    helpAction.Title = Localization.GetString(ModuleActionType.OnlineHelp, Localization.GlobalResourceFile);
                    helpAction.CommandName = ModuleActionType.OnlineHelp;
                    helpAction.CommandArgument = string.Empty;
                    helpAction.Icon = "action_help.gif";
                    helpAction.Url = Globals.FormatHelpUrl(helpUrl, this.PortalSettings, this.ModuleConfiguration.FriendlyName);
                    helpAction.Secure = SecurityAccessLevel.Edit;
                    helpAction.UseActionEvent = true;
                    helpAction.Visible = true;
                    helpAction.NewWindow = true;
                    actions.Add(helpAction);
                }
                return actions;
            }
        }

        #endregion

        /// <summary>
        /// Sends an iCalendar to the client to download.
        /// </summary>
        /// <param name="response">The response to use to send the iCalendar.</param>
        /// <param name="content">The content of the iCalendar.</param>
        /// <param name="name">The name of the file.</param>
        protected static void SendICalendarToClient(HttpResponse response, string content, string name)
        {
            response.ClearContent();

            // Stream The ICalendar 
            response.ContentEncoding = Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.ANSICodePage);
            response.BufferOutput = true;
            response.ContentType = "text/calendar";
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.AppendHeader("Content-Class", "urn:content-classes:calendarmessage");
            response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(name) + ".ics");

            ////Engage.Logging.FileNotifier fn = new Engage.Logging.FileNotifier();
            ////fn.Notify("Ical", "", content);

            response.Write(content);

            response.Flush();
        }

        public string GetOnLineHelp(string helpUrl, ModuleInfo moduleConfig)
        {
            return (HostSettings.GetHostSetting("EnableModuleOnLineHelp") != "Y") ? string.Empty : helpUrl;
        }
    }
}