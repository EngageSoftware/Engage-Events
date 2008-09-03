// <copyright file="CancelAction.ascx.cs" company="Engage Software">
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
    using Display;
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
    public partial class CancelAction : ActionControlBase
    {
        /// <summary>
        /// Occurs when the Cancel (or UnCancel) button is pressed.
        /// </summary>
        public event EventHandler Cancel;

        /// <summary>
        /// Sets the visibility of each of the buttons.  Also, sets the text for the cancel/uncancel button, and the delete confirm.
        /// </summary>
        protected override void BindData()
        {
        }

        /// <summary>
        /// Raises the <see cref="Cancel"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnCancel(EventArgs e)
        {
            this.InvokeCancel(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // since the global navigation control is not loaded using DNN mechanisms we need to set it here so that calls to 
            // module related information will appear the same as the actual control this navigation is sitting on.hk
            this.LocalResourceFile = "~" + DesktopModuleFolderName + "Navigation/App_LocalResources/EventAdminActions";
            this.Load += this.Page_Load;
            this.CancelButton.Click += this.CancelButton_Click;
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
        /// Handles the OnClick event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            // since we are reloading the object each time (no caching yet) you can't do the following
            ////this.CurrentEvent.Cancelled = !this.CurrentEvent.Cancelled;  hk

            Event ev = this.CurrentEvent;
            ev.Cancelled = !this.CurrentEvent.Cancelled;
            ev.Save(this.UserId);
            this.OnCancel(e);

            EventListingItem listing = Engage.Utility.FindParentControl<EventListingItem>(this);
            if (listing != null)
            {
                listing.BindData();
            }
        }

        /// <summary>
        /// Invokes the cancel.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void InvokeCancel(EventArgs e)
        {
            EventHandler cancelHandler = this.Cancel;
            if (cancelHandler != null)
            {
                cancelHandler(this, e);
            }
        }

        /// <summary>
        /// Sets the visibility of this control's child controls.
        /// </summary>
        private void SetVisibility()
        {
            this.CancelButton.Visible = this.IsAdmin;
        }

        /// <summary>
        /// Localizes this control's child controls.
        /// </summary>
        private void LocalizeControls()
        {
            this.CancelButton.Text = this.CurrentEvent.Cancelled
                                         ? Localization.GetString("UnCancel", this.LocalResourceFile)
                                         : Localization.GetString("Cancel", this.LocalResourceFile);

            ClientAPI.AddButtonConfirm(
                this.CancelButton, Localization.GetString(this.CurrentEvent.Cancelled ? "ConfirmUnCancel" : "ConfirmCancel", this.LocalResourceFile));
        }
    }
}