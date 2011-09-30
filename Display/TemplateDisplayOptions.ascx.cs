// <copyright file="TemplateDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Events - http://www.EngageSoftware.com
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Events.Display
{
    using System;
    using System.Reflection;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;

    using Telerik.Web.UI;

    using SettingsBase = Engage.Dnn.Events.SettingsBase;
    using Utility = Dnn.Utility;

    /// <summary>
    /// The settings for a template
    /// </summary>
    public partial class TemplateDisplayOptions : SettingsBase
    {
        /// <summary>
        /// Backing field for <see cref="DateRangeFromSettings"/>
        /// </summary>
        private DateRange dateRangeFromSettings;

        /// <summary>
        /// Backing field for <see cref="ParseInputToDateRange"/>
        /// </summary>
        private DateRange parsedDateRange;

        /// <summary>
        /// Gets a JSON string representing the start date range bound.
        /// </summary>
        protected string StartDateRangeBoundJson 
        { 
            get
            {
                return new DateRangeBoundJsonTransferObject(this.DateRangeFromSettings.Start).AsJson();
            }
        }

        /// <summary>
        /// Gets a JSON string representing the end date range bound.
        /// </summary>
        protected string EndDateRangeBoundJson 
        { 
            get
            {
                return new DateRangeBoundJsonTransferObject(this.DateRangeFromSettings.End).AsJson();
            }
        }

        /// <summary>
        /// Gets the number of events to display per page.
        /// </summary>
        /// <value>The number of events to display per page.</value>
        private int RecordsPerPage
        {
            get
            {
                int? recordsPerPage = Dnn.Events.ModuleSettings.RecordsPerPage.GetValueAsInt32For(this);
                return recordsPerPage.HasValue && recordsPerPage.Value > 0 ? recordsPerPage.Value : Dnn.Events.ModuleSettings.RecordsPerPage.DefaultValue;
            }
        }

        /// <summary>
        /// Gets the module's configured date range.
        /// </summary>
        private DateRange DateRangeFromSettings
        {
            get
            {
                if (this.dateRangeFromSettings == null)
                {
                    this.dateRangeFromSettings = Dnn.Events.ModuleSettings.GetDateRangeFor(this);
                }

                return this.dateRangeFromSettings;
            }
        }

        /// <summary>
        /// Loads the possible settings, and selects the current values.
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                base.LoadSettings();

                ((ScriptManager)AJAX.ScriptManagerControl(this.Page)).Scripts.Add(new ScriptReference("Engage.Dnn.Events.JavaScript.EngageEvents.TemplatedDisplayOptions.combined.js", Assembly.GetExecutingAssembly().FullName));

                this.LoadBoundSettings(
                    this.DateRangeFromSettings.Start,
                    this.RangeStartDropDownList,
                    this.StartSpecificDatePicker,
                    this.StartWindowAmountTextBox,
                    this.StartWindowIntervalDropDownList);

                this.LoadBoundSettings(
                    this.DateRangeFromSettings.End,
                    this.RangeEndDropDownList,
                    this.EndSpecificDatePicker,
                    this.EndWindowAmountTextBox,
                    this.EndWindowIntervalDropDownList);

                this.RecordsPerPageTextBox.Value = this.RecordsPerPage;

                this.Load += (_, __) => this.DataBind();
                this.DateRangeValidator.ServerValidate += this.DateRangeValidator_ServerValidate;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Updates the settings for this module.
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
                base.UpdateSettings();

                if (!this.Page.IsValid)
                {
                    return;
                }

                var dateRange = this.ParseInputToDateRange();
                Dnn.Events.ModuleSettings.SetDateRangeSettings(this, dateRange);
                Dnn.Events.ModuleSettings.RecordsPerPage.Set(this, (int)this.RecordsPerPageTextBox.Value.Value);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Selects the given value in the list, if an item with that value is in the list.
        /// </summary>
        /// <param name="list">The list of items whose selected value is to be set.</param>
        /// <param name="selectedValue">The value of the item to be selected.</param>
        private static void SelectListValue(ListControl list, string selectedValue)
        {
            ListItem li = list.Items.FindByValue(selectedValue);
            if (li != null)
            {
                li.Selected = true;
            }
        }

        /// <summary>
        /// Sets the initial values of the controls for a date range bound.
        /// </summary>
        /// <param name="bound">The bound to load</param>
        /// <param name="rangeBoundList">The main range bound list.</param>
        /// <param name="specificDatePicker">The picker for the specific date.</param>
        /// <param name="windowAmountTextBox">The text box for the window amount.</param>
        /// <param name="windowIntervalList">The window interval list.</param>
        private void LoadBoundSettings(DateRangeBound bound, ListControl rangeBoundList, RadDatePicker specificDatePicker, RadNumericTextBox windowAmountTextBox, ListControl windowIntervalList)
        {
            Utility.LocalizeListControl(rangeBoundList, this.LocalResourceFile);
            Utility.LocalizeListControl(windowIntervalList, this.LocalResourceFile);

            SelectListValue(rangeBoundList, DateRangeBoundJsonTransferObject.GetListValueForBound(bound));

            if (bound.IsSpecificDate)
            {
                specificDatePicker.SelectedDate = bound.SpecificDate;
            }
            else if (bound.IsWindow)
            {
                windowAmountTextBox.Value = bound.WindowAmount;
                SelectListValue(windowIntervalList, bound.WindowInterval.Value.ToString());
            }
        }

        /// <summary>
        /// Parses the form's input into a date range.
        /// </summary>
        /// <returns>A <see cref="DateRange"/> instance</returns>
        private DateRange ParseInputToDateRange()
        {
            if (this.parsedDateRange == null)
            {
                var startRangeBound = DateRangeBound.Parse(
                    this.RangeStartDropDownList.SelectedValue,
                    this.StartSpecificDatePicker.SelectedDate,
                    this.StartWindowAmountTextBox.Value,
                    this.StartWindowIntervalDropDownList.SelectedValue);

                var endRangeBound = DateRangeBound.Parse(
                    this.RangeEndDropDownList.SelectedValue,
                    this.EndSpecificDatePicker.SelectedDate,
                    this.EndWindowAmountTextBox.Value,
                    this.EndWindowIntervalDropDownList.SelectedValue);

                this.parsedDateRange = new DateRange(startRangeBound, endRangeBound);
            }

            return this.parsedDateRange;
        }

        /// <summary>
        /// Handles the <see cref="CustomValidator.ServerValidate"/> event of the <see cref="DateRangeValidator"/> control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        private void DateRangeValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateRange dateRange;
            try
            {
                dateRange = this.ParseInputToDateRange();
            }
            catch (ArgumentNullException exc)
            {
                args.IsValid = false;
                this.DateRangeValidator.ErrorMessage = this.Localize("Missing " + exc.ParamName);
                return;
            }

            args.IsValid = dateRange.IsValid;
            this.DateRangeValidator.ErrorMessage = this.Localize(dateRange.ErrorMessage);
        }
    }
}