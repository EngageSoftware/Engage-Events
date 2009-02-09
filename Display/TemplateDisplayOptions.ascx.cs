// <copyright file="TemplateDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2009
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
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;
    using Framework;
    using Framework.Templating;
    using Setting = Setting;
    using Utility = Dnn.Utility;

    /// <summary>
    /// The settings for a template
    /// </summary>
    public partial class TemplateDisplayOptions : ModuleSettingsBase
    {
        /// <summary>
        /// Gets or sets the <see cref="ListingMode"/> for this listing.
        /// </summary>
        /// <value>The <see cref="ListingMode"/> for this listing</value>
        internal string DisplayModeOption
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Setting.DisplayModeOption.PropertyName, value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                return Utility.GetStringSetting(this.Settings, Setting.DisplayModeOption.PropertyName, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the template used to display the header of the event listing.
        /// </summary>
        /// <value>The template used to display the header of the event listing.</value>
        internal string HeaderTemplate
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.HeaderTemplate.PropertyName, value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                return Utility.GetStringSetting(this.Settings, Framework.Setting.HeaderTemplate.PropertyName, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the template used to display each event in the event listing.
        /// </summary>
        /// <value>The template used to display each event in the event listing.</value>
        internal string ItemTemplate
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.ItemTemplate.PropertyName, value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                return Utility.GetStringSetting(this.Settings, Framework.Setting.ItemTemplate.PropertyName, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the template used to display the footer of the event listing.
        /// </summary>
        /// <value>The template used to display the footer of the event listing</value>
        internal string FooterTemplate
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.FooterTemplate.PropertyName, value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                return Utility.GetStringSetting(this.Settings, Framework.Setting.FooterTemplate.PropertyName, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the template used to display event details.
        /// </summary>
        /// <value>The template used to display event details</value>
        internal string DetailTemplate
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.DetailTemplate.PropertyName, value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                return Utility.GetStringSetting(this.Settings, Framework.Setting.DetailTemplate.PropertyName, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the number of records per page.
        /// </summary>
        /// <value>The number of records per page.</value>
        internal int RecordsPerPage
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.RecordsPerPage.PropertyName, value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                int recordsPerPage = Utility.GetIntSetting(this.Settings, Framework.Setting.RecordsPerPage.PropertyName, 10);
                return recordsPerPage > 0 ? recordsPerPage : 10;
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
                SelectListValue(this.DisplayModeDropDown, this.DisplayModeOption);

                SetupTemplateList(this.HeaderDropDownList, TemplateEngine.GetHeaderTemplates(Framework.Utility.GetPhysicalTemplatesFolderName(Dnn.Events.Utility.DesktopModuleName)), this.HeaderTemplate);
                SetupTemplateList(this.ItemDropDownList, TemplateEngine.GetItemTemplates(Framework.Utility.GetPhysicalTemplatesFolderName(Dnn.Events.Utility.DesktopModuleName)), this.ItemTemplate);
                SetupTemplateList(this.FooterDropDownList, TemplateEngine.GetFooterTemplates(Framework.Utility.GetPhysicalTemplatesFolderName(Dnn.Events.Utility.DesktopModuleName)), this.FooterTemplate);
                SetupTemplateList(this.DetailDropDownList, TemplateEngine.GetDetailTemplates(Framework.Utility.GetPhysicalTemplatesFolderName(Dnn.Events.Utility.DesktopModuleName)), this.DetailTemplate);

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
                this.DisplayModeOption = this.DisplayModeDropDown.SelectedValue;
                this.HeaderTemplate = this.HeaderDropDownList.SelectedValue;
                this.ItemTemplate = this.ItemDropDownList.SelectedValue;
                this.FooterTemplate = this.FooterDropDownList.SelectedValue;
                this.DetailTemplate = this.DetailDropDownList.SelectedValue;
                this.RecordsPerPage = (int)this.RecordsPerPageTextBox.Value.Value;
            }
        }

        /// <summary>
        /// Sets up a <see cref="ListControl"/> with the given list of templates, selecting the given <paramref name="settingValue"/>.
        /// </summary>
        /// <param name="list">The list to fill.</param>
        /// <param name="templates">The templates which which to fill the <paramref name="list"/>.</param>
        /// <param name="settingValue">The value which should be selected in the list.</param>
        private static void SetupTemplateList(ListControl list, IEnumerable templates, string settingValue)
        {
            FillListControl(list, templates, "DocumentName", "DocumentName");
            SelectListValue(list, settingValue);
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