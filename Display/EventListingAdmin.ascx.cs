// <copyright file="EventListingAdmin.ascx.cs" company="Engage Software">
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
    using Engage.Events;

    /// <summary>
    /// The administrative listing of events.
    /// </summary>
    ////[System.Runtime.InteropServices.GuidAttribute("2de915e1-df71-3443-9f4d-32259c92ced2")]
    ////[LicenseProvider(typeof(EngageLicenseProvider))]
    public partial class EventListingAdmin : ModuleBase
    {
        ////private License license;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                // adds Validate to the control's constructor.
                ////license = LicenseManager.Validate(typeof(EventListingAdmin), this);
                this.BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }

        ////protected void rbSort_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    BindData();
        ////}

        ////protected void rbStatus_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    BindData();
        ////}

        ////private void actions_ActionCompleted(object sender, ActionEventArg e)
        ////{
        ////    if (e.ActionStatus == Action.Success)
        ////    {
        ////        BindData();
        ////    }
        ////}

        /// <summary>
        /// Handles the ItemDataBound event of the rpEventListing control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void RpEventListing_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EventAdminActions actions = (EventAdminActions)e.Item.FindControl("ccEventActions");
            actions.CurrentEvent = (Event)e.Item.DataItem;
            ////actions.ActionCompleted += new ActionEventHandler(actions_ActionCompleted);
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            this.rpEventListing.DataSource = EventCollection.Load(this.PortalId, this.rbSort.SelectedValue, 0, 0, this.rbStatus.SelectedValue == "All");
            this.rpEventListing.DataBind();
        }

        ////public override void Dispose()
        ////{
        ////    if (license != null)
        ////    {
        ////        license.Dispose();
        ////        license = null;
        ////    }
        ////    base.Dispose();
        ////}
    }
}

