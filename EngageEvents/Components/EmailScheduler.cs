using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.FileSystem;
using Engage.Routing;

namespace Engage.Dnn.Events
{
    public class EmailScheduler : DotNetNuke.Services.Scheduling.SchedulerClient
    {
        public EmailScheduler(DotNetNuke.Services.Scheduling.ScheduleHistoryItem objScheduleHistoryItem)
        {
            ScheduleHistoryItem = objScheduleHistoryItem;
        }

        public override void DoWork()
        {
            RoutingManager rm = RoutingManager.Instance;
            rm.RunServiceEvents(0);

            ScheduleHistoryItem.Succeeded = true;
            ScheduleHistoryItem.AddLogNote("Email Scheduler completed successfully.<br>");

        }
    }
}