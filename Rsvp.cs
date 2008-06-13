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
    public enum RsvpStatus
    {
        NoResponse = 0,
        Attending = 1,
        NotAttending =2
    }

    public class Rsvp : IEditableObject, INotifyPropertyChanged
    {

        private Rsvp()
        {

        }

        private Rsvp(int eventId, string firstName, string lastName, string email)
        {
            _eventId = eventId;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
        }

        #region Static Methods

        public static Rsvp Load(int eventId, string email)
        {
            return new Rsvp();
        }

        public static Rsvp Load(int id)
        {
            IDataProvider dp = DataProvider.Instance;
            Rsvp r = null;

            try
            {
                using (DataSet ds = dp.ExecuteDataset(CommandType.StoredProcedure, dp.NamePrefix + "spGetRsvp",
                 Engage.Utility.CreateIntegerParam("@EventId", id)))
                {
                    r = Fill(ds.Tables[0].Rows[0]);
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetEvents", se);
            }

            return r;
        }

        public static Rsvp Create(int eventId, string firstName, string lastName, string email)
        {
            return new Rsvp(eventId, firstName, lastName, email);
        }

        public static RsvpStatus GetRsvpStatus(int eventId, string email)
        {
            IDataProvider dp = DataProvider.Instance;
            RsvpStatus status = RsvpStatus.NoResponse;

            try
            {
                using (IDataReader dr = dp.ExecuteReader(CommandType.StoredProcedure, dp.NamePrefix + "spGetRsvpByEmail",
                 Engage.Utility.CreateIntegerParam("@EventId", eventId),
                 Engage.Utility.CreateVarcharParam("@Email", email)))
                {
                    if (dr.Read())
                    {
                        status = (RsvpStatus)Enum.Parse(typeof(RsvpStatus), dr["Status"].ToString());
                    }
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetRsvpByEmail", se);
            }

            return status;

        }

        internal static Rsvp Fill(DataRow row)
        {
            Rsvp r = new Rsvp();

            r._id = (int)row["RsvpId"];
            r._eventId = (int)row["EventId"];
            r._lastName = row["LastName"].ToString();
            r._firstName = row["FirstName"].ToString();
            r._email = row["Email"].ToString();
            r._status = (RsvpStatus) Enum.Parse(typeof(RsvpStatus),  row["Status"].ToString());
            r._createdBy = (int)row["CreatedBy"];
            r._creationDate = (DateTime)row["CreationDate"];
            //when constructing a collection of events the stored procedure for paging includes a TotalRecords
            //field. When loading a single Event this does not exist.hk
            if (row.Table.Columns.Contains("TotalRecords"))
            {
                r._totalRecords = (int)row["TotalRecords"];
            }

            return r;
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
                _id = dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spInsertRsvp",
                Engage.Utility.CreateIntegerParam("@EventId", _eventId),
                Engage.Utility.CreateVarcharParam("@FirstName", _firstName),
                Engage.Utility.CreateVarcharParam("@LastName", _lastName),
                Engage.Utility.CreateVarcharParam("@Email", _email),
                Engage.Utility.CreateVarcharParam("@Status", _status.ToString()),
                Engage.Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spInsertEvent", de);
            }
        }

        private void Update(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(CommandType.StoredProcedure, dp.NamePrefix + "spUpdateRsvp",
                    Engage.Utility.CreateIntegerParam("@RsvpId", _id),
                    Engage.Utility.CreateVarcharParam("@FirstName", _firstName),
                    Engage.Utility.CreateVarcharParam("@LastName", _lastName),
                    Engage.Utility.CreateVarcharParam("@Email", _email),
                    Engage.Utility.CreateVarcharParam("@Status", _status.ToString()),
                    Engage.Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spUpdateEvent", de);
            }

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
        private int _eventId = -1;
        public int EventId
        {
            [DebuggerStepThrough]
            get { return _eventId; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Name
        {
            [DebuggerStepThrough]
            get { return _lastName +  ", " + _firstName; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Company
        {
            [DebuggerStepThrough]
            //Need to get this from somewhere without referencing the DotNetNuke.dll (userinfo class).
            //May need to make part of engage_spGetRsvps to try and look this up from profile?? This 
            //would still create a dependency though. hk
            get { return ""; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _firstName = string.Empty;
        public string FirstName
        {
            [DebuggerStepThrough]
            get { return _firstName; }
            [DebuggerStepThrough]
            set { _firstName = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _lastName = string.Empty;
        public string LastName
        {
            [DebuggerStepThrough]
            get { return _lastName; }
            [DebuggerStepThrough]
            set { _lastName = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _email = string.Empty;
        public string Email
        {
            [DebuggerStepThrough]
            get { return _email; }
            [DebuggerStepThrough]
            set { _email = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RsvpStatus _status = RsvpStatus.NoResponse;
        public RsvpStatus Status
        {
            [DebuggerStepThrough]
            get { return _status; }
            [DebuggerStepThrough]
            set { _status = value; }
        }
           
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _createdBy = -1;
        public int CreatedBy
        {
            [DebuggerStepThrough]
            get { return _createdBy; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime _creationDate;
        public DateTime CreationDate
        {
            [DebuggerStepThrough]
            get { return _creationDate; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _totalRecords = 0;
        public int TotalRecords
        {
            [DebuggerStepThrough]
            get { return _totalRecords; }
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
