// <copyright file="ManageCategories.ascx.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI;
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
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!this.PermissionsService.CanManageCategories)
            {
                this.DenyAccess();
            }

            base.OnInit(e);

            this.Load += this.Page_Load;
            this.CategoriesGrid.DeleteCommand += CategoriesGrid_DeleteCommand;
            this.CategoriesGrid.ItemCreated += CategoriesGrid_ItemCreated;
            this.CategoriesGrid.NeedDataSource += this.CategoriesGrid_NeedDataSource;
            this.CategoriesGrid.InsertCommand += this.CategoriesGrid_InsertCommand;
            this.CategoriesGrid.UpdateCommand += this.CategoriesGrid_UpdateCommand;
        }

        /// <summary>
        /// Gets the text to display for the given color value.
        /// </summary>
        /// <param name="color">The value for the color to display.</param>
        /// <returns>
        /// The the localized version of the given color, if available
        /// </returns>
        protected string GetColorName(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                return this.Localize("NoColor.Text");
            }

            var localizedColorName = this.Localize(color);
            return string.IsNullOrEmpty(localizedColorName) ? color : localizedColorName;
        }

        /// <summary>
        /// Handles the <see cref="CustomValidator.ServerValidate"/> event of the <c>UniqueNameValidator</c> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="ServerValidateEventArgs"/> instance containing the event data.</param>
        protected void UniqueNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var gridItem = Engage.Utility.FindParentControl<GridDataItem>((CustomValidator)source);
            var categoryId = gridItem.ItemIndex >= 0 ? (int)gridItem.OwnerTableView.DataKeyValues[gridItem.ItemIndex]["Id"] : -1;
            args.IsValid = !CategoryCollection.Load(this.PortalId).Any(category => category.Id != categoryId && category.Name.Equals(args.Value, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.DeleteCommand"/> event of the <see cref="CategoriesGrid"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridCommandEventArgs"/> instance containing the event data.</param>
        private static void CategoriesGrid_DeleteCommand(object source, GridCommandEventArgs e)
        {
            var categoryId = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"];
            Category.Delete(categoryId);
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
            else
            {
                var editableItem = e.Item as GridEditableItem;
                if (editableItem != null && e.Item.IsInEditMode)
                {
                    const int EnterKey = 13;
                    ClientAPI.RegisterKeyCapture(
                        editableItem["Name"].Controls.OfType<TextBox>().Single(),
                        editableItem["EditButtons"].Controls[0],
                        EnterKey);
                }
                else
                {
                    var normalItem = e.Item as GridDataItem;
                    if (normalItem != null && e.Item.DataItem != null)
                    {
                        var category = (Category)e.Item.DataItem;
                        normalItem["Delete"].Controls.OfType<LinkButton>().Single().Visible = category.EventCount == 0;
                    }
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
            IEnumerable<Category> categories = CategoryCollection.Load(this.PortalId);
            if (this.CategoryIds.Any())
            {
                categories = categories.Where(category => this.CategoryIds.Contains(category.Id));
            }

            this.CategoriesGrid.DataSource = categories;
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

            var newValues = new Dictionary<string, string>(2);
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, (GridEditableItem)e.Item);
            var category = Category.Create(this.PortalId, newValues["Name"], newValues["Color"]);
            category.Save(this.UserId);

            // if this is a new category and the categories are restricted in the settings, make sure that this category can show up in this module
            if (this.CategoryIds.Any())
            {
                ModuleSettings.Categories.Set(this, ModuleSettings.Categories.GetValueAsStringFor(this) + "," + category.Id.ToString(CultureInfo.InvariantCulture));
                this.ClearCategoryIdsCache();
            }

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

            var newValues = new Dictionary<string, string>(2);
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, (GridEditableItem)e.Item);
            category.Name = newValues["Name"];
            category.Color = newValues["Color"];
            if (string.IsNullOrEmpty(category.Color))
            {
                category.Color = null;
            }

            category.Save(this.UserId);

            this.SuccessModuleMessage.Visible = true;
            this.SuccessModuleMessage.Text = this.Localize("CategoryUpdateSuccess");
        }

        /// <summary>
        /// Raises the <see cref="Control.Load"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> object that contains the event data.</param>
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
            this.CategoriesGrid.MasterTableView.NoMasterRecordsText = this.Localize("NoCategories.Text");
            this.CategoriesGrid.MasterTableView.CommandItemSettings.AddNewRecordText = this.Localize("AddCategory.Text");

            var editColumn = (GridEditCommandColumn)this.CategoriesGrid.Columns.FindByUniqueName("EditButtons");
            editColumn.EditText = this.Localize("EditCategory.Text");
            editColumn.CancelText = this.Localize("CancelEdit.Text");
            editColumn.UpdateText = this.Localize("UpdateCategory.Text");
            editColumn.InsertText = this.Localize("CreateCategory.Text");

            var categoryNameColumn = (GridTemplateColumn)this.CategoriesGrid.Columns.FindByUniqueName("Name");
            categoryNameColumn.HeaderText = this.Localize("Name.Header");

            var colorColumn = (GridTemplateColumn)this.CategoriesGrid.Columns.FindByUniqueName("Color");
            colorColumn.HeaderText = this.Localize("Color.Header");

            var deleteColumn = (GridButtonColumn)this.CategoriesGrid.Columns.FindByUniqueName("Delete");
            deleteColumn.Text = this.Localize("DeleteCategory.Text");
            deleteColumn.ConfirmText = this.Localize("DeleteConfirmation.Text");
        }
    }
}