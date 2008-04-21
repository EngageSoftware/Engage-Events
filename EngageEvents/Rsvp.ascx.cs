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
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Events.Data;
using Engage.Dnn.Events.Util;
using Engage.Routing;
using Engage.Communication.Email;
using Engage.Services.Client;
using Engage.Events;

namespace Engage.Dnn.Events
{
    public partial class Rsvp : ModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void BindData()
        {
            Event e = Event.Load(EventId);
            lblEventName.Text = "To RSVP for " + e.Title + " please enter your email address below.";

            if (Request.IsAuthenticated == true)
            {
                txtEmail.Text = UserInfo.Email;
            }
            else
            {
                //Send them to registration?
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Engage.Events.Rsvp rsvp = Engage.Events.Rsvp.Create(EventId, UserInfo.FirstName, UserInfo.LastName, UserInfo.Email);
            rsvp.Status = (RsvpStatus)Enum.Parse(typeof(RsvpStatus), rbRsvp.SelectedValue);
            rsvp.Save(UserId);

            Response.Redirect(Globals.NavigateURL(), true);
        }

        protected void lbAddToCalendar_OnClick(object sender, EventArgs e)
        {
            Event evnt = Event.Load(EventId);
            string email = txtEmail.Text;

            SendICalendarToClient(evnt.ToICal(email), evnt.Title);
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            aspNetEmail.EmailMessage message = new aspNetEmail.EmailMessage();
            lbAddToCalendar.Enabled = message.IsValidEmail(txtEmail.Text);
        }

    }
}

