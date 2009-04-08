// <copyright file="RegisterAction.ascx.cs" company="Engage Software">
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
    using System.Web.UI;
    using DotNetNuke.Services.Localization;

    /// <summary>
    /// Displays the actions that users can perform on an event instance.
    /// </summary>
    /// <remarks>
    /// This control's behavior changed from using LinkButtons to standard buttons. Something to do with a postback
    /// not occurring on the container form. Not sure why? Anyhow, it stores the EventID in viewstate and uses it if needed. hk
    /// Note: the visibility of this control must be done outside by calling code.
    /// </remarks>
    public partial class RegisterAction : ActionControlBase
    {
        /// <summary>
        /// Performs all necessary operations to display the control's data correctly.
        /// </summary>
        protected override void BindData()
        {
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.RegisterButton.Click += this.RegisterButton_Click;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.LocalizeControls();
            if (IsLoggedIn)
            {
                this.SetupFancyBox();
            }
        }

        /// <summary>
        /// Handles the Click event of the <see cref="RegisterButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(Dnn.Utility.GetLoginUrl(this.PortalSettings, this.Request));
        }

        /// <summary>
        /// Localizes this control's child controls.
        /// </summary>
        private void LocalizeControls()
        {
            string resourceKey = IsLoggedIn ? "RegisterButton.Text" : "LoginToRegisterButton.Text";
            this.RegisterButton.Text = Localization.GetString(resourceKey, this.LocalResourceFile);
        }

        /// <summary>
        /// Sets up the FancyBox plugin to allow registration within an on-page popup.
        /// </summary>
        private void SetupFancyBox()
        {
            this.AddJQueryReference();
            ScriptManager.RegisterClientScriptResource(this, typeof(RegisterAction), "Engage.Dnn.Events.JavaScript.jquery.fancybox-1.0.0.js");

            string[] parameters = Utility.GetEventParameters(
                    this.CurrentEvent.Id,
                    this.CurrentEvent.EventStart,
                    "ModuleId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture),
                    "TabId=" + this.TabId.ToString(CultureInfo.InvariantCulture));

            this.PopupTriggerLink.NavigateUrl = string.Format(
                    CultureInfo.InvariantCulture,
                    "../RespondPage.aspx?{0}&{1}&{2}&{3}",
                    parameters);
        }
    }
}