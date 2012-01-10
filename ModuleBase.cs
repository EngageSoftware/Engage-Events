// <copyright file="ModuleBase.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.WebControls;

    using Engage.Dnn.Events.Components;
    using Engage.Events;

#if TRIAL
    using Engage.Licensing;
#endif

    /// <summary>
    /// This class extends the framework version in order for developers to add on any specific methods/behavior.
    /// </summary>
    public class ModuleBase : Framework.ModuleBase, IActionable
    {
        /// <summary>
        /// Backing field for <see cref="CategoryIds"/>
        /// </summary>
        private IEnumerable<int> categoryIds;

        /// <summary>
        /// Backing field for <see cref="PermissionsService"/>
        /// </summary>
        private PermissionsService permissionsService;

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
                var actions = new ModuleActionCollection();

                if (this.PermissionsService.CanManageEvents)
                {
                    actions.Add(
                        new ModuleAction(this.GetNextActionID())
                            {
                                Title = this.Localize("Add Event.Action", this.LocalSharedResourceFile),
                                CommandName = ModuleActionType.AddContent,
                                Url = this.BuildLinkUrl(this.ModuleId, "EventEdit"),
                                Secure = SecurityAccessLevel.View,
                                Icon = "add.gif"
                            });
                    actions.Add(
                        new ModuleAction(this.GetNextActionID())
                            {
                                Title = this.Localize("Manage Events.Action", this.LocalSharedResourceFile),
                                CommandName = ModuleActionType.EditContent,
                                Url = this.BuildLinkUrl(this.ModuleId, "EventListingAdmin"),
                                Secure = SecurityAccessLevel.View
                            });
                }

                if (this.PermissionsService.CanManageCategories)
                {
                    actions.Add(
                        new ModuleAction(this.GetNextActionID())
                            {
                                Title = this.Localize("Manage Categories.Action", this.LocalSharedResourceFile),
                                CommandName = ModuleActionType.EditContent,
                                Url = this.BuildLinkUrl(this.ModuleId, "ManageCategories"),
                                Secure = SecurityAccessLevel.View
                            });
                }

                if (this.PermissionsService.CanViewResponses)
                {
                    actions.Add(
                        new ModuleAction(this.GetNextActionID())
                            {
                                Title = this.Localize("View Responses.Action", this.LocalSharedResourceFile),
                                CommandName = ModuleActionType.EditContent,
                                Url = this.BuildLinkUrl(this.ModuleId, "ResponseSummary"),
                                Secure = SecurityAccessLevel.View,
                                Icon = "view.gif"
                            });
                }

                if (this.PermissionsService.CanManageDisplay)
                {
                    actions.Add(
                        new ModuleAction(this.GetNextActionID())
                            {
                                Title = this.Localize("Manage Display.Action", this.LocalSharedResourceFile),
                                CommandName = ModuleActionType.ModuleSettings,
                                Url = this.BuildLinkUrl(this.ModuleId, "ChooseDisplay"),
                                Secure = SecurityAccessLevel.View,
                                Icon = "icon_skins_16px.gif"
                            });
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
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Backwards compatibility")]
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
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Backwards compatability")]
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
        /// Gets the list of category IDs by which this display should filter its events list.
        /// </summary>
        /// <value>The category IDs.</value>
        protected IEnumerable<int> CategoryIds 
        { 
            get 
            {
                if (this.categoryIds == null)
                {
                    var categoriesSettingValue = ModuleSettings.Categories.GetValueAsStringFor(this);
                    this.categoryIds = string.IsNullOrEmpty(categoriesSettingValue)
                                           ? Enumerable.Empty<int>()
                                           : categoriesSettingValue.Split(',').Select(id => int.Parse(id, CultureInfo.InvariantCulture));
                }

                return this.categoryIds;
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
            get { return ModuleSettings.FeaturedOnly.GetValueAsBooleanFor(this).Value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance should show events which have hit their registration cap.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance should not display events which have hit their registration cap; otherwise, <c>false</c>.
        /// </value>
        protected bool HideFullEvents
        {
            get { return ModuleSettings.HideFullEvents.GetValueAsBooleanFor(this).Value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is configured.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is configured; otherwise, <c>false</c>.
        /// </value>
        protected override bool IsConfigured
        {
            get
            {
                return "CALENDAR".Equals(ModuleSettings.DisplayType.GetValueAsStringFor(this), StringComparison.OrdinalIgnoreCase) ||
                       (Engage.Utility.HasValue(ModuleSettings.Template.GetValueAsStringFor(this)) &&
                        Engage.Utility.HasValue(ModuleSettings.SingleItemTemplate.GetValueAsStringFor(this)));
            }
        }

        /// <summary>
        /// Gets the index of the current page from the QueryString.
        /// </summary>
        /// <value>The index of the current page.</value>
        protected virtual int CurrentPageIndex
        {
            get
            {
                int index;
                if (!int.TryParse(this.Request.QueryString[this.PageIndexQueryStringKey], NumberStyles.Integer, CultureInfo.InvariantCulture, out index))
                {
                    index = 1;
                }

                return index;
            }
        }

        /// <summary>
        /// Gets the query-string key for the index of the current page.
        /// </summary>
        protected string PageIndexQueryStringKey
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, this.Localize("PageIndexKey.Format", this.LocalSharedResourceFile), this.ModuleId);
            }
        }

        /// <summary>
        /// Gets the <see cref="Components.PermissionsService"/>.
        /// </summary>
        /// <value>The permissions service.</value>
        protected PermissionsService PermissionsService
        {
            get
            {
                if (this.permissionsService == null)
                {
                    if (this.ModuleConfiguration == null)
                    {
                        this.SetModuleConfiguration();
                    }

                    this.permissionsService = new PermissionsService(this.ModuleConfiguration);
                }

                return this.permissionsService;
            }
        }

        /// <summary>
        /// Gets the Tab ID to use when displaying module details.
        /// </summary>
        /// <value>The Tab ID to use when displaying module details.</value>
        protected int DetailsTabId
        {
            get { return ModuleSettings.DetailsDisplayTabId.GetValueAsInt32For(this) ?? this.TabId; }
        }

        /// <summary>
        /// Gets the Module ID to use when displaying module details.
        /// </summary>
        /// <value>The Module ID to use when displaying module details.</value>
        protected int DetailsModuleId
        {
            get { return ModuleSettings.DetailsDisplayModuleId.GetValueAsInt32For(this) ?? this.ModuleId; }
        }

        /// <summary>
        /// Sends an <c>iCalendar</c> to the client to download.
        /// </summary>
        /// <param name="response">The response to use to send the <c>iCalendar</c>.</param>
        /// <param name="content">The content of the <c>iCalendar</c>.</param>
        /// <param name="name">The name of the file.</param>
        protected static void SendICalendarToClient(HttpResponse response, string content, string name)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

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
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (queryStringKeys == null)
            {
                throw new ArgumentNullException("queryStringKeys");
            }

            var queryString = new StringBuilder(64);
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
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
#if TRIAL
            this.LicenseProvider = new TrialLicenseProvider(FeaturesController.ModuleLicenseKey);
#endif

            base.OnInit(e);

            this.SetModuleConfiguration();
            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + Localization.LocalResourceDirectory + "/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);
        }

        /// <summary>
        /// Determines whether this instance of the module can display the given event (based on the Categories setting).
        /// </summary>
        /// <param name="event">The event to check.</param>
        /// <returns>
        /// <c>true</c> if this instance can show the event; otherwise, <c>false</c>.
        /// </returns>
        protected bool CanShowEvent(Event @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException("event");
            }
            
            return !this.CategoryIds.Any() || this.CategoryIds.Contains(@event.CategoryId);
        }

        /// <summary>
        /// Clears the cached value for <see cref="CategoryIds"/>.  This should be called when the setting is updated and needs to be read in the same request.
        /// </summary>
        protected void ClearCategoryIdsCache()
        {
            this.categoryIds = null;
        }

        /// <summary>
        /// Denies the user access to this control, showing them an "Access Denied" message is they're logged in, or the login page if they aren't.
        /// </summary>
        protected void DenyAccess()
        {
            this.Response.Redirect(IsLoggedIn ? Globals.NavigateURL("Access Denied") : Dnn.Utility.GetLoginUrl(this.PortalSettings, this.Request));
        }

        /// <summary>
        /// Sets the <see cref="PortalModuleBase.ModuleConfiguration"/> for controls not loaded by DNN (i.e. when it's <c>null</c>), 
        /// getting it from the parent control
        /// </summary>
        protected void SetModuleConfiguration()
        {
            PortalModuleBase parentControl = this;
            while (this.ModuleConfiguration == null)
            {
                parentControl = Engage.Utility.FindParentControl<PortalModuleBase>(parentControl);
                if (parentControl == null)
                {
                    break;
                }

                this.ModuleConfiguration = parentControl.ModuleConfiguration;
            }
        }

        /// <summary>
        /// Sets up a DNN <see cref="PagingControl"/>.
        /// </summary>
        /// <param name="pagingControl">The pager control.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <param name="queryStringKeys">The QueryString keys which should be persisted when the paging links are clicked.</param>
        protected void SetupPagingControl(PagingControl pagingControl, int totalRecords, params string[] queryStringKeys)
        {
            if (pagingControl == null)
            {
                throw new ArgumentNullException("pagingControl");
            }

            pagingControl.Visible = totalRecords != 0;
            pagingControl.TotalRecords = totalRecords;
            pagingControl.CurrentPage = this.CurrentPageIndex;
            pagingControl.TabID = this.TabId;
            pagingControl.QuerystringParams = GenerateQueryStringParameters(this.Request, queryStringKeys);
        }

        /// <summary>
        /// Gets the name of the default category.
        /// </summary>
        /// <returns>The default category's display name</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does not represent object state")]
        protected string GetDefaultCategoryName()
        {
            return this.Localize("DefaultCategory.Text", this.LocalSharedResourceFile);
        }
    }
}
