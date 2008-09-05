// <copyright file="Utility.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Web.Hosting;
    using DotNetNuke.Common;
    using DotNetNuke.Services.Localization;
    using Engage.Events;
    using Telerik.Web.UI;

    /// <summary>
    /// All common, shared functionality for the Engage: Events module.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// The friendly name of this module.
        /// </summary>
        public const string DesktopModuleName = "Engage: Events";
        
        /// <summary>
        /// The host setting key base for whether this module have been configured
        /// </summary>
        public const string ModuleConfigured = "ModuleConfigured";

        /// <summary>
        /// The License Key for the Engage: Events module
        /// </summary>
        internal static readonly Guid LicenseKey = new Guid("2de915e1-df71-3443-9f4d-32259c92ced2");

        /// <summary>
        /// Backing field for <see cref="OrdinalValues"/>
        /// </summary>
        private static readonly IDictionary<int, string> ordinalValues = GetOrdinalValues();

        /// <summary>
        /// Gets the name of the desktop module folder.
        /// </summary>
        /// <value>The name of the desktop module folder.</value>
        public static string DesktopModuleFolderName
        {
            get
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append("/DesktopModules/");
                sb.Append(Globals.GetDesktopModuleByName(DesktopModuleName).FolderName);
                sb.Append("/");
                return sb.ToString();
            }
        }
        public static string LocalSharedResourceFile
        {
            get { return "~" + DesktopModuleFolderName + Localization.LocalResourceDirectory + "/" + Localization.LocalSharedResourceFile; }
        }

        /// <summary>
        /// Gets the relative path to the templates folder.
        /// </summary>
        /// <value>The relative path to the templates folder</value>
        public static string TemplatesFolderName
        {
            get
            {
                return DesktopModuleFolderName + "Templates/";
            }
        }

        /// <summary>
        /// Gets the full physical path for the templates folder.
        /// </summary>
        /// <value>The full physical path for the templates folder</value>
        public static string PhysicalTemplatesFolderName
        {
            get
            {
                return HostingEnvironment.MapPath("~" + TemplatesFolderName);
            }
        }

        /// <summary>
        /// Gets a dictionary mapping ordinal day values (based on <see cref="RecurrencePattern.DayOrdinal"/>) to their localization resource keys.
        /// </summary>
        public static IDictionary<int, string> OrdinalValues
        {
            get { return ordinalValues; }
        }

        /// <summary>
        /// Determines whether the specified email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns>
        /// <c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            return Engage.Utility.ValidateEmailAddress(emailAddress);
        }

        /// <summary>
        /// Gets <c>QueryString</c> parameter(s) that represent an instance of an <see cref="Event"/>.
        /// </summary>
        /// <param name="selectedEvent">The <see cref="Event"/> to represent.</param>
        /// <returns>A list of <c>QueryString</c> parameters that represent <paramref name="selectedEvent"/></returns>
        public static string[] GetEventParameters(Event selectedEvent)
        {
            return GetEventParameters(selectedEvent.Id, selectedEvent.EventStart);
        }

        /// <summary>
        /// Gets <c>QueryString</c> parameter(s) that represent the given event information
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date and time at which this occurrence starts.</param>
        /// <returns>A list of <c>QueryString</c> parameters that represent the given event information</returns>
        public static string[] GetEventParameters(int eventId, DateTime eventStart)
        {
            return GetEventParameters(eventId, eventStart, new string[0]);
        }

        /// <summary>
        /// Gets <c>QueryString</c> parameter(s) that represent the given event information
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date and time at which this occurrence starts.</param>
        /// <param name="additionalParameters">Any other querystring parameters.</param>
        /// <returns>A list of <c>QueryString</c> parameters that represent the given event information</returns>
        public static string[] GetEventParameters(int eventId, DateTime eventStart, params string[] additionalParameters)
        {
            Array.Resize(ref additionalParameters, additionalParameters.Length + 2);
            additionalParameters[additionalParameters.Length - 1] = "eventid=" + eventId.ToString(CultureInfo.InvariantCulture);
            additionalParameters[additionalParameters.Length - 2] = "start=" + eventStart.Ticks.ToString(CultureInfo.InvariantCulture);

            return additionalParameters;
        }

        /// <summary>
        /// Gets the formatted date string for this event.
        /// </summary>
        /// <param name="startDate">The event's start date.</param>
        /// <param name="endDate">The event's end date.</param>
        /// <returns>A formatted string representing the timespan over which this event occurs.</returns>
        public static string GetFormattedEventDate(DateTime startDate, DateTime endDate)
        {
            return GetFormattedEventDate(startDate, endDate, LocalSharedResourceFile);
        }

        /// <summary>
        /// Gets the formatted date string for this event.
        /// </summary>
        /// <param name="startDate">The event's start date.</param>
        /// <param name="endDate">The event's end date.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>
        /// A formatted string representing the timespan over which this event occurs.
        /// </returns>
        public static string GetFormattedEventDate(DateTime startDate, DateTime endDate, string resourceFile)
        {
            string timespanResourceKey;
            if (startDate.Year != endDate.Year)
            {
                timespanResourceKey = "TimespanDifferentYear.Text";
            }
            else if (startDate.Month != endDate.Month)
            {
                timespanResourceKey = "TimespanDifferentMonth.Text";
            }
            else if (startDate.Day != endDate.Day)
            {
                timespanResourceKey = "TimespanDifferentDay.Text";
            }
            else
            {
                timespanResourceKey = "Timespan.Text";
            }

            return String.Format(CultureInfo.CurrentCulture, Localization.GetString(timespanResourceKey, resourceFile), startDate, endDate);
        }

        /// <summary>
        /// Gets the recurrence summary for the recurrence pattern of the given <paramref name="recurrenceRule"/>.
        /// </summary>
        /// <param name="recurrenceRule">The recurrence rule to summarize.</param>
        /// <returns>
        /// A human-readable, localized summary of the provided recurrence pattern.
        /// </returns>
        public static string GetRecurrenceSummary(RecurrenceRule recurrenceRule)
        {
            string recurrenceSummary = String.Empty;
            if (recurrenceRule != null)
            {
                switch (recurrenceRule.Pattern.Frequency)
                {
                    case RecurrenceFrequency.Weekly:
                        recurrenceSummary = GetWeeklyRecurrenceSummary(recurrenceRule.Pattern, LocalSharedResourceFile);
                        break;
                    case RecurrenceFrequency.Monthly:
                        recurrenceSummary = GetMonthlyRecurrenceSummary(recurrenceRule.Pattern, LocalSharedResourceFile);
                        break;
                    case RecurrenceFrequency.Yearly:
                        recurrenceSummary = GetYearlyRecurrenceSummary(recurrenceRule.Pattern, LocalSharedResourceFile);
                        break;
                        ////case RecurrenceFrequency.Daily:
                    default:
                        recurrenceSummary = GetDailyRecurrenceSummary(recurrenceRule.Pattern, LocalSharedResourceFile);
                        break;
                }
            }

            return recurrenceSummary;
        }

        /// <summary>
        /// Gets the recurrence summary for the recurrence pattern of the given <paramref name="recurrenceRule"/>.
        /// </summary>
        /// <param name="recurrenceRule">The recurrence rule to summarize.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>
        /// A human-readable, localized summary of the provided recurrence pattern.
        /// </returns>
        public static string GetRecurrenceSummary(RecurrenceRule recurrenceRule, string resourceFile)
        {
            string recurrenceSummary = String.Empty;
            if (recurrenceRule != null)
            {
                switch (recurrenceRule.Pattern.Frequency)
                {
                    case RecurrenceFrequency.Weekly:
                        recurrenceSummary = GetWeeklyRecurrenceSummary(recurrenceRule.Pattern, resourceFile);
                        break;
                    case RecurrenceFrequency.Monthly:
                        recurrenceSummary = GetMonthlyRecurrenceSummary(recurrenceRule.Pattern, resourceFile);
                        break;
                    case RecurrenceFrequency.Yearly:
                        recurrenceSummary = GetYearlyRecurrenceSummary(recurrenceRule.Pattern, resourceFile);
                        break;
                        ////case RecurrenceFrequency.Daily:
                    default:
                        recurrenceSummary = GetDailyRecurrenceSummary(recurrenceRule.Pattern, resourceFile);
                        break;
                }
            }

            return recurrenceSummary;
        }

        /// <summary>
        /// Fills <see cref="ordinalValues"/>.
        /// </summary>
        /// <returns>A dictionary mapping ordinal day values (based on <see cref="RecurrencePattern.DayOrdinal"/>) to their localization resource keys</returns>
        private static IDictionary<int, string> GetOrdinalValues()
        {
            IDictionary<int, string> ordinalsDictionary = new Dictionary<int, string>();
            
            ordinalsDictionary.Add(1, "First");
            ordinalsDictionary.Add(2, "Second");
            ordinalsDictionary.Add(3, "Third");
            ordinalsDictionary.Add(4, "Fourth");
            ordinalsDictionary.Add(-1, "Last");

            return ordinalsDictionary;
        }

        /// <summary>
        /// Gets a comma-delimited list of the days of week from the given <paramref name="daysOfWeekMask"/> with localized day names.
        /// </summary>
        /// <param name="daysOfWeekMask">The days of week mask.</param>
        /// <returns>A list of the days of week from the given <paramref name="daysOfWeekMask"/> with localized day names</returns>
        private static string GetDaysOfWeekList(RecurrenceDay daysOfWeekMask)
        {
            StringBuilder daysOfWeek = new StringBuilder();

            AddDayToList(daysOfWeekMask, daysOfWeek, RecurrenceDay.Sunday, DayOfWeek.Sunday);
            AddDayToList(daysOfWeekMask, daysOfWeek, RecurrenceDay.Monday, DayOfWeek.Monday);
            AddDayToList(daysOfWeekMask, daysOfWeek, RecurrenceDay.Tuesday, DayOfWeek.Tuesday);
            AddDayToList(daysOfWeekMask, daysOfWeek, RecurrenceDay.Wednesday, DayOfWeek.Wednesday);
            AddDayToList(daysOfWeekMask, daysOfWeek, RecurrenceDay.Thursday, DayOfWeek.Thursday);
            AddDayToList(daysOfWeekMask, daysOfWeek, RecurrenceDay.Friday, DayOfWeek.Friday);
            AddDayToList(daysOfWeekMask, daysOfWeek, RecurrenceDay.Saturday, DayOfWeek.Saturday);

            return daysOfWeek.ToString();
        }

        /// <summary>
        /// Adds the day to list.
        /// </summary>
        /// <param name="daysOfWeekMask">The days of week mask.</param>
        /// <param name="daysOfWeek">The days of week.</param>
        /// <param name="recurrenceDay">The recurrence day.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        private static void AddDayToList(RecurrenceDay daysOfWeekMask, StringBuilder daysOfWeek, RecurrenceDay recurrenceDay, DayOfWeek dayOfWeek)
        {
            if ((daysOfWeekMask & recurrenceDay) != 0)
            {
                if (daysOfWeek.Length > 0)
                {
                    daysOfWeek.Append(", ");
                }

                daysOfWeek.Append(CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dayOfWeek));
            }
        }

        /// <summary>
        /// Gets the recurrence summary for a weekly recurrence pattern.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>A human-readable, localized summary of the provided recurrence pattern.</returns>
        private static string GetWeeklyRecurrenceSummary(RecurrencePattern pattern, string resourceFile)
        {
            return String.Format(
                CultureInfo.CurrentCulture,
                Localization.GetString("WeeklyRecurrence.Text", resourceFile),
                pattern.Interval,
                GetDaysOfWeekList(pattern.DaysOfWeekMask));
        }

        /// <summary>
        /// Gets the recurrence summary for a monthly recurrence pattern.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>A human-readable, localized summary of the provided recurrence pattern.</returns>
        private static string GetMonthlyRecurrenceSummary(RecurrencePattern pattern, string resourceFile)
        {
            if (pattern.DayOfMonth > 0)
            {
                return String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("MonthlyRecurrenceOnDate.Text", resourceFile),
                    pattern.DayOfMonth,
                    pattern.Interval);
            }
            else
            {
                return String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("MonthlyRecurrenceOnGivenDay.Text", resourceFile),
                    Localization.GetString(ordinalValues[pattern.DayOrdinal], resourceFile),
                    GetLocalizedDayOfWeek(pattern.DaysOfWeekMask, resourceFile),
                    pattern.Interval);
            }
        }

        /// <summary>
        /// Gets the recurrence summary for a yearly recurrence pattern.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>A human-readable, localized summary of the provided recurrence pattern.</returns>
        private static string GetYearlyRecurrenceSummary(RecurrencePattern pattern, string resourceFile)
        {
            if (pattern.DayOfMonth > 0)
            {
                return String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("YearlyRecurrenceOnDate.Text", resourceFile),
                    new DateTime(1, (int)pattern.Month, 1),
                    pattern.DayOfMonth);
            }
            else
            {
                return String.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("YearlyRecurrenceOnGivenDay.Text", resourceFile),
                    Localization.GetString(ordinalValues[pattern.DayOrdinal], resourceFile),
                    GetLocalizedDayOfWeek(pattern.DaysOfWeekMask, resourceFile),
                    new DateTime(1, (int)pattern.Month, 1));
            }
        }

        /// <summary>
        /// Gets the recurrence summary for a daily recurrence pattern.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>
        /// A human-readable, localized summary of the provided recurrence pattern.
        /// </returns>
        private static string GetDailyRecurrenceSummary(RecurrencePattern pattern, string resourceFile)
        {
            if (pattern.DaysOfWeekMask == RecurrenceDay.WeekDays)
            {
                return Localization.GetString("DailyRecurrenceWeekdays.Text", resourceFile);
            }
            else
            {
                return String.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.GetString("DailyRecurrence.Text", resourceFile), 
                    pattern.Interval);
            }
        }

        /// <summary>
        /// Gets the localized resource for the day of week.  
        /// Uses <see cref="DateTimeFormatInfo.GetDayName"/> if it's a day of the week, otherwise uses localization for composite values.
        /// </summary>
        /// <param name="daysOfWeekMask">The days of week mask.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>
        /// A human-readable, localized representation of the given <paramref name="daysOfWeekMask"/>
        /// </returns>
        private static string GetLocalizedDayOfWeek(RecurrenceDay daysOfWeekMask, string resourceFile)
        {
            DayOfWeek dayOfWeek;
            switch (daysOfWeekMask)
            {
                case RecurrenceDay.Sunday:
                    dayOfWeek = DayOfWeek.Sunday;
                    break;
                case RecurrenceDay.Monday:
                    dayOfWeek = DayOfWeek.Monday;
                    break;
                case RecurrenceDay.Tuesday:
                    dayOfWeek = DayOfWeek.Tuesday;
                    break;
                case RecurrenceDay.Wednesday:
                    dayOfWeek = DayOfWeek.Wednesday;
                    break;
                case RecurrenceDay.Thursday:
                    dayOfWeek = DayOfWeek.Thursday;
                    break;
                case RecurrenceDay.Friday:
                    dayOfWeek = DayOfWeek.Friday;
                    break;
                case RecurrenceDay.Saturday:
                    dayOfWeek = DayOfWeek.Saturday;
                    break;
                    
                    // If it's not a day of the week, it should be a named composite value, like EveryDay, WeekDays, etc.
                default:
                    return Localization.GetString(daysOfWeekMask.ToString(), resourceFile);
            }

            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dayOfWeek);
        }
    }
}