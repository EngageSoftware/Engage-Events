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
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;

namespace Engage.Dnn.Events
{
    public partial class CalendarDisplayOptions : ModuleSettingsBase
    {
        public override void LoadSettings()
        {
            try
            {
                ddlSkin.Items.Add(new ListItem("Black", "Black"));
                ddlSkin.Items.Add(new ListItem("WebBlue", "WebBlue"));
                ddlSkin.Items.Add(new ListItem("Default", "Default"));
                ddlSkin.Items.Add(new ListItem("Hay", "Hay"));
                ddlSkin.Items.Add(new ListItem("Inox", "Inox"));
                ddlSkin.Items.Add(new ListItem("Office2007", "Office2007"));
                ddlSkin.Items.Add(new ListItem("Mac", "Mac"));
                ddlSkin.Items.Add(new ListItem("Outlook", "Outlook"));
                ddlSkin.Items.Add(new ListItem("Telerik", "Telerik"));
                ddlSkin.Items.Add(new ListItem("Sunset", "Sunset"));
                ddlSkin.Items.Add(new ListItem("Vista", "Vista"));
                ddlSkin.Items.Add(new ListItem("Web20", "Web20"));

                ListItem li = ddlSkin.Items.FindByValue(SkinOption);
                if (li != null) li.Selected = true;

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
                SkinOption = ddlSkin.SelectedValue;                
            }
        }

        internal string SkinOption
        {
            set
            {
                ModuleController modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, Setting.SkinSelection.PropertyName, value.ToString(CultureInfo.InvariantCulture));
                
            }
            get
            {
                object o = Settings[Setting.SkinSelection.PropertyName];
                return (o == null ? string.Empty : o.ToString());
            }
        }
      
    }
}

