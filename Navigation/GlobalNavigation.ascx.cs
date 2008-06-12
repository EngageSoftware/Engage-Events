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
    public partial class GlobalNavigation : ModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //since the global navigation control is not loaded using DNN mechanisms we need to set it here so that calls to 
            //module related information will appear the same as the actual control this navigation is sitting on.hk
            this.ModuleConfiguration = ((PortalModuleBase)base.Parent).ModuleConfiguration;
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "App_LocalResources/GlobalNavigation";
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                lbSettings.Visible = IsAdmin;
                lbAddAnEvent.Visible = IsAdmin;
                lbResponses.Visible = IsAdmin;
                lbManageEvents.Visible = IsAdmin;
                
                //TODO: change to swap out css based on parent control, eventedit, admin, whatever.hk

                switch (Parent.ID)
                {
                    case "EventEdit":
                        lbAddAnEvent.ImageUrl = "~/desktopmodules/EngageEvents/Images/add_event_disabled.gif";
                        break;
                    case "EventListingAdmin":
                        lbManageEvents.ImageUrl = "~/desktopmodules/EngageEvents/Images/manage_events_disabled.gif";
                        break;
                    case "RsvpSummary":
                        lbResponses.ImageUrl = "~/desktopmodules/EngageEvents/Images/responses_disabled.gif";
                        break;
                    default:
                        break;
                }
                //foreach (Control c in this.Controls)
                //{
                //    if (c is LinkButton)
                //    {
                //        ImageButton img = (ImageButton)c;
                //        if (img.ImageUrl = 
                //    }
                //}
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        protected void lbResponses_OnClick(object sender, ImageClickEventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=RsvpSummary");
            Response.Redirect(href, true);
        }

        protected void lbAddAnEvent_OnClick(object sender, ImageClickEventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit");
            Response.Redirect(href, true);
        }

        protected void lbSettings_OnClick(object sender, ImageClickEventArgs e)
        {
            string href = EditUrl("ModuleId", ModuleId.ToString(CultureInfo.InvariantCulture), "Module");
            Response.Redirect(href, true);
        }

        protected void lbManageEvents_OnClick(object sender, ImageClickEventArgs e)
        {
            string href = BuildLinkUrl("&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventListingAdmin");
            Response.Redirect(href, true);
        }

    }
}

