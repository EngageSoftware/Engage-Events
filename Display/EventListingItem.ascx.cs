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
    using Engage.Events;
    using Framework.Templating;
    using Templating;

    /// <summary>
    /// Custom event listing item
    /// </summary>
    public partial class EventListingItem : TemplatedDisplayModuleBase
    {
        /// <summary>
        /// Backing field for <see cref="CategoryId"/>
        /// </summary>
        private int? categoryId;

        /// <summary>
        /// Gets the listing mode used for this display.
        /// </summary>
        /// <value>The listing mode used for this display</value>
        public ListingMode ListingMode { get; private set; }

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
        /// Gets the ID of the category by which to filter events, or <c>null</c> to display all events.
        /// </summary>
        /// <value>The ID of the category to filter by.</value>
        private int? CategoryId
        {
            get
            {
                if (this.categoryId == null)
                {
                    int parsedCategoryId;
                    string categoryIdValue = this.Request.QueryString["catId"];
                    if (Engage.Utility.HasValue(categoryIdValue) &&
                        int.TryParse(categoryIdValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedCategoryId))
                    {
                        this.categoryId = parsedCategoryId;
                    }
                }

                return this.categoryId;
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
        /// Sets the listing mode used for this display.
        /// </summary>
        /// <param name="listingModeValue">The listing mode used for this display.</param>
        public void SetListingMode(string listingModeValue)
        {
            if (!string.IsNullOrEmpty(listingModeValue))
            {
                try
                {
                    this.ListingMode = (ListingMode)Enum.Parse(typeof(ListingMode), listingModeValue, true);
                }
                catch (ArgumentException)
                {
                    // if listingModeValue does not parse, just leave this.ListingMode to its default
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="EventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.ListingMode = ModuleSettings.DisplayModeOption.GetValueAsEnumFor<ListingMode>(this).Value;
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
            var selectedCategoryId = this.CategoryFilterAction != null ? this.CategoryFilterAction.SelectedCategoryId : null;
            this.Response.Redirect(this.GetPageUrl(pageNumber, this.SortAction.SelectedValue, this.StatusFilterAction.SelectedValue, selectedCategoryId), true);
        }

        /// <summary>
        /// Gets the URL to use for the paging buttons, with the page number templated out for use with <see cref="string.Format(IFormatProvider,string,object[])"/> (that is, "{0}")
        /// </summary>
        /// <param name="sortExpression">The field on which to sort the event list.</param>
        /// <param name="status">The status of events to retrieve.</param>
        /// <param name="filterCategoryId">The ID of the category by which results should be filtered, or <c>null</c> for all results.</param>
        /// <returns>
        /// The URL to use for the paging buttons, with the page number templated out for use with <see cref="string.Format(IFormatProvider,string,object[])"/> (that is, "{0}")
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "Backwards compatibility")]
        protected string GetPageUrlTemplate(string sortExpression, string status, int? filterCategoryId)
        {
            // We can't just send {0} to BuildLinkUrl, because it will get "special treatment" by the friendly URL provider for its special characters
            const string UniqueReplaceableTemplateValue = "__--0--__";
            string controlKey = this.GetCurrentControlKey();
            if (!Engage.Utility.HasValue(controlKey))
            {
                controlKey = MainContainer.DefaultControlKey;
            }

            var categoryIdParameter = filterCategoryId.HasValue ? "catId=" + filterCategoryId.Value.ToString(CultureInfo.InvariantCulture) : null;
            return this.BuildLinkUrl(this.ModuleId, controlKey, "sort=" + sortExpression, "status=" + status, categoryIdParameter, "currentPage=" + UniqueReplaceableTemplateValue).Replace(UniqueReplaceableTemplateValue, "{0}");
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
        /// <param name="filterCategoryId">The ID of the category by which results should be filtered, or <c>null</c> for all results.</param>
        /// <returns>
        /// The URL to use for this page, for a listing with the given <paramref name="pageNumber"/>, <paramref name="sortExpression"/>, and <paramref name="status"/>.
        /// </returns>
        private string GetPageUrl(int pageNumber, string sortExpression, string status, int? filterCategoryId)
        {
            return string.Format(CultureInfo.InvariantCulture, this.GetPageUrlTemplate(sortExpression, status, filterCategoryId), pageNumber);
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
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "The complexity cannot easily be reduced and the method is easy to understand, test, and maintain")]
        private bool ProcessTag(Control container, Tag tag, ITemplateable templateItem, string resourceFile)
        {
            Event currentEvent = (Event)templateItem;
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
                        this.CategoryFilterAction.CategoryChanged += this.SortActions_SortChanged;

                        container.Controls.Add(this.CategoryFilterAction);
                        break;
                    case "READMORE":
                        if (currentEvent == null || Engage.Utility.HasValue(currentEvent.Description))
                        {
                            StringBuilder detailLinkBuilder = new StringBuilder();
                            string linkUrl;
                            if (currentEvent != null)
                            {
                                linkUrl = this.BuildLinkUrl(this.DetailsTabId, this.DetailsModuleId, "EventDetail", Dnn.Events.Utility.GetEventParameters(currentEvent));
                            }
                            else
                            {
                                linkUrl = Globals.NavigateURL(this.DetailsTabId);
                            }

                            detailLinkBuilder.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    "<a href=\"{0}\"",
                                    HttpUtility.HtmlAttributeEncode(linkUrl));

                            string detailLinkCssClass = TemplateEngine.GetAttributeValue(tag, templateItem, resourceFile, "CssClass", "class");
                            if (Engage.Utility.HasValue(detailLinkCssClass))
                            {
                                detailLinkBuilder.AppendFormat(
                                        CultureInfo.InvariantCulture, 
                                        "class=\"{0}\"", 
                                        HttpUtility.HtmlAttributeEncode(detailLinkCssClass));
                            }

                            detailLinkBuilder.Append(">");

                            if (!tag.HasChildTags)
                            {
                                detailLinkBuilder
                                    .Append(TemplateEngine.GetAttributeValue(tag, templateItem, resourceFile, "Text"))
                                    .Append("</a>");
                            }

                            container.Controls.Add(new LiteralControl(detailLinkBuilder.ToString()));
                        }

                        return true;
                    default:
                        return this.ProcessCommonTag(container, tag, currentEvent, resourceFile);
                }
            }
            else if (tag.TagType == TagType.Close)
            {
                switch (tag.LocalName.ToUpperInvariant())
                {
                    case "READMORE":
                        container.Controls.Add(new LiteralControl("</a>"));
                        break;
                    default:
                        return this.ProcessCommonTag(container, tag, currentEvent, resourceFile);
                }
            }

            return false;
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
                this.GetPageUrlTemplate(this.SortExpression, this.Status, this.CategoryId),
                new ItemPagingState(this.CurrentPageIndex, this.TotalNumberOfEvents, this.RecordsPerPage), 
                this.ProcessTag,
                this.GetEvents);
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
            var events = EventCollection.Load(
                    this.PortalId,
                    this.IsManageEvents ? ListingMode.All : this.ListingMode,
                    this.SortExpression,
                    this.CurrentPageIndex - 1,
                    this.RecordsPerPage,
                    this.Status.Equals("All", StringComparison.OrdinalIgnoreCase),
                    this.IsManageEvents ? false : this.IsFeatured,
                    this.IsManageEvents ? false : this.HideFullEvents,
                    IsLoggedIn ? this.UserInfo.Email : null,
                    this.CategoryId.HasValue ? new[] { this.CategoryId.Value } : this.CategoryIds);
            
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