// <copyright file="MainContainer.ascx.cs" company="Engage Software">
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
            else if (Engage.Utility.IsLoggedIn)
            {
                this.Response.Redirect(Globals.NavigateURL(this.TabId), true);
            }
            else
            {
                this.Response.Redirect(Dnn.Utility.GetLoginUrl(this.PortalSettings, this.Request), true);
            }
        }

        /// <summary>
        /// Fills <see cref="ControlKeys"/>.
        /// </summary>
        /// <returns>A dictionary mapping control keys to user controls.</returns>
        private static IDictionary<string, SubControlInfo> FillControlKeys()
        {
            IDictionary<string, SubControlInfo> keyDictionary = new Dictionary<string, SubControlInfo>(11, StringComparer.OrdinalIgnoreCase);

            keyDictionary.Add("EmailEdit", new SubControlInfo("EmailEdit.ascx", true));
            keyDictionary.Add("EventEdit", new SubControlInfo("EventEdit.ascx", true));
            keyDictionary.Add(DefaultControlKey, DefaultSubControl);
            keyDictionary.Add("EventListingAdmin", new SubControlInfo("Display/EventListingItem.ascx", true));
            keyDictionary.Add("ResponseSummary", new SubControlInfo("ResponseSummaryDisplay.ascx", true));
            keyDictionary.Add("ResponseDetail", new SubControlInfo("ResponseDetail.ascx", true));
            keyDictionary.Add("Response", new SubControlInfo("Respond.ascx", false));
            keyDictionary.Add("EmailAFriend", new SubControlInfo("EmailAFriend.ascx", false));
            keyDictionary.Add("Register", new SubControlInfo("Register.ascx", false));
            keyDictionary.Add("EventDetail", new SubControlInfo("Display/EventDetail.ascx", false));
            keyDictionary.Add("ChooseDisplay", new SubControlInfo("ChooseDisplay.ascx", true));

            return keyDictionary;
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
            if (Engage.Utility.HasValue(keyParam))
            {
                return ControlKeys[keyParam];
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