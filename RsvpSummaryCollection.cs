using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Engage.Data;

namespace Engage.Events
{
    /// <summary>
    /// This class inherits from BindingList for future support.
    /// </summary>
    public class RsvpSummaryCollection: System.ComponentModel.BindingList<RsvpSummary>
    {
        private RsvpSummaryCollection() { }

        public static RsvpSummaryCollection Load(int portalId, string sortColumn, int index, int pageSize)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (DataSet ds = dp.ExecuteDataset(CommandType.StoredProcedure, dp.NamePrefix + "spGetRsvpSummary",
                 Engage.Utility.CreateIntegerParam("@portalId", portalId),
                 Engage.Utility.CreateVarcharParam("@sortColumn", sortColumn, 200),
                 Engage.Utility.CreateIntegerParam("@index", index),
                 Engage.Utility.CreateIntegerParam("@pageSize", pageSize)))
                {
                    return FillRsvpSummary(ds);
                }
            }
            catch (Exception se)
            {
                throw new DBException("spGetRsvpSummary", se);
            }
        }

        private static RsvpSummaryCollection FillRsvpSummary(DataSet ds)
        {
            RsvpSummaryCollection rsvps = new RsvpSummaryCollection();
            
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                RsvpSummary r = RsvpSummary.Fill(row);
                //so on the outside EventCollection will report the total # of rows for paging.
                rsvps._totalRecords = r.TotalRecords;
                rsvps.Add(r);
            }

            return rsvps;
        }

        #region Properties
        
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
