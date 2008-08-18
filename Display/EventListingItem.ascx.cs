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
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Localization;
    using Engage.Events;
    using Framework.Templating;
    using Templating;
    using Utility = Engage.Utility;

    /// <summary>
    /// Custom event listing item
    /// </summary>
    public partial class EventListingItem : RepeaterItemListing
    {
        /// <summary>
        /// Backing field for <see cref="SortAction"/>
        /// </summary>
        private SortAction sortAction;

        /// <summary>
        /// Backing field for <see cref="SortStatusAction"/>
        /// </summary>
        private SortStatusAction sortStatusAction;

        /// <summary>
        /// Backing field for <see cref="ListingMode"/>
        /// </summary>
        private ListingMode listingMode;

        /// <summary>
        /// The collection of events to display
        /// </summary>
        private EventCollection events; // keep a reference around of the data that you have loaded

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
        public SortStatusAction SortStatusAction
        {
            get { return this.sortStatusAction; }
            set { this.sortStatusAction = value; }
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
            get { return this.PlaceHolderHeader; }
        }

        /// <summary>
        /// Gets the footer container.
        /// </summary>
        /// <value>The footer container.</value>
        protected override PlaceHolder FooterContainer
        {
            get { return this.PlaceHolderFooter; }
        }

        /// <summary>
        /// Sets the listing mode used for this display.
        /// </summary>
        /// <param name="listingMode">The listing mode used for this display.</param>
        public void SetListingMode(string listingMode)
        {
            if (!string.IsNullOrEmpty(listingMode))
            {
                this.ListingMode = (ListingMode)Enum.Parse(typeof(ListingMode), listingMode, true);
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

            string statusSort = "Active";
            if (this.sortStatusAction != null)
            {
                statusSort = this.sortStatusAction.SelectedValue;
            }

            this.events = EventCollection.Load(this.PortalId, this.listingMode, sort, this.CurrentPageIndex, this.RecordsPerPage, statusSort == "All", this.IsFeatured);
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
            // we must set the local resource file first since process will occur before we define the LocalResourceFile
            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + Localization.LocalResourceDirectory + "/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);
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
        protected override void ProcessTag(Control container, Tag tag, object engageObject, string resourceFile)
        {
            Event ev = (Event)engageObject;

            switch (tag.LocalName.ToUpperInvariant())
            {
                case "EDITEVENTBUTTON":
                    ButtonAction editEventAction = (ButtonAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/ButtonAction.ascx");
                    editEventAction.CurrentEvent = ev;
                    editEventAction.ModuleConfiguration = this.ModuleConfiguration;
                    editEventAction.Href = this.BuildLinkUrl(this.ModuleId, "EventEdit", "eventId=" + ev.Id.ToString(CultureInfo.InvariantCulture));
                    editEventAction.Text = Localization.GetString("EditEventButton", "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions");
                    container.Controls.Add(editEventAction);
                    editEventAction.Visible = this.IsAdmin;
                    break;
                case "VIEWRESPONSESBUTTON":
                    ButtonAction responsesEventAction = (ButtonAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/ButtonAction.ascx");
                    responsesEventAction.CurrentEvent = ev;
                    responsesEventAction.ModuleConfiguration = this.ModuleConfiguration;
                    responsesEventAction.Href = this.BuildLinkUrl(this.ModuleId, "RsvpDetail", "eventid=" + ev.Id.ToString(CultureInfo.InvariantCulture));
                    responsesEventAction.Text = Localization.GetString("ResponsesButton", "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions");
                    container.Controls.Add(responsesEventAction);
                    responsesEventAction.Visible = this.IsAdmin;
                    break;
                case "REGISTERBUTTON":
                    ButtonAction registerEventAction = (ButtonAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/ButtonAction.ascx");
                    registerEventAction.CurrentEvent = ev;
                    registerEventAction.ModuleConfiguration = this.ModuleConfiguration;
                    registerEventAction.Href = this.BuildLinkUrl(this.ModuleId, "Register", "eventid=" + ev.Id.ToString(CultureInfo.InvariantCulture));
                    registerEventAction.Text = Localization.GetString("RegisterButton", "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions");
                    container.Controls.Add(registerEventAction);

                    // to register must be an event that allows registrations, be active, and have not ended
                    registerEventAction.Visible = ev.AllowRegistrations && !ev.Cancelled && ev.EventEnd > DateTime.Now;

                    break;
                case "ADDTOCALENDARBUTTON":
                    AddToCalendarAction addToCalendarAction =
                        (AddToCalendarAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/AddToCalendarAction.ascx");
                    addToCalendarAction.CurrentEvent = ev;
                    addToCalendarAction.ModuleConfiguration = this.ModuleConfiguration;

                    // must be an active event and has not ended
                    addToCalendarAction.Visible = ev.Cancelled == false && ev.EventEnd > DateTime.Now;
                    container.Controls.Add(addToCalendarAction);
                    break;
                case "DELETEBUTTON":
                    DeleteAction deleteAction = (DeleteAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/DeleteAction.ascx");
                    deleteAction.CurrentEvent = ev;
                    deleteAction.ModuleConfiguration = this.ModuleConfiguration;
                    container.Controls.Add(deleteAction);
                    break;
                case "CANCELBUTTON":
                    CancelAction cancelAction = (CancelAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/CancelAction.ascx");
                    cancelAction.CurrentEvent = ev;
                    cancelAction.ModuleConfiguration = this.ModuleConfiguration;
                    container.Controls.Add(cancelAction);
                    break;
                case "EDITEMAILBUTTON":
                    ButtonAction editEmailAction = (ButtonAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/ButtonAction.ascx");
                    editEmailAction.CurrentEvent = ev;
                    editEmailAction.ModuleConfiguration = this.ModuleConfiguration;
                    editEmailAction.Href = this.BuildLinkUrl(this.ModuleId, "EmailEdit", "eventid=" + ev.Id.ToString(CultureInfo.InvariantCulture));
                    editEmailAction.Text = Localization.GetString("EditEmailButton", "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions");
                    container.Controls.Add(editEmailAction);
                    editEmailAction.Visible = this.IsAdmin;
                    break;
                case "SORTEVENTBYDATE":
                    this.sortAction = (SortAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/SortAction.ascx");
                    this.sortAction.ModuleConfiguration = this.ModuleConfiguration;
                    this.sortAction.SortChanged += this.SortStatusAction_SortChanged;
                    container.Controls.Add(this.sortAction);
                    break;
                case "SORTEVENTBYSTATUS":
                    this.sortStatusAction = (SortStatusAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/SortStatusAction.ascx");
                    this.sortStatusAction.ModuleConfiguration = this.ModuleConfiguration;
                    this.sortStatusAction.SortChanged += this.SortStatusAction_SortChanged;
                    container.Controls.Add(this.sortStatusAction);
                    break;
                case "PREVIOUSPAGE":
                    this.PreviousButton = new LinkButton();
                    this.PreviousButton.Text = Localization.GetString("PreviousButton", this.LocalResourceFile);
                    this.PreviousButton.CssClass = tag.GetAttributeValue("CssClass");
                    this.PreviousButton.CommandName = "PreviousPage";
                    this.PreviousButton.EnableViewState = true;
                    this.PreviousButton.ToolTip = Localization.GetString(tag.GetAttributeValue("ToolTipResourceKey"), this.LocalResourceFile);
                    this.PreviousButton.Click += this.PreviousButton_Click;
                    container.Controls.Add(this.PreviousButton);
                    break;
                case "NEXTPAGE":
                    this.NextButton = new LinkButton();
                    this.NextButton.Text = Localization.GetString("NextButton", this.LocalResourceFile);
                    this.NextButton.CssClass = tag.GetAttributeValue("CssClass");
                    this.NextButton.CommandName = "NextPage";
                    this.NextButton.EnableViewState = true;
                    this.NextButton.ToolTip = Localization.GetString(tag.GetAttributeValue("ToolTipResourceKey"), this.LocalResourceFile);
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
                    this.CurrentPageLabel.Text = (this.CurrentPageIndex + 1).ToString();
                    this.CurrentPageLabel.CssClass = tag.GetAttributeValue("CssClass");
                    this.CurrentPageLabel.ToolTip = Localization.GetString("CurrentPageToolTip", this.LocalResourceFile);
                    container.Controls.Add(this.CurrentPageLabel);
                    break;
                case "PAGECOUNT":
                    this.PageCountLabel = new Label();
                    this.PageCountLabel.CssClass = tag.GetAttributeValue("CssClass");
                    container.Controls.Add(this.PageCountLabel);
                    break;
                case "READMORE":
                    if (Utility.HasValue(ev.Description))
                    {
                        HyperLink detailLink = new HyperLink();
                        detailLink.Text = Localization.GetString(tag.GetAttributeValue("ResourceKey"), this.LocalResourceFile);
                        if (detailLink.Text.Length == 0)
                        {
                            detailLink.Text = "Read More...";
                        }

                        detailLink.CssClass = tag.GetAttributeValue("CssClass");
                        detailLink.NavigateUrl = this.BuildLinkUrl(this.ModuleId, "EventDetail", "eventid=" + ev.Id.ToString(CultureInfo.InvariantCulture));

                        container.Controls.Add(detailLink);
                    }

                    break;
                case "RECURRENCESUMMARY":
                    container.Controls.Add(new LiteralControl(Util.Utility.GetRecurrenceSummary(ev.RecurrenceRule)));
                    break;
                default:
                    break;
            }
        }
    }
}
