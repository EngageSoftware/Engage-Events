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
    using System.Globalization;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Localization;
    using Engage.Events;
    using Framework.Templating;
    using Templating;

    /// <summary>
    /// Custom event listing item
    /// </summary>
    public partial class EventListingItem : RepeaterItemListing
    {
        /// <summary>
        /// Relative path to the folder where the action controls are located in this module
        /// </summary>
        private static readonly string ActionsControlsFolder = "~" + DesktopModuleFolderName + "Actions/";

        /// <summary>
        /// Backing field for <see cref="SortAction"/>
        /// </summary>
        private SortAction sortAction;

        /// <summary>
        /// Backing field for <see cref="StatusFilterAction"/>
        /// </summary>
        private StatusFilterAction statusFilterAction;

        /// <summary>
        /// Backing field for <see cref="ListingMode"/>
        /// </summary>
        private ListingMode listingMode;

        /// <summary>
        /// The collection of events to display
        /// </summary>
        /// <remarks>
        /// keep a reference around of the data that you have loaded
        /// </remarks>
        private EventCollection events;

        /// <summary>
        /// Gets the listing mode used for this display.
        /// </summary>
        /// <value>The listing mode used for this display</value>
        public ListingMode ListingMode
        {
            get { return this.listingMode; }
            private set { this.listingMode = value; }
        }

        /// <summary>
        /// Gets or sets the sort action control.
        /// </summary>
        /// <value>The sort action control.</value>
        public SortAction SortAction
        {
            get { return this.sortAction; }
            set { this.sortAction = value; }
        }

        /// <summary>
        /// Gets or sets the sort status action control.
        /// </summary>
        /// <value>The sort status action control.</value>
        public StatusFilterAction StatusFilterAction
        {
            get { return this.statusFilterAction; }
            set { this.statusFilterAction = value; }
        }

        /// <summary>
        /// Gets the total records. 
        /// </summary>
        /// <value>The total records.</value>
        protected override int TotalRecords
        {
            get { return this.events.TotalRecords; }
        }

        /// <summary>
        /// Gets the header container.
        /// </summary>
        /// <value>The header container.</value>
        protected override PlaceHolder HeaderContainer
        {
            get { return this.HeaderPlaceholder; }
        }

        /// <summary>
        /// Gets the footer container.
        /// </summary>
        /// <value>The footer container.</value>
        protected override PlaceHolder FooterContainer
        {
            get { return this.FooterPlaceholder; }
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
        /// Binds the data.
        /// </summary>
        public override void BindData()
        {
            string sort = "EventStart";
            if (this.sortAction != null)
            {
                sort = this.sortAction.SelectedValue;
            }

            string status = "Active";
            if (this.statusFilterAction != null)
            {
                status = this.statusFilterAction.SelectedValue;
            }

            this.events = EventCollection.Load(this.PortalId, this.listingMode, sort, this.CurrentPageIndex, this.RecordsPerPage, status.Equals("All", StringComparison.Ordinal), this.IsFeatured);
            this.RepeaterEvents.DataSource = this.events;
            this.RepeaterEvents.DataBind();
        }

        /// <summary>
        /// Gets the footer template.
        /// </summary>
        /// <returns>The footer template</returns>
        protected override Template GetFooterTemplate()
        {
            string templateName = this.FooterTemplateName.Length == 0
                                      ? Dnn.Utility.GetStringSetting(this.Settings, Framework.Setting.FooterTemplate.PropertyName)
                                      : this.FooterTemplateName;
            Template footerTemplate = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            return footerTemplate;
        }

        /// <summary>
        /// Raises the <see cref="EventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.listingMode = Dnn.Utility.GetEnumSetting(this.Settings, Setting.DisplayModeOption.PropertyName, ListingMode.All);
        }

        /// <summary>
        /// Gets the item template.
        /// </summary>
        /// <returns>The item template</returns>
        protected override Template GetItemTemplate()
        {
            string templateName = this.ItemTemplateName.Length == 0
                                      ? Dnn.Utility.GetStringSetting(this.Settings, Framework.Setting.ItemTemplate.PropertyName)
                                      : this.ItemTemplateName;
            Template itemTemplate = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            return itemTemplate;
        }

        /// <summary>
        /// Gets the header template.
        /// </summary>
        /// <returns>The header template</returns>
        protected override Template GetHeaderTemplate()
        {
            string templateName = this.HeaderTemplateName.Length == 0
                                      ? Dnn.Utility.GetStringSetting(this.Settings, Framework.Setting.HeaderTemplate.PropertyName)
                                      : this.HeaderTemplateName;
            Template headerTemplate = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            return headerTemplate;
        }

        /// <summary>
        /// Method used to process a token. This method is invoked from the TemplateEngine class. Since this control knows
        /// best on how to contruct the page. ListingHeader, ListingItem and Listing Footer templates are processed here.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="tag">The tag being processed.</param>
        /// <param name="engageObject">The engage object.</param>
        /// <param name="resourceFile">The resource file name.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "The complexity cannot easily be reduced and the method is easy to understand, test, and maintain")]
        protected override void ProcessTag(Control container, Tag tag, object engageObject, string resourceFile)
        {
            Event currentEvent = (Event)engageObject;

            if (tag.TagType == TagType.Open)
            {
                switch (tag.LocalName.ToUpperInvariant())
                {
                    case "EDITEVENTBUTTON":
                        ButtonAction editEventAction = (ButtonAction)this.LoadControl(ActionsControlsFolder + "ButtonAction.ascx");
                        editEventAction.CurrentEvent = currentEvent;
                        editEventAction.ModuleConfiguration = this.ModuleConfiguration;
                        editEventAction.Href = this.BuildLinkUrl(this.ModuleId, "EventEdit", Dnn.Events.Utility.GetEventParameters(currentEvent));
                        editEventAction.Text = Localization.GetString("EditEventButton", resourceFile);
                        container.Controls.Add(editEventAction);
                        editEventAction.Visible = this.IsAdmin;
                        break;
                    case "VIEWRESPONSESBUTTON":
                        ButtonAction responsesEventAction = (ButtonAction)this.LoadControl(ActionsControlsFolder + "ButtonAction.ascx");
                        responsesEventAction.CurrentEvent = currentEvent;
                        responsesEventAction.ModuleConfiguration = this.ModuleConfiguration;
                        responsesEventAction.Href = this.BuildLinkUrl(this.ModuleId, "ResponseDetail", Dnn.Events.Utility.GetEventParameters(currentEvent));
                        responsesEventAction.Text = Localization.GetString("ResponsesButton", resourceFile);
                        container.Controls.Add(responsesEventAction);
                        responsesEventAction.Visible = this.IsAdmin;
                        break;
                    case "REGISTERBUTTON":
                        ButtonAction registerEventAction = (ButtonAction)this.LoadControl(ActionsControlsFolder + "ButtonAction.ascx");
                        registerEventAction.CurrentEvent = currentEvent;
                        registerEventAction.ModuleConfiguration = this.ModuleConfiguration;
                        registerEventAction.Href = this.BuildLinkUrl(this.ModuleId, "Register", Dnn.Events.Utility.GetEventParameters(currentEvent));
                        registerEventAction.Text = Localization.GetString("RegisterButton", resourceFile);
                        container.Controls.Add(registerEventAction);

                        // to register must be an event that allows registrations, be active, and have not ended
                        registerEventAction.Visible = currentEvent.AllowRegistrations && !currentEvent.Cancelled && currentEvent.EventEnd > DateTime.Now;

                        break;
                    case "ADDTOCALENDARBUTTON":
                        AddToCalendarAction addToCalendarAction = (AddToCalendarAction)this.LoadControl(ActionsControlsFolder + "AddToCalendarAction.ascx");
                        addToCalendarAction.CurrentEvent = currentEvent;
                        addToCalendarAction.ModuleConfiguration = this.ModuleConfiguration;

                        // must be an active event and has not ended
                        addToCalendarAction.Visible = !currentEvent.Cancelled && currentEvent.EventEnd > DateTime.Now;
                        container.Controls.Add(addToCalendarAction);
                        break;
                    case "DELETEBUTTON":
                        DeleteAction deleteAction = (DeleteAction)this.LoadControl(ActionsControlsFolder + "DeleteAction.ascx");
                        deleteAction.CurrentEvent = currentEvent;
                        deleteAction.ModuleConfiguration = this.ModuleConfiguration;
                        container.Controls.Add(deleteAction);
                        break;
                    case "CANCELBUTTON":
                        CancelAction cancelAction = (CancelAction)this.LoadControl(ActionsControlsFolder + "CancelAction.ascx");
                        cancelAction.CurrentEvent = currentEvent;
                        cancelAction.ModuleConfiguration = this.ModuleConfiguration;
                        container.Controls.Add(cancelAction);
                        break;
                    case "EDITEMAILBUTTON":
                        ButtonAction editEmailAction = (ButtonAction)this.LoadControl(ActionsControlsFolder + "ButtonAction.ascx");
                        editEmailAction.CurrentEvent = currentEvent;
                        editEmailAction.ModuleConfiguration = this.ModuleConfiguration;
                        editEmailAction.Href = this.BuildLinkUrl(this.ModuleId, "EmailEdit", Dnn.Events.Utility.GetEventParameters(currentEvent));
                        editEmailAction.Text = Localization.GetString("EditEmailButton", resourceFile);
                        container.Controls.Add(editEmailAction);
                        editEmailAction.Visible = this.IsAdmin;
                        break;
                    case "EVENTSORT":
                        this.sortAction = (SortAction)this.LoadControl(ActionsControlsFolder + "SortAction.ascx");
                        this.sortAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.sortAction.SortChanged += this.SortStatusAction_SortChanged;
                        container.Controls.Add(this.sortAction);
                        break;
                    case "STATUSFILTER":
                        this.statusFilterAction = (StatusFilterAction)this.LoadControl(ActionsControlsFolder + "StatusFilterAction.ascx");
                        this.statusFilterAction.ModuleConfiguration = this.ModuleConfiguration;
                        this.statusFilterAction.SortChanged += this.SortStatusAction_SortChanged;
                        container.Controls.Add(this.statusFilterAction);
                        break;
                    case "PREVIOUSPAGE":
                        this.PreviousButton = new LinkButton();
                        this.PreviousButton.Text = Localization.GetString("PreviousButton", resourceFile);
                        this.PreviousButton.CssClass = tag.GetAttributeValue("CssClass");
                        this.PreviousButton.CommandName = "PreviousPage";
                        this.PreviousButton.EnableViewState = true;
                        this.PreviousButton.ToolTip = Localization.GetString(tag.GetAttributeValue("ToolTipResourceKey"), resourceFile);
                        this.PreviousButton.Click += this.PreviousButton_Click;
                        container.Controls.Add(this.PreviousButton);
                        break;
                    case "NEXTPAGE":
                        this.NextButton = new LinkButton();
                        this.NextButton.Text = Localization.GetString("NextButton", resourceFile);
                        this.NextButton.CssClass = tag.GetAttributeValue("CssClass");
                        this.NextButton.CommandName = "NextPage";
                        this.NextButton.EnableViewState = true;
                        this.NextButton.ToolTip = Localization.GetString(tag.GetAttributeValue("ToolTipResourceKey"), resourceFile);
                        this.NextButton.Click += this.NextButton_Click;
                        container.Controls.Add(this.NextButton);
                        break;
                    case "PAGER":
                        ////int cp;
                        ////string fs = Localization.GetString("Pager", LocalResourceFile);
                        ////DropDownList objDropDown = new DropDownList();
                        ////objDropDown.ID = "lnkPgHPages";
                        ////objDropDown.CssClass = oRepositoryBusinessController.GetSkinAttribute(xmlHeaderDoc, "PAGER", "CssClass", "normal");
                        ////objDropDown.Width = System.Web.UI.WebControls.Unit.Parse(oRepositoryBusinessController.GetSkinAttribute(xmlHeaderDoc, "PAGER", "Width", "75"));
                        ////objDropDown.AutoPostBack = true;
                        ////cp = 1;
                        ////while (cp < lstObjects.PageCount + 1)
                        ////{
                        ////    objdropdown.Items.Add(new ListItem(string.Format(fs, cp), cp));
                        ////    cp = cp + 1;
                        ////}

                        ////objdropdown.SelectedValue = lstObjects.CurrentPageIndex + 1;
                        ////objDropDown.SelectedIndexChanged += lnkPg_SelectedIndexChanged;
                        ////hPlaceHolder.Controls.Add(objDropDown);
                        break;
                    case "CURRENTPAGE":
                        this.CurrentPageLabel = new Label();
                        this.CurrentPageLabel.Text = (this.CurrentPageIndex + 1).ToString(CultureInfo.CurrentCulture);
                        this.CurrentPageLabel.CssClass = tag.GetAttributeValue("CssClass");
                        this.CurrentPageLabel.ToolTip = Localization.GetString("CurrentPageToolTip", resourceFile);
                        container.Controls.Add(this.CurrentPageLabel);
                        break;
                    case "PAGECOUNT":
                        this.PageCountLabel = new Label();
                        this.PageCountLabel.CssClass = tag.GetAttributeValue("CssClass");
                        container.Controls.Add(this.PageCountLabel);
                        break;
                    case "PAGEXOFY":
                        this.PageXOfYLabel = new Label();
                        this.PageXOfYLabel.CssClass = tag.GetAttributeValue("CssClass");
                        container.Controls.Add(this.PageXOfYLabel);
                        this.PageXOfYFormatTemplate = Localization.GetString(tag.GetAttributeValue("ResourceKey"), resourceFile);
                        break;
                    case "READMORE":
                        if (Engage.Utility.HasValue(currentEvent.Description))
                        {
                            HyperLink detailLink = new HyperLink();
                            detailLink.Text = Localization.GetString(tag.GetAttributeValue("ResourceKey"), resourceFile);
                            if (detailLink.Text.Length == 0)
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
    }
}