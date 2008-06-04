using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Engage.Dnn.Events.Util;

namespace Engage.Dnn.Events.Recurrence
{
    public enum WeekDayType
    {
        WeekDay,
        WeekendDay
    }

    public class RecurrenceHelper
    {

        //public static IList CreateNthOccurrenceList()
        //{
        //    ArrayList dataSource = new ArrayList();
        //    dataSource.Add(new LookupPair("first", 1));
        //    dataSource.Add(new LookupPair("second", 2));
        //    dataSource.Add(new LookupPair("third", 3));
        //    dataSource.Add(new LookupPair("fourth", 4));
        //    dataSource.Add(new LookupPair("last", -1));

        //    return dataSource;
        //}

        //public static IList CreateWeekDayOccurrenceList()
        //{
        //    ArrayList dataSource = new ArrayList();
        //    dataSource.Add(new LookupPair("first", 1));
        //    dataSource.Add(new LookupPair("last", -1));
        //    return dataSource;
        //}

        //public static IList CreateWeekDayTypeList()
        //{
        //    ArrayList dataSource = new ArrayList();
        //    dataSource.Add(new LookupPair("weekday", (int)WeekDayType.WeekDay));
        //    dataSource.Add(new LookupPair("weekend day", (int)WeekDayType.WeekendDay));
        //    return dataSource;
        //}

        //public static IList CreateDayOfWeekList()
        //{
        //    ArrayList dataSource = new ArrayList();
        //    dataSource.Add(CreateDayOfWeekItem(DayOfWeek.Sunday));
        //    dataSource.Add(CreateDayOfWeekItem(DayOfWeek.Monday));
        //    dataSource.Add(CreateDayOfWeekItem(DayOfWeek.Tuesday));
        //    dataSource.Add(CreateDayOfWeekItem(DayOfWeek.Wednesday));
        //    dataSource.Add(CreateDayOfWeekItem(DayOfWeek.Thursday));
        //    dataSource.Add(CreateDayOfWeekItem(DayOfWeek.Friday));
        //    dataSource.Add(CreateDayOfWeekItem(DayOfWeek.Saturday));
        //    return dataSource;
        //}

        ////private static LookupPair CreateDayOfWeekItem(DayOfWeek dayOfWeek)
        ////{
        ////    return new LookupPair(CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)dayOfWeek], (int)dayOfWeek);
        ////}

        //public static IList CreateMonthList()
        //{
        //    ArrayList dataSource = new ArrayList();
        //    for (int i = 1; i <= 12; ++i)
        //    {
        //        string month = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i - 1];
        //        dataSource.Add(new LookupPair(month, i));
        //    }
        //    return dataSource;
        //}

        //public static IList CreateDayList()
        //{
        //    ArrayList dataSource = new ArrayList();
        //    for (int i = 1; i <= 31; ++i)
        //        dataSource.Add(new LookupPair(i.ToString(), i));

        //    dataSource.Add(new LookupPair("last", -1));
        //    return dataSource;
        //}

    }
}
