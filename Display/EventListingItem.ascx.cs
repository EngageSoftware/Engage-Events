// <copyright file="EventListingItem.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2009
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
    using System.Diagnostics;
    using System.Globalization;
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
    public partial class EventListingItem : ModuleBase
    {
        /// <summary>
        /// Relative path to the folder where the action controls are located in this module
        /// </summary>
        private readonly string ActionsControlsFolder;

        /// <summary>
        /// Keeps track of the last event processed in <see cref="ProcessTag"/> to enable alternating behavior
        /// </summary>
        private Event lastEventProcessed;

        /// <summary>
        /// Keeps track of whether the current event being processed in <see cref="ProcessTag"/> is an alternating (even number) event
        /// </summary>
        private bool isAlternatingEvent = true;

        /// <summary>
        /// Backing field for <see cref="SortAction"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SortAction sortAction;

        /// <summary>
        /// Backing field for <see cref="StatusFilterAction"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StatusFilterAction statusFilterAction;

        /// <summary>
        /// Backing field for <see cref="ListingMode"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ListingMode listingMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventListingItem"/> class.
        /// </summary>
        public EventListingItem()
        {
            this.ActionsControlsFolder = "~" + this.DesktopModuleFolderName + "Actions/";
        }

        /// <summary>
        /// Gets the listing mode used for this display.
        /// </summary>
        /// <value>The listing mode used for this display</value>
        public ListingMode ListingMode
        {
            [DebuggerStepThrough]
            get { return this.listingMode; }
            [DebuggerStepThrough]
            private set { this.listingMode = value; }
        }

        /// <summary>
        /// Gets or sets the sort action control.
        /// </summary>
        /// <value>The sort action control.</value>
        public SortAction SortAction
        {
            [DebuggerStepThrough]
            get { return this.sortAction; }
            [DebuggerStepThrough]
            set { this.sortAction = value; }
        }

        /// <summary>
        /// Gets or sets the sort status action control.
        /// </summary>
        /// <value>The sort status action control.</value>
        public StatusFilterAction StatusFilterAction
        {
            [DebuggerStepThrough]
            get { return this.statusFilterAction; }
            [DebuggerStepThrough]
            set { this.statusFilterAction = value; }
        }

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
            get { return Dnn.Utility.GetIntSetting(this.Settings, Framework.Setting.RecordsPerPage.PropertyName, 1); }
        }

        /// <summary>
        /// Gets the Tab ID to use when displaying module details.
        /// </summary>
        /// <value>The Tab ID to use when displaying module details.</value>
        private int DetailsTabId
        {
            get { return Dnn.Utility.GetIntSetting(this.Settings, "DetailsDisplayTabId", this.TabId); }
        }

        /// <summary>
        /// Gets the Module ID to use when displaying module details.
        /// </summary>
        /// <value>The Module ID to use when displaying module details.</value>
        private int DetailsModuleId
        {
            get { return Dnn.Utility.GetIntSetting(this.Settings, "DetailsDisplayModuleId", this.ModuleId); }
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
                string sortValue = this.Request.QueryString["sort"];
                if (Engage.Utility.HasValue(sortValue))
                {
                    return sortValue;
                }

                return "EventStart";
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
            this.listingMode = Dnn.Utility.GetEnumSetting(this.Settings, "DisplayModeOption", ListingMode.All);
            this.SetupTemplateProvider();
            base.OnInit(e);            
        }

        /// <summary>
        /// Appends the given attribute to <paramref name="cssClassBuilder"/>, adding a space beforehand if necessary.
        /// </summary>
        /// <param name="tag">The tag whose attribute we are appending.</param>
        /// <param name="cssClassBuilder">The <see cref="StringBuilder"/> which will contain the appended CSS class.</param>
        /// <param name="attributeName">Name of the attribute being appended.</param>
        private static void AppendCssClassAttribute(Tag tag, StringBuilder cssClassBuilder, string attributeName)
        {
            if (cssClassBuilder.Length > 0)
            {
                cssClassBuilder.Append(" ");
            }

            cssClassBuilder.Append(tag.GetAttributeValue(attributeName));
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
        /// Handles the <see cref="DeleteAction.Delete"/> and <see cref="CancelAction.Cancel"/> events, 
        /// reloading the list of events to reflect the changes made by those controls
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ShowUpdatedEvents(object sender, EventArgs args)
        {
            this.ReloadPage(this.CurrentPageIndex);
        }

        /// <summary>
        /// Reloads the page.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        private void ReloadPage(int pageNumber)
        {
            this.Response.Redirect(this.GetPageUrl(pageNumber, this.SortAction.SelectedValue, this.StatusFilterAction.SelectedValue), true);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "The complexity cannot easily be reduced and the method is easy to understand, test, and maintain")]
        private bool ProcessTag(Control container, Tag tag, ITemplateable templateItem, string resourceFile)
        {
            Event currentEvent = (Event)templateItem;
            if (currentEvent != null && currentEvent != this.lastEventProcessed)
            {
                this.isAlternatingEvent = !this.isAlternatingEvent;
                this.lastEventProcessed = currentEvent;
            }

            if (tag.TagType == TagType.Open)
            {
                switch (tag.LocalName.ToUpperInvariant())
                {
                    case "EDITEVENTBUTTON":
                        if (this.IsAdmin)
                        {
                            ButtonAction editEventAction = (ButtonAction)this.LoadControl(this.ActionsControlsFolder + "ButtonAction.ascx");
                            editEventAction.CurrentEvent = currentEvent;
                            editEventAction.ModuleConfiguration = this.ModuleConfiguration;
                            editEventAction.Href = this.BuildLinkUrl(this.ModuleId, "EventEdit", Dnn.Events.Utility.GetEventParameters(currentEvent));
                            editEventAction.ResourceKey = "EditEventButton";
                            container.Controls.Add(editEventAction);
                        }

                        break;
                    case "VIEWRESPONSESBUTTON":
                        if (this.IsAdmin)
                        {
                            ButtonAction responsesEventAction = (ButtonAction)this.LoadControl(this.ActionsControlsFolder + "ButtonAction.ascx");
                            responsesEventAction.CurrentEvent = currentEvent;
                            responsesEventAction.ModuleConfiguration = this.ModuleConfiguration;
                            responsesEventAction.Href = this.BuildLinkUrl(
                                    this.ModuleId, "ResponseDetail", Dnn.Events.Utility.GetEventParameters(currentEvent));
                            responsesEventAction.ResourceKey = "ResponsesButton";
                            container.Controls.Add(responsesEventAction);
                        }

                        break;
                    case "REGISTERBUTTON":
                        // to register must be an event that allows registrations, be active, and have not ended
                        // TODO: add message when event is cancelled, rather than hiding registration button
                        if (currentEvent != null && currentEvent.AllowRegistrations && !currentEvent.Canceled && currentEvent.EventEnd > DateTime.Now)
                        {
                            RegisterAction registerEventAction = (RegisterAction)this.LoadControl(this.ActionsControlsFolder + "RegisterAction.ascx");
                            registerEventAction.CurrentEvent = currentEvent;
                            registerEventAction.ModuleConfiguration = this.ModuleConfiguration;
                            registerEventAction.LocalResourceFile = resourceFile;

                            container.Controls.Add(registerEventAction);
                        }

                        break;
                    case "ADDTOCALENDARBUTTON":
                        // must be an active event and has not ended
                        if (currentEvent != null && !currentEvent.Canceled && currentEvent.EventEnd > DateTime.Now)
                        {
                            AddToCalendarAction addToCalendarAction = (AddToCalendarAction)this.LoadControl(this.ActionsControlsFolder + "AddToCalendarAction.ascx");
                            addToCalendarAction.CurrentEvent = currentEvent;
                            addToCalendarAction.ModuleConfiguration = this.ModuleConfiguration;
                            addToCalendarAction.LocalResourceFile = resourceFile;

                            container.Controls.Add(addToCalendarAction);
                        }

                        break;
                    case "DELETEBUTTON":
                        DeleteAction deleteAction = (DeleteAction)this.LoadControl(this.ActionsControlsFolder + "DeleteAction.ascx");
                        deleteAction.CurrentEvent = currentEvent;
                        deleteAction.ModuleConfiguration = this.ModuleConfiguration;
                        deleteAction.LocalResourceFile = resourceFile;
                        deleteAction.Delete += this.ShowUpdatedEvents;

                        container.Controls.Add(deleteAction);
                        break;
                    case "CANCELBUTTON":
                        CancelAction cancelAction = (CancelAction)this.LoadControl(this.ActionsControlsFolder + "CancelAction.ascx");
                        cancelAction.CurrentEvent = currentEvent;
                        cancelAction.ModuleConfiguration = this.ModuleConfiguration;
                        cancelAction.LocalResourceFile = resourceFile;
                        cancelAction.Cancel += this.ShowUpdatedEvents;
                        
                        container.Controls.Add(cancelAction);
                        break;
                    case "EDITEMAILBUTTON":
                        if (this.IsAdmin)
                        {
                            ButtonAction editEmailAction = (ButtonAction)this.LoadControl(this.ActionsControlsFolder + "ButtonAction.ascx");
                            editEmailAction.CurrentEvent = currentEvent;
                            editEmailAction.ModuleConfiguration = this.ModuleConfiguration;
                            editEmailAction.Href = this.BuildLinkUrl(this.ModuleId, "EmailEdit", Dnn.Events.Utility.GetEventParameters(currentEvent));
                            editEmailAction.ResourceKey = "EditEmailButton";
                            container.Controls.Add(editEmailAction);
                        }

                        break;
                    case "EVENTSORT":
                        this.sortAction = (SortAction)this.LoadControl(this.ActionsControlsFolder + "SortAction.ascx");
                        this.sortAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.sortAction.LocalResourceFile = resourceFile;
                        this.sortAction.SortChanged += this.SortActions_SortChanged;

                        container.Controls.Add(this.sortAction);
                        break;
                    case "STATUSFILTER":
                        this.statusFilterAction = (StatusFilterAction)this.LoadControl(this.ActionsControlsFolder + "StatusFilterAction.ascx");
                        this.statusFilterAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.statusFilterAction.LocalResourceFile = resourceFile;
                        this.statusFilterAction.SortChanged += this.SortActions_SortChanged;

                        container.Controls.Add(this.statusFilterAction);
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
                    case "RECURRENCESUMMARY":
                        if (currentEvent != null)
                        {
                            container.Controls.Add(new LiteralControl(Dnn.Events.Utility.GetRecurrenceSummary(currentEvent.RecurrenceRule)));
                        }

                        break;
                    case "EVENTWRAPPER":
                        if (currentEvent != null)
                        {
                            StringBuilder cssClass = new StringBuilder(TemplateEngine.GetAttributeValue(tag, templateItem, resourceFile, "CssClass", "class"));
                            if (currentEvent.IsRecurring)
                            {
                                AppendCssClassAttribute(tag, cssClass, "RecurringEventCssClass");
                            }

                            if (currentEvent.IsFeatured)
                            {
                                AppendCssClassAttribute(tag, cssClass, "FeaturedEventCssClass");
                            }

                            if (this.isAlternatingEvent)
                            {
                                AppendCssClassAttribute(tag, cssClass, "AlternatingCssClass");
                            }

                            container.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, "<div class=\"{0}\">", cssClass.ToString())));
                        }

                        return true;
                    case "DURATION":
                        if (currentEvent != null)
                        {
                            container.Controls.Add(
                                    new LiteralControl(
                                            HttpUtility.HtmlEncode(
                                                    Dnn.Events.Utility.GetFormattedEventDate(currentEvent.EventStart, currentEvent.EventEnd))));
                        }

                        break;
                    default:
                        break;
                }
            }
            else if (tag.TagType == TagType.Close)
            {
                switch (tag.LocalName.ToUpperInvariant())
                {
                    case "EVENTWRAPPER":
                        container.Controls.Add(new LiteralControl("</div>"));
                        break;
                    case "READMORE":
                        container.Controls.Add(new LiteralControl("</a>"));
                        break;
                    default:
                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets up the <see cref="TemplateProvider"/> for this control.
        /// </summary>
        private void SetupTemplateProvider()
        {
            string templateFolderName = this.IsManageEvents ? "Admin/ManageEvents" : Dnn.Utility.GetStringSetting(this.Settings, "Template");
            this.TemplateProvider = new PagingTemplateProvider(
                this.GetTemplate(templateFolderName),
                this,
                this.GetPageUrlTemplate(this.SortExpression, this.Status),
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
            EventCollection events = EventCollection.Load(
                    this.PortalId,
                    this.IsManageEvents ? ListingMode.All : this.listingMode,
                    this.SortExpression,
                    this.CurrentPageIndex - 1,
                    this.RecordsPerPage,
                    this.Status.Equals("All", StringComparison.Ordinal),
                    this.IsManageEvents ? false : this.IsFeatured);

            this.TotalNumberOfEvents = events.TotalRecords;
            this.TemplateProvider.ItemPagingState = new ItemPagingState(this.CurrentPageIndex, events.TotalRecords, this.RecordsPerPage);

            return events;
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
        /// Gets the URL to use for the paging buttons, with the page number templated out for use with <see cref="string.Format(IFormatProvider,string,object[])"/> (that is, "{0}")
        /// </summary>
        /// <param name="sortExpression">The field on which to sort the event list.</param>
        /// <param name="status">The status of events to retrieve.</param>
        /// <returns>
        /// The URL to use for the paging buttons, with the page number templated out for use with <see cref="string.Format(IFormatProvider,string,object[])"/> (that is, "{0}")
        /// </returns>
        private string GetPageUrlTemplate(string sortExpression, string status)
        {
            // We can't just send {0} to BuildLinkUrl, because it will get "special treatment" by the friendly URL provider for its special characters
            const string UniqueReplaceableTemplateValue = "__--0--__";
            return this.BuildLinkUrl(this.ModuleId, this.GetCurrentControlKey(), "sort=" + sortExpression, "status=" + status, "currentPage=" + UniqueReplaceableTemplateValue).Replace(UniqueReplaceableTemplateValue, "{0}");
        }
    }
}