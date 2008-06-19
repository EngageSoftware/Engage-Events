// <copyright file="EventListingAdmin.ascx.cs" company="Engage Software">
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
    using System.ComponentModel;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;

    // using Engage.Licensing;

    // [System.Runtime.InteropServices.GuidAttribute("2de915e1-df71-3443-9f4d-32259c92ced2")]
    // [LicenseProvider(typeof(EngageLicenseProvider))]

    /// <summary>
    /// The Event Listing Admin class allows for the management of events.
    /// </summary>
    public partial class EventListingAdmin : ModuleBase
    {
        #region Licensing

        /// <summary>
        /// Create a license
        /// </summary>
        private License license;

        /// <summary>
        /// Enables a server control to perform final clean up before it is released from memory.
        /// </summary>
        public override void Dispose()
        {
            if (this.license != null)
            {
                this.license.Dispose();
                this.license = null;
            }

            base.Dispose();
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                // adds Validate to the control's constructor.
                // license = LicenseManager.Validate(typeof(EventListingAdmin), this);
                if (!Page.IsPostBack)
                {
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }

        /// <summary>
        /// Handles the ItemDataBound event of the EventListingRepeater control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void EventListingRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EventAdminActions actions = (EventAdminActions)e.Item.FindControl("EventActions");
            actions.CurrentEvent = (Event)e.Item.DataItem;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the StatusRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void StatusRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the SortRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            bool showAll = false;

            if (StatusRadioButtonList.SelectedValue == "All")
            {
                showAll = true;
            }

            EventCollection events = EventCollection.Load(PortalId, SortRadioButtonList.SelectedValue, 0, 0, showAll);
            EventListingRepeater.DataSource = events;
            EventListingRepeater.DataBind();
        }

        #endregion
    }
}

