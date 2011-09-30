// <copyright file="MultipleCategoriesFilterAction.ascx.cs" company="Engage Software">
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

    using Engage.Events;

    using Telerik.Web.UI;

    /// <summary>
    /// Allows the user to choose whether to display all events or only active events.
    /// </summary>
    public partial class MultipleCategoriesFilterAction : ActionControlBase
    {
        /// <summary>
        /// Backing field for <see cref="SessionCategoryIds"/>
        /// </summary>
        private int[] sessionCategoryIds;

        /// <summary>
        /// Occurs when the sort has changed.
        /// </summary>
        public event EventHandler CategoryChanged;

        /// <summary>
        /// Gets the ID of the category of event to display.
        /// </summary>
        /// <value>The category ID, or <c>null</c> for all categories.</value>
        internal int[] SelectedCategoryIds
        {
            get
            {
                if (this.CategoriesTreeView.Nodes.Count == 0 ||
                    this.CategoriesTreeView.Nodes.Cast<RadTreeNode>().First() == this.CategoriesTreeView.CheckedNodes.FirstOrDefault())
                {
                    return null;
                }

                var selectedCategoryIds = new List<int>();
                foreach (var node in this.CategoriesTreeView.CheckedNodes)
                {
                    int id;
                    if (int.TryParse(node.Value, out id))
                    {
                        selectedCategoryIds.Add(id);
                    }
                }

                return selectedCategoryIds.ToArray();
            }
        }

        /// <summary>
        /// Gets the session category ids.
        /// </summary>
        private int[] SessionCategoryIds
        {
            get
            {
                if (this.sessionCategoryIds == null && this.Session["categoryIds"] != null)
                {
                    this.sessionCategoryIds = (int[])this.Session["categoryIds"];
                }

                return this.sessionCategoryIds;
            }
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
            this.CategoriesTreeView.DataTextField = "Name";
            this.CategoriesTreeView.DataValueField = "Id";
            this.CategoriesTreeView.DataFieldID = "Id";
            this.CategoriesTreeView.DataFieldParentID = "ParentId";
            
            IEnumerable<Category> categories = CategoryCollection.Load(this.PortalId);
            if (this.CategoryIds.Any())
            {
                var categoryIdsWithAncestor = Utility.AddAncestorIds(this.CategoryIds.ToArray(), categories.ToArray(), true).ToArray();
                categories = categories.Where(category => categoryIdsWithAncestor.Contains(category.Id));
            }

            var categoryNodeItems =
                categories.Select(
                    category =>
                    new
                        {
                            Name =
                        string.IsNullOrEmpty(category.Name)
                            ? this.Localize("DefaultCategory.Text", this.LocalSharedResourceFile)
                            : category.Name,
                            Id = category.Id.ToString(CultureInfo.InvariantCulture),
                            ParentId =
                        category.ParentId.HasValue
                            ? category.ParentId.Value.ToString(CultureInfo.InvariantCulture)
                            : string.Empty
                        }).ToList();

            if (categoryNodeItems.Count > 1)
            {
                categoryNodeItems.Add(new { Name = this.Localize("AllListItem.Text"), Id = string.Empty, ParentId = (string)null });
            }
            else
            {
                this.CategoriesTreeView.Enabled = false;
            }

            this.CategoriesTreeView.DataSource = categoryNodeItems;
            this.CategoriesTreeView.DataBind();

            ////var categories = (from category in CategoryCollection.Load(this.PortalId)
            ////                  where !this.CategoryIds.Any() || this.CategoryIds.Contains(category.Id)
            ////                  select
            ////                      new
            ////                          {
            ////                              Name =
            ////                      string.IsNullOrEmpty(category.Name)
            ////                          ? this.Localize("DefaultCategory.Text", this.LocalSharedResourceFile)
            ////                          : category.Name,
            ////                              Id = category.Id.ToString(CultureInfo.InvariantCulture),
            ////                              ParentId =
            ////                      category.ParentId.HasValue
            ////                          ? category.ParentId.Value.ToString(CultureInfo.InvariantCulture)
            ////                          : string.Empty
            ////                          }).ToList();
            ////if (categories.Count > 1)
            ////{
            ////    categories.Add(new { Name = this.Localize("AllListItem.Text"), Id = string.Empty, ParentId = (string)null });
            ////}
            ////else
            ////{
            ////    this.CategoriesTreeView.Enabled = false;
            ////}

            ////this.CategoriesTreeView.DataSource = categories;
            ////this.CategoriesTreeView.DataBind();

            ////if (this.CategoriesTreeView.Nodes.Count > 1)
            ////{
            ////    this.CategoriesTreeView.Nodes.Insert(0, new RadTreeNode(this.Localize("AllListItem.Text"), "0"));
            ////}
            ////else
            ////{
            ////    this.CategoriesTreeView.Enabled = false;
            ////}
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;

            this.CategoriesTreeView.NodeCreated += this.CategoriesTreeView_NodeCreated;
            this.CategoriesTreeView.NodeDataBound += this.CategoriesTreeView_NodeDataBound;

            if (!this.IsPostBack)
            {
                this.BindData();
                this.SetInitialValue();
            }
        }

        /// <summary>
        /// Handles the Click event of the MultipleCategoriesFilterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ApplyButton_Click(object sender, EventArgs e)
        {
            this.InvokeCategoryChanged(new EventArgs());
        }

        /// <summary>
        /// Handles the NodeDataBound event of the CategoriesTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        private void CategoriesTreeView_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            
            int id;
            if (int.TryParse(e.Node.Value, out id))
            {
                var isEnabled = this.CategoryIds.Contains(id);
                e.Node.Attributes.Add("enabled", isEnabled ? "1" : "0");
                e.Node.Enabled = isEnabled;
                e.Node.Checked = this.SessionCategoryIds == null || this.SessionCategoryIds.Contains(id);
            }
            else if (this.SessionCategoryIds == null)
            {
                e.Node.Checked = true;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.LocalizeControls();
            this.SetupjQueryUIDialog();
        }

        /// <summary>
        /// Checks the first node selected.
        /// </summary>
        /// <param name="node">The node.</param>
        private void CheckFirstNodeSelected(RadTreeNode node)
        {
            var nodes = this.CategoriesTreeView.Nodes.Cast<RadTreeNode>().ToList();
            if (nodes.Count > 0 && node == nodes.First())
            {
                // the first "all list item" node is checked
                this.SetEnabledAllNodes(node.Nodes.Cast<RadTreeNode>(), !node.Checked);
            }
        }

        /// <summary>
        /// Localizeds the controls.
        /// </summary>
        private void LocalizeControls()
        {
            this.FilterButton.Text = this.Localize("FilterButton");
            this.ApplyButton.Text = this.Localize("ApplyButton");
        }

        /// <summary>
        /// Handles the NodeCreated event of the CategoriesTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Telerik.Web.UI.RadTreeNodeEventArgs"/> instance containing the event data.</param>
        private void CategoriesTreeView_NodeCreated(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.Expanded = true;
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
                int id;
                if (int.TryParse(node.Value, out id) && !this.CategoryIds.Contains(id))
                {
                    node.Enabled = false;
                    node.Checked = false;
                }

                node.Enabled = enabled;
                node.Checked = enabled;

                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    this.SetEnabledAllNodes(node.Nodes.Cast<RadTreeNode>(), enabled);
                }
            }
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
            if (this.CategoriesTreeView.Nodes.Count == 0)
            {
                return;
            }

            if (this.SessionCategoryIds == null)
            {
                // select all
                var allCategoriesNode  = this.CategoriesTreeView.Nodes.Cast<RadTreeNode>().First();
                allCategoriesNode.Checked = true;
                this.CheckFirstNodeSelected(allCategoriesNode);
            }
        }

        /// <summary>
        /// Setup the jQueryUI dialog to enable overlay for filter tree.
        /// </summary>
        private void SetupjQueryUIDialog()
        {
            this.AddJQueryReference();
            ScriptManager.RegisterClientScriptResource(this, typeof(RegisterAction), "Engage.Dnn.Events.JavaScript.jquery-ui-1.8.16.dialog.min.js");
        }
    }
}