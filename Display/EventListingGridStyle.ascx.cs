// <copyright file="EventListingGridStyle.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;
    using Engage.Events;

    /// <summary>
    /// THIS CLASS IS CURRENTLY NO LONGER USED BUT KEPT FOR CODE USED FOR PAGING SORTING AS AN EXAMPLE hk
    /// </summary>
    public partial class EventListingGridStyle : ModuleBase
    {
        /// <summary>
        /// Gets the index of the current page.
        /// </summary>
        /// <value>The index of the current page.</value>
        private int CurrentPageIndex
        {
            get
            {
                int index = 1;

                // Get the currentpage index from the url parameter
                if (this.Request.QueryString["currentpage"] != null)
                {
                    index = Convert.ToInt32(this.Request.QueryString["currentpage"]);
                }

                return index;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }

        /// <summary>
        /// Handles the Click event of the btnNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BtnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("Edit"), true);
        }

        /// <summary>
        /// Handles the SortCommand event of the grdEvents control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridSortCommandEventArgs"/> instance containing the event data.</param>
        protected void GrdEvents_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string sort = grdEvents.Attributes["SortColumn"];
            string newSort;
            string direction = grdEvents.Attributes["SortDirection"];

            if (sort != null && sort == e.SortExpression)
            {
                newSort = e.SortExpression + " DESC";
            }
            else
            {
                newSort = e.SortExpression + " ASC";
            }

            this.BindData(newSort);

            grdEvents.Attributes.Add("SortColumn", e.SortExpression);
        }

        /// <summary>
        /// Handles the ItemDataBound event of the grdEvents control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void GrdEvents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Event row = e.Item.DataItem as Event;
                if (row != null)
                {
                    LinkButton lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;
                    if (lnkDelete != null)
                    {
                        ClientAPI.AddButtonConfirm(lnkDelete, Localization.GetString("DeleteEvent", this.LocalResourceFile));
                    }
                }
            }
        }

        ////protected void grdEvents_DeleteCommand(object source, DataGridCommandEventArgs e)
        ////{
        ////    LinkButton delete = (LinkButton)Engage.Utility.FindControlRecursive(grdEvents, "lnkDelete");
        ////    base.DeleteEventButton_Click(delete, e);

        ////    BindData();
        ////}

        ////protected void grdEvents_EditCommand(object source, DataGridCommandEventArgs e)
        ////{
        ////    LinkButton edit = (LinkButton)Engage.Utility.FindControlRecursive(grdEvents, "lnkEdit");
        ////    base.EditEventButton_Click(edit, e);
        ////}

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            this.BindData("Title");
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        private void BindData(string sortColumn)
        {
            EventCollection events = EventCollection.Load(PortalId, sortColumn, this.CurrentPageIndex - 1, grdEvents.PageSize, false);
            grdEvents.DataSource = events;
            grdEvents.DataBind();

            pager.TotalRecords = events.TotalRecords;
            pager.PageSize = grdEvents.PageSize;
            pager.CurrentPage = this.CurrentPageIndex;
            pager.TabID = TabId;

            grdEvents.Attributes.Add("SortColumn", sortColumn);
        }
    }
}