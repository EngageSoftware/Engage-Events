// <copyright file="ModuleBase.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2009
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
    using System.IO;
    using System.Text;
    using System.Web;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.WebControls;

    /// <summary>
    /// This class extends the framework version in order for developers to add on any specific methods/behavior.
    /// </summary>
    public class ModuleBase : Framework.ModuleBase, IActionable
    {
        /// <summary>
        /// Gets the name of the this module's desktop module record in DNN.
        /// </summary>
        /// <value>The name of this module's desktop module record in DNN.</value>
        public override string DesktopModuleName
        {
            get { return Utility.DesktopModuleName; }
        }

        /// <summary>
        /// Gets the module actions.
        /// </summary>
        /// <value>The module actions.</value>
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();

                if (HostSettings.GetHostSetting("EnableModuleOnLineHelp") == "Y" && Engage.Utility.HasValue(this.ModuleConfiguration.HelpUrl))
                {
                    ModuleAction helpAction = new ModuleAction(this.GetNextActionID());
                    helpAction.Title = Localization.GetString(ModuleActionType.OnlineHelp, Localization.GlobalResourceFile);
                    helpAction.CommandName = ModuleActionType.OnlineHelp;
                    helpAction.CommandArgument = string.Empty;
                    helpAction.Icon = "action_help.gif";
                    helpAction.Url = Globals.FormatHelpUrl(this.ModuleConfiguration.HelpUrl, this.PortalSettings, this.ModuleConfiguration.FriendlyName);
                    helpAction.Secure = SecurityAccessLevel.Edit;
                    helpAction.UseActionEvent = true;
                    helpAction.Visible = true;
                    helpAction.NewWindow = true;
                    actions.Add(helpAction);
                }

                return actions;
            }
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        protected int? EventId
        {
            get
            {
                if (this.Request.QueryString["eventId"] != null)
                {
                    int eventId;
                    if (int.TryParse(this.Request.QueryString["eventId"], NumberStyles.Integer, CultureInfo.InvariantCulture, out eventId))
                    {
                        return eventId;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the occurrence date.
        /// </summary>
        /// <value>The date when the event occurs.</value>
        /// <exception cref="InvalidOperationException">EventStart is not present on QueryString</exception>
        protected DateTime EventStart
        {
            get
            {
                if (this.Request.QueryString["start"] != null)
                {
                    long startTicks;
                    if (long.TryParse(this.Request.QueryString["start"], NumberStyles.Integer, CultureInfo.InvariantCulture, out startTicks))
                    {
                        return new DateTime(startTicks);
                    }
                }

                throw new InvalidOperationException("EventStart is not present on QueryString: " + this.Request.RawUrl);
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
                int? eventId = this.EventId;
                if (eventId.HasValue)
                {
                    return this.BuildLinkUrl(this.ModuleId, "Register", Utility.GetEventParameters(eventId.Value, this.EventStart));
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the Response URL.
        /// </summary>
        /// <value>The Response URL.</value>
        protected string ResponseUrl
        {
            get
            {
                int? eventId = this.EventId;
                if (eventId.HasValue)
                {
                    return this.BuildLinkUrl(this.ModuleId, "Response", Utility.GetEventParameters(eventId.Value, this.EventStart));
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance should display only featured events.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance should only display featured events; otherwise, <c>false</c>.
        /// </value>
        protected bool IsFeatured
        {
            get { return Dnn.Utility.GetBoolSetting(this.Settings, "FeaturedOnly", false); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsConfigured
        {
            get { return Engage.Utility.HasValue(Dnn.Utility.GetStringSetting(this.Settings, "Template")) && Engage.Utility.HasValue(Dnn.Utility.GetStringSetting(this.Settings, "SingleItemTemplate")); }
        }

        /// <summary>
        /// Gets the index of the current page from the QueryString.
        /// </summary>
        /// <value>The index of the current page.</value>
        protected int CurrentPageIndex
        {
            get
            {
                int index;
                if (!int.TryParse(this.Request.QueryString["currentPage"], NumberStyles.Integer, CultureInfo.InvariantCulture, out index))
                {
                    index = 1;
                }

                return index;
            }
        }

        /// <summary>
        /// Sends an <c>iCalendar</c> to the client to download.
        /// </summary>
        /// <param name="response">The response to use to send the <c>iCalendar</c>.</param>
        /// <param name="content">The content of the <c>iCalendar</c>.</param>
        /// <param name="name">The name of the file.</param>
        protected static void SendICalendarToClient(HttpResponse response, string content, string name)
        {
            response.Clear();

            // Stream The ICalendar 
            response.Buffer = true;

            response.ContentType = "text/calendar";
            response.ContentEncoding = Encoding.UTF8;
            response.Charset = "utf-8";

            response.AddHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(name) + ".ics" + "\"");

            response.Write(content);
            response.End();
        }

        /// <summary>
        /// Generates a list of QueryString parameters for the given list of <paramref name="queryStringKeys"/>.
        /// </summary>
        /// <param name="request">The current request.</param>
        /// <param name="queryStringKeys">The keys for which to generate parameters.</param>
        /// <returns>
        /// A list of QueryString parameters for the given list of <paramref name="queryStringKeys"/>
        /// </returns>
        protected static string GenerateQueryStringParameters(HttpRequest request, params string[] queryStringKeys)
        {
            StringBuilder queryString = new StringBuilder(64);
            foreach (string key in queryStringKeys)
            {
                if (Engage.Utility.HasValue(request.QueryString[key]))
                {
                    if (queryString.Length > 0)
                    {
                        queryString.Append("&");
                    }

                    queryString.Append(key).Append("=").Append(request.QueryString[key]);
                }
            }

            return queryString.ToString();
        }

        /// <summary>
        /// Sets up a DNN <see cref="PagingControl"/>.
        /// </summary>
        /// <param name="pagingControl">The pager control.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <param name="queryStringKeys">The QueryString keys which should be persisted when the paging links are clicked.</param>
        protected void SetupPagingControl(PagingControl pagingControl, int totalRecords, params string[] queryStringKeys)
        {
            pagingControl.Visible = totalRecords != 0;
            pagingControl.TotalRecords = totalRecords;
            pagingControl.CurrentPage = this.CurrentPageIndex;
            pagingControl.TabID = this.TabId;
            pagingControl.QuerystringParams = GenerateQueryStringParameters(this.Request, queryStringKeys);
        }

        /// <summary>
        /// Gets localized text for the given resource key using this control's <see cref="DotNetNuke.Entities.Modules.PortalModuleBase.LocalResourceFile"/>.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>Localized text for the given resource key</returns>
        protected string Localize(string resourceKey)
        {
            return Localization.GetString(resourceKey, this.LocalResourceFile);
        }

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + Localization.LocalResourceDirectory + "/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);
        }
    }
}
