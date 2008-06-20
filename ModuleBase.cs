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
    using DotNetNuke.Entities.Modules;
    using Util;

    /// <summary>
    /// The base class for all controls in the Engage: Events module.
    /// </summary>
    public class ModuleBase : PortalModuleBase
    {
        /////// <summary>
        /////// The backing field for <see cref="UseCache"/>.
        /////// </summary>
        ////private bool useCache = true;

        /// <summary>
        /// Gets the application URL.
        /// </summary>
        /// <value>The application URL.</value>
        public static string ApplicationUrl
        {
            get 
            {
                return HttpContext.Current.Request.ApplicationPath != "/" ? HttpContext.Current.Request.ApplicationPath : string.Empty;
            }
        }

        /// <summary>
        /// Gets the name of the desktop module folder for this module.
        /// </summary>
        /// <value>The name of the desktop module folder for this module.</value>
        public static string DesktopModuleFolderName
        {
            get
            {
                return Utility.DesktopModuleFolderName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current user has edit rights to this module.
        /// </summary>
        /// <value><c>true</c> if the current user can edit the module; otherwise, <c>false</c>.</value>
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
                    return IsEditable;

                    // Later we can add checks to control access by custom roles. hk
                    ////return PortalSecurity.IsInRole(HostSettings.GetHostSetting(Engage.Dnn.Events.Util.Utility.AdminRole + PortalId));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current user is logged in.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the current user is logged in; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoggedIn
        {
            get
            {
                return this.Request.IsAuthenticated;
            }
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        protected int EventId
        {
            get
            {
                int id = -1;
                if (Request.QueryString["eventId"] != null)
                {
                    id = Convert.ToInt32(Request.QueryString["eventId"]);
                }

                return id;
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
                return this.BuildLinkUrl("&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Register&eventid=" + this.EventId.ToString(CultureInfo.InvariantCulture));
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
                return this.BuildLinkUrl("&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Rsvp&eventid=" + this.EventId.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Builds a URL for this TabId, using the given querystring parameters.
        /// </summary>
        /// <param name="querystringParameters">The qs parameters.</param>
        /// <returns>A URL to the current TabId, with the given querystring parameters</returns>
        public string BuildLinkUrl(string querystringParameters)
        {
            return DotNetNuke.Common.Globals.NavigateURL(this.TabId, "", querystringParameters);
        }

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

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }            
        }
    }
}

