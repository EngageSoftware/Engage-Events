// <copyright file="Event.cs" company="Engage Software">
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
    using System.Xml.Serialization;
    using Data;
    using Telerik.Web.UI;

    /// <summary>
    /// An event, with a title, description, location, and start and end date.
    /// </summary>
    [XmlRoot(ElementName = "event", IsNullable = false)]
    public class Event : IEditableObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Backing field for <see cref="Canceled"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool canceled;

        /// <summary>
        /// Backing field for <see cref="IsFeatured"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isFeatured;

        /// <summary>
        /// Backing field for <see cref="IsDeleted"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isDeleted;

        /// <summary>
        /// Backing field for <see cref="CreatedBy"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int createdBy = -1;

        /// <summary>
        /// Backing field for <see cref="EventEnd"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime eventEnd;

        /// <summary>
        /// Backing field for <see cref="EventStart"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime eventStart;

        /// <summary>
        /// Backing field for <see cref="Id"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int id = -1;

        /// <summary>
        /// Backing field for <see cref="InvitationUrl"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string invitationUrl = string.Empty;

        /// <summary>
        /// Backing field for <see cref="Location"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string location = string.Empty;

        /// <summary>
        /// Backing field for <see cref="LocationUrl"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string locationUrl = string.Empty;

        /// <summary>
        /// Backing field for <see cref="ModuleId"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int moduleId = -1;

        /// <summary>
        /// Backing field for <see cref="Organizer"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string organizer = string.Empty;

        /// <summary>
        /// Backing field for <see cref="OrganizerEmail"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string organizerEmail = string.Empty;

        /// <summary>
        /// Backing field for <see cref="Overview"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string overview = string.Empty;

        /// <summary>
        /// Backing field for <see cref="Overview"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string description = string.Empty;

        /// <summary>
        /// Backing field for <see cref="PortalId"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int portalId = -1;

        /// <summary>
        /// Backing field for <see cref="RecapUrl"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string recapUrl = string.Empty;

        /// <summary>
        /// Backing field for <see cref="RecurrenceParentId"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? recurrenceParentId;

        /// <summary>
        /// Backing field for <see cref="RecurrenceRule"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RecurrenceRule recurrenceRule;

        /// <summary>
        /// Backing field for <see cref="Title"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string title = string.Empty;

        /// <summary>
        /// Backing field for <see cref="AllowRegistrations"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool allowRegistrations = true;

        /// <summary>
        /// Backing field for <see cref="TimeZoneOffset"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan timeZoneOffset;

        /// <summary>
        /// Backing field for <see cref="Capacity"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? capacity;

        /// <summary>
        /// Backing field for <see cref="InDaylightTime"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool inDaylightTime;

        /// <summary>
        /// Backing field for <see cref="CapacityMetMessage"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string capacityMetMessage;

        /// <summary>
        /// Prevents a default instance of the Event class from being created.
        /// </summary>
        private Event()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="portalId">The portal id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="organizerEmail">The organizer email.</param>
        /// <param name="title">The title of the event.</param>
        /// <param name="overview">The overview (short description) of the event.</param>
        /// <param name="description">The event description.</param>
        /// <param name="eventStart">When the event starts.</param>
        /// <param name="eventEnd">When the event ends.</param>
        /// <param name="timeZoneOffset">The time zone offset.</param>
        /// <param name="location">The location of the event.</param>
        /// <param name="isFeatured">if set to <c>true</c> this event is featured.</param>
        /// <param name="allowRegistrations">if set to <c>true</c> this event allows users to register for it.</param>
        /// <param name="recurrenceRule">The recurrence rule.</param>
        /// <param name="capacity">The maximum number of registrants for this event, or <c>null</c> if there is no maximum.</param>
        /// <param name="inDaylightTime">if set to <c>true</c> this event occurs in Daylight Time.</param>
        /// <param name="capacityMetMessage">
        /// The the message to display to a user who wants to register for this
        /// event when the <see cref="Capacity"/> for this event has been met,  or 
        /// <c>null</c> or <see cref="string.Empty"/> to display a generic message.
        /// </param>
        private Event(int portalId, int moduleId, string organizerEmail, string title, string overview, string description, DateTime eventStart, DateTime eventEnd, TimeSpan timeZoneOffset, string location, bool isFeatured, bool allowRegistrations, RecurrenceRule recurrenceRule, int? capacity, bool inDaylightTime, string capacityMetMessage)
            : this(portalId, moduleId, organizerEmail, title, overview, description, eventStart, eventEnd, timeZoneOffset, location, isFeatured, allowRegistrations, recurrenceRule, false, capacity, inDaylightTime, capacityMetMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="portalId">The portal id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="organizerEmail">The organizer email.</param>
        /// <param name="title">The title of the event.</param>
        /// <param name="overview">The overview (short description) of the event.</param>
        /// <param name="description">The event description.</param>
        /// <param name="eventStart">When the event starts.</param>
        /// <param name="eventEnd">When the event ends.</param>
        /// <param name="timeZoneOffset">The time zone offset.</param>
        /// <param name="location">The location of the event.</param>
        /// <param name="isFeatured">if set to <c>true</c> this event is featured.</param>
        /// <param name="allowRegistrations">if set to <c>true</c> this event allows users to register for it.</param>
        /// <param name="recurrenceRule">The recurrence rule.</param>
        /// <param name="canceled">if set to <c>true</c> this event is canceled.</param>
        /// <param name="capacity">The maximum number of registrants for this event, or <c>null</c> if there is no maximum.</param>
        /// <param name="inDaylightTime">if set to <c>true</c> this event occurs in Daylight Time.</param>
        /// <param name="capacityMetMessage">
        /// The the message to display to a user who wants to register for this
        /// event when the <see cref="Capacity"/> for this event has been met,  or 
        /// <c>null</c> or <see cref="string.Empty"/> to display a generic message.
        /// </param>
        private Event(int portalId, int moduleId, string organizerEmail, string title, string overview, string description, DateTime eventStart, DateTime eventEnd, TimeSpan timeZoneOffset, string location, bool isFeatured, bool allowRegistrations, RecurrenceRule recurrenceRule, bool canceled, int? capacity, bool inDaylightTime, string capacityMetMessage)
        {
            this.portalId = portalId;
            this.moduleId = moduleId;
            this.organizerEmail = organizerEmail ?? string.Empty;
            this.title = title;
            this.overview = overview;
            this.description = description;
            this.eventStart = eventStart;
            this.eventEnd = eventEnd;
            this.timeZoneOffset = timeZoneOffset;
            this.location = location;
            this.isFeatured = isFeatured;
            this.allowRegistrations = allowRegistrations;
            this.recurrenceRule = recurrenceRule;
            this.canceled = canceled;
            this.capacity = capacity;
            this.inDaylightTime = inDaylightTime;
            this.capacityMetMessage = capacityMetMessage;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Gets the id of this event.
        /// </summary>
        /// <value>This <see cref="Event"/>'s id.</value>
        [XmlElement(Order = 1)]
        public int Id
        {
            [DebuggerStepThrough]
            get { return this.id; }
        }

        /// <summary>
        /// Gets the portal id.
        /// </summary>
        /// <value>The portal id.</value>
        public int PortalId
        {
            [DebuggerStepThrough]
            get { return this.portalId; }
        }

        /// <summary>
        /// Gets the module id.
        /// </summary>
        /// <value>The module id.</value>
        public int ModuleId
        {
            [DebuggerStepThrough]
            get { return this.moduleId; }
        }

        /// <summary>
        /// Gets the ID of the user who created this event.
        /// </summary>
        /// <value>The ID of the user that created this event.</value>
        public int CreatedBy
        {
            [DebuggerStepThrough]
            get { return this.createdBy; }
        }

        /// <summary>
        /// Gets the email address of the organizer of this event.
        /// </summary>
        /// <value>The organizer's email.</value>
        [XmlElement(Order = 5)]
        public string OrganizerEmail
        {
            [DebuggerStepThrough]
            get { return this.organizerEmail; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title of this event.</value>
        [XmlElement(Order = 2)]
        public string Title
        {
            [DebuggerStepThrough]
            get { return this.title; }
            [DebuggerStepThrough]
            set { this.title = value; }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location of this event.</value>
        [XmlElement(Order = 6)]
        public string Location
        {
            [DebuggerStepThrough]
            get { return this.location; }
            [DebuggerStepThrough]
            set { this.location = value; }
        }

        /// <summary>
        /// Gets or sets the URL for this event's <see cref="Location"/>.
        /// </summary>
        /// <value>The URL for this event's <see cref="Location"/>.</value>
        [XmlElement(Order = 7)]
        public string LocationUrl
        {
            [DebuggerStepThrough]
            get { return this.locationUrl; }
            [DebuggerStepThrough]
            set { this.locationUrl = value; }
        }

        /// <summary>
        /// Gets or sets the URL where the invitation HTML message is.
        /// </summary>
        /// <value>The URL for the invitation HTML message.</value>
        public string InvitationUrl
        {
            [DebuggerStepThrough]
            get { return this.invitationUrl; }
            [DebuggerStepThrough]
            set { this.invitationUrl = value; }
        }

        /// <summary>
        /// Gets or sets the URL where the recap HTML message is.
        /// </summary>
        /// <value>The URL for the recap HTML message.</value>
        public string RecapUrl
        {
            [DebuggerStepThrough]
            get { return this.recapUrl; }
            [DebuggerStepThrough]
            set { this.recapUrl = value; }
        }

        /// <summary>
        /// Gets or sets the overview.
        /// </summary>
        /// <value>The overview.</value>
        [XmlElement(Order = 3)]
        public string Overview
        {
            [DebuggerStepThrough]
            get { return this.overview; }
            [DebuggerStepThrough]
            set { this.overview = value; }
        }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>The Description.</value>
        [XmlElement(Order = 8)]
        public string Description
        {
            [DebuggerStepThrough]
            get { return this.description; }
            [DebuggerStepThrough]
            set { this.description = value; }
        }

        /// <summary>
        /// Gets or sets when the event starts.
        /// </summary>
        /// <value>The event's start date and time.</value>
        [XmlElement(Order = 9)]
        public DateTime EventStart
        {
            [DebuggerStepThrough]
            get { return this.eventStart; }
            [DebuggerStepThrough]
            set { this.eventStart = value; }
        }

        /// <summary>
        /// Gets or sets when this event ends.
        /// </summary>
        /// <value>The event's end date and time.</value>
        [XmlElement(Order = 10)]
        public DateTime EventEnd
        {
            [DebuggerStepThrough]
            get { return this.eventEnd; }
            [DebuggerStepThrough]
            set { this.eventEnd = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the parent event if this event is overriding a recurring event.
        /// </summary>
        /// <value>The recurrence id.</value>
        public int? RecurrenceParentId
        {
            [DebuggerStepThrough]
            get { return this.recurrenceParentId; }
            [DebuggerStepThrough]
            set { this.recurrenceParentId = value; }
        }

        /// <summary>
        /// Gets or sets the recurrence rule.
        /// </summary>
        /// <value>The recurrence rule.</value>
        public RecurrenceRule RecurrenceRule
        {
            [DebuggerStepThrough]
            get { return this.recurrenceRule; }
            [DebuggerStepThrough]
            set { this.recurrenceRule = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Event"/> is canceled.
        /// </summary>
        /// <value><c>true</c> if canceled; otherwise, <c>false</c>.</value>
        public bool Canceled
        {
            [DebuggerStepThrough]
            get { return this.canceled; }
            [DebuggerStepThrough]
            set { this.canceled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is featured.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is featured; otherwise, <c>false</c>.
        /// </value>
        [XmlElement(Order = 11)]
        public bool IsFeatured
        {
            [DebuggerStepThrough]
            get { return this.isFeatured; }
            [DebuggerStepThrough]
            set { this.isFeatured = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        [XmlElement(Order = 12)]
        public bool IsDeleted
        {
            [DebuggerStepThrough]
            get { return this.isDeleted; }
            [DebuggerStepThrough]
            set { this.isDeleted = value; }
        }

        /// <summary>
        /// Gets or sets the name of the organizer of this event.
        /// </summary>
        /// <value>The event's organizer.</value>
        [XmlElement(Order = 4)]
        public string Organizer
        {
            [DebuggerStepThrough]
            get { return this.organizer; }
            [DebuggerStepThrough]
            set { this.organizer = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether users can register to attend this event.
        /// </summary>
        /// <value><c>true</c> if users can register to attend this event; otherwise, <c>false</c>.</value>
        [XmlElement(Order = 13)]
        public bool AllowRegistrations
        {
            [DebuggerStepThrough]
            get { return this.allowRegistrations; }
            [DebuggerStepThrough]
            set { this.allowRegistrations = value; }
        }

        /// <summary>
        /// Gets or sets the time zone offset for this event.
        /// </summary>
        /// <value>The time zone offset for this event.</value>
        [XmlElement(Order = 14)]
        public TimeSpan TimeZoneOffset
        {
            [DebuggerStepThrough]
            get { return this.timeZoneOffset; }
            [DebuggerStepThrough]
            set { this.timeZoneOffset = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of attending registrants this event can have, or <c>null</c> if there is no maximum.
        /// </summary>
        /// <value>The the maximum number of attending registrants this event can have, or <c>null</c> if there is no maximum.</value>
        [XmlElement(Order = 15)]
        public int? Capacity
        {
            [DebuggerStepThrough]
            get { return this.capacity; }
            [DebuggerStepThrough]
            set { this.capacity = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this event occurs during Daylight Time.
        /// </summary>
        /// <remarks>
        /// This property is mainly needed to determine whether to check the Daylight Time check box on the event's edit page.
        /// The <see cref="TimeZoneOffset"/> property is already adjusted based on that same check box.
        /// </remarks>
        /// <value><c>true</c> if this event occurs during Daylight Time; otherwise, <c>false</c>.</value>
        [XmlElement(Order = 16)]
        public bool InDaylightTime
        {
            [DebuggerStepThrough]
            get { return this.inDaylightTime; }
            [DebuggerStepThrough]
            set { this.inDaylightTime = value; }
        }

        /// <summary>
        /// Gets or sets the message to display to a user who wants to register for this
        /// event when the <see cref="Capacity"/> for this event has been met.
        /// </summary>
        /// <value>
        /// The the message to display to a user who wants to register for this
        /// event when the <see cref="Capacity"/> for this event has been met,  or 
        /// <c>null</c> or <see cref="string.Empty"/> to display a generic message.
        /// </value>
        [XmlElement(Order = 17)]
        public string CapacityMetMessage
        {
            [DebuggerStepThrough]
            get { return this.capacityMetMessage; }
            [DebuggerStepThrough]
            set { this.capacityMetMessage = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is recurring.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is recurring; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsRecurring
        {
            get { return this.RecurrenceRule != null; }
        }

        /// <summary>
        /// Gets the duration of this event.
        /// </summary>
        /// <value>The event's duration</value>
        [XmlIgnore]
        public TimeSpan Duration
        {
            get { return this.eventEnd - this.eventStart; }
        }

        /// <summary>
        /// Gets the final recurring end date.
        /// </summary>
        /// <value>The final recurring end date.</value>
        [XmlIgnore]
        private DateTime? FinalRecurringEndDate
        {
            get
            {
                if (this.recurrenceRule == null 
                    || (0 == this.recurrenceRule.Range.MaxOccurrences && this.recurrenceRule.Range.RecursUntil == DateTime.MaxValue))
                {
                    return null;
                }

                // TODO: Calculate FinalRecurringEndDate efficiently
                int originalMaxOccurrences = this.recurrenceRule.Range.MaxOccurrences;
                this.recurrenceRule.Range.MaxOccurrences = int.MaxValue;

                DateTime lastOccurrence = this.eventStart;
                foreach (DateTime occurrence in this.recurrenceRule.Occurrences)
                {
                    lastOccurrence = occurrence;
                }

                this.recurrenceRule.Range.MaxOccurrences = originalMaxOccurrences;

                return lastOccurrence + this.recurrenceRule.Range.EventDuration;
            }
        }

        /// <summary>
        /// Loads the specified event.
        /// </summary>
        /// <param name="id">The id of an event to load.</param>
        /// <returns>The requested Event</returns>
        /// <exception cref="DBException">If an error occurs while going to the database to get the event</exception>
        public static Event Load(int id)
        {
            IDataProvider dp = DataProvider.Instance;
            Event e = null;

            try
            {
                using (IDataReader reader = dp.ExecuteReader(CommandType.StoredProcedure, dp.NamePrefix + "spGetEvent", Utility.CreateIntegerParam("@EventId", id)))
                {
                    if (reader.Read())
                    {
                        e = Fill(reader);
                    }
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetEvent", se);
            }

            return e;
        }

        /// <summary>
        /// Creates the specified event.
        /// </summary>
        /// <param name="portalId">The portal id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="organizerEmail">The organizer email.</param>
        /// <param name="title">The title of the event.</param>
        /// <param name="overview">The overview or description of the event.</param>
        /// <param name="description">The description.</param>
        /// <param name="eventStart">The event's start date and time.</param>
        /// <param name="eventEnd">The event end.</param>
        /// <param name="timeZoneOffset">The time zone offset.</param>
        /// <param name="location">The location of the event.</param>
        /// <param name="isFeatured">if set to <c>true</c> the event should be listed in featured displays.</param>
        /// <param name="allowRegistrations">if set to <c>true</c> this event allows users to register for it.</param>
        /// <param name="recurrenceRule">The recurrence rule.</param>
        /// <param name="capacity">The maximum number of registrants for this event, or <c>null</c> if there is no maximum.</param>
        /// <param name="inDaylightTime">if set to <c>true</c> this event occurs in Daylight Time.</param>
        /// <param name="capacityMetMessage">
        /// The the message to display to a user who wants to register for this
        /// event when the <see cref="Capacity"/> for this event has been met,  or 
        /// <c>null</c> or <see cref="string.Empty"/> to display a generic message.
        /// </param>
        /// <returns>A new event object.</returns>
        public static Event Create(int portalId, int moduleId, string organizerEmail, string title, string overview, string description, DateTime eventStart, DateTime eventEnd, TimeSpan timeZoneOffset, string location, bool isFeatured, bool allowRegistrations, RecurrenceRule recurrenceRule, int? capacity, bool inDaylightTime, string capacityMetMessage)
        {
            return new Event(portalId, moduleId, organizerEmail, title, overview, description, eventStart, eventEnd, timeZoneOffset, location, isFeatured, allowRegistrations, recurrenceRule, capacity, inDaylightTime, capacityMetMessage);
        }

        /// <summary>
        /// Deletes the specified event.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <exception cref="DBException">If an error occurs while going to the database to delete the event</exception>
        public static void Delete(int eventId)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spDeleteEvent", Utility.CreateIntegerParam("@EventId", eventId));
            }
            catch (Exception se)
            {
                throw new DBException("spDeleteEvent", se);
            }
        }

        /// <summary>
        /// Creates an occurrence of this <see cref="Event"/>, for the given <paramref name="occurrenceStart"/>.
        /// </summary>
        /// <param name="occurrenceStart">The date and time at which this occurrence starts.</param>
        /// <returns>An occurrence of this <see cref="Event"/></returns>
        public Event CreateOccurrence(DateTime occurrenceStart)
        {
            Event occurrence = new Event(this.portalId, this.moduleId, this.organizerEmail, this.title, this.overview, this.description, occurrenceStart, occurrenceStart + this.Duration, this.TimeZoneOffset, this.location, this.isFeatured, this.allowRegistrations, this.recurrenceRule, this.canceled, this.capacity, this.inDaylightTime, this.capacityMetMessage);
            occurrence.recurrenceParentId = this.id;
            occurrence.id = this.id;
            return occurrence;
        }

        /// <summary>
        /// Saves this event.
        /// </summary>
        /// <param name="revisingUser">The user who is saving this event.</param>
        public void Save(int revisingUser)
        {
            if (this.id < 0)
            {
                this.Insert(revisingUser);
            }
            else
            {
                this.Update(revisingUser);
            }
        }

        /// <summary>
        /// Creates an iCal representation of this event.
        /// </summary>
        /// <returns>An iCal representation of this event</returns>
        public string ToICal()
        {
            string rule = this.RecurrenceRule != null ? this.RecurrenceRule.ToString() : null;
            return Util.ICalUtil.Export(this.overview, this.location, new Appointment(this.Id, this.EventStart, this.EventEnd, this.Title, rule), true, this.TimeZoneOffset);
        }

        #region IEditableObject Members

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
        }

        #endregion

        /// <summary>
        /// Fills an Event with the data in the specified <paramref name="eventRecord"/>.
        /// </summary>
        /// <param name="eventRecord">A pre-initialized data record that represents an Event instance.</param>
        /// <returns>An instantiated Event object.</returns>
        internal static Event Fill(IDataRecord eventRecord)
        {
            Event e = new Event();

            e.id = (int)eventRecord["EventId"];
            e.moduleId = (int)eventRecord["ModuleId"];
            e.title = eventRecord["Title"].ToString();
            e.overview = eventRecord["OverView"].ToString();
            e.description = eventRecord["Description"].ToString();
            e.eventStart = (DateTime)eventRecord["EventStart"];
            e.eventEnd = (DateTime)eventRecord["EventEnd"];
            e.timeZoneOffset = new TimeSpan(0, (int)eventRecord["TimeZoneOffset"], 0);
            e.createdBy = (int)eventRecord["CreatedBy"];
            e.canceled = (bool)eventRecord["Canceled"];
            e.isFeatured = (bool)eventRecord["IsFeatured"];
            e.isDeleted = (bool)eventRecord["IsDeleted"];
            e.allowRegistrations = (bool)eventRecord["AllowRegistrations"];
            e.organizer = eventRecord["Organizer"].ToString();
            e.organizerEmail = eventRecord["OrganizerEmail"].ToString();
            e.location = eventRecord["Location"].ToString();
            e.invitationUrl = eventRecord["InvitationUrl"].ToString();
            e.recapUrl = eventRecord["RecapUrl"].ToString();
            e.recurrenceParentId = eventRecord["RecurrenceParentId"] as int?;
            e.capacity = eventRecord["Capacity"] as int?;
            e.inDaylightTime = (bool)eventRecord["InDaylightTime"];

            int columnIndex = eventRecord.GetOrdinal("CapacityMetMessage");
            e.capacityMetMessage = eventRecord.IsDBNull(eventRecord.GetOrdinal("CapacityMetMessage")) ? null : eventRecord.GetString(columnIndex);
            
            RecurrenceRule rule;
            if (RecurrenceRule.TryParse(eventRecord["RecurrenceRule"].ToString(), out rule))
            {
                e.recurrenceRule = rule;
            }
            
            return e;
        }

        /// <summary>
        /// Inserts this event.
        /// </summary>
        /// <param name="revisingUser">The user who is inserting this event.</param>
        /// <exception cref="DBException">If an error occurs while going to the database to insert the event</exception>
        private void Insert(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                this.id = dp.ExecuteNonQuery(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spInsertEvent",
                        Utility.CreateIntegerParam("@PortalId", this.portalId),
                        Utility.CreateIntegerParam("@ModuleId", this.moduleId),
                        Utility.CreateVarcharParam("@Title", this.title),
                        Utility.CreateTextParam("@Overview", this.overview),
                        Utility.CreateTextParam("@Description", this.description),
                        Utility.CreateDateTimeParam("@EventStart", this.eventStart),
                        Utility.CreateDateTimeParam("@EventEnd", this.eventEnd),
                        Utility.CreateIntegerParam("@TimeZoneOffset", (int)this.timeZoneOffset.TotalMinutes),
                        Utility.CreateVarcharParam("@Organizer", this.organizer),
                        Utility.CreateVarcharParam("@OrganizerEmail", this.organizerEmail),
                        Utility.CreateVarcharParam("@Location", this.location),
                        Utility.CreateVarcharParam("@LocationUrl", this.locationUrl),
                        Utility.CreateVarcharParam("@InvitationUrl", this.invitationUrl),
                        Utility.CreateVarcharParam("@RecapUrl", this.recapUrl),
                        Utility.CreateIntegerParam("@RecurrenceParentId", this.recurrenceParentId),
                        Utility.CreateVarcharParam("@RecurrenceRule", this.recurrenceRule != null ? this.recurrenceRule.ToString() : null),
                        Utility.CreateBitParam("@AllowRegistrations", this.allowRegistrations),
                        Utility.CreateBitParam("@isFeatured", this.isFeatured),
                        Utility.CreateIntegerParam("@CreatedBy", revisingUser),
                        Utility.CreateDateTimeParam("@FinalRecurringEndDate", this.FinalRecurringEndDate),
                        Utility.CreateIntegerParam("@Capacity", this.capacity),
                        Utility.CreateBitParam("@InDaylightTime", this.inDaylightTime),
                        Utility.CreateTextParam("@CapacityMetMessage", this.capacityMetMessage),
                        Utility.CreateBitParam("@IsDeleted", this.isDeleted));
            }
            catch (SystemException de)
            {
                throw new DBException("spInsertEvent", de);
            }
        }

        /// <summary>
        /// Updates this event.
        /// </summary>
        /// <param name="revisingUser">The user responsible for updating this event.</param>
        /// <exception cref="DBException">If an error occurs while going to the database to update the event</exception>
        private void Update(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spUpdateEvent",
                        Utility.CreateIntegerParam("@EventId", this.id),
                        Utility.CreateVarcharParam("@Title", this.title),
                        Utility.CreateTextParam("@Overview", this.overview),
                        Utility.CreateTextParam("@Description", this.description),
                        Utility.CreateDateTimeParam("@EventStart", this.eventStart),
                        Utility.CreateDateTimeParam("@EventEnd", this.eventEnd),
                        Utility.CreateIntegerParam("@TimeZoneOffset", (int)this.timeZoneOffset.TotalMinutes),
                        Utility.CreateVarcharParam("@Organizer", this.organizer),
                        Utility.CreateVarcharParam("@OrganizerEmail", this.organizerEmail),
                        Utility.CreateVarcharParam("@Location", this.location),
                        Utility.CreateVarcharParam("@LocationUrl", this.locationUrl),
                        Utility.CreateVarcharParam("@InvitationUrl", this.invitationUrl),
                        Utility.CreateVarcharParam("@RecapUrl", this.recapUrl),
                        Utility.CreateTextParam("@RecurrenceRule", this.recurrenceRule != null ? this.recurrenceRule.ToString() : null),
                        Utility.CreateIntegerParam("@RecurrenceParentId", this.recurrenceParentId),
                        Utility.CreateBitParam("@AllowRegistrations", this.allowRegistrations),
                        Utility.CreateBitParam("@Canceled", this.canceled),
                        Utility.CreateBitParam("@isFeatured", this.isFeatured),
                        Utility.CreateIntegerParam("@RevisingUser", revisingUser),
                        Utility.CreateDateTimeParam("@FinalRecurringEndDate", this.FinalRecurringEndDate),
                        Utility.CreateIntegerParam("@Capacity", this.capacity),
                        Utility.CreateBitParam("@InDaylightTime", this.inDaylightTime),
                        Utility.CreateTextParam("@CapacityMetMessage", this.capacityMetMessage),
                        Utility.CreateBitParam("@IsDeleted", this.isDeleted));
            }
            catch (SystemException de)
            {
                throw new DBException("spUpdateEvent", de);
            }
        }
    }
}
