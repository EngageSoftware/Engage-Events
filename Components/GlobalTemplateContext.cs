// <copyright file="GlobalTemplateContext.cs" company="Engage Software">
// Engage: Events
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

using System.Linq;

namespace Engage.Dnn.Events.Components
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using DotNetNuke.UI.Modules;

    using Engage.Dnn.Framework;
    using Engage.Dnn.Framework.Templating;
    using Engage.Events;

    using Utility = Engage.Dnn.Events.Utility;

    public class GlobalTemplateContext : ITemplateable
    {
        public GlobalTemplateContext(ModuleInstanceContext moduleContext)
        {
            this.ModuleContext = moduleContext;
        }

        public ModuleInstanceContext ModuleContext { get; private set; }

        protected int PortalId
        {
            get { return this.ModuleContext.PortalId; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public string GetValue(string propertyName)
        {
            return this.GetValue(propertyName, null);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public string GetValue(string propertyName, string format)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            switch (propertyName.ToUpperInvariant())
            {
                case "MULTIPLE CATEGORIES":
                case "MULTIPLECATEGORIES":
                    IEnumerable<Category> categories = CategoryCollection.Load(this.PortalId);
                    var moduleCategoryIds = ModuleSettings.GetCategoriesFor(new FakeModuleControlBase(Utility.DesktopModuleName, this.ModuleContext.Configuration));
                    if (moduleCategoryIds.Any())
                    {
                        var categoryIdsWithAncestor = Utility.AddAncestorIds(moduleCategoryIds.ToArray(), categories.ToArray(), true);
                        categories = categories.Where(category => categoryIdsWithAncestor.Contains(category.Id));
                    }

                    return (categories.Count() > 1).ToString(CultureInfo.InvariantCulture);
            }

            return String.Empty;
        }
    }
}