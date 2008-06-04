//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Events;
using Engage.Dnn.Events.Util;

namespace Engage.Dnn.Events
{
    public partial class EventListingGridStyle : ModuleBase
    {
        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("Edit"), true);
        }

        protected void grdEvents_SortCommand(object source, DataGridSortCommandEventArgs e)
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

            BindData(newSort);

            grdEvents.Attributes.Add("SortColumn", e.SortExpression);
        }

        protected void grdEvents_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            LinkButton delete = (LinkButton)Engage.Utility.FindControlRecursive(grdEvents, "lnkDelete");
            base.lbDeleteEvent_OnClick(delete, e);

            BindData();
        }

        protected void grdEvents_EditCommand(object source, DataGridCommandEventArgs e)
        {
            LinkButton edit = (LinkButton)Engage.Utility.FindControlRecursive(grdEvents, "lnkEdit");
            base.lbEditEvent_OnClick(edit, e);
        }

        #endregion

        #region Methods

        private void BindData()
        {
            BindData("Title");
        }

        private void BindData(string sortColumn)
        {
            EventCollection events = EventCollection.Load(PortalId, sortColumn, CurrentPageIndex -1, grdEvents.PageSize);
            grdEvents.DataSource = events;
            grdEvents.DataBind();

            pager.TotalRecords = events.TotalRecords;
            pager.PageSize = grdEvents.PageSize;
            pager.CurrentPage = CurrentPageIndex;
            pager.TabID = TabId;

            grdEvents.Attributes.Add("SortColumn", sortColumn);
        }

        private int CurrentPageIndex
        {
            get
            {
                int index = 1;
                //Get the currentpage index from the url parameter
                if (Request.QueryString["currentpage"] != null)
                {
                    index = Convert.ToInt32(Request.QueryString["currentpage"]);
                }

                return index;
            }
        }

        #endregion

        protected void grdEvents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Event row = e.Item.DataItem as Event;
                if (row != null)
                {
                    LinkButton lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;
                    if (lnkDelete != null)
                    {
                        ClientAPI.AddButtonConfirm(lnkDelete, Localization.GetString("DeleteEvent", LocalResourceFile));
                    }
                }
            }

            
        }

        //protected override void lbEditEmail_OnClick(object sender, EventArgs e)
        //{
        //    LinkButton button = (LinkButton)sender;
        //    DataGridItem item = (DataGridItem)button.NamingContainer;

        //    int eventId = Convert.ToInt32(item.Cells[0].Text);

        //    string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EmailEdit&eventid=" + eventId.ToString());

        //    Response.Redirect(href, true);
        //}

        //protected override void lbEditEvent_OnClick(object sender, EventArgs e)
        //{
        //    int eventId = GetId(sender);
        //    string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit&eventId=" + eventId.ToString());

        //    Response.Redirect(href, true);
        //}

        //protected void lnkAddToCalendar_OnClick(object sender, EventArgs e)
        //{
        //    LinkButton button = (LinkButton)sender;
        //    DataGridItem  item = (DataGridItem) button.NamingContainer;

        //    int eventId = Convert.ToInt32(item.Cells[0].Text);
        //    Event ee = Event.Load(eventId);

        //    //Stream The vCalendar 
        //    HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding(CultureInfo.CurrentUICulture.TextInfo.ANSICodePage);
        //    HttpContext.Current.Response.ContentType = "text/x-iCalendar";
        //    HttpContext.Current.Response.AppendHeader("Content-Disposition", "filename=" + HttpUtility.UrlEncode(ee.Title) + ".vcs");
        //    HttpContext.Current.Response.Write(ee.ToICal("hkenuam@engagesoftware.com"));
        //}

        //#region IActionable Members

        //public ModuleActionCollection ModuleActions
        //{
        //    get
        //    {
        //        ModuleActionCollection Actions = new ModuleActionCollection();
        //        Actions.Add(GetNextActionID(), Localization.GetString("Title", this.LocalResourceFile), "", "", "", EditUrl(Utility.RsvpListingContainer), false, SecurityAccessLevel.Edit, true, false);
        //        return Actions;
        //    }
        //}

        //#endregion

    }
}

