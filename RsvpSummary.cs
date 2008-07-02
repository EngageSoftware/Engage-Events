// <copyright file="RsvpSummary.cs" company="Engage Software">
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
    using System.Data;
    using System.Diagnostics;
    using Data;

    /// <summary>
    /// A representation of all of the responses received for a particular event.
    /// </summary>
    public class RsvpSummary
    {
        /// <summary>
        /// Backing field for <see cref="Attending"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int attending;

        /// <summary>
        /// Backing field for <see cref="EventId"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int eventId = -1;

        /// <summary>
        /// Backing field for <see cref="Event"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Event @event;

        /// <summary>
        /// Backing field for <see cref="EventStart"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime eventStart;

        /// <summary>
        /// Backing field for <see cref="NoResponse"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int noResponse;

        /// <summary>
        /// Backing field for <see cref="NotAttending"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int notAttending;

        /// <summary>
        /// Backing field for <see cref="Title"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string title = string.Empty;

        /// <summary>
        /// Backing field for <see cref="TotalRecords"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int totalRecords;

        /// <summary>
        /// Initializes a new instance of the <see cref="RsvpSummary"/> class.
        /// </summary>
        private RsvpSummary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsvpSummary"/> class.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="title">The title of the event.</param>
        /// <param name="attending">The number of folks attending.</param>
        /// <param name="notAttending">The number of folks not attending.</param>
        /// <param name="noResponse">The number of folks who haven't responded.</param>
        private RsvpSummary(int eventId, string title, int attending, int notAttending, int noResponse)
        {
            this.eventId = eventId;
            this.title = title;
            this.attending = attending;
            this.notAttending = notAttending;
            this.noResponse = noResponse;
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        public int EventId
        {
            [DebuggerStepThrough]
            get { return this.eventId; }
        }

        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <value>The event.</value>
        public Event Event
        {
            get
            {
                if (this.@event == null)
                {
                    this.@event = Event.Load(this.eventId);
                }

                return this.@event;
            }
        }

        /// <summary>
        /// Gets the title of the event.
        /// </summary>
        /// <value>The title of the event.</value>
        public string Title
        {
            [DebuggerStepThrough]
            get { return this.Event.Title; }
        }

        /// <summary>
        /// Gets the date the event starts.
        /// </summary>
        /// <value>The date the event starts.</value>
        public DateTime EventStart
        {
            [DebuggerStepThrough]
            get { return this.Event.EventStart; }
        }

        /// <summary>
        /// Gets the date the event starts.
        /// </summary>
        /// <value>The date the event starts.</value>
        public DateTime EventEnd
        {
            [DebuggerStepThrough]
            get { return this.Event.EventEnd; }
        }

        /// <summary>
        /// Gets the number of folks attending.
        /// </summary>
        /// <value>The number of folks attending.</value>
        public int Attending
        {
            [DebuggerStepThrough]
            get { return this.attending; }
        }

        /// <summary>
        /// Gets the number of folks not attending.
        /// </summary>
        /// <value>The number of folks not attending.</value>
        public int NotAttending
        {
            [DebuggerStepThrough]
            get { return this.notAttending; }
        }

        /// <summary>
        /// Gets the number of folks who haven't responded.
        /// </summary>
        /// <value>The number of folks who haven't responded.</value>
        public int NoResponse
        {
            [DebuggerStepThrough]
            get { return this.noResponse; }
        }

        /// <summary>
        /// Gets the total number of records.
        /// </summary>
        /// <value>The total records.</value>
        public int TotalRecords
        {
            [DebuggerStepThrough]
            get { return this.totalRecords; }
        }

        /// <summary>
        /// Creates a new <see cref="RsvpSummary"/>.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="title">The title of the event.</param>
        /// <param name="attending">The number of folks attending.</param>
        /// <param name="notAttending">The number of folks not attending.</param>
        /// <param name="noResponse">The number of folks who haven't responded.</param>
        /// <returns>A new <see cref="RsvpSummary"/> instance</returns>
        public static RsvpSummary Create(int eventId, string title, int attending, int notAttending, int noResponse)
        {
            return new RsvpSummary(eventId, title, attending, notAttending, noResponse);
        }

        /// <summary>
        /// Loads the <see cref="RsvpSummary"/> for the specified event.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <returns>The <see cref="RsvpSummary"/> for the specified event</returns>
        /// <exception cref="DBException">If an error occurs when retrieving the <see cref="RsvpSummary"/> from the database</exception>
        public static RsvpSummary Load(int eventId)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (IDataReader dataReader = dp.ExecuteReader(
                    CommandType.StoredProcedure, 
                    dp.NamePrefix + "spGetRsvpSummary",
                    Utility.CreateIntegerParam("@EventId", eventId)))
                {
                    if (dataReader.Read())
                    {
                        return Fill(dataReader);
                    }
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetRsvps", se);
            }

            return null;
        }

        /// <summary>
        /// Instantiates a new <see cref="RsvpSummary"/> based on the provided <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dataRecord">The data record containing a representation of an <see cref="RsvpSummary"/>.</param>
        /// <returns>The instantiated <see cref="RsvpSummary"/></returns>
        internal static RsvpSummary Fill(IDataRecord dataRecord)
        {
            RsvpSummary r = new RsvpSummary();
            r.eventId = (int)dataRecord["EventId"];
            r.attending = (int)dataRecord["Attending"];
            r.notAttending = (int)dataRecord["NotAttending"];
            r.noResponse = (int)dataRecord["NoResponse"];

            return r;
        }
    }
}