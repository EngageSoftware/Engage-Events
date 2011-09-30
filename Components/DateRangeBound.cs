// <copyright file="DateRangeBound.cs" company="Engage Software">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// One end of a <see cref="DateRange"/>
    /// </summary>
    public class DateRangeBound
    {
        /// <summary>
        /// A shared instance for all unbounded bounds
        /// </summary>
        private static readonly DateRangeBound UnboundedBound = new DateRangeBound(null, null, null, null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeBound"/> class.
        /// </summary>
        /// <param name="relativeAmount">The relative amount.</param>
        /// <param name="relativeInterval">The relative interval.</param>
        /// <param name="specificDate">The specific date.</param>
        /// <param name="windowAmount">The window amount.</param>
        /// <param name="windowInterval">The window interval.</param>
        public DateRangeBound(int? relativeAmount, DateInterval? relativeInterval, DateTime? specificDate, int? windowAmount, DateInterval? windowInterval)
        {
            if (specificDate.HasValue)
            {
                this.SpecificDate = specificDate;
            }
            else if (windowAmount.HasValue && windowInterval.HasValue)
            {
                this.WindowAmount = windowAmount;
                this.WindowInterval = windowInterval;
            }
            else
            {
                this.RelativeAmount = relativeAmount;
                this.RelativeInterval = relativeInterval;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DateRangeBound"/> class from being created.
        /// </summary>
        private DateRangeBound()
        {
        }

        /// <summary>
        /// Gets the amount this bound is different from the relative starting point, or <c>null</c> if this is not a relative bound.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public int? RelativeAmount { get; private set; }

        /// <summary>
        /// Gets the interval/unit this bound is different from the relative starting point, or <c>null</c> if this is not a relative bound.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public DateInterval? RelativeInterval { get; private set; }

        /// <summary>
        /// Gets the specific date for this bound, or <c>null</c> if this is not a specific date bound.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public DateTime? SpecificDate { get; private set; }

        /// <summary>
        /// Gets the amount this bound is different from the other side of the range, or <c>null</c> if this is not a window bound.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public int? WindowAmount { get; private set; }

        /// <summary>
        /// Gets the interval/unit this bound is different from the other side of the range, or <c>null</c> if this is not a window bound.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public DateInterval? WindowInterval { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance represents a relative bound.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a relative bound; otherwise, <c>false</c>.
        /// </value>
        public bool IsRelative
        {
            get { return this.RelativeInterval.HasValue; } 
        }

        /// <summary>
        /// Gets a value indicating whether this instance represents a window bound.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a window bound; otherwise, <c>false</c>.
        /// </value>
        public bool IsWindow
        {
            get { return this.WindowInterval.HasValue; } 
        }

        /// <summary>
        /// Gets a value indicating whether this instance represents a specific date bound.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a specific date bound; otherwise, <c>false</c>.
        /// </value>
        public bool IsSpecificDate
        {
            get { return this.SpecificDate.HasValue; } 
        }

        /// <summary>
        /// Gets a value indicating whether this instance is unbounded.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is unbounded; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnbounded
        {
            get { return !this.IsRelative && !this.IsWindow && !this.IsSpecificDate; } 
        }

        /// <summary>
        /// Parses the specified values into a <see cref="DateRangeBound"/>.
        /// </summary>
        /// <param name="value">The "main" selected value, containing either a pipe-delimited pair of relative amount and relative interval, or an indication of the other type of range represented.</param>
        /// <param name="specificDate">The specific date, or <c>null</c>.</param>
        /// <param name="windowAmount">The window amount as an integer, or <c>null</c>.</param>
        /// <param name="windowInterval">The window interval as a <see cref="DateInterval"/> value, or <c>null</c>.</param>
        /// <returns>A new <see cref="DateRangeBound"/> instance</returns>
        public static DateRangeBound Parse(string value, DateTime? specificDate, double? windowAmount, string windowInterval)
        {
            var dateRangeBound = new DateRangeBound();

            if (value == "specific-date")
            {
                if (specificDate == null)
                {
                    throw new ArgumentNullException("specificDate");
                }

                dateRangeBound.SpecificDate = specificDate;
            }
            else if (value == "window")
            {
                if (windowAmount == null)
                {
                    throw new ArgumentNullException("windowAmount");
                }

                if (windowInterval == null)
                {
                    throw new ArgumentNullException("windowInterval");
                }

                dateRangeBound.WindowAmount = (int)windowAmount.Value;
                dateRangeBound.WindowInterval = (DateInterval)Enum.Parse(typeof(DateInterval), windowInterval);
            }
            else if (!string.IsNullOrEmpty(value))
            {
                var relativeRangeParts = value.Split('|');
                if (relativeRangeParts.Length != 2)
                {
                    throw new ArgumentException("Value must be relative amount and relative interval, separated by pipe", "value");
                }

                int relativeAmount;
                if (!int.TryParse(relativeRangeParts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out relativeAmount))
                {
                    throw new ArgumentException("First part of value must be an integer", "value");
                }

                if (!Enum.IsDefined(typeof(DateInterval), relativeRangeParts[1]))
                {
                    throw new ArgumentException("Second part of value must be a DateInterval value", "value");
                }

                dateRangeBound.RelativeAmount = relativeAmount;
                dateRangeBound.RelativeInterval = (DateInterval)Enum.Parse(typeof(DateInterval), relativeRangeParts[1]);
            }

            return dateRangeBound;
        }

        /// <summary>
        /// Creates a new relative bound.
        /// </summary>
        /// <param name="relativeAmount">The relative amount.</param>
        /// <param name="relativeInterval">The relative interval.</param>
        /// <returns>A new <see cref="DateRangeBound"/> instance representing a relative bound</returns>
        public static DateRangeBound CreateRelativeBound(int relativeAmount, DateInterval relativeInterval)
        {
            return new DateRangeBound(relativeAmount, relativeInterval, null, null, null);
        }

        /// <summary>
        /// Creates a new bound representing a specific date.
        /// </summary>
        /// <param name="specificDate">The specific date.</param>
        /// <returns>A new <see cref="DateRangeBound"/> instance representing a specific date bound</returns>
        public static DateRangeBound CreateSpecificDateBound(DateTime specificDate)
        {
            return new DateRangeBound(null, null, specificDate, null, null);
        }

        /// <summary>
        /// Creates a new window bound.
        /// </summary>
        /// <param name="windowAmount">The window amount.</param>
        /// <param name="windowInterval">The window interval.</param>
        /// <returns>A new <see cref="DateRangeBound"/> instance representing a window bound</returns>
        public static DateRangeBound CreateWindowBound(int windowAmount, DateInterval windowInterval)
        {
            return new DateRangeBound(null, null, null, windowAmount, windowInterval);
        }

        /// <summary>
        /// Creates an unbounded bound.
        /// </summary>
        /// <returns>A <see cref="DateRangeBound"/> instance representing an unbounded bound</returns>
        public static DateRangeBound CreateUnboundedBound()
        {
            return UnboundedBound;
        }
    }
}