// <copyright file="ResponseSummaryCollection.cs" company="Engage Software">
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
    using System.Diagnostics;
    using Data;

    /// <summary>
    /// </summary
    /// <remarks>
    /// This class inherits from <see cref="BindingList{T}"/> for future support.
    /// </remarks>
    public class ResponseSummaryCollection : BindingList<ResponseSummary>
    {
        /// <summary>
        /// Backing field for <see cref="TotalRecords"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int totalRecords;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseSummaryCollection"/> class.
        /// </summary>
        private ResponseSummaryCollection(int totalRecords)
        {
            this.totalRecords = totalRecords;
        }

        /// <summary>
        /// Gets the total number of records for the query that returned this collection.
        /// </summary>
        /// <value>The total number of records for the query that returned this collection.</value>
        public int TotalRecords
        {
            [DebuggerStepThrough]
            get { return this.totalRecords; }
        }

        /// <summary>
        /// Loads the specified collection of <see cref="ResponseSummary"/> objects.
        /// </summary>
        /// <param name="portalId">The portal id.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="index">The page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>The specified collection of <see cref="ResponseSummary"/> objects.</returns>
        /// <exception cref="DBException">If an error occurs while retrieving the collection from the database.</exception>
        public static ResponseSummaryCollection Load(int portalId, string sortColumn, int index, int pageSize)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (IDataReader reader = dp.ExecuteReader(
                    CommandType.StoredProcedure,
                    dp.NamePrefix + "spGetResponseSummaries",
                    Utility.CreateIntegerParam("@portalId", portalId),
                    Utility.CreateVarcharParam("@sortColumn", sortColumn, 200),
                    Utility.CreateIntegerParam("@index", index),
                    Utility.CreateIntegerParam("@pageSize", pageSize)))
                {
                    return FillResponseSummary(reader);
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetResponseSummaries", se);
            }
        }

        /// <summary>
        /// Fills the Response summary collection.
        /// </summary>
        /// <param name="dataReader">The dataReader which has the list of Response Summaries.</param>
        /// <returns>A collection of <see cref="ResponseSummary"/> objects.</returns>
        /// <exception cref="DBException">Data reader did not have the expected structure.  An error must have occurred in the query.</exception>
        private static ResponseSummaryCollection FillResponseSummary(IDataReader dataReader)
        {
            if (dataReader.Read())
            {
                ResponseSummaryCollection responses = new ResponseSummaryCollection((int)dataReader["TotalRecords"]);

                if (dataReader.NextResult())
                {
                    while (dataReader.Read())
                    {
                        responses.Add(ResponseSummary.Fill(dataReader));
                    }
                    return responses;
                }
            }
            throw new DBException("Data reader did not have the expected structure.  An error must have occurred in the query.");
        }
    }
}
