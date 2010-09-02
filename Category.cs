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
        /// Prevents a default instance of the <see cref="Category"/> class from being created.
        /// </summary>
        private Category()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="portalId">The portal ID.</param>
        /// <param name="name">The name of the category.</param>
        private Category(int portalId, string name)
        {
            this.PortalId = portalId;
            this.Name = name;
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
        /// <param name="name">The name of the category.</param>
        /// <returns>A new <see cref="Category"/> instance.</returns>
        public static Category Create(int portalId, string name)
        {
            return new Category(portalId, name);
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
        /// <param name="format">A numeric or DateTime format string, or <c>null</c> or <see cref="string.Empty"/> to apply the default format.</param>
        /// <returns>The string representation of the value of this instance as specified by <paramref name="format"/>.</returns>
        public string GetValue(string propertyName, string format)
        {
            switch (propertyName.ToUpperInvariant())
            {
                case "ID":
                    return this.Id.ToString(format, CultureInfo.CurrentCulture);
                case "NAME":
                case "TITLE":
                    return this.Name;
                case "SAFENAME":
                case "SAFE NAME":
                case "SAFETITLE":
                case "SAFE TITLE":
                    return Utility.InvalidCssCharactersRegex.Replace(this.Name, "-").TrimStart('-');
            }

            return string.Empty;
        }

        /// <summary>
        /// Fills an Event with the data in the specified <paramref name="eventRecord"/>.
        /// </summary>
        /// <param name="eventRecord">A pre-initialized data record that represents an Event instance.</param>
        /// <returns>An instantiated Event object.</returns>
        internal static Category Fill(IDataRecord eventRecord)
        {
            return new Category 
                {
                    Id = (int)eventRecord["CategoryId"],
                    Name = eventRecord["Name"].ToString()
                };
        }

        /// <summary>
        /// Inserts this category.
        /// </summary>
        /// <param name="revisingUser">The user who is inserting this category.</param>
        /// <exception cref="DBException">If an error occurs while going to the database to insert the event</exception>
        private void Insert(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                this.Id = dp.ExecuteScalar(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spInsertEventCategory",
                        Utility.CreateIntegerParam("@PortalId", this.PortalId),
                        Utility.CreateVarcharParam("@Name", this.Name),
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
        /// <exception cref="DBException">If an error occurs while going to the database to update the event</exception>
        private void Update(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spUpdateEventCategory",
                        Utility.CreateIntegerParam("@CategoryId", this.Id),
                        Utility.CreateVarcharParam("@Name", this.Name),
                        Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spUpdateEventCategory", de);
            }
        }
    }
}
