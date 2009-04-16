// <copyright file="NotConfigured.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2009
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
            this.Response.Redirect(Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Sets up the module with a default configuration.
        /// </summary>
        private void SetupDefaultSettings()
        {
            ModuleController modules = new ModuleController();
            modules.UpdateTabModuleSetting(this.TabModuleId, "DisplayType", "LIST");
            modules.UpdateTabModuleSetting(this.TabModuleId, "DisplayModeOption", ListingMode.All.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "RecordsPerPage", 10.ToString(CultureInfo.InvariantCulture));

            // TODO: add error handling if no templates exist?
            TemplateInfo defaultTemplate = this.GetTemplates(TemplateType.List)[0];
            modules.UpdateTabModuleSetting(this.TabModuleId, "Template", defaultTemplate.FolderName);

            string singleItemTemplateFolderName;
            if (defaultTemplate.Settings.ContainsKey("SingleItemTemplate"))
            {
                singleItemTemplateFolderName = defaultTemplate.Settings["SingleItemTemplate"];
            }
            else
            {
                singleItemTemplateFolderName = this.GetTemplates(TemplateType.SingleItem)[0].FolderName;
            }

            modules.UpdateTabModuleSetting(this.TabModuleId, "SingleItemTemplate", singleItemTemplateFolderName);
        }
    }
}