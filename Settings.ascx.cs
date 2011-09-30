// <copyright file="Settings.ascx.cs" company="Engage Software">
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
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.UI.WebControls;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;

    using Engage.Events;

    using Telerik.Web.UI;

    /// <summary>
    /// This is the settings code behind for Event related Settings.
    /// </summary>
    public partial class Settings : SettingsBase
    {
        /// <summary>
        /// Backing field for <see cref="Categories"/>
        /// </summary>
        private string[] categories;
        
        /// <summary>
        /// Backing field for the current module settings base selected.
        /// </summary>
        private ModuleSettingsBase specificSettingsControl;

        /// <summary>
        /// Gets the selected categories from module settings.
        /// </summary>
        private string[] Categories
        {
            get
            {
                if (this.categories == null)
                {
                    var categoriesSettingValue = Dnn.Events.ModuleSettings.Categories.GetValueAsStringFor(this);
                    this.categories = categoriesSettingValue.Split(',');
                }

                return this.categories;
            }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                if (!this.IsPostBack)
                {
#pragma warning disable 618 // Can't transition to DNN's LocalizeGridView until we're on DNN 4.6
                    Dnn.Utility.LocalizeGridView(ref this.DetailsDisplayModuleGrid, this.LocalResourceFile);
#pragma warning restore 618
                    this.DetailsDisplayModuleGrid.DataSource = new ModuleController().GetModulesByDefinition(this.PortalId, Utility.ModuleDefinitionFriendlyName);
                    this.DetailsDisplayModuleGrid.DataBind();

                    this.CategoriesCheckBoxTreeView.DataTextField = "Name";
                    this.CategoriesCheckBoxTreeView.DataValueField = "Id";
                    this.CategoriesCheckBoxTreeView.DataFieldID = "Id";
                    this.CategoriesCheckBoxTreeView.DataFieldParentID = "ParentId";
                    var categoryList = (from category in CategoryCollection.Load(this.PortalId)
                                      select
                                          new
                                              {
                                                  Name =
                                          string.IsNullOrEmpty(category.Name)
                                              ? this.Localize("DefaultCategory", this.LocalSharedResourceFile)
                                              : category.Name,
                                                  Id = category.Id.ToString(CultureInfo.InvariantCulture),
                                                  ParentId =
                                          category.ParentId.HasValue
                                              ? category.ParentId.Value.ToString(CultureInfo.InvariantCulture)
                                              : string.Empty
                                              }).ToList();
                    categoryList.Add(new { Name = this.Localize("All Categories.Text"), Id = string.Empty, ParentId = (string)null });
                    this.CategoriesCheckBoxTreeView.DataSource = categoryList;
                    this.CategoriesCheckBoxTreeView.DataBind();

                    this.SetOptions();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Updates the settings.
        /// </summary>
        public override void UpdateSettings()
        {
            if (this.Page.IsValid)
            {
                try
                {
                    Dnn.Events.ModuleSettings.FeaturedOnly.Set(this, this.FeaturedCheckBox.Checked);
                    Dnn.Events.ModuleSettings.HideFullEvents.Set(this, this.HideFullEventsCheckBox.Checked);
                    Dnn.Events.ModuleSettings.DetailsDisplayTabId.Set(this, this.GetSelectedDetailsDisplayTabId());
                    Dnn.Events.ModuleSettings.DetailsDisplayModuleId.Set(this, this.GetSelectedDetailsDisplayModuleId());

                    string cats;
                    var nodes = this.CategoriesCheckBoxTreeView.Nodes.Cast<RadTreeNode>().ToList();
                    if (nodes.Count() > 0 && nodes.First().Checked)
                    {
                        cats = string.Empty;
                    }
                    else
                    {
                        ////var selectedCategoryIds = this.CategoriesCheckBoxTreeView.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => item.Value);
                        var selectedCategoryIds = this.CategoriesCheckBoxTreeView.CheckedNodes.Select(
                            node => node.Value);
                        cats = string.Join(",", selectedCategoryIds.ToArray());
                    }

                    Dnn.Events.ModuleSettings.Categories.Set(this, cats);

                    this.specificSettingsControl.UpdateSettings();
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += this.Page_Load;
            ////this.AllCategoriesCheckBox.CheckedChanged += this.AllCategoriesCheckBox_CheckedChanged;
            this.CategoriesCheckBoxTreeView.NodeCheck += this.CategoriesCheckBoxTreeView_NodeCheck;
            this.CategoriesCheckBoxTreeView.NodeClick += this.CategoriesCheckBoxTreeView_NodeClick;
            this.CategoriesListValidator.ServerValidate += this.CategoriesListValidator_ServerValidate;
            this.DetailsDisplayModuleValidator.ServerValidate += this.DetailsDisplayModuleValidator_ServerValidate;
            this.CategoriesCheckBoxTreeView.NodeCreated += this.CategoriesCheckBoxTreeView_NodeCreated;
            this.CategoriesCheckBoxTreeView.NodeDataBound += this.CategoriesCheckBoxTreeView_NodeDataBound;
        }

        /// <summary>
        /// Handles the <see cref="CheckBox.CheckedChanged"/> event of the <c>DetailsDisplayModuleRadioButton</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void DetailsDisplayModuleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var selectedRow = Engage.Utility.FindParentControl<TableRow>((RadioButton)sender);            
            this.SetDetailsDisplayModuleGridSelection(GetTabIdFromRow(selectedRow), GetModuleIdFromRow(selectedRow));
        }

        /// <summary>
        /// Gets the tab ID of the given <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The row from which to get the tab ID.</param>
        /// <returns>The tab ID of the given <paramref name="row"/>.</returns>
        private static int GetTabIdFromRow(TableRow row)
        {
            return int.Parse(row.Cells[2].Text, NumberStyles.Integer, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the module ID of the given <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The row from which to get the module ID.</param>
        /// <returns>The module ID of the given <paramref name="row"/>.</returns>
        private static int GetModuleIdFromRow(TableRow row)
        {
            return int.Parse(row.Cells[4].Text, NumberStyles.Integer, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Handles the NodeCheck event of the CategoriesCheckBoxTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        private void CategoriesCheckBoxTreeView_NodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                // all categories node
                this.SetEnabledAllNodes(e.Node.Nodes.Cast<RadTreeNode>(), !e.Node.Checked);
            }
        }

        /// <summary>
        /// Handles the NodeClick event of the CategoriesCheckBoxTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        private void CategoriesCheckBoxTreeView_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.Selected = false;
        }

        /// <summary>
        /// Disables / enables the all nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        private void SetEnabledAllNodes(IEnumerable<RadTreeNode> nodes, bool enabled)
        {
            foreach (var node in nodes)
            {
                node.Enabled = enabled;
                node.Checked = enabled;

                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    this.SetEnabledAllNodes(node.Nodes.Cast<RadTreeNode>(), enabled);
                }
            }
        }

        /// <summary>
        /// Handles the NodeDataBound event of the CategoriesCheckBoxTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        private void CategoriesCheckBoxTreeView_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            if (this.Categories != null & this.Categories.Contains(e.Node.Value))
            {
                e.Node.Checked = true;
            }
        }

        /// <summary>
        /// Handles the NodeCreated event of the CategoriesCheckBoxTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        private void CategoriesCheckBoxTreeView_NodeCreated(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.Expanded = true;
        }

        /////// <summary>
        /////// Handles the <see cref="CheckBox.CheckedChanged"/> event of the <see cref="AllCategoriesCheckBox"/> control.
        /////// </summary>
        /////// <param name="sender">The source of the event.</param>
        /////// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        ////private void AllCategoriesCheckBox_CheckedChanged(object sender, EventArgs e)
        ////{
        ////    this.CategoriesCheckBoxTreeView.Enabled = !this.AllCategoriesCheckBox.Checked;
        ////}

        /// <summary>
        /// Handles the <see cref="CustomValidator.ServerValidate"/> event of the <see cref="CategoriesListValidator"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="ServerValidateEventArgs"/> instance containing the event data.</param>
        private void CategoriesListValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            ////if (this.AllCategoriesCheckBox.Checked)
            ////{
            ////    args.IsValid = true;
            ////    return;
            ////}

            ////args.IsValid = this.CategoriesCheckBoxList.Items.Cast<ListItem>().Any(categoryItem => categoryItem.Selected);
            args.IsValid = this.CategoriesCheckBoxTreeView.CheckedNodes.Any();
        }

        /// <summary>
        /// Handles the <see cref="CustomValidator.ServerValidate"/> event of the <see cref="DetailsDisplayModuleValidator"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void DetailsDisplayModuleValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int checkedCount = 0;
            foreach (GridViewRow row in this.DetailsDisplayModuleGrid.Rows)
            {
                var detailsDisplayModuleRadioButton = (RadioButton)row.FindControl("DetailsDisplayModuleRadioButton");
                if (detailsDisplayModuleRadioButton.Checked)
                {
                    checkedCount++;
                }
            }

            args.IsValid = checkedCount == 1;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.DisplaySettingsControl();
        }

        /// <summary>
        /// Sets the options.
        /// </summary>
        private void SetOptions()
        {
            this.FeaturedCheckBox.Checked = Dnn.Events.ModuleSettings.FeaturedOnly.GetValueAsBooleanFor(this).Value;
            this.HideFullEventsCheckBox.Checked = Dnn.Events.ModuleSettings.HideFullEvents.GetValueAsBooleanFor(this).Value;
            this.SetDetailsDisplayModuleGridSelection(Dnn.Events.ModuleSettings.DetailsDisplayTabId.GetValueAsInt32For(this) ?? this.TabId, Dnn.Events.ModuleSettings.DetailsDisplayModuleId.GetValueAsInt32For(this) ?? this.ModuleId);

            var categoriesSettingValue = Dnn.Events.ModuleSettings.Categories.GetValueAsStringFor(this);
            if (string.IsNullOrEmpty(categoriesSettingValue))
            {
                var all = this.CategoriesCheckBoxTreeView.Nodes.Cast<RadTreeNode>().FirstOrDefault();
                if (all != null)
                {
                    all.Checked = true;
                    this.SetEnabledAllNodes(all.Nodes.Cast<RadTreeNode>(), false);
                }
                ////this.CategoriesCheckBoxTreeView.Enabled = false;
            }

            // Note: the set options for the selected categories is moved to NodeDataBound event on the treeview.
        }

        /// <summary>
        /// Selects the <see cref="RadioButton"/> for the row in <see cref="DetailsDisplayModuleGrid"/> with the given <paramref name="selectedTabId"/> and <paramref name="selectedModuleId"/>.
        /// </summary>
        /// <param name="selectedTabId">The Tab ID of the row to select.</param>
        /// /// <param name="selectedModuleId">The Module ID of the row to select.</param>
        private void SetDetailsDisplayModuleGridSelection(int selectedTabId, int selectedModuleId)
        {
            foreach (GridViewRow row in this.DetailsDisplayModuleGrid.Rows)
            {
                var detailsDisplayModuleRadioButton = (RadioButton)row.FindControl("DetailsDisplayModuleRadioButton");

                detailsDisplayModuleRadioButton.Checked = GetTabIdFromRow(row) == selectedTabId && GetModuleIdFromRow(row) == selectedModuleId;
            }
        }

        /// <summary>
        /// Gets the selected Tab ID for the DetailsDisplayTabId setting.
        /// </summary>
        /// <returns>The TabID that was selected for the Details Display TabID setting</returns>
        /// <exception cref="InvalidOperationException">This method cannot be called until validation has run, ensuring that a Tab ID has been selected</exception>
        private int GetSelectedDetailsDisplayTabId()
        {
            foreach (GridViewRow row in this.DetailsDisplayModuleGrid.Rows)
            {
                var detailsDisplayModuleRadioButton = (RadioButton)row.FindControl("DetailsDisplayModuleRadioButton");
                if (detailsDisplayModuleRadioButton.Checked)
                {
                    return GetTabIdFromRow(row);
                }
            }

            throw new InvalidOperationException("This method cannot be called until validation has run, ensuring that a Tab ID has been selected");
        }

        /// <summary>
        /// Gets the selected Module ID for the DetailsDisplayModuleId setting.
        /// </summary>
        /// <returns>The ModuleID that was selected for the Details Display ModuleID setting</returns>
        /// <exception cref="InvalidOperationException">This method cannot be called until validation has run, ensuring that a Module ID has been selected</exception>
        private int GetSelectedDetailsDisplayModuleId()
        {
            foreach (GridViewRow row in this.DetailsDisplayModuleGrid.Rows)
            {
                var detailsDisplayModuleRadioButton = (RadioButton)row.FindControl("DetailsDisplayModuleRadioButton");
                if (detailsDisplayModuleRadioButton.Checked)
                {
                    return GetModuleIdFromRow(row);
                }
            }

            throw new InvalidOperationException("This method cannot be called until validation has run, ensuring that a Module ID has been selected");
        }

        /// <summary>
        /// Displays the settings control.
        /// </summary>
        private void DisplaySettingsControl()
        {
            switch (Dnn.Events.ModuleSettings.DisplayType.GetValueAsStringFor(this).ToUpperInvariant())
            {
                case "LIST":
                    this.LoadSettingsControl("Display/TemplateDisplayOptions.ascx", "List Settings.Header");
                    break;
                case "CALENDAR":
                    this.LoadSettingsControl("Display/CalendarDisplayOptions.ascx", "Calendar Settings.Header");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Loads the settings control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        /// <param name="headerResourceKey">The resource key for the specific settings control's header.</param>
        private void LoadSettingsControl(string controlName, string headerResourceKey)
        {
            this.SpecificSettingsPlaceholder.EnableViewState = false;
            this.SpecificSettingsPlaceholder.Controls.Clear();

            this.specificSettingsControl = this.CreateSettingsControl(controlName);
            this.SpecificSettingsPlaceholder.Controls.Add(this.specificSettingsControl);

            this.SpecificSettingsHeaderLiteral.Text = this.Localize(headerResourceKey);
        }

        /// <summary>
        /// Creates the settings control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        /// <returns>Module Settings Base</returns>
        private ModuleSettingsBase CreateSettingsControl(string controlName)
        {
            var settingsControl = (ModuleSettingsBase)this.LoadControl(controlName);
            ModuleInfo mi = new ModuleController().GetModule(this.ModuleId, this.TabId);
            settingsControl.ModuleConfiguration = mi;
            settingsControl.ID = Path.GetFileNameWithoutExtension(controlName);
            settingsControl.LoadSettings();

            return settingsControl;
        }
    }
}