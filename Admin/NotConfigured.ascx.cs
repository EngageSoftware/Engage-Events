// <copyright file="NotConfigured.ascx.cs" company="Engage Software">
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
    using System.Linq;
    using System.Web.UI;

    using DotNetNuke.Common;

    using Engage.Events;

    using Framework.Templating;

    /// <summary>
    /// Displayed when the module has not yet been configured.  Sets up a default configuration.
    /// </summary>
    public partial class NotConfigured : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.SetupDefaultSettings();
            this.Response.Redirect(Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Sets up the module with a default configuration.
        /// </summary>
        private void SetupDefaultSettings()
        {
            this.EnsureDefaultCategoryExists();

            ModuleSettings.DisplayType.Set(this, ModuleSettings.DisplayType.DefaultValue);
            ModuleSettings.DisplayModeOption.Set(this, ModuleSettings.DisplayModeOption.DefaultValue);
            ModuleSettings.RecordsPerPage.Set(this, ModuleSettings.RecordsPerPage.DefaultValue);

            // TODO: add error handling if no templates exist?
            TemplateInfo defaultTemplate = this.GetTemplates(TemplateType.List)[0];
            ModuleSettings.Template.Set(this, defaultTemplate.FolderName);

            string singleItemTemplateFolderName;
            if (!defaultTemplate.Settings.TryGetValue("SingleItemTemplate", out singleItemTemplateFolderName))
            {
                singleItemTemplateFolderName = this.GetTemplates(TemplateType.SingleItem)[0].FolderName;
            }

            ModuleSettings.SingleItemTemplate.Set(this, singleItemTemplateFolderName);
        }

        /// <summary>
        /// Ensures that there is at least one category in this portal; otherwise, creates the default category.
        /// </summary>
        private void EnsureDefaultCategoryExists()
        {
            if (CategoryCollection.Load(this.PortalId).Any())
            {
                return;
            }

            var defaultCategory = Category.Create(this.PortalId, string.Empty, null);
            defaultCategory.Save(this.UserId);
        }
    }
}