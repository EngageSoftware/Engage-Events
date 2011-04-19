// <copyright file="GlobalNavigation.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI;

    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;

    /// <summary>
    /// A navigation control that is always displayed at the top of the module.  Currently only for admins.
    /// </summary>
    public partial class GlobalNavigation : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);

                this.Load += this.Page_Load;
            }
            catch (LicenseException)
            {
                // swallow this exception so that MainContainer can handle it
                this.Visible = false;
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.SetupLinks();
                this.SetVisibility();
                this.LocalizeMenu();
                this.SetCurrentlySelectedMenu();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Highlights the path of the menu item for the current page.
        /// </summary>
        private void SetCurrentlySelectedMenu()
        {
            var controlKey = this.GetCurrentControlKey();
            var currentItem = this.NavigationMenu.FindItemByValue(controlKey);
            if (!string.IsNullOrEmpty(controlKey) && currentItem != null)
            {
                // Highlight the current item and his parents
                currentItem.HighlightPath();
            }
            else
            {
                this.NavigationMenu.Items[0].HighlightPath();
            }
        }

        /// <summary>
        /// Localizes the menu items.
        /// </summary>
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
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Telerik.Web.UI.ControlItem.set_Value(System.String)", Justification = "Value is not a human-readable field")]
        private void SetupLinks()
        {
            this.HomeItem.NavigateUrl = Globals.NavigateURL();

            this.AddEventItem.Value = @"EventEdit";
            this.AddEventItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.AddEventItem.Value);

            this.ManageEventsItem.Value = @"EventListingAdmin";
            this.ManageEventsItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.ManageEventsItem.Value);

            this.ManageResponsesItem.Value = @"ResponseSummary";
            this.ManageResponsesItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.ManageResponsesItem.Value);

            this.ManageCategoriesItem.Value = @"ManageCategories";
            this.ManageCategoriesItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.ManageCategoriesItem.Value);

            this.ModuleSettingsItem.NavigateUrl = this.EditUrl("ModuleId", this.ModuleId.ToString(CultureInfo.InvariantCulture), "Module");

            this.ChooseDisplayItem.Value = @"ChooseDisplay";
            this.ChooseDisplayItem.NavigateUrl = this.BuildLinkUrl(this.ModuleId, this.ChooseDisplayItem.Value);
        }

        /// <summary>
        /// Sets the visibility.
        /// </summary>
        private void SetVisibility()
        {
            this.Visible = this.PermissionsService.HasAnyPermission;
            if (!this.Visible)
            {
                return;
            }

            this.AddEventItem.Visible = this.PermissionsService.CanManageEvents;
            
            this.ManageEventsItem.Visible = this.PermissionsService.CanManageEvents;
            this.ManageResponsesItem.Visible = this.PermissionsService.CanViewResponses;
            this.ManageCategoriesItem.Visible = this.PermissionsService.CanManageCategories;
            this.ManageItem.Visible = this.ManageEventsItem.Visible || this.ManageResponsesItem.Visible || this.ManageCategoriesItem.Visible;

            this.ModuleSettingsItem.Visible = this.PermissionsService.CanAccessModuleSettings;
            this.ChooseDisplayItem.Visible = this.PermissionsService.CanManageDisplay;
            this.SettingsItem.Visible = this.ModuleSettingsItem.Visible || this.ChooseDisplayItem.Visible;
        }
    }
}