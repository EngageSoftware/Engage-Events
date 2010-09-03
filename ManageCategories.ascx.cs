// <copyright file="ManageCategories.ascx.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;

    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.UI.Utilities;

    using Engage.Events;

    using Telerik.Web.UI;

    /// <summary>
    /// Control to allow module editors to manage the categories for this portal
    /// </summary>
    public partial class ManageCategories : ModuleBase
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += this.Page_Load;
            this.CategoriesGrid.ItemCreated += CategoriesGrid_ItemCreated;
            this.CategoriesGrid.NeedDataSource += this.CategoriesGrid_NeedDataSource;
            this.CategoriesGrid.InsertCommand += this.CategoriesGrid_InsertCommand;
            this.CategoriesGrid.UpdateCommand += this.CategoriesGrid_UpdateCommand;
        }

        /// <summary>
        /// Gets the name of the default category.
        /// </summary>
        /// <returns>The default category's display name</returns>
        protected string GetDefaultCategoryName()
        {
            return this.Localize("DefaultCategory.Text", this.LocalSharedResourceFile);
        }

        /// <summary>
        /// Handles the <see cref="CustomValidator.ServerValidate"/> event of the <c>UniqueNameValidator</c> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="ServerValidateEventArgs"/> instance containing the event data.</param>
        protected void UniqueNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !CategoryCollection.Load(this.PortalId).Any(category => category.Name.Equals(args.Value, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.ItemCreated"/> event of the <see cref="CategoriesGrid"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridItemEventArgs"/> instance containing the event data.</param>
        private static void CategoriesGrid_ItemCreated(object sender, GridItemEventArgs e)
        {
            var commandItem = e.Item as GridCommandItem;
            if (commandItem != null)
            {
                // control names from http://www.telerik.com/help/aspnet-ajax/grddefaultbehavior.html
                commandItem.FindControl("RefreshButton").Visible = false;
                commandItem.FindControl("RebindGridButton").Visible = false;
            }
            else if (e.Item.IsInEditMode)
            {
                var editableItem = e.Item as GridEditableItem;
                if (editableItem != null)
                {
                    const int EnterKey = 13;
                    ClientAPI.RegisterKeyCapture(
                        editableItem["Name"].Controls.OfType<TextBox>().Single(), 
                        editableItem["Buttons"].Controls[0], 
                        EnterKey);
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.NeedDataSource"/> event of the <see cref="CategoriesGrid"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridNeedDataSourceEventArgs"/> instance containing the event data.</param>
        private void CategoriesGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            this.CategoriesGrid.DataSource = CategoryCollection.Load(this.PortalId);
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.InsertCommand"/> event of the <see cref="CategoriesGrid"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridCommandEventArgs"/> instance containing the event data.</param>
        private void CategoriesGrid_InsertCommand(object source, GridCommandEventArgs e)
        {
            if (!this.Page.IsValid)
            {
                e.Canceled = true;
                return;
            }

            var newValues = new Dictionary<string, string>(1);
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, (GridEditableItem)e.Item);
            var category = Category.Create(this.PortalId, newValues["Name"]);
            category.Save(this.UserId);

            this.SuccessModuleMessage.Visible = true;
            this.SuccessModuleMessage.Text = this.Localize("CategoryInsertSuccess");
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.UpdateCommand"/> event of the <see cref="CategoriesGrid"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridUpdatedEventArgs"/> instance containing the event data.</param>
        private void CategoriesGrid_UpdateCommand(object source, GridCommandEventArgs e)
        {
            if (!this.Page.IsValid)
            {
                e.Canceled = true;
                return;
            }

            var categoryId = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"];
            var category = Category.Load(categoryId);
            if (category == null)
            {
                e.Canceled = true;
                return;
            }

            var newValues = new Dictionary<string, string>(1);
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, (GridEditableItem)e.Item);
            category.Name = newValues["Name"];
            category.Save(this.UserId);

            this.SuccessModuleMessage.Visible = true;
            this.SuccessModuleMessage.Text = this.Localize("CategoryUpdateSuccess");
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
                if (!this.IsPostBack)
                {
                    this.LocalizeGrid();
                }

                this.SuccessModuleMessage.Visible = false;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Localizes the <see cref="CategoriesGrid"/>.
        /// </summary>
        private void LocalizeGrid()
        {
            this.CategoriesGrid.MasterTableView.CommandItemSettings.AddNewRecordText = this.Localize("AddCategory.Text");

            var categoryNameColumn = (GridTemplateColumn)this.CategoriesGrid.Columns.FindByUniqueName("Name");
            categoryNameColumn.HeaderText = this.Localize("Name.Header");
        }
    }
}