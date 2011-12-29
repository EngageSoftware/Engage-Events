// <copyright file="ModuleSettings.cs" company="Engage Software">
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
    using System.Linq;

    using Engage.Dnn.Framework;
    using Engage.Events;

    /// <summary>
    /// A static collection of all settings that apply to this module
    /// </summary>
    public static class ModuleSettings 
    {
        /// <summary>
        /// The categories by which to limit the display of events
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<string> Categories = new Setting<string>("Categories", SettingScope.TabModule, string.Empty);

        /// <summary>
        /// Whether to only display featured events
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<bool> FeaturedOnly = new Setting<bool>("FeaturedOnly", SettingScope.TabModule, false);

        /// <summary>
        /// The ID of the tab on which to display details when an event's details link is clicked
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<int?> DetailsDisplayTabId = new Setting<int?>("DetailsDisplayTabId", SettingScope.TabModule, null);

        /// <summary>
        /// The ID of the module in which to display details when an event's details link is clicked
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<int?> DetailsDisplayModuleId = new Setting<int?>("DetailsDisplayModuleId", SettingScope.TabModule, null);

        /// <summary>
        /// The skin used for the Calendar display
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<TelerikSkin> SkinSelection = new Setting<TelerikSkin>("SkinSelection", SettingScope.TabModule, TelerikSkin.Default);

        /// <summary>
        /// The number of events to display on a single day in the calendar's month view
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<int> EventsPerDay = new Setting<int>("EventsPerDay", SettingScope.TabModule, 3);

        /// <summary>
        /// The template used to display a list of events
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<string> Template = new Setting<string>("Template", SettingScope.TabModule, string.Empty);

        /// <summary>
        /// The template used to display a single event
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<string> SingleItemTemplate = new Setting<string>("SingleItemTemplate", SettingScope.TabModule, string.Empty);

        /// <summary>
        /// The chosen display type (calendar or listing) for this module 
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<string> DisplayType = new Setting<string>("DisplayType", SettingScope.TabModule, "LIST");

        /// <summary>
        /// The chosen display type (calendar or listing) for this module 
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<int> RecordsPerPage = new Setting<int>("RecordsPerPage", SettingScope.TabModule, 10);

        /// <summary>
        /// Whether to hide events which have hit their registration cap
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        public static readonly Setting<bool> HideFullEvents = new Setting<bool>("HideFullEvents", SettingScope.TabModule, false);

        /// <summary>
        /// The amount the range starts from today, if the range start is relative (the unit of the amount is determined by <see cref="RangeStartRelativeInterval"/>)
        /// </summary>
        /// <example>
        /// If the range starts "today," then the amount is <c>0</c>, if it is "yesterday", then the amount is <c>-1</c> 
        /// (assuming <see cref="RangeStartRelativeInterval"/> is <see cref="DateInterval.Day"/>)
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<int?> RangeStartRelativeAmount = new Setting<int?>("RangeStartRelativeAmount", SettingScope.TabModule, 0);

        /// <summary>
        /// The interval to apply to the <see cref="RangeStartRelativeAmount"/>, if the range start is relative
        /// </summary>
        /// <example>
        /// Given a <see cref="RangeStartRelativeAmount"/> of <c>1</c>, 
        /// if the interval is <see cref="DateInterval.Day"/> then the range starts tomorrow,
        /// if the interval is <see cref="DateInterval.Month"/> then the range starts at the beginning of next month
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<DateInterval?> RangeStartRelativeInterval = new Setting<DateInterval?>("RangeStartRelativeInterval", SettingScope.TabModule, DateInterval.Day);

        /// <summary>
        /// The amount the range starts before the range's end, if the range start is a window (the unit of the amount is determined by <see cref="RangeStartWindowInterval"/>)
        /// </summary>
        /// <example>
        /// If the range starts 30 days before the range's end, then the amount is <c>30</c>
        /// (assuming <see cref="RangeStartWindowInterval"/> is <see cref="DateInterval.Day"/>)
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<int?> RangeStartWindowAmount = new Setting<int?>("RangeStartWindowAmount", SettingScope.TabModule, null);

        /// <summary>
        /// The interval to apply to the <see cref="RangeStartWindowAmount"/>, if the range start is a window
        /// </summary>
        /// <example>
        /// Given a <see cref="RangeStartWindowAmount"/> of <c>1</c>, 
        /// if the interval is <see cref="DateInterval.Day"/> then the range starts one day before the range ends,
        /// if the interval is <see cref="DateInterval.Month"/> then the range starts one month before the range ends
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<DateInterval?> RangeStartWindowInterval = new Setting<DateInterval?>("RangeStartWindowInterval", SettingScope.TabModule, null);

        /// <summary>
        /// The specific date on which the range starts, if the range start is a specific date
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<DateTime?> RangeStartSpecificDate = new Setting<DateTime?>("RangeStartSpecificDate", SettingScope.TabModule, null);

        /// <summary>
        /// The amount the range ends from today, if the range end is relative (the unit of the amount is determined by <see cref="RangeEndRelativeInterval"/>)
        /// </summary>
        /// <example>
        /// If the range ends "today," then the amount is <c>0</c>, if it is "yesterday", then the amount is <c>-1</c> 
        /// (assuming <see cref="RangeEndRelativeInterval"/> is <see cref="DateInterval.Day"/>)
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<int?> RangeEndRelativeAmount = new Setting<int?>("RangeEndRelativeAmount", SettingScope.TabModule, null);

        /// <summary>
        /// The interval to apply to the <see cref="RangeEndRelativeAmount"/>, if the range end is relative
        /// </summary>
        /// <example>
        /// Given a <see cref="RangeEndRelativeAmount"/> of <c>1</c>, 
        /// if the interval is <see cref="DateInterval.Day"/> then the range ends tomorrow,
        /// if the interval is <see cref="DateInterval.Month"/> then the range ends at the end of next month
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<DateInterval?> RangeEndRelativeInterval = new Setting<DateInterval?>("RangeEndRelativeInterval", SettingScope.TabModule, null);

        /// <summary>
        /// The amount the range ends after the range's start, if the range end is a window (the unit of the amount is determined by <see cref="RangeEndWindowInterval"/>)
        /// </summary>
        /// <example>
        /// If the range ends 30 days after the range's start, then the amount is <c>30</c>
        /// (assuming <see cref="RangeEndWindowInterval"/> is <see cref="DateInterval.Day"/>)
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<int?> RangeEndWindowAmount = new Setting<int?>("RangeEndWindowAmount", SettingScope.TabModule, null);

        /// <summary>
        /// The interval to apply to the <see cref="RangeEndWindowAmount"/>, if the range end is a window
        /// </summary>
        /// <example>
        /// Given a <see cref="RangeEndWindowAmount"/> of <c>1</c>, 
        /// if the interval is <see cref="DateInterval.Day"/> then the range ends one day after the range starts,
        /// if the interval is <see cref="DateInterval.Month"/> then the range ends one month after the range starts
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<DateInterval?> RangeEndWindowInterval = new Setting<DateInterval?>("RangeEndWindowInterval", SettingScope.TabModule, null);

        /// <summary>
        /// The specific date on which the range ends, if the range end is a specific date
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting nullable types isn't a big deal")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<DateTime?> RangeEndSpecificDate = new Setting<DateTime?>("RangeEndSpecificDate", SettingScope.TabModule, null);

#pragma warning disable 618
        /// <summary>
        /// The chosen display type (calendar or listing) for this module 
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Setting<T> is immutable")]
        private static readonly Setting<ListingMode> DisplayModeOption = new Setting<ListingMode>("DisplayModeOption", SettingScope.TabModule, ListingMode.All);
#pragma warning restore 618

        public static IEnumerable<int> GetCategoriesFor(IModuleControlBase moduleControl)
        {
            var categoriesSettingValue = Categories.GetValueAsStringFor(moduleControl);
            return string.IsNullOrEmpty(categoriesSettingValue)
                       ? Enumerable.Empty<int>()
                       : categoriesSettingValue.Split(',').Select(id => int.Parse(id, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gets the date range for the given module.
        /// </summary>
        /// <param name="moduleControl">The module control.</param>
        /// <returns>The date range</returns>
        public static DateRange GetDateRangeFor(IModuleControlBase moduleControl)
        {
            if (!RangeStartRelativeAmount.IsSettingDefinedFor(moduleControl) && DisplayModeOption.IsSettingDefinedFor(moduleControl))
            {
                var dateRange = GetDateRangeForListingMode(moduleControl);
                SetDateRangeSettings(moduleControl, dateRange);
                return dateRange;
            }

            var startRangeBound = new DateRangeBound(
                GetValueAsNullableInt32(RangeStartRelativeAmount, moduleControl),
                GetValueAsNullableEnum(RangeStartRelativeInterval, moduleControl),
                GetValueAsNullableDateTime(RangeStartSpecificDate, moduleControl),
                GetValueAsNullableInt32(RangeStartWindowAmount, moduleControl),
                GetValueAsNullableEnum(RangeStartWindowInterval, moduleControl));
            var endRangeBound = new DateRangeBound(
                GetValueAsNullableInt32(RangeEndRelativeAmount, moduleControl),
                GetValueAsNullableEnum(RangeEndRelativeInterval, moduleControl),
                GetValueAsNullableDateTime(RangeEndSpecificDate, moduleControl),
                GetValueAsNullableInt32(RangeEndWindowAmount, moduleControl),
                GetValueAsNullableEnum(RangeEndWindowInterval, moduleControl));
            return new DateRange(startRangeBound, endRangeBound);
        }

        /// <summary>
        /// Sets the module's settings to the given <paramref name="range"/>.
        /// </summary>
        /// <param name="moduleControl">The module control.</param>
        /// <param name="range">The range to which the settings should be set.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="range"/> is <c>null</c></exception>
        public static void SetDateRangeSettings(IModuleControlBase moduleControl, DateRange range)
        {
            if (range == null)
            {
                throw new ArgumentNullException("range");
            }

            RangeStartRelativeAmount.Set(moduleControl, range.Start.RelativeAmount);
            RangeStartRelativeInterval.Set(moduleControl, range.Start.RelativeInterval);
            RangeStartSpecificDate.Set(moduleControl, range.Start.SpecificDate);
            RangeStartWindowAmount.Set(moduleControl, range.Start.WindowAmount);
            RangeStartWindowInterval.Set(moduleControl, range.Start.WindowInterval);
            RangeEndRelativeAmount.Set(moduleControl, range.End.RelativeAmount);
            RangeEndRelativeInterval.Set(moduleControl, range.End.RelativeInterval);
            RangeEndSpecificDate.Set(moduleControl, range.End.SpecificDate);
            RangeEndWindowAmount.Set(moduleControl, range.End.WindowAmount);
            RangeEndWindowInterval.Set(moduleControl, range.End.WindowInterval);
        }

        /// <summary>
        /// Gets the value of this setting for the given <paramref name="moduleControl"/> as an <see cref="int"/>,
        /// or <see cref="Setting{T}.DefaultValue"/> if the setting hasn't been set or isn't an <see cref="int"/>.
        /// </summary>
        /// <param name="setting">The setting to get the value for.</param>
        /// <param name="moduleControl">A module control instance to which this setting applies.</param>
        /// <returns>
        /// The value of this setting for the given <paramref name="moduleControl"/>
        /// or <see cref="Setting{T}.DefaultValue"/> if the value does not exist yet or cannot be converted to an <see cref="int"/>,
        /// or <c>null</c> if <see cref="Setting{T}.DefaultValue"/> is not an <see cref="int"/>.
        /// </returns>
        /// <remarks>
        /// Adjusts for <see cref="Setting{T}.GetValueAsEnumFor{TEnum}(Engage.Dnn.Framework.IModuleControlBase)"/>'s assumption 
        /// that a value of <c>null</c> is not valid and so should return <see cref="Setting{T}.DefaultValue"/>
        /// </remarks>
        private static int? GetValueAsNullableInt32(Setting<int?> setting, IModuleControlBase moduleControl)
        {
            return setting.GetValueAsInt32For(moduleControl.DesktopModuleName, moduleControl.ModuleConfiguration, setting.IsSettingDefinedFor(moduleControl) ? null : setting.DefaultValue);
        }

        /// <summary>
        /// Gets the value of this setting for the given <paramref name="moduleControl"/> as a <see cref="DateTime"/>,
        /// or <see cref="Setting{T}.DefaultValue"/> if the setting hasn't been set or isn't an <see cref="DateTime"/>.
        /// </summary>
        /// <param name="setting">The setting to get the value for.</param>
        /// <param name="moduleControl">A module control instance to which this setting applies.</param>
        /// <returns>
        /// The value of this setting for the given <paramref name="moduleControl"/>
        /// or <see cref="Setting{T}.DefaultValue"/> if the value does not exist yet or cannot be converted to a <see cref="DateTime"/>,
        /// or <c>null</c> if <see cref="Setting{T}.DefaultValue"/> is not a <see cref="DateTime"/>.
        /// </returns>
        /// <remarks>
        /// Adjusts for <see cref="Setting{T}.GetValueAsEnumFor{TEnum}(Engage.Dnn.Framework.IModuleControlBase)"/>'s assumption 
        /// that a value of <c>null</c> is not valid and so should return <see cref="Setting{T}.DefaultValue"/>
        /// </remarks>
        private static DateTime? GetValueAsNullableDateTime(Setting<DateTime?> setting, IModuleControlBase moduleControl)
        {
            return setting.GetValueAsDateTimeFor(moduleControl.DesktopModuleName, moduleControl.ModuleConfiguration, setting.IsSettingDefinedFor(moduleControl) ? null : setting.DefaultValue);
        }

        /// <summary>
        /// Gets the value of this setting for the given <paramref name="moduleControl"/> as an <see cref="Enum"/> of <typeparamref name="TEnum"/>,
        /// or <see cref="Setting{T}.DefaultValue"/> if the setting hasn't been set or isn't an <see cref="Enum"/> of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the <see cref="Enum"/> to which the value should be converted.</typeparam>
        /// <param name="setting">The setting to get the value for.</param>
        /// <param name="moduleControl">A module control instance to which this setting applies.</param>
        /// <returns>
        /// The value of this setting for the given <paramref name="moduleControl"/>
        /// or <see cref="Setting{T}.DefaultValue"/> if the value does not exist yet or cannot be converted to a <typeparamref name="TEnum"/>,
        /// or <c>null</c> if <see cref="Setting{T}.DefaultValue"/> is not a <typeparamref name="TEnum"/>.
        /// </returns>
        /// <remarks>
        /// Adjusts for <see cref="Setting{T}.GetValueAsEnumFor{TEnum}(Engage.Dnn.Framework.IModuleControlBase)"/>'s assumption 
        /// that a value of <c>null</c> is not valid and so should return <see cref="Setting{T}.DefaultValue"/>
        /// </remarks>
        private static TEnum? GetValueAsNullableEnum<TEnum>(Setting<TEnum?> setting, IModuleControlBase moduleControl) where TEnum : struct
        {
            return setting.GetValueAsEnumFor(moduleControl.DesktopModuleName, moduleControl.ModuleConfiguration, setting.IsSettingDefinedFor(moduleControl) ? null : setting.DefaultValue);
        }

        /// <summary>
        /// Gets the date range represented by the listing mode, 
        /// to allow migrating from old settings (<see cref="ListingMode"/>) to new settings (<see cref="DateRange"/>).
        /// </summary>
        /// <param name="moduleControl">The module control for which to get the date range.</param>
        /// <returns>A new <see cref="DateRange"/> instance</returns>
        private static DateRange GetDateRangeForListingMode(IModuleControlBase moduleControl)
        {
            DateRangeBound startRangeBound;
            DateRangeBound endRangeBound;

#pragma warning disable 618
            switch (DisplayModeOption.GetValueAsEnumFor<ListingMode>(moduleControl))
            {
                case ListingMode.CurrentMonth:
                    startRangeBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Day);
                    endRangeBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Month);
                    break;
                case ListingMode.Future:
                    startRangeBound = DateRangeBound.CreateRelativeBound(1, DateInterval.Month);
                    endRangeBound = DateRangeBound.CreateUnboundedBound();
                    break;
                case ListingMode.Past:
                    startRangeBound = DateRangeBound.CreateUnboundedBound();
                    endRangeBound = DateRangeBound.CreateRelativeBound(0, DateInterval.Day);
                    break;
                    ////case ListingMode.All:
                default:
                    startRangeBound = DateRangeBound.CreateUnboundedBound();
                    endRangeBound = DateRangeBound.CreateUnboundedBound();
                    break;
            }
#pragma warning restore 618

            return new DateRange(startRangeBound, endRangeBound);
        }
    }
}