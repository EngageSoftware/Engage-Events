// <copyright file="Settings.ascx.cs" company="Engage Software">
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
    using System.IO;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using System.Globalization;

    /// <summary>
    /// This is the settings code behind for Event related Settings.
    /// </summary>
    public partial class Settings : ModuleSettingsBase
    {
        /// <summary>
        /// Backing field for the current module settings base selected.
        /// </summary>
        private ModuleSettingsBase currentSettingsBase;

        /// <summary>
        /// Updates the settings.
        /// </summary>
        public override void UpdateSettings()
        {
            if (Page.IsValid)
            {
                try
                {
                    HostSettingsController controller = new HostSettingsController();
                    controller.UpdateHostSetting(Framework.Utility.ModuleConfigured + PortalId.ToString(CultureInfo.InvariantCulture), "true");

                    ModuleController modules = new ModuleController();
                    modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.DisplayTemplate.PropertyName, this.DropDownChooseDisplay.SelectedValue);

                    modules.UpdateTabModuleSetting(this.TabModuleId, Setting.FeaturedOnly.PropertyName, this.FeaturedCheckbox.Checked.ToString());
                    
                    this.currentSettingsBase.UpdateSettings();
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                if (Page.IsPostBack == false)
                {
                    ListItem templatedListItem = new ListItem(Localization.GetString("EventListingTemplate", LocalResourceFile), "Display.Listing.html");
                    ListItem calendarListItem = new ListItem(Localization.GetString("EventCalendar", LocalResourceFile), "Display.Calendar.html");

                    this.DropDownChooseDisplay.Items.Add(templatedListItem);
                    this.DropDownChooseDisplay.Items.Add(calendarListItem);

                    this.SetOptions();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DisplaySettingsControl();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the DropDownChooseDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void DropDownChooseDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DisplaySettingsControl();
        }

        /// <summary>
        /// Sets the options.
        /// </summary>
        private void SetOptions()
        {
            string displayType = Utility.GetStringSetting(Settings, Framework.Setting.DisplayTemplate.PropertyName);

            ListItem li = this.DropDownChooseDisplay.Items.FindByValue(displayType);
            if (li != null)
            {
                li.Selected = true;
            }

            string featured = Utility.GetStringSetting(Settings, Setting.FeaturedOnly.PropertyName);
            if (!string.IsNullOrEmpty(featured))
            {
                this.FeaturedCheckbox.Checked = Convert.ToBoolean(featured);
            }
        }

        /// <summary>
        /// Displays the settings control.
        /// </summary>
        private void DisplaySettingsControl()
        {
            string selectedDisplayType = this.DropDownChooseDisplay.SelectedValue;
            switch (selectedDisplayType)
            {
                case "Display.Listing.html":
                    this.LoadSettingsControl("Display/TemplateDisplayOptions.ascx");
                    break;
                case "Display.Calendar.html":
                    this.LoadSettingsControl("Display/CalendarDisplayOptions.ascx");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Loads the settings control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        private void LoadSettingsControl(string controlName)
        {
            this.phControls.EnableViewState = false;
            this.phControls.Controls.Clear();

            this.currentSettingsBase = this.CreateSettingsControl(controlName);
            this.phControls.Controls.Add(this.currentSettingsBase);
        }

        /// <summary>
        /// Creates the settings control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        /// <returns>Module Settings Base</returns>
        private ModuleSettingsBase CreateSettingsControl(string controlName)
        {
            ModuleSettingsBase settingsControl = (ModuleSettingsBase)LoadControl(controlName);
            ModuleController mc = new ModuleController();
            ModuleInfo mi = mc.GetModule(ModuleId, TabId);
            settingsControl.ModuleConfiguration = mi;

            // SEE LINE BELOW remove the following two lines for 4.6 because 4.6 no longer supports setting the moduleid, you have to get it through the module configuration.
            // the following appears to work fine in 4.6.2 now
            settingsControl.ModuleId = ModuleId;
            settingsControl.TabModuleId = TabModuleId;

            settingsControl.ID = Path.GetFileNameWithoutExtension(controlName);
            settingsControl.LoadSettings();

            return settingsControl;
        }
    }
}

