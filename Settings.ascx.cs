//Engage: Events - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Host;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System.Globalization;

namespace Engage.Dnn.Events
{
    public partial class Settings : ModuleSettingsBase
    {
        private ModuleSettingsBase currentSettingsBase;

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
                    controller.UpdateHostSetting(Dnn.Events.Util.Utility.ModuleConfigured + PortalId.ToString(CultureInfo.InvariantCulture), "true");

                    ModuleController modules = new ModuleController();
                    modules.UpdateTabModuleSetting(this.TabModuleId, Setting.DisplayType.PropertyName, DropDownChooseDisplay.SelectedValue);
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
                    ListItem listItemTemplated = new ListItem(Localization.GetString("EventListingTemplate", LocalResourceFile), "Display/EventListingTemplate");
                    ListItem listItemListing = new ListItem(Localization.GetString("EventListing", LocalResourceFile), "Display/EventListing");
                    ListItem listItemCalendar = new ListItem(Localization.GetString("EventCalendar", LocalResourceFile), "Display/EventCalendar");

                    DropDownChooseDisplay.Items.Add(listItemTemplated);
                    DropDownChooseDisplay.Items.Add(listItemListing);
                    DropDownChooseDisplay.Items.Add(listItemCalendar);

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
            string displayType = Utility.GetStringSetting(Settings, Setting.DisplayType.PropertyName);

            ListItem li = DropDownChooseDisplay.Items.FindByValue(displayType);
            if (li != null)
            {
                li.Selected = true;
            }
        }

        protected void DropDownChooseDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySettingsControl();
        }

        private void DisplaySettingsControl()
        {

            string selectedDisplayType = DropDownChooseDisplay.SelectedItem.ToString();
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

