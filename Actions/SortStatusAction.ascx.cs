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

    /// <summary>
    /// Displays the actions that users can perform on an event instance.
    /// </summary>
    /// <remarks>
    /// This control's behavior changed from using LinkButtons to standard buttons. Something to do with a postback
    /// not occurring on the container form. Not sure why? Anyhow, it stores the EventID in viewstate and uses it if needed.hk
    /// </remarks>
    public partial class SortStatusAction : ActionControlBase
    {

        /// <summary>
        /// Occurs when the sort has changed.
        /// </summary>
        public event EventHandler SortChanged;


        /// <summary>
        /// Raises the <see cref="SortChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnSortChanged(EventArgs e)
        {
            this.InvokeSortChanged(e);
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
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.LocalResourceFile = "~" + DesktopModuleFolderName + "Display/App_LocalResources/EventListingItem.ascx.resx";
            this.RadioButtonListStatusSort.SelectedIndexChanged += this.RadioButtonListStatusSort_SelectedIndexChanged;
        }

        private void RadioButtonListStatusSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnSortChanged(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.SetVisibility();
            this.LocalizeControls();
        }

     
        /// <summary>
        /// Sets the visibility of each of the buttons.  Also, sets the text for the cancel/uncancel button, and the delete confirm.
        /// </summary>
        protected override void BindData()
        {
        }

        /// <summary>
        /// Sets the visibility of this control's child controls.
        /// </summary>
        private void SetVisibility()
        {
        }

        /// <summary>
        /// Localizes this control's child controls.
        /// </summary>
        private void LocalizeControls()
        {
        }

        internal string SelectedValue
        {
            get { return this.RadioButtonListStatusSort.SelectedValue; }
        }
    }
}