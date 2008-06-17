// <copyright file="EventListingCustom.ascx.cs" company="Engage Software">
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
    /// Custom event listing
    /// </summary>
    public partial class EventListingCustom : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                ////if (!Page.IsPostBack)
                ////{
                this.BindData();
                ////}
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }
           
        ////protected void lbCViewInvite_OnClick(object sender, EventArgs e)
        ////{
        ////    int eventId = GetId(sender);
        ////    //navigate to invite url
        ////}

        ////protected void lbCeMailAFriend_OnClick(object sender, EventArgs e)
        ////{
        ////    int eventId = GetId(sender);
        ////    //show email a friend

        ////    string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EmailAFriend&eventid=" + eventId.ToString());

        ////    Response.Redirect(href, true);
        ////}

        ////protected void lbCPrint_OnClick(object sender, EventArgs e)
        ////{
        ////    int eventId = GetId(sender);
        ////    //print
        ////}

        ////protected void lbCICal_OnClick(object sender, EventArgs e)
        ////{
        ////    int eventId = GetId(sender);
        ////    Event evnt = Event.Load(eventId);

        ////    SendICalendarToClient(evnt.ToICal("hkenuam@engagesoftware.com"), evnt.Title);
        ////}

        /// <summary>
        /// Handles the ItemDataBound event of the Listing control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void Listing_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EventAdminActions actions = (EventAdminActions)e.Item.FindControl("ccEventActions");
            if (actions != null)
            {
                actions.CurrentEvent = (Event)e.Item.DataItem;
                ////actions.ActionCompleted += new ActionEventHandler(actions_ActionCompleted);
            }

            actions = (EventAdminActions)e.Item.FindControl("ccEventActions2");
            if (actions != null)
            {
                actions.CurrentEvent = (Event)e.Item.DataItem;
                ////actions.ActionCompleted += new ActionEventHandler(actions_ActionCompleted);
            }

            ////LinkButton lbICal = e.Item.FindControl("lbCICal") as LinkButton;
            ////if (lbICal != null) lbICal.Visible = IsLoggedIn;

            ////lbICal = e.Item.FindControl("lbUICal") as LinkButton;
            ////if (lbICal != null) lbICal.Visible = IsLoggedIn;
        }

        ////private void actions_ActionCompleted(object sender, ActionEventArg e)
        ////{
        ////    if (e.ActionStatus == Action.Success)
        ////    {
        ////        BindData();
        ////    }
        ////}

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            EventCollection events = EventCollection.Load(PortalId, true, 0, 0);
            rpCurrentEventListing.DataSource = events;
            rpCurrentEventListing.DataBind();

            events = EventCollection.Load(PortalId, false, 0, 0);
            rpUpcomingEventListing.DataSource = events;
            rpUpcomingEventListing.DataBind();
        }

        ////protected bool HasInviteUrl(object invitationUrl)
        ////{
        ////    return (invitationUrl.ToString().Length > 0);
        ////}
    }
}

