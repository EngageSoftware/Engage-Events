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
                 BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }

        //protected void rbSort_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindData();
        //}

        //protected void rbStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindData();
        //}

        private void actions_ActionCompleted(object sender, ActionEventArg e)
        {
            if (e.ActionStatus == Action.Success)
            {
                BindData();
            }
        }

        protected void rpEventListing_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EventAdminActions actions = (EventAdminActions)e.Item.FindControl("ccEventActions");
            actions.DataItem = (Event)e.Item.DataItem;
            actions.ActionCompleted += new ActionEventHandler(actions_ActionCompleted);
        }

        #endregion

        #region Methods

        private void BindData()
        {
            bool showAll = false;
            if (rbStatus.SelectedValue == "All") showAll = true;
            EventCollection events = EventCollection.Load(PortalId, rbSort.SelectedValue, 0, 0, showAll);
            rpEventListing.DataSource = events;
            rpEventListing.DataBind();
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
    }
}

