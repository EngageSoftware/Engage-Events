// <copyright file="EventListingItem.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Display
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using DotNetNuke.Common;

    using Engage.Dnn.Events.Components;
    using Engage.Events;
    using Framework.Templating;
    using Templating;

    using Utility = Engage.Dnn.Events.Utility;

    /// <summary>
    /// Custom event listing item
    /// </summary>
    public partial class EventListingItem : TemplatedDisplayModuleBase
    {
        /// <summary>
        /// Backing field for <see cref="FilterCategoryIds"/>
        /// </summary>
        private IEnumerable<int> filterCategoryId;

        /// <summary>
        /// Gets or sets the sort action control.
        /// </summary>
        /// <value>The sort action control.</value>
        public SortAction SortAction { get; set; }

        /// <summary>
        /// Gets or sets the status filter action control.
        /// </summary>
        /// <value>The status filter action control.</value>
        public StatusFilterAction StatusFilterAction { get; set; }

        /// <summary>
        /// Gets or sets the category filter action control.
        /// </summary>
        /// <value>The category filter action control.</value>
        public CategoryFilterAction CategoryFilterAction { get; set; }

        /// <summary>
        /// Gets or sets the multiple category filter action.
        /// </summary>
        /// <value>
        /// The multiple category filter action.
        /// </value>
        public MultipleCategoriesFilterAction MultipleCategoriesFilterAction { get; set; }

        /// <summary>
        /// Gets or sets the template provider to use for providing templating functionality within this control.
        /// </summary>
        /// <value>The template provider to use for providing templating functionality within this control</value>
        /// <exception cref="ArgumentNullException"><c>value</c> is null.</exception>
        public new PagingTemplateProvider TemplateProvider
        {
            get { return (PagingTemplateProvider)base.TemplateProvider; }
            set { base.TemplateProvider = value; }
        }

        /// <summary>
        /// Gets or sets the total number of events.
        /// </summary>
        /// <remarks>
        /// So that we can have proper paging information constructed before we query for the list of events, we need to persist the total number of events between postbacks.
        /// </remarks>
        /// <value>The total number of events.</value>
        private int TotalNumberOfEvents
        {
            get
            {
                // if we haven't set the value in ViewState yet, just make sure that the next page gets populated (it will later be hidden if it isn't necessary)
                return this.ViewState["TotalNumberOfEvents"] as int? ?? int.MaxValue;
            }

            set 
            { 
                this.ViewState["TotalNumberOfEvents"] = value; 
            }
        }

        /// <summary>
        /// Gets the number of events per page.
        /// </summary>
        /// <value>The number of events per page.</value>
        private int RecordsPerPage
        {
            get { return ModuleSettings.RecordsPerPage.GetValueAsInt32For(this).Value; }
        }

        /// <summary>
        /// Gets the status of events to retrieve.  Possible values are "Active" and "All".  "Active" by default.
        /// </summary>
        /// <value>The status of events to retrieve.</value>
        private string Status
        {
            get
            {
                string statusValue = this.Request.QueryString["status"];
                if (Engage.Utility.HasValue(statusValue))
                {
                    return statusValue;
                }

                return "Active";
            }
        }

        /// <summary>
        /// Gets the field on which to sort the event list.  "EventStart" by default.
        /// </summary>
        /// <value>The field on which to sort the event list.</value>
        private string SortExpression
        {
            get
            {
                return "TITLE".Equals(this.Request.QueryString["sort"], StringComparison.OrdinalIgnoreCase)
                           ? Utility.GetPropertyName(e => e.Title)
                           : Utility.GetPropertyName(e => e.EventStart);
            }
        }

        /// <summary>
        /// Gets the IDs of the category by which to filter events, or <c>null</c> to display all events.
        /// </summary>
        /// <value>
        /// The ID of the category to filter by.
        /// </value>
        private IEnumerable<int> FilterCategoryIds 
        {
            get
            {
                if (this.filterCategoryId == null && this.Session["categoryIds"] != null)
                {
                    this.filterCategoryId = (int[])this.Session["categoryIds"];
                }

                return this.filterCategoryId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is displaying the Manage Events page, rather than a public-facing list.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is displaying Manage Events; otherwise, <c>false</c>.
        /// </value>
        private bool IsManageEvents
        {
            get
            {
                return "EVENTLISTINGADMIN".Equals(this.GetCurrentControlKey(), StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Raises the <see cref="EventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.SetupTemplateProvider();
            base.OnInit(e);            
        }

        /// <summary>
        /// Handles the <see cref="DeleteAction.Delete"/> and <see cref="CancelAction.Cancel"/> events,
        /// reloading the list of events to reflect the changes made by those controls
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void ReloadPage(object sender, EventArgs args)
        {
            this.ReloadPage(this.CurrentPageIndex);
        }

        /// <summary>
        /// Handles the <see cref="DeleteAction.Delete"/> event,
        /// reloading the list of events to reflect the changes made by those controls
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void ReturnToList(object sender, EventArgs args)
        {
            this.ReloadPage(this.CurrentPageIndex);
        }

        /// <summary>
        /// Reloads the page.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        protected void ReloadPage(int pageNumber)
        {
            var sortExpression = this.SortAction != null ? this.SortAction.SelectedValue : null;
            var status = this.StatusFilterAction != null ? this.StatusFilterAction.SelectedValue : null;
            ////var selectedCategoryIds = this.CategoryFilterAction != null ? this.CategoryFilterAction.SelectedCategoryId : null;
            this.Response.Redirect(this.GetPageUrl(pageNumber, sortExpression, status), true);
        }

        /// <summary>
        /// Gets the URL to use for the paging buttons, with the page number templated out for use with <see cref="string.Format(IFormatProvider,string,object[])"/> (that is, "{0}")
        /// </summary>
        /// <param name="sortExpression">The field on which to sort the event list.</param>
        /// <param name="status">The status of events to retrieve.</param>
        /// <returns>
        /// The URL to use for the paging buttons, with the page number templated out for use with <see cref="string.Format(IFormatProvider,string,object[])"/> (that is, "{0}")
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Backwards compatibility")]
        protected string GetPageUrlTemplate(string sortExpression, string status)
        {
            // We can't just send {0} to BuildLinkUrl, because it will get "special treatment" by the friendly URL provider for its special characters
            const string UniqueReplaceableTemplateValue = "__--0--__";
            string controlKey = this.GetCurrentControlKey();
            if (!Engage.Utility.HasValue(controlKey))
            {
                controlKey = MainContainer.DefaultControlKey;
            }

            var sortParameter = sortExpression != null ? "sort=" + sortExpression : null;
            var statusParameter = status != null ? "status=" + status : null;
            return this.BuildLinkUrl(this.ModuleId, controlKey, sortParameter, statusParameter, this.PageIndexQueryStringKey + "=" + UniqueReplaceableTemplateValue)
                       .Replace(UniqueReplaceableTemplateValue, "{0}");
        }

        /// <summary>
        /// Handles the <see cref="Events.SortAction.SortChanged"/> event of the <see cref="SortAction"/> control and the 
        /// <see cref="Events.StatusFilterAction.SortChanged"/> of the <see cref="StatusFilterAction"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SortActions_SortChanged(object sender, EventArgs e)
        {
            const int PageNumber = 1;
            this.ReloadPage(PageNumber);
        }

        /// <summary>
        /// Gets the URL to use for this page, for a listing with the given <paramref name="pageNumber"/>, <paramref name="sortExpression"/>, and <paramref name="status"/>.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="sortExpression">The field on which to sort the event list.</param>
        /// <param name="status">The status of events to retrieve.</param>
        /// <returns>
        /// The URL to use for this page, for a listing with the given <paramref name="pageNumber"/>, <paramref name="sortExpression"/>, and <paramref name="status"/>.
        /// </returns>
        private string GetPageUrl(int pageNumber, string sortExpression, string status)
        {
            return string.Format(CultureInfo.InvariantCulture, this.GetPageUrlTemplate(sortExpression, status), pageNumber);
        }

        /// <summary>
        /// Method used to process a token. This method is invoked from the <see cref="TemplateEngine"/> class. Since this control knows best on how to construct
        /// the page.
        /// </summary>
        /// <param name="container">The container into which created controls should be added</param>
        /// <param name="tag">The tag to process</param>
        /// <param name="templateItem">The object to query for data to implement the given tag</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>Whether to process the tag's ChildTags collection</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by container")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "The complexity cannot easily be reduced and the method is easy to understand, test, and maintain")]
        private bool ProcessTag(Control container, Tag tag, ITemplateable templateItem, string resourceFile)
        {
            var currentEvent = (Event)templateItem;
            if (tag.TagType == TagType.Open)
            {
                switch (tag.LocalName.ToUpperInvariant())
                {
                    case "EVENTSORT":
                        this.SortAction = (SortAction)this.LoadControl(this.ActionsControlsFolder + "SortAction.ascx");
                        this.SortAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.SortAction.LocalResourceFile = resourceFile;
                        this.SortAction.SortChanged += this.SortActions_SortChanged;

                        container.Controls.Add(this.SortAction);
                        break;
                    case "STATUSFILTER":
                        this.StatusFilterAction = (StatusFilterAction)this.LoadControl(this.ActionsControlsFolder + "StatusFilterAction.ascx");
                        this.StatusFilterAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.StatusFilterAction.LocalResourceFile = resourceFile;
                        this.StatusFilterAction.SortChanged += this.SortActions_SortChanged;

                        container.Controls.Add(this.StatusFilterAction);
                        break;
                    case "CATEGORYFILTER":
                        this.CategoryFilterAction = (CategoryFilterAction)this.LoadControl(this.ActionsControlsFolder + "CategoryFilterAction.ascx");
                        this.CategoryFilterAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.CategoryFilterAction.LocalResourceFile = resourceFile;
                        this.CategoryFilterAction.CategoryChanged += this.CategoryFilter_SortChanged;

                        container.Controls.Add(this.CategoryFilterAction);
                        break;
                    case "MULTIPLECATEGORYFILTER":
                        this.MultipleCategoriesFilterAction = (MultipleCategoriesFilterAction)this.LoadControl(this.ActionsControlsFolder + "MultipleCategoriesFilterAction.ascx");
                        this.MultipleCategoriesFilterAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.MultipleCategoriesFilterAction.LocalResourceFile = resourceFile;
                        this.MultipleCategoriesFilterAction.CategoryChanged += this.MultipleCategoriesActions_SortChanged;

                        container.Controls.Add(MultipleCategoriesFilterAction);
                        break;
                    case "READMORE":
                        if (currentEvent != null && !Engage.Utility.HasValue(currentEvent.Description))
                        {
                            break;
                        }

                        var linkUrl = currentEvent != null
                                          ? this.BuildLinkUrl(
                                              this.DetailsTabId,
                                              this.DetailsModuleId,
                                              "EventDetail",
                                              Dnn.Events.Utility.GetEventParameters(currentEvent))
                                          : Globals.NavigateURL(this.DetailsTabId);

                        TemplateEngine.AddControl(container, TemplateEngine.CreateLink(tag, templateItem, null, resourceFile, linkUrl));
                        break;
                    default:
                        return this.ProcessCommonTag(container, tag, currentEvent, resourceFile);
                }
            }
            else if (tag.TagType == TagType.Close)
            {
                return this.ProcessCommonTag(container, tag, currentEvent, resourceFile);
            }

            return false;
        }

        /// <summary>
        /// Handles the SortChanged event of the MultipleCategoriesActions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MultipleCategoriesActions_SortChanged(object sender, EventArgs e)
        {
            this.Session.Add("categoryIds", this.MultipleCategoriesFilterAction.SelectedCategoryIds);
            this.ReloadPage(this.CurrentPageIndex);
        }

        /// <summary>
        /// Handles the SortChanged event of the CategoryFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CategoryFilter_SortChanged(object sender, EventArgs e)
        {
            if (this.CategoryFilterAction.SelectedCategoryId.HasValue)
            {
                this.Session.Add("categoryIds", new[] { this.CategoryFilterAction.SelectedCategoryId.Value });
            }
            else
            {
                this.Session.Remove("categoryIds");
            }

            this.ReloadPage(this.CurrentPageIndex);
        }

        /// <summary>
        /// Sets up the <see cref="TemplateProvider"/> for this control.
        /// </summary>
        private void SetupTemplateProvider()
        {
            string templateFolderName = this.IsManageEvents ? "Admin/ManageEvents" : ModuleSettings.Template.GetValueAsStringFor(this);
            this.TemplateProvider = new PagingTemplateProvider(
                this.GetTemplate(templateFolderName),
                this,
                this.GetPageUrlTemplate(this.SortExpression, this.Status),
                new ItemPagingState(this.CurrentPageIndex, this.TotalNumberOfEvents, this.RecordsPerPage), 
                this.ProcessTag,
                this.GetEvents, 
                null, 
                new GlobalTemplateContext(this.ModuleContext));
        }

        /// <summary>
        /// Gets a list of the <see cref="Event"/>s for this module.  Does not take the <paramref name="listTag"/> or <paramref name="context"/> into account,
        /// effectively only supporting one data source.
        /// </summary>
        /// <remarks>
        /// The <paramref name="context"/> parameter should always be <c>null</c> unless the Engage:List tag is nested inside of another Engage:List.
        /// </remarks>
        /// <param name="listTag">The Engage:List <see cref="Tag"/> for which to return a data source</param>
        /// <param name="context">The current <see cref="Event"/> being processed, or <c>null</c> if no list is currently being processed</param>
        /// <returns>A list of the <see cref="Event"/>s over which the given <paramref name="listTag"/> should be processed</returns>
        private IEnumerable<ITemplateable> GetEvents(Tag listTag, ITemplateable context)
        {
            var dateRange = this.IsManageEvents
                                ? new DateRange(DateRangeBound.CreateUnboundedBound(), DateRangeBound.CreateUnboundedBound())
                                : ModuleSettings.GetDateRangeFor(this);

            var events = EventCollection.Load(
                    this.PortalId,
                    dateRange.GetStartDate(),
                    dateRange.GetEndDate(),
                    this.SortExpression,
                    this.CurrentPageIndex - 1,
                    this.RecordsPerPage,
                    this.Status.Equals("All", StringComparison.OrdinalIgnoreCase),
                    !this.IsManageEvents && this.IsFeatured,
                    !this.IsManageEvents && this.HideFullEvents,
                    IsLoggedIn ? this.UserInfo.Email : null,
                    this.FilterCategoryIds ?? this.CategoryIds);
            
            this.TotalNumberOfEvents = events.TotalRecords;
            this.TemplateProvider.ItemPagingState = new ItemPagingState(this.CurrentPageIndex, events.TotalRecords, this.RecordsPerPage);

            return ((IEnumerable<Event>)events).Select(e =>
                {
                    if (string.IsNullOrEmpty(e.Category.Name))
                    {
                        e.Category.Name = this.Localize("DefaultCategory", this.LocalSharedResourceFile);
                    }

                    return (ITemplateable)e;
                });
        }
    }
}