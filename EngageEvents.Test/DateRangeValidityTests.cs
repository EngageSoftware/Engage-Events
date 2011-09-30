// <copyright file="DateRangeValidityTests.cs" company="Engage Software">
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

    [TestFixture]
    public class DateRangeValidityTests : AssertionHelper
    {
        [UsedImplicitly]
        private static readonly IEnumerable<DateInterval> AllDateInvervals = Enum.GetValues(typeof(DateInterval)).Cast<DateInterval>();
        [UsedImplicitly]
        private static readonly IEnumerable<int> SampleRelativeAmounts = new[] { -90, -60, -30, -3, -2, -1, 0, 1, 2, 3, 30, 60, 90 };
        [UsedImplicitly]
        private static readonly IEnumerable<int> SampleWindowAmounts = new[] { 1, 2, 3, 30, 60, 90 };
        [UsedImplicitly]
        private static readonly IEnumerable<DateTime> SampleDates = new[] { new DateTime(2008, 12, 22), new DateTime(2006, 8, 5), new DateTime(2012, 2, 29), new DateTime(2011, 1, 1), new DateTime(2051, 12, 31) };

        [Test, Combinatorial]
        public void TwoWindowsAreInvalid(
            [ValueSource("SampleWindowAmounts")]int startAmount, 
            [ValueSource("AllDateInvervals")]DateInterval startInterval, 
            [ValueSource("SampleWindowAmounts")]int endAmount,
            [ValueSource("AllDateInvervals")]DateInterval endInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(startAmount, startInterval);
            var endBound = DateRangeBound.CreateWindowBound(endAmount, endInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.ErrorMessage, Is.EqualTo("Both Window"), "A range from window to window is always invalid");
        }

        [Test, Combinatorial]
        public void StartWindowIsInvalidWithUnboundedEnd(
            [ValueSource("SampleWindowAmounts")]int startAmount, 
            [ValueSource("AllDateInvervals")]DateInterval startInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(startAmount, startInterval);
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            this.Expect(range.ErrorMessage, Is.EqualTo("Window to End of Time"), "A range from window to unbounded is always invalid");
        }

        [Test, Combinatorial]
        public void StartWindowIsValidWithEndingRelative(
            [ValueSource("SampleWindowAmounts")]int startAmount, 
            [ValueSource("AllDateInvervals")]DateInterval startInterval, 
            [ValueSource("SampleRelativeAmounts")]int endWindow,
            [ValueSource("AllDateInvervals")]DateInterval endInterval)
        {
            var startBound = DateRangeBound.CreateWindowBound(startAmount, startInterval);
            var endBound = DateRangeBound.CreateRelativeBound(endWindow, endInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.IsValid, Is.True, "A range from window to relative is always valid");
        }

        [Test, Combinatorial]
        public void StartWindowIsValidWithEndingSpecific(
            [ValueSource("SampleWindowAmounts")]int startAmount, 
            [ValueSource("AllDateInvervals")]DateInterval startInterval, 
            [ValueSource("SampleDates")]DateTime endingDate)
        {
            var startBound = DateRangeBound.CreateWindowBound(startAmount, startInterval);
            var endBound = DateRangeBound.CreateSpecificDateBound(endingDate);
            var range = new DateRange(startBound, endBound);

            Expect(range.IsValid, Is.True, "A range from window to specific is always valid");
        }

        [Test, Combinatorial]
        public void EndWindowIsInvalidWithUnboundedStart(
            [ValueSource("SampleWindowAmounts")]int endAmount, 
            [ValueSource("AllDateInvervals")]DateInterval endInterval)
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateWindowBound(endAmount, endInterval);
            var range = new DateRange(startBound, endBound);

            this.Expect(range.ErrorMessage, Is.EqualTo("Window to Beginning of Time"), "A range from unbounded to window is always invalid");
        }

        [Test, Combinatorial]
        public void EndWindowIsValidWithStartingRelative(
            [ValueSource("SampleRelativeAmounts")]int startAmount, 
            [ValueSource("AllDateInvervals")]DateInterval startInterval,
            [ValueSource("SampleWindowAmounts")]int endAmount,
            [ValueSource("AllDateInvervals")]DateInterval endInterval)
        {
            var startBound = DateRangeBound.CreateRelativeBound(startAmount, startInterval);
            var endBound = DateRangeBound.CreateWindowBound(endAmount, endInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.IsValid, Is.True, "A range from relative to window is always valid");
        }

        [Test, Combinatorial]
        public void EndWindowIsValidWithStartingSpecific(
            [ValueSource("SampleDates")]DateTime starting, 
            [ValueSource("SampleWindowAmounts")]int endAmount, 
            [ValueSource("AllDateInvervals")]DateInterval endInterval)
        {
            var startBound = DateRangeBound.CreateSpecificDateBound(starting);
            var endBound = DateRangeBound.CreateWindowBound(endAmount, endInterval);
            var range = new DateRange(startBound, endBound);

            Expect(range.IsValid, Is.True, "A range from specific to window is always valid");
        }

        [Test]
        public void TwoUnboundedAreValid()
        {
            var startBound = DateRangeBound.CreateUnboundedBound();
            var endBound = DateRangeBound.CreateUnboundedBound();
            var range = new DateRange(startBound, endBound);

            Expect(range.IsValid, Is.True, "A range from unbounded to unbounded is always valid");
        }
    }
}