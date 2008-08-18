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

namespace Engage.Dnn.Events.Util
{
    using System;
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
        public const string DnnFriendlyModuleName = "Engage: Events";
        
        /// <summary>
        /// The host setting key base for whether this module have been configured
        /// </summary>
        public const string ModuleConfigured = "ModuleConfigured";
        
        ////public const string AdminContainer = "AdminContainer";
        ////public const string Container = "UserContainer";
        ////public const string AdminRole = "EventsAminRole";

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
                sb.Append(Globals.GetDesktopModuleByName(DnnFriendlyModuleName).FolderName);
                sb.Append("/");
                return sb.ToString();
            }
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
        /// Determines whether the specified email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns>
        /// 	<c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            return Engage.Utility.ValidateEmailAddress(emailAddress);
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
        /// Gets a comma-delimited list of the days of week from the given <paramref name="daysOfWeekMask"/> with localized day names.
        /// </summary>
        /// <param name="daysOfWeekMask">The days of week mask.</param>
        /// <returns>A list of the days of week from the given <paramref name="daysOfWeekMask"/> with localized day names</returns>
        public static string GetDaysOfWeekList(RecurrenceDay daysOfWeekMask)
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
        public static void AddDayToList(RecurrenceDay daysOfWeekMask, StringBuilder daysOfWeek, RecurrenceDay recurrenceDay, DayOfWeek dayOfWeek)
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
        public static string GetWeeklyRecurrenceSummary(RecurrencePattern pattern, string resourceFile)
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
        public static string GetMonthlyRecurrenceSummary(RecurrencePattern pattern, string resourceFile)
        {
            if (pattern.DayOfMonth > 0)
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("MonthlyRecurrenceOnDate.Text", resourceFile),
                    pattern.DayOfMonth,
                    pattern.Interval);
            }
            else
            {
                // TODO: Localize DayOrdinal and Day of Week
                return string.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("MonthlyRecurrenceOnGivenDay.Text", resourceFile),
                    pattern.DayOrdinal,
                    pattern.DaysOfWeekMask,
                    pattern.Interval);
            }
        }

        /// <summary>
        /// Gets the recurrence summary for a yearly recurrence pattern.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        /// <param name="resourceFile">The resource file to use to find get localized text.</param>
        /// <returns>A human-readable, localized summary of the provided recurrence pattern.</returns>
        public static string GetYearlyRecurrenceSummary(RecurrencePattern pattern, string resourceFile)
        {
            if (pattern.DayOfMonth > 0)
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("YearlyRecurrenceOnDate.Text", resourceFile),
                    new DateTime(1, (int)pattern.Month, 1),
                    pattern.DayOfMonth);
            }
            else
            {
                // TODO: Localize DayOrdinal, Day of Week
                return string.Format(
                    CultureInfo.CurrentCulture,
                    Localization.GetString("YearlyRecurrenceOnGivenDay.Text", resourceFile),
                    pattern.DayOrdinal,
                    pattern.DaysOfWeekMask,
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
        public static string GetDailyRecurrenceSummary(RecurrencePattern pattern, string resourceFile)
        {
            if (pattern.DaysOfWeekMask == RecurrenceDay.WeekDays)
            {
                return Localization.GetString("DailyRecurrenceWeekdays.Text", resourceFile);
            }
            else
            {
                return string.Format(
                    CultureInfo.CurrentCulture, 
                    Localization.GetString("DailyRecurrence.Text", resourceFile), 
                    pattern.Interval);
            }
        }
    }
}

