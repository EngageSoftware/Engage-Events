/*globals Sys */
(function (window, $find, $get) {
    'use strict';

    window.engageEvents = window.engageEvents || { };

    window.engageEvents.setupEventEdit = function (options) {
        window[options.startDateTimePickerDateSelectedFunctionName] = function (sender, eventArgs) {
            var EndDateTimePicker = $find(options.endDateTimePickerId),
                endDate = EndDateTimePicker.get_selectedDate(),
                newStartDate = eventArgs.get_newDate(),
                endDateAfterNewDate = endDate > newStartDate,
                selectedDateSpan = 1800000; // 30 minutes
            
            // don't update end date if there's already an end date but not an old start date
            // or if the start date was cleared
            if (newStartDate && (EndDateTimePicker.isEmpty() || eventArgs.get_oldDate() || !endDateAfterNewDate)) {
                if (!EndDateTimePicker.isEmpty() && endDateAfterNewDate) {
                    selectedDateSpan = endDate - eventArgs.get_oldDate();
                }

                EndDateTimePicker.set_selectedDate(new Date(newStartDate.getTime() + selectedDateSpan));
            }
        };

        window[options.categoryComboBoxSelectedIndexChangedFunctionName] = function (sender, eventArgs) {
            $get(options.categoryCreationPendingLabelId).style.display = eventArgs.get_item() ? 'none' : 'inline';
        };

        window[options.categoryComboBoxTextChangeFunctionName] = function () {
            $get(options.categoryCreationPendingLabelId).style.display = 'inline';
        };
        
        var originalValidationSummaryOnSubmit = window.ValidationSummaryOnSubmit;
        window.ValidationSummaryOnSubmit = function (validationGroup) {
            var originalScrollTo = window.scrollTo;
            window.scrollTo = function () { };
            originalValidationSummaryOnSubmit(validationGroup);
            window.scrollTo = originalScrollTo;
        };
    };
}(this, Sys.Application.findComponent, Sys.UI.DomElement.getElementById));
