// <copyright file="ResponseDetail.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Web.UI;
    using DotNetNuke.Common;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Engage.Events;
    using Telerik.Web.UI;

    /// <summary>
    /// Lists details for user event registrations
    /// </summary>
    public partial class ResponseDetail : ModuleBase
    {
        /// <summary>
        /// The default column on which to sort results
        /// </summary>
        private const string DefaultSortColumn = "CreationDate";

        /// <summary>
        /// Gets the status of responses to display, or <c>null</c> for all statuses.
        /// </summary>
        /// <value>The status of responses to display.</value>
        private string Status
        {
            get
            {
                return this.Request.QueryString["status"];
            }
        }

        /// <summary>
        /// Gets the status icon.
        /// </summary>
        /// <param name="responseStatus">An object representing the registration status</param>
        /// <returns>the url to the icon for the corresponding registration status</returns>
        protected string GetStatusIcon(object responseStatus)
        {
            if (responseStatus == null)
            {
                throw new ArgumentNullException("responseStatus", "responseStatus must not be null");
            }

            string url = string.Empty;
            ResponseStatus status = (ResponseStatus)Enum.Parse(typeof(ResponseStatus), responseStatus.ToString());

            switch (status)
            {
                case ResponseStatus.Attending:
                    return "~" + this.DesktopModuleFolderName + "Images/attending_icon.gif";
                case ResponseStatus.NotAttending:
                    return "~" + this.DesktopModuleFolderName + "Images/not_attending_icon.gif";
                case ResponseStatus.NoResponse:
                    return "~" + this.DesktopModuleFolderName + "Images/no_response_icon.gif";
            }

            return url;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!this.PermissionsService.CanViewResponses)
            {
                this.DenyAccess();
            }

            AJAX.RegisterPostBackControl(this.ExportToCsvButton);
            AJAX.RegisterPostBackControl(this.ExportToExcelButton);

            base.OnInit(e);
            this.Load += this.Page_Load;
            this.ExportToCsvButton.Click += this.ExportToCsvButton_Click;
            this.ExportToExcelButton.Click += this.ExportToExcelButton_Click;
            this.SortRadioButtonList.SelectedIndexChanged += this.SortRadioButtonList_SelectedIndexChanged;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.SetupControl();
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the <see cref="ExportToCsvButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        private void ExportToCsvButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.PrepareForExport();
                this.ResponseDetailGrid.MasterTableView.ExportToCSV();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the <see cref="ExportToExcelButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        private void ExportToExcelButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.PrepareForExport();
                this.ResponseDetailGrid.MasterTableView.ExportToExcel();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the <see cref="SortRadioButtonList"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData(this.SortRadioButtonList.SelectedValue);
        }

        /// <summary>
        /// Sets up this control.  Sets localization and sets the NavigateUrl for the <see cref="CancelGoHomeLink"/>.
        /// </summary>
        private void SetupControl()
        {
            this.LocalizeGridHeaders();
            this.CancelGoHomeLink.NavigateUrl = Globals.NavigateURL();
            this.CancelGoHomeImage.AlternateText = this.Localize("CancelGoHomeLink.Alt");
            this.ExportToCsvButton.AlternateText = this.Localize("Export To CSV.Alt");
            this.ExportToExcelButton.AlternateText = this.Localize("Export To Excel.Alt");
        }

        /// <summary>
        /// Localizes the header text for each column in the <see cref="ResponseDetailGrid"/>.
        /// </summary>
        private void LocalizeGridHeaders()
        {
            foreach (GridColumn column in this.ResponseDetailGrid.Columns)
            {
                if (!string.IsNullOrEmpty(column.HeaderText))
                {
                    string localizedHeaderText = Localization.GetString(column.HeaderText + ".Header", this.LocalResourceFile);
                    if (!string.IsNullOrEmpty(localizedHeaderText))
                    {
                        column.HeaderText = localizedHeaderText;
                    }
                }
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            this.BindData(DefaultSortColumn);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        private void BindData(string sortColumn)
        {
            this.BindData(sortColumn, false);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="getAll">if set to <c>true</c> gets all responses, regardless of page size or number; otherwise only gets one page of responses.</param>
        private void BindData(bool getAll)
        {
            this.BindData(DefaultSortColumn, getAll);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="getAll">if set to <c>true</c> gets all responses, regardless of page size or number; otherwise only gets one page of responses.</param>
        private void BindData(string sortColumn, bool getAll)
        {
            int? eventId = this.EventId;
            if (!eventId.HasValue)
            {
                return;
            }

            var responseSummary = Engage.Events.ResponseSummary.Load(eventId.Value, this.EventStart);
            if (!this.CanShowEvent(responseSummary.Event))
            {
                return;
            }

            int pageIndex = getAll ? 0 : this.CurrentPageIndex - 1;
            int pageSize = getAll ? 0 : this.ResponseDetailGrid.PageSize;
            ResponseCollection responses = ResponseCollection.Load(eventId.Value, this.EventStart, this.Status, sortColumn, pageIndex, pageSize, this.CategoryIds);
            this.ResponseDetailGrid.DataSource = responses;
            this.ResponseDetailGrid.DataBind();
            ////this.ResponseDetailGrid.Attributes.Add("SortColumn", sortColumn);

            this.pager.PageSize = this.ResponseDetailGrid.PageSize;
            this.SetupPagingControl(this.pager, responses.TotalRecords, "modId", "key", "status", "eventId", "start");

            this.responseDisplay.SetResponseSummary(responseSummary);
            this.responseDisplay.ModuleConfiguration = this.ModuleConfiguration;
        }

        /// <summary>
        /// Prepares the grid to be exported to a file.
        /// </summary>
        private void PrepareForExport()
        {
            this.BindData(true);

            this.ResponseDetailGrid.Columns.FindByUniqueName("Status").Visible = false;
            this.ResponseDetailGrid.Columns.FindByUniqueName("ExportStatus").Visible = true;

            Event currentEvent = this.EventId.HasValue ? Event.Load(this.EventId.Value) : null;
            if (currentEvent != null && !this.CanShowEvent(currentEvent))
            {
                currentEvent = null;
            }

            this.ResponseDetailGrid.ExportSettings.FileName = string.Format(
                CultureInfo.CurrentCulture,
                Localization.GetString("Export Filename.Text", this.LocalResourceFile),
                currentEvent == null ? null : currentEvent.Title,
                this.EventStart);
        }
    }
}