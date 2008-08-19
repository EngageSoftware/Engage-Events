// <copyright file="TemplateDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using Framework.Templating;
    using Engage.Events;
    using Framework;
    using Templating;
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
                return Utility.GetIntSetting(this.Settings, Framework.Setting.RecordsPerPage.PropertyName, 0);
            }
        }

        /// <summary>
        /// Loads the possible settings, and selects the current values.
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                this.DisplayModeDropDown.Items.Add(new ListItem(ListingMode.All.ToString()));
                this.DisplayModeDropDown.Items.Add(new ListItem(ListingMode.Past.ToString()));
                this.DisplayModeDropDown.Items.Add(new ListItem(ListingMode.CurrentMonth.ToString()));
                this.DisplayModeDropDown.Items.Add(new ListItem(ListingMode.Future.ToString()));

                ListItem li = this.DisplayModeDropDown.Items.FindByValue(this.DisplayModeOption);
                if (li != null)
                {
                    li.Selected = true;
                }

                List<Template> templates = TemplateEngine.GetHeaderTemplates(ModuleBase.PhysicialTemplatesFolderName);
                this.HeaderDropDownList.DataTextField = "DocumentName";
                this.HeaderDropDownList.DataValueField = "DocumentName";
                this.HeaderDropDownList.DataSource = templates;
                this.HeaderDropDownList.DataBind();

                li = this.HeaderDropDownList.Items.FindByValue(this.HeaderTemplate);
                if (li != null)
                {
                    li.Selected = true;
                }

                templates = TemplateEngine.GetItemTemplates(ModuleBase.PhysicialTemplatesFolderName);
                this.ItemDropDownList.DataTextField = "DocumentName";
                this.ItemDropDownList.DataValueField = "DocumentName";
                this.ItemDropDownList.DataSource = templates;
                this.ItemDropDownList.DataBind();

                li = this.ItemDropDownList.Items.FindByValue(this.ItemTemplate);
                if (li != null)
                {
                    li.Selected = true;
                }

                templates = TemplateEngine.GetFooterTemplates(ModuleBase.PhysicialTemplatesFolderName);
                this.FooterDropDownList.DataTextField = "DocumentName";
                this.FooterDropDownList.DataValueField = "DocumentName";
                this.FooterDropDownList.DataSource = templates;
                this.FooterDropDownList.DataBind();

                li = this.FooterDropDownList.Items.FindByValue(this.FooterTemplate);
                if (li != null)
                {
                    li.Selected = true;
                }

                templates = TemplateEngine.GetDetailTemplates(ModuleBase.PhysicialTemplatesFolderName);
                this.DetailDropDownList.DataTextField = "DocumentName";
                this.DetailDropDownList.DataValueField = "DocumentName";
                this.DetailDropDownList.DataSource = templates;
                this.DetailDropDownList.DataBind();

                li = this.DetailDropDownList.Items.FindByValue(this.DetailTemplate);
                if (li != null)
                {
                    li.Selected = true;
                }

                string recordPerPage = Utility.GetStringSetting(this.Settings, Framework.Setting.RecordsPerPage.PropertyName);
                if (recordPerPage != null)
                {
                    this.RecordsPerPageTextbox.Value = Convert.ToDouble(this.RecordsPerPage);
                }
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
                this.RecordsPerPage = (int)this.RecordsPerPageTextbox.Value.Value;
            }
        }
    }
}