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
    public partial class RsvpSummary : ModuleBase
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

        #endregion

        #region Methods

        private void BindData()
        {
            BindData("EventStart");
        }

        private void BindData(string sortColumn)
        {
            RsvpSummaryCollection rsvps = RsvpSummaryCollection.Load(PortalId, sortColumn, CurrentPageIndex - 1, 10);
            rpSummary.DataSource = rsvps;
            rpSummary.DataBind();

            pager.TotalRecords = rsvps.TotalRecords;
            pager.PageSize = 10;
            pager.CurrentPage = CurrentPageIndex;
            pager.TabID = TabId;

        }

        protected string GetDetailUrl(object o, string status, object c)
        {

            int eventId = Convert.ToInt32(o);
            int count = Convert.ToInt32(c);
            string href = string.Empty;

            if (count > 0)
            {
                href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=RsvpDetail&eventid=" + eventId.ToString() + "&status=" + status);
            }

            return href;
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

        protected void rbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData(rbSort.SelectedValue);
        }

    }
}

;