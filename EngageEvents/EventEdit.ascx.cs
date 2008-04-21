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
    public partial class EventEdit : ModuleBase
    {

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (EventId > 0) BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void lbAddAnEvent_Click(object sender, EventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit");

            Response.Redirect(href, true);

        }

        protected void lbSave_Click(object sender, EventArgs e)
        {

            Save();

            Response.Redirect(Globals.NavigateURL(), true);
        }

        protected void lbSaveAndCreateNew_Click(object sender, EventArgs e)
        {
            Save();

            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit");
            Response.Redirect(href, true);
        }

        #endregion

        #region Methods

        private void Save()
        {
            if (EventId > 0)
            {
                Update();
            }
            else
            {
                Insert();
            }
        }

        private void Update()
        {
            Event e = Event.Load(EventId);

            string date = txtEventDate.Text + " " + txtEventTime.Text + " " + rblEventTime.SelectedValue;
            e.EventStart = Convert.ToDateTime(date);
            e.Location = txtEventLocation.Text;
            e.Title = txtEventTitle.Text;
            e.Save(UserId);
        }

        private void Insert()
        {
            string date = txtEventDate.Text + " " + txtEventTime.Text + " " + rblEventTime.SelectedValue;

            Event e = Event.Create(PortalId, ModuleId, UserInfo.Email, txtEventTitle.Text, txtEventDescription.Text, Convert.ToDateTime(date));
            e.Location = txtEventLocation.Text;

            e.Save(UserId);

        }

        private void BindData()
        {
            Event e = Event.Load(EventId);

            txtEventTitle.Text = e.Title;
            txtEventTime.Text = e.EventStart.ToString("h:mm");
            txtEventLocation.Text = e.Location;
            txtEventDescription.Text = e.Overview;
            txtEventDate.Text = e.EventStart.ToString("dd/MM/yyyy");
            ListItem li = rblEventTime.Items.FindByValue(e.EventStart.ToString("tt"));
            if (li != null) li.Selected = true;

        }



        #endregion

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(), true);
        }
    }
}

