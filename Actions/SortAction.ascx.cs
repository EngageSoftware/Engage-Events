// <copyright file="SortAction.ascx.cs" company="Engage Software">
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

    /// <summary>
    /// Allows the user to choose whether to sort the events by date or title.
    /// </summary>
    public partial class SortAction : ActionControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortAction"/> class.
        /// </summary>
        protected SortAction()
        {
            this.CssClass = "Normal";
        }

        /// <summary>
        /// Occurs when the sort has changed.
        /// </summary>
        public event EventHandler SortChanged = (_, __) => { };

        /// <summary>
        /// Gets the selected field by which to sort the event listing.
        /// </summary>
        /// <value>The selected field by which to sort the event listing</value>
        internal string SelectedValue
        {
            get { return this.SortRadioButtonList.SelectedValue; }
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
            this.SortRadioButtonList.SelectedIndexChanged += this.SortRadioButtonList_SelectedIndexChanged;
        }

        /// <summary>
        /// Sets the initial value for the sort, based on the <c>QueryString</c>.
        /// </summary>
        private void SetInitialValue()
        {
            var sortValue = this.Request.QueryString["sort"];
            if (!Engage.Utility.HasValue(sortValue))
            {
                return;
            }

            this.SortRadioButtonList.SelectedValue = "TITLE".Equals(sortValue, StringComparison.OrdinalIgnoreCase)
                                                         ? Utility.GetPropertyName(e => e.Title)
                                                         : Utility.GetPropertyName(e => e.EventStart);
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
        /// Handles the <see cref="ListControl.SelectedIndexChanged"/> event of the <see cref="SortRadioButtonList"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
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