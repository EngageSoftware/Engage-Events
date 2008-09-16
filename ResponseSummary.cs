// <copyright file="ResponseSummary.cs" company="Engage Software">
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
    public class ResponseSummary
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
        /// Backing field for <see cref="EventStart"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime eventStart;

        /// <summary>
        /// Prevents a default instance of the ResponseSummary class from being created.
        /// </summary>
        private ResponseSummary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseSummary"/> class.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">When the event starts.</param>
        /// <param name="title">The title of the event.</param>
        /// <param name="attending">The number of folks attending.</param>
        /// <param name="notAttending">The number of folks not attending.</param>
        /// <param name="noResponse">The number of folks who haven't responded.</param>
        private ResponseSummary(int eventId, DateTime eventStart, string title, int attending, int notAttending, int noResponse)
        {
            this.eventId = eventId;
            this.eventStart = eventStart;
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
            get { return this.eventStart; }
        }

        /// <summary>
        /// Gets the date the event starts.
        /// </summary>
        /// <value>The date the event starts.</value>
        public DateTime EventEnd
        {
            [DebuggerStepThrough]
            get { return this.eventStart + this.Event.Duration; }
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
        /// Creates a new <see cref="ResponseSummary"/>.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">When the event starts.</param>
        /// <param name="title">The title of the event.</param>
        /// <param name="attending">The number of folks attending.</param>
        /// <param name="notAttending">The number of folks not attending.</param>
        /// <param name="noResponse">The number of folks who haven't responded.</param>
        /// <returns>
        /// A new <see cref="ResponseSummary"/> instance
        /// </returns>
        public static ResponseSummary Create(int eventId, DateTime eventStart, string title, int attending, int notAttending, int noResponse)
        {
            return new ResponseSummary(eventId, eventStart, title, attending, notAttending, noResponse);
        }

        /// <summary>
        /// Loads the <see cref="ResponseSummary"/> for the specified event.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date for which this response was made.</param>
        /// <returns>
        /// The <see cref="ResponseSummary"/> for the specified event
        /// </returns>
        /// <exception cref="DBException">If an error occurs when retrieving the <see cref="ResponseSummary"/> from the database</exception>
        public static ResponseSummary Load(int eventId, DateTime eventStart)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (IDataReader dataReader = dp.ExecuteReader(
                    CommandType.StoredProcedure, 
                    dp.NamePrefix + "spGetResponseSummary",
                    Utility.CreateIntegerParam("@EventId", eventId),
                    Utility.CreateDateTimeParam("@EventStart", eventStart)))
                {
                    if (dataReader.Read())
                    {
                        return Fill(dataReader);
                    }
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetResponseSummary", se);
            }

            return null;
        }

        /// <summary>
        /// Instantiates a new <see cref="ResponseSummary"/> based on the provided <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dataRecord">The data record containing a representation of an <see cref="ResponseSummary"/>.</param>
        /// <returns>The instantiated <see cref="ResponseSummary"/></returns>
        internal static ResponseSummary Fill(IDataRecord dataRecord)
        {
            ResponseSummary r = new ResponseSummary();
            r.eventId = (int)dataRecord["EventId"];
            r.eventStart = (DateTime)dataRecord["EventStart"];
            r.attending = (int)dataRecord["Attending"];
            r.notAttending = (int)dataRecord["NotAttending"];
            r.noResponse = (int)dataRecord["NoResponse"];

            return r;
        }
    }
}