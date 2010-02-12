// <copyright file="DeleteAction.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2010
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
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;
    using Engage.Events;

    /// <summary>
    /// Displays the actions that users can perform on an event instance.
    /// </summary>
    /// <remarks>
    /// This control's behavior changed from using LinkButtons to standard buttons. Something to do with a postback
    /// not occurring on the container form. Not sure why? Anyhow, it stores the EventID in viewstate and uses it if needed.hk
    /// </remarks>
    public partial class DeleteAction : ActionControlBase
    {
        /// <summary>
        /// Occurs when the Delete button is pressed.
        /// </summary>
        public event EventHandler Delete;

        /// <summary>
        /// Sets the visibility of each of the buttons.  Also, sets the text for the cancel/uncancel button, and the delete confirm.
        /// </summary>
        protected override void BindData()
        {
        }

        /// <summary>
        /// Raises the <see cref="Delete"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnDelete(EventArgs e)
        {
            this.InvokeDelete(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += this.Page_Load;
            this.DeleteEventButton.Click += this.DeleteEventButton_Click;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.SetVisibility();
            this.LocalizeControls();
        }

        /// <summary>
        /// Handles the OnClick event of the DeleteEventButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DeleteEventButton_Click(object sender, EventArgs e)
        {
            Event.Delete(this.CurrentEvent.Id);
            this.OnDelete(e);

            ////EventListingItem listing = Engage.Utility.FindParentControl<EventListingItem>(this);
            ////if (listing != null)
            ////{
            ////    listing.BindData();
            ////}
        }

        /// <summary>
        /// Invokes the delete.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void InvokeDelete(EventArgs e)
        {
            EventHandler deleteHandler = this.Delete;
            if (deleteHandler != null)
            {
                deleteHandler(this, e);
            }
        }

        /// <summary>
        /// Sets the visibility of this control's child controls.
        /// </summary>
        private void SetVisibility()
        {
            this.DeleteEventButton.Visible = this.IsAdmin;
        }

        /// <summary>
        /// Localizes this control's child controls.
        /// </summary>
        private void LocalizeControls()
        {
            ClientAPI.AddButtonConfirm(this.DeleteEventButton, Localization.GetString("ConfirmDelete", this.LocalResourceFile));
        }
    }
}