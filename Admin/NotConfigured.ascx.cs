// <copyright file="NotConfigured.ascx.cs" company="Engage Software">
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
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using Engage.Events;
    using Framework.Templating;

    /// <summary>
    /// Displayed when the module has not yet been configured.  Sets up a default configuration.
    /// </summary>
    public partial class NotConfigured : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ////ConfigureLink.Text = Localization.GetString("UnableToFindAction", LocalResourceFile);
            ////ConfigureLink.NavigateUrl = EditUrl("ModuleId", ModuleId.ToString(CultureInfo.InvariantCulture), "Module");
            ////ConfigureLink.Visible = true;

            this.SetupDefaultSettings();
            this.Response.Redirect(Globals.NavigateURL());
        }

        /// <summary>
        /// Sets up the module with a default configuration.
        /// </summary>
        private void SetupDefaultSettings()
        {
            HostSettingsController controller = new HostSettingsController();
            controller.UpdateHostSetting(Framework.Utility.ModuleConfigured + PortalId.ToString(CultureInfo.InvariantCulture), "true");

            ModuleController modules = new ModuleController();
            modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.DisplayTemplate.PropertyName, "Display.Listing.html");
            modules.UpdateTabModuleSetting(this.TabModuleId, Setting.DisplayModeOption.PropertyName, ListingMode.All.ToString());

            // TODO: add error handling if a folder doesn't have any documents of a particular type
            modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.HeaderTemplate.PropertyName, TemplateEngine.GetHeaderTemplates(PhysicialTemplatesFolderName)[0].DocumentName);
            modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.ItemTemplate.PropertyName, TemplateEngine.GetItemTemplates(PhysicialTemplatesFolderName)[0].DocumentName);
            modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.FooterTemplate.PropertyName, TemplateEngine.GetFooterTemplates(PhysicialTemplatesFolderName)[0].DocumentName);
            modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.DetailTemplate.PropertyName, TemplateEngine.GetDetailTemplates(PhysicialTemplatesFolderName)[0].DocumentName);
            modules.UpdateTabModuleSetting(this.TabModuleId, Framework.Setting.RecordsPerPage.PropertyName, 0.ToString(CultureInfo.InvariantCulture));
        }
    }
}