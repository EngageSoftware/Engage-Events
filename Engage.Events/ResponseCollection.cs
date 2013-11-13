// <copyright file="ResponseCollection.cs" company="Engage Software">
// Engage: Events
// Copyright (c) 2004-2011
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    using Data;

    /// <summary>
    /// The collection of <see cref="Response"/> objects for a given event.
    /// </summary>
    /// <remarks>
    /// This class inherits from <see cref="BindingList{T}"/> for future support.
    /// </remarks>
    public class ResponseCollection : BindingList<Response>
    {
        /// <summary>
        /// Backing field for <see cref="TotalRecords"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int totalRecords;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseCollection"/> class.
        /// </summary>
        /// <param name="totalRecords">The total number of records returned by the query that filled this collection.</param>
        private ResponseCollection(int totalRecords)
        {
            this.totalRecords = totalRecords;
        }

        /// <summary>
        /// Gets the total records.
        /// </summary>
        /// <value>The total records.</value>
        public int TotalRecords
        {
            [DebuggerStepThrough]
            get { return this.totalRecords; }
        }

        /// <summary>
        /// Loads the collection of <see cref="Response"/> objects for the specified event.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date for which this response was made.</param>
        /// <param name="status">The status, or <c>null</c> for all statuses.</param>
        /// <param name="sortColumn">
        /// The sort column (any column on Response, e.g. <c>CreationDate</c>, <c>FirstName</c>, <c>LastName</c>, or <c>ResponseId</c>)
        /// or <c>null</c> for a default column (<c>CreationDate</c>).
        /// </param>
        /// <param name="index">The page index (0-based).</param>
        /// <param name="pageSize">Size of the page (or <c>0</c> for all records).</param>
        /// <param name="categoryIds">
        /// A sequence of IDs for the category/ies that events must be in in order to retrieve their responses, 
        /// or an empty/<c>null</c> sequence to get responses regardless of the event's category.
        /// </param>
        /// <returns>The collection of <see cref="Response"/> objects for the specified event.</returns>
        /// <exception cref="DBException">If an error occurs while loading the collection from the database</exception>
        public static ResponseCollection Load(int eventId, DateTime eventStart, string status, string sortColumn, int index, int pageSize, IEnumerable<int> categoryIds)
        {
            var dp = DataProvider.Instance;
            var categoryIdsValue = categoryIds != null && categoryIds.Any()
                                       ? string.Join(",", categoryIds.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray())
                                       : null;
            try
            {
                using (
                    IDataReader dataReader = dp.ExecuteReader(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spGetResponses",
                        Utility.CreateIntegerParam("@EventId", eventId),
                        Utility.CreateDateTimeParam("@EventStart", eventStart),
                        Utility.CreateVarcharParam("@Status", status),
                        Utility.CreateVarcharParam("@sortColumn", sortColumn ?? "CreationDate", 200),
                        Utility.CreateIntegerParam("@index", index),
                        Utility.CreateIntegerParam("@pageSize", pageSize),
                        Utility.CreateVarcharParam("@categoryIds", categoryIdsValue)))
                {
                    return FillResponses(dataReader);
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetResponses", se);
            }
        }

        /// <summary>
        /// Loads a response report for all events that occur after <see cref="DateTime.Now"/>.
        /// Columns returned include:
        /// <list type="number">
        ///     <listheader>
        ///             <term>Name</term>
        ///             <description>Type</description>
        ///         </listheader>
        ///         <item>
        ///             <term>CategoryName</term>
        ///             <description>nvarchar(500)</description>
        ///         </item>
        ///         <item>
        ///             <term>EventTitle</term>
        ///             <description>nvarchar(500)</description>
        ///         </item>
        ///         <item>
        ///             <term>EventStart</term>
        ///             <description>DateTime</description>
        ///         </item>
        ///         <item>
        ///             <term>EventEnd</term>
        ///             <description>DateTime</description>
        ///         </item>
        ///         <item>
        ///             <term>FirstName</term>
        ///             <description>nvarchar(100)</description>
        ///         </item>
        ///         <item>
        ///             <term>LastName</term>
        ///             <description>nvarchar(100)</description>
        ///         </item>
        ///         <item>
        ///             <term>Email</term>
        ///             <description>nvarchar(100)</description>
        ///         </item>
        ///         <item>
        ///             <term>ResponseDate</term>
        ///             <description>DateTime</description>
        ///         </item>
        ///         <item>
        ///             <term>Status</term>
        ///             <description>nvarchar(40)</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="startDate">The date on which to start including events (or <c>null</c> to start from the beginning)</param>
        /// <param name="endDate">The data on which to stop including events (or <c>null</c> to not stop)</param>
        /// <param name="categoryIds">
        ///   A sequence of IDs for the category/ies that events must be in in order to retrieve their responses, 
        ///   or an empty/<c>null</c> sequence to get responses regardless of the event's category.
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> instance with the nine columns mentioned in the summary section, for each response to an event that hasn't already occurred.
        /// </returns>
        public static IDataReader LoadReport(DateTime? startDate, DateTime? endDate, IEnumerable<int> categoryIds)
        {
            var dataProvider = DataProvider.Instance;
            var categoryIdsValue = categoryIds != null && categoryIds.Any()
                                       ? string.Join(",", categoryIds.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray())
                                       : null;
            try
            {
                return dataProvider.ExecuteReader(
                    CommandType.StoredProcedure,
                    dataProvider.NamePrefix + "spGetResponseReport",
                    Utility.CreateDateTimeParam("@startDate", startDate),
                    Utility.CreateDateTimeParam("@endDate", endDate),
                    Utility.CreateVarcharParam("@categoryIds", categoryIdsValue));
            }
            catch (Exception se)
            {
                throw new DBException("spGetResponseReport", se);
            }
        }

        /// <summary>
        /// Fills the Responses.
        /// </summary>
        /// <param name="dataReader">A data reader representing the record of a Response.</param>
        /// <returns>The collection of <see cref="Response"/> objects for the given event.</returns>
        /// <exception cref="DBException">Data reader did not have the expected structure.  An error must have occurred in the query.</exception>
        private static ResponseCollection FillResponses(IDataReader dataReader)
        {
            if (dataReader.Read())
            {
                ResponseCollection responses = new ResponseCollection((int)dataReader["TotalRecords"]);

                if (dataReader.NextResult())
                {
                    while (dataReader.Read())
                    {
                        responses.Add(Response.Fill(dataReader));
                    }
                }

                return responses;
            }

            throw new DBException("Data reader did not have the expected structure.  An error must have occurred in the query.");
        }
    }
}