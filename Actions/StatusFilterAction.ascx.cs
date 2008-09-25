// <copyright file="StatusFilterAction.ascx.cs" company="Engage Software">
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
    /// Allows the user to choose whether to display all events or only active events.
    /// </summary>
    /// <remarks>
    /// This control's behavior changed from using LinkButtons to standard buttons. Something to do with a postback
    /// not occurring on the container form. Not sure why? Anyhow, it stores the EventID in viewstate and uses it if needed.hk
    /// </remarks>
    public partial class StatusFilterAction : ActionControlBase
    {
        /// <summary>
        /// Occurs when the sort has changed.
        /// </summary>
        public event EventHandler SortChanged;

        /// <summary>
        /// Gets the selected status of event to display.
        /// </summary>
        /// <value>The selected status of event to display.</value>
        internal string SelectedValue
        {
            get { return this.StatusRadioButtonList.SelectedValue; }
        }

        /// <summary>
        /// Raises the <see cref="SortChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnSortChanged(EventArgs e)
        {
            this.InvokeSortChanged(e);
        }

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

            if (!this.IsPostBack)
            {
                this.SetInitialValue();
            }

            this.LocalResourceFile = TemplateResourceFile;
            this.StatusRadioButtonList.SelectedIndexChanged += this.StatusRadioButtonList_SelectedIndexChanged;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the StatusRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void StatusRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnSortChanged(e);
        }

        /// <summary>
        /// Invokes the sort changed.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void InvokeSortChanged(EventArgs e)
        {
            EventHandler handler = this.SortChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Sets the initial status value from the <c>QueryString</c>.
        /// </summary>
        private void SetInitialValue()
        {
            string status = this.Request.QueryString["status"];
            if (Engage.Utility.HasValue(status))
            {
                this.StatusRadioButtonList.SelectedValue = status;
            }
        }
    }
}