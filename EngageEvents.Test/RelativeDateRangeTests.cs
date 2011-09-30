// <copyright file="RelativeDateRangeTests.cs" company="Engage Software">
// Engage: Events Tests
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Engage.Annotations;
    using Engage.Dnn.Events;

    using NUnit.Framework;

// ReSharper disable UnusedMember.Global
    [TestFixture]
    public class RelativeDateRangeTests : AssertionHelper
    {
        [UsedImplicitly]
        private static readonly IEnumerable<DateTime> SampleDates = new[] { new DateTime(2008, 12, 22), new DateTime(2006, 8, 5), new DateTime(2012, 2, 29), new DateTime(2011, 1, 1), new DateTime(2051, 12, 31) };
        [UsedImplicitly]
        private static readonly IEnumerable<DateTime> DayBeforeSampleDates = new[] { new DateTime(2008, 12, 21), new DateTime(2006, 8, 4), new DateTime(2012, 2, 28), new DateTime(2010, 12, 31), new DateTime(2051, 12, 30) };
        [UsedImplicitly]
        private static readonly IEnumerable<DateTime> DayAfterSampleDates = new[] { new DateTime(2008, 12, 23), new DateTime(2006, 8, 6), new DateTime(2012, 3, 1), new DateTime(2011, 1, 2), new DateTime(2052, 1, 1) };

        [Test]
        public void CanCalculateRelativeStartDateForToday([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Day);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(today), "A relative range of zero days should always produce today");
        }

        [Test]
        public void CanCalculateRelativeEndDateForToday([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Day);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(today), "A relative range of zero days should always produce today");
        }

        [Test, Sequential]
        public void CanCalculateRelativeStartDateForYesterday([ValueSource("SampleDates")]DateTime today, [ValueSource("DayBeforeSampleDates")]DateTime yesterday)
        {
            var startBound = DateRangeBound.CreateRelativeBound(-1, DateInterval.Day);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(yesterday), "A relative range of -1 day should always produce the day before 'today'");
        }

        [Test, Sequential]
        public void CanCalculateRelativeEndDateForYesterday([ValueSource("SampleDates")]DateTime today, [ValueSource("DayBeforeSampleDates")]DateTime yesterday)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(-1, DateInterval.Day);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(yesterday), "A relative range of -1 day should always produce the day before 'today'");
        }

        [Test, Sequential]
        public void CanCalculateRelativeStartDateForTomorrow([ValueSource("SampleDates")]DateTime today, [ValueSource("DayAfterSampleDates")]DateTime tomorrow)
        {
            var startBound = DateRangeBound.CreateRelativeBound(1, DateInterval.Day);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(tomorrow), "A relative range of 1 day should always produce the day after 'today'");
        }

        [Test, Sequential]
        public void CanCalculateRelativeEndDateForTomorrow([ValueSource("SampleDates")]DateTime today, [ValueSource("DayAfterSampleDates")]DateTime tomorrow)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(1, DateInterval.Day);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(tomorrow), "A relative range of 1 day should always produce the day after 'today'");
        }

        [Test]
        public void CanCalculateRelativeStartDateForThisMonth([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Month);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            var startDate = range.GetStartDate(today);

            Expect(startDate.HasValue, Is.True, "Should have a start date");
            Expect(startDate.Value.Year, Is.EqualTo(today.Year), "A relative range of zero months should always produce the same year");
            Expect(startDate.Value.Month, Is.EqualTo(today.Month), "A relative range of zero months should always produce the same month");
            Expect(startDate.Value.Day, Is.EqualTo(1), "A starting relative range of zero months should produce the first day of the month");
        }

        [Test]
        public void CanCalculateRelativeEndDateForThisMonth([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Month);
            var range = new DateRange(startBound, endBound);

            var endDate = range.GetEndDate(today);

            Expect(endDate.HasValue, Is.True, "Should have a start date");
            Expect(endDate.Value.Year, Is.EqualTo(today.Year), "A relative range of zero months should always produce the same year");
            Expect(endDate.Value.Month, Is.EqualTo(today.Month), "A relative range of zero months should always produce the same month");
            Expect(endDate.Value.Day, Is.EqualTo(GetLastDayOfMonth(today).Day), "An ending relative range of zero months should always produce the last day of 'today's' month");
        }

        [Test, Sequential]
        public void CanCalculateRelativeStartDateForLastMonth([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateRelativeBound(-1, DateInterval.Month);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            var startDate = range.GetStartDate(today);

            Expect(startDate.HasValue, Is.True, "Should have a start date");
            Expect(startDate.Value.Year, Is.EqualTo(today.AddMonths(-1).Year), "A relative range of -1 month should always produce the month before 'today'");
            Expect(startDate.Value.Month, Is.EqualTo(today.AddMonths(-1).Month), "A relative range of -1 month should always produce the month before 'today'");
            Expect(startDate.Value.Day, Is.EqualTo(1), "A starting relative range of -1 month should always produce the first day of the month");
        }

        [Test, Sequential]
        public void CanCalculateRelativeEndDateForLastMonth([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(-1, DateInterval.Month);
            var range = new DateRange(startBound, endBound);

            var endDate = range.GetEndDate(today);

            Expect(endDate.HasValue, Is.True, "Should have a start date");
            Expect(endDate.Value.Year, Is.EqualTo(today.AddMonths(-1).Year), "An relative range of -1 month should always produce the month before 'today'");
            Expect(endDate.Value.Month, Is.EqualTo(today.AddMonths(-1).Month), "A relative range of -1 month should always produce the month before 'today'");
            Expect(endDate.Value.Day, Is.EqualTo(GetLastDayOfMonth(today.AddMonths(-1)).Day), "An ending relative range of -1 month should always produce the last day of the month before 'today'");
        }

        [Test, Sequential]
        public void CanCalculateRelativeStartDateForNextMonth([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateRelativeBound(1, DateInterval.Month);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            var startDate = range.GetStartDate(today);

            Expect(startDate.HasValue, Is.True, "Should have a start date");
            Expect(startDate.Value.Year, Is.EqualTo(today.AddMonths(1).Year), "A relative range of 1 month should always produce the month after 'today'");
            Expect(startDate.Value.Month, Is.EqualTo(today.AddMonths(1).Month), "A relative range of 1 month should always produce the month after 'today'");
            Expect(startDate.Value.Day, Is.EqualTo(1), "A starting relative range of 1 month should always produce the first day of the month");
        }

        [Test, Sequential]
        public void CanCalculateRelativeEndDateForNextMonth([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(1, DateInterval.Month);
            var range = new DateRange(startBound, endBound);

            var endDate = range.GetEndDate(today);

            Expect(endDate.HasValue, Is.True, "Should have a start date");
            Expect(endDate.Value.Year, Is.EqualTo(today.AddMonths(1).Year), "A relative range of 1 month should always produce the month after 'today'");
            Expect(endDate.Value.Month, Is.EqualTo(today.AddMonths(1).Month), "A relative range of 1 month should always produce the month after 'today'");
            Expect(endDate.Value.Day, Is.EqualTo(GetLastDayOfMonth(today.AddMonths(1)).Day), "A relative range of 1 month should always produce the last day of the month after 'today'");
        }

        [Test]
        public void CanCalculateRelativeStartDateForThisYear([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Year);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            var startDate = range.GetStartDate(today);

            Expect(startDate.HasValue, Is.True, "Should have a start date");
            Expect(startDate.Value.Year, Is.EqualTo(today.Year), "A relative range of zero years should always produce the same year");
            Expect(startDate.Value.Month, Is.EqualTo(1), "A starting relative range of years should always produce January");
            Expect(startDate.Value.Day, Is.EqualTo(1), "A starting relative range of years should produce January 1");
        }

        [Test]
        public void CanCalculateRelativeEndDateForThisYear([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Year);
            var range = new DateRange(startBound, endBound);

            var endDate = range.GetEndDate(today);

            Expect(endDate.HasValue, Is.True, "Should have a start date");
            Expect(endDate.Value.Year, Is.EqualTo(today.Year), "A relative range of zero years should always produce the same year");
            Expect(endDate.Value.Month, Is.EqualTo(12), "An ending relative range of years should always produce December");
            Expect(endDate.Value.Day, Is.EqualTo(31), "An ending relative range of years should always produce December 31");
        }

        [Test, Sequential]
        public void CanCalculateRelativeStartDateForLastYear([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateRelativeBound(-1, DateInterval.Year);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            var startDate = range.GetStartDate(today);

            Expect(startDate.HasValue, Is.True, "Should have a start date");
            Expect(startDate.Value.Year, Is.EqualTo(today.AddYears(-1).Year), "A relative range of -1 years should always produce the year before 'today'");
            Expect(startDate.Value.Month, Is.EqualTo(1), "A starting relative range of years should always produce January");
            Expect(startDate.Value.Day, Is.EqualTo(1), "A starting relative range of years should always produce January 1");
        }

        [Test, Sequential]
        public void CanCalculateRelativeEndDateForLastYear([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(-1, DateInterval.Year);
            var range = new DateRange(startBound, endBound);

            var endDate = range.GetEndDate(today);

            Expect(endDate.HasValue, Is.True, "Should have a start date");
            Expect(endDate.Value.Year, Is.EqualTo(today.AddYears(-1).Year), "A relative range of -1 year should always produce the year before 'today'");
            Expect(endDate.Value.Month, Is.EqualTo(12), "An ending relative range of years should always produce December");
            Expect(endDate.Value.Day, Is.EqualTo(31), "An ending relative range of years should always produce December 31");
        }

        [Test, Sequential]
        public void CanCalculateRelativeStartDateForNextYear([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateRelativeBound(1, DateInterval.Year);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            var startDate = range.GetStartDate(today);

            Expect(startDate.HasValue, Is.True, "Should have a start date");
            Expect(startDate.Value.Year, Is.EqualTo(today.AddYears(1).Year), "A relative range of 1 year should always produce the year after 'today'");
            Expect(startDate.Value.Month, Is.EqualTo(1), "A starting relative range of years should always produce January");
            Expect(startDate.Value.Day, Is.EqualTo(1), "A starting relative range of years should always produce January 1");
        }

        [Test, Sequential]
        public void CanCalculateRelativeEndDateForNextYear([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateRelativeBound(1, DateInterval.Year);
            var range = new DateRange(startBound, endBound);

            var endDate = range.GetEndDate(today);

            Expect(endDate.HasValue, Is.True, "Should have a start date");
            Expect(endDate.Value.Year, Is.EqualTo(today.AddYears(1).Year), "A relative range of 1 year should always produce the year after 'today'");
            Expect(endDate.Value.Month, Is.EqualTo(12), "An ending relative range of years should always produce December");
            Expect(endDate.Value.Day, Is.EqualTo(31), "An ending relative range of years should always produce December 31");
        }

        private static DateTime GetLastDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.AddMonths(1).Month, 1).AddDays(-1);
        }
    }
// ReSharper restore UnusedMember.Global
}