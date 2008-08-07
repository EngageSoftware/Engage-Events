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
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;
    using aspNetEmail;
    using aspNetEmail.Calendaring;
    using Telerik.Web.UI;
    using Data;

    /// <summary>
    /// An event, with a title, description, location, and start and end date.
    /// </summary>
    [XmlRoot(ElementName = "event", IsNullable = false)]
    public class Event : IEditableObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Backing field for <see cref="Cancelled"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool cancelled;

        /// <summary>
        /// Backing field for <see cref="IsFeatured"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isFeatured;

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
        /// Indicates whether the license for this assembly has yet been loaded.
        /// </summary>
        private bool aspnetEmailLicenseLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
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
        /// <param name="overview">The overview or description of the event.</param>
        /// <param name="eventStart">When the event starts.</param>
        private Event(int portalId, int moduleId, string organizerEmail, string title, string overview, DateTime eventStart)
        {
            this.moduleId = moduleId;
            this.portalId = portalId;
            this.title = title;
            this.overview = overview;
            this.eventStart = eventStart;
            this.organizerEmail = organizerEmail ?? string.Empty;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance is recurring.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is recurring; otherwise, <c>false</c>.
        /// </value>
        public bool IsRecurring
        {
            get { return this.RecurrenceRule != null; }
        }

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
        /// Gets <see cref="EventStart"/> with "short" formatting (M.dd.yyyy).
        /// </summary>
        /// <value>The event start formatted "short".</value>
        public string EventStartFormatted
        {
            [DebuggerStepThrough]
            get { return this.eventStart.ToString("M.dd.yyyy"); }
        }

        /// <summary>
        /// Gets <see cref="EventStart"/> with "long" formatting (dddd, MMMM d, yyyy, h:mm tt).
        /// </summary>
        /// <value>The event start formatted "long".</value>
        public string EventStartLongFormatted
        {
            [DebuggerStepThrough]
            get { return this.eventStart.ToString("dddd, MMMM d, yyyy, h:mm tt"); }
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
        /// Gets the ID of the user who created this event.
        /// </summary>
        /// <value>The ID of the user that created this event.</value>
        public int CreatedBy
        {
            [DebuggerStepThrough]
            get { return this.createdBy; }
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
        /// Gets or sets a value indicating whether this <see cref="Event"/> is cancelled.
        /// </summary>
        /// <value><c>true</c> if cancelled; otherwise, <c>false</c>.</value>
        public bool Cancelled
        {
            [DebuggerStepThrough]
            get { return this.cancelled; }
            [DebuggerStepThrough]
            set { this.cancelled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is featured.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is featured; otherwise, <c>false</c>.
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
        /// <param name="eventStart">The event's start date and time.</param>
        /// <returns>A new event object.</returns>
        public static Event Create(int portalId, int moduleId, string organizerEmail, string title, string overview, DateTime eventStart)
        {
            return new Event(portalId, moduleId, organizerEmail, title, overview, eventStart);
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
        /// <param name="attendeeEmail">The attendee email.</param>
        /// <returns>An iCal representation of this event</returns>
        public string ToICal(string attendeeEmail)
        {
            iCalendar ic = this.GenerateICalendar(attendeeEmail);
            return ic.ToString();
        }

        /// <summary>
        /// Creates an iCal representation of this event, then saves it in a file.
        /// </summary>
        /// <param name="pathAndFileName">{Path to and name of the file to create.</param>
        /// <param name="attendeeEmail">The attendee email.</param>
        public void ToiCalAsFile(string pathAndFileName, string attendeeEmail)
        {
            iCalendar ic = this.GenerateICalendar(attendeeEmail);
            ic.WriteToFile(pathAndFileName, iCalendarType.iCal);
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
            e.createdBy = (int)eventRecord["CreatedBy"];
            e.cancelled = (bool)eventRecord["Cancelled"];
            e.isFeatured = (bool)eventRecord["IsFeatured"];
            e.organizer = eventRecord["Organizer"].ToString();
            e.organizerEmail = eventRecord["OrganizerEmail"].ToString();
            e.location = eventRecord["Location"].ToString();
            e.invitationUrl = eventRecord["InvitationUrl"].ToString();
            e.recapUrl = eventRecord["RecapUrl"].ToString();
            if (!(eventRecord["RecurrenceParentId"] is DBNull))
            {
                e.recurrenceParentId = (int)eventRecord["RecurrenceParentId"];
            }

            RecurrenceRule rule;
            if (RecurrenceRule.TryParse(eventRecord["RecurrenceRule"].ToString(), out rule))
            {
                e.recurrenceRule = rule;
            }
            
            return e;
        }

        /// <summary>
        /// Gets a string representation of the embedded license for the aspnetEmail component.
        /// </summary>
        /// <returns>A string representation of the embedded license for the aspnetEmail component</returns>
        private static string GetLicenseString()
        {
            // embedded resources are embedded using the following convention
            // namespace.foldername.subfolder.subfolder.subfolder.filename etc...
            // in our case, the license is embedded as
            const string resourceLocation = "Engage.Events.aspnetemail.xml.lic";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceLocation))
            {
                if (stream != null)
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }

            return null;
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
                    Utility.CreateVarcharParam("@Organizer", this.organizer),
                    Utility.CreateVarcharParam("@OrganizerEmail", this.organizerEmail),
                    Utility.CreateVarcharParam("@Location", this.location),
                    Utility.CreateVarcharParam("@LocationUrl", this.locationUrl),
                    Utility.CreateVarcharParam("@InvitationUrl", this.invitationUrl),
                    Utility.CreateVarcharParam("@RecapUrl", this.recapUrl),
                    Utility.CreateTextParam("@RecurrenceRule", this.recurrenceRule != null ? this.recurrenceRule.ToString() : null),
                    Utility.CreateBitParam("@CanRsvp", true),
                    Utility.CreateBitParam("@isFeatured", this.isFeatured),
                    Utility.CreateIntegerParam("@CreatedBy", revisingUser));
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
                    Utility.CreateVarcharParam("@Organizer", this.organizer),
                    Utility.CreateVarcharParam("@OrganizerEmail", this.organizerEmail),
                    Utility.CreateVarcharParam("@Location", this.location),
                    Utility.CreateVarcharParam("@LocationUrl", this.locationUrl),
                    Utility.CreateVarcharParam("@InvitationUrl", this.invitationUrl),
                    Utility.CreateVarcharParam("@RecapUrl", this.recapUrl),
                    Utility.CreateTextParam("@RecurrenceRule", this.recurrenceRule != null ? this.recurrenceRule.ToString() : null),
                    Utility.CreateIntegerParam("@RecurrenceParentId", this.recurrenceParentId),
                    Utility.CreateBitParam("@CanRsvp", true),
                    Utility.CreateBitParam("@Cancelled", this.cancelled),
                    Utility.CreateBitParam("@isFeatured", this.isFeatured),
                    Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spUpdateEvent", de);
            }
        }

        /// <summary>
        /// Generates an iCalendar representation of this event.
        /// </summary>
        /// <param name="attendeeEmail">The attendee email.</param>
        /// <returns>A strongly-typed iCalendar representation of this event.</returns>
        private iCalendar GenerateICalendar(string attendeeEmail)
        {
            this.LoadAspnetEmailLicense();

            iCalendar ic = new iCalendar();
            
            ic.OptimizedFormat = OptimizedFormat.Exchange2003;

            // create the organizer
            ic.Event.Organizer.FullName = this.organizer;
            ic.Event.Organizer.Email = this.organizerEmail;

            // set the timezone - this will need to be based on the current environment timezone.hk
            ic.TimeZone.TimeZoneIndex = TimeZoneHelper.CentralAmericaGMTm0600;
            ic.Type = iCalendarType.iCal;

            // define the event
            ic.Event.Summary.Text = this.title;
            ic.Event.Description.Text = this.overview;

            // set the location 
            ic.Event.Location.Text = this.location;

            // set the dates.
            ic.Event.DateStart.Date = this.eventStart;
            ic.Event.DateEnd.Date = this.eventEnd;

            // mark the time as busy (not available to free-busy searches).
            ic.Event.TimeTransparency.TransparencyType = TransparencyType.Opaque;
            ////ic.Method = new Method(Method.PublishMethod);
            ic.Method = new Method(Method.RequestMethod);

            // set an alarm/reminder
            Alarm a = new DisplayAlarm("This is a reminder of an upcoming event.");

            // repeat the alarm for 10 times (snooze)
            a.Repeat.Count = 10;

            // triggers 30 minutes before the event starts
            a.Trigger.RelativeTrigger.Negative = true;
            a.Trigger.RelativeTrigger.TimeSpan = new TimeSpan(0, 30, 0);

            // delay period after which the alarm will repeat
            a.DelayPeriod.TimeSpan = new TimeSpan(0, 10, 0);

            // Add the Attendee
            Attendee att1 = new Attendee();
            ////att1.FullName = attendeeEmail;
            att1.Email = attendeeEmail;
            att1.ParticipationStatus = ParticipationStatus.NEEDSACTION;
            att1.Role = RoleType.REQ_PARTICIPANT;
            ////att1.RSVP = true;
            ic.Event.Attendees.Add(att1);

            ic.Event.Classification.ClassificationType = ClassificationType.Private;
            ic.Event.Categories.Add(CategoryType.APPOINTMENT);

            if (this.recurrenceRule != null)
            {
                // make this a recurring event - NOT YET IMPLEMENTED!!!!!hk
                // Day 31 of every two months for 10 months. For some months it will fall on the last day.
                MonthlyRecurrence mr = new MonthlyRecurrence();

                mr.WeekStart = iCalendarDay.Sunday; // change the default weekstart to sunday
                mr.Interval = 2;
                mr.Occurs = 10;
                mr.DayNumber = 31;
                ic.Event.Recurrence = mr;
            }

            ////ic.WriteToFile(@"c:\inetpub\wwwroot\aspnetgenerated.ics");

            return ic;
        }

        /// <summary>
        /// Loads the license for the aspnetEmail component.
        /// </summary>
        private void LoadAspnetEmailLicense()
        {
            // we only need to load the license once. 
            // aspNetEmail will cache it internally.
            if (!this.aspnetEmailLicenseLoaded)
            {
                // load the license, or an exception will be thrown about "unable to locate license"
                string contents = GetLicenseString();
                EmailMessage.LoadLicenseString(contents);

                // set the flag that the license has already been loaded
                this.aspnetEmailLicenseLoaded = true;
            }
        }
    }
}
