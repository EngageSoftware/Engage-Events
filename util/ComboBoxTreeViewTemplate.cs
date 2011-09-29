// <copyright file="ComboBoxTreeViewTemplate.cs" company="Engage Software">
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
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Telerik.Web.UI;

    /// <summary>
    /// Template for RadComboBox with RadTreeView.
    /// </summary>
    public class ComboBoxTreeViewTemplate : ITemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBoxTreeViewTemplate"/> class.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        public ComboBoxTreeViewTemplate(RadTreeView treeView)
        {
            this.TreeView = treeView;
        }

        /// <summary>
        /// Gets or sets the tree view.
        /// </summary>
        /// <value>
        /// The tree view.
        /// </value>
        private RadTreeView TreeView { get; set; }

        /// <summary>
        /// Defines the <see cref="T:System.Web.UI.Control"/> object that child controls and templates belong to. These child controls are in turn defined within an inline template.
        /// </summary>
        /// <param name="container">The <see cref="T:System.Web.UI.Control"/> object to contain the instances of controls from the inline template.</param>
        public void InstantiateIn(Control container)
        {
            Label label1 = new Label();
            label1.ID = "ItemLabel";
            label1.Text = "Text";
            label1.Font.Size = 15;
            label1.Font.Bold = true;
            container.Controls.Add(label1);   
            ////container.Controls.Add(this.TreeView);
        }
    }
}