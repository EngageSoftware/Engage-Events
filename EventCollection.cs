// <copyright file="EventCollection.cs" company="Engage Software">
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
    /// A strongly-typed collection of <see cref="Event"/> objects.
    /// </summary>
    /// <remarks>
    /// This class inherits from BindingList for future support.
    /// </remarks>
    public class EventCollection : BindingList<Event>
    {
        /// <summary>
        /// Backing field for <see cref="TotalRecords"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int totalRecords;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCollection"/> class.
        /// </summary>
        private EventCollection()
        {
        }

        /// <summary>
        /// Gets the total number of events in this collection.
        /// </summary>
        /// <value>The total number of events in this collection.</value>
        public int TotalRecords
        {
            [DebuggerStepThrough]
            get { return this.totalRecords; }
        }

        /// <summary>
        /// Loads the specified page of events.
        /// </summary>
        /// <param name="portalId">The ID of the portal that the events are for.</param>
        /// <param name="sortColumn">The column by which to sort the event list.</param>
        /// <param name="index">The index of the page to retrieve.</param>
        /// <param name="pageSize">Size of the page, or <c>0</c> if all records should be returned.</param>
        /// <param name="showAll">if set to <c>true</c>, gets cancelled and end-dated events, too.</param>
        /// <returns>A page of events in the given <paramref name="portalId"/>.</returns>
        public static EventCollection Load(int portalId, string sortColumn, int index, int pageSize, bool showAll)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (DataSet ds = dp.ExecuteDataset(
                    CommandType.StoredProcedure, 
                    dp.NamePrefix + "spGetEvents", 
                    Utility.CreateIntegerParam("@portalId", portalId),
                    Utility.CreateVarcharParam("@sortColumn", sortColumn, 200), 
                    Utility.CreateIntegerParam("@index", index),
                    Utility.CreateIntegerParam("@pageSize", pageSize), 
                    Utility.CreateBitParam("@showAll", showAll)))
                {
                    return FillEvents(ds.Tables[0]);
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetEvents", se);
            }
        }

        /// <summary>
        /// Loads a page of events either for the current month, or all future months.
        /// </summary>
        /// <param name="portalId">The ID of the portal that the events are for.</param>
        /// <param name="currentMonth">if set to <c>true</c> gets events that start in this month, otherwise gets events that start in any month after this month.</param>
        /// <param name="index">The index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>A page of events for either this month, or all future months.</returns>
        public static EventCollection Load(int portalId, bool currentMonth, int index, int pageSize)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (DataSet ds = dp.ExecuteDataset(
                    CommandType.StoredProcedure, 
                    dp.NamePrefix + "spGetEventsSpecific",
                    Utility.CreateIntegerParam("@portalId", portalId), 
                    Utility.CreateBitParam("@currentMonth", currentMonth),
                    Utility.CreateIntegerParam("@index", index), 
                    Utility.CreateIntegerParam("@pageSize", pageSize)))
                {
                    return FillEvents(ds.Tables[0]);
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetEvents", se);
            }
        }

        /// <summary>
        /// Fills a collection of events from a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="eventTable">The event table.</param>
        /// <returns>A collection of instantiated <see cref="Event"/> object, as represented in <paramref name="eventTable"/></returns>
        private static EventCollection FillEvents(DataTable eventTable)
        {
            EventCollection events = new EventCollection();
            foreach (DataRow row in eventTable.Rows)
            {
                if (events.totalRecords == 0)
                {
                    events.totalRecords = (int)row["TotalRecords"];
                }

                events.Add(Event.Fill(row));
            }

            return events;
        }
    }
}