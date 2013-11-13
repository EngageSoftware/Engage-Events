// <copyright file="ICalUtil.cs" company="Engage Software">
// Engage.Events - http://www.engagemodules.com
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Events.Util
{
    using System;
    using System.Globalization;
    using System.Text;
    using Telerik.Web.UI;

    /// <summary>
    /// Utilities for creating iCalendar files
    /// </summary>
    /// <remarks>
    /// Based off of class from Telerik's RadControl for ASP.NET AJAX
    /// </remarks>
    internal static class ICalUtil
    {
        /// <summary>
        /// Format to use for dates within the iCalendar file
        /// </summary>
        private const string DateFormat = "yyyyMMddTHHmmssZ";

        /// <summary>
        /// Exports the specified appointment as an iCalendar file.
        /// </summary>
        /// <param name="description">The event's description.</param>
        /// <param name="location">The event's location.</param>
        /// <param name="app">The event to export.</param>
        /// <param name="outlookCompatibleMode">if set to <c>true</c> make the output compatible with Outlook.</param>
        /// <returns>The given event in an iCalendar format</returns>
        public static string Export(string description, string location, Appointment app, bool outlookCompatibleMode)
        {
            var output = new StringBuilder();
            WriteFileHeader(output, outlookCompatibleMode);

            WriteTask(description, location, output, app, outlookCompatibleMode);

            WriteFileFooter(output);

            return output.ToString();
        }

        /// <summary>
        /// Writes the the entry for an event.
        /// </summary>
        /// <param name="description">The event's description.</param>
        /// <param name="location">The event's location.</param>
        /// <param name="output">The <see cref="StringBuilder"/> into which the output should be appended.</param>
        /// <param name="app">The appointment to export.</param>
        /// <param name="outlookCompatibleMode">if set to <c>true</c> make the output compatible with Outlook.</param>
        /// <exception cref="InvalidOperationException">Invalid recurrence rule.</exception>
        private static void WriteTask(string description, string location, StringBuilder output, Appointment app, bool outlookCompatibleMode)
        {
            output.AppendLine("BEGIN:VEVENT");
            output.AppendLine("DESCRIPTION:" + description.Replace("\n", "\\n").Replace("\r", "\\r"));
            output.AppendLine("LOCATION:" + location);

            if (!string.IsNullOrEmpty(app.RecurrenceRule))
            {
                RecurrenceRule rrule;
                if (!RecurrenceRule.TryParse(app.RecurrenceRule, out rrule))
                {
                    throw new InvalidOperationException("Invalid recurrence rule.");
                }

                if (outlookCompatibleMode)
                {
                    foreach (DateTime occ in rrule.Occurrences)
                    {
                        // Outlook requires that the start date is the same as the first occurrence.
                        rrule.Range.Start = occ;
                        break;
                    }

                    if ((rrule.Pattern.Frequency == RecurrenceFrequency.Daily ||
                        rrule.Pattern.Frequency == RecurrenceFrequency.Monthly) &&
                        rrule.Pattern.DaysOfWeekMask == RecurrenceDay.EveryDay)
                    {
                        rrule.Pattern.DaysOfWeekMask = RecurrenceDay.None;
                    }
                }

                output.Append(rrule.ToString());
            }
            else
            {
                output.AppendFormat("DTSTART:{0}\r\n", FormatDate(app.Start));
                output.AppendFormat("DTEND:{0}\r\n", FormatDate(app.End));
            }

            if (outlookCompatibleMode)
            {
                output.AppendFormat("UID:{0}-{1}\r\n", FormatDate(DateTime.UtcNow), app.ID);
                output.AppendFormat("DTSTAMP:{0}\r\n", FormatDate(DateTime.UtcNow));
            }

            string summary = app.Subject.Replace("\r\n", "\\n");
            summary = summary.Replace("\n", "\\n");
            output.AppendFormat("SUMMARY:{0}\r\n", summary);
            output.AppendLine("END:VEVENT");
        }

        /// <summary>
        /// Writes the file header.
        /// </summary>
        /// <param name="output">The <see cref="StringBuilder"/> into which the output should be appended.</param>
        /// <param name="outlookCompatibleMode">if set to <c>true</c> make the output compatible with Outlook.</param>
        private static void WriteFileHeader(StringBuilder output, bool outlookCompatibleMode)
        {
            output.AppendLine("BEGIN:VCALENDAR");
            output.AppendLine("VERSION:2.0");
            ////output.AppendLine("PRODID:-//Telerik Inc.//NONSGML RadScheduler//EN");
            output.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");

            if (outlookCompatibleMode)
            {
                output.AppendLine("METHOD:PUBLISH");
            }
        }

        /// <summary>
        /// Writes the file footer.
        /// </summary>
        /// <param name="output">The <see cref="StringBuilder"/> into which the output should be appended.</param>
        private static void WriteFileFooter(StringBuilder output)
        {
            output.AppendLine("END:VCALENDAR");
        }

        /// <summary>
        /// Formats the given date to the iCalendar date format.
        /// </summary>
        /// <param name="date">The date to be formatted.</param>
        /// <returns>The given date formatted for an iCalendar file.</returns>
        private static string FormatDate(DateTime date)
        {
            return date.ToString(DateFormat, CultureInfo.InvariantCulture);
        }
    }
}
