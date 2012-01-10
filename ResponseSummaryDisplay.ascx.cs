// <copyright file="ResponseSummaryDisplay.ascx.cs" company="Engage Software">
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
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;

    using Engage.Events;

    using Telerik.Web.UI;

    /// <summary>
    /// Displays a summary of who has and hasn't responded for events.
    /// </summary>
    public partial class ResponseSummaryDisplay : ModuleBase
    {
        /// <summary>
        /// Gets the index of the current page.
        /// </summary>
        /// <value>The index of the current page.</value>
        protected override int CurrentPageIndex
        {
            get
            {
                int index;
                if (!int.TryParse(this.Request.QueryString["currentpage"], NumberStyles.Integer, CultureInfo.InvariantCulture, out index))
                {
                    index = 1;
                }

                return index;
            }
        }

        /// <summary>
        /// Gets the index at which the list of responses should start (based on page number).
        /// </summary>
        /// <value>The start index of the responses list.</value>
        protected int StartIndex
        {
            get 
            {
                return ((this.CurrentPageIndex - 1) * this.SummaryPager.PageSize) + 1;
            }
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!this.PermissionsService.CanViewResponses)
            {
                this.DenyAccess();
            }

            base.OnInit(e);
            this.Load += this.Page_Load;
            this.SortRadioButtonList.SelectedIndexChanged += this.SortRadioButtonList_SelectedIndexChanged;
            this.SummaryRepeater.ItemDataBound += this.SummaryRepeater_ItemDataBound;
            this.ExportToExcelButton.Click += this.ExportToExcelButton_Click;
            this.ExportToCsvButton.Click += this.ExportToCsvButton_Click;

            AJAX.RegisterPostBackControl(this.ExportToCsvButton);
            AJAX.RegisterPostBackControl(this.ExportToExcelButton);
        }

        /// <summary>
        /// Handles the <see cref="ImageButton.Click"/> event of the <see cref="ExportToExcelButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        private void ExportToExcelButton_Click(object sender, ImageClickEventArgs e)
        {
            this.ExportResponses(ExportFormat.Excel);
        }

        /// <summary>
        /// Handles the <see cref="ImageButton.Click"/> event of the <see cref="ExportToCsvButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        private void ExportToCsvButton_Click(object sender, ImageClickEventArgs e)
        {
            this.ExportResponses(ExportFormat.Csv);
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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
        /// Handles the ItemDataBound event of the SummaryRepeater control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        private void SummaryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var responseDisplay = (Display.ResponseDisplay)e.Item.FindControl("ResponseDisplay");
                responseDisplay.SetResponseSummary((ResponseSummary)e.Item.DataItem);
                responseDisplay.ModuleConfiguration = this.ModuleConfiguration;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the SortRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData(this.SortRadioButtonList.SelectedValue);
        }

        /// <summary>
        /// Sets up this control.  Sets localization and sets the <c>NavigateUrl</c> for the cancel and go home button.
        /// </summary>
        private void SetupControl()
        {
            this.LocalizeGridHeaders();
            this.CancelGoHomeImage.AlternateText = this.Localize("CancelGoHomeLink.Alt");
            this.CancelGoHomeLink.NavigateUrl = Globals.NavigateURL();

            this.ExportToExcelButton.AlternateText = this.Localize("ExportToExcel.Alt");
            this.ExportToExcelButton.ToolTip = this.Localize("ExportToExcel.ToolTip");
            this.ExportToCsvButton.AlternateText = this.Localize("ExportToCsv.Alt");
            this.ExportToCsvButton.ToolTip = this.Localize("ExportToCsv.ToolTip");
        }

        /// <summary>
        /// Localizes the header text for each column in the <see cref="ReportExportGrid"/>.
        /// </summary>
        private void LocalizeGridHeaders()
        {
            foreach (GridColumn column in this.ReportExportGrid.Columns)
            {
                if (string.IsNullOrEmpty(column.HeaderText))
                {
                    continue;
                }

                var localizedHeaderText = this.Localize(column.HeaderText + ".Header");
                if (!string.IsNullOrEmpty(localizedHeaderText))
                {
                    column.HeaderText = localizedHeaderText;
                }
            }
        }

        /// <summary>
        /// Exports the responses report.
        /// </summary>
        /// <param name="format">The format of the export.</param>
        private void ExportResponses(ExportFormat format)
        {
            this.ReportExportGrid.Visible = true;
            this.ReportExportGrid.ExportSettings.FileName = this.Localize("ReportFileName.Text");
            if (format == ExportFormat.Excel)
            {
                this.ReportExportGrid.MasterTableView.ExportToExcel();
            }
            else
            {
                this.ReportExportGrid.MasterTableView.ExportToCSV();
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        private void BindData(string sortColumn = "EventStart")
        {
            var responses = ResponseSummaryCollection.Load(this.PortalId, sortColumn, this.CurrentPageIndex - 1, this.SummaryPager.PageSize, this.CategoryIds);
            this.SummaryRepeater.DataSource = responses;
            this.SummaryRepeater.DataBind();

            this.BindReport();

            this.SetupPagingControl(this.SummaryPager, responses.TotalRecords, "modId", "key");
            this.NoResponsesMessageLabel.Visible = !this.SummaryPager.Visible;
        }

        /// <summary>
        /// Binds the <see cref="ReportExportGrid"/>.
        /// </summary>
        private void BindReport()
        {
            var reportReader = ResponseCollection.LoadReport(DateTime.Now, null, this.CategoryIds);
            var categoryNameColumnIndex = reportReader.GetOrdinal("CategoryName");
            var eventTitleColumnIndex = reportReader.GetOrdinal("EventTitle");
            var eventStartColumnIndex = reportReader.GetOrdinal("EventStart");
            var eventEndColumnIndex = reportReader.GetOrdinal("EventEnd");
            var firstNameColumnIndex = reportReader.GetOrdinal("FirstName");
            var lastNameColumnIndex = reportReader.GetOrdinal("LastName");
            var emailColumnIndex = reportReader.GetOrdinal("Email");
            var responseDateColumnIndex = reportReader.GetOrdinal("ResponseDate");
            var statusColumnIndex = reportReader.GetOrdinal("Status");
            this.ReportExportGrid.DataSource = reportReader.AsEnumerable().Select(row => new
                {
                    CategoryName = string.IsNullOrEmpty(row.GetString(categoryNameColumnIndex)) ? this.GetDefaultCategoryName() : row.GetString(categoryNameColumnIndex),
                    EventTitle = row.GetString(eventTitleColumnIndex),
                    EventStart = row.GetDateTime(eventStartColumnIndex),
                    EventEnd = row.GetDateTime(eventEndColumnIndex),
                    ResponderName = row.GetString(firstNameColumnIndex) + " " + row.GetString(lastNameColumnIndex),
                    Email = row.GetString(emailColumnIndex),
                    ResponseDate = row.GetDateTime(responseDateColumnIndex),
                    Status = this.Localize(row.GetString(statusColumnIndex)),
                });
            this.ReportExportGrid.DataBind();
        }
    }
}