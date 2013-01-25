// <copyright file="ButtonAction.ascx.cs" company="Engage Software">
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
    using System.Web.UI;

    /// <summary>
    /// Displays the actions that users can perform on an event instance.
    /// </summary>
    /// <remarks>
    /// Note: the visibility of this control must be done outside by calling code.
    /// </remarks>
    public partial class ButtonAction : ActionControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonAction"/> class.
        /// </summary>
        protected ButtonAction()
        {
            this.CssClass = "Normal";
        }

        /// <summary>
        /// Gets or sets the URL to navigate to when this button is clicked.
        /// </summary>
        /// <value>The URL to navigate to when this button is clicked.</value>
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets the localization resource key whose value is the text to display on this button.
        /// </summary>
        /// <value>The resource key for this button's text</value>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.Button.Click += this.Button_Click;
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();
        }

        /// <summary>
        /// Handles the <see cref="System.Web.UI.WebControls.Button.Click"/> event of the <see cref="Button"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, EventArgs e)
        {            
            this.Response.Redirect(this.Href, true);
        }
    }
}