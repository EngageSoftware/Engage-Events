using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text;
using aspNetEmail.Calendaring;
using Engage.Data;

namespace Engage.Events
{
    public class Event : IEditableObject, INotifyPropertyChanged
    {
        private Event()
        {
        }

        private Event(int portalId, int moduleId,  string organizerEmail, string title, string overview, DateTime eventStart)
        {
            _moduleId = moduleId;
            _portalId = portalId;
            _title = title;
            _overview = overview;
            _eventStart = eventStart;
        }

        #region Static Methods

        public static Event Load(int id)
        {
            IDataProvider dp = DataProvider.Instance;
            Event e = null;

            try
            {
                using (DataSet ds = dp.ExecuteDataset(CommandType.StoredProcedure, dp.NamePrefix + "spGetEvent",
                 Engage.Utility.CreateIntegerParam("@EventId", id)))
                {
                    e = Fill(ds.Tables[0].Rows[0]);
                }
            }
            catch (Exception se)
            {
                throw new DbException("spGetEvents", se);
            }

            return e;
        }

        public static Event Create(int portalId, int moduleId, string organizerEmail, string title, string overview, DateTime eventStart)
        {
            return new Event(portalId, moduleId, organizerEmail, title, overview, eventStart);
        }

        public static void Delete(int eventId)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spDeleteEvent",
                 Engage.Utility.CreateIntegerParam("@EventId", eventId));
            }
            catch (Exception se)
            {
                throw new DbException("spDeleteEvent", se);
            }

        }

        internal static Event Fill(DataRow row)
        {
            Event e = new Event();

            e._id = (int) row["EventId"];
            e._moduleId = (int) row["ModuleId"];
            e._title = row["Title"].ToString();
            e._overview = row["OverView"].ToString();
            e._eventStart = (DateTime)row["EventStart"];
            if (!(row["EventEnd"] is DBNull))
            {
                e._eventEnd = (DateTime)row["EventEnd"];
            }
            e._createdBy = (int)row["CreatedBy"];
            //when constructing a collection of events the stored procedure for paging includes a TotalRecords
            //field. When loading a single Event this does not exist.hk
            if (row.Table.Columns.Contains("TotalRecords"))
            {
                e._totalRecords = (int)row["TotalRecords"];
            }
            e._cancelled = (bool)row["Cancelled"];
            e._organizer = row["Organizer"].ToString();
            e._organizerEmail = row["OrganizerEmail"].ToString();
            e._location = row["Location"].ToString();
            e._invitationUrl = row["InvitationUrl"].ToString();
            e._recapUrl = row["RecapUrl"].ToString();

            return e;
        }
        
        #endregion

        #region Methods

        public void Save(int revisingUser)
        {
            if (_id < 0)
            {
                Insert(revisingUser);
            }
            else
            {
                Update(revisingUser);
            }
        }

        private void Insert(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                _id = dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spInsertEvent",
                Engage.Utility.CreateIntegerParam("@PortalId", _portalId),
                Engage.Utility.CreateIntegerParam("@ModuleId", _moduleId),
                Engage.Utility.CreateVarcharParam("@Title", _title),
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

        private void Update(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spUpdateEvent",
                Engage.Utility.CreateIntegerParam("@EventId", _id),
                Engage.Utility.CreateVarcharParam("@Title", _title),
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
            LoadEmailLicense();

            iCalendar ic = new iCalendar();
            ic.OptimizedFormat = OptimizedFormat.Exchange2003;

            //create the organizer
            ic.Event.Organizer.FullName = _organizer;
            ic.Event.Organizer.Email = _organizerEmail;

            //set the timezone - this will need to be based on the current environment timezone.hk
            ic.TimeZone.TimeZoneIndex = TimeZoneHelper.CentralAmericaGMTm0600;
            ic.Type = iCalendarType.iCal;

            //define the event
            ic.Event.Summary.Text = _title;
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
            //ic.Method = new Method(Method.PublishMethod);
            ic.Method = new Method(Method.RequestMethod);

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
                //make this a recurring event - NOT YET IMPLEMENTED!!!!!hk
                // Day 31 of every two months for 10 months. For some months it will fall on the last day.
                MonthlyRecurrence mr = new MonthlyRecurrence();
                mr.WeekStart = iCalendarDay.Sunday;  // change the default weekstart to sunday
                mr.Interval = 2;
                mr.Occurs = 10;
                mr.DayNumber = 31;
                ic.Event.Recurrence = mr;
            }

            //ic.WriteToFile(@"c:\inetpub\wwwroot\aspnetgenerated.ics");

            return ic;
        }

        public bool IsRecurring
        {
            get { return false; }
        }

        #endregion

        //private static string AspNetEmailLicense
        //{
        //    get
        //    {
        //        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        //        doc.
        //        StringBuilder key = new StringBuilder(256);

        //        key.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        //        key.Append("<license version=\"1.1\">");
        //        key.Append("<Customer>");
        //        key.Append("<name>advancedintellect-4511 Henry Kenuam   (Kenuam, Henry) 11819 Manchester Rd. St. Louis, MO	 63131 US United States 314.966.4000 num:1</name>");
        //        key.Append("</Customer> <products>   <Product>  <name>aspNetEmail</name><version>3</version><licensetype>server</licensetype> ");
        //        key.Append("<expires>-1</expires> ");
        //        key.Append("</Product></products><LicenseInformation><Version>1.1</Version><H1>0</H1><H2>MKQAnFN/lhWGUILWW3NrHg==</H2> ");
        //        key.Append("</LicenseInformation><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\">");
        //        key.Append("<SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" />");
        //        key.Append("<SignatureMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#rsa-sha1\" />");
        //        key.Append("<Reference URI=\"\"><Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" />");
        //        key.Append("</Transforms><DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" /> ");
        //        key.Append("<DigestValue>5t6PLBPCEjBdaexBExBz71Wxo0A=</DigestValue>");
        //        key.Append("</Reference></SignedInfo><SignatureValue>HAbLpoccIVxp7dU+4Cb6cqUejItXGNr3RAMH/jMkTH5wPQ/kSEmg1JsoxMd8R8ILSdTB1c/tHn4NUtESkF3BVNXLYO7Jn/Ar5zP5TQo8xeNQYh7m7v0MZLIB3ulBU6O24nqOWFMesn9KRGAdRszexACTI1xhV/Rg/2Wpq7Jq8mg=</SignatureValue> ");
        //        key.Append("</Signature></license>");

        //        return key.ToString();
        //    }
        //}

        #region helper methods to load an email message object
        private bool LicenseLoaded = false;
        private void LoadEmailLicense()
        {
            //we only need to load the license once. 
            //aspNetEmail will cache it internally.

            if (!LicenseLoaded)
            {
                //load the license, or an exception will be thrown about "unable to locate license"
                string contents = GetLicenseString();
                aspNetEmail.EmailMessage.LoadLicenseString(contents);

                //set the flag that the license has already been loaded
                LicenseLoaded = true;

            }
        }

        private string GetLicenseString()
        {
            //embedded resources are embedded using the following convention
            //namespace.foldername.subfolder.subfolder.subfolder.filename etc...
            //in our case, the license is embedded as

            string resourceLocation = "Engage.Events.aspnetemail.xml.lic"; //namespace.foldername.filename
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceLocation);

            StreamReader sr = new StreamReader(stream);
            string contents = sr.ReadToEnd();
            sr.Close();

            return contents;
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
        private string _title = string.Empty;
        public string Title
        {
            [DebuggerStepThrough]
            get { return _title; }
            [DebuggerStepThrough]
            set { _title = value; }
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

        public string EventStartFormatted
        {
            [DebuggerStepThrough]
            get { return _eventStart.ToString("M.dd.yyyy"); }
        }

        public string EventStartLongFormatted
        {
            [DebuggerStepThrough]
            get { return _eventStart.ToString("dddd, MMMM d, yyyy, h:mm tt"); }
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
        private bool _cancelled;
        public bool Cancelled
        {
            [DebuggerStepThrough]
            get { return _cancelled; }
            [DebuggerStepThrough]
            set { _cancelled = value; }
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
