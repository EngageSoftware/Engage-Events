// <copyright file="DateRangeBoundJsonTransferObject.cs" company="Engage Software">
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.Script.Serialization;

    using Engage.Annotations;

    /// <summary>
    /// Transfers date range bounds to the <see cref="SettingsService"/>
    /// </summary>
    public struct DateRangeBoundJsonTransferObject : IEquatable<DateRangeBoundJsonTransferObject>
    {
        /// <summary>
        /// A JSON serializer
        /// </summary>
        private static readonly JavaScriptSerializer JsonSerializer = CreateJsonSerializer();

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeBoundJsonTransferObject"/> struct.
        /// </summary>
        /// <param name="bound">The bound.</param>
        public DateRangeBoundJsonTransferObject([NotNull] DateRangeBound bound)
            : this()
        {
            if (bound == null)
            {
                throw new ArgumentNullException("bound");
            }

            this.value = GetListValueForBound(bound);
            this.specificDate = bound.SpecificDate;
            this.windowAmount = bound.WindowAmount;
            this.windowInterval = bound.WindowInterval.HasValue ? bound.WindowInterval.Value.ToString() : null;
        }

        /// <summary>
        /// Gets or sets the "main" selected value, containing either a pipe-delimited pair of relative amount and relative interval, or an indication of the other type of range represented..
        /// </summary>
        /// <value>
        /// The "main" selected value, containing either a pipe-delimited pair of relative amount and relative interval, or an indication of the other type of range represented.
        /// </value>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "value", Justification = "JSON transfer object"), SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public string value { get; set; }

        /// <summary>
        /// Gets or sets the specific date.
        /// </summary>
        /// <value>
        /// The specific date, or <c>null</c>.
        /// </value>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "specific", Justification = "JSON transfer object"), SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public DateTime? specificDate { get; set; }

        /// <summary>
        /// Gets or sets the window amount.
        /// </summary>
        /// <value>
        /// The window amount, or <c>null</c>.
        /// </value>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "window", Justification = "JSON transfer object"), SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public int? windowAmount { get; set; }

        /// <summary>
        /// Gets or sets the window interval.
        /// </summary>
        /// <value>
        /// The window interval as a <see cref="DateInterval"/> value, or <c>null</c>.
        /// </value>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "window", Justification = "JSON transfer object"), SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "JSON transfer object")]
        public string windowInterval { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(DateRangeBoundJsonTransferObject left, DateRangeBoundJsonTransferObject right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(DateRangeBoundJsonTransferObject left, DateRangeBoundJsonTransferObject right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Gets the value for the main drop down list for the given <paramref name="bound"/>.
        /// </summary>
        /// <param name="bound">The date range bound.</param>
        /// <returns>The value from the drop down list matching the bound</returns>
        public static string GetListValueForBound([NotNull] DateRangeBound bound)
        {
            if (bound == null)
            {
                throw new ArgumentNullException("bound");
            }

            return bound.IsUnbounded
                       ? string.Empty
                       : bound.IsWindow
                             ? "window"
                             : bound.IsSpecificDate
                                   ? "specific-date"
                                   : string.Format(
                                       CultureInfo.InvariantCulture, 
                                       "{0}|{1}", 
                                       bound.RelativeAmount.Value, 
                                       bound.RelativeInterval.Value);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(DateRangeBoundJsonTransferObject other)
        {
            return Equals(other.value, this.value) 
                   && other.specificDate.Equals(this.specificDate) 
                   && other.windowAmount.Equals(this.windowAmount)
                   && Equals(other.windowInterval, this.windowInterval);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (!(obj is DateRangeBoundJsonTransferObject))
            {
                return false;
            }

            return this.Equals((DateRangeBoundJsonTransferObject)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Engage.Utility.CombineHashCodes(
                this.value != null ? this.value.GetHashCode() : 0,
                this.specificDate.HasValue ? this.specificDate.Value.GetHashCode() : 0,
                this.windowAmount.HasValue ? this.windowAmount.Value : 0,
                this.windowInterval != null ? this.windowInterval.GetHashCode() : 0);
        }

        /// <summary>
        /// Gets this instance as a JSON string
        /// </summary>
        /// <returns>A JSON <see cref="string"/></returns>
        public string AsJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Creates the json serializer.
        /// </summary>
        /// <returns>A new <see cref="JavaScriptSerializer"/> instance that can serialize <see cref="DateRangeBoundJsonTransferObject"/>s</returns>
        private static JavaScriptSerializer CreateJsonSerializer()
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.RegisterConverters(new[] { new DateRangeBoundJsonConverter() });
            return javaScriptSerializer;
        }

        /// <summary>
        /// Converts a <see cref="DateRangeBoundJsonTransferObject"/> into a JSON structure, in order to get a parsable DateTime value
        /// </summary>
        private class DateRangeBoundJsonConverter : JavaScriptConverter
        {
            /// <summary>
            /// Gets a collection of the supported types.
            /// </summary>
            /// <returns>
            /// An object that implements <see cref="IEnumerable{T}"/> that represents the types supported by the converter.
            /// </returns>
            public override IEnumerable<Type> SupportedTypes
            {
                get
                {
                    yield return typeof(DateRangeBoundJsonTransferObject);
                }
            }

            /// <summary>
            /// Converts the provided dictionary into an object of the specified type.
            /// </summary>
            /// <returns>
            /// The deserialized object.
            /// </returns>
            /// <param name="dictionary">An <see cref="IDictionary{TKey,TValue}"/> instance of property data stored as name/value pairs.</param>
            /// <param name="type">The type of the resulting object.</param>
            /// <param name="serializer">The <see cref="JavaScriptSerializer"/> instance.</param>
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException("Shouldn't be used for deserializing");
            }

            /// <summary>
            /// Builds a dictionary of name/value pairs.
            /// </summary>
            /// <returns>
            /// An object that contains key/value pairs that represent the object’s data.
            /// </returns>
            /// <param name="obj">The object to serialize.</param>
            /// <param name="serializer">The object that is responsible for the serialization.</param>
            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                if (!(obj is DateRangeBoundJsonTransferObject))
                {
                    throw new ArgumentException("Must be DateRangeBoundJsonTransferObject", "obj");
                }

                var dto = (DateRangeBoundJsonTransferObject)obj;
                return new Dictionary<string, object>(4)
                    {
                        { "value", dto.value },
                        { "windowAmount", dto.windowAmount },
                        { "windowInterval", dto.windowInterval },
                        { "specificDate", dto.specificDate.HasValue ? new { year = dto.specificDate.Value.Year, month = dto.specificDate.Value.Month - 1, day = dto.specificDate.Value.Day } : null }
                    };
            }
        }
    }
}