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
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;

    public partial class CalendarDisplayOptions : ModuleSettingsBase
    {
        internal string SkinOption
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, Setting.SkinSelection.PropertyName, value);
            }

            get
            {
                return Utility.GetStringSetting(this.Settings, Setting.SkinSelection.PropertyName);
            }
        }

        public override void LoadSettings()
        {
            try
            {
                this.ddlSkin.Items.Clear();
                this.ddlSkin.Items.Add(new ListItem("Black"));
                this.ddlSkin.Items.Add(new ListItem("WebBlue"));
                this.ddlSkin.Items.Add(new ListItem("Default"));
                this.ddlSkin.Items.Add(new ListItem("Hay"));
                this.ddlSkin.Items.Add(new ListItem("Inox"));
                this.ddlSkin.Items.Add(new ListItem("Office2007"));
                this.ddlSkin.Items.Add(new ListItem("Mac"));
                this.ddlSkin.Items.Add(new ListItem("Outlook"));
                this.ddlSkin.Items.Add(new ListItem("Telerik"));
                this.ddlSkin.Items.Add(new ListItem("Sunset"));
                this.ddlSkin.Items.Add(new ListItem("Vista"));
                this.ddlSkin.Items.Add(new ListItem("Web20"));

                ListItem li = this.ddlSkin.Items.FindByValue(this.SkinOption);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            base.UpdateSettings();

            if (this.Page.IsValid)
            {
                this.SkinOption = this.ddlSkin.SelectedValue;
            }
        }
    }
}