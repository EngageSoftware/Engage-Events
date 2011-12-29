// <copyright file="Event.cs" company="Engage Software">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Serialization;
    using Data;
    using Dnn.Framework.Templating;

    using Engage.Annotations;

    using Telerik.Web.UI;

    /// <summary>
    /// An event, with a title, description, location, and start and end date.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Event", Justification = "Breaking change")]
    [XmlRoot(ElementName = "event", IsNullable = false)]
    public class Event : ITemplateable
    {
        /// <summary>
        /// Backing field for <see cref="Category"/>
        /// </summary>
        private Category category;

        /// <summary>
        /// Backing field for <see cref="HasAttendees"/> and <see cref="AttendeeCount"/>
        /// </summary>
        private int? attendeeCount;

        /// <summary>
        /// Backing field for <see cref="HasResponses"/> and <see cref="ResponseCount"/>
        /// </summary>
        private int? responseCount;

        /// <summary>
        /// Prevents a default instance of the Event class from being created.
        /// </summary>
        private Event()
        {
            this.Id = -1;
            this.PortalId = -1;
            this.ModuleId = -1;
            this.CreatedBy = -1;
            this.OrganizerEmail = string.Empty;
            this.Title = string.Empty;
            this.Location = string.Empty;
            this.LocationUrl = string.Empty;
            this.InvitationUrl = string.Empty;
            this.RecapUrl = string.Empty;
            this.Overview = string.Empty;
            this.Description = string.Empty;
            this.AllowRegistrations = true;
            this.Organizer = string.Empty;
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
        /// <param name="categoryId">The ID of the event's <see cref="Category"/>.</param>
        private Event(int portalId, int moduleId, string organizerEmail, string title, string overview, string description, DateTime eventStart, DateTime eventEnd, TimeSpan timeZoneOffset, string location, bool isFeatured, bool allowRegistrations, RecurrenceRule recurrenceRule, int? capacity, bool inDaylightTime, string capacityMetMessage, int categoryId)
            : this(portalId, moduleId, organizerEmail, title, overview, description, eventStart, eventEnd, timeZoneOffset, location, isFeatured, allowRegistrations, recurrenceRule, false, capacity, inDaylightTime, capacityMetMessage, categoryId)
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
        /// <param name="capacityMetMessage">The the message to display to a user who wants to register for this
        /// event when the <see cref="Capacity"/> for this event has been met,  or
        /// <c>null</c> or <see cref="string.Empty"/> to display a generic message.</param>
        /// <param name="categoryId">The ID of the event's <see cref="Category"/>.</param>
        private Event(int portalId, int moduleId, string organizerEmail, string title, string overview, string description, DateTime eventStart, DateTime eventEnd, TimeSpan timeZoneOffset, string location, bool isFeatured, bool allowRegistrations, RecurrenceRule recurrenceRule, bool canceled, int? capacity, bool inDaylightTime, string capacityMetMessage, int categoryId)
        {
            this.Id = -1;
            this.CreatedBy = -1;
            this.LocationUrl = string.Empty;
            this.InvitationUrl = string.Empty;
            this.RecapUrl = string.Empty;
            this.Organizer = string.Empty;
            this.PortalId = portalId;
            this.ModuleId = moduleId;
            this.OrganizerEmail = organizerEmail ?? string.Empty;
            this.Title = title;
            this.Overview = overview;
            this.Description = description;
            this.EventStart = eventStart;
            this.EventEnd = eventEnd;
            this.TimeZoneOffset = timeZoneOffset;
            this.Location = location;
            this.IsFeatured = isFeatured;
            this.AllowRegistrations = allowRegistrations;
            this.RecurrenceRule = recurrenceRule;
            this.Canceled = canceled;
            this.Capacity = capacity;
            this.InDaylightTime = inDaylightTime;
            this.CapacityMetMessage = capacityMetMessage;
            this.CategoryId = categoryId;
        }

        /// <summary>
        /// Gets the id of this event.
        /// </summary>
        /// <value>This <see cref="Event"/>'s id.</value>
        [XmlElement(Order = 1)]
        public int Id { get; private set; }

        /// <summary>
        /// Gets the portal id.
        /// </summary>
        /// <value>The portal id.</value>
        public int PortalId { get; private set; }

        /// <summary>
        /// Gets the module id.
        /// </summary>
        /// <value>The module id.</value>
        public int ModuleId { get; private set; }

        /// <summary>
        /// Gets the ID of the user who created this event.
        /// </summary>
        /// <value>The ID of the user that created this event.</value>
        public int CreatedBy { get; private set; }

        /// <summary>
        /// Gets the date when this event was created.
        /// </summary>
        /// <value>The date when this event was created.</value>
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// Gets the date when this event was last edited.
        /// </summary>
        /// <value>The date when this event was last edited.</value>
        public DateTime RevisionDate { get; private set; }

        /// <summary>
        /// Gets the email address of the organizer of this event.
        /// </summary>
        /// <value>The organizer's email.</value>
        [XmlElement(Order = 5)]
        public string OrganizerEmail { get; private set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title of this event.</value>
        [XmlElement(Order = 2)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location of this event.</value>
        [XmlElement(Order = 6)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the URL for this event's <see cref="Location"/>.
        /// </summary>
        /// <value>The URL for this event's <see cref="Location"/>.</value>
        [XmlElement(Order = 7)]
        public string LocationUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL where the invitation HTML message is.
        /// </summary>
        /// <value>The URL for the invitation HTML message.</value>
        public string InvitationUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL where the recap HTML message is.
        /// </summary>
        /// <value>The URL for the recap HTML message.</value>
        public string RecapUrl { get; set; }

        /// <summary>
        /// Gets or sets the overview.
        /// </summary>
        /// <value>The overview.</value>
        [XmlElement(Order = 3)]
        public string Overview { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>The Description.</value>
        [XmlElement(Order = 8)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets when the event starts.
        /// </summary>
        /// <value>The event's start date and time.</value>
        [XmlElement(Order = 9)]
        public DateTime EventStart { get; set; }

        /// <summary>
        /// Gets or sets when this event ends.
        /// </summary>
        /// <value>The event's end date and time.</value>
        [XmlElement(Order = 10)]
        public DateTime EventEnd { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent event if this event is overriding a recurring event.
        /// </summary>
        /// <value>The recurrence id.</value>
        public int? RecurrenceParentId { get; set; }

        /// <summary>
        /// Gets or sets the recurrence rule.
        /// </summary>
        /// <value>The recurrence rule.</value>
        public RecurrenceRule RecurrenceRule { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Event"/> is canceled.
        /// </summary>
        /// <value><c>true</c> if canceled; otherwise, <c>false</c>.</value>
        public bool Canceled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is featured.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is featured; otherwise, <c>false</c>.
        /// </value>
        [XmlElement(Order = 11)]
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        [XmlElement(Order = 12)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the name of the organizer of this event.
        /// </summary>
        /// <value>The event's organizer.</value>
        [XmlElement(Order = 4)]
        public string Organizer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users can register to attend this event.
        /// </summary>
        /// <value><c>true</c> if users can register to attend this event; otherwise, <c>false</c>.</value>
        [XmlElement(Order = 13)]
        public bool AllowRegistrations { get; set; }

        /// <summary>
        /// Gets or sets the time zone offset for this event.
        /// </summary>
        /// <value>The time zone offset for this event.</value>
        [XmlElement(Order = 14)]
        public TimeSpan TimeZoneOffset { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of attending registrants this event can have, or <c>null</c> if there is no maximum.
        /// </summary>
        /// <value>The the maximum number of attending registrants this event can have, or <c>null</c> if there is no maximum.</value>
        [XmlElement(Order = 15)]
        public int? Capacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this event occurs during Daylight Time.
        /// </summary>
        /// <remarks>
        /// This property is mainly needed to determine whether to check the Daylight Time check box on the event's edit page.
        /// The <see cref="TimeZoneOffset"/> property is already adjusted based on that same check box.
        /// </remarks>
        /// <value><c>true</c> if this event occurs during Daylight Time; otherwise, <c>false</c>.</value>
        [XmlElement(Order = 16)]
        public bool InDaylightTime { get; set; }

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
        public string CapacityMetMessage { get; set; }

        /// <summary>
        /// Gets or sets the ID of this event's category.
        /// </summary>
        /// <value>This <see cref="Event"/>'s category ID.</value>
        [XmlElement(Order = 18)]
        public int CategoryId { get; set; }

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
            get { return this.EventEnd - this.EventStart; }
        }

        /// <summary>
        /// Gets this event's category.
        /// </summary>
        /// <value>The category in which this event lives.</value>
        [XmlIgnore]
        public Category Category 
        {
            get 
            { 
                if (this.category == null)
                {
                    this.category = Category.Load(this.CategoryId);
                }

                return this.category;
            }
        }

        /// <summary>
        /// Gets the number of responses to this event.
        /// </summary>
        [XmlIgnore]
        public int ResponseCount
        {
            get
            {
                if (this.responseCount == null)
                {
                    this.responseCount = ResponseCollection.Load(this.Id, this.EventStart, null, null, 0, -1, null).TotalRecords;
                }

                return this.responseCount.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has any responses.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has responses; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool HasResponses
        {
            get
            {
                return this.ResponseCount > 0;
            }
        }

        /// <summary>
        /// Gets the number of responses to this event.
        /// </summary>
        [XmlIgnore]
        public int AttendeeCount
        {
            get
            {
                if (this.attendeeCount == null)
                {
                    this.attendeeCount = ResponseCollection.Load(this.Id, this.EventStart, ResponseStatus.Attending.ToString(), null, 0, -1, null).TotalRecords;
                }

                return this.attendeeCount.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has any attending responses.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has anyone registered as attending; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool HasAttendees
        {
            get
            {
                return this.AttendeeCount > 0;
            }
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
                if (this.RecurrenceRule == null 
                    || (0 == this.RecurrenceRule.Range.MaxOccurrences && this.RecurrenceRule.Range.RecursUntil == DateTime.MaxValue))
                {
                    return null;
                }

                int originalMaxOccurrences = this.RecurrenceRule.Range.MaxOccurrences;
                if (originalMaxOccurrences == 0)
                {
                    // if RecursUntil is set, we need to allow that limit to be hit
                    // otherwise, Occurrences will return nothing, since MaxOccurrences is zero
                    this.RecurrenceRule.Range.MaxOccurrences = int.MaxValue;
                }

                // TODO: Calculate FinalRecurringEndDate more efficiently
                var lastOccurrence = this.RecurrenceRule.Occurrences.Cast<DateTime?>().LastOrDefault() ?? this.EventStart;

                this.RecurrenceRule.Range.MaxOccurrences = originalMaxOccurrences;

                return lastOccurrence + this.RecurrenceRule.Range.EventDuration;
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
        /// <param name="categoryId">The ID of the event's <see cref="Category"/>.</param>
        /// <returns>A new event object.</returns>
        public static Event Create(int portalId, int moduleId, string organizerEmail, string title, string overview, string description, DateTime eventStart, DateTime eventEnd, TimeSpan timeZoneOffset, string location, bool isFeatured, bool allowRegistrations, RecurrenceRule recurrenceRule, int? capacity, bool inDaylightTime, string capacityMetMessage, int categoryId)
        {
            return new Event(portalId, moduleId, organizerEmail, title, overview, description, eventStart, eventEnd, timeZoneOffset, location, isFeatured, allowRegistrations, recurrenceRule, capacity, inDaylightTime, capacityMetMessage, categoryId);
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
            return new Event(this.PortalId, this.ModuleId, this.OrganizerEmail, this.Title, this.Overview, this.Description, occurrenceStart, occurrenceStart + this.Duration, this.TimeZoneOffset, this.Location, this.IsFeatured, this.AllowRegistrations, this.RecurrenceRule, this.Canceled, this.Capacity, this.InDaylightTime, this.CapacityMetMessage, this.CategoryId)
                {
                    RecurrenceParentId = this.Id,
                    Id = this.Id
                };
        }

        /// <summary>
        /// Saves this event.
        /// </summary>
        /// <param name="revisingUser">The user who is saving this event.</param>
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
        /// Creates an iCal representation of this event.
        /// </summary>
        /// <returns>An iCal representation of this event</returns>
        public string ToICal()
        {
            string rule = this.RecurrenceRule != null ? this.RecurrenceRule.ToString() : null;
            return Util.ICalUtil.Export(this.Overview, this.Location, new Appointment(this.Id, this.EventStart, this.EventEnd, this.Title, rule), true, this.TimeZoneOffset);
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
        public string GetValue([NotNull] string propertyName, string format)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            var subPropertyIndicatorIndex = propertyName.IndexOf('-');
            if (subPropertyIndicatorIndex > -1 && propertyName.Substring(0, subPropertyIndicatorIndex).Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
            {
                return this.Category.GetValue(propertyName.Substring(subPropertyIndicatorIndex + 1), format);
            }

            format = string.IsNullOrEmpty(format) ? null : format;

            switch (propertyName.ToUpperInvariant())
            {
                case "ID":
                    return this.Id.ToString(format, CultureInfo.CurrentCulture);
                case "TITLE":
                    return TemplateEngine.FormatString(this.Title, format ?? "HTML");
                case "OVERVIEW":
                    return TemplateEngine.FormatString(this.Overview, format ?? "HTML");
                case "DESCRIPTION":
                    return TemplateEngine.FormatString(this.Description, format ?? "RAW");
                case "EVENTSTART":
                case "EVENT START":
                    return this.EventStart.ToString(format, CultureInfo.CurrentCulture);
                case "EVENTEND":
                case "EVENT END":
                    return this.EventEnd.ToString(format, CultureInfo.CurrentCulture);
                case "HASCAPACITY":
                case "HAS CAPACITY":
                    return this.Capacity.HasValue.ToString(CultureInfo.InvariantCulture);
                case "CAPACITY":
                    return this.Capacity.HasValue ? this.Capacity.Value.ToString(format, CultureInfo.CurrentCulture) : string.Empty;
                case "ALLOWS REGISTRATION":
                case "ALLOWSREGISTRATION":
                    return this.AllowRegistrations.ToString(CultureInfo.InvariantCulture);
                case "HAS RESPONSES":
                case "HASRESPONSES":
                    return this.HasResponses.ToString(CultureInfo.InvariantCulture);
                case "HAS ATTENDEES":
                case "HASATTENDEES":
                    return this.HasAttendees.ToString(CultureInfo.InvariantCulture);
                case "RESPONSE COUNT":
                case "RESPONSECOUNT":
                    return this.ResponseCount.ToString(format, CultureInfo.InvariantCulture);
                case "ATTENDEE COUNT":
                case "ATTENDEECOUNT":
                    return this.AttendeeCount.ToString(format, CultureInfo.InvariantCulture);
                case "IS FULL":
                case "ISFULL":
                    return (this.AttendeeCount >= this.Capacity).ToString(CultureInfo.InvariantCulture);
                case "LOCATION":
                    return TemplateEngine.FormatString(this.Location, format ?? "HTML");
                case "CATEGORY":
                    return TemplateEngine.FormatString(this.Category.Name, format ?? "HTML");
            }

            return string.Empty;
        }

        /// <summary>
        /// Fills an Event with the data in the specified <paramref name="eventRecord"/>.
        /// </summary>
        /// <param name="eventRecord">A pre-initialized data record that represents an Event instance.</param>
        /// <returns>An instantiated Event object.</returns>
        internal static Event Fill(IDataRecord eventRecord)
        {
            var e = new Event 
            {
                Id = (int)eventRecord["EventId"],
                ModuleId = (int)eventRecord["ModuleId"],
                Title = eventRecord["Title"].ToString(),
                Overview = eventRecord["Overview"].ToString(),
                Description = eventRecord["Description"].ToString(),
                EventStart = (DateTime)eventRecord["EventStart"],
                EventEnd = (DateTime)eventRecord["EventEnd"],
                TimeZoneOffset = new TimeSpan(0, (int)eventRecord["TimeZoneOffset"], 0),
                CreatedBy = (int)eventRecord["CreatedBy"],
                CreationDate = (DateTime)eventRecord["CreationDate"],
                RevisionDate = (DateTime)eventRecord["RevisionDate"],
                Canceled = (bool)eventRecord["Canceled"],
                IsFeatured = (bool)eventRecord["IsFeatured"],
                IsDeleted = (bool)eventRecord["IsDeleted"],
                AllowRegistrations = (bool)eventRecord["AllowRegistrations"],
                Organizer = eventRecord["Organizer"].ToString(),
                OrganizerEmail = eventRecord["OrganizerEmail"].ToString(),
                Location = eventRecord["Location"].ToString(),
                InvitationUrl = eventRecord["InvitationUrl"].ToString(),
                RecapUrl = eventRecord["RecapUrl"].ToString(),
                RecurrenceParentId = eventRecord["RecurrenceParentId"] as int?,
                Capacity = eventRecord["Capacity"] as int?,
                InDaylightTime = (bool)eventRecord["InDaylightTime"],
                CategoryId = (int)eventRecord["CategoryId"]
            };

            var capacityMetMessageColumnIndex = eventRecord.GetOrdinal("CapacityMetMessage");
            e.CapacityMetMessage = eventRecord.IsDBNull(capacityMetMessageColumnIndex) ? null : eventRecord.GetString(capacityMetMessageColumnIndex);

            RecurrenceRule rule;
            if (RecurrenceRule.TryParse(eventRecord["RecurrenceRule"].ToString(), out rule))
            {
                e.RecurrenceRule = rule;
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
                this.Id = dp.ExecuteNonQuery(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spInsertEvent",
                        Utility.CreateIntegerParam("@PortalId", this.PortalId),
                        Utility.CreateIntegerParam("@ModuleId", this.ModuleId),
                        Utility.CreateVarcharParam("@Title", this.Title),
                        Utility.CreateTextParam("@Overview", this.Overview),
                        Utility.CreateTextParam("@Description", this.Description),
                        Utility.CreateDateTimeParam("@EventStart", this.EventStart),
                        Utility.CreateDateTimeParam("@EventEnd", this.EventEnd),
                        Utility.CreateIntegerParam("@TimeZoneOffset", (int)this.TimeZoneOffset.TotalMinutes),
                        Utility.CreateVarcharParam("@Organizer", this.Organizer),
                        Utility.CreateVarcharParam("@OrganizerEmail", this.OrganizerEmail),
                        Utility.CreateVarcharParam("@Location", this.Location),
                        Utility.CreateVarcharParam("@LocationUrl", this.LocationUrl),
                        Utility.CreateVarcharParam("@InvitationUrl", this.InvitationUrl),
                        Utility.CreateVarcharParam("@RecapUrl", this.RecapUrl),
                        Utility.CreateIntegerParam("@RecurrenceParentId", this.RecurrenceParentId),
                        Utility.CreateVarcharParam("@RecurrenceRule", this.RecurrenceRule != null ? this.RecurrenceRule.ToString() : null),
                        Utility.CreateBitParam("@AllowRegistrations", this.AllowRegistrations),
                        Utility.CreateBitParam("@isFeatured", this.IsFeatured),
                        Utility.CreateIntegerParam("@CreatedBy", revisingUser),
                        Utility.CreateDateTimeParam("@FinalRecurringEndDate", this.FinalRecurringEndDate),
                        Utility.CreateIntegerParam("@Capacity", this.Capacity),
                        Utility.CreateBitParam("@InDaylightTime", this.InDaylightTime),
                        Utility.CreateTextParam("@CapacityMetMessage", this.CapacityMetMessage),
                        Utility.CreateBitParam("@IsDeleted", this.IsDeleted),
                        Utility.CreateIntegerParam("@CategoryId", this.CategoryId));
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
                        Utility.CreateIntegerParam("@EventId", this.Id),
                        Utility.CreateVarcharParam("@Title", this.Title),
                        Utility.CreateTextParam("@Overview", this.Overview),
                        Utility.CreateTextParam("@Description", this.Description),
                        Utility.CreateDateTimeParam("@EventStart", this.EventStart),
                        Utility.CreateDateTimeParam("@EventEnd", this.EventEnd),
                        Utility.CreateIntegerParam("@TimeZoneOffset", (int)this.TimeZoneOffset.TotalMinutes),
                        Utility.CreateVarcharParam("@Organizer", this.Organizer),
                        Utility.CreateVarcharParam("@OrganizerEmail", this.OrganizerEmail),
                        Utility.CreateVarcharParam("@Location", this.Location),
                        Utility.CreateVarcharParam("@LocationUrl", this.LocationUrl),
                        Utility.CreateVarcharParam("@InvitationUrl", this.InvitationUrl),
                        Utility.CreateVarcharParam("@RecapUrl", this.RecapUrl),
                        Utility.CreateTextParam("@RecurrenceRule", this.RecurrenceRule != null ? this.RecurrenceRule.ToString() : null),
                        Utility.CreateIntegerParam("@RecurrenceParentId", this.RecurrenceParentId),
                        Utility.CreateBitParam("@AllowRegistrations", this.AllowRegistrations),
                        Utility.CreateBitParam("@Canceled", this.Canceled),
                        Utility.CreateBitParam("@isFeatured", this.IsFeatured),
                        Utility.CreateIntegerParam("@RevisingUser", revisingUser),
                        Utility.CreateDateTimeParam("@FinalRecurringEndDate", this.FinalRecurringEndDate),
                        Utility.CreateIntegerParam("@Capacity", this.Capacity),
                        Utility.CreateBitParam("@InDaylightTime", this.InDaylightTime),
                        Utility.CreateTextParam("@CapacityMetMessage", this.CapacityMetMessage),
                        Utility.CreateBitParam("@IsDeleted", this.IsDeleted),
                        Utility.CreateIntegerParam("@CategoryId", this.CategoryId));
            }
            catch (SystemException de)
            {
                throw new DBException("spUpdateEvent", de);
            }
        }
    }
}
