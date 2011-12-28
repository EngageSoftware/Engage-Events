/*globals jQuery, confirm */

jQuery(function ($) {
    'use strict';
    $('[data-eng-events-confirm]').replaceOnclick(function () {
        return confirm($(this).attr('data-eng-events-confirm'));
    });
});