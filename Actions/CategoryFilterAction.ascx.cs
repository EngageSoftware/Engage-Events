// <copyright file="CategoryFilterAction.ascx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Engage.Events;
    using Engage.Util;

    /// <summary>
    /// Allows the user to filter the list to one category
    /// </summary>
    public partial class CategoryFilterAction : ActionControlBase
    {
        /// <summary>
        /// Occurs when the sort has changed.
        /// </summary>
        public event EventHandler CategoryChanged = (_, __) => { };

        /// <summary>
        /// Gets the ID of the category of event to display.
        /// </summary>
        /// <value>The category ID, or <c>null</c> for all categories.</value>
        internal int? SelectedCategoryId
        {
            get
            {
                int selectedValue;
                if (int.TryParse(this.CategoriesList.SelectedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out selectedValue))
                {
                    return selectedValue;
                }

                return null;
            }
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
            this.DataBind();

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
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.IsPostBack)
            {
                this.BindData();
                this.SetInitialValue();
            }

            this.CategoriesList.SelectedIndexChanged += this.CategoriesList_SelectedIndexChanged;
        }

        /// <summary>
        /// Sets the initial status value from the <c>QueryString</c>.
        /// </summary>
        private void SetInitialValue()
        {
            var categoryIds = this.Session["categoryIds"] as int[];
            if (categoryIds == null || categoryIds.Length <= 0)
            {
                return;
            }

            this.CategoriesList.SetSelectedInt32(categoryIds[0]);
        }

        /// <summary>
        /// Handles the <see cref="ListControl.SelectedIndexChanged"/> event of the <see cref="CategoriesList"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CategoriesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnCategoryChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="CategoryChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCategoryChanged(EventArgs e)
        {
            this.CategoryChanged(this, e);
        }
    }
}