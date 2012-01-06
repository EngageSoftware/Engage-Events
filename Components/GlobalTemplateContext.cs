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

namespace Engage.Dnn.Events.Components
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DotNetNuke.UI.Modules;

    using Engage.Dnn.Framework;
    using Engage.Dnn.Framework.Templating;
    using Engage.Events;

    using Utility = Engage.Dnn.Events.Utility;

    /// <summary>
    /// Provides information that is always accessible to a template (not related to the particular item being displayed by the template)
    /// </summary>
    public class GlobalTemplateContext : GlobalTemplateContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalTemplateContext"/> class.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        public GlobalTemplateContext(ModuleInstanceContext moduleContext)
        {
            this.ModuleContext = moduleContext;
        }

        /// <summary>
        /// Gets the module context.
        /// </summary>
        public ModuleInstanceContext ModuleContext { get; private set; }

        /// <summary>
        /// Gets the portal ID.
        /// </summary>
        private int PortalId
        {
            get { return this.ModuleContext.PortalId; }
        }

        /// <summary>
        /// Gets the value of the property with the given <paramref name="propertyName"/>, or <see cref="string.Empty"/> if a property with that name does not exist on this object or is <c>null</c>.
        /// </summary>
        /// <remarks>
        /// To avoid conflicts with template syntax, avoid using the following symbols in the property name
        /// <list type="bullet">
        ///     <item><description>:</description></item>
        ///     <item><description>%</description></item>
        ///     <item><description>$</description></item>
        ///     <item><description>#</description></item>
        ///     <item><description>&gt;</description></item>
        ///     <item><description>&lt;</description></item>
        ///     <item><description>"</description></item>
        ///     <item><description>'</description></item>
        /// </list>
        /// </remarks>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="format">
        /// A numeric or DateTime format string, or one of the string formatting options accepted by <see cref="TemplateEngine.FormatString"/>,
        /// or <c>null</c> or <see cref="string.Empty"/> to apply the default format.
        /// </param>
        /// <returns>The string representation of the value of this instance as specified by <paramref name="format"/>.</returns>
        public override string GetValue(string propertyName, string format)
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

            return base.GetValue(propertyName, format);
        }
    }
}