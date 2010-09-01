// <copyright file="Response.cs" company="Engage Software">
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
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using Data;

    /// <summary>
    /// An indication of how a particular individual responded to an invitation to an event.
    /// </summary>
    public class Response : IEditableObject, INotifyPropertyChanged
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
        /// Backing field for <see cref="EventStart"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime eventStart;

        /// <summary>
        /// Backing field for <see cref="Status"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ResponseStatus status = ResponseStatus.NoResponse;

        /// <summary>
        /// Prevents a default instance of the Response class from being created.
        /// </summary>
        private Response()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date for which this response was made.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        private Response(int eventId, DateTime eventStart, string firstName, string lastName, string email)
        {
            this.eventId = eventId;
            this.eventStart = eventStart;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the user this Response was created by.
        /// </summary>
        /// <value>The user this Response was created by.</value>
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
        /// Gets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company
        {
            // Need to get this from somewhere without referencing the DotNetNuke.dll (userinfo class).
            // May need to make part of engage_spGetResponses to try and look this up from profile?? This 
            // would still create a dependency though. hk
            get { return string.Empty; }
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
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        public int EventId
        {
            [DebuggerStepThrough]
            get { return this.eventId; }
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
        /// Gets the id of this Response.
        /// </summary>
        /// <value>The Response id.</value>
        public int Id
        {
            [DebuggerStepThrough]
            get { return this.id; }
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
        /// Gets the full name of the invitee.
        /// </summary>
        /// <value>The invitee's full name.</value>
        public string Name
        {
            get { return this.lastName + ", " + this.firstName; }
        }

        /// <summary>
        /// Gets the date and time when the event for which this response was made occurs.
        /// </summary>
        /// <value>The date and time when the event for which this response was made occurs.</value>
        public DateTime EventStart
        {
            [DebuggerStepThrough]
            get { return this.eventStart; }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ResponseStatus Status
        {
            [DebuggerStepThrough]
            get { return this.status; }
            [DebuggerStepThrough]
            set { this.status = value; }
        }

        /// <summary>
        /// Loads the specified Response by the invitee's email address.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date for which this response was made.</param>
        /// <param name="email">The email.</param>
        /// <returns>The Response of the given invitee.</returns>
        /// <exception cref="DBException">If an error occurs while retrieving the record from the database</exception>
        public static Response Load(int eventId, DateTime eventStart, string email)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (IDataReader dataReader = dp.ExecuteReader(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spGetResponseByEmail",
                        Utility.CreateIntegerParam("@EventId", eventId),
                        Utility.CreateDateTimeParam("@EventStart", eventStart),
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
                throw new DBException("spGetResponseByEmail", se);
            }

            return null;
        }

        /// <summary>
        /// Creates a new Response instance.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date for which this response was made.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <returns>The newly created Response instance</returns>
        public static Response Create(int eventId, DateTime eventStart, string firstName, string lastName, string email)
        {
            return new Response(eventId, eventStart, firstName, lastName, email);
        }

        /// <summary>
        /// Gets the status of an Response.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date for which this response was made.</param>
        /// <param name="email">The email.</param>
        /// <returns>The status of the specified Response</returns>
        /// <exception cref="DBException">If an error occurs while retrieving this record</exception>
        public static ResponseStatus GetResponseStatus(int eventId, DateTime eventStart, string email)
        {
            IDataProvider dp = DataProvider.Instance;
            ResponseStatus status = ResponseStatus.NoResponse;

            try
            {
                using (IDataReader dr = dp.ExecuteReader(
                        CommandType.StoredProcedure,
                        dp.NamePrefix + "spGetResponseByEmail",
                        Utility.CreateIntegerParam("@EventId", eventId),
                        Utility.CreateDateTimeParam("@EventStart", eventStart),
                        Utility.CreateVarcharParam("@Email", email)))
                {
                    if (dr.Read())
                    {
                        status = (ResponseStatus)Enum.Parse(typeof(ResponseStatus), dr["Status"].ToString());
                    }
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetResponseByEmail", se);
            }

            return status;
        }

        /// <summary>
        /// Saves this Response.
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

        /// <summary>
        /// Fills an Response from the provided data record.
        /// </summary>
        /// <param name="dataRecord">A data record representing an Response instance.</param>
        /// <returns>The requested Response instance</returns>
        internal static Response Fill(IDataRecord dataRecord)
        {
            Response response = new Response();

            response.id = (int)dataRecord["ResponseId"];
            response.eventId = (int)dataRecord["EventId"];
            response.eventStart = (DateTime)dataRecord["EventStart"];
            response.lastName = dataRecord["LastName"].ToString();
            response.firstName = dataRecord["FirstName"].ToString();
            response.email = dataRecord["Email"].ToString();
            response.status = (ResponseStatus)Enum.Parse(typeof(ResponseStatus), dataRecord["Status"].ToString());
            response.createdBy = (int)dataRecord["CreatedBy"];
            response.creationDate = (DateTime)dataRecord["CreationDate"];

            return response;
        }

        /// <summary>
        /// Inserts this Response into the database.
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
                    dp.NamePrefix + "spInsertResponse",
                    Utility.CreateIntegerParam("@EventId", this.eventId),
                    Utility.CreateDateTimeParam("@EventStart", this.eventStart),
                    Utility.CreateVarcharParam("@FirstName", this.firstName),
                    Utility.CreateVarcharParam("@LastName", this.lastName),
                    Utility.CreateVarcharParam("@Email", this.email),
                    Utility.CreateVarcharParam("@Status", this.status.ToString()),
                    Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spInsertResponse", de);
            }
        }

        /// <summary>
        /// Updates this Response record.
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
                    dp.NamePrefix + "spUpdateResponse",
                    Utility.CreateIntegerParam("@ResponseId", this.id),
                    Utility.CreateVarcharParam("@FirstName", this.firstName),
                    Utility.CreateVarcharParam("@LastName", this.lastName),
                    Utility.CreateVarcharParam("@Email", this.email),
                    Utility.CreateVarcharParam("@Status", this.status.ToString()),
                    Utility.CreateIntegerParam("@RevisingUser", revisingUser));
            }
            catch (SystemException de)
            {
                throw new DBException("spUpdateResponse", de);
            }
        }
    }
}