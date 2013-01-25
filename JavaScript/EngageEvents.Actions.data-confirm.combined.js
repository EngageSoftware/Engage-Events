/*global jQuery */
/*jslint evil:true, sloppy:true */
/*jshint strict:false */

(function ($) {
    // Do _not_ add "use strict", because doPostback hates it
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
/*globals jQuery, confirm */

jQuery(function ($) {
    'use strict';
    $('[data-eng-events-confirm]').replaceOnclick(function () {
        return confirm($(this).attr('data-eng-events-confirm'));
    });
});
