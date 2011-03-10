// <copyright file="TemplateDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Display
{
    using System;
    using System.Collections;
    using System.Web.UI.WebControls;

    using DotNetNuke.Services.Exceptions;
    using Engage.Events;
    using Utility = Dnn.Utility;

    /// <summary>
    /// The settings for a template
    /// </summary>
    public partial class TemplateDisplayOptions : SettingsBase
    {
        /// <summary>
        /// Gets the number of events to display per page.
        /// </summary>
        /// <value>The number of events to display per page.</value>
        internal int RecordsPerPage
        {
            get
            {
                int? recordsPerPage = Dnn.Events.ModuleSettings.RecordsPerPage.GetValueAsInt32For(this);
                return recordsPerPage.HasValue && recordsPerPage.Value > 0 ? recordsPerPage.Value : Dnn.Events.ModuleSettings.RecordsPerPage.DefaultValue;
            }
        }

        /// <summary>
        /// Loads the possible settings, and selects the current values.
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                FillListControl(this.DisplayModeDropDown, Enum.GetNames(typeof(ListingMode)), string.Empty, string.Empty);
                Utility.LocalizeListControl(this.DisplayModeDropDown, this.LocalResourceFile);
                SelectListValue(this.DisplayModeDropDown, Dnn.Events.ModuleSettings.DisplayModeOption.GetValueAsStringFor(this));

                this.RecordsPerPageTextBox.Value = this.RecordsPerPage;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Updates the settings for this module.
        /// </summary>
        public override void UpdateSettings()
        {
            base.UpdateSettings();

            if (this.Page.IsValid)
            {
                Dnn.Events.ModuleSettings.DisplayModeOption.Set(this, this.DisplayModeDropDown.SelectedValue);
                Dnn.Events.ModuleSettings.RecordsPerPage.Set(this, (int)this.RecordsPerPageTextBox.Value.Value);
            }
        }

        /// <summary>
        /// Fills the given list control with the items.
        /// </summary>
        /// <param name="list">The list to fill.</param>
        /// <param name="items">The items with which to fill the <paramref name="list"/>.</param>
        /// <param name="dataTextField">Name of the field in the <paramref name="items"/> that should be displayed as text in the <paramref name="list"/></param>
        /// <param name="dataValueField">Name of the field in the <paramref name="items"/> that should be the value of the item in the <paramref name="list"/></param>
        private static void FillListControl(ListControl list, IEnumerable items, string dataTextField, string dataValueField)
        {
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataSource = items;
            list.DataBind();
        }

        /// <summary>
        /// Selects the given value in the list, if an item with that value is in the list.
        /// </summary>
        /// <param name="list">The list of items whose selected value is to be set.</param>
        /// <param name="selectedValue">The value of the item to be selected.</param>
        private static void SelectListValue(ListControl list, string selectedValue)
        {
            ListItem li = list.Items.FindByValue(selectedValue);
            if (li != null)
            {
                li.Selected = true;
            }
        }
    }
}