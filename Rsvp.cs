using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using aspNetEmail.Calendaring;
using Engage.Data;

namespace Engage.Events
{
    public class Rsvp : IEditableObject, INotifyPropertyChanged
    {
        private Rsvp()
        {

        }

        private Rsvp(int portalId, int moduleId, string organizerEmail, string name, string overview, DateTime eventStart, int createdBy)
        {
            _moduleId = moduleId;
            _portalId = portalId;
            _name = name;
            _overview = overview;
            _eventStart = eventStart;
            _createdBy = createdBy;
        }

        #region Static Methods

        public static Rsvp Load(int id)
        {
            IDataProvider dp = DataProvider.Instance;
            Rsvp r = null;

            try
            {
                using (DataSet ds = dp.ExecuteDataset(CommandType.StoredProcedure, dp.NamePrefix + "spGetEvent",
                 Engage.Utility.CreateIntegerParam("@EventId", id)))
                {
                    r = Fill(ds.Tables[0].Rows[0]);
                }
            }
            catch (Exception se)
            {
                throw new DbException("spGetEvents", se);
            }

            return r;
        }

        public static Rsvp Create(int portalId, int moduleId, string organizerEmail, string name, string overview, DateTime eventStart, int createdBy)
        {
            return new Rsvp(portalId, moduleId, organizerEmail, name, overview, eventStart, createdBy);
        }

        internal static Rsvp Fill(DataRow row)
        {
            Rsvp r = new Rsvp();

            r._id = (int) row["EventId"];
            r._moduleId = (int) row["ModuleId"];
            r._name = row["Name"].ToString();
            r._overview = row["OverView"].ToString();
            r._eventStart = (DateTime)row["EventStart"];
            if (!(row["EventEnd"] is DBNull))
            {
                r._eventEnd = (DateTime)row["EventEnd"];
            }
            r._createdBy = (int)row["CreatedBy"];
            //when constructing a collection of events the stored procedure for paging includes a TotalRecords
            //field. When loading a single Event this does not exist.hk
            if (row.Table.Columns.Contains("TotalRecords"))
            {
                r._totalRecords = (int)row["TotalRecords"];
            }
            r._organizer = row["Organizer"].ToString();
            r._organizerEmail = row["OrganizerEmail"].ToString();
            r._location = row["Location"].ToString();

            return r;
        }
        
        #endregion

        #region Methods

        public void Save(int revisingUser)
        {
            if (_id < 0)
            {
                InsertEvent(revisingUser);
            }
            else
            {
                UpdateEvent(revisingUser);
            }
        }

        private void InsertEvent(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                _id = dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spInsertEvent",
                Engage.Utility.CreateIntegerParam("@PortalId", _portalId),
                Engage.Utility.CreateIntegerParam("@ModuleId", _moduleId),
                Engage.Utility.CreateVarcharParam("@Name", _name),
                Engage.Utility.CreateTextParam("@Overview", _overview),
                Engage.Utility.CreateDateTimeParam("@EventStart", _eventStart),
                Engage.Utility.CreateDateTimeParam("@EventEnd", _eventEnd),
                Engage.Utility.CreateVarcharParam("@Organizer", _organizer),
                Engage.Utility.CreateVarcharParam("@OrganizerEmail", _organizerEmail),
                Engage.Utility.CreateVarcharParam("@Location", _location),
                Engage.Utility.CreateVarcharParam("@LocationUrl", _locationUrl),
                Engage.Utility.CreateVarcharParam("@InvitationUrl", _invitationUrl),
                Engage.Utility.CreateVarcharParam("@RecapUrl", _recapUrl),
                Engage.Utility.CreateIntegerParam("@RecurrenceId", _recurrenceId),
                Engage.Utility.CreateBitParam("@CanRsvp", true),
                Engage.Utility.CreateIntegerParam("@CreatedBy", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DbException("spInsertEvent", de);
            }
        }

        private void UpdateEvent(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spUpdateEvent",
                Engage.Utility.CreateIntegerParam("@EventId", _id),
                Engage.Utility.CreateVarcharParam("@Name", _name),
                Engage.Utility.CreateTextParam("@Overview", _overview),
                Engage.Utility.CreateDateTimeParam("@EventStart", _eventStart),
                Engage.Utility.CreateDateTimeParam("@EventEnd", _eventEnd),
                Engage.Utility.CreateVarcharParam("@Organizer", _organizer),
                Engage.Utility.CreateVarcharParam("@OrganizerEmail", _organizerEmail),
                Engage.Utility.CreateVarcharParam("@Location", _location),
                Engage.Utility.CreateVarcharParam("@LocationUrl", _locationUrl),
                Engage.Utility.CreateVarcharParam("@InvitationUrl", _invitationUrl),
                Engage.Utility.CreateVarcharParam("@RecapUrl", _recapUrl),
                Engage.Utility.CreateIntegerParam("@RecurrenceId", _recurrenceId),
                Engage.Utility.CreateBitParam("@CanRsvp", true),
                Engage.Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DbException("spUpdateEvent", de);
            }

        }

        public string ToICal(string attendeeEmail)
        {
            iCalendar ic = GenerateICalendar(attendeeEmail);
            return ic.ToString();
        }

        public void ToiCalAsFile(string pathAndFileName, string attendeeEmail)
        {

            iCalendar ic = GenerateICalendar(attendeeEmail);
            ic.WriteToFile(pathAndFileName, iCalendarType.iCal);
        }

        private iCalendar GenerateICalendar(string attendeeEmail)
        {
            iCalendar ic = new iCalendar();
            ic.OptimizedFormat = OptimizedFormat.Exchange2003;

            //create the organizer
            ic.Event.Organizer.FullName = _organizer;
            ic.Event.Organizer.Email = _organizerEmail;

            //set the timezone - this will need to be based on the current environment timezone.hk
            ic.TimeZone.TimeZoneIndex = TimeZoneHelper.CentralAmericaGMTm0600;
            ic.Type = iCalendarType.iCal;

            //define the event
            ic.Event.Summary.Text = _name;
            ic.Event.Description.Text = _overview;

            //set the location 
            ic.Event.Location.Text = _location;

            //set the dates.
            //ic.Event.DateStart.Date = new DateTime(2008, 10, 31, 14, 0, 0);
            //ic.Event.DateEnd.Date = new DateTime(2008, 10, 31, 15, 0, 0);
            ic.Event.DateStart.Date = _eventStart;
            if (_eventEnd != null)
            {
                ic.Event.DateEnd.Date = (DateTime)_eventEnd;
            }
           
            //mark the time as busy (not available to free-busy searches).
            ic.Event.TimeTransparency.TransparencyType = TransparencyType.Opaque;
            ic.Method = new Method(Method.PublishMethod);

            //set an alarm/reminder
            Alarm a = new DisplayAlarm("This is a reminder of an upcoming event.");
            // repeat the alarm for 10 times (snooze)
            a.Repeat.Count = 10;

            // triggers 30 minutes before the event starts			
            a.Trigger.RelativeTrigger.Negative = true;
            a.Trigger.RelativeTrigger.TimeSpan = new TimeSpan(0, 30, 0);

            // delay period after which the alarm will repeat
            a.DelayPeriod.TimeSpan = new TimeSpan(0, 10, 0);

            //Add the Attendee
            Attendee att1 = new Attendee();
            //att1.FullName = attendeeEmail;
            att1.Email = attendeeEmail;
            att1.ParticipationStatus = ParticipationStatus.ACCEPTED;
            att1.Role = RoleType.REQ_PARTICIPANT;
            //att1.RSVP = true;
            ic.Event.Attendees.Add(att1);

            ic.Event.Classification.ClassificationType = ClassificationType.Private;
            ic.Event.Categories.Add(CategoryType.APPOINTMENT);

            if (IsRecurring)
            {
                //make this a recurring event
                // Day 31 of every two months for 10 months. For some months it will fall on the last day.
                MonthlyRecurrence mr = new MonthlyRecurrence();
                mr.WeekStart = iCalendarDay.Sunday;  // change the default weekstart to sunday
                mr.Interval = 2;
                mr.Occurs = 10;
                mr.DayNumber = 31;
                ic.Event.Recurrence = mr;
            }

            return ic;
        }

        public bool IsRecurring
        {
            get { return false; }
        }

        #endregion
        
        #region Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _id = -1;
        public int Id
        {
            [DebuggerStepThrough]
            get { return _id; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _portalId = -1;
        public int PortalId
        {
            [DebuggerStepThrough]
            get { return _portalId; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _moduleId = -1;
        public int ModuleId
        {
            [DebuggerStepThrough]
            get { return _moduleId; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name = string.Empty;
        public string Name
        {
            [DebuggerStepThrough]
            get { return _name; }
            [DebuggerStepThrough]
            set { _name = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _location = string.Empty;
        public string Location
        {
            [DebuggerStepThrough]
            get { return _location; }
            [DebuggerStepThrough]
            set { _location = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _locationUrl = string.Empty;
        public string LocationUrl
        {
            [DebuggerStepThrough]
            get { return _locationUrl; }
            [DebuggerStepThrough]
            set { _locationUrl = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _invitationUrl = string.Empty;
        public string InvitationUrl
        {
            [DebuggerStepThrough]
            get { return _invitationUrl; }
            [DebuggerStepThrough]
            set { _invitationUrl = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _recapUrl = string.Empty;
        public string RecapUrl
        {
            [DebuggerStepThrough]
            get { return _recapUrl; }
            [DebuggerStepThrough]
            set { _recapUrl = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _overview = string.Empty;
        public string Overview
        {
            [DebuggerStepThrough]
            get { return _overview; }
            [DebuggerStepThrough]
            set { _overview = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime _eventStart;
        public DateTime EventStart
        {
            [DebuggerStepThrough]
            get { return _eventStart; }
            [DebuggerStepThrough]
            set { _eventStart = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? _eventEnd;
        public DateTime? EventEnd
        {
            [DebuggerStepThrough]
            get { return _eventEnd; }
            [DebuggerStepThrough]
            set { _eventEnd = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _createdBy = -1;
        public int CreatedBy
        {
            [DebuggerStepThrough]
            get { return _createdBy; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _recurrenceId;
        public int RecurrenceId
        {
            [DebuggerStepThrough]
            get { return _recurrenceId; }
            [DebuggerStepThrough]
            set { _recurrenceId = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _totalRecords = 0;
        public int TotalRecords
        {
            [DebuggerStepThrough]
            get { return _totalRecords; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _organizer = string.Empty;
        public string Organizer
        {
            [DebuggerStepThrough]
            get { return _organizer; }
            [DebuggerStepThrough]
            set { _organizer = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _organizerEmail = string.Empty;
        public string OrganizerEmail
        {
            [DebuggerStepThrough]
            get {return _organizerEmail;}
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IEditableObject Members

        public void BeginEdit()
        {
        }

        public void CancelEdit()
        {
        }

        public void EndEdit()
        {
        }

        #endregion
    }
}
