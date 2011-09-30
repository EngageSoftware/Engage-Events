// <copyright file="SettingsService.asmx.cs" company="Engage Software">
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
    using System.Globalization;
    using System.Linq;
    using System.Web.Script.Services;
    using System.Web.Services;

    using DotNetNuke.Services.Localization;

    /// <summary>
    /// Web services for module settings
    /// </summary>
    [WebService(Namespace = "http://www.engagesoftware.com/services/engage_events")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class SettingsService : WebService
    {
        /// <summary>
        /// The resource file from which to get localized text for the template display options control
        /// </summary>
        private static readonly string ResourceFileRoot = Utility.DesktopModuleFolderName + "Display/App_LocalResources/TemplateDisplayOptions.ascx";

        /// <summary>
        /// Gets the date range formatted in a friendly text.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        /// <returns>The example date range text with the correct date values</returns>
        [WebMethod]
        public object FormatDateRange(DateRangeBoundJsonTransferObject start, DateRangeBoundJsonTransferObject end)
        {
            DateRangeBound startRangeBound;
            DateRangeBound endRangeBound;
            try
            {
                startRangeBound = DateRangeBound.Parse(start.value, start.specificDate, start.windowAmount, start.windowInterval);
                endRangeBound = DateRangeBound.Parse(end.value, end.specificDate, end.windowAmount, end.windowInterval);
            }
            catch (ArgumentNullException exc)
            {
                return new { isError = true, message = Localization.GetString("Missing " + exc.ParamName, ResourceFileRoot) };
            }

            var dateRange = new DateRange(startRangeBound, endRangeBound);
            if (!string.IsNullOrEmpty(dateRange.ErrorMessage))
            {
                return new { isError = true, message = Localization.GetString(dateRange.ErrorMessage, ResourceFileRoot) };
            }

            var dateRangeResourceKey = startRangeBound.IsUnbounded && endRangeBound.IsUnbounded
                                           ? "Date Range Without Bounds.Text"
                                           : startRangeBound.IsUnbounded
                                                 ? "Date Range Without Start.Format"
                                                 : endRangeBound.IsUnbounded ? "Date Range Without End.Format" : "Date Range.Format";

            var dateRangeText = string.Format(
                CultureInfo.CurrentCulture,
                Localization.GetString(dateRangeResourceKey, ResourceFileRoot),
                dateRange.GetStartDate(),
                dateRange.GetEndDate());

#if DEBUG
            dateRangeText = dateRangeText.Replace("[L]", string.Empty);
#endif

            return new { isError = false, message = dateRangeText };
        }
    }
}