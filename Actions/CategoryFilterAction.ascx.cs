// <copyright file="CategoryFilterAction.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Linq;
    using System.Web.UI.WebControls;

    using Engage.Events;

    /// <summary>
    /// Allows the user to choose whether to display all events or only active events.
    /// </summary>
    public partial class CategoryFilterAction : ActionControlBase
    {
        /// <summary>
        /// Occurs when the sort has changed.
        /// </summary>
        public event EventHandler CategoryChanged;

        /// <summary>
        /// Gets the selected status of event to display.
        /// </summary>
        /// <value>The selected status of event to display.</value>
        internal string SelectedValue
        {
            get { return this.CategoriesList.SelectedValue; }
        }

        /// <summary>
        /// Raises the <see cref="CategoryChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnCategoryChanged(EventArgs e)
        {
            this.InvokeCategoryChanged(e);
        }

        /// <summary>
        /// Performs all necessary operations to display the control's data correctly.
        /// </summary>
        protected override void BindData()
        {
            this.CategoriesList.DataTextField = "Name";
            this.CategoriesList.DataValueField = "Id";
            this.CategoriesList.DataSource = from category in CategoryCollection.Load(this.PortalId)
                                             where !this.CategoryIds.Any() || this.CategoryIds.Contains(category.Id)
                                             select new
                                                 {
                                                     Name = string.IsNullOrEmpty(category.Name)
                                                                ? this.Localize("DefaultCategory.Text", this.LocalSharedResourceFile)
                                                                : category.Name,
                                                     Id = category.Id.ToString(CultureInfo.InvariantCulture)
                                                 };
            this.CategoriesList.DataBind();

            if (this.CategoriesList.Items.Count > 1)
            {
                this.CategoriesList.Items.Insert(0, new ListItem(this.Localize("AllListItem.Text"), string.Empty));
            }
            else
            {
                this.CategoriesList.Enabled = false;
            }
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
                this.BindData();
                this.SetInitialValue();
            }

            this.CategoriesList.SelectedIndexChanged += this.StatusRadioButtonList_SelectedIndexChanged;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the StatusRadioButtonList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void StatusRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnCategoryChanged(e);
        }

        /// <summary>
        /// Invokes the <see cref="CategoryChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void InvokeCategoryChanged(EventArgs e)
        {
            EventHandler handler = this.CategoryChanged;
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
            string categoryId = this.Request.QueryString["catId"];
            if (Engage.Utility.HasValue(categoryId) && this.CategoriesList.Items.Cast<ListItem>().Any(item => item.Value == categoryId))
            {
                this.CategoriesList.SelectedValue = categoryId;
            }
        }
    }
}