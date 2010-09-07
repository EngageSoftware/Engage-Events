// <copyright file="GlobalNavigation.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Navigation
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Security.Permissions;
    using DotNetNuke.Services.Exceptions;

    /// <summary>
    /// A navigation control that is always displayed at the top of the module.  Currently only for admins.
    /// </summary>
    public partial class GlobalNavigation : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);

                // since the global navigation control is not loaded using DNN mechanisms we need to set it here so that calls to 
                // module related information will appear the same as the actual control this navigation is sitting on.hk
                this.ModuleConfiguration = Engage.Utility.FindParentControl<PortalModuleBase>(this).ModuleConfiguration;

                this.Load += this.Page_Load;
            }
            catch (LicenseException)
            {
                // swallow this exception so that MainContainer can handle it
                this.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////this.BasePage.AddStyleSheet("Engage-Global-Nav", this.ResolveUrl("EngageSkin/Menu.Engage.css"));
                this.SetupLinks();
                this.SetVisibility();
                this.LocalizeMenu();
                this.SetCurrentlySelectedMenu();
                ////this.SetDisabledImages();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetCurrentlySelectedMenu()
        {
            var controlKey = this.GetCurrentControlKey();
            var currentItem = NavigationMenu.FindItemByValue(controlKey);
            if (controlKey != "" && currentItem != null)
            {
                //Highlight the current item and his parents
                currentItem.HighlightPath();
            }
            else
            {
                NavigationMenu.Items[0].HighlightPath();
            }
        }

        private void LocalizeMenu()
        {
            this.HomeItem.Text = this.Localize("Home");
            this.AddEventItem.Text = this.Localize("Add Event");
            this.ManageItem.Text = this.Localize("Manage");
            this.ManageEventsItem.Text = this.Localize("Manage Events");
            this.ManageResponsesItem.Text = this.Localize("Responses");
            this.ManageCategoriesItem.Text = this.Localize("Manage Categories");
            this.SettingsItem.Text = this.Localize("Settings");
            this.ModuleSettingsItem.Text = this.Localize("Module Settings");
            this.ChooseDisplayItem.Text = this.Localize("Choose Display");
        }

        /// <summary>
        /// Sets up the URLs for each of the links.
        /// </summary>
        private void SetupLinks()
        {
            this.HomeItem.NavigateUrl = Globals.NavigateURL();

            this.ModuleSettingsItem.NavigateUrl = this.EditUrl("ModuleId", this.ModuleId.ToString(CultureInfo.InvariantCulture), "Module");

            this.ChooseDisplayItem.Value = "ChooseDisplay";
            this.ChooseDisplayItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.ChooseDisplayItem.Value);

            this.AddEventItem.Value = "EventEdit";
            this.AddEventItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.AddEventItem.Value);

            this.ManageResponsesItem.Value = "ResponseSummary";
            this.ManageResponsesItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.ManageResponsesItem.Value);

            this.ManageEventsItem.Value = "EventListingAdmin";
            this.ManageEventsItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.ManageEventsItem.Value);

            this.ManageCategoriesItem.Value = "ManageCategories";
            this.ManageCategoriesItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.ManageCategoriesItem.Value);
            
        }

        /// <summary>
        /// Sets the visibility.
        /// </summary>
        private void SetVisibility()
        {
            this.Visible = this.IsAdmin;
            this.ModuleSettingsItem.Visible = TabPermissionController.HasTabPermission("EDIT");
        }
    }
}