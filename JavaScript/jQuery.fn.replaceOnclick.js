/*globals jQuery */
/*jslint evil:true */

(function ($) {
    'use strict';
    $.fn.replaceOnclick = $.fn.replaceOnclick || function (callback) {
        return this.each(function () {
            $(this).data('original-onclick', $(this).attr('onclick'));
        })
            .removeAttr('onclick')
            .click(function () {
                if (!$.isFunction(callback) || callback.call(this)) {
                    eval($(this).data('original-onclick'));
                } else {
                    return false;
                }
            });
    };
}(jQuery));