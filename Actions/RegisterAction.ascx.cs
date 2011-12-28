// <copyright file="RegisterAction.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Web.UI;

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
        /// Initializes a new instance of the <see cref="RegisterAction"/> class.
        /// </summary>
        protected RegisterAction()
        {
            this.CssClass = "Normal";
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.RegisterButton.Click += this.RegisterButton_Click;
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (IsLoggedIn)
            {
                this.SetupFancyBox();
            }

            this.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="RegisterButton"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(Dnn.Utility.GetLoginUrl(this.PortalSettings, this.Request));
        }

        /// <summary>
        /// Sets up the FancyBox plugin to allow registration within an on-page popup.
        /// </summary>
        private void SetupFancyBox()
        {
            this.AddJQueryReference();
            ScriptManager.RegisterClientScriptResource(this, typeof(RegisterAction), "Engage.Dnn.Events.JavaScript.EngageEvents.Actions.RegisterAction.combined.js");

            var parameters = Utility.GetEventParameters(
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