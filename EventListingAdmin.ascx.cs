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
using System.ComponentModel;
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
//using Engage.Licensing;

namespace Engage.Dnn.Events
{
    //[System.Runtime.InteropServices.GuidAttribute("2de915e1-df71-3443-9f4d-32259c92ced2")]
    //[LicenseProvider(typeof(EngageLicenseProvider))]
    public partial class EventListingAdmin : ModuleBase
    {
        private License license;

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                //adds Validate to the control's constructor.
                //license = LicenseManager.Validate(typeof(EventListingAdmin), this);
                if (!Page.IsPostBack)
                {
                    BindData("EventStart", rbStatus.SelectedValue);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }

        protected void lbDelete_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            Event.Delete(eventId);

            string selectedSort = rbSort.SelectedValue;
            BindData(selectedSort, rbStatus.SelectedValue);
        }

        protected void lbCancel_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            Event ev = Event.Load(eventId);
            ev.Cancelled = !ev.Cancelled;
            ev.Save(UserId);

            BindData(rbSort.SelectedValue, rbStatus.SelectedValue);
        }

        protected void rbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData(rbSort.SelectedValue, rbStatus.SelectedValue);
        }

        protected void rbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData(rbSort.SelectedValue, rbStatus.SelectedValue);
        }
        
        #endregion

        #region Methods

        private void BindData(string sortColumn, string status)
        {
            bool showAll = false;
            if (status == "All") showAll = true;
            EventCollection events = EventCollection.Load(PortalId, sortColumn, 0, 0, showAll);
            rpEventListing.DataSource = events;
            rpEventListing.DataBind();
        }

        protected string GetActionText(object cancelled)
        {
            bool b = (bool)cancelled;
            string cancelText = Localization.GetString("Cancel", LocalResourceFile);
            if (b == true)
            {
                cancelText = Localization.GetString("UnCancel", LocalResourceFile);
            }
            return cancelText;

        }
        #endregion

        public override void Dispose()
        {
            if (license != null)
            {
                license.Dispose();
                license = null;
            }
            base.Dispose();
        }

        protected void rpEventListing_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton lnkDelete = (LinkButton) e.Item.FindControl("lbDelete");
            lnkDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("ConfirmDelete", LocalResourceFile) + "');");

            LinkButton lnkCancel = (LinkButton)e.Item.FindControl("lbCancel");
            Event ev = (Event)e.Item.DataItem;
            if (ev.Cancelled)
            {
                lnkCancel.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("ConfirmUnCancel", LocalResourceFile) + "');");
            }
            else
            {
                lnkCancel.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("ConfirmCancel", LocalResourceFile) + "');");
            }
        }
      
    }
}

