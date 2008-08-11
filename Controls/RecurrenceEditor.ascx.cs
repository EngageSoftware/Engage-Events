// <copyright file="RecurrenceEditor.ascx.cs" company="Engage Software">
// Engage: Events - http://www.engagemodules.com
// Copyright (c) 2004-2008
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Recurrence
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Localization;
    using Telerik.Web.UI;

    /// <summary>
    /// Displays the possible types of recurrence available when editing an event.
    /// </summary>
    public partial class RecurrenceEditor : ModuleBase
    {
        /// <summary>
        /// The list of possible day selections, from <see cref="RecurrenceDay"/>.
        /// </summary>
        private static readonly ListItem[] DayMaskItems = 
        {
            new ListItem(RecurrenceDay.EveryDay.ToString()), 
            new ListItem(RecurrenceDay.WeekDays.ToString()),
            new ListItem(RecurrenceDay.WeekendDays.ToString()), 
            new ListItem(RecurrenceDay.Sunday.ToString()),
            new ListItem(RecurrenceDay.Monday.ToString()), 
            new ListItem(RecurrenceDay.Tuesday.ToString()),
            new ListItem(RecurrenceDay.Wednesday.ToString()), 
            new ListItem(RecurrenceDay.Thursday.ToString()),
            new ListItem(RecurrenceDay.Friday.ToString()), 
            new ListItem(RecurrenceDay.Saturday.ToString())
        };

        /// <summary>
        /// The list of possible months selections, from <see cref="RecurrenceMonth"/>.
        /// </summary>
        private static readonly ListItem[] MonthItems = 
        {
            new ListItem(RecurrenceMonth.January.ToString()), 
            new ListItem(RecurrenceMonth.February.ToString()),
            new ListItem(RecurrenceMonth.March.ToString()), 
            new ListItem(RecurrenceMonth.April.ToString()),
            new ListItem(RecurrenceMonth.May.ToString()), 
            new ListItem(RecurrenceMonth.June.ToString()),
            new ListItem(RecurrenceMonth.July.ToString()), 
            new ListItem(RecurrenceMonth.August.ToString()),
            new ListItem(RecurrenceMonth.September.ToString()), 
            new ListItem(RecurrenceMonth.October.ToString()),
            new ListItem(RecurrenceMonth.November.ToString()), 
            new ListItem(RecurrenceMonth.December.ToString())
        };

        /// <summary>
        /// The list of possible ordinal selections, for example, First, Second or Last.  Based on <see cref="RecurrencePattern.DayOrdinal"/>.
        /// </summary>
        private static readonly ListItem[] OrdinalItems = 
        {
            new ListItem("First", "1"), 
            new ListItem("Second", "2"), 
            new ListItem("Third", "3"),
            new ListItem("Fourth", "4"), 
            new ListItem("Last", "-1")
        };

        /// <summary>
        /// Gets a value indicating whether the this control is in a valid state.
        /// </summary>
        /// <remarks>
        /// Checks whether the currently selected recurrence rule is defined in part by any <see cref="RadNumericTextBox"/>es.  
        /// If so, it makes sure that the <see cref="RadNumericTextBox"/> has a value.
        /// </remarks>
        /// <value><c>true</c> if this instance is in a valid state; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                bool isValid;
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Weekly:
                        isValid = this.WeeklyRepeatInterval.Value.HasValue;
                        break;
                    case RecurrenceFrequency.Monthly:
                        if (this.RepeatEveryNthMonthOnDate.Checked)
                        {
                            isValid = this.MonthlyRepeatDate.Value.HasValue && this.MonthlyRepeatIntervalForDate.Value.HasValue;
                        }
                        else
                        {
                            isValid = this.MonthlyRepeatIntervalForGivenDay.Value.HasValue;
                        }

                        break;
                    case RecurrenceFrequency.Yearly:
                        if (this.RepeatEveryYearOnDate.Checked)
                        {
                            isValid = this.YearlyRepeatDate.Value.HasValue;
                        }
                        else
                        {
                            isValid = true;
                        }

                        break;
                    ////case RecurrenceFrequency.Daily:
                    default:
                        if (this.RepeatEveryNthDay.Checked)
                        {
                            isValid = this.DailyRepeatInterval.Value.HasValue;
                        }
                        else
                        {
                            isValid = true;
                        }

                        break;
                }

                if (isValid && this.RepeatGivenOccurrences.Checked)
                {
                    isValid = this.RangeOccurrences.Value.HasValue;
                }

                return isValid;
            }
        }

        /// <summary>
        /// Gets or sets the skin name for the interface of the <see cref="RadDatePicker"/> in this control.
        /// </summary>
        /// <value>The skin name for the interface of the <see cref="RadDatePicker"/> in this control</value>
        public string DatePickerSkin
        {
            get
            {
                return this.RangeEndDate.Skin;
            }

            set
            {
                this.RangeEndDate.Skin = value;
            }
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        private RecurrenceFrequency Frequency
        {
            get
            {
                if (this.Visible)
                {
                    if (this.RepeatFrequencyDaily.Checked)
                    {
                        return RecurrenceFrequency.Daily;
                    }

                    if (this.RepeatFrequencyWeekly.Checked)
                    {
                        return RecurrenceFrequency.Weekly;
                    }

                    if (this.RepeatFrequencyMonthly.Checked)
                    {
                        return RecurrenceFrequency.Monthly;
                    }

                    if (this.RepeatFrequencyYearly.Checked)
                    {
                        return RecurrenceFrequency.Yearly;
                    }
                }

                return RecurrenceFrequency.None;
            }
        }

        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>The interval.</value>
        private int Interval
        {
            get
            {
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Daily:
                        if (this.RepeatEveryNthDay.Checked)
                        {
                            return (int)this.DailyRepeatInterval.Value.Value;
                        }

                        break;
                    case RecurrenceFrequency.Weekly:
                        return (int)this.WeeklyRepeatInterval.Value.Value;
                    case RecurrenceFrequency.Monthly:
                        if (this.RepeatEveryNthMonthOnDate.Checked)
                        {
                            return (int)this.MonthlyRepeatIntervalForDate.Value.Value;
                        }

                        return (int)this.MonthlyRepeatIntervalForGivenDay.Value.Value;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the days of week mask.
        /// </summary>
        /// <value>The days of week mask.</value>
        private RecurrenceDay DaysOfWeekMask
        {
            get
            {
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Daily:
                        return this.RepeatEveryWeekday.Checked ? RecurrenceDay.WeekDays : RecurrenceDay.EveryDay;

                    case RecurrenceFrequency.Weekly:
                        RecurrenceDay finalMask = RecurrenceDay.None;
                        finalMask |= this.WeeklyWeekDayMonday.Checked ? RecurrenceDay.Monday : finalMask;
                        finalMask |= this.WeeklyWeekDayTuesday.Checked ? RecurrenceDay.Tuesday : finalMask;
                        finalMask |= this.WeeklyWeekDayWednesday.Checked ? RecurrenceDay.Wednesday : finalMask;
                        finalMask |= this.WeeklyWeekDayThursday.Checked ? RecurrenceDay.Thursday : finalMask;
                        finalMask |= this.WeeklyWeekDayFriday.Checked ? RecurrenceDay.Friday : finalMask;
                        finalMask |= this.WeeklyWeekDaySaturday.Checked ? RecurrenceDay.Saturday : finalMask;
                        finalMask |= this.WeeklyWeekDaySunday.Checked ? RecurrenceDay.Sunday : finalMask;

                        return finalMask;

                    case RecurrenceFrequency.Monthly:
                        if (this.RepeatEveryNthMonthOnGivenDay.Checked)
                        {
                            return (RecurrenceDay)Enum.Parse(typeof(RecurrenceDay), this.MonthlyDayMaskDropDown.SelectedValue);
                        }

                        break;
                    case RecurrenceFrequency.Yearly:
                        if (this.RepeatEveryYearOnGivenDay.Checked)
                        {
                            return (RecurrenceDay)Enum.Parse(typeof(RecurrenceDay), this.YearlyDayMaskDropDown.SelectedValue);
                        }

                        break;
                }

                return RecurrenceDay.None;
            }
        }

        /// <summary>
        /// Gets the day of month.
        /// </summary>
        /// <value>The day of month.</value>
        private int DayOfMonth
        {
            get
            {
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Monthly:
                        return (this.RepeatEveryNthMonthOnDate.Checked ? (int)this.MonthlyRepeatDate.Value.Value : 0);

                    case RecurrenceFrequency.Yearly:
                        return (this.RepeatEveryYearOnDate.Checked ? (int)this.YearlyRepeatDate.Value.Value : 0);
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the day ordinal.
        /// </summary>
        /// <value>The day ordinal.</value>
        private int DayOrdinal
        {
            get
            {
                switch (this.Frequency)
                {
                    case RecurrenceFrequency.Monthly:
                        if (this.RepeatEveryNthMonthOnGivenDay.Checked)
                        {
                            return int.Parse(this.MonthlyDayOrdinalDropDown.SelectedValue);
                        }

                        break;
                    case RecurrenceFrequency.Yearly:
                        if (this.RepeatEveryYearOnGivenDay.Checked)
                        {
                            return int.Parse(this.YearlyDayOrdinalDropDown.SelectedValue);
                        }

                        break;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the month.
        /// </summary>
        /// <value>The month.</value>
        private RecurrenceMonth Month
        {
            get
            {
                if (this.Frequency == RecurrenceFrequency.Yearly)
                {
                    string selectedMonth = this.RepeatEveryYearOnDate.Checked
                                               ? this.YearlyRepeatMonthForDate.SelectedValue
                                               : this.YearlyRepeatMonthForGivenDay.SelectedValue;

                    return (RecurrenceMonth)Enum.Parse(typeof(RecurrenceMonth), selectedMonth);
                }

                return RecurrenceMonth.None;
            }
        }

        /// <summary>
        /// Gets the pattern.
        /// </summary>
        /// <value>The pattern.</value>
        private RecurrencePattern Pattern
        {
            get
            {
                if (!this.Visible)
                {
                    return null;
                }

                RecurrencePattern submittedPattern = new RecurrencePattern();
                submittedPattern.Frequency = this.Frequency;
                submittedPattern.Interval = this.Interval;
                submittedPattern.DaysOfWeekMask = this.DaysOfWeekMask;
                submittedPattern.DayOfMonth = this.DayOfMonth;
                submittedPattern.DayOrdinal = this.DayOrdinal;
                submittedPattern.Month = this.Month;

                if (submittedPattern.Frequency == RecurrenceFrequency.Weekly)
                {
                    submittedPattern.FirstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                }

                return submittedPattern;
            }
        }

        /// <summary>
        /// Gets the recurrence rule for this control instance.
        /// </summary>
        /// <param name="startDate">The start date of the event in question.</param>
        /// <param name="endDate">The end date of the event in question.</param>
        /// <returns>The recurrence rule for the event in question</returns>
        internal RecurrenceRule GetRecurrenceRule(DateTime startDate, DateTime endDate)
        {
            return RecurrenceRule.FromPatternAndRange(this.Pattern, this.GetRange(startDate, endDate));
        }

        /// <summary>
        /// Sets the recurrence rule for this control instance.
        /// </summary>
        /// <param name="rule">The recurrence rule with which to populate this control.</param>
        internal void SetRecurrenceRule(RecurrenceRule rule)
        {
            if (rule != null)
            {
                this.SetRecurrencePattern(rule.Pattern);
                this.SetRecurrenceRange(rule.Range);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.RepeatFrequencyDaily.CheckedChanged += this.RepeatFrequency_CheckedChanged;
            this.RepeatFrequencyWeekly.CheckedChanged += this.RepeatFrequency_CheckedChanged;
            this.RepeatFrequencyMonthly.CheckedChanged += this.RepeatFrequency_CheckedChanged;
            this.RepeatFrequencyYearly.CheckedChanged += this.RepeatFrequency_CheckedChanged;

            this.LocalResourceFile = this.AppRelativeTemplateSourceDirectory + Localization.LocalResourceDirectory + "/" + Path.GetFileNameWithoutExtension(this.TemplateControl.AppRelativeVirtualPath);

            // We need to call this before the Load event, since SetRecurrencePattern depends on it.
            if (!this.IsPostBack)
            {
                this.FillDropDowns();
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.SetRecurrenceFrequency();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the any of the RepeatFrequency controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RepeatFrequency_CheckedChanged(object sender, EventArgs e)
        {
            this.SetRecurrenceFrequency();
        }

        /// <summary>
        /// Sets the active control based on the current recurrence frequency.
        /// </summary>
        private void SetRecurrenceFrequency()
        {
            View activeView;
            switch (this.Frequency)
            {
                case RecurrenceFrequency.Weekly:
                    activeView = this.RecurrencePatternWeeklyView;
                    break;
                case RecurrenceFrequency.Monthly:
                    activeView = this.RecurrencePatternMonthlyView;
                    break;
                case RecurrenceFrequency.Yearly:
                    activeView = this.RecurrencePatternYearlyView;
                    break;
                ////case RecurrenceFrequency.Daily:
                default:
                    activeView = this.RecurrencePatternDailyView;
                    break;
            }

            this.RecurrencePatternMultiView.SetActiveView(activeView);
        }

        /// <summary>
        /// Sets the pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        private void SetRecurrencePattern(RecurrencePattern pattern)
        {
            this.InitializeRecurrenceFrequency();

            switch (pattern.Frequency)
            {
                case RecurrenceFrequency.Weekly:
                    this.SetupWeeklyRecurrence(pattern);
                    break;
                case RecurrenceFrequency.Monthly:
                    this.SetupMonthlyRecurrence(pattern);
                    break;
                case RecurrenceFrequency.Yearly:
                    this.SetupYearlyRecurrence(pattern);
                    break;
                ////case RecurrenceFrequency.Daily:
                default:
                    this.SetupDailyRecurrence(pattern);
                    break;
            }
        }

        /// <summary>
        /// Clears the selection state of the recurrence frequency <see cref="RadioButton"/>s, so that it can be accurately reset.  
        /// </summary>
        private void InitializeRecurrenceFrequency()
        {
            this.RepeatFrequencyDaily.Checked
                = this.RepeatFrequencyWeekly.Checked
                = this.RepeatFrequencyMonthly.Checked
                = this.RepeatFrequencyYearly.Checked
                = false;
        }

        /// <summary>
        /// Sets up the controls in <see cref="RecurrencePatternDailyView"/>.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        private void SetupDailyRecurrence(RecurrencePattern pattern)
        {
            this.RepeatFrequencyDaily.Checked = true;
            this.RepeatEveryWeekday.Checked = (pattern.DaysOfWeekMask == RecurrenceDay.WeekDays);
            this.RepeatEveryNthDay.Checked = !this.RepeatEveryWeekday.Checked;
            if (this.RepeatEveryNthDay.Checked)
            {
                this.DailyRepeatInterval.Value = pattern.Interval;
            }
        }

        /// <summary>
        /// Sets up the controls in <see cref="RecurrencePatternWeeklyView"/>.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        private void SetupWeeklyRecurrence(RecurrencePattern pattern)
        {
            this.RepeatFrequencyWeekly.Checked = true;

            this.WeeklyRepeatInterval.Value = pattern.Interval;
            this.WeeklyWeekDayMonday.Checked = (pattern.DaysOfWeekMask & RecurrenceDay.Monday) != 0;
            this.WeeklyWeekDayTuesday.Checked = (pattern.DaysOfWeekMask & RecurrenceDay.Tuesday) != 0;
            this.WeeklyWeekDayWednesday.Checked = (pattern.DaysOfWeekMask & RecurrenceDay.Wednesday) != 0;
            this.WeeklyWeekDayThursday.Checked = (pattern.DaysOfWeekMask & RecurrenceDay.Thursday) != 0;
            this.WeeklyWeekDayFriday.Checked = (pattern.DaysOfWeekMask & RecurrenceDay.Friday) != 0;
            this.WeeklyWeekDaySaturday.Checked = (pattern.DaysOfWeekMask & RecurrenceDay.Saturday) != 0;
            this.WeeklyWeekDaySunday.Checked = (pattern.DaysOfWeekMask & RecurrenceDay.Sunday) != 0;
        }

        /// <summary>
        /// Sets up the controls in <see cref="RecurrencePatternMonthlyView"/>.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        private void SetupMonthlyRecurrence(RecurrencePattern pattern)
        {
            this.RepeatFrequencyMonthly.Checked = true;

            this.RepeatEveryNthMonthOnDate.Checked = pattern.DayOfMonth > 0;
            this.RepeatEveryNthMonthOnGivenDay.Checked = !this.RepeatEveryNthMonthOnDate.Checked;

            if (this.RepeatEveryNthMonthOnDate.Checked)
            {
                this.MonthlyRepeatDate.Value = pattern.DayOfMonth;
                this.MonthlyRepeatIntervalForDate.Value = pattern.Interval;
            }
            else
            {
                this.MonthlyDayOrdinalDropDown.SelectedValue = pattern.DayOrdinal.ToString(CultureInfo.CurrentCulture);
                this.MonthlyDayMaskDropDown.SelectedValue = pattern.DaysOfWeekMask.ToString();
                this.MonthlyRepeatIntervalForGivenDay.Value = pattern.Interval;
            }
        }

        /// <summary>
        /// Sets up the controls in <see cref="RecurrencePatternYearlyView"/>.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        private void SetupYearlyRecurrence(RecurrencePattern pattern)
        {
            this.RepeatFrequencyYearly.Checked = true;

            this.RepeatEveryYearOnDate.Checked = pattern.DayOfMonth > 0;
            this.RepeatEveryYearOnGivenDay.Checked = !this.RepeatEveryYearOnDate.Checked;

            if (this.RepeatEveryYearOnDate.Checked)
            {
                this.YearlyRepeatMonthForDate.SelectedValue = pattern.Month.ToString();
                this.YearlyRepeatDate.Value = pattern.DayOfMonth;
            }
            else
            {
                this.YearlyDayOrdinalDropDown.SelectedValue = pattern.DayOrdinal.ToString(CultureInfo.CurrentCulture);
                this.YearlyDayMaskDropDown.SelectedValue = pattern.DaysOfWeekMask.ToString();
                this.YearlyRepeatMonthForGivenDay.SelectedValue = pattern.Month.ToString();
            }
        }

        /// <summary>
        /// Sets the range.
        /// </summary>
        /// <param name="range">The range.</param>
        private void SetRecurrenceRange(RecurrenceRange range)
        {
            this.RepeatIndefinitely.Checked = this.RepeatGivenOccurrences.Checked = this.RepeatUntilGivenDate.Checked = false;
            if (range.MaxOccurrences != int.MaxValue)
            {
                this.RepeatGivenOccurrences.Checked = true;
                this.RangeOccurrences.Value = range.MaxOccurrences;
            }
            else if (range.RecursUntil != DateTime.MaxValue)
            {
                this.RepeatUntilGivenDate.Checked = true;

                // TODO: only remove a day if it isn't an all day event, if we start to support all day events.  BD
                this.RangeEndDate.SelectedDate = range.RecursUntil.AddDays(-1);
            }
            else
            {
                this.RepeatIndefinitely.Checked = true;
            }
        }

        /// <summary>
        /// Gets the recurrence range for this control.
        /// </summary>
        /// <param name="startDate">The start date of the event in question.</param>
        /// <param name="endDate">The end date of the event in question.</param>
        /// <returns>The recurrence range for the event in question</returns>
        private RecurrenceRange GetRange(DateTime startDate, DateTime endDate)
        {
            RecurrenceRange range = new RecurrenceRange();
            range.Start = startDate;
            range.EventDuration = endDate - startDate;
            range.MaxOccurrences = 0;
            range.RecursUntil = DateTime.MaxValue;

            if (this.Visible)
            {
                if (this.RepeatGivenOccurrences.Checked)
                {
                    range.MaxOccurrences = (int)this.RangeOccurrences.Value.Value;
                }
                else if (this.RepeatUntilGivenDate.Checked && this.RangeEndDate.SelectedDate.HasValue)
                {
                    range.RecursUntil = this.RangeEndDate.SelectedDate.Value;

                    ////if (!_allDayEvent.Checked)
                    {
                        range.RecursUntil = range.RecursUntil.AddDays(1);
                    }
                }
            }

            return range;
        }

        /// <summary>
        /// Fills the drop downs on this control.
        /// </summary>
        private void FillDropDowns()
        {
            this.FillListControl(this.MonthlyDayOrdinalDropDown, OrdinalItems);
            this.FillListControl(this.MonthlyDayMaskDropDown, DayMaskItems);
            this.FillListControl(this.YearlyRepeatMonthForDate, MonthItems);
            this.FillListControl(this.YearlyDayOrdinalDropDown, OrdinalItems);
            this.FillListControl(this.YearlyDayMaskDropDown, DayMaskItems);
            this.FillListControl(this.YearlyRepeatMonthForGivenDay, MonthItems);
        }

        /// <summary>
        /// Fills a list with <see cref="ListItem"/>s and localizes the list.
        /// </summary>
        /// <param name="listControl">The list control to fill with the given items.</param>
        /// <param name="listItems">The list items to use when filling the list.</param>
        private void FillListControl(ListControl listControl, ListItem[] listItems)
        {
            listControl.Items.Clear();
            for (int i = 0; i < listItems.Length; i++)
            {
                ListItem listItem = listItems[i];
                listControl.Items.Add(new ListItem(listItem.Text, listItem.Value));
            }

            Utility.LocalizeListControl(listControl, this.LocalResourceFile);
        }
    }
}