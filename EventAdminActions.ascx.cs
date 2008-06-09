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
using System.Globalization;
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
    public partial class EventAdminActions : ModuleBase
    {
        public event ActionEventHandler ActionCompleted;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //since the global navigation control is not loaded using DNN mechanisms we need to set it here so that calls to 
            //module related information will appear the same as the actual control this navigation is sitting on.hk
            this.ModuleConfiguration = ((PortalModuleBase)base.Parent.Parent.Parent).ModuleConfiguration;
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "App_LocalResources/EventActions";
        }

        private Engage.Events.Event _event;
        internal Engage.Events.Event DataItem
        {
            get { return _event; }
            set 
            { 
                _event = value;
                BindData();
            }
        }

        private void BindData()
        {
            lbAddToCalendar.Visible = IsLoggedIn;

            lbCancel.Visible = IsAdmin;
            lbDelete.Visible = IsAdmin;
            lbEditEvent.Visible = IsAdmin;
            lbResponses.Visible = IsAdmin;
            lbViewInvite.Visible = false; //for now. hk
            lbEditEmail.Visible = false; //for now. hk

            string cancelText = Localization.GetString("Cancel", LocalResourceFile);
            if (_event.Cancelled == true)
            {
                cancelText = Localization.GetString("UnCancel", LocalResourceFile);
            }
            lbCancel.Text = cancelText;

            lbViewInvite.NavigateUrl = _event.InvitationUrl;
            lbViewInvite.Visible = _event.InvitationUrl.Length > 0;

            lbDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("ConfirmDelete", LocalResourceFile) + "');");

            if (_event.Cancelled)
            {
                lbCancel.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("ConfirmUnCancel", LocalResourceFile) + "');");
            }
            else
            {
                lbCancel.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("ConfirmCancel", LocalResourceFile) + "');");
            }

        }

        #region Event Handlers

        protected void lbEditEvent_OnClick(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit&eventId=" + _event.Id.ToString());
            Response.Redirect(href, true);
        }

        protected void lbRegister_OnClick(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=Register&eventid=" + _event.Id.ToString());
            Response.Redirect(href, true);        
        }

        protected void lbResponses_OnClick(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=RsvpDetail&eventid=" + _event.Id.ToString());
            Response.Redirect(href, true);
        }

        protected void lbDeleteEvent_OnClick(object sender, EventArgs e)
        {
            Event.Delete(_event.Id);
            if (ActionCompleted != null)
            {
                ActionCompleted(this, new ActionEventArg(Action.Success));
            }
        }

        protected void lbCancel_OnClick(object sender, EventArgs e)
        {
            _event.Cancelled = !_event.Cancelled;
            _event.Save(UserId);

            if (ActionCompleted != null)
            {
                ActionCompleted(this, new ActionEventArg(Action.Success));
            }
        }

        protected void lbEditEmail_OnClick(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EmailEdit&eventid=" + _event.Id.ToString());
            Response.Redirect(href, true);
        }

        protected void lbAddToCalendar_OnClick(object sender, EventArgs e)
        {
            SendICalendarToClient(_event.ToICal(base.UserInfo.Email), _event.Title);
        }

        #endregion
    }
}

