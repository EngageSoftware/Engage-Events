// <copyright file="Category.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Events
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Xml.Serialization;
    using Data;
    using Dnn.Framework.Templating;

    /// <summary>
    /// An event category
    /// </summary>
    [XmlRoot(ElementName = "category", IsNullable = false)]
    public class Category : ITemplateable
    {
        /// <summary>
        /// Backing field for <see cref="ParentId"/>
        /// </summary>
        private int? parentId;

        /// <summary>
        /// Prevents a default instance of the <see cref="Category"/> class from being created.
        /// </summary>
        private Category()
        {
            this.Id = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="portalId">The portal ID.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="name">The name of the category.</param>
        /// <param name="color">The color of the category.</param>
        private Category(int portalId, int? parentId, string name, string color) : this()
        {
            this.PortalId = portalId;
            this.ParentId = parentId;
            this.Name = name;
            this.Color = color;
        }

        /// <summary>
        /// Gets the ID of this category.
        /// </summary>
        /// <value>This <see cref="Category"/>'s ID.</value>
        [XmlElement(Order = 1)]
        public int Id { get; private set; }

        /// <summary>
        /// Gets the portal ID.
        /// </summary>
        /// <value>The portal ID.</value>
        [XmlElement(Order = 2)]
        public int PortalId { get; private set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        /// <value>The category name.</value>
        [XmlElement(Order = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category color.
        /// </summary>
        /// <value>The category color.</value>
        [XmlElement(Order = 4)]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        [XmlElement(Order = 5)]
        public int? ParentId
        {
            get
            {
                return this.parentId;
            }

            set
            {
                this.parentId = value > 0 ? value : null;
            }
        }

        /// <summary>
        /// Gets or sets the number of events in this category.
        /// </summary>
        /// <value>The number of events in this category.</value>
        [XmlIgnore]
        public int EventCount { get; set; }

        /// <summary>
        /// Loads the specified category.
        /// </summary>
        /// <param name="id">The id of a category to load.</param>
        /// <returns>The requested <see cref="Category"/></returns>
        /// <exception cref="DBException">If an error occurs while going to the database to get the category</exception>
        public static Category Load(int id)
        {
            IDataProvider dp = DataProvider.Instance;
            Category category = null;

            try
            {
                using (IDataReader reader = dp.ExecuteReader(CommandType.StoredProcedure, dp.NamePrefix + "spGetEventCategory", Utility.CreateIntegerParam("@CategoryId", id)))
                {
                    if (reader.Read())
                    {
                        category = Fill(reader);
                    }
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetEventCategory", se);
            }

            return category;
        }

        /// <summary>
        /// Creates the specified category.
        /// </summary>
        /// <param name="portalId">The portal ID.</param>
        /// <param name="parentId">The parent id.</param>
        /// <param name="name">The name of the category.</param>
        /// <param name="color">The color of the category.</param>
        /// <returns>
        /// A new <see cref="Category"/> instance.
        /// </returns>
        public static Category Create(int portalId, int? parentId, string name, string color)
        {
            return new Category(portalId, parentId, name, color);
        }

        /// <summary>
        /// Deletes the specified category.
        /// </summary>
        /// <param name="categoryId">The category ID.</param>
        /// <exception cref="DBException">If an error occurs while going to the database to delete the category</exception>
        public static void Delete(int categoryId)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spDeleteEventCategory", Utility.CreateIntegerParam("@CategoryId", categoryId));
            }
            catch (Exception se)
            {
                throw new DBException("spDeleteEventCategory", se);
            }
        }

        /// <summary>
        /// Saves this category.
        /// </summary>
        /// <param name="revisingUser">The user who is saving this category.</param>
        public void Save(int revisingUser)
        {
            if (this.Id < 0)
            {
                this.Insert(revisingUser);
            }
            else
            {
                this.Update(revisingUser);
            }
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
        /// <returns>The string representation of the value of this instance.</returns>
        public string GetValue(string propertyName)
        {
            return this.GetValue(propertyName, null);
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
        public string GetValue(string propertyName, string format)
        {
            format = string.IsNullOrEmpty(format) ? null : format;
            switch (propertyName.ToUpperInvariant())
            {
                case "ID":
                    return this.Id.ToString(format, CultureInfo.CurrentCulture);
                case "PARENT":
                case "PARENTID":
                    return this.ParentId.HasValue ? this.ParentId.Value.ToString(format, CultureInfo.CurrentCulture) : string.Empty;
                case "NAME":
                case "TITLE":
                    return TemplateEngine.FormatString(this.Name, format ?? "HTML");
                case "COLOR":
                    return TemplateEngine.FormatString(this.Color, format ?? "HTML");
            }

            return string.Empty;
        }

        /// <summary>
        /// Fills a <see cref="Category"/> with the data in the specified <paramref name="categoryRecord"/>.
        /// </summary>
        /// <param name="categoryRecord">A pre-initialized data record that represents a Category instance.</param>
        /// <returns>An instantiated Category object.</returns>
        internal static Category Fill(IDataRecord categoryRecord)
        {
            return new Category 
                {
                    Id = (int)categoryRecord["CategoryId"],
                    ParentId = categoryRecord["ParentId"] as int?,
                    Name = categoryRecord["Name"].ToString(),
                    Color = categoryRecord["Color"] as string,
                    EventCount = (int)categoryRecord["EventCount"]
                };
        }

        /// <summary>
        /// Inserts this category.
        /// </summary>
        /// <param name="revisingUser">The user who is inserting this category.</param>
        /// <exception cref="DBException">If an error occurs while going to the database to insert the category</exception>
        private void Insert(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                this.Id = dp.ExecuteScalar(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spInsertEventCategory",
                        Utility.CreateIntegerParam("@PortalId", this.PortalId),
                        Utility.CreateIntegerParam("@ParentId", this.ParentId),
                        Utility.CreateVarcharParam("@Name", this.Name, 250),
                        Utility.CreateVarcharParam("@Color", this.Color, 50),
                        Utility.CreateIntegerParam("@CreatedBy", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spInsertEventCategory", de);
            }
        }

        /// <summary>
        /// Updates this category.
        /// </summary>
        /// <param name="revisingUser">The user responsible for updating this category.</param>
        /// <exception cref="DBException">If an error occurs while going to the database to update the category</exception>
        private void Update(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spUpdateEventCategory",
                        Utility.CreateIntegerParam("@CategoryId", this.Id),
                        Utility.CreateIntegerParam("@ParentId", this.ParentId),
                        Utility.CreateVarcharParam("@Name", this.Name, 250),
                        Utility.CreateVarcharParam("@Color", this.Color, 50),
                        Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spUpdateEventCategory", de);
            }
        }
    }
}
