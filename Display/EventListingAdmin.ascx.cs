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
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Exceptions;
    using Engage.Events;

    /// <summary>
    /// The Event Listing Admin class allows for the management of events.
    /// </summary>
    public partial class EventListingAdmin : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.SortRadioButtonList.SelectedIndexChanged += this.SortRadioButtonList_SelectedIndexChanged;
            this.StatusRadioButtonList.SelectedIndexChanged += this.StatusRadioButtonList_SelectedIndexChanged;
            this.EventListingRepeater.ItemDataBound += EventListingRepeater_ItemDataBound;
        }

        /// <summary>
        /// Handles the Cancel event of the EventActions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void EventActions_Cancel(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Handles the Delete event of the EventActions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void EventActions_Delete(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Handles the ItemDataBound event of the EventListingRepeater control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        private static void EventListingRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EventAdminActions eventActions = (EventAdminActions)e.Item.FindControl("EventActions");
            eventActions.CurrentEvent = (Event)e.Item.DataItem;
            ////eventActions.Delete += this.EventActions_Delete;
            ////eventActions.Cancel += this.EventActions_Cancel;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }    
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the StatusRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void StatusRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the SortRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SortRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            EventCollection events = EventCollection.Load(PortalId, SortRadioButtonList.SelectedValue, 0, 0, StatusRadioButtonList.SelectedValue == "All");
            EventListingRepeater.DataSource = events;
            EventListingRepeater.DataBind();
        }
    }
}

