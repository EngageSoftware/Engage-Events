/// <reference path="json2.js" />
/// <reference path="knockout-1.2.1.js"/>

/*globals jQuery, ko, Sys */
(function ($, window, ko, $find) {
    'use strict';

    $(function () {
        var data = window.engageEventsDateRangeData,
            getClientControl = function ($element) {
                return $find($element.attr('data-client-id'));
            },
            createRangeChooser = function (rangeBound) {
                var specificDate = rangeBound.specificDate ? new Date(
                                                                    rangeBound.specificDate.year, 
                                                                    rangeBound.specificDate.month, 
                                                                    rangeBound.specificDate.day) 
                                                           : null,
                    chooser = {
                    value: ko.observable(rangeBound.value),
                    specificDate: ko.observable(specificDate),
                    windowAmount: ko.observable(rangeBound.windowAmount),
                    windowInterval: ko.observable(rangeBound.windowInterval)
                };

                chooser.showSpecificDateSection = ko.dependentObservable(function () {
                    return this.value() === 'specific-date';
                }, chooser);
                chooser.showWindowSection = ko.dependentObservable(function () {
                    return this.value() === 'window';
                }, chooser);

                chooser.toJSON = function () {
                    return {
                        value: chooser.value(),
                        specificDate: chooser.specificDate(),
                        windowAmount: chooser.windowAmount(),
                        windowInterval: chooser.windowInterval()
                    };
                };

                return chooser;
            },
            $dateRangeWrap = $('#eng-date-range-wrap'),
            viewModel = {
                start: createRangeChooser(data.start), 
                end: createRangeChooser(data.end),
                exampleDateRangeHtml: ko.observable(''),
                dateRangeIsValid: ko.observable(true)
            },
            dateRangeResponseCache = {},
            updateViewModelWithFormatResponse = function (dateRangeResponse) {
                viewModel.dateRangeIsValid(dateRangeResponse.isError);
                viewModel.exampleDateRangeHtml(dateRangeResponse.message);
            },
            delayIfClientControlNotInitlialized;

        ko.dependentObservable(function () {
            var viewModelJson = JSON.stringify(viewModel),
                cachedDateRangeResponse = dateRangeResponseCache[viewModelJson];
            
            if (cachedDateRangeResponse) {
                updateViewModelWithFormatResponse(cachedDateRangeResponse);
                return;
            }
            
            $.ajax({
                    type: 'POST',
                    url: data.serviceUrl + '/FormatDateRange',
                    data: viewModelJson,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (dateRangeResponse) {
                        if (dateRangeResponse.hasOwnProperty('d')) {
                            dateRangeResponse = dateRangeResponse.d;
                        }

                        dateRangeResponseCache[viewModelJson] = dateRangeResponse;
                        updateViewModelWithFormatResponse(dateRangeResponse);
                    }
                });
            
        });

        delayIfClientControlNotInitlialized = function ($element, callback) {
            var control = getClientControl($element);
            if (!control) {
                setTimeout(function () {
                    delayIfClientControlNotInitlialized($element, callback);
                }, 0);
                return;
            }

            callback(control);
        };

        ko.bindingHandlers.date = {
            init: function (element, getValue) {
                var dateObservable = getValue();
                delayIfClientControlNotInitlialized($(element), function (datePicker) {
                    datePicker.add_dateSelected(function (sender, args) {
                        dateObservable(args.get_newDate());
                    }); 
                });
            },
            update: function (element, getValue) {
                var dateValue = ko.utils.unwrapObservable(getValue());
                delayIfClientControlNotInitlialized($(element), function (datePicker) {
                    datePicker.set_selectedDate(dateValue);
                });
            }
        };

        ko.bindingHandlers.integer = {
            init: function (element, getValue) {
                var intObservable = getValue();
                delayIfClientControlNotInitlialized($(element), function (textBox) {
                    textBox.add_valueChanged(function (sender, args) {
                        intObservable(args.get_newValue());
                    });                    
                });
            },
            update: function (element, getValue) {
                var intValue = ko.utils.unwrapObservable(getValue());
                delayIfClientControlNotInitlialized($(element), function (textBox) {
                    textBox.set_value(intValue);
                });
            }
        };

        ko.applyBindings(viewModel, $dateRangeWrap.get(0));
    });
}(jQuery, this, ko, Sys.Application.findComponent));