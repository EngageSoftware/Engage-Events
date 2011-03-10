// <copyright file="CalendarDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
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

    /// <summary>
    /// The settings page for the calendar display mode.
    /// </summary>
    public partial class CalendarDisplayOptions : SettingsBase
    {
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

                var telerikSkin = Dnn.Events.ModuleSettings.SkinSelection.GetValueAsEnumFor<TelerikSkin>(this).Value;
                ListItem li = this.SkinDropDownList.Items.FindByValue(telerikSkin.ToString());
                if (li != null)
                {
                    li.Selected = true;
                }

                this.EventsPerDayTextBox.Value = Dnn.Events.ModuleSettings.EventsPerDay.GetValueAsInt32For(this);
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
                Dnn.Events.ModuleSettings.SkinSelection.Set(this, this.SkinDropDownList.SelectedValue);

                if (this.EventsPerDayTextBox.Value.HasValue)
                {
                    Dnn.Events.ModuleSettings.EventsPerDay.Set(this, (int)this.EventsPerDayTextBox.Value.Value);
                }
            }
        }
    }
}