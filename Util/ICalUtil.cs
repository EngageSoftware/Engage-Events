namespace Engage.Events.Util
{
    using System;
    using System.Text;
    using Telerik.Web.UI;

    internal static class ICalUtil
    {
        private const string DateFormat = "yyyyMMddTHHmmssZ";

        public static string Export(string description, string location, Appointment app, bool outlookCompatibleMode, TimeSpan timeZoneOffset)
        {
            StringBuilder output = new StringBuilder();
            WriteFileHeader(output, outlookCompatibleMode);

            //if (app.RecurrenceState != RecurrenceState.Occurrence)
            //{
            //    if (outlookCompatibleMode)
            //    {
            //        ValidateOutlookCompatibility(app);
            //    }
            //}

            WriteTask(description, location,output, app, outlookCompatibleMode, timeZoneOffset);

            WriteFileFooter(output);

            return output.ToString();
        }

        private static void ValidateOutlookCompatibility(Appointment app)
        {
            if (app.RecurrenceRule != string.Empty)
            {
                RecurrenceRule rrule;
                if (!RecurrenceRule.TryParse(app.RecurrenceRule, out rrule))
                {
                    throw new InvalidOperationException("Invalid recurrence rule.");
                }

                if (rrule.Pattern.Frequency == RecurrenceFrequency.Hourly)
                {
                    throw new InvalidOperationException("Cannot export appointments with hourly recurrence in Outlook compatible mode.");
                }

                if (rrule.Pattern.Frequency == RecurrenceFrequency.Daily)
                {
                    if (rrule.Pattern.DaysOfWeekMask != RecurrenceDay.EveryDay &&
                        rrule.Pattern.DaysOfWeekMask != RecurrenceDay.WeekDays)
                    {
                        throw new InvalidOperationException("Cannot export appointments with daily recurrence and custom DaysOfWeekMask in Outlook compatible mode.");
                    }

                    if (rrule.Pattern.Interval != 1)
                    {
                        throw new InvalidOperationException("Cannot export appointments with daily recurrence and interval different than 1 in Outlook compatible mode.");
                    }
                }
            }
        }

        private static void WriteTask(string description, string location, StringBuilder output, Appointment app, bool outlookCompatibleMode, TimeSpan timeZoneOffset)
        {
            output.AppendLine("BEGIN:VEVENT");
            /// output.AppendLine("DESCRIPTION:" + Engage.Util.Utility.RemoveHtmlTags(description, true));
            output.AppendLine("DESCRIPTION:" + description);
            output.AppendLine("LOCATION:" + location);

            if (app.RecurrenceRule != string.Empty)
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

                ConvertRecurrenceRuleToUtc(rrule, timeZoneOffset);
                output.Append(rrule.ToString());
            }
            else
            {
                output.AppendFormat("DTSTART:{0}\r\n", FormatDate(ClientToUtc(app.Start, timeZoneOffset)));
                output.AppendFormat("DTEND:{0}\r\n", FormatDate(ClientToUtc(app.End, timeZoneOffset)));
            }

            if (outlookCompatibleMode)
            {
                output.AppendFormat("UID:{0}-{1}\r\n", FormatDate(DateTime.Now.ToUniversalTime()), app.ID);
                output.AppendFormat("DTSTAMP:{0}\r\n", FormatDate(DateTime.Now.ToUniversalTime()));
            }

            string summary = app.Subject.Replace("\r\n", "\\n");
            summary = summary.Replace("\n", "\\n");
            output.AppendFormat("SUMMARY:{0}\r\n", summary);
            output.AppendLine("END:VEVENT");
        }

        private static void WriteFileHeader(StringBuilder output, bool outlookCompatibleMode)
        {
            output.AppendLine("BEGIN:VCALENDAR");
            output.AppendLine("VERSION:2.0");
            //output.AppendLine("PRODID:-//Telerik Inc.//NONSGML RadScheduler//EN");
            output.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");

            if (outlookCompatibleMode)
            {
                output.AppendLine("METHOD:PUBLISH");
            }
        }

        private static void WriteFileFooter(StringBuilder output)
        {
            output.AppendLine("END:VCALENDAR");
        }

        private static DateTime ClientToUtc(DateTime date, TimeSpan offset)
        {
            return new DateTime(date.Add(-offset).Ticks, DateTimeKind.Utc);
        }

        private static string FormatDate(DateTime date)
        {
            return date.ToString(DateFormat);
        }

        private static void ConvertRecurrenceRuleToUtc(RecurrenceRule rrule, TimeSpan offset)
        {
            rrule.Range.Start = ClientToUtc(rrule.Range.Start, offset);

            if (rrule.Range.RecursUntil < DateTime.MaxValue)
            {
                rrule.Range.RecursUntil = ClientToUtc(rrule.Range.RecursUntil, offset);
            }

            for (int i = 0; i < rrule.Exceptions.Count; i++)
            {
                rrule.Exceptions[i] = ClientToUtc(rrule.Exceptions[i], offset);
            }
        }
    }
}
