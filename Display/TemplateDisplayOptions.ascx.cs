//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

namespace Engage.Dnn.Events
{
    using System.Collections.Generic;
    using Engage.Events;
    using Templating;
    
    public partial class TemplateDisplayOptions : ModuleSettingsBase
    {
        public override void LoadSettings()
        {
            try
            {
                DisplayModeDropDown.Items.Add(new ListItem(ListingMode.All.ToString(), ListingMode.All.ToString()));
                DisplayModeDropDown.Items.Add(new ListItem(ListingMode.Past.ToString(), ListingMode.Past.ToString()));
                DisplayModeDropDown.Items.Add(new ListItem(ListingMode.CurrentMonth.ToString(), ListingMode.CurrentMonth.ToString()));
                DisplayModeDropDown.Items.Add(new ListItem(ListingMode.Future.ToString(), ListingMode.Future.ToString()));

                ListItem li = DisplayModeDropDown.Items.FindByValue(DisplayModeOption);
                if (li != null) li.Selected = true;

                List<Template> templates = TemplateEngine.GetHeaderTemplates(ModuleBase.PhysicialTemplatesFolderName);
                HeaderDropdownlist.DataTextField = "DocumentName";
                HeaderDropdownlist.DataValueField = "DocumentName";
                HeaderDropdownlist.DataSource = templates;
                HeaderDropdownlist.DataBind();

                li = HeaderDropdownlist.Items.FindByValue(HeaderTemplate);
                if (li != null) li.Selected = true;

                templates = TemplateEngine.GetItemTemplates(ModuleBase.PhysicialTemplatesFolderName);
                ItemDropdownlist.DataTextField = "DocumentName";
                ItemDropdownlist.DataValueField = "DocumentName";
                ItemDropdownlist.DataSource = templates;
                ItemDropdownlist.DataBind();

                li = ItemDropdownlist.Items.FindByValue(ItemTemplate);
                if (li != null) li.Selected = true;

                templates = TemplateEngine.GetFooterTemplates(ModuleBase.PhysicialTemplatesFolderName);
                FooterDropdownlist.DataTextField = "DocumentName";
                FooterDropdownlist.DataValueField = "DocumentName";
                FooterDropdownlist.DataSource = templates;
                FooterDropdownlist.DataBind();

                li = FooterDropdownlist.Items.FindByValue(FooterTemplate);
                if (li != null) li.Selected = true;

                templates = TemplateEngine.GetDetailTemplates(ModuleBase.PhysicialTemplatesFolderName);
                DetailDropdownlist.DataTextField = "DocumentName";
                DetailDropdownlist.DataValueField = "DocumentName";
                DetailDropdownlist.DataSource = templates;
                DetailDropdownlist.DataBind();

                li = DetailDropdownlist.Items.FindByValue(DetailTemplate);
                if (li != null) li.Selected = true;
                
                string recordPerPage = Utility.GetStringSetting(Settings, Setting.RecordsPerPage.PropertyName);
                if (recordPerPage != null) RadNumericRecordsPerPage.Value = Convert.ToDouble(RecordsPerPage);


            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            base.UpdateSettings();

            if (Page.IsValid)
            {
                DisplayModeOption = DisplayModeDropDown.SelectedValue;
                HeaderTemplate = HeaderDropdownlist.SelectedValue;
                ItemTemplate = ItemDropdownlist.SelectedValue;
                FooterTemplate = FooterDropdownlist.SelectedValue;
                DetailTemplate = DetailDropdownlist.SelectedValue;
                RecordsPerPage = Convert.ToInt32(RadNumericRecordsPerPage.Value);
            }
        }

        internal string DisplayModeOption
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, Setting.DisplayModeOption.PropertyName, value.ToString(CultureInfo.InvariantCulture));
                
            }
            get
            {
                object o = Settings[Setting.DisplayModeOption.PropertyName];
                return (o == null ? string.Empty : o.ToString());
            }
        }

        internal string HeaderTemplate
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, Setting.HeaderTemplate.PropertyName, value.ToString(CultureInfo.InvariantCulture));

            }
            get
            {
                object o = Settings[Setting.HeaderTemplate.PropertyName];
                return (o == null ? string.Empty : o.ToString());
            }
        }

        internal string ItemTemplate
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, Setting.ItemTemplate.PropertyName, value.ToString(CultureInfo.InvariantCulture));

            }
            get
            {
                object o = Settings[Setting.ItemTemplate.PropertyName];
                return (o == null ? string.Empty : o.ToString());
            }
        }

        internal string FooterTemplate
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, Setting.FooterTemplate.PropertyName, value.ToString(CultureInfo.InvariantCulture));

            }
            get
            {
                object o = Settings[Setting.FooterTemplate.PropertyName];
                return (o == null ? string.Empty : o.ToString());
            }
        }

        internal string DetailTemplate
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, Setting.DetailTemplate.PropertyName, value.ToString(CultureInfo.InvariantCulture));

            }
            get
            {
                object o = Settings[Setting.DetailTemplate.PropertyName];
                return (o == null ? string.Empty : o.ToString());
            }
        }

        internal int RecordsPerPage
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, Setting.RecordsPerPage.PropertyName, value.ToString(CultureInfo.InvariantCulture));

            }
            get
            {
                object o = Settings[Setting.RecordsPerPage.PropertyName];
                return (o == null ? 0: int.Parse(o.ToString()));
            }
        }
    }
}

