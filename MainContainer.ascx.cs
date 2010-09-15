// <copyright file="MainContainer.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2010
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using Framework;

    /// <summary>
    /// The main container that is used by the Engage: Events module.  
    /// This control is registered with DNN, and is in charge of loading other requested control.
    /// </summary>
    public partial class MainContainer : ModuleBase
    {
        /// <summary>
        /// The control key for the <see cref="DefaultSubControl"/>
        /// </summary>
        protected internal const string DefaultControlKey = "EventListing";

        /// <summary>
        /// The default sub-control to load when no control key is provided
        /// </summary>
        private static readonly SubControlInfo DefaultSubControl = new SubControlInfo("Display/EventDisplay.ascx", false);

        /// <summary>
        /// The sub-control to load when there is an error with the license
        /// </summary>
        private static readonly SubControlInfo LicenseErrorControl = new SubControlInfo("Admin/LicenseError.ascx", false);

        /// <summary>
        /// A dictionary mapping control keys to user controls.
        /// </summary>
        private static readonly IDictionary<string, SubControlInfo> ControlKeys = FillControlKeys();

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            SubControlInfo controlToLoad;
            try
            {
                base.OnInit(e);

                controlToLoad = this.GetControlToLoad();
            }
            catch (LicenseException)
            {
                controlToLoad = LicenseErrorControl;
            }

            if (!controlToLoad.RequiresEditPermission || PortalSecurity.HasNecessaryPermission(SecurityAccessLevel.Edit, this.PortalSettings, this.ModuleConfiguration, this.UserInfo.Username))
            {
                this.LoadChildControl(controlToLoad);
            }
            else
            {
                this.DenyAccess();
            }
        }

        /// <summary>
        /// Fills <see cref="ControlKeys"/>.
        /// </summary>
        /// <returns>A dictionary mapping control keys to user controls.</returns>
        private static IDictionary<string, SubControlInfo> FillControlKeys()
        {
            return new Dictionary<string, SubControlInfo>(11, StringComparer.OrdinalIgnoreCase)
                {
                    { "EventEdit", new SubControlInfo("EventEdit.ascx", false) },
                    { DefaultControlKey, DefaultSubControl },
                    { "EventListingAdmin", new SubControlInfo("Display/EventListingItem.ascx", false) },
                    { "ResponseSummary", new SubControlInfo("ResponseSummaryDisplay.ascx", false) },
                    { "ResponseDetail", new SubControlInfo("ResponseDetail.ascx", false) },
                    { "Response", new SubControlInfo("Respond.ascx", false) },
                    { "EmailAFriend", new SubControlInfo("EmailAFriend.ascx", false) },
                    { "Register", new SubControlInfo("Register.ascx", false) },
                    { "EventDetail", new SubControlInfo("Display/EventDetail.ascx", false) },
                    { "ChooseDisplay", new SubControlInfo("ChooseDisplay.ascx", false) },
                    { "ManageCategories", new SubControlInfo("ManageCategories.ascx", false) }
                };
        }

        /// <summary>
        /// Gets the control to load, based on the key (or lack thereof) that is passed on the querystring.
        /// </summary>
        /// <returns>A relative path to the control that should be loaded into this container</returns>
        private SubControlInfo GetControlToLoad()
        {
            if (!IsConfigured)
            {
                return new SubControlInfo("Admin/NotConfigured.ascx", false);
            }

            string keyParam = this.GetCurrentControlKey();
            SubControlInfo control;
            if (Engage.Utility.HasValue(keyParam) && ControlKeys.TryGetValue(keyParam, out control))
            {
                return control;
            }

            return DefaultSubControl;
        }

        /// <summary>
        /// Loads the child control to be displayed in this container.
        /// </summary>
        /// <param name="controlToLoad">The control to load.</param>
        private void LoadChildControl(SubControlInfo controlToLoad)
        {
            try
            {
                PortalModuleBase mb = (PortalModuleBase)this.LoadControl(controlToLoad.ControlPath);
                mb.ModuleConfiguration = this.ModuleConfiguration;
                mb.ID = Path.GetFileNameWithoutExtension(controlToLoad.ControlPath);
                this.phControls.Controls.Add(mb);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}