// <copyright file="StatusFilterAction.ascx.cs" company="Engage Software">
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
    using System.Web.UI.WebControls;

    using Engage.Util;

    /// <summary>
    /// Allows the user to choose whether to display all events or only active events.
    /// </summary>
    public partial class StatusFilterAction : ActionControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusFilterAction"/> class.
        /// </summary>
        protected StatusFilterAction()
        {
            this.CssClass = "Normal";
        }

        /// <summary>
        /// Occurs when the sort has changed.
        /// </summary>
        public event EventHandler SortChanged = (_, __) => { };

        /// <summary>
        /// Gets the selected status of event to display.
        /// </summary>
        /// <value>The selected status of event to display.</value>
        internal string SelectedValue
        {
            get { return this.StatusRadioButtonList.SelectedValue; }
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.IsPostBack)
            {
                this.SetInitialValue();
            }

            this.Load += this.Page_Load;
            this.StatusRadioButtonList.SelectedIndexChanged += this.StatusRadioButtonList_SelectedIndexChanged;
        }

        /// <summary>
        /// Sets the initial status value from the <c>QueryString</c>.
        /// </summary>
        private void SetInitialValue()
        {
            var status = this.Request.QueryString["status"];
            if (!Engage.Utility.HasValue(status))
            {
                return;
            }

            this.StatusRadioButtonList.SetSelectedString(status);
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs eventArgs)
        {
            this.DataBind();
        }

        /// <summary>
        /// Handles the <see cref="ListControl.SelectedIndexChanged"/> event of the <see cref="StatusRadioButtonList"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StatusRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnSortChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="SortChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSortChanged(EventArgs e)
        {
            this.SortChanged(this, e);
        }
    }
}