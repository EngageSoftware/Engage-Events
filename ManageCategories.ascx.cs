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
        /// Backing field for <see cref="ParentCategories"/>
        /// </summary>
        private IEnumerable<Category> parentCategories;

        /// <summary>
        /// Gets the parent categories.
        /// </summary>
        protected IEnumerable<Category> ParentCategories
        {
            get
            {
                yield return Category.Create(this.PortalId, null, this.Localize("NoParent"), string.Empty);

                foreach (var parentCategory in this.parentCategories)
                {
                    yield return parentCategory;
                }
            }
        }

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
            this.CategoriesGrid.ColumnCreated += this.CategoriesGrid_ColumnCreated;
            this.CategoriesGrid.DeleteCommand += this.CategoriesGrid_DeleteCommand;
            this.CategoriesGrid.ItemCreated += this.CategoriesGrid_ItemCreated;
            this.CategoriesGrid.NeedDataSource += this.CategoriesGrid_NeedDataSource;
            this.CategoriesGrid.InsertCommand += this.CategoriesGrid_InsertCommand;
            this.CategoriesGrid.UpdateCommand += this.CategoriesGrid_UpdateCommand;
            this.CategoriesGrid.ItemDataBound += this.CategoriesGrid_ItemDataBound;
            this.PreRender += (s, o) => this.HideExpandColumnRecursive(this.CategoriesGrid.MasterTableView);

            this.parentCategories = CategoryCollection.Load(this.PortalId);
            foreach (var category in this.parentCategories.Where(c => string.IsNullOrEmpty(c.Name)))
            {
                category.Name = this.GetDefaultCategoryName();
            }
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
        /// Finds the name of the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>The category name of the given id</returns>
        protected string FindCategoryName(int? categoryId)
        {
            if (!categoryId.HasValue)
            {
                return string.Empty;
            }

            var category = this.parentCategories.Where(c => c.Id == categoryId).FirstOrDefault();
            return category != null ? category.Name : string.Empty;
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.DeleteCommand"/> event of the <see cref="CategoriesGrid"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="GridCommandEventArgs"/> instance containing the event data.</param>
        private void CategoriesGrid_DeleteCommand(object source, GridCommandEventArgs e)
        {
            var categoryId = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"];
            Category.Delete(categoryId);
        }

        /// <summary>
        /// Handles the <see cref="RadGrid.ItemCreated"/> event of the <see cref="CategoriesGrid"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridItemEventArgs"/> instance containing the event data.</param>
        private void CategoriesGrid_ItemCreated(object sender, GridItemEventArgs e)
        {
            this.CreateExpandCollapseButton(e.Item, "Name");

            var commandItem = e.Item as GridCommandItem;
            if (commandItem != null)
            {
                if (commandItem.OwnerTableView != this.CategoriesGrid.MasterTableView)
                {
                    commandItem.Visible = false;
                }

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

                // hide grid header for nested grid.
                if (e.Item is GridHeaderItem && e.Item.OwnerTableView != this.CategoriesGrid.MasterTableView)
                {
                    e.Item.Style["display"] = "none";
                }

                if (e.Item is GridNestedViewItem)
                {
                    e.Item.Cells[0].Visible = false;
                }

                if (e.Item is GridNoRecordsItem && e.Item.OwnerTableView != this.CategoriesGrid.MasterTableView)
                {
                    e.Item.OwnerTableView.Visible = false;
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

            var newValues = new Dictionary<string, string>(3);
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, (GridEditableItem)e.Item);
            int parentId;
            var category = Category.Create(
                this.PortalId,
                int.TryParse(((RadComboBox)e.Item.FindControl("ParentCategoriesComboBox")).SelectedValue, out parentId) ? parentId : (int?)null,
                newValues["Name"],
                newValues["Color"]);
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

            var newValues = new Dictionary<string, string>(3);
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, (GridEditableItem)e.Item);
            int parentId;
            category.ParentId = int.TryParse(((RadComboBox)e.Item.FindControl("ParentCategoriesComboBox")).SelectedValue, out parentId) ? parentId : (int?)null;
            category.Name = newValues["Name"];
            category.Color = newValues["Color"];
            if (string.IsNullOrEmpty(category.Color))
            {
                category.Color = null;
            }

            category.Save(this.UserId);

            this.SuccessModuleMessage.Visible = true;
            this.SuccessModuleMessage.Text = this.Localize("CategoryUpdateSuccess");

            this.CategoriesGrid.Rebind();
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
                    this.CategoriesGrid.MasterTableView.FilterExpression = "ParentId == NULL";
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

            // TODO: Fix this => this doesn't work on nested self referencing hierarchy grid.
            ////var editColumn = (GridEditCommandColumn)this.CategoriesGrid.Columns.FindByUniqueName("EditButtons");
            ////editColumn.EditText = this.Localize("EditCategory.Text");
            ////editColumn.CancelText = this.Localize("CancelEdit.Text");
            ////editColumn.UpdateText = this.Localize("UpdateCategory.Text");
            ////editColumn.InsertText = this.Localize("CreateCategory.Text");

            ////var deleteColumn = (GridButtonColumn)this.CategoriesGrid.Columns.FindByUniqueName("Delete");
            ////deleteColumn.Text = this.Localize("DeleteCategory.Text");
            ////deleteColumn.ConfirmText = this.Localize("DeleteConfirmation.Text");

            var parentIdName = (GridTemplateColumn)this.CategoriesGrid.Columns.FindByUniqueName("ParentId");
            parentIdName.HeaderText = this.Localize("ParentId.Header");

            var categoryNameColumn = (GridTemplateColumn)this.CategoriesGrid.Columns.FindByUniqueName("Name");
            categoryNameColumn.HeaderText = this.Localize("Name.Header");

            var colorColumn = (GridTemplateColumn)this.CategoriesGrid.Columns.FindByUniqueName("Color");
            colorColumn.HeaderText = this.Localize("Color.Header");
        }

        /// <summary>
        /// Handles the ItemDataBound event of the CategoriesGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.GridItemEventArgs"/> instance containing the event data.</param>
        private void CategoriesGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            this.CreateExpandCollapseButton(e.Item, "Name");

            if (e.Item is GridEditableItem && (e.Item as GridEditableItem).IsInEditMode)
            {
                var editedItem = e.Item as GridEditableItem;
                var category = e.Item.DataItem as Category;
                var dropDown = editedItem.FindControl("ParentCategoriesComboBox") as RadComboBox;
                if (dropDown != null)
                {
                    dropDown.DataSource = (category != null)
                                              ? this.ParentCategories.Where(
                                                  c => c.Id != category.Id)
                                              : this.ParentCategories;
                    dropDown.DataBind();
                    dropDown.SelectedValue = (category != null && category.ParentId.HasValue)
                                                 ? category.ParentId.Value.ToString(CultureInfo.CurrentCulture)
                                                 : string.Empty;
                }
            }
        }

        /// <summary>
        /// Creates the expand collapse button.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="columnUniqueName">Name of the column unique.</param>
        private void CreateExpandCollapseButton(GridItem item, string columnUniqueName)
        {
            if (item is GridDataItem)
            {
                if (item.FindControl("MyExpandCollapseButton") == null)
                {
                    Button button = null;
                    try
                    {
                        button = new Button();
                        button.Click += this.Button_Click;
                        button.CommandName = "ExpandCollapse";
                        button.CssClass = item.Expanded ? "rgCollapse" : "rgExpand";
                        button.ID = "MyExpandCollapseButton";

                        if (item.OwnerTableView.HierarchyLoadMode == GridChildLoadMode.Client)
                        {
                            var script = string.Format(
                                @"$find(""{0}"")._toggleExpand(this, event); return false;", item.Parent.Parent.ClientID);

                            button.OnClientClick = script;
                        }

                        var level = item.ItemIndexHierarchical.Split(':').Length - 1;

                        button.Style["margin-left"] = (level * 15) + "px";

                        var cell = ((GridDataItem)item)[columnUniqueName];
                        cell.FindControl("ExpandCollapseButtonPlaceHolder").Controls.Add(button);
                    }
                    catch
                    {
                        if (button != null)
                        {
                            button.Dispose();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, EventArgs e)
        {
            ((Button)sender).CssClass = (((Button)sender).CssClass == "rgExpand") ? "rgCollapse" : "rgExpand";
        }

        /// <summary>
        /// Handles the ColumnCreated event of the CategoriesGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.GridColumnCreatedEventArgs"/> instance containing the event data.</param>
        private void CategoriesGrid_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (e.Column is GridExpandColumn)
            {
                e.Column.Visible = false;
            }
        }

        /// <summary>
        /// Hides the expand column recursive.
        /// </summary>
        /// <param name="tableView">The table view.</param>
        private void HideExpandColumnRecursive(GridTableView tableView)
        {
            GridItem[] nestedViewItems = tableView.GetItems(GridItemType.NestedView);
            foreach (GridNestedViewItem nestedViewItem in nestedViewItems)
            {
                foreach (GridTableView nestedView in nestedViewItem.NestedTableViews)
                {
                    nestedView.Style["border"] = "0";

                    var myExpandCollapseButton = (Button)nestedView.ParentItem.FindControl("MyExpandCollapseButton");
                    if (nestedView.Items.Count == 0)
                    {
                        if (myExpandCollapseButton != null)
                        {
                            myExpandCollapseButton.Style["visibility"] = "hidden";
                        }
                        nestedViewItem.Visible = false;
                    }
                    else
                    {
                        if (myExpandCollapseButton != null)
                        {
                            myExpandCollapseButton.Style.Remove("visibility");
                        }
                    }

                    if (nestedView.HasDetailTables)
                    {
                        this.HideExpandColumnRecursive(nestedView);
                    }
                }
            }
        } 
    }
}