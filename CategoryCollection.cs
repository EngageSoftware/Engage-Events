// <copyright file="CategoryCollection.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
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
    using System.ComponentModel;
    using System.Data;

    using Data;

    /// <summary>
    /// The collection of <see cref="Category"/> objects.
    /// </summary>
    /// <remarks>
    /// This class inherits from <see cref="BindingList{T}"/> for future support.
    /// </remarks>
    public class CategoryCollection : BindingList<Category>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryCollection"/> class.
        /// </summary>
        /// <param name="totalRecords">The total number of records returned by the query that filled this collection.</param>
        private CategoryCollection(int totalRecords)
        {
            this.TotalRecords = totalRecords;
        }

        /// <summary>
        /// Gets the total number of categories represented by the query that returned this list 
        /// (i.e. the number of categories that would come back if there was no paging)
        /// </summary>
        /// <value>The total number of records.</value>
        public int TotalRecords { get; private set; }

        /// <summary>
        /// Loads the collection of <see cref="Category"/> objects for the specified portal, ordered by name.
        /// </summary>
        /// <param name="portalId">The portal ID.</param>
        /// <returns>
        /// The collection of <see cref="Category"/> objects for the specified portal.
        /// </returns>
        /// <exception cref="DBException">If an error occurs while loading the collection from the database</exception>
        public static CategoryCollection Load(int portalId)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (
                    IDataReader dataReader = dp.ExecuteReader(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spGetEventCategories",
                        Utility.CreateIntegerParam("@PortalId", portalId)))
                {
                    return FillCategories(dataReader);
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetEventCategories", se);
            }
        }

        /// <summary>
        /// Fills the Categories.
        /// </summary>
        /// <param name="dataReader">A data reader representing the record of a Category.</param>
        /// <returns>The collection of <see cref="Category"/> objects for the given event.</returns>
        /// <exception cref="DBException">Data reader did not have the expected structure.  An error must have occurred in the query.</exception>
        private static CategoryCollection FillCategories(IDataReader dataReader)
        {
            if (dataReader.Read())
            {
                var categories = new CategoryCollection((int)dataReader["TotalRecords"]);

                if (dataReader.NextResult())
                {
                    while (dataReader.Read())
                    {
                        categories.Add(Category.Fill(dataReader));
                    }
                }

                return categories;
            }

            throw new DBException("Data reader did not have the expected structure.  An error must have occurred in the query.");
        }
    }
}