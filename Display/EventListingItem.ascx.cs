// <copyright file="EventListing.ascx.cs" company="Engage Software">
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
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Localization;
    using Framework.Templating;
    using Engage.Events;
    using Templating;
    using Setting=Setting;

    /// <summary>
    /// Custom event listing item
    /// </summary>
    public partial class EventListingItem : RepeaterItemListing
    {
        private ListingMode listingMode;
        private EventCollection events; //keep a reference around of the data that you have loaded
        protected SortStatusAction sortStatusAction;
        protected SortAction sortAction;

        /// <summary>
        /// Raises the <see cref="EventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //we must set the local resource file first since process will occur before we define the LocalResourceFile
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "Display/App_LocalResources/EventListingItem.ascx.resx";
            base.OnInit(e);

            string setting = this.mode.Length == 0 ? Utility.GetStringSetting(Settings, Setting.DisplayModeOption.PropertyName) : this.mode;
            this.listingMode = (ListingMode)Enum.Parse(typeof(ListingMode), setting);

        }

        /// <summary>
        /// Gets the item template.
        /// </summary>
        /// <returns></returns>
        protected override Template GetItemTemplate()
        {
            string templateName = this.itemTemplateName.Length == 0 ? Utility.GetStringSetting(Settings, Framework.Setting.ItemTemplate.PropertyName) : this.itemTemplateName;
            Template itemTemplate = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            return itemTemplate;
        }

        /// <summary>
        /// Processes the header.
        /// </summary>
        /// <returns></returns>
        protected override Template GetHeaderTemplate()
        {
            string templateName = this.headerTemplateName.Length == 0 ? Utility.GetStringSetting(Settings, Framework.Setting.HeaderTemplate.PropertyName) : this.headerTemplateName;
            Template headerTemplate = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            return headerTemplate;
        }

        /// <summary>
        /// Processes the footer.
        /// </summary>
        protected override Template GetFooterTemplate()
        {
            string templateName = this.footerTemplateName.Length == 0 ? Utility.GetStringSetting(Settings, Framework.Setting.FooterTemplate.PropertyName) : this.footerTemplateName;
            Template footerTemplate = TemplateEngine.GetTemplate(PhysicialTemplatesFolderName, templateName);
            return footerTemplate;
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        public override  void BindData()
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

            int pageSize;
            //need to make sure that paging got rendered. if not, get all records
            if (this.PreviousButton == null || this.NextButton == null)
            {
                pageSize = 0;
            }
            else
            {
                pageSize = RecordsPerPage;
            }
            
            this.events = EventCollection.Load(PortalId, this.listingMode, sort, this.CurrentPageIndex, pageSize, statusSort == "All", this.IsFeatured);
            RepeaterEvents.DataSource = events;
            RepeaterEvents.DataBind();
        }

        /// <summary>
        /// Gets the total records. 
        /// </summary>
        /// <value>The total records.</value>
        protected override int TotalRecords
        {
            get
            {
                return this.events.TotalRecords;
            }
        }

        private string mode = string.Empty;
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public string Mode
        {
            get { return this.mode; }
            set { this.mode = value; }

        }


        /// <summary>
        /// Method used to process a token. This method is invoked from the TemplateEngine class. Since this control knows
        /// best on how to contruct the page. ListingHeader, ListingItem and Listing Footer templates are processed here.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="engageObject">The engage object.</param>
        /// <param name="resourceFile">The resource file name.</param>
        protected override void ProcessTag(Control container, Tag tag, object engageObject, string resourceFile)
        {
            Event ev = (Event)engageObject;

            string href;

            switch (tag.LocalName.ToUpperInvariant())
            {
                case "EDITEVENTBUTTON":
                    ButtonAction editEventAction = (ButtonAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/ButtonAction.ascx");
                    editEventAction.CurrentEvent = ev;
                    editEventAction.ModuleConfiguration = this.ModuleConfiguration;
                    href = this.BuildLinkUrl("&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit&eventId=" + ev.Id.ToString(CultureInfo.InvariantCulture));
                    editEventAction.Href = href;
                    editEventAction.Text = Localization.GetString("EditEventButton", "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions");
                    container.Controls.Add(editEventAction);
                    break;
                case "VIEWRESPONSESBUTTON":
                    ButtonAction responsesEventAction = (ButtonAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/ButtonAction.ascx");
                    responsesEventAction.CurrentEvent = ev;
                    responsesEventAction.ModuleConfiguration = this.ModuleConfiguration;
                    href = this.BuildLinkUrl("&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=RsvpDetail&eventid=" + ev.Id.ToString(CultureInfo.InvariantCulture));
                    responsesEventAction.Href = href;
                    responsesEventAction.Text = Localization.GetString("ResponsesButton", "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions");
                    container.Controls.Add(responsesEventAction);
                    break;
                case "REGISTERBUTTON":
                    ButtonAction registerEventAction = (ButtonAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/ButtonAction.ascx");
                    registerEventAction.CurrentEvent = ev;
                    registerEventAction.ModuleConfiguration = this.ModuleConfiguration;
                    href = this.BuildLinkUrl("&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Register&eventid=" + ev.Id.ToString(CultureInfo.InvariantCulture));
                    registerEventAction.Href = href;
                    registerEventAction.Text = Localization.GetString("RegisterButton", "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions");
                    container.Controls.Add(registerEventAction);
                    break;
                case "ADDTOCALENDARBUTTON":
                    AddToCalendarAction addToCalendarAction = (AddToCalendarAction)this.LoadControl("~" + DesktopModuleFolderName + "Actions/AddToCalendarAction.ascx");
                    addToCalendarAction.CurrentEvent = ev;
                    addToCalendarAction.ModuleConfiguration = this.ModuleConfiguration;
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
                    href = this.BuildLinkUrl("&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EmailEdit&eventid=" + ev.Id.ToString(CultureInfo.InvariantCulture));
                    editEmailAction.Href = href;
                    editEmailAction.Text = Localization.GetString("EditEmailButton", "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions");
                    container.Controls.Add(editEmailAction);
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
                    //int cp;
                    //string fs = Localization.GetString("Pager", LocalResourceFile);
                    //DropDownList objDropDown = new DropDownList();
                    //objDropDown.ID = "lnkPgHPages";
                    //objDropDown.CssClass = oRepositoryBusinessController.GetSkinAttribute(xmlHeaderDoc, "PAGER", "CssClass", "normal");
                    //objDropDown.Width = System.Web.UI.WebControls.Unit.Parse(oRepositoryBusinessController.GetSkinAttribute(xmlHeaderDoc, "PAGER", "Width", "75"));
                    //objDropDown.AutoPostBack = true;
                    //cp = 1;
                    //while (cp < lstObjects.PageCount + 1)
                    //{
                    //    objdropdown.Items.Add(new ListItem(string.Format(fs, cp), cp));
                    //    cp = cp + 1;
                    //}

                    //objdropdown.SelectedValue = lstObjects.CurrentPageIndex + 1;
                    //objDropDown.SelectedIndexChanged += lnkPg_SelectedIndexChanged;
                    //hPlaceHolder.Controls.Add(objDropDown);
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
                    if (Engage.Utility.HasValue(ev.Description))
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
                default:
                    break;
            }

        }

        protected override PlaceHolder HeaderContainer
        {
            get { return this.PlaceHolderHeader; }
        }

        protected override PlaceHolder FooterContainer
        {
            get { return this.PlaceHolderFooter; }
        }

    }
}

