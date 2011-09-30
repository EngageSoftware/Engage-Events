/*
    http://www.JSON.org/json2.js
    2011-02-23

    Public Domain.

    NO WARRANTY EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.

    See http://www.JSON.org/js.html


    This code should be minified before deployment.
    See http://javascript.crockford.com/jsmin.html

    USE YOUR OWN COPY. IT IS EXTREMELY UNWISE TO LOAD CODE FROM SERVERS YOU DO
    NOT CONTROL.


    This file creates a global JSON object containing two methods: stringify
    and parse.

        JSON.stringify(value, replacer, space)
            value       any JavaScript value, usually an object or array.

            replacer    an optional parameter that determines how object
                        values are stringified for objects. It can be a
                        function or an array of strings.

            space       an optional parameter that specifies the indentation
                        of nested structures. If it is omitted, the text will
                        be packed without extra whitespace. If it is a number,
                        it will specify the number of spaces to indent at each
                        level. If it is a string (such as '\t' or '&nbsp;'),
                        it contains the characters used to indent at each level.

            This method produces a JSON text from a JavaScript value.

            When an object value is found, if the object contains a toJSON
            method, its toJSON method will be called and the result will be
            stringified. A toJSON method does not serialize: it returns the
            value represented by the name/value pair that should be serialized,
            or undefined if nothing should be serialized. The toJSON method
            will be passed the key associated with the value, and this will be
            bound to the value

            For example, this would serialize Dates as ISO strings.

                Date.prototype.toJSON = function (key) {
                    function f(n) {
                        // Format integers to have at least two digits.
                        return n < 10 ? '0' + n : n;
                    }

                    return this.getUTCFullYear()   + '-' +
                         f(this.getUTCMonth() + 1) + '-' +
                         f(this.getUTCDate())      + 'T' +
                         f(this.getUTCHours())     + ':' +
                         f(this.getUTCMinutes())   + ':' +
                         f(this.getUTCSeconds())   + 'Z';
                };

            You can provide an optional replacer method. It will be passed the
            key and value of each member, with this bound to the containing
            object. The value that is returned from your method will be
            serialized. If your method returns undefined, then the member will
            be excluded from the serialization.

            If the replacer parameter is an array of strings, then it will be
            used to select the members to be serialized. It filters the results
            such that only members with keys listed in the replacer array are
            stringified.

            Values that do not have JSON representations, such as undefined or
            functions, will not be serialized. Such values in objects will be
            dropped; in arrays they will be replaced with null. You can use
            a replacer function to replace those with JSON values.
            JSON.stringify(undefined) returns undefined.

            The optional space parameter produces a stringification of the
            value that is filled with line breaks and indentation to make it
            easier to read.

            If the space parameter is a non-empty string, then that string will
            be used for indentation. If the space parameter is a number, then
            the indentation will be that many spaces.

            Example:

            text = JSON.stringify(['e', {pluribus: 'unum'}]);
            // text is '["e",{"pluribus":"unum"}]'


            text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
            // text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'

            text = JSON.stringify([new Date()], function (key, value) {
                return this[key] instanceof Date ?
                    'Date(' + this[key] + ')' : value;
            });
            // text is '["Date(---current time---)"]'


        JSON.parse(text, reviver)
            This method parses a JSON text to produce an object or array.
            It can throw a SyntaxError exception.

            The optional reviver parameter is a function that can filter and
            transform the results. It receives each of the keys and values,
            and its return value is used instead of the original value.
            If it returns what it received, then the structure is not modified.
            If it returns undefined then the member is deleted.

            Example:

            // Parse the text. Values that look like ISO date strings will
            // be converted to Date objects.

            myData = JSON.parse(text, function (key, value) {
                var a;
                if (typeof value === 'string') {
                    a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                    if (a) {
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
                            +a[5], +a[6]));
                    }
                }
                return value;
            });

            myData = JSON.parse('["Date(09/09/2001)"]', function (key, value) {
                var d;
                if (typeof value === 'string' &&
                        value.slice(0, 5) === 'Date(' &&
                        value.slice(-1) === ')') {
                    d = new Date(value.slice(5, -1));
                    if (d) {
                        return d;
                    }
                }
                return value;
            });


    This is a reference implementation. You are free to copy, modify, or
    redistribute.
*/

/*jslint evil: true, strict: false, regexp: false */

/*members "", "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
    call, charCodeAt, getUTCDate, getUTCFullYear, getUTCHours,
    getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join,
    lastIndex, length, parse, prototype, push, replace, slice, stringify,
    test, toJSON, toString, valueOf
*/


// Create a JSON object only if one does not already exist. We create the
// methods in a closure to avoid creating global variables.

var JSON;
if (!JSON) {
    JSON = {};
}

(function () {
    "use strict";

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {

        Date.prototype.toJSON = function (key) {

            return isFinite(this.valueOf()) ?
                this.getUTCFullYear()     + '-' +
                f(this.getUTCMonth() + 1) + '-' +
                f(this.getUTCDate())      + 'T' +
                f(this.getUTCHours())     + ':' +
                f(this.getUTCMinutes())   + ':' +
                f(this.getUTCSeconds())   + 'Z' : null;
        };

        String.prototype.toJSON      =
            Number.prototype.toJSON  =
            Boolean.prototype.toJSON = function (key) {
                return this.valueOf();
            };
    }

    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"' : '\\"',
            '\\': '\\\\'
        },
        rep;


    function quote(string) {

// If the string contains no control characters, no quote characters, and no
// backslash characters, then we can safely slap some quotes around it.
// Otherwise we must also replace the offending characters with safe escape
// sequences.

        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            return typeof c === 'string' ? c :
                '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        }) + '"' : '"' + string + '"';
    }


    function str(key, holder) {

// Produce a string from holder[key].

        var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

// If the value has a toJSON method, call it to obtain a replacement value.

        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }

// If we were called with a replacer function, then call the replacer to
// obtain a replacement value.

        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }

// What happens next depends on the value's type.

        switch (typeof value) {
        case 'string':
            return quote(value);

        case 'number':

// JSON numbers must be finite. Encode non-finite numbers as null.

            return isFinite(value) ? String(value) : 'null';

        case 'boolean':
        case 'null':

// If the value is a boolean or null, convert it to a string. Note:
// typeof null does not produce 'null'. The case is included here in
// the remote chance that this gets fixed someday.

            return String(value);

// If the type is 'object', we might be dealing with an object or an array or
// null.

        case 'object':

// Due to a specification blunder in ECMAScript, typeof null is 'object',
// so watch out for that case.

            if (!value) {
                return 'null';
            }

// Make an array to hold the partial results of stringifying this object value.

            gap += indent;
            partial = [];

// Is the value an array?

            if (Object.prototype.toString.apply(value) === '[object Array]') {

// The value is an array. Stringify every element. Use null as a placeholder
// for non-JSON values.

                length = value.length;
                for (i = 0; i < length; i += 1) {
                    partial[i] = str(i, value) || 'null';
                }

// Join all of the elements together, separated with commas, and wrap them in
// brackets.

                v = partial.length === 0 ? '[]' : gap ?
                    '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' :
                    '[' + partial.join(',') + ']';
                gap = mind;
                return v;
            }

// If the replacer is an array, use it to select the members to be stringified.

            if (rep && typeof rep === 'object') {
                length = rep.length;
                for (i = 0; i < length; i += 1) {
                    if (typeof rep[i] === 'string') {
                        k = rep[i];
                        v = str(k, value);
                        if (v) {
                            partial.push(quote(k) + (gap ? ': ' : ':') + v);
                        }
                    }
                }
            } else {

// Otherwise, iterate through all of the keys in the object.

                for (k in value) {
                    if (Object.prototype.hasOwnProperty.call(value, k)) {
                        v = str(k, value);
                        if (v) {
                            partial.push(quote(k) + (gap ? ': ' : ':') + v);
                        }
                    }
                }
            }

// Join all of the member texts together, separated with commas,
// and wrap them in braces.

            v = partial.length === 0 ? '{}' : gap ?
                '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' :
                '{' + partial.join(',') + '}';
            gap = mind;
            return v;
        }
    }

// If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function (value, replacer, space) {

// The stringify method takes a value and an optional replacer, and an optional
// space parameter, and returns a JSON text. The replacer can be a function
// that can replace values, or an array of strings that will select the keys.
// A default replacer method can be provided. Use of the space parameter can
// produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

// If the space parameter is a number, make an indent string containing that
// many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

// If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

// If there is a replacer, it must be a function or an array.
// Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                    typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

// Make a fake root object containing our value under the key of ''.
// Return the result of stringifying the value.

            return str('', {'': value});
        };
    }


// If the JSON object does not yet have a parse method, give it one.

    if (typeof JSON.parse !== 'function') {
        JSON.parse = function (text, reviver) {

// The parse method takes a text and an optional reviver function, and returns
// a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

// The walk method is used to recursively walk the resulting structure so
// that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


// Parsing happens in four stages. In the first stage, we replace certain
// Unicode characters with escape sequences. JavaScript handles many characters
// incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

// In the second stage, we run the text against regular expressions that look
// for non-JSON patterns. We are especially concerned with '()' and 'new'
// because they can cause invocation, and '=' because it can cause mutation.
// But just to be safe, we want to reject all unexpected forms.

// We split the second stage into 4 regexp operations in order to work around
// crippling inefficiencies in IE's and Safari's regexp engines. First we
// replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
// replace all simple value tokens with ']' characters. Third, we delete all
// open brackets that follow a colon or comma or that begin the text. Finally,
// we look to see that the remaining characters are only whitespace or ']' or
// ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/
                    .test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@')
                        .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
                        .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

// In the third stage we use the eval function to compile the text into a
// JavaScript structure. The '{' operator is subject to a syntactic ambiguity
// in JavaScript: it can begin a block or an object literal. We wrap the text
// in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

// In the optional fourth stage, we recursively walk the new structure, passing
// each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function' ?
                    walk({'': j}, '') : j;
            }

// If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }
}());

/*! Knockout JavaScript library v1.2.1
 * (c) Steven Sanderson - http://knockoutjs.com/
 * License: MIT (http://www.opensource.org/licenses/mit-license.php) */

(function(window,undefined){ 
function c(e){throw e;}var m=void 0,o=null,p=window.ko={};p.b=function(e,d){for(var b=e.split("."),a=window,f=0;f<b.length-1;f++)a=a[b[f]];a[b[b.length-1]]=d};p.i=function(e,d,b){e[d]=b};
p.a=new function(){function e(a,b){if(a.tagName!="INPUT"||!a.type)return!1;if(b.toLowerCase()!="click")return!1;var d=a.type.toLowerCase();return d=="checkbox"||d=="radio"}var d=/^(\s|\u00A0)+|(\s|\u00A0)+$/g,b=/MSIE 6/i.test(navigator.userAgent),a=/MSIE 7/i.test(navigator.userAgent),f={},h={};f[/Firefox\/2/i.test(navigator.userAgent)?"KeyboardEvent":"UIEvents"]=["keyup","keydown","keypress"];f.MouseEvents=["click","dblclick","mousedown","mouseup","mousemove","mouseover","mouseout","mouseenter","mouseleave"];
for(var g in f){var i=f[g];if(i.length)for(var k=0,j=i.length;k<j;k++)h[i[k]]=g}return{ca:["authenticity_token",/^__RequestVerificationToken(_.*)?$/],g:function(a,b){for(var d=0,e=a.length;d<e;d++)b(a[d])},h:function(a,b){if(typeof a.indexOf=="function")return a.indexOf(b);for(var d=0,e=a.length;d<e;d++)if(a[d]===b)return d;return-1},xa:function(a,b,d){for(var e=0,f=a.length;e<f;e++)if(b.call(d,a[e]))return a[e];return o},N:function(a,b){var d=p.a.h(a,b);d>=0&&a.splice(d,1)},L:function(a){for(var a=
a||[],b=[],d=0,e=a.length;d<e;d++)p.a.h(b,a[d])<0&&b.push(a[d]);return b},M:function(a,b){for(var a=a||[],d=[],e=0,f=a.length;e<f;e++)d.push(b(a[e]));return d},K:function(a,b){for(var a=a||[],d=[],e=0,f=a.length;e<f;e++)b(a[e])&&d.push(a[e]);return d},u:function(a,b){for(var d=0,e=b.length;d<e;d++)a.push(b[d])},Q:function(a){for(;a.firstChild;)p.removeNode(a.firstChild)},Xa:function(a,b){p.a.Q(a);b&&p.a.g(b,function(b){a.appendChild(b)})},ka:function(a,b){var d=a.nodeType?[a]:a;if(d.length>0){for(var e=
d[0],f=e.parentNode,h=0,g=b.length;h<g;h++)f.insertBefore(b[h],e);h=0;for(g=d.length;h<g;h++)p.removeNode(d[h])}},ma:function(a,b){navigator.userAgent.indexOf("MSIE 6")>=0?a.setAttribute("selected",b):a.selected=b},da:function(a,b){if(!a||a.nodeType!=1)return[];var d=[];a.getAttribute(b)!==o&&d.push(a);for(var e=a.getElementsByTagName("*"),f=0,h=e.length;f<h;f++)e[f].getAttribute(b)!==o&&d.push(e[f]);return d},k:function(a){return(a||"").replace(d,"")},ab:function(a,b){for(var d=[],e=(a||"").split(b),
f=0,h=e.length;f<h;f++){var g=p.a.k(e[f]);g!==""&&d.push(g)}return d},Za:function(a,b){a=a||"";if(b.length>a.length)return!1;return a.substring(0,b.length)===b},Ha:function(a,b){if(b===m)return(new Function("return "+a))();return(new Function("sc","with(sc) { return ("+a+") }"))(b)},Fa:function(a,b){if(b.compareDocumentPosition)return(b.compareDocumentPosition(a)&16)==16;for(;a!=o;){if(a==b)return!0;a=a.parentNode}return!1},P:function(a){return p.a.Fa(a,document)},t:function(a,b,d){if(typeof jQuery!=
"undefined"){if(e(a,b))var f=d,d=function(a,b){var d=this.checked;if(b)this.checked=b.Aa!==!0;f.call(this,a);this.checked=d};jQuery(a).bind(b,d)}else typeof a.addEventListener=="function"?a.addEventListener(b,d,!1):typeof a.attachEvent!="undefined"?a.attachEvent("on"+b,function(b){d.call(a,b)}):c(Error("Browser doesn't support addEventListener or attachEvent"))},qa:function(a,b){(!a||!a.nodeType)&&c(Error("element must be a DOM node when calling triggerEvent"));if(typeof jQuery!="undefined"){var d=
[];e(a,b)&&d.push({Aa:a.checked});jQuery(a).trigger(b,d)}else if(typeof document.createEvent=="function")typeof a.dispatchEvent=="function"?(d=document.createEvent(h[b]||"HTMLEvents"),d.initEvent(b,!0,!0,window,0,0,0,0,0,!1,!1,!1,!1,0,a),a.dispatchEvent(d)):c(Error("The supplied element doesn't support dispatchEvent"));else if(typeof a.fireEvent!="undefined"){if(b=="click"&&a.tagName=="INPUT"&&(a.type.toLowerCase()=="checkbox"||a.type.toLowerCase()=="radio"))a.checked=a.checked!==!0;a.fireEvent("on"+
b)}else c(Error("Browser doesn't support triggering events"))},d:function(a){return p.C(a)?a():a},Ea:function(a,b){return p.a.h((a.className||"").split(/\s+/),b)>=0},pa:function(a,b,d){var e=p.a.Ea(a,b);if(d&&!e)a.className=(a.className||"")+" "+b;else if(e&&!d){for(var d=(a.className||"").split(/\s+/),e="",f=0;f<d.length;f++)d[f]!=b&&(e+=d[f]+" ");a.className=p.a.k(e)}},Ua:function(a,b){for(var a=p.a.d(a),b=p.a.d(b),d=[],e=a;e<=b;e++)d.push(e);return d},U:function(a){for(var b=[],d=0,e=a.length;d<
e;d++)b.push(a[d]);return b},S:b,Ma:a,ea:function(a,b){for(var d=p.a.U(a.getElementsByTagName("INPUT")).concat(p.a.U(a.getElementsByTagName("TEXTAREA"))),e=typeof b=="string"?function(a){return a.name===b}:function(a){return b.test(a.name)},f=[],h=d.length-1;h>=0;h--)e(d[h])&&f.push(d[h]);return f},F:function(a){if(typeof a=="string"&&(a=p.a.k(a))){if(window.JSON&&window.JSON.parse)return window.JSON.parse(a);return(new Function("return "+a))()}return o},Y:function(a){(typeof JSON=="undefined"||typeof JSON.stringify==
"undefined")&&c(Error("Cannot find JSON.stringify(). Some browsers (e.g., IE < 8) don't support it natively, but you can overcome this by adding a script reference to json2.js, downloadable from http://www.json.org/json2.js"));return JSON.stringify(p.a.d(a))},Ta:function(a,b,d){var d=d||{},e=d.params||{},f=d.includeFields||this.ca,h=a;if(typeof a=="object"&&a.tagName=="FORM")for(var h=a.action,g=f.length-1;g>=0;g--)for(var i=p.a.ea(a,f[g]),k=i.length-1;k>=0;k--)e[i[k].name]=i[k].value;var b=p.a.d(b),
j=document.createElement("FORM");j.style.display="none";j.action=h;j.method="post";for(var u in b)a=document.createElement("INPUT"),a.name=u,a.value=p.a.Y(p.a.d(b[u])),j.appendChild(a);for(u in e)a=document.createElement("INPUT"),a.name=u,a.value=e[u],j.appendChild(a);document.body.appendChild(j);d.submitter?d.submitter(j):j.submit();setTimeout(function(){j.parentNode.removeChild(j)},0)}}};p.b("ko.utils",p.a);p.b("ko.utils.arrayForEach",p.a.g);p.b("ko.utils.arrayFirst",p.a.xa);
p.b("ko.utils.arrayFilter",p.a.K);p.b("ko.utils.arrayGetDistinctValues",p.a.L);p.b("ko.utils.arrayIndexOf",p.a.h);p.b("ko.utils.arrayMap",p.a.M);p.b("ko.utils.arrayPushAll",p.a.u);p.b("ko.utils.arrayRemoveItem",p.a.N);p.b("ko.utils.fieldsIncludedWithJsonPost",p.a.ca);p.b("ko.utils.getElementsHavingAttribute",p.a.da);p.b("ko.utils.getFormFields",p.a.ea);p.b("ko.utils.postJson",p.a.Ta);p.b("ko.utils.parseJson",p.a.F);p.b("ko.utils.registerEventHandler",p.a.t);p.b("ko.utils.stringifyJson",p.a.Y);
p.b("ko.utils.range",p.a.Ua);p.b("ko.utils.toggleDomNodeCssClass",p.a.pa);p.b("ko.utils.triggerEvent",p.a.qa);p.b("ko.utils.unwrapObservable",p.a.d);Function.prototype.bind||(Function.prototype.bind=function(e){var d=this,b=Array.prototype.slice.call(arguments),e=b.shift();return function(){return d.apply(e,b.concat(Array.prototype.slice.call(arguments)))}});
p.a.e=new function(){var e=0,d="__ko__"+(new Date).getTime(),b={};return{get:function(a,b){var d=p.a.e.getAll(a,!1);return d===m?m:d[b]},set:function(a,b,d){d===m&&p.a.e.getAll(a,!1)===m||(p.a.e.getAll(a,!0)[b]=d)},getAll:function(a,f){var h=a[d];if(!h){if(!f)return;h=a[d]="ko"+e++;b[h]={}}return b[h]},clear:function(a){var e=a[d];e&&(delete b[e],a[d]=o)}}};
p.a.p=new function(){function e(a,d){var e=p.a.e.get(a,b);e===m&&d&&(e=[],p.a.e.set(a,b,e));return e}function d(a){var b=e(a,!1);if(b)for(var b=b.slice(0),d=0;d<b.length;d++)b[d](a);p.a.e.clear(a);typeof jQuery=="function"&&typeof jQuery.cleanData=="function"&&jQuery.cleanData([a])}var b="__ko_domNodeDisposal__"+(new Date).getTime();return{ba:function(a,b){typeof b!="function"&&c(Error("Callback must be a function"));e(a,!0).push(b)},ja:function(a,d){var h=e(a,!1);h&&(p.a.N(h,d),h.length==0&&p.a.e.set(a,
b,m))},v:function(a){if(!(a.nodeType!=1&&a.nodeType!=9)){d(a);var b=[];p.a.u(b,a.getElementsByTagName("*"));for(var a=0,e=b.length;a<e;a++)d(b[a])}},removeNode:function(a){p.v(a);a.parentNode&&a.parentNode.removeChild(a)}}};p.v=p.a.p.v;p.removeNode=p.a.p.removeNode;p.b("ko.cleanNode",p.v);p.b("ko.removeNode",p.removeNode);p.b("ko.utils.domNodeDisposal",p.a.p);p.b("ko.utils.domNodeDisposal.addDisposeCallback",p.a.p.ba);p.b("ko.utils.domNodeDisposal.removeDisposeCallback",p.a.p.ja);
p.a.Sa=function(e){if(typeof jQuery!="undefined")e=jQuery.clean([e]);else{var d=p.a.k(e).toLowerCase(),b=document.createElement("div"),d=d.match(/^<(thead|tbody|tfoot)/)&&[1,"<table>","</table>"]||!d.indexOf("<tr")&&[2,"<table><tbody>","</tbody></table>"]||(!d.indexOf("<td")||!d.indexOf("<th"))&&[3,"<table><tbody><tr>","</tr></tbody></table>"]||[0,"",""];for(b.innerHTML=d[1]+e+d[2];d[0]--;)b=b.lastChild;e=p.a.U(b.childNodes)}return e};
p.a.Ya=function(e,d){p.a.Q(e);if(d!==o&&d!==m)if(typeof d!="string"&&(d=d.toString()),typeof jQuery!="undefined")jQuery(e).html(d);else for(var b=p.a.Sa(d),a=0;a<b.length;a++)e.appendChild(b[a])};
p.l=function(){function e(){return((1+Math.random())*4294967296|0).toString(16).substring(1)}function d(a,b){if(a)if(a.nodeType==8){var e=p.l.ha(a.nodeValue);e!=o&&b.push({Da:a,Pa:e})}else if(a.nodeType==1)for(var e=0,g=a.childNodes,i=g.length;e<i;e++)d(g[e],b)}var b={};return{V:function(a){typeof a!="function"&&c(Error("You can only pass a function to ko.memoization.memoize()"));var d=e()+e();b[d]=a;return"<\!--[ko_memo:"+d+"]--\>"},ra:function(a,d){var e=b[a];e===m&&c(Error("Couldn't find any memo with ID "+
a+". Perhaps it's already been unmemoized."));try{return e.apply(o,d||[]),!0}finally{delete b[a]}},sa:function(a,b){var e=[];d(a,e);for(var g=0,i=e.length;g<i;g++){var k=e[g].Da,j=[k];b&&p.a.u(j,b);p.l.ra(e[g].Pa,j);k.nodeValue="";k.parentNode&&k.parentNode.removeChild(k)}},ha:function(a){return(a=a.match(/^\[ko_memo\:(.*?)\]$/))?a[1]:o}}}();p.b("ko.memoization",p.l);p.b("ko.memoization.memoize",p.l.V);p.b("ko.memoization.unmemoize",p.l.ra);p.b("ko.memoization.parseMemoText",p.l.ha);
p.b("ko.memoization.unmemoizeDomNodeAndDescendants",p.l.sa);p.$a=function(e,d){this.za=e;this.n=function(){this.La=!0;d()}.bind(this);p.i(this,"dispose",this.n)};p.Z=function(){var e=[];this.$=function(d,b){var a=b?d.bind(b):d,f=new p.$a(a,function(){p.a.N(e,f)});e.push(f);return f};this.z=function(d){p.a.g(e.slice(0),function(b){b&&b.La!==!0&&b.za(d)})};this.Ja=function(){return e.length};p.i(this,"subscribe",this.$);p.i(this,"notifySubscribers",this.z);p.i(this,"getSubscriptionsCount",this.Ja)};
p.ga=function(e){return typeof e.$=="function"&&typeof e.z=="function"};p.b("ko.subscribable",p.Z);p.b("ko.isSubscribable",p.ga);p.A=function(){var e=[];return{ya:function(){e.push([])},end:function(){return e.pop()},ia:function(d){p.ga(d)||c("Only subscribable things can act as dependencies");e.length>0&&e[e.length-1].push(d)}}}();var x={undefined:!0,"boolean":!0,number:!0,string:!0};function y(e,d){return e===o||typeof e in x?e===d:!1}
p.s=function(e){function d(){if(arguments.length>0){if(!d.equalityComparer||!d.equalityComparer(b,arguments[0]))b=arguments[0],d.z(b);return this}else return p.A.ia(d),b}var b=e;d.o=p.s;d.H=function(){d.z(b)};d.equalityComparer=y;p.Z.call(d);p.i(d,"valueHasMutated",d.H);return d};p.C=function(e){if(e===o||e===m||e.o===m)return!1;if(e.o===p.s)return!0;return p.C(e.o)};p.D=function(e){if(typeof e=="function"&&e.o===p.s)return!0;if(typeof e=="function"&&e.o===p.j&&e.Ka)return!0;return!1};
p.b("ko.observable",p.s);p.b("ko.isObservable",p.C);p.b("ko.isWriteableObservable",p.D);
p.Ra=function(e){arguments.length==0&&(e=[]);e!==o&&e!==m&&!("length"in e)&&c(Error("The argument passed when initializing an observable array must be an array, or null, or undefined."));var d=new p.s(e);p.a.g(["pop","push","reverse","shift","sort","splice","unshift"],function(b){d[b]=function(){var a=d(),a=a[b].apply(a,arguments);d.H();return a}});p.a.g(["slice"],function(b){d[b]=function(){var a=d();return a[b].apply(a,arguments)}});d.remove=function(b){for(var a=d(),e=[],h=[],g=typeof b=="function"?
b:function(a){return a===b},i=0,k=a.length;i<k;i++){var j=a[i];g(j)?h.push(j):e.push(j)}d(e);return h};d.Va=function(b){if(b===m){var a=d();d([]);return a}if(!b)return[];return d.remove(function(a){return p.a.h(b,a)>=0})};d.O=function(b){for(var a=d(),e=typeof b=="function"?b:function(a){return a===b},h=a.length-1;h>=0;h--)e(a[h])&&(a[h]._destroy=!0);d.H()};d.Ca=function(b){if(b===m)return d.O(function(){return!0});if(!b)return[];return d.O(function(a){return p.a.h(b,a)>=0})};d.indexOf=function(b){var a=
d();return p.a.h(a,b)};d.replace=function(b,a){var e=d.indexOf(b);e>=0&&(d()[e]=a,d.H())};p.i(d,"remove",d.remove);p.i(d,"removeAll",d.Va);p.i(d,"destroy",d.O);p.i(d,"destroyAll",d.Ca);p.i(d,"indexOf",d.indexOf);return d};p.b("ko.observableArray",p.Ra);
p.j=function(e,d,b){function a(){p.a.g(n,function(a){a.n()});n=[]}function f(b){a();p.a.g(b,function(a){n.push(a.$(h))})}function h(){if(k&&typeof b.disposeWhen=="function"&&b.disposeWhen())g.n();else{try{p.A.ya(),i=b.owner?b.read.call(b.owner):b.read()}finally{var a=p.a.L(p.A.end());f(a)}g.z(i);k=!0}}function g(){if(arguments.length>0)if(typeof b.write==="function"){var a=arguments[0];b.owner?b.write.call(b.owner,a):b.write(a)}else c("Cannot write a value to a dependentObservable unless you specify a 'write' option. If you wish to read the current value, don't pass any parameters.");
else return k||h(),p.A.ia(g),i}var i,k=!1;e&&typeof e=="object"?b=e:(b=b||{},b.read=e||b.read,b.owner=d||b.owner);typeof b.read!="function"&&c("Pass a function that returns the value of the dependentObservable");var j=typeof b.disposeWhenNodeIsRemoved=="object"?b.disposeWhenNodeIsRemoved:o,l=o;if(j){l=function(){g.n()};p.a.p.ba(j,l);var q=b.disposeWhen;b.disposeWhen=function(){return!p.a.P(j)||typeof q=="function"&&q()}}var n=[];g.o=p.j;g.Ia=function(){return n.length};g.Ka=typeof b.write==="function";
g.n=function(){j&&p.a.p.ja(j,l);a()};p.Z.call(g);b.deferEvaluation!==!0&&h();p.i(g,"dispose",g.n);p.i(g,"getDependenciesCount",g.Ia);return g};p.j.o=p.s;p.b("ko.dependentObservable",p.j);
(function(){function e(a,f,h){h=h||new b;a=f(a);if(!(typeof a=="object"&&a!==o&&a!==m))return a;var g=a instanceof Array?[]:{};h.save(a,g);d(a,function(b){var d=f(a[b]);switch(typeof d){case "boolean":case "number":case "string":case "function":g[b]=d;break;case "object":case "undefined":var j=h.get(d);g[b]=j!==m?j:e(d,f,h)}});return g}function d(a,b){if(a instanceof Array)for(var d=0;d<a.length;d++)b(d);else for(d in a)b(d)}function b(){var a=[],b=[];this.save=function(d,e){var i=p.a.h(a,d);i>=0?
b[i]=e:(a.push(d),b.push(e))};this.get=function(d){d=p.a.h(a,d);return d>=0?b[d]:m}}p.oa=function(a){arguments.length==0&&c(Error("When calling ko.toJS, pass the object you want to convert."));return e(a,function(a){for(var b=0;p.C(a)&&b<10;b++)a=a();return a})};p.toJSON=function(a){a=p.oa(a);return p.a.Y(a)}})();p.b("ko.toJS",p.oa);p.b("ko.toJSON",p.toJSON);
p.f={m:function(e){if(e.tagName=="OPTION"){if(e.__ko__hasDomDataOptionValue__===!0)return p.a.e.get(e,p.c.options.W);return e.getAttribute("value")}else return e.tagName=="SELECT"?e.selectedIndex>=0?p.f.m(e.options[e.selectedIndex]):m:e.value},I:function(e,d){if(e.tagName=="OPTION")switch(typeof d){case "string":case "number":p.a.e.set(e,p.c.options.W,m);"__ko__hasDomDataOptionValue__"in e&&delete e.__ko__hasDomDataOptionValue__;e.value=d;break;default:p.a.e.set(e,p.c.options.W,d),e.__ko__hasDomDataOptionValue__=
!0,e.value=""}else if(e.tagName=="SELECT")for(var b=e.options.length-1;b>=0;b--){if(p.f.m(e.options[b])==d){e.selectedIndex=b;break}}else{if(d===o||d===m)d="";e.value=d}}};p.b("ko.selectExtensions",p.f);p.b("ko.selectExtensions.readValue",p.f.m);p.b("ko.selectExtensions.writeValue",p.f.I);
p.r=function(){function e(a,b){return a.replace(d,function(a,d){return b[d]})}var d=/\[ko_token_(\d+)\]/g,b=/^[\_$a-z][\_$a-z0-9]*(\[.*?\])*(\.[\_$a-z][\_$a-z0-9]*(\[.*?\])*)*$/i,a=["true","false"];return{F:function(a){a=p.a.k(a);if(a.length<3)return{};for(var b=[],d=o,i,k=a.charAt(0)=="{"?1:0;k<a.length;k++){var j=a.charAt(k);if(d===o)switch(j){case '"':case "'":case "/":d=k;i=j;break;case "{":d=k;i="}";break;case "[":d=k,i="]"}else if(j==i){j=a.substring(d,k+1);b.push(j);var l="[ko_token_"+(b.length-
1)+"]",a=a.substring(0,d)+l+a.substring(k+1);k-=j.length-l.length;d=o}}d={};a=a.split(",");i=0;for(k=a.length;i<k;i++){var l=a[i],q=l.indexOf(":");q>0&&q<l.length-1&&(j=p.a.k(l.substring(0,q)),l=p.a.k(l.substring(q+1)),j.charAt(0)=="{"&&(j=j.substring(1)),l.charAt(l.length-1)=="}"&&(l=l.substring(0,l.length-1)),j=p.a.k(e(j,b)),l=p.a.k(e(l,b)),d[j]=l)}return d},R:function(d){var e=p.r.F(d),g=[],i;for(i in e){var k=e[i],j;j=k;j=p.a.h(a,p.a.k(j).toLowerCase())>=0?!1:j.match(b)!==o;j&&(g.length>0&&g.push(", "),
g.push(i+" : function(__ko_value) { "+k+" = __ko_value; }"))}g.length>0&&(d=d+", '_ko_property_writers' : { "+g.join("")+" } ");return d}}}();p.b("ko.jsonExpressionRewriting",p.r);p.b("ko.jsonExpressionRewriting.parseJson",p.r.F);p.b("ko.jsonExpressionRewriting.insertPropertyAccessorsIntoJson",p.r.R);p.c={};
p.J=function(e,d,b,a){function f(a){return function(){return i[a]}}function h(){return i}var g=!0,a=a||"data-bind",i;new p.j(function(){var k;if(!(k=typeof d=="function"?d():d)){var j=e.getAttribute(a);try{var l=" { "+p.r.R(j)+" } ";k=p.a.Ha(l,b===o?window:b)}catch(q){c(Error("Unable to parse binding attribute.\nMessage: "+q+";\nAttribute value: "+j))}}i=k;if(g)for(var n in i)p.c[n]&&typeof p.c[n].init=="function"&&(0,p.c[n].init)(e,f(n),h,b);for(n in i)p.c[n]&&typeof p.c[n].update=="function"&&(0,p.c[n].update)(e,
f(n),h,b)},o,{disposeWhenNodeIsRemoved:e});g=!1};p.ua=function(e,d){d&&d.nodeType==m&&c(Error("ko.applyBindings: first parameter should be your view model; second parameter should be a DOM node (note: this is a breaking change since KO version 1.05)"));var d=d||window.document.body,b=p.a.da(d,"data-bind");p.a.g(b,function(a){p.J(a,o,e)})};p.b("ko.bindingHandlers",p.c);p.b("ko.applyBindings",p.ua);p.b("ko.applyBindingsToNode",p.J);
p.a.g(["click"],function(e){p.c[e]={init:function(d,b,a,f){return p.c.event.init.call(this,d,function(){var a={};a[e]=b();return a},a,f)}}});p.c.event={init:function(e,d,b,a){var f=d()||{},h;for(h in f)(function(){var f=h;typeof f=="string"&&p.a.t(e,f,function(e){var h,j=d()[f];if(j){var l=b();try{h=j.apply(a,arguments)}finally{if(h!==!0)e.preventDefault?e.preventDefault():e.returnValue=!1}if(l[f+"Bubble"]===!1)e.cancelBubble=!0,e.stopPropagation&&e.stopPropagation()}})})()}};
p.c.submit={init:function(e,d,b,a){typeof d()!="function"&&c(Error("The value for a submit binding must be a function to invoke on submit"));p.a.t(e,"submit",function(b){var h,g=d();try{h=g.call(a,e)}finally{if(h!==!0)b.preventDefault?b.preventDefault():b.returnValue=!1}})}};p.c.visible={update:function(e,d){var b=p.a.d(d()),a=e.style.display!="none";if(b&&!a)e.style.display="";else if(!b&&a)e.style.display="none"}};
p.c.enable={update:function(e,d){var b=p.a.d(d());if(b&&e.disabled)e.removeAttribute("disabled");else if(!b&&!e.disabled)e.disabled=!0}};p.c.disable={update:function(e,d){p.c.enable.update(e,function(){return!p.a.d(d())})}};
p.c.value={init:function(e,d,b){var a=["change"],f=b().valueUpdate;f&&(typeof f=="string"&&(f=[f]),p.a.u(a,f),a=p.a.L(a));p.a.g(a,function(a){var f=!1;p.a.Za(a,"after")&&(f=!0,a=a.substring(5));var i=f?function(a){setTimeout(a,0)}:function(a){a()};p.a.t(e,a,function(){i(function(){var a=d(),f=p.f.m(e);p.D(a)?a(f):(a=b(),a._ko_property_writers&&a._ko_property_writers.value&&a._ko_property_writers.value(f))})})})},update:function(e,d){var b=p.a.d(d()),a=p.f.m(e),f=b!=a;b===0&&a!==0&&a!=="0"&&(f=!0);
f&&(a=function(){p.f.I(e,b)},a(),e.tagName=="SELECT"&&setTimeout(a,0));e.tagName=="SELECT"&&(a=p.f.m(e),a!==b&&p.a.qa(e,"change"))}};
p.c.options={update:function(e,d,b){e.tagName!="SELECT"&&c(Error("options binding applies only to SELECT elements"));var a=p.a.M(p.a.K(e.childNodes,function(a){return a.tagName&&a.tagName=="OPTION"&&a.selected}),function(a){return p.f.m(a)||a.innerText||a.textContent}),f=e.scrollTop,h=p.a.d(d());p.a.Q(e);if(h){var g=b();typeof h.length!="number"&&(h=[h]);if(g.optionsCaption){var i=document.createElement("OPTION");i.innerHTML=g.optionsCaption;p.f.I(i,m);e.appendChild(i)}b=0;for(d=h.length;b<d;b++){var i=
document.createElement("OPTION"),k=typeof g.optionsValue=="string"?h[b][g.optionsValue]:h[b],k=p.a.d(k);p.f.I(i,k);var j=g.optionsText;optionText=typeof j=="function"?j(h[b]):typeof j=="string"?h[b][j]:k;if(optionText===o||optionText===m)optionText="";optionText=p.a.d(optionText).toString();typeof i.innerText=="string"?i.innerText=optionText:i.textContent=optionText;e.appendChild(i)}h=e.getElementsByTagName("OPTION");b=g=0;for(d=h.length;b<d;b++)p.a.h(a,p.f.m(h[b]))>=0&&(p.a.ma(h[b],!0),g++);if(f)e.scrollTop=
f}}};p.c.options.W="__ko.bindingHandlers.options.optionValueDomData__";
p.c.selectedOptions={fa:function(e){for(var d=[],e=e.childNodes,b=0,a=e.length;b<a;b++){var f=e[b];f.tagName=="OPTION"&&f.selected&&d.push(p.f.m(f))}return d},init:function(e,d,b){p.a.t(e,"change",function(){var a=d();p.D(a)?a(p.c.selectedOptions.fa(this)):(a=b(),a._ko_property_writers&&a._ko_property_writers.value&&a._ko_property_writers.value(p.c.selectedOptions.fa(this)))})},update:function(e,d){e.tagName!="SELECT"&&c(Error("values binding applies only to SELECT elements"));var b=p.a.d(d());if(b&&
typeof b.length=="number")for(var a=e.childNodes,f=0,h=a.length;f<h;f++){var g=a[f];g.tagName=="OPTION"&&p.a.ma(g,p.a.h(b,p.f.m(g))>=0)}}};p.c.text={update:function(e,d){var b=p.a.d(d());if(b===o||b===m)b="";typeof e.innerText=="string"?e.innerText=b:e.textContent=b}};p.c.html={update:function(e,d){var b=p.a.d(d());p.a.Ya(e,b)}};p.c.css={update:function(e,d){var b=p.a.d(d()||{}),a;for(a in b)if(typeof a=="string"){var f=p.a.d(b[a]);p.a.pa(e,a,f)}}};
p.c.style={update:function(e,d){var b=p.a.d(d()||{}),a;for(a in b)if(typeof a=="string"){var f=p.a.d(b[a]);e.style[a]=f||""}}};p.c.uniqueName={init:function(e,d){if(d())e.name="ko_unique_"+ ++p.c.uniqueName.Ba,p.a.S&&e.mergeAttributes(document.createElement("<input name='"+e.name+"'/>"),!1)}};p.c.uniqueName.Ba=0;
p.c.checked={init:function(e,d,b){p.a.t(e,"click",function(){var a;if(e.type=="checkbox")a=e.checked;else if(e.type=="radio"&&e.checked)a=e.value;else return;var f=d();e.type=="checkbox"&&p.a.d(f)instanceof Array?(a=p.a.h(p.a.d(f),e.value),e.checked&&a<0?f.push(e.value):!e.checked&&a>=0&&f.splice(a,1)):p.D(f)?f()!==a&&f(a):(f=b(),f._ko_property_writers&&f._ko_property_writers.checked&&f._ko_property_writers.checked(a))});e.type=="radio"&&!e.name&&p.c.uniqueName.init(e,function(){return!0})},update:function(e,
d){var b=p.a.d(d());if(e.type=="checkbox")e.checked=b instanceof Array?p.a.h(b,e.value)>=0:b,b&&p.a.S&&e.mergeAttributes(document.createElement("<input type='checkbox' checked='checked' />"),!1);else if(e.type=="radio")e.checked=e.value==b,e.value==b&&(p.a.S||p.a.Ma)&&e.mergeAttributes(document.createElement("<input type='radio' checked='checked' />"),!1)}};
p.c.attr={update:function(e,d){var b=p.a.d(d())||{},a;for(a in b)if(typeof a=="string"){var f=p.a.d(b[a]);f===!1||f===o||f===m?e.removeAttribute(a):e.setAttribute(a,f.toString())}}};
p.aa=function(){this.renderTemplate=function(){c("Override renderTemplate in your ko.templateEngine subclass")};this.isTemplateRewritten=function(){c("Override isTemplateRewritten in your ko.templateEngine subclass")};this.rewriteTemplate=function(){c("Override rewriteTemplate in your ko.templateEngine subclass")};this.createJavaScriptEvaluatorBlock=function(){c("Override createJavaScriptEvaluatorBlock in your ko.templateEngine subclass")}};p.b("ko.templateEngine",p.aa);
p.G=function(){var e=/(<[a-z]+\d*(\s+(?!data-bind=)[a-z0-9\-]+(=(\"[^\"]*\"|\'[^\']*\'))?)*\s+)data-bind=(["'])([\s\S]*?)\5/gi;return{Ga:function(d,b){b.isTemplateRewritten(d)||b.rewriteTemplate(d,function(a){return p.G.Qa(a,b)})},Qa:function(d,b){return d.replace(e,function(a,d,e,g,i,k,j){a=p.r.R(j);return b.createJavaScriptEvaluatorBlock("ko.templateRewriting.applyMemoizedBindingsToNextSibling(function() {                     return (function() { return { "+a+" } })()                 })")+d})},
va:function(d){return p.l.V(function(b,a){b.nextSibling&&p.J(b.nextSibling,d,a)})}}}();p.b("ko.templateRewriting",p.G);p.b("ko.templateRewriting.applyMemoizedBindingsToNextSibling",p.G.va);
(function(){function e(b,a,e,h,g){var i=p.a.d(h),g=g||{},k=g.templateEngine||d;p.G.Ga(e,k);e=k.renderTemplate(e,i,g);(typeof e.length!="number"||e.length>0&&typeof e[0].nodeType!="number")&&c("Template engine must return an array of DOM nodes");e&&p.a.g(e,function(a){p.l.sa(a,[h])});switch(a){case "replaceChildren":p.a.Xa(b,e);break;case "replaceNode":p.a.ka(b,e);break;case "ignoreTargetNode":break;default:c(Error("Unknown renderMode: "+a))}g.afterRender&&g.afterRender(e,h);return e}var d;p.na=function(b){b!=
m&&!(b instanceof p.aa)&&c("templateEngine must inherit from ko.templateEngine");d=b};p.X=function(b,a,f,h,g){f=f||{};(f.templateEngine||d)==m&&c("Set a template engine before calling renderTemplate");g=g||"replaceChildren";if(h){var i=h.nodeType?h:h.length>0?h[0]:o;return new p.j(function(){var d=typeof b=="function"?b(a):b,d=e(h,g,d,a,f);g=="replaceNode"&&(h=d,i=h.nodeType?h:h.length>0?h[0]:o)},o,{disposeWhen:function(){return!i||!p.a.P(i)},disposeWhenNodeIsRemoved:i&&g=="replaceNode"?i.parentNode:
i})}else return p.l.V(function(d){p.X(b,a,f,d,"replaceNode")})};p.Wa=function(b,a,d,h){return new p.j(function(){var g=p.a.d(a)||[];typeof g.length=="undefined"&&(g=[g]);g=p.a.K(g,function(a){return d.includeDestroyed||!a._destroy});p.a.la(h,g,function(a){var g=typeof b=="function"?b(a):b;return e(o,"ignoreTargetNode",g,a,d)},d)},o,{disposeWhenNodeIsRemoved:h})};p.c.template={update:function(b,a,d,e){a=p.a.d(a());d=typeof a=="string"?a:a.name;if(typeof a.foreach!="undefined")e=p.Wa(d,a.foreach||[],
{templateOptions:a.templateOptions,afterAdd:a.afterAdd,beforeRemove:a.beforeRemove,includeDestroyed:a.includeDestroyed,afterRender:a.afterRender},b);else var g=a.data,e=p.X(d,typeof g=="undefined"?e:g,{templateOptions:a.templateOptions,afterRender:a.afterRender},b);(a=p.a.e.get(b,"__ko__templateSubscriptionDomDataKey__"))&&typeof a.n=="function"&&a.n();p.a.e.set(b,"__ko__templateSubscriptionDomDataKey__",e)}}})();p.b("ko.setTemplateEngine",p.na);p.b("ko.renderTemplate",p.X);
p.a.w=function(e,d,b){if(b===m)return p.a.w(e,d,1)||p.a.w(e,d,10)||p.a.w(e,d,Number.MAX_VALUE);else{for(var e=e||[],d=d||[],a=e,f=d,h=[],g=0;g<=f.length;g++)h[g]=[];for(var g=0,i=Math.min(a.length,b);g<=i;g++)h[0][g]=g;g=1;for(i=Math.min(f.length,b);g<=i;g++)h[g][0]=g;for(var i=a.length,k,j=f.length,g=1;g<=i;g++){var l=Math.min(j,g+b);for(k=Math.max(1,g-b);k<=l;k++)h[k][g]=a[g-1]===f[k-1]?h[k-1][g-1]:Math.min(h[k-1][g]===m?Number.MAX_VALUE:h[k-1][g]+1,h[k][g-1]===m?Number.MAX_VALUE:h[k][g-1]+1)}b=
e.length;a=d.length;f=[];g=h[a][b];if(g===m)h=o;else{for(;b>0||a>0;){i=h[a][b];k=a>0?h[a-1][b]:g+1;j=b>0?h[a][b-1]:g+1;l=a>0&&b>0?h[a-1][b-1]:g+1;if(k===m||k<i-1)k=g+1;if(j===m||j<i-1)j=g+1;l<i-1&&(l=g+1);k<=j&&k<l?(f.push({status:"added",value:d[a-1]}),a--):(j<k&&j<l?f.push({status:"deleted",value:e[b-1]}):(f.push({status:"retained",value:e[b-1]}),a--),b--)}h=f.reverse()}return h}};p.b("ko.utils.compareArrays",p.a.w);
(function(){function e(d,b,a){var e=[],d=p.j(function(){var d=b(a)||[];e.length>0&&p.a.ka(e,d);e.splice(0,e.length);p.a.u(e,d)},o,{disposeWhenNodeIsRemoved:d,disposeWhen:function(){return e.length==0||!p.a.P(e[0])}});return{Oa:e,j:d}}p.a.la=function(d,b,a,f){for(var b=b||[],f=f||{},h=p.a.e.get(d,"setDomNodeChildrenFromArrayMapping_lastMappingResult")===m,g=p.a.e.get(d,"setDomNodeChildrenFromArrayMapping_lastMappingResult")||[],i=p.a.M(g,function(a){return a.wa}),k=p.a.w(i,b),b=[],j=0,l=[],i=[],q=
o,n=0,v=k.length;n<v;n++)switch(k[n].status){case "retained":var r=g[j];b.push(r);r.B.length>0&&(q=r.B[r.B.length-1]);j++;break;case "deleted":g[j].j.n();p.a.g(g[j].B,function(a){l.push({element:a,index:n,value:k[n].value});q=a});j++;break;case "added":var s=e(d,a,k[n].value),r=s.Oa;b.push({wa:k[n].value,B:r,j:s.j});for(var s=0,w=r.length;s<w;s++){var t=r[s];i.push({element:t,index:n,value:k[n].value});q==o?d.firstChild?d.insertBefore(t,d.firstChild):d.appendChild(t):q.nextSibling?d.insertBefore(t,
q.nextSibling):d.appendChild(t);q=t}}p.a.g(l,function(a){p.v(a.element)});a=!1;if(!h){if(f.afterAdd)for(n=0;n<i.length;n++)f.afterAdd(i[n].element,i[n].index,i[n].value);if(f.beforeRemove){for(n=0;n<l.length;n++)f.beforeRemove(l[n].element,l[n].index,l[n].value);a=!0}}a||p.a.g(l,function(a){a.element.parentNode&&a.element.parentNode.removeChild(a.element)});p.a.e.set(d,"setDomNodeChildrenFromArrayMapping_lastMappingResult",b)}})();p.b("ko.utils.setDomNodeChildrenFromArrayMapping",p.a.la);
p.T=function(){this.q=function(){if(typeof jQuery=="undefined"||!jQuery.tmpl)return 0;if(jQuery.tmpl.tag){if(jQuery.tmpl.tag.tmpl&&jQuery.tmpl.tag.tmpl.open&&jQuery.tmpl.tag.tmpl.open.toString().indexOf("__")>=0)return 3;return 2}return 1}();this.getTemplateNode=function(d){var b=document.getElementById(d);b==o&&c(Error("Cannot find template with ID="+d));return b};var e=RegExp("__ko_apos__","g");this.renderTemplate=function(d,b,a){a=a||{};this.q==0&&c(Error("jquery.tmpl not detected.\nTo use KO's default template engine, reference jQuery and jquery.tmpl. See Knockout installation documentation for more details."));
if(this.q==1)return d='<script type="text/html">'+this.getTemplateNode(d).text+"<\/script>",b=jQuery.tmpl(d,b)[0].text.replace(e,"'"),jQuery.clean([b],document);if(!(d in jQuery.template)){var f=this.getTemplateNode(d).text;jQuery.template(d,f)}b=[b];b=jQuery.tmpl(d,b,a.templateOptions);b.appendTo(document.createElement("div"));jQuery.fragments={};return b};this.isTemplateRewritten=function(d){if(d in jQuery.template)return!0;return this.getTemplateNode(d).Na===!0};this.rewriteTemplate=function(d,
b){var a=this.getTemplateNode(d),e=b(a.text);this.q==1&&(e=p.a.k(e),e=e.replace(/([\s\S]*?)(\${[\s\S]*?}|{{[\=a-z][\s\S]*?}}|$)/g,function(a,b,d){return b.replace(/\'/g,"__ko_apos__")+d}));a.text=e;a.Na=!0};this.createJavaScriptEvaluatorBlock=function(d){if(this.q==1)return"{{= "+d+"}}";return"{{ko_code ((function() { return "+d+" })()) }}"};this.ta=function(d,b){document.write("<script type='text/html' id='"+d+"'>"+b+"<\/script>")};p.i(this,"addTemplate",this.ta);this.q>1&&(jQuery.tmpl.tag.ko_code=
{open:(this.q<3?"_":"__")+".push($1 || '');"})};p.T.prototype=new p.aa;p.na(new p.T);p.b("ko.jqueryTmplTemplateEngine",p.T);
})(window);                  

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
