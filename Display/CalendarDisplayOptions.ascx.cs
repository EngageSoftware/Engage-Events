// <copyright file="CalendarDisplayOptions.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;

    /// <summary>
    /// The settings page for the calendar display mode.
    /// </summary>
    public partial class CalendarDisplayOptions : ModuleSettingsBase
    {
        /// <summary>
        /// Gets or sets which Skin to use for the calendar display.
        /// </summary>
        /// <value>The Skin to use for the calendar display</value>
        internal TelerikSkin SkinOption
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Setting.SkinSelection.PropertyName, value.ToString());
            }

            get
            {
                return Dnn.Utility.GetEnumSetting(this.Settings, Setting.SkinSelection.PropertyName, TelerikSkin.Default);
            }
        }

        /// <summary>
        /// Gets or sets the number of events to display on a single day in the calendar's month view.
        /// </summary>
        /// <value>The number of events to display on a single day in the calendar's month view</value>
        internal int EventsPerDay
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Setting.EventsPerDay.PropertyName, value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                return Dnn.Utility.GetIntSetting(this.Settings, Setting.EventsPerDay.PropertyName, 3);
            }
        }

        /// <summary>
        /// Sets up this control.
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                this.SkinDropDownList.DataSource = Enum.GetNames(typeof(TelerikSkin));
                this.SkinDropDownList.DataBind();
                Dnn.Utility.LocalizeListControl(this.SkinDropDownList, this.LocalResourceFile);
                
                ListItem li = this.SkinDropDownList.Items.FindByValue(this.SkinOption.ToString());
                if (li != null)
                {
                    li.Selected = true;
                }

                this.EventsPerDayTextBox.Value = this.EventsPerDay;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Saves the settings for this control.
        /// </summary>
        public override void UpdateSettings()
        {
            base.UpdateSettings();

            if (this.Page.IsValid)
            {
                this.SkinOption = (TelerikSkin)Enum.Parse(typeof(TelerikSkin), this.SkinDropDownList.SelectedValue);

                if (this.EventsPerDayTextBox.Value.HasValue)
                {
                    this.EventsPerDay = (int)this.EventsPerDayTextBox.Value.Value;
                }
            }
        }
    }
}