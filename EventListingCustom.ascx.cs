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
    public partial class EventListingCustom : ModuleBase
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
           
        protected void lbCViewInvite_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            //navigate to invite url
        }

        protected void lbCeMailAFriend_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            //show email a friend

            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EmailAFriend&eventid=" + eventId.ToString());

            Response.Redirect(href, true);
        }

        protected void lbCPrint_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            //print
        }

        protected void lbCICal_OnClick(object sender, EventArgs e)
        {
            int eventId = GetId(sender);
            Event evnt = Event.Load(eventId);

            SendICalendarToClient(evnt.ToICal("hkenuam@engagesoftware.com"), evnt.Title);
        }

        protected void Listing_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton lbICal = e.Item.FindControl("lbCICal") as LinkButton;
            if (lbICal != null) lbICal.Visible = IsLoggedIn;

            lbICal = e.Item.FindControl("lbUICal") as LinkButton;
            if (lbICal != null) lbICal.Visible = IsLoggedIn;

        }

        #endregion

        #region Methods

        private void BindData()
        {
            EventCollection events = EventCollection.Load(PortalId, true, 0, 0);
            rpCurrentEventListing.DataSource = events;
            rpCurrentEventListing.DataBind();

            events = EventCollection.Load(PortalId, false, 0, 0);
            rpUpcomingEventListing.DataSource = events;
            rpUpcomingEventListing.DataBind();

        }

       
        #endregion

        protected bool HasInviteUrl(object invitationUrl)
        {
            return (invitationUrl.ToString().Length > 0);
        }

     
    }
}

