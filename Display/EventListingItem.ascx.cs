// <copyright file="EventListingItem.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Localization;
    using Engage.Events;
    using Framework.Templating;
    using Licensing;
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
        /// Backing field for <see cref="IsFeatured"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isFeatured;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventListingItem"/> class.
        /// </summary>
        public EventListingItem()
        {
            this.ActionsControlsFolder = "~" + this.DesktopModuleFolderName + "Actions/";
        }

        /// <summary>
        /// Gets the name of the this module's desktop module record in DNN.
        /// </summary>
        /// <value>The name of this module's desktop module record in DNN.</value>
        public override string DesktopModuleName
        {
            get { return Utility.DesktopModuleName; }
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
        /// Gets or sets a value indicating whether this instance should display only featured events.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance should only display featured events; otherwise, <c>false</c>.
        /// </value>
        public bool IsFeatured
        {
            [DebuggerStepThrough]
            get { return this.isFeatured; }
            [DebuggerStepThrough]
            set { this.isFeatured = value; }
        }

        /// <summary>
        /// Gets or sets the template provider to use for providing templating functionality within this control.
        /// </summary>
        /// <value>The template provider to use for providing templating functionality within this control</value>
        /// <exception cref="ArgumentNullException"><c>value</c> is null.</exception>
        public new RepeaterTemplateProvider TemplateProvider
        {
            get { return (RepeaterTemplateProvider)base.TemplateProvider; }
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
        /// Binds the data for this control.
        /// </summary>
        public void BindData()
        {
            EventCollection events = EventCollection.Load(this.PortalId, this.listingMode, this.SortExpression, this.CurrentPageIndex - 1, this.RecordsPerPage, this.Status.Equals("All", StringComparison.Ordinal), this.IsFeatured);

            this.TotalNumberOfEvents = events.TotalRecords;
            this.TemplateProvider.ItemPagingState = new ItemPagingState(this.CurrentPageIndex, events.TotalRecords, this.RecordsPerPage);
            this.TemplateProvider.DataSource = events;
            this.TemplateProvider.DataBind();
        }

        /// <summary>
        /// Raises the <see cref="EventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.SetupTemplateProvider();

            base.OnInit(e);
            this.listingMode = Dnn.Utility.GetEnumSetting(this.Settings, Setting.DisplayModeOption.PropertyName, ListingMode.All);

            this.Load += this.Page_Load;
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
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Handles the SortChanged event of the SortAction and StatusFilterAction controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SortStatusAction_SortChanged(object sender, EventArgs e)
        {
            const int PageNumber = 1;
            this.Response.Redirect(this.GetPageUrl(PageNumber, this.SortAction.SelectedValue, this.StatusFilterAction.SelectedValue), true);
        }

        /// <summary>
        /// Handles the Cancel event of the CancelAction control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelAction_Cancel(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Handles the Delete event of the DeleteAction control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DeleteAction_Delete(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Method used to process a token. This method is invoked from the TemplateEngine class. Since this control knows
        /// best on how to contruct the page. ListingHeader, ListingItem and Listing Footer templates are processed here.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="tag">The tag being processed.</param>
        /// <param name="engageObject">The engage object.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "The complexity cannot easily be reduced and the method is easy to understand, test, and maintain")]
        private void ProcessTag(Control container, Tag tag, object engageObject, string resourceFile)
        {
            Event currentEvent = (Event)engageObject;

            if (tag.TagType == TagType.Open)
            {
                switch (tag.LocalName.ToUpperInvariant())
                {
                    case "EDITEVENTBUTTON":
                        ButtonAction editEventAction = (ButtonAction)this.LoadControl(this.ActionsControlsFolder + "ButtonAction.ascx");
                        editEventAction.CurrentEvent = currentEvent;
                        editEventAction.ModuleConfiguration = this.ModuleConfiguration;
                        editEventAction.Href = this.BuildLinkUrl(this.ModuleId, "EventEdit", Dnn.Events.Utility.GetEventParameters(currentEvent));
                        editEventAction.Text = Localization.GetString("EditEventButton", resourceFile);
                        container.Controls.Add(editEventAction);
                        editEventAction.Visible = this.IsAdmin;
                        break;
                    case "VIEWRESPONSESBUTTON":
                        ButtonAction responsesEventAction = (ButtonAction)this.LoadControl(this.ActionsControlsFolder + "ButtonAction.ascx");
                        responsesEventAction.CurrentEvent = currentEvent;
                        responsesEventAction.ModuleConfiguration = this.ModuleConfiguration;
                        responsesEventAction.Href = this.BuildLinkUrl(this.ModuleId, "ResponseDetail", Dnn.Events.Utility.GetEventParameters(currentEvent));
                        responsesEventAction.Text = Localization.GetString("ResponsesButton", resourceFile);
                        container.Controls.Add(responsesEventAction);
                        responsesEventAction.Visible = this.IsAdmin;
                        break;
                    case "REGISTERBUTTON":
                        ButtonAction registerEventAction = (ButtonAction)this.LoadControl(this.ActionsControlsFolder + "ButtonAction.ascx");
                        registerEventAction.CurrentEvent = currentEvent;
                        registerEventAction.ModuleConfiguration = this.ModuleConfiguration;
                        registerEventAction.Href = this.BuildLinkUrl(this.ModuleId, "Register", Dnn.Events.Utility.GetEventParameters(currentEvent));
                        registerEventAction.Text = Localization.GetString("RegisterButton", resourceFile);
                        container.Controls.Add(registerEventAction);

                        // to register must be an event that allows registrations, be active, and have not ended
                        registerEventAction.Visible = currentEvent.AllowRegistrations && !currentEvent.Canceled && currentEvent.EventEnd > DateTime.Now;

                        break;
                    case "ADDTOCALENDARBUTTON":
                        AddToCalendarAction addToCalendarAction = (AddToCalendarAction)this.LoadControl(this.ActionsControlsFolder + "AddToCalendarAction.ascx");
                        addToCalendarAction.CurrentEvent = currentEvent;
                        addToCalendarAction.ModuleConfiguration = this.ModuleConfiguration;

                        // must be an active event and has not ended
                        addToCalendarAction.Visible = !currentEvent.Canceled && currentEvent.EventEnd > DateTime.Now;
                        container.Controls.Add(addToCalendarAction);
                        break;
                    case "DELETEBUTTON":
                        DeleteAction deleteAction = (DeleteAction)this.LoadControl(this.ActionsControlsFolder + "DeleteAction.ascx");
                        deleteAction.CurrentEvent = currentEvent;
                        deleteAction.ModuleConfiguration = this.ModuleConfiguration;
                        deleteAction.Delete += this.DeleteAction_Delete;
                        container.Controls.Add(deleteAction);
                        break;
                    case "CANCELBUTTON":
                        CancelAction cancelAction = (CancelAction)this.LoadControl(this.ActionsControlsFolder + "CancelAction.ascx");
                        cancelAction.CurrentEvent = currentEvent;
                        cancelAction.ModuleConfiguration = this.ModuleConfiguration;
                        cancelAction.Cancel += this.CancelAction_Cancel;
                        container.Controls.Add(cancelAction);
                        break;
                    case "EDITEMAILBUTTON":
                        ButtonAction editEmailAction = (ButtonAction)this.LoadControl(this.ActionsControlsFolder + "ButtonAction.ascx");
                        editEmailAction.CurrentEvent = currentEvent;
                        editEmailAction.ModuleConfiguration = this.ModuleConfiguration;
                        editEmailAction.Href = this.BuildLinkUrl(this.ModuleId, "EmailEdit", Dnn.Events.Utility.GetEventParameters(currentEvent));
                        editEmailAction.Text = Localization.GetString("EditEmailButton", resourceFile);
                        container.Controls.Add(editEmailAction);
                        editEmailAction.Visible = this.IsAdmin;
                        break;
                    case "EVENTSORT":
                        this.sortAction = (SortAction)this.LoadControl(this.ActionsControlsFolder + "SortAction.ascx");
                        this.sortAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.sortAction.SortChanged += this.SortStatusAction_SortChanged;
                        container.Controls.Add(this.sortAction);
                        break;
                    case "STATUSFILTER":
                        this.statusFilterAction = (StatusFilterAction)this.LoadControl(this.ActionsControlsFolder + "StatusFilterAction.ascx");
                        this.statusFilterAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.statusFilterAction.SortChanged += this.SortStatusAction_SortChanged;
                        container.Controls.Add(this.statusFilterAction);
                        break;
                    case "READMORE":
                        if (Engage.Utility.HasValue(currentEvent.Description))
                        {
                            HyperLink detailLink = new HyperLink();
                            detailLink.Text = Localization.GetString(tag.GetAttributeValue("ResourceKey"), resourceFile);
                            if (string.IsNullOrEmpty(detailLink.Text))
                            {
                                detailLink.Text = "Read More...";
                            }

                            detailLink.CssClass = tag.GetAttributeValue("CssClass");
                            detailLink.NavigateUrl = this.BuildLinkUrl(this.ModuleId, "EventDetail", Dnn.Events.Utility.GetEventParameters(currentEvent));

                            container.Controls.Add(detailLink);
                        }

                        break;
                    case "RECURRENCESUMMARY":
                        container.Controls.Add(new LiteralControl(Dnn.Events.Utility.GetRecurrenceSummary(currentEvent.RecurrenceRule)));
                        break;
                    case "EVENTWRAPPER":
                        StringBuilder cssClass = new StringBuilder(tag.GetAttributeValue("CssClass"));
                        if (currentEvent.IsRecurring)
                        {
                            AppendCssClassAttribute(tag, cssClass, "RecurringEventCssClass");
                        }

                        if (currentEvent.IsFeatured)
                        {
                            AppendCssClassAttribute(tag, cssClass, "FeaturedEventCssClass");
                        }

                        container.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, "<div class=\"{0}\">", cssClass.ToString())));
                        break;
                    case "DURATION":
                        container.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(Dnn.Events.Utility.GetFormattedEventDate(currentEvent.EventStart, currentEvent.EventEnd))));
                        break;
                    default:
                        break;
                }
            }
            else if (tag.TagType == TagType.Close)
            {
                if (tag.LocalName.Equals("EVENTWRAPPER", StringComparison.OrdinalIgnoreCase))
                {
                    container.Controls.Add(new LiteralControl("</div>"));
                }
            }
        }

        /// <summary>
        /// Sets up the <see cref="TemplateProvider"/> for this control.
        /// </summary>
        private void SetupTemplateProvider()
        {
            string headerTemplateName = Dnn.Utility.GetStringSetting(this.Settings, Framework.Setting.HeaderTemplate.PropertyName);
            string itemTemplateName = Dnn.Utility.GetStringSetting(this.Settings, Framework.Setting.ItemTemplate.PropertyName);
            string footerTemplateName = Dnn.Utility.GetStringSetting(this.Settings, Framework.Setting.FooterTemplate.PropertyName);

            this.TemplateProvider = new RepeaterTemplateProvider(
                Utility.DesktopModuleName,
                TemplateEngine.GetTemplate(this.PhysicialTemplatesFolderName, headerTemplateName),
                this.HeaderPlaceholder,
                TemplateEngine.GetTemplate(this.PhysicialTemplatesFolderName, itemTemplateName),
                this.ItemPlaceholder,
                TemplateEngine.GetTemplate(this.PhysicialTemplatesFolderName, footerTemplateName),
                this.FooterPlaceholder,
                this.GetPageUrlTemplate(this.SortExpression, this.Status),
                new ItemPagingState(this.CurrentPageIndex, this.TotalNumberOfEvents, this.RecordsPerPage), 
                this.ProcessTag);
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
            return this.BuildLinkUrl(this.ModuleId, "EventListing", "sort=" + sortExpression, "status=" + status, "currentPage=" + UniqueReplaceableTemplateValue).Replace(UniqueReplaceableTemplateValue, "{0}");
        }
    }
}