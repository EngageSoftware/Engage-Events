// <copyright file="EventCollection.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Data;
    using Dnn.Framework.Templating;

    /// <summary>
    /// A strongly-typed collection of <see cref="Event"/> objects.
    /// </summary>
    /// <remarks>
    /// This class inherits from <see cref="BindingList{T}"/> for future support.
    /// </remarks>
    public class EventCollection : BindingList<Event>, IEnumerable<ITemplateable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCollection"/> class with the specified list.
        /// </summary>
        /// <param name="list">An <see cref="T:System.Collections.Generic.IList`1" /> of items to be contained in the <see cref="EventCollection"/>.</param>
        /// <param name="totalRecords">The total number of events in this collection.</param>
        private EventCollection(IList<Event> list, int totalRecords)
            : base(list)
        {
            this.TotalRecords = totalRecords;
        }

        /// <summary>
        /// Gets the total number of events in this collection.
        /// </summary>
        /// <value>The total number of events in this collection.</value>
        public int TotalRecords { get; private set; }

        /// <summary>
        /// Loads a page of events based on the given <paramref name="startDateUtc"/> and <paramref name="endDateUtc"/>.
        /// </summary>
        /// <param name="portalId">The ID of the portal that the events are for.</param>
        /// <param name="startDateUtc">The starting date for events to retrieve (in UTC), or <c>null</c> to have no starting bound.</param>
        /// <param name="endDateUtc">The ending date for events to retrieve (in UTC), or <c>null</c> to have no ending bound.</param>
        /// <param name="showAll">if set to <c>true</c> included canceled events.</param>
        /// <param name="featuredOnly">if set to <c>true</c> only include events that are featured.</param>
        /// <param name="hideFullEvents">if set to <c>true</c> only include events that have not hit their registration cap (or have no registration cap)</param>
        /// <param name="email">The email address of the user requesting the events</param>
        /// <param name="categoryIds">A sequence of IDs for the category/ies that events must be in in order to be retrieved, or an empty/<c>null</c> sequence to get events regardless of category.</param>
        /// <returns>A page of events</returns>
        /// <exception cref="DBException">if there's an error while going to the database to retrieve the events</exception>
        public static EventCollection Load(int portalId, DateTime? startDateUtc, DateTime? endDateUtc, bool showAll, bool featuredOnly, bool hideFullEvents, string email, IEnumerable<int> categoryIds)
        {
            return Load(portalId, startDateUtc, endDateUtc, null, null, null, showAll, featuredOnly, hideFullEvents, email, categoryIds, false);
        }

        /// <summary>
        /// Loads a page of events based on the given <paramref name="startDateUtc"/> and <paramref name="endDateUtc"/>.
        /// </summary>
        /// <param name="portalId">The ID of the portal that the events are for.</param>
        /// <param name="startDateUtc">The starting date for events to retrieve (in UTC), or <c>null</c> to have no starting bound.</param>
        /// <param name="endDateUtc">The ending date for events to retrieve (in UTC), or <c>null</c> to have no ending bound.</param>
        /// <param name="sortExpression">The property by which the events should be sorted.</param>
        /// <param name="pageIndex">The index of the page of events.</param>
        /// <param name="pageSize">Size of the page of events.</param>
        /// <param name="showAll">if set to <c>true</c> included canceled events.</param>
        /// <param name="featuredOnly">if set to <c>true</c> only include events that are featured.</param>
        /// <param name="hideFullEvents">if set to <c>true</c> only include events that have not hit their registration cap (or have no registration cap)</param>
        /// <param name="email">The email address of the user requesting the events</param>
        /// <param name="categoryIds">A sequence of IDs for the category/ies that events must be in in order to be retrieved, or an empty/<c>null</c> sequence to get events regardless of category.</param>
        /// <returns>A page of events</returns>
        /// <exception cref="DBException">if there's an error while going to the database to retrieve the events</exception>
        public static EventCollection Load(int portalId, DateTime? startDateUtc, DateTime? endDateUtc, string sortExpression, int pageIndex, int pageSize, bool showAll, bool featuredOnly, bool hideFullEvents, string email, IEnumerable<int> categoryIds)
        {
            return Load(portalId, startDateUtc, endDateUtc, sortExpression, pageIndex, pageSize, showAll, featuredOnly, hideFullEvents, email, categoryIds, true);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<ITemplateable> IEnumerable<ITemplateable>.GetEnumerator()
        {
            foreach (var @event in this)
            {
                yield return @event;
            }
        }

        /// <summary>
        /// Loads a page of events based on the given <paramref name="startDateUtc"/> and <paramref name="endDateUtc"/>.
        /// </summary>
        /// <param name="portalId">The ID of the portal that the events are for.</param>
        /// <param name="startDateUtc">The starting date for events to retrieve (in UTC), or <c>null</c> to have no starting bound.</param>
        /// <param name="endDateUtc">The ending date for events to retrieve (in UTC), or <c>null</c> to have no ending bound.</param>
        /// <param name="sortExpression">The property by which the events should be sorted.</param>
        /// <param name="pageIndex">The index of the page of events.</param>
        /// <param name="pageSize">Size of the page of events.</param>
        /// <param name="showAll">if set to <c>true</c> included canceled events.</param>
        /// <param name="featuredOnly">if set to <c>true</c> only include events that are featured.</param>
        /// <param name="hideFullEvents">if set to <c>true</c> only include events that have not hit their registration cap (or have no registration cap)</param>
        /// <param name="email">The email address of the user requesting the events</param>
        /// <param name="categoryIds">A sequence of IDs for the category/ies that events must be in in order to be retrieved, or an empty/<c>null</c> sequence to get events regardless of category.</param>
        /// <param name="processCollection">if set to <c>true</c> the collection should be sorted and paged, and each recurring event should be replaced by its earliest occurrence.</param>
        /// <returns>A page of events</returns>
        /// <exception cref="DBException">if there's an error while going to the database to retrieve the events</exception>
        private static EventCollection Load(int portalId, DateTime? startDateUtc, DateTime? endDateUtc, string sortExpression, int? pageIndex, int? pageSize, bool showAll, bool featuredOnly, bool hideFullEvents, string email, IEnumerable<int> categoryIds, bool processCollection)
        {
            var dataProvider = DataProvider.Instance;
            try
            {
                var categoryIdsValue = categoryIds != null && categoryIds.Any()
                                           ? string.Join(",", categoryIds.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray())
                                           : null;

                // Since events are stored in local time, and the start/end dates are in UTC
                // we need to extend the range to make sure we don't miss events because of time zone
                using (var reader = dataProvider.ExecuteReader(
                            CommandType.StoredProcedure,
                            dataProvider.NamePrefix + "spGetEvents",
                            Utility.CreateIntegerParam("@portalId", portalId),
                            Utility.CreateBitParam("@showAll", showAll),
                            Utility.CreateBitParam("@featured", featuredOnly),
                            Utility.CreateBitParam("@hideFullEvents", hideFullEvents),
                            Utility.CreateVarcharParam("@email", email),
                            Utility.CreateDateTimeParam("@startDate", AddTimeSpan(startDateUtc, TimeSpan.FromDays(-1))),
                            Utility.CreateDateTimeParam("@endDate", AddTimeSpan(endDateUtc, TimeSpan.FromDays(1))),
                            Utility.CreateVarcharParam("@categoryIds", categoryIdsValue)))
                {
                    return FillEvents(reader, processCollection, pageIndex, pageSize, sortExpression, startDateUtc, endDateUtc);
                }
            }
            catch (Exception exc)
            {
                throw new DBException("spGetEvents", exc);
            }
        }

        /// <summary>
        /// Fills a collection of events from a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="reader">An un-initialized data reader with two records.
        /// The first should be a collection of records representing the events requested.
        /// The second should be a single integer, representing the total number of events (non-paged) for the requested query.</param>
        /// <param name="processCollection">if set to <c>true</c> the collection should be sorted and paged, and each recurring event should be replaced by its earliest occurrence.</param>
        /// <param name="pageIndex">Index of the page of events being retrieved.</param>
        /// <param name="pageSize">Size of the page (number of events) being retrieved.</param>
        /// <param name="sortExpression">The property by which we should sort.</param>
        /// <param name="startDateUtc">The beginning date of the range of dates being retrieved (in UTC).</param>
        /// <param name="endDateUtc">The ending date of the range of dates being retrieved (in UTC).</param>
        /// <returns>
        /// A collection of instantiated <see cref="Event"/> object, as represented in <paramref name="reader"/>.
        /// </returns>
        /// <exception cref="DBException">Data reader did not have the expected structure.  An error must have occurred in the query.</exception>
        private static EventCollection FillEvents(IDataReader reader, bool processCollection, int? pageIndex, int? pageSize, string sortExpression, DateTime? startDateUtc, DateTime? endDateUtc)
        {
            int? beginIndex = processCollection && pageIndex.HasValue && pageSize.HasValue ? pageIndex * pageSize : null;
            int? endIndex = beginIndex.HasValue ? (pageIndex + 1) * pageSize : null;
            var events = new List<Event>(pageSize ?? 0);

            while (reader.Read())
            {
                var masterEvent = Event.Fill(reader);
                if ((masterEvent.EventStartUtc <= endDateUtc || endDateUtc == null)
                    && (masterEvent.EventEndUtc >= startDateUtc || startDateUtc == null))
                {
                    events.Add(masterEvent);
                }
                else
                {
                    if (!processCollection || !masterEvent.IsRecurring)
                    {
                        continue;
                    }

                    var eventOccurrence = GetEventOccurrence(masterEvent, startDateUtc, endDateUtc);
                    if (eventOccurrence != null)
                    {
                        events.Add(eventOccurrence);
                    }
                }
            }

            // After all events have been added (and recurring events outside of the date range have been removed),
            // we need to get the total count before we remove events from the list for paging
            var totalRecords = events.Count;

            // We don't need to sort or page if we are never ending, they should be sorted from the database in that case
            if (processCollection && endIndex.HasValue)
            {
                ProcessCollection(beginIndex.Value, endIndex.Value, sortExpression, events);
            }

            return new EventCollection(events, totalRecords);
        }

        /// <summary>
        /// Gets an occurrence of <paramref name="masterEvent"/> that fits within the given time span.
        /// </summary>
        /// <param name="masterEvent">The master event.</param>
        /// <param name="startDateUtc">The range's start date in UTC.</param>
        /// <param name="endDateUtc">The range's end date in UTC.</param>
        /// <returns>An <see cref="Event"/> instance, or <c>null</c></returns>
        private static Event GetEventOccurrence(Event masterEvent, DateTime? startDateUtc, DateTime? endDateUtc)
        {
            var startDate = UtcToEventTime(masterEvent.TimeZone, startDateUtc, DateTime.MinValue);
            var endDate = UtcToEventTime(masterEvent.TimeZone, endDateUtc, DateTime.MaxValue);
            masterEvent.RecurrenceRule.SetEffectiveRange(startDate, endDate);

            return (from eventStart in masterEvent.RecurrenceRule.Occurrences
                    select masterEvent.CreateOccurrence(DateTime.SpecifyKind(eventStart, DateTimeKind.Unspecified)))
                    .FirstOrDefault();
        }

        /// <summary>
        /// Converts a UTC time into an event's local time.
        /// </summary>
        /// <param name="eventTimeZone">The event time zone.</param>
        /// <param name="utcDate">The UTC date.</param>
        /// <param name="defaultTime">The default time.</param>
        /// <returns></returns>
        private static DateTime UtcToEventTime(TimeZoneInfo eventTimeZone, DateTime? utcDate, DateTime defaultTime)
        {
            var eventTime = defaultTime;
            if (utcDate.HasValue)
            {
                eventTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate.Value, eventTimeZone);
            }

            return eventTime;
        }

        /// <summary>
        /// Sorts and pages the collection.
        /// </summary>
        /// <param name="beginIndex">The index of the event which should begin the list.</param>
        /// <param name="endIndex">The index of the event which should end the list.</param>
        /// <param name="sortExpression">The name of the property on which the events should be sorted.</param>
        /// <param name="events">The events collection to sort and page.</param>
        private static void ProcessCollection(int beginIndex, int endIndex, string sortExpression, List<Event> events)
        {
            // TODO: we may need to remove this GenericComparer if performance becomes an issue
            var eventComparer = new GenericComparer<Event>(sortExpression, SortDirection.Ascending);
            events.Sort(eventComparer);

            int endCount = events.Count - endIndex;
            if (endCount > 0)
            {
                events.RemoveRange(endIndex, endCount);
            }

            if (beginIndex > 0)
            {
                events.RemoveRange(0, Math.Min(beginIndex, events.Count));
            }
        }

        /// <summary>
        /// Extends the date by the given span.
        /// </summary>
        /// <param name="utcDate">The UTC date.</param>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>The altered date</returns>
        private static DateTime? AddTimeSpan(DateTime? utcDate, TimeSpan timeSpan)
        {
            if (utcDate.HasValue)
            {
                utcDate = utcDate.Value.Add(timeSpan);
            }

            return utcDate;
        }
    }
}