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

    /// <summary>
    /// This class extends the framework version in order for developers to add on any specific methods/behavior.
    /// </summary>
    public class ModuleBase : Framework.ModuleBase
    {

        /// <summary>
        /// This method looks at the query string and the currently logged in user (if any) and checks for
        /// an existing RSVP(registration) for the user.
        /// </summary>
        protected bool IsRegistered
        {
            get
            {
                bool registered = false;
                if (this.EventId > 0 && this.IsLoggedIn)
                {
                    Engage.Events.Rsvp rsvp = Engage.Events.Rsvp.Load(this.EventId, this.UserInfo.Email);
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
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        protected int EventId
        {
            get
            {
                int id = -1;
                if (this.Request.QueryString["eventId"] != null)
                {
                    id = Convert.ToInt32(this.Request.QueryString["eventId"]);
                }

                return id;
            }
        }

        protected override bool IsConfigured
        {
            get
            {
                string setting = Dnn.Utility.GetStringSetting(Settings, Setting.DisplayTemplate.PropertyName);

                return (setting.Length > 0);
            }
        }
    }
}