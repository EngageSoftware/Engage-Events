//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Host;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using System.Globalization;
using Engage.Dnn.Events.Util;

namespace Engage.Dnn.Events
{
    public partial class Settings : ModuleSettingsBase
    {
        private ModuleSettingsBase currentSettingsBase;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DisplaySettingsControl();
        }

        public override void UpdateSettings()
        {

            if (Page.IsValid)
            {
                try
                {
                    HostSettingsController controller = new HostSettingsController();
                    controller.UpdateHostSetting(Engage.Dnn.Events.Util.Utility.ModuleConfigured + PortalId.ToString(CultureInfo.InvariantCulture), "true");

                    ModuleController modules = new ModuleController();
                    modules.UpdateTabModuleSetting(this.TabModuleId, Setting.DisplayType.PropertyName, ddlChooseDisplayType.SelectedValue.ToString());
                    //modules.UpdateTabModuleSetting(this.TabModuleId, Setting.UnsubscribeUrl.PropertyName, txtUnsubscribeUrl.Text);
                    //modules.UpdateTabModuleSetting(this.TabModuleId, Setting.PrivacyPolicyUrl.PropertyName, txtPrivacyPolicyUrl.Text);
                    //modules.UpdateTabModuleSetting(this.TabModuleId, Setting.OpenLinkUrl.PropertyName, txtOpenLinkUrl.Text);

                    currentSettingsBase.UpdateSettings();

                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                if (Page.IsPostBack == false)
                {
                    ListItem eventListingCustom = new ListItem(Localization.GetString("EventListingCustom", LocalResourceFile), "Display/EventListingCustom");
                    ListItem eventListing = new ListItem(Localization.GetString("EventListing", LocalResourceFile), "Display/EventListing");
                    ListItem eventCalendar = new ListItem(Localization.GetString("EventCalendar", LocalResourceFile), "Display/EventCalendar");

                    ddlChooseDisplayType.Items.Add(eventListingCustom);
                    ddlChooseDisplayType.Items.Add(eventCalendar);
                    ddlChooseDisplayType.Items.Add(eventListing);

                    SetOptions();

                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetOptions()
        {
            object o = Settings["DisplayType"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                ListItem li = ddlChooseDisplayType.Items.FindByValue(Settings["DisplayType"].ToString());
                if (li != null)
                {
                    li.Selected = true;
                }
            }

            //o = Settings[Setting.UnsubscribeUrl.PropertyName];
            //if (o != null && !String.IsNullOrEmpty(o.ToString()))
            //{
            //    txtUnsubscribeUrl.Text = o.ToString();
            //}

            //o = Settings[Setting.PrivacyPolicyUrl.PropertyName];
            //if (o != null && !String.IsNullOrEmpty(o.ToString()))
            //{
            //    txtPrivacyPolicyUrl.Text = o.ToString();    
            //}

            //o = Settings[Setting.OpenLinkUrl.PropertyName];
            //if (o != null && !String.IsNullOrEmpty(o.ToString()))
            //{
            //     txtOpenLinkUrl.Text = o.ToString();
            //}
        }

        protected void ddlChooseDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySettingsControl();
        }

        private void DisplaySettingsControl()
        {

            string selectedDisplayType = ddlChooseDisplayType.SelectedItem.ToString();
            switch (selectedDisplayType)
            {
                case "Calendar Display":
                    LoadSettingsControl("Display/CalendarDisplayOptions.ascx"/*, "Calendar Display"*/);
                    break;
                default:
                    break;
            }
        }

        private void LoadSettingsControl(string controlName)
        {
            this.phControls.EnableViewState = false;
            this.phControls.Controls.Clear();

            currentSettingsBase = CreateSettingsControl(controlName);
            this.phControls.Controls.Add(currentSettingsBase);
        }

        private ModuleSettingsBase CreateSettingsControl(string controlName)
        {
            ModuleSettingsBase settingsControl = (ModuleSettingsBase)LoadControl(controlName);
            ModuleController mc = new ModuleController();
            ModuleInfo mi = mc.GetModule(ModuleId, TabId);
            settingsControl.ModuleConfiguration = mi;

            //SEE LINE BELOW remove the following two lines for 4.6 because 4.6 no longer supports setting the moduleid, you have to get it through the module configuration.
            //the following appears to work fine in 4.6.2 now
            settingsControl.ModuleId = ModuleId;
            settingsControl.TabModuleId = TabModuleId;

            settingsControl.ID = Path.GetFileNameWithoutExtension(controlName);
            settingsControl.LoadSettings();

            return settingsControl;
        }
    }
}

