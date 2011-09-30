// <copyright file="WindowDateRangeTests.cs" company="Engage Software">
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
    public class WindowDateRangeTests : AssertionHelper
    {
        [UsedImplicitly]
        private static readonly IEnumerable<DateInterval> AllDateInvervals = Enum.GetValues(typeof(DateInterval)).Cast<DateInterval>();
        [UsedImplicitly]
        private static readonly IEnumerable<int> SampleRelativeAmounts = new[] { -90, -60, -30, -3, -2, -1, 0, 1, 2, 3, 30, 60, 90 };
        [UsedImplicitly]
        private static readonly IEnumerable<DateTime> SampleDates = new[] { new DateTime(2008, 12, 22), new DateTime(2006, 8, 5), new DateTime(2012, 2, 29), new DateTime(2011, 1, 1), new DateTime(2051, 12, 31) };

        [Test]
        public void CanCalculateWindowOneDayBeforeSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateWindowBound(1, DateInterval.Day);
            var endBound = DateRangeBound.CreateSpecificDateBound(today);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(DateTime.MinValue), Is.EqualTo(today.AddDays(-1)), "A starting window range of 1 day should always produce one day before the end");
        }

        [Test]
        public void CanCalculateWindowOneDayAfterSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateSpecificDateBound(today);
            var endBound = DateRangeBound.CreateWindowBound(1, DateInterval.Day);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(DateTime.MinValue), Is.EqualTo(today.AddDays(1)), "An ending window range of 1 day should always produce one day after the start");
        }

        [Test]
        public void CanCalculateWindowTenDaysBeforeSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateWindowBound(10, DateInterval.Day);
            var endBound = DateRangeBound.CreateSpecificDateBound(today);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(DateTime.MinValue), Is.EqualTo(today.AddDays(-10)), "A starting window range of 10 days should always produce ten days before the end");
        }

        [Test]
        public void CanCalculateWindowTenDaysAfterSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateSpecificDateBound(today);
            var endBound = DateRangeBound.CreateWindowBound(10, DateInterval.Day);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(DateTime.MinValue), Is.EqualTo(today.AddDays(10)), "An ending window range of 10 days should always produce ten days after the start");
        }

        [Test]
        public void CanCalculateWindowOneMonthBeforeSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateWindowBound(1, DateInterval.Month);
            var endBound = DateRangeBound.CreateSpecificDateBound(today);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(DateTime.MinValue), Is.EqualTo(today.AddMonths(-1)), "A starting window range of 1 month should always produce one month before the end");
        }

        [Test]
        public void CanCalculateWindowOneMonthAfterSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateSpecificDateBound(today);
            var endBound = DateRangeBound.CreateWindowBound(1, DateInterval.Month);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(DateTime.MinValue), Is.EqualTo(today.AddMonths(1)), "An ending window range of 1 month should always produce one month after the start");
        }

        [Test]
        public void CanCalculateWindowTenMonthsBeforeSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateWindowBound(10, DateInterval.Month);
            var endBound = DateRangeBound.CreateSpecificDateBound(today);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(DateTime.MinValue), Is.EqualTo(today.AddMonths(-10)), "A starting window range of 10 months should always produce ten months before the end");
        }

        [Test]
        public void CanCalculateWindowTenMonthsAfterSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateSpecificDateBound(today);
            var endBound = DateRangeBound.CreateWindowBound(10, DateInterval.Month);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(DateTime.MinValue), Is.EqualTo(today.AddMonths(10)), "An ending window range of 10 months should always produce ten months after the start");
        }

        [Test]
        public void CanCalculateWindowOneYearBeforeSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateWindowBound(1, DateInterval.Year);
            var endBound = DateRangeBound.CreateSpecificDateBound(today);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(DateTime.MinValue), Is.EqualTo(today.AddYears(-1)), "A starting window range of 1 year should always produce one year before the end");
        }

        [Test]
        public void CanCalculateWindowOneYearAfterSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateSpecificDateBound(today);
            var endBound = DateRangeBound.CreateWindowBound(1, DateInterval.Year);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(DateTime.MinValue), Is.EqualTo(today.AddYears(1)), "An ending window range of 1 year should always produce one year after the start");
        }

        [Test]
        public void CanCalculateWindowTenYearsBeforeSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateWindowBound(10, DateInterval.Year);
            var endBound = DateRangeBound.CreateSpecificDateBound(today);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(DateTime.MinValue), Is.EqualTo(today.AddYears(-10)), "A starting window range of 10 years should always produce ten years before the end");
        }

        [Test]
        public void CanCalculateWindowTenYearsAfterSpecificDate([ValueSource("SampleDates")]DateTime today)
        {
            var startBound = DateRangeBound.CreateSpecificDateBound(today);
            var endBound = DateRangeBound.CreateWindowBound(10, DateInterval.Year);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(DateTime.MinValue), Is.EqualTo(today.AddYears(10)), "An ending window range of 10 years should always produce ten years after the start");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowOneDayBeforeRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(1, DateInterval.Day);
            var endBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(range.GetEndDate(today).Value.AddDays(-1)), "A starting window range of 1 day should always produce one day before the end");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowOneDayAfterRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var endBound = DateRangeBound.CreateWindowBound(1, DateInterval.Day);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(range.GetStartDate(today).Value.AddDays(1)), "An ending window range of 1 day should always produce one day after the start");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowTenDaysBeforeRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(10, DateInterval.Day);
            var endBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(range.GetEndDate(today).Value.AddDays(-10)), "A starting window range of 10 days should always produce ten days before the end");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowTenDaysAfterRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var endBound = DateRangeBound.CreateWindowBound(10, DateInterval.Day);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(range.GetStartDate(today).Value.AddDays(10)), "An ending window range of 10 days should always produce ten days after the start");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowOneMonthBeforeRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(1, DateInterval.Month);
            var endBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(range.GetEndDate(today).Value.AddMonths(-1)), "A starting window range of 1 month should always produce one month before the end");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowOneMonthAfterRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var endBound = DateRangeBound.CreateWindowBound(1, DateInterval.Month);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(range.GetStartDate(today).Value.AddMonths(1)), "An ending window range of 1 month should always produce one month after the start");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowTenMonthsBeforeRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(10, DateInterval.Month);
            var endBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(range.GetEndDate(today).Value.AddMonths(-10)), "A starting window range of 10 months should always produce ten months before the end");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowTenMonthsAfterRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var endBound = DateRangeBound.CreateWindowBound(10, DateInterval.Month);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(range.GetStartDate(today).Value.AddMonths(10)), "An ending window range of 10 months should always produce ten months after the start");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowOneYearBeforeRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(1, DateInterval.Year);
            var endBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(range.GetEndDate(today).Value.AddYears(-1)), "A starting window range of 1 year should always produce one year before the end");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowOneYearAfterRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var endBound = DateRangeBound.CreateWindowBound(1, DateInterval.Year);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(range.GetStartDate(today).Value.AddYears(1)), "An ending window range of 1 year should always produce one year after the start");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowTenYearsBeforeRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(10, DateInterval.Year);
            var endBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetStartDate(today), Is.EqualTo(range.GetEndDate(today).Value.AddYears(-10)), "A starting window range of 10 years should always produce ten years before the end");
        }

        [Test, Combinatorial]
        public void CanCalculateWindowTenYearsAfterRelativeDate(
            [ValueSource("SampleDates")]DateTime today,
            [ValueSource("SampleRelativeAmounts")]int relativeAmount,
            [ValueSource("AllDateInvervals")]DateInterval relativeInterval)
        {
            var startBound = DateRangeBound.CreateRelativeBound(relativeAmount, relativeInterval);
            var endBound = DateRangeBound.CreateWindowBound(10, DateInterval.Year);
            var range = new DateRange(startBound, endBound);

            Expect(range.GetEndDate(today), Is.EqualTo(range.GetStartDate(today).Value.AddYears(10)), "An ending window range of 10 years should always produce ten years after the start");
        }
    }
// ReSharper restore UnusedMember.Global
}