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
        public static readonly Setting<string> Categories = new Setting<string>("Categories", SettingScope.TabModule, string.Empty);

        /// <summary>
        /// Whether to only display featured events
        /// </summary>
        public static readonly Setting<bool> FeaturedOnly = new Setting<bool>("FeaturedOnly", SettingScope.TabModule, false);

        /// <summary>
        /// The ID of the tab on which to display details when an event's details link is clicked
        /// </summary>
        public static readonly Setting<int?> DetailsDisplayTabId = new Setting<int?>("DetailsDisplayTabId", SettingScope.TabModule, null);

        /// <summary>
        /// The ID of the module in which to display details when an event's details link is clicked
        /// </summary>
        public static readonly Setting<int?> DetailsDisplayModuleId = new Setting<int?>("DetailsDisplayModuleId", SettingScope.TabModule, null);

        /// <summary>
        /// The skin used for the Calendar display
        /// </summary>
        public static readonly Setting<TelerikSkin> SkinSelection = new Setting<TelerikSkin>("SkinSelection", SettingScope.TabModule, TelerikSkin.Default);

        /// <summary>
        /// The number of events to display on a single day in the calendar's month view
        /// </summary>
        public static readonly Setting<int> EventsPerDay = new Setting<int>("EventsPerDay", SettingScope.TabModule, 3);

        /// <summary>
        /// The template used to display a list of events
        /// </summary>
        public static readonly Setting<string> Template = new Setting<string>("Template", SettingScope.TabModule, string.Empty);

        /// <summary>
        /// The template used to display a single event
        /// </summary>
        public static readonly Setting<string> SingleItemTemplate = new Setting<string>("SingleItemTemplate", SettingScope.TabModule, string.Empty);

        /// <summary>
        /// The chosen display type (calendar or listing) for this module 
        /// </summary>
        public static readonly Setting<string> DisplayType = new Setting<string>("DisplayType", SettingScope.TabModule, "LIST");

        /// <summary>
        /// The chosen display type (calendar or listing) for this module 
        /// </summary>
        public static readonly Setting<ListingMode> DisplayModeOption = new Setting<ListingMode>("DisplayModeOption", SettingScope.TabModule, ListingMode.All);

        /// <summary>
        /// The chosen display type (calendar or listing) for this module 
        /// </summary>
        public static readonly Setting<int> RecordsPerPage = new Setting<int>("RecordsPerPage", SettingScope.TabModule, 10);

        /// <summary>
        /// Whether to hide events which have hit their registration cap
        /// </summary>
        public static readonly Setting<bool> HideFullEvents = new Setting<bool>("HideFullEvents", SettingScope.TabModule, false);
    }
}