// <copyright file="EventAdminActions.ascx.cs" company="Engage Software">
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
    using DotNetNuke.Entities.Modules;

    /// <summary>
    /// Displays the actions that users can perform on an event instance.
    /// </summary>
    /// <remarks>
    /// This control's behavior changed from using LinkButtons to standard buttons. Something to do with a postback
    /// not occurring on the container form. Not sure why? Anyhow, it stores the EventID in viewstate and uses it if needed.hk
    /// </remarks>
    public partial class ButtonAction : ActionControlBase
    {
        private string href;
        private string text;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Button.Click += this.Button_Click;
        }

        protected override void BindData()
        {
            this.LocalizeControls();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.SetVisibility();
            this.Button.Text = text;
        }

        /// <summary>
        /// Sets the visibility of this control's child controls.
        /// </summary>
        private void SetVisibility()
        {
            this.Button.Visible = this.IsAdmin;
        }

        /// <summary>
        /// Localizes this control's child controls.
        /// </summary>
        private void LocalizeControls()
        {
        }

        /// <summary>
        /// Handles the OnClick event of the EditEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, EventArgs e)
        {
            //string href =
            //    this.BuildLinkUrl(
            //        "&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit&eventId="
            //        + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture));
            
            this.Response.Redirect(href, true);
        }

        public string Href
        {
            get { return this.href; }
            set { this.href = value; }
        }

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
    }
}