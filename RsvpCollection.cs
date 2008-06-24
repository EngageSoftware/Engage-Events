using System;
using System.Data;
using System.Diagnostics;
using Engage.Data;

namespace Engage.Events
{
    /// <summary>
    /// This class inherits from BindingList for future support.
    /// </summary>
    public class RsvpCollection: System.ComponentModel.BindingList<Rsvp>
    {
        private RsvpCollection() { }

        public static RsvpCollection Load(int eventId, string status, string sortColumn, int index, int pageSize)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (DataSet ds = dp.ExecuteDataset(CommandType.StoredProcedure, dp.NamePrefix + "spGetRsvps",
                 Utility.CreateIntegerParam("@EventId", eventId),
                 Utility.CreateVarcharParam("@Status", status),
                 Utility.CreateVarcharParam("@sortColumn", sortColumn, 200),
                 Utility.CreateIntegerParam("@index", index),
                 Utility.CreateIntegerParam("@pageSize", pageSize)))
                {
                    return FillRsvps(ds);
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetRsvps", se);
            }
        }

        private static RsvpCollection FillRsvps(DataSet ds)
        {
            RsvpCollection rsvps = new RsvpCollection();
            
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Rsvp r = Rsvp.Fill(row);
                //so on the outside EventCollection will report the total # of rows for paging.
                rsvps._totalRecords = r.TotalRecords;
                rsvps.Add(r);
            }

            return rsvps;
        }

        #region Properties
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _totalRecords;
        public int TotalRecords
        {
            [DebuggerStepThrough]
            get { return _totalRecords; }
        }

        #endregion

    }
}
