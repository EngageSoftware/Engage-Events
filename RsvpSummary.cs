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
 
    public class RsvpSummary
    {
        private RsvpSummary()
        {

        }

        private RsvpSummary(int eventId, string title, DateTime eventStart, int attending, int notAttending, int noResponse)
        {
            _eventId = eventId;
            _title = title;
            _eventStart = eventStart;
            _attending = attending;
            _notAttending = notAttending;
            _noResponse = noResponse;
        }

        #region Static Methods

        public static RsvpSummary Create(int eventId, string title, DateTime eventStart, int attending, int notAttending, int noResponse)
        {
            return new RsvpSummary(eventId, title, eventStart, attending, notAttending, noResponse);
        }

        internal static RsvpSummary Fill(DataRow row)
        {
            RsvpSummary r = new RsvpSummary();

            r._eventId = (int)row["EventId"];
            r._title = row["Title"].ToString();
            r._eventStart = (DateTime)row["EventStart"];
            r._attending = (int)row["Attending"];
            r._notAttending = (int)row["NotAttending"];
            r._noResponse = (int)row["NoResponse"];
            //when constructing a collection of events the stored procedure for paging includes a TotalRecords
            //field. When loading a single Event this does not exist.hk
            if (row.Table.Columns.Contains("TotalRecords"))
            {
                r._totalRecords = (int)row["TotalRecords"];
            }

            return r;
        }
        
        #endregion

        
        #region Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _eventId = -1;
        public int EventId
        {
            [DebuggerStepThrough]
            get { return _eventId; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _title = string.Empty;
        public string Title
        {
            [DebuggerStepThrough]
            get { return _title; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime _eventStart;
        public DateTime EventStart
        {
            [DebuggerStepThrough]
            get { return _eventStart; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string EventStartFormatted
        {
            [DebuggerStepThrough]
            get { return _eventStart.ToString("dddd, MMMM d, yyyy, h:mm tt"); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _attending;
        public int Attending
        {
            [DebuggerStepThrough]
            get { return _attending; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _notAttending;
        public int NotAttending
        {
            [DebuggerStepThrough]
            get { return _notAttending; }
        }

        private int _noResponse;
        public int NoResponse
        {
            [DebuggerStepThrough]
            get { return _noResponse; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _totalRecords = 0;
        public int TotalRecords
        {
            [DebuggerStepThrough]
            get { return _totalRecords; }
        }

        #endregion
  
    }
}
