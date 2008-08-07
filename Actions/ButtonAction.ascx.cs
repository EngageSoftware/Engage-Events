// <copyright file="ButtonAction.ascx.cs" company="Engage Software">
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

    /// <summary>
    /// Displays the actions that users can perform on an event instance.
    /// </summary>
    /// <remarks>
    /// This control's behavior changed from using LinkButtons to standard buttons. Something to do with a postback
    /// not occurring on the container form. Not sure why? Anyhow, it stores the EventID in viewstate and uses it if needed.hk
    /// Note: the visibility of this control must be done outside by calling code.
    /// </remarks>
    public partial class ButtonAction : ActionControlBase
    {
        /// <summary>
        /// Backing field for <see cref="Href"/>
        /// </summary>
        private string href;

        /// <summary>
        /// Backing field for <see cref="Text"/>
        /// </summary>
        private string text;

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

        protected override void BindData()
        {
            this.LocalizeControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.Button.Click += this.Button_Click;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.Button.Text = this.text;
        }

        /// <summary>
        /// Handles the OnClick event of the EditEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, EventArgs e)
        {
            ////string href =
            ////    this.BuildLinkUrl(
            ////        "&modId=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) + "&key=EventEdit&eventId="
            ////        + this.CurrentEvent.Id.ToString(CultureInfo.InvariantCulture));
            
            this.Response.Redirect(this.href, true);
        }

        /// <summary>
        /// Localizes this control's child controls.
        /// </summary>
        private void LocalizeControls()
        {
        }
    }
}