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
    public class EventCollection: System.ComponentModel.BindingList<Event>
    {
        private EventCollection(){}

        public static EventCollection Load(int portalId, string sortColumn, int index, int pageSize, bool showAll)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (DataSet ds = dp.ExecuteDataset(CommandType.StoredProcedure, dp.NamePrefix + "spGetEvents",
                    Engage.Utility.CreateIntegerParam("@portalId", portalId),
                    Engage.Utility.CreateVarcharParam("@sortColumn", sortColumn, 200),
                    Engage.Utility.CreateIntegerParam("@index", index),
                    Engage.Utility.CreateIntegerParam("@pageSize", pageSize),
                    Engage.Utility.CreateBitParam("@showAll", showAll))
                )
                {
                    return FillEvents(ds);
                }
            }
            catch (Exception se)
            {
                throw new DbException("spGetEvents", se);
            }
        }

        public static EventCollection Load(int portalId, bool currentMonth, int index, int pageSize)
        {
            IDataProvider dp = DataProvider.Instance;
            try
            {
                using (DataSet ds = dp.ExecuteDataset(CommandType.StoredProcedure, dp.NamePrefix + "spGetEventsSpecific",
                 Engage.Utility.CreateIntegerParam("@portalId", portalId),
                 Engage.Utility.CreateBitParam("@currentMonth", currentMonth),
                 Engage.Utility.CreateIntegerParam("@index", index),
                 Engage.Utility.CreateIntegerParam("@pageSize", pageSize)))
                {
                    return FillEvents(ds);
                }
            }
            catch (Exception se)
            {
                throw new DbException("spGetEvents", se);
            }
        }

        private static EventCollection FillEvents(DataSet ds)
        {
            EventCollection events = new EventCollection();
            
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Event e = Event.Fill(row);
                //so on the outside EventCollection will report the total # of rows for paging.
                events._totalRecords = e.TotalRecords;
                events.Add(e);
            }

            return events;
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
