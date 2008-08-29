// <copyright file="GlobalNavigation.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.Globalization;
    using System.IO;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Security.Permissions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    /// <summary>
    /// A navigation control that is always displayed at the top of the module.  Currently only for admins.
    /// </summary>
    public partial class GlobalNavigation : ModuleBase
    {
        /////// <summary>
        /////// Gets the current control key.
        /////// </summary>
        /////// <value>The current control key.</value>
        ////private string CurrentControlKey
        ////{
        ////    get
        ////    {
        ////        return this.Request.QueryString["key"] ?? string.Empty;
        ////    }
        ////}

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // since the global navigation control is not loaded using DNN mechanisms we need to set it here so that calls to 
            // module related information will appear the same as the actual control this navigation is sitting on.hk
            this.ModuleConfiguration = ((PortalModuleBase)base.Parent).ModuleConfiguration;
            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + Localization.LocalResourceDirectory + "/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);

            this.Load += this.Page_Load;
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
                this.SetupLinks();
                this.SetVisibility();
                ////this.SetDisabledImages();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Sets up the URLs for each of the links.
        /// </summary>
        private void SetupLinks()
        {
            this.HomeLink.NavigateUrl = Globals.NavigateURL();
            this.SettingsLink.NavigateUrl = this.EditUrl("ModuleId", this.ModuleId.ToString(CultureInfo.InvariantCulture), "Module");
            this.AddAnEventLink.NavigateUrl = this.BuildLinkUrl(this.ModuleId, "EventEdit");
            this.ResponsesLink.NavigateUrl = this.BuildLinkUrl(this.ModuleId, "ResponseSummary");
            this.ManageEventsLink.NavigateUrl = this.BuildLinkUrl(this.ModuleId, "EventListingAdmin");
        }

        /// <summary>
        /// Sets the visibility.
        /// </summary>
        private void SetVisibility()
        {
            this.Visible = this.IsAdmin;
            this.SettingsLink.Visible = TabPermissionController.HasTabPermission("EDIT");
        }

        /////// <summary>
        /////// Sets the image for the current page to a disabled image, if appropriate.
        /////// </summary>
        ////private void SetDisabledImages()
        ////{
        ////    switch (this.CurrentControlKey)
        ////    {
        ////        case "EventEdit":
        ////            this.AddAnEventLink.ImageUrl = "~/DesktopModules/EngageEvents/Images/add_event_disabled.gif";
        ////            break;
        ////        case "EventListingAdmin":
        ////            this.ManageEventsLink.ImageUrl = "~/DesktopModules/EngageEvents/Images/manage_events_disabled.gif";
        ////            break;
        ////        case "ResponseSummaryDisplay":
        ////            this.ResponsesLink.ImageUrl = "~/DesktopModules/EngageEvents/Images/responses_disabled.gif";
        ////            break;
        ////        default:
        ////            this.HomeLink.ImageUrl = "~/DesktopModules/EngageEvents/Images/home_disabled.gif";
        ////            break;
        ////    }
        ////}
    }
}