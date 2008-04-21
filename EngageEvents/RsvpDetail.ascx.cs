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
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Events;
using Engage.Dnn.Events.Util;

namespace Engage.Dnn.Events
{
    public partial class RsvpDetail : ModuleBase
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

        protected void lbSettings_OnClick(object sender, EventArgs e)
        {

        }

        protected void lbMyEvents_OnClick(object sender, EventArgs e)
        {

        }

        protected void lbAddEvents_OnClick(object sender, EventArgs e)
        {

        }

        protected void lbManageEmail_OnClick(object sender, EventArgs e)
        {

        }

        #endregion

        #region Methods

        private void BindData()
        {
            BindData("FirstName");
        }

        private void BindData(string sortColumn)
        {
            RsvpCollection rsvps = RsvpCollection.Load(EventId, Status, sortColumn, CurrentPageIndex - 1, grdRsvpDetail.PageSize);
            grdRsvpDetail.DataSource = rsvps;
            grdRsvpDetail.DataBind();

            pager.TotalRecords = rsvps.TotalRecords;
            pager.PageSize = grdRsvpDetail.PageSize;
            pager.CurrentPage = CurrentPageIndex;
            pager.TabID = TabId;
            pager.QuerystringParams = "&mid=" + ModuleId.ToString() + "&key=rsvpDetail&status=" + Status + "&eventid=" + EventId;
            grdRsvpDetail.Attributes.Add("SortColumn", sortColumn);

            Event e = Event.Load(EventId);
            lblDate.Text= e.EventStartLongFormatted;
            lblName.Text= e.Title;
        }

        protected string GetStatusIcon(object o)
        {

            string url = string.Empty;
            RsvpStatus status = (RsvpStatus)Enum.Parse(typeof(RsvpStatus), o.ToString());

            switch (status)
            {
                case RsvpStatus.Attending:
                    return "~/desktopmodules/engageevents/Images/yes.gif";
                case RsvpStatus.NotAttending:
                    return "~/desktopmodules/engageevents/Images/no.gif";
                case RsvpStatus.NoResponse:
                    return "~/desktopmodules/engageevents/Images/noresponse.gif";
            }

            return url;
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

        private string Status
        {
            get
            {
                string status = "";
                //Get the currentpage index from the url parameter
                if (Request.QueryString["status"] != null)
                {
                    status = Request.QueryString["status"].ToString();
                }

                return status;
            }

        }


        #endregion

        protected void rbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData(rbSort.SelectedValue);
        }

    }
}

;