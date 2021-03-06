// <copyright file="Utility.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
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
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    using DotNetNuke.Services.Localization;

    using Engage.Annotations;
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
        /// The friendly name of this module's definition.
        /// </summary>
        public const string ModuleDefinitionFriendlyName = "Engage: Events";

        /// <summary>
        /// Backing field for <see cref="OrdinalValues"/>
        /// </summary>
        private static readonly IDictionary<int, string> OrdinalValuesDictionary = GetOrdinalValues();

        /// <summary>
        /// Gets the name of the desktop module folder.
        /// </summary>
        /// <value>The name of the desktop module folder.</value>
        public static string DesktopModuleFolderName
        {
            get
            {
                return Dnn.Utility.GetDesktopModuleFolderName(DesktopModuleName);
            }
        }

        /// <summary>
        /// Gets the relative path to the resource file holding resources that are shared by multiple controls within the module.
        /// </summary>
        /// <value>The relative path to the resource file holding resources that are shared by multiple controls within the module</value>
        public static string LocalSharedResourceFile
        {
            get { return "~" + DesktopModuleFolderName + Localization.LocalResourceDirectory + "/" + Localization.LocalSharedResourceFile; }
        }

        /// <summary>
        /// Gets a dictionary mapping ordinal day values (based on <see cref="RecurrencePattern.DayOrdinal"/>) to their localization resource keys.
        /// </summary>
        /// <value>The mapping between ordinal day values and their localization resource keys.</value>
        public static IDictionary<int, string> OrdinalValues
        {
            get { return OrdinalValuesDictionary; }
        }

        /// <summary>
        /// Gets query-string parameter(s) that represent an instance of an <see cref="Event"/>.
        /// </summary>
        /// <param name="selectedEvent">The <see cref="Event"/> to represent.</param>
        /// <param name="additionalParameters">Any other query-string parameters.</param>
        /// <returns>A list of query-string parameters that represent <paramref name="selectedEvent"/></returns>
        public static string[] GetEventParameters(Event selectedEvent, params string[] additionalParameters)
        {
            if (selectedEvent == null)
            {
                throw new ArgumentNullException("selectedEvent");
            }

            return GetEventParameters(selectedEvent.Id, selectedEvent.EventStart, additionalParameters);
        }

        /// <summary>
        /// Gets query-string parameter(s) that represent the given event information
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventStart">The date and time at which this occurrence starts.</param>
        /// <param name="additionalParameters">Any other query-string parameters.</param>
        /// <returns>A list of query-string parameters that represent the given event information</returns>
        public static string[] GetEventParameters(int eventId, DateTime eventStart, params string[] additionalParameters)
        {
            if (additionalParameters == null)
            {
                throw new ArgumentNullException("additionalParameters");
            }

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

            return string.Format(CultureInfo.CurrentCulture, Localization.GetString(timespanResourceKey, resourceFile), startDate, endDate);
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
            return GetRecurrenceSummary(recurrenceRule, LocalSharedResourceFile);
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
            string recurrenceSummary = string.Empty;
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
        /// Gets the name of the <see cref="Event"/> property in the given property expression.
        /// </summary>
        /// <remarks>
        /// Blows up if the expression isn't a simple property access expression
        /// based on <see href="http://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression/672330#672330"/>
        /// </remarks>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="propertyExpression">An expression representing accessing a property.</param>
        /// <returns>The name of the property in the expression</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Sorry, can't use expressions without nesting types")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Requires specific signature")]
        public static string GetPropertyName<T>(Expression<Func<Event, T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }
            
            return ((MemberExpression)propertyExpression.Body).Member.Name;
        }

        /// <summary>
        /// Wraps the <paramref name="dataReader"/> in a <see cref="DataReaderEnumerable"/>.
        /// </summary>
        /// <param name="dataReader">The data reader to wrap.</param>
        /// <returns>A <see cref="DataReaderEnumerable"/> instance wrapping the <paramref name="dataReader"/></returns>
        public static DataReaderEnumerable AsEnumerable(this IDataReader dataReader)
        {
            return new DataReaderEnumerable(dataReader);
        }

        /// <summary>
        /// Adds the ancestor ids.
        /// </summary>
        /// <param name="categoryIds">The category ids.</param>
        /// <param name="categories">The categories.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns>
        /// Array of the categoryIds with its ancestors
        /// </returns>
        public static IEnumerable<int> AddAncestorIds([NotNull] IEnumerable<int> categoryIds, IEnumerable<Category> categories, bool recursive)
        {
            if (categoryIds == null)
            {
                throw new ArgumentNullException("categoryIds");
            }

            var ancestorList = new List<int>();
            foreach (var categoryId in categoryIds)
            {
                var lambdaCategoryId = categoryId;
                var category = categories.FirstOrDefault(c => c.Id == lambdaCategoryId);
                if (category == null)
                {
                    continue;
                }

                // find its parent
                var parent = categories.FirstOrDefault(c => c.Id == category.ParentId);
                if (parent != null && !ancestorList.Contains(parent.Id) && !categoryIds.Contains(parent.Id))
                {
                    ancestorList.Add(parent.Id);
                }
            }

            if (ancestorList.Count > 0 && recursive)
            {
                // need to find the next ancestor for the categories in this list.
                var nextAncestorList = AddAncestorIds(ancestorList.ToArray(), categories, true);
                return nextAncestorList.Concat(categoryIds);
            }

            ancestorList.AddRange(categoryIds);
            return ancestorList;
        }

        /// <summary>
        /// Fills <see cref="OrdinalValuesDictionary"/>.
        /// </summary>
        /// <returns>A dictionary mapping ordinal day values (based on <see cref="RecurrencePattern.DayOrdinal"/>) to their localization resource keys</returns>
        private static IDictionary<int, string> GetOrdinalValues()
        {
            return new Dictionary<int, string>
                {
                           { 1, "First" },
                           { 2, "Second" },
                           { 3, "Third" },
                           { 4, "Fourth" },
                           { -1, "Last" }
                       };
        }

        /// <summary>
        /// Gets a comma-delimited list of the days of week from the given <paramref name="daysOfWeekMask"/> with localized day names.
        /// </summary>
        /// <param name="daysOfWeekMask">The days of week mask.</param>
        /// <returns>A list of the days of week from the given <paramref name="daysOfWeekMask"/> with localized day names</returns>
        private static string GetDaysOfWeekList(RecurrenceDay daysOfWeekMask)
        {
            var daysOfWeek = new StringBuilder();

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
            return string.Format(
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
                return string.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("MonthlyRecurrenceOnDate.Text", resourceFile),
                    pattern.DayOfMonth,
                    pattern.Interval);
            }
            
            return string.Format(
                CultureInfo.CurrentCulture,
                Localization.GetString("MonthlyRecurrenceOnGivenDay.Text", resourceFile),
                Localization.GetString(OrdinalValuesDictionary[pattern.DayOrdinal], resourceFile),
                GetLocalizedDayOfWeek(pattern.DaysOfWeekMask, resourceFile),
                pattern.Interval);
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
                return string.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("YearlyRecurrenceOnDate.Text", resourceFile),
                    new DateTime(1, (int)pattern.Month, 1),
                    pattern.DayOfMonth);
            }
            
            return string.Format(
                CultureInfo.CurrentCulture,
                Localization.GetString("YearlyRecurrenceOnGivenDay.Text", resourceFile),
                Localization.GetString(OrdinalValuesDictionary[pattern.DayOrdinal], resourceFile),
                GetLocalizedDayOfWeek(pattern.DaysOfWeekMask, resourceFile),
                new DateTime(1, (int)pattern.Month, 1));
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
            
            return string.Format(
                CultureInfo.CurrentCulture, 
                Localization.GetString("DailyRecurrence.Text", resourceFile), 
                pattern.Interval);
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