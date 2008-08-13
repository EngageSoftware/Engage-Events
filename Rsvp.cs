// <copyright file="Rsvp.cs" company="Engage Software">
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
    /// The status of an RSVP; that is, how the invitee responded to the invitation.
    /// </summary>
    public enum RsvpStatus
    {
        /// <summary>
        /// The invitee did not respond to an invitation
        /// </summary>
        NoResponse = 0,

        /// <summary>
        /// The invitee indicated that they will attend
        /// </summary>
        Attending = 1,

        /// <summary>
        /// The invitee indicated that they will not attend
        /// </summary>
        NotAttending = 2
    }

    /// <summary>
    /// An indication of how a particular individual responded to an invitation to an event.
    /// </summary>
    public class Rsvp : IEditableObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Backing field for <see cref="CreatedBy"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int createdBy = -1;

        /// <summary>
        /// Backing field for <see cref="CreationDate"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime creationDate;

        /// <summary>
        /// Backing field for <see cref="Email"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string email = string.Empty;

        /// <summary>
        /// Backing field for <see cref="EventId"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int eventId = -1;

        /// <summary>
        /// Backing field for <see cref="FirstName"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string firstName = string.Empty;

        /// <summary>
        /// Backing field for <see cref="Id"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int id = -1;

        /// <summary>
        /// Backing field for <see cref="LastName"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string lastName = string.Empty;

        /// <summary>
        /// Backing field for <see cref="Status"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RsvpStatus status = RsvpStatus.NoResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rsvp"/> class.
        /// </summary>
        private Rsvp()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rsvp"/> class.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        private Rsvp(int eventId, string firstName, string lastName, string email)
        {
            this.eventId = eventId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Gets the id of this RSVP.
        /// </summary>
        /// <value>The RSVP id.</value>
        public int Id
        {
            [DebuggerStepThrough]
            get { return this.id; }
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
        /// Gets the full name of the invitee.
        /// </summary>
        /// <value>The invitee's full name.</value>
        public string Name
        {
            get { return this.lastName + ", " + this.firstName; }
        }

        /// <summary>
        /// Gets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company
        {
            // Need to get this from somewhere without referencing the DotNetNuke.dll (userinfo class).
            // May need to make part of engage_spGetRsvps to try and look this up from profile?? This 
            // would still create a dependency though. hk
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            [DebuggerStepThrough]
            get { return this.firstName; }
            [DebuggerStepThrough]
            set { this.firstName = value; }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            [DebuggerStepThrough]
            get { return this.lastName; }
            [DebuggerStepThrough]
            set { this.lastName = value; }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            [DebuggerStepThrough]
            get { return this.email; }
            [DebuggerStepThrough]
            set { this.email = value; }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public RsvpStatus Status
        {
            [DebuggerStepThrough]
            get { return this.status; }
            [DebuggerStepThrough]
            set { this.status = value; }
        }

        /// <summary>
        /// Gets the user this RSVP was created by.
        /// </summary>
        /// <value>The user this RSVP was created by.</value>
        public int CreatedBy
        {
            [DebuggerStepThrough]
            get { return this.createdBy; }
        }

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        public DateTime CreationDate
        {
            [DebuggerStepThrough]
            get { return this.creationDate; }
        }

        /// <summary>
        /// Loads the specified RSVP by the invitee's email address.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="email">The email.</param>
        /// <returns>The RSVP of the given invitee.</returns>
        /// <exception cref="DBException">If an error occurs while retrieving the record from the database</exception>
        public static Rsvp Load(int eventId, string email)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (IDataReader dataReader = dp.ExecuteReader(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spGetRsvpByEmail",
                        Utility.CreateIntegerParam("@EventId", eventId),
                        Utility.CreateVarcharParam("@Email", email, 100)))
                {
                    if (dataReader.Read())
                    {
                        return Fill(dataReader);
                    }
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetRsvpByEmail", se);
            }

            return null;
        }

        /// <summary>
        /// Loads the specified RSVP.
        /// </summary>
        /// <param name="id">The RSVP id.</param>
        /// <returns>The specified RSVP</returns>
        /// <exception cref="DBException">If an error occurs while loading this record</exception>
        public static Rsvp Load(int id)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                using (IDataReader dataReader = dp.ExecuteReader(
                    CommandType.StoredProcedure, 
                    dp.NamePrefix + "spGetRsvp", 
                    Utility.CreateIntegerParam("@EventId", id)))
                {
                    if (dataReader.Read())
                    {
                        return Fill(dataReader);
                    }
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetEvents", se);
            }

            return null;
        }

        /// <summary>
        /// Creates a new RSVP instance.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <returns>The newly created RSVP instance</returns>
        public static Rsvp Create(int eventId, string firstName, string lastName, string email)
        {
            return new Rsvp(eventId, firstName, lastName, email);
        }

        /// <summary>
        /// Gets the status of an RSVP.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="email">The email.</param>
        /// <returns>The status of the specified RSVP</returns>
        /// <exception cref="DBException">If an error occurs while retrieving this record</exception>
        public static RsvpStatus GetRsvpStatus(int eventId, string email)
        {
            IDataProvider dp = DataProvider.Instance;
            RsvpStatus status = RsvpStatus.NoResponse;

            try
            {
                using (IDataReader dr = dp.ExecuteReader(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spGetRsvpByEmail",
                        Utility.CreateIntegerParam("@EventId", eventId),
                        Utility.CreateVarcharParam("@Email", email)))
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

        /// <summary>
        /// Saves this RSVP.
        /// </summary>
        /// <param name="revisingUser">The revising user.</param>
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
        /// Fills an RSVP from the provided data record.
        /// </summary>
        /// <param name="dataRecord">A data record representing an RSVP instance.</param>
        /// <returns>The requested RSVP instance</returns>
        internal static Rsvp Fill(IDataRecord dataRecord)
        {
            Rsvp rsvp = new Rsvp();

            rsvp.id = ((int)dataRecord["RsvpId"]);
            rsvp.eventId = ((int)dataRecord["EventId"]);
            rsvp.lastName = dataRecord["LastName"].ToString();
            rsvp.firstName = dataRecord["FirstName"].ToString();
            rsvp.email = dataRecord["Email"].ToString();
            rsvp.status = ((RsvpStatus)Enum.Parse(typeof(RsvpStatus), dataRecord["Status"].ToString()));
            rsvp.createdBy = ((int)dataRecord["CreatedBy"]);
            rsvp.creationDate = ((DateTime)dataRecord["CreationDate"]);

            return rsvp;
        }

        /// <summary>
        /// Inserts this RSVP into the database.
        /// </summary>
        /// <param name="revisingUser">The revising user.</param>
        /// /// <exception cref="DBException">If an error occurs while inserting this record</exception>
        private void Insert(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                this.id = dp.ExecuteNonQuery(
                    CommandType.StoredProcedure,
                    dp.NamePrefix + "spInsertRsvp",
                    Utility.CreateIntegerParam("@EventId", this.eventId),
                    Utility.CreateVarcharParam("@FirstName", this.firstName),
                    Utility.CreateVarcharParam("@LastName", this.lastName),
                    Utility.CreateVarcharParam("@Email", this.email),
                    Utility.CreateVarcharParam("@Status", this.status.ToString()),
                    Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spInsertRsvp", de);
            }
        }

        /// <summary>
        /// Updates this RSVP record.
        /// </summary>
        /// <param name="revisingUser">The revising user.</param>
        /// <exception cref="DBException">If an error occurs while updating this record</exception>
        private void Update(int revisingUser)
        {
            IDataProvider dp = DataProvider.Instance;

            try
            {
                dp.ExecuteNonQuery(
                    CommandType.StoredProcedure,
                    dp.NamePrefix + "spUpdateRsvp",
                    Utility.CreateIntegerParam("@RsvpId", this.id),
                    Utility.CreateVarcharParam("@FirstName", this.firstName),
                    Utility.CreateVarcharParam("@LastName", this.lastName),
                    Utility.CreateVarcharParam("@Email", this.email),
                    Utility.CreateVarcharParam("@Status", this.status.ToString()),
                    Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spUpdateRsvp", de);
            }
        }
    }
}