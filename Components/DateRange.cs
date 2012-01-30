// <copyright file="DateRange.cs" company="Engage Software">
// Engage: Events
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

    /// <summary>
    /// A range of dates
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// Backing field for <see cref="ErrorMessage"/>
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange"/> class.
        /// </summary>
        /// <param name="start">The starting bound of the date range.</param>
        /// <param name="end">The ending bound of the date range.</param>
        public DateRange(DateRangeBound start, DateRangeBound end)
        {
            if (start == null)
            {
                throw new ArgumentNullException("start");
            }

            if (end == null)
            {
                throw new ArgumentNullException("end");
            }

            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Gets the range's starting bound.
        /// </summary>
        public DateRangeBound Start { get; private set; }

        /// <summary>
        /// Gets the ranges' ending bound.
        /// </summary>
        public DateRangeBound End { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance represents a valid range.
        /// </summary>
        /// <remarks>
        /// We're considering ranges invalid only if they cannot be represented as two dates.
        /// A range will still be "valid" if the start date can occur after the end date.
        /// </remarks>
        /// <value>
        /// <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get { return string.IsNullOrEmpty(this.ErrorMessage); }
        }

        /// <summary>
        /// Gets the error message for an invalid range, or <see cref="string.Empty"/> if the range is valid.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                if (this.errorMessage == null)
                {
                    this.errorMessage = this.GetErrorMessage();
                }

                return this.errorMessage;
            }
        }

        /// <summary>
        /// Gets the start date for this range today.
        /// </summary>
        /// <returns>The range's starting date for today</returns>
        /// <remarks>Make sure to check <see cref="IsValid"/> before calling this; an invalid will go into an infinite loop</remarks>
        public DateTime? GetStartDate()
        {
            return this.GetStartDate(DateTime.Today);
        }

        /// <summary>
        /// Gets the start date for this range today, in UTC.
        /// </summary>
        /// <returns>The range's starting date for today, in UTC</returns>
        /// <remarks>Make sure to check <see cref="IsValid"/> before calling this; an invalid will go into an infinite loop</remarks>
        public DateTime? GetStartDateUtc()
        {
            var startDate = this.GetStartDate();
            DateTime? startDateUtc = null;
            if (startDate.HasValue)
            {
                startDateUtc = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(startDate.Value, DateTimeKind.Unspecified), Dnn.Utility.GetUserTimeZone());
            }

            return startDateUtc;
        }

        /// <summary>
        /// Gets the start date for this range based on the given base date.
        /// </summary>
        /// <param name="relativeBaseDate">The base date from which relative dates should be calculated.</param>
        /// <returns>The range's starting date</returns>
        /// <remarks>Make sure to check <see cref="IsValid"/> before calling this; an invalid will go into an infinite loop</remarks>
        public DateTime? GetStartDate(DateTime relativeBaseDate)
        {
// ReSharper disable PossibleInvalidOperationException
            if (this.Start.IsUnbounded)
            {
                return null;
            }

            if (this.Start.IsSpecificDate)
            {
                return this.Start.SpecificDate.Value;
            }

            if (this.Start.IsWindow)
            {
                var otherDate = this.GetEndDate(relativeBaseDate).Value;
                if (this.Start.WindowInterval.Value == DateInterval.Year)
                {
                    return otherDate.AddYears(-1 * this.Start.WindowAmount.Value);
                }

                if (this.Start.WindowInterval.Value == DateInterval.Month)
                {
                    return otherDate.AddMonths(-1 * this.Start.WindowAmount.Value);
                }

                return otherDate.AddDays(-1 * this.Start.WindowAmount.Value);
            }

            if (this.Start.RelativeInterval.Value == DateInterval.Year)
            {
                return new DateTime(relativeBaseDate.Year, 1, 1).AddYears(this.Start.RelativeAmount.Value);
            }

            if (this.Start.RelativeInterval.Value == DateInterval.Month)
            {
                return new DateTime(relativeBaseDate.Year, relativeBaseDate.Month, 1).AddMonths(this.Start.RelativeAmount.Value);
            }

            return relativeBaseDate.AddDays(this.Start.RelativeAmount.Value);
// ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Gets the end date for this range today.
        /// </summary>
        /// <returns>The range's ending date for today</returns>
        /// <remarks>Make sure to check <see cref="IsValid"/> before calling this; an invalid will go into an infinite loop</remarks>
        public DateTime? GetEndDate()
        {
            return this.GetEndDate(DateTime.Today);
        }

        /// <summary>
        /// Gets the end date for this range today, in UTC.
        /// </summary>
        /// <returns>The range's ending date for today, in UTC</returns>
        /// <remarks>Make sure to check <see cref="IsValid"/> before calling this; an invalid will go into an infinite loop</remarks>
        public DateTime? GetEndDateUtc()
        {
            var endDate = this.GetEndDate();
            DateTime? endDateUtc = null;
            if (endDate.HasValue)
            {
                endDateUtc = TimeZoneInfo.ConvertTimeToUtc(endDate.Value, Dnn.Utility.GetUserTimeZone());
            }

            return endDateUtc;
        }

        /// <summary>
        /// Gets the end date for this range based on the given base date.
        /// </summary>
        /// <param name="relativeBaseDate">The base date from which relative dates should be calculated.</param>
        /// <returns>The range's ending date</returns>
        /// <remarks>Make sure to check <see cref="IsValid"/> before calling this; an invalid will go into an infinite loop</remarks>
        public DateTime? GetEndDate(DateTime relativeBaseDate)
        {
// ReSharper disable PossibleInvalidOperationException
            if (this.End.IsUnbounded)
            {
                return null;
            }

            if (this.End.IsSpecificDate)
            {
                return this.End.SpecificDate.Value;
            }

            if (this.End.IsWindow)
            {
                var otherDate = this.GetStartDate(relativeBaseDate).Value;
                if (this.End.WindowInterval.Value == DateInterval.Year)
                {
                    return otherDate.AddYears(this.End.WindowAmount.Value);
                }

                if (this.End.WindowInterval.Value == DateInterval.Month)
                {
                    return otherDate.AddMonths(this.End.WindowAmount.Value);
                }

                return otherDate.AddDays(this.End.WindowAmount.Value);
            }

            if (this.End.RelativeInterval.Value == DateInterval.Year)
            {
                return new DateTime(relativeBaseDate.Year, 1, 1).AddYears(this.End.RelativeAmount.Value + 1).AddDays(-1);
            }

            if (this.End.RelativeInterval.Value == DateInterval.Month)
            {
                return new DateTime(relativeBaseDate.Year, relativeBaseDate.Month, 1).AddMonths(this.End.RelativeAmount.Value + 1).AddDays(-1);
            }

            return relativeBaseDate.AddDays(this.End.RelativeAmount.Value);
// ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Gets the error message for an invalid range, or <see cref="string.Empty"/> if the range is valid.
        /// </summary>
        /// <returns>The error message or <see cref="string.Empty"/></returns>
        private string GetErrorMessage()
        {
            if (this.Start.IsWindow && this.End.IsWindow)
            {
                return "Both Window";
            }

            if (this.Start.IsWindow && this.End.IsUnbounded)
            {
                return "Window to End of Time";
            }

            if (this.End.IsWindow && this.Start.IsUnbounded)
            {
                return "Window to Beginning of Time";
            }

            return string.Empty;
        }
    }
}