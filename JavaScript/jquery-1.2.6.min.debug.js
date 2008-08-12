/*                 JQuery header comments
 *          for Visual Studio IntelliSense support
 * 
 * **************************************************************
 * ************ CONTAINS NO FUNCTIONAL JQUERY CODE **************
 * **************************************************************
 * 
 * Generated with InfoBasis JQuery IntelliSense Header Generator
 * 
 * Sources:
 *     API version: 1.2.6
 *     Documentation version: 1.1.2
 * 
 * JQuery is Copyright (c) John Resig (jquery.com)
 */


jQuery = $ = function (expr, context) {
	/// <summary>
	/// 1: $(expr, context) - This function accepts a string containing a CSS or basic XPath selector which is then used to match a set of elements.
	/// 2: $(html) - Create DOM elements on-the-fly from the provided String of raw HTML.
	/// 3: $(elems) - Wrap jQuery functionality around a single or multiple DOM Element(s).
	/// 4: $(fn) - A shorthand for $(document).
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr">
	/// 1: expr - An expression to search with
	/// 2: html - A string of HTML to create on the fly.
	/// 3: elems - DOM element(s) to be encapsulated by a jQuery object.
	/// 4: fn - The function to execute when the DOM is ready.
	/// </param>
	/// <param name="context" optional="true">
	/// 1: context - (optional) A DOM Element, Document or jQuery to use as context
	/// </param>
	/// <field name="jquery" type="String">The current version of jQuery.</field>
	/// <field name="length" type="Number">The number of elements currently matched.</field>
};

$.prototype = {
	length: {},
	prevObject: {},
	init: function (selector, context) {},
	jquery: {},
	size: function () {
	/// <summary>
	/// Get the number of elements currently matched.
	/// </summary>
	/// <returns type="Number"></returns>
},
	get: function (num) {
	/// <summary>
	/// 1: get() - Access all matched DOM elements.
	///   returns Array<Element>
	/// 2: get(num) - Access a single matched DOM element at a specified index in the matched set.
	///   returns Element
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="num" optional="true">
	/// 2: num - Access the element in the Nth position.
	/// </param>
},
	pushStack: function (elems) {
	/// <summary>
	/// Set the jQuery object to an array of elements, while maintaining the stack.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="elems">An array of elements</param>
},
	setArray: function (elems) {
	/// <summary>
	/// Set the jQuery object to an array of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="elems">An array of elements</param>
},
	each: function (fn) {
	/// <summary>
	/// Execute a function within the context of every matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to execute</param>
},
	index: function (subject) {
	/// <summary>
	/// Searches every matched element for the object and returns the index of the element, if found, starting with zero.
	/// </summary>
	/// <returns type="Number"></returns>
	/// <param name="subject">Object to search for</param>
},
	attr: function (name, value) {
	/// <summary>
	/// 1: attr(name) - Access a property on the first matched element.
	/// 2: attr(properties) - Set a key/value object as properties to all matched elements.
	///   returns jQuery
	/// 3: attr(key, value) - Set a single property to a value, on all matched elements.
	///   returns jQuery
	/// 4: attr(key, value) - Set a single property to a computed value, on all matched elements.
	///   returns jQuery
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="name">
	/// 1: name - The name of the property to access.
	/// 2: properties - Key/value pairs to set as object properties.
	/// 3: key - The name of the property to set.
	/// 4: key - The name of the property to set.
	/// </param>
	/// <param name="value" optional="true">
	/// 3: value - The value to set the property to.
	/// 4: value - A function returning the value to set. Scope: Current element, argument: Index of current element
	/// </param>
},
	css: function (name, value) {
	/// <summary>
	/// 1: css(name) - Access a style property on the first matched element.
	///   returns String
	/// 2: css(properties) - Set a key/value object as style properties to all matched elements.
	///   returns jQuery
	/// 3: css(key, value) - Set a single style property to a value, on all matched elements.
	///   returns jQuery
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="name">
	/// 1: name - The name of the property to access.
	/// 2: properties - Key/value pairs to set as style properties.
	/// 3: key - The name of the property to set.
	/// </param>
	/// <param name="value" optional="true">
	/// 3: value - The value to set the property to.
	/// </param>
},
	text: function (val) {
	/// <summary>
	/// 1: text() - Get the text contents of all matched elements.
	/// 2: text(val) - Set the text contents of all matched elements.
	/// </summary>
	/// <returns type="String"></returns>
	/// <param name="val" optional="true">
	/// 2: val - The text value to set the contents of the element to.
	/// </param>
},
	wrapAll: function (html) {},
	wrapInner: function (html) {},
	wrap: function (html) {
	/// <summary>
	/// 1: wrap(html) - Wrap all matched elements with a structure of other elements.
	/// 2: wrap(elem) - Wrap all matched elements with a structure of other elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="html">
	/// 1: html - A string of HTML, that will be created on the fly and wrapped around the target.
	/// 2: elem - A DOM element that will be wrapped around the target.
	/// </param>
},
	append: function (content) {
	/// <summary>
	/// Append content to the inside of every matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="content">Content to append to the target</param>
},
	prepend: function (content) {
	/// <summary>
	/// Prepend content to the inside of every matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="content">Content to prepend to the target.</param>
},
	before: function (content) {
	/// <summary>
	/// Insert content before each of the matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="content">Content to insert before each target.</param>
},
	after: function (content) {
	/// <summary>
	/// Insert content after each of the matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="content">Content to insert after each target.</param>
},
	end: function () {
	/// <summary>
	/// Revert the most recent 'destructive' operation, changing the set of matched elements to its previous state (right before the destructive operation).
	/// </summary>
	/// <returns type="jQuery"></returns>
},
	find: function (expr) {
	/// <summary>
	/// Searches for all elements that match the specified expression.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr">An expression to search with.</param>
},
	clone: function (deep) {
	/// <summary>
	/// Clone matched DOM Elements and select the clones.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="deep" optional="true">(Optional) Set to false if you don't want to clone all descendant nodes, in addition to the element itself.</param>
},
	filter: function (expression) {
	/// <summary>
	/// 1: filter(expression) - Removes all elements from the set of matched elements that do not match the specified expression(s).
	/// 2: filter(filter) - Removes all elements from the set of matched elements that do not pass the specified filter.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expression">
	/// 1: expression - Expression(s) to search with.
	/// 2: filter - A function to use for filtering
	/// </param>
},
	not: function (el) {
	/// <summary>
	/// 1: not(el) - Removes the specified Element from the set of matched elements.
	/// 2: not(expr) - Removes elements matching the specified expression from the set of matched elements.
	/// 3: not(elems) - Removes any elements inside the array of elements from the set of matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="el">
	/// 1: el - An element to remove from the set
	/// 2: expr - An expression with which to remove matching elements
	/// 3: elems - A set of elements to remove from the jQuery set of matched elements.
	/// </param>
},
	add: function (expr) {
	/// <summary>
	/// 1: add(expr) - Adds more elements, matched by the given expression, to the set of matched elements.
	/// 2: add(html) - Adds more elements, created on the fly, to the set of matched elements.
	/// 3: add(elements) - Adds one or more Elements to the set of matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr">
	/// 1: expr - An expression whose matched elements are added
	/// 2: html - A string of HTML to create on the fly.
	/// 3: elements - One or more Elements to add
	/// </param>
},
	is: function (expr) {
	/// <summary>
	/// Checks the current selection against an expression and returns true, if at least one element of the selection fits the given expression.
	/// </summary>
	/// <returns type="Boolean"></returns>
	/// <param name="expr">The expression with which to filter</param>
},
	hasClass: function (selector) {},
	val: function (val) {
	/// <summary>
	/// 1: val() - Get the content of the value attribute of the first matched element.
	///   returns String
	/// 2: val(val) -  Set the value attribute of every matched element.
	///   returns jQuery
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="val" optional="true">
	/// 2: val - Set the property to the specified value.
	/// </param>
},
	html: function (val) {
	/// <summary>
	/// 1: html() - Get the html contents of the first matched element.
	///   returns String
	/// 2: html(val) - Set the html contents of every matched element.
	///   returns jQuery
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="val" optional="true">
	/// 2: val - Set the html contents to the specified value.
	/// </param>
},
	replaceWith: function (value) {},
	eq: function (pos) {
	/// <summary>
	/// Reduce the set of matched elements to a single element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="pos">The index of the element that you wish to limit to.</param>
},
	slice: function () {},
	map: function (callback) {},
	andSelf: function () {},
	data: function (key, value) {},
	removeData: function (key) {},
	domManip: function (args, table, dir, fn) {
	/// <summary>
	/// 
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="args"></param>
	/// <param name="table">Insert TBODY in TABLEs if one is not found.</param>
	/// <param name="dir">If dir<0, process args in reverse order.</param>
	/// <param name="fn">The function doing the DOM manipulation.</param>
},
	extend: function () {},
	parent: function (expr) {
	/// <summary>
	/// Get a set of elements containing the unique parents of the matched set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr" optional="true">(optional) An expression to filter the parents with</param>
},
	parents: function (expr) {
	/// <summary>
	/// Get a set of elements containing the unique ancestors of the matched set of elements (except for the root element).
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr" optional="true">(optional) An expression to filter the ancestors with</param>
},
	next: function (expr) {
	/// <summary>
	/// Get a set of elements containing the unique next siblings of each of the matched set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr" optional="true">(optional) An expression to filter the next Elements with</param>
},
	prev: function (expr) {
	/// <summary>
	/// Get a set of elements containing the unique previous siblings of each of the matched set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr" optional="true">(optional) An expression to filter the previous Elements with</param>
},
	nextAll: function (selector) {},
	prevAll: function (selector) {},
	siblings: function (expr) {
	/// <summary>
	/// Get a set of elements containing all of the unique siblings of each of the matched set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr" optional="true">(optional) An expression to filter the sibling Elements with</param>
},
	children: function (expr) {
	/// <summary>
	/// Get a set of elements containing all of the unique children of each of the matched set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr" optional="true">(optional) An expression to filter the child Elements with</param>
},
	contents: function (selector) {},
	appendTo: function (content) {
	/// <summary>
	/// Append all of the matched elements to another, specified, set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="content">Content to append to the selected element to.</param>
},
	prependTo: function (content) {
	/// <summary>
	/// Prepend all of the matched elements to another, specified, set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="content">Content to prepend to the selected element to.</param>
},
	insertBefore: function (content) {
	/// <summary>
	/// Insert all of the matched elements before another, specified, set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="content">Content to insert the selected element before.</param>
},
	insertAfter: function (content) {
	/// <summary>
	/// Insert all of the matched elements after another, specified, set of elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="content">Content to insert the selected element after.</param>
},
	replaceAll: function () {},
	removeAttr: function (name) {
	/// <summary>
	/// Remove an attribute from each of the matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="name">The name of the attribute to remove.</param>
},
	addClass: function (cssClass) {
	/// <summary>
	/// Adds the specified class(es) to each of the set of matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="cssClass">One or more CSS classes to add to the elements</param>
},
	removeClass: function (cssClass) {
	/// <summary>
	/// Removes all or the specified class(es) from the set of matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="cssClass" optional="true">(optional) One or more CSS classes to remove from the elements</param>
},
	toggleClass: function (cssClass) {
	/// <summary>
	/// Adds the specified class if it is not present, removes it if it is present.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="cssClass">A CSS class with which to toggle the elements</param>
},
	remove: function (expr) {
	/// <summary>
	/// Removes all matched elements from the DOM.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="expr" optional="true">(optional) A jQuery expression to filter elements by.</param>
},
	empty: function () {
	/// <summary>
	/// Removes all child nodes from the set of matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
},
	height: function (val) {
	/// <summary>
	/// 1: height() - Get the current computed, pixel, height of the first matched element.
	///   returns String
	/// 2: height(val) - Set the CSS height of every matched element.
	///   returns jQuery
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="val" optional="true">
	/// 2: val - Set the CSS property to the specified value.
	/// </param>
},
	width: function (val) {
	/// <summary>
	/// 1: width() - Get the current computed, pixel, width of the first matched element.
	///   returns String
	/// 2: width(val) - Set the CSS width of every matched element.
	///   returns jQuery
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="val" optional="true">
	/// 2: val - Set the CSS property to the specified value.
	/// </param>
},
	bind: function (type, data, fn) {
	/// <summary>
	/// Binds a handler to a particular event (like click) for each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="type">An event type</param>
	/// <param name="data" optional="true">(optional) Additional data passed to the event handler as event.data</param>
	/// <param name="fn">A function to bind to the event on each of the set of matched elements</param>
},
	one: function (type, data, fn) {
	/// <summary>
	/// Binds a handler to a particular event (like click) for each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="type">An event type</param>
	/// <param name="data" optional="true">(optional) Additional data passed to the event handler as event.data</param>
	/// <param name="fn">A function to bind to the event on each of the set of matched elements</param>
},
	unbind: function (type, fn) {
	/// <summary>
	/// The opposite of bind, removes a bound event from each of the matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="type" optional="true">(optional) An event type</param>
	/// <param name="fn" optional="true">(optional) A function to unbind from the event on each of the set of matched elements</param>
},
	trigger: function (type, data) {
	/// <summary>
	/// Trigger a type of event on every matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="type">An event type to trigger.</param>
	/// <param name="data" optional="true">(optional) Additional data to pass as arguments (after the event object) to the event handler</param>
},
	triggerHandler: function (type, data, fn) {},
	toggle: function (even, odd) {
	/// <summary>
	/// 1: toggle(even, odd) - Toggle between two function calls every other click.
	/// 2: toggle() - Toggles each of the set of matched elements.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="even" optional="true">
	/// 1: even - The function to execute on every even click.
	/// </param>
	/// <param name="odd" optional="true">
	/// 1: odd - The function to execute on every odd click.
	/// </param>
},
	hover: function (over, out) {
	/// <summary>
	/// A method for simulating hovering (moving the mouse on, and off, an object).
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="over">The function to fire whenever the mouse is moved over a matched element.</param>
	/// <param name="out">The function to fire whenever the mouse is moved off of a matched element.</param>
},
	ready: function (fn) {
	/// <summary>
	/// Bind a function to be executed whenever the DOM is ready to be traversed and manipulated.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">The function to be executed when the DOM is ready.</param>
},
	blur: function (fn) {
	/// <summary>
	/// 1: blur(fn) - Bind a function to the blur event of each matched element.
	/// 2: blur() - Trigger the blur event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn" optional="true">
	/// 1: fn - A function to bind to the blur event on each of the matched elements.
	/// </param>
},
	focus: function (fn) {
	/// <summary>
	/// 1: focus(fn) - Bind a function to the focus event of each matched element.
	/// 2: focus() - Trigger the focus event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn" optional="true">
	/// 1: fn - A function to bind to the focus event on each of the matched elements.
	/// </param>
},
	load: function (fn, params, callback) {
	/// <summary>
	/// 1: load(fn) - Bind a function to the load event of each matched element.
	/// 2: load(url, params, callback) - Load HTML from a remote file and inject it into the DOM.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">
	/// 1: fn - A function to bind to the load event on each of the matched elements.
	/// 2: url - The URL of the HTML file to load.
	/// </param>
	/// <param name="params" optional="true">
	/// 2: params - (optional) A set of key/value pairs that will be sent as data to the server.
	/// </param>
	/// <param name="callback" optional="true">
	/// 2: callback - (optional) A function to be executed whenever the data is loaded (parameters: responseText, status and response itself).
	/// </param>
},
	resize: function (fn) {
	/// <summary>
	/// Bind a function to the resize event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the resize event on each of the matched elements.</param>
},
	scroll: function (fn) {
	/// <summary>
	/// Bind a function to the scroll event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the scroll event on each of the matched elements.</param>
},
	unload: function (fn) {
	/// <summary>
	/// Bind a function to the unload event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the unload event on each of the matched elements.</param>
},
	click: function (fn) {
	/// <summary>
	/// 1: click(fn) - Bind a function to the click event of each matched element.
	/// 2: click() - Trigger the click event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn" optional="true">
	/// 1: fn - A function to bind to the click event on each of the matched elements.
	/// </param>
},
	dblclick: function (fn) {
	/// <summary>
	/// Bind a function to the dblclick event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the dblclick event on each of the matched elements.</param>
},
	mousedown: function (fn) {
	/// <summary>
	/// Bind a function to the mousedown event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the mousedown event on each of the matched elements.</param>
},
	mouseup: function (fn) {
	/// <summary>
	/// Bind a function to the mouseup event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the mouseup event on each of the matched elements.</param>
},
	mousemove: function (fn) {
	/// <summary>
	/// Bind a function to the mousemove event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the mousemove event on each of the matched elements.</param>
},
	mouseover: function (fn) {
	/// <summary>
	/// Bind a function to the mouseover event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the mousedown event on each of the matched elements.</param>
},
	mouseout: function (fn) {
	/// <summary>
	/// Bind a function to the mouseout event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the mouseout event on each of the matched elements.</param>
},
	change: function (fn) {
	/// <summary>
	/// Bind a function to the change event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the change event on each of the matched elements.</param>
},
	select: function (fn) {
	/// <summary>
	/// 1: select(fn) - Bind a function to the select event of each matched element.
	/// 2: select() - Trigger the select event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn" optional="true">
	/// 1: fn - A function to bind to the select event on each of the matched elements.
	/// </param>
},
	submit: function (fn) {
	/// <summary>
	/// 1: submit(fn) - Bind a function to the submit event of each matched element.
	/// 2: submit() - Trigger the submit event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn" optional="true">
	/// 1: fn - A function to bind to the submit event on each of the matched elements.
	/// </param>
},
	keydown: function (fn) {
	/// <summary>
	/// Bind a function to the keydown event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the keydown event on each of the matched elements.</param>
},
	keypress: function (fn) {
	/// <summary>
	/// Bind a function to the keypress event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the keypress event on each of the matched elements.</param>
},
	keyup: function (fn) {
	/// <summary>
	/// Bind a function to the keyup event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the keyup event on each of the matched elements.</param>
},
	error: function (fn) {
	/// <summary>
	/// Bind a function to the error event of each matched element.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="fn">A function to bind to the error event on each of the matched elements.</param>
},
	_load: function (fn) {},
	serialize: function () {
	/// <summary>
	/// Serializes a set of input elements into a string of data.
	/// </summary>
	/// <returns type="String"></returns>
},
	serializeArray: function () {},
	ajaxStart: function (callback) {
	/// <summary>
	/// Attach a function to be executed whenever an AJAX request begins and there is none already active.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="callback">The function to execute.</param>
},
	ajaxStop: function (callback) {
	/// <summary>
	/// Attach a function to be executed whenever all AJAX requests have ended.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="callback">The function to execute.</param>
},
	ajaxComplete: function (callback) {
	/// <summary>
	/// Attach a function to be executed whenever an AJAX request completes.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="callback">The function to execute.</param>
},
	ajaxError: function (callback) {
	/// <summary>
	/// Attach a function to be executed whenever an AJAX request fails.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="callback">The function to execute.</param>
},
	ajaxSuccess: function (callback) {
	/// <summary>
	/// Attach a function to be executed whenever an AJAX request completes successfully.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="callback">The function to execute.</param>
},
	ajaxSend: function (callback) {
	/// <summary>
	/// Attach a function to be executed before an AJAX request is sent.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="callback">The function to execute.</param>
},
	show: function (speed, callback) {
	/// <summary>
	/// 1: show() - Displays each of the set of matched elements if they are hidden.
	/// 2: show(speed, callback) - Show all matched elements using a graceful animation and firing an optional callback after completion.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="speed" optional="true">
	/// 2: speed - A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).
	/// </param>
	/// <param name="callback" optional="true">
	/// 2: callback - (optional) A function to be executed whenever the animation completes.
	/// </param>
},
	hide: function (speed, callback) {
	/// <summary>
	/// 1: hide() - Hides each of the set of matched elements if they are shown.
	/// 2: hide(speed, callback) - Hide all matched elements using a graceful animation and firing an optional callback after completion.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="speed" optional="true">
	/// 2: speed - A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).
	/// </param>
	/// <param name="callback" optional="true">
	/// 2: callback - (optional) A function to be executed whenever the animation completes.
	/// </param>
},
	_toggle: function (fn) {},
	slideDown: function (speed, callback) {
	/// <summary>
	/// Reveal all matched elements by adjusting their height and firing an optional callback after completion.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="speed" optional="true">(optional) A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the animation completes.</param>
},
	slideUp: function (speed, callback) {
	/// <summary>
	/// Hide all matched elements by adjusting their height and firing an optional callback after completion.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="speed" optional="true">(optional) A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the animation completes.</param>
},
	slideToggle: function (speed, callback) {
	/// <summary>
	/// Toggle the visibility of all matched elements by adjusting their height and firing an optional callback after completion.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="speed" optional="true">(optional) A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the animation completes.</param>
},
	fadeIn: function (speed, callback) {
	/// <summary>
	/// Fade in all matched elements by adjusting their opacity and firing an optional callback after completion.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="speed" optional="true">(optional) A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the animation completes.</param>
},
	fadeOut: function (speed, callback) {
	/// <summary>
	/// Fade out all matched elements by adjusting their opacity and firing an optional callback after completion.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="speed" optional="true">(optional) A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the animation completes.</param>
},
	fadeTo: function (speed, opacity, callback) {
	/// <summary>
	/// Fade the opacity of all matched elements to a specified opacity and firing an optional callback after completion.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="speed">A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).</param>
	/// <param name="opacity">The opacity to fade to (a number from 0 to 1).</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the animation completes.</param>
},
	animate: function (params, speed, easing, callback) {
	/// <summary>
	/// A function for making your own, custom animations.
	/// </summary>
	/// <returns type="jQuery"></returns>
	/// <param name="params">A set of style attributes that you wish to animate, and to what end.</param>
	/// <param name="speed" optional="true">(optional) A string representing one of the three predefined speeds ("slow", "normal", or "fast") or the number of milliseconds to run the animation (e.g. 1000).</param>
	/// <param name="easing" optional="true">(optional) The name of the easing effect that you want to use (Plugin Required).</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the animation completes.</param>
},
	queue: function (type, fn) {},
	stop: function (clearQueue, gotoEnd) {},
	dequeue: function (type) {},
	offset: function () {},
	position: function () {},
	offsetParent: function () {},
	scrollLeft: function (val) {},
	scrollTop: function (val) {},
	innerHeight: function () {},
	outerHeight: function (margin) {},
	innerWidth: function () {},
	outerWidth: function (margin) {}
};

$.extend = function (prop, prop1, propN) {
	/// <summary>
	/// 1: $.extend(prop) - Extends the jQuery object itself.
	/// 2: $.extend(target, prop1, propN) - Extend one object with one or more others, returning the original, modified, object.
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="prop">
	/// 1: prop - The object that will be merged into the jQuery object
	/// 2: target - The object to extend
	/// </param>
	/// <param name="prop1" optional="true">
	/// 2: prop1 - The object that will be merged into the first.
	/// </param>
	/// <param name="propN" optional="true">
	/// 2: propN - (optional) More objects to merge into the first
	/// </param>
};

$.noConflict = function () {
	/// <summary>
	/// Run this function to give control of the $ variable back to whichever library first implemented it.
	/// </summary>
};

$.isFunction = function (fn) {};

$.isXMLDoc = function (elem) {};

$.globalEval = function (data) {};

$.nodeName = function (elem, name) {};

$.cache = {};

$.data = function (elem, name, data) {};

$.removeData = function (elem, name) {};

$.each = function (obj, fn) {
	/// <summary>
	/// A generic iterator function, which can be used to seamlessly iterate over both objects and arrays.
	/// </summary>
	/// <returns type="Object"></returns>
	/// <param name="obj">The object, or array, to iterate over.</param>
	/// <param name="fn">The function that will be executed on every object.</param>
};

$.prop = function (elem, value, type, i, name) {};

$.className = {};

$.swap = function (elem, options, callback) {};

$.css = function (elem, name, force) {};

$.curCSS = function (elem, name, force) {};

$.clean = function (elems, context) {};

$.attr = function (elem, name, value) {};

$.trim = function (str) {
	/// <summary>
	/// Remove the whitespace from the beginning and end of a string.
	/// </summary>
	/// <returns type="String"></returns>
	/// <param name="str">The string to trim.</param>
};

$.makeArray = function (array) {};

$.inArray = function (elem, array) {};

$.merge = function (first, second) {
	/// <summary>
	/// Merge two arrays together, removing all duplicates.
	/// </summary>
	/// <returns type="Array"></returns>
	/// <param name="first">The first array to merge, the unique elements of second added.</param>
	/// <param name="second">The second array to merge into the first, unaltered.</param>
};

$.unique = function (array) {};

$.grep = function (array, fn, inv) {
	/// <summary>
	/// Filter items out of an array, by using a filter function.
	/// </summary>
	/// <returns type="Array"></returns>
	/// <param name="array">The Array to find items in.</param>
	/// <param name="fn">The function to process each item against.</param>
	/// <param name="inv">Invert the selection - select the opposite of the function.</param>
};

$.map = function (array, fn) {
	/// <summary>
	/// Translate all items in an array to another array of items.
	/// </summary>
	/// <returns type="Array"></returns>
	/// <param name="array">The Array to translate.</param>
	/// <param name="fn">The function to process each item against.</param>
};

$.browser = {};

$.boxModel = {};

$.props = {};

$.expr = {};

$.parse = {};

$.multiFilter = function (expr, elems, not) {};

$.find = function () {
	/// <summary>
	/// 
	/// </summary>
	/// <returns type="Array<Element>"></returns>
};

$.classFilter = function (r, m, not) {};

$.filter = function (t, r, not) {};

$.dir = function (elem, dir) {};

$.nth = function (cur, num, dir) {
	/// <summary>
	/// A handy, and fast, way to traverse in a particular direction and find a specific element.
	/// </summary>
	/// <returns type="DOMElement"></returns>
	/// <param name="cur">The element to search from.</param>
	/// <param name="num">The Nth result to match. Can be a number or a string (like 'even' or 'odd').</param>
	/// <param name="dir">The direction to move in (pass in something like 'previousSibling' or 'nextSibling').</param>
};

$.sibling = function (elem) {
	/// <summary>
	/// All elements on a specified axis.
	/// </summary>
	/// <returns type="Array"></returns>
	/// <param name="elem">The element to find all the siblings of (including itself).</param>
};

$.event = {};

$.isReady = {};

$.readyList = {};

$.ready = function () {};

$.get = function (url, params, callback) {
	/// <summary>
	/// Load a remote page using an HTTP GET request.
	/// </summary>
	/// <returns type="XMLHttpRequest"></returns>
	/// <param name="url">The URL of the page to load.</param>
	/// <param name="params" optional="true">(optional) Key/value pairs that will be sent to the server.</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the data is loaded successfully.</param>
};

$.getScript = function (url, callback) {
	/// <summary>
	/// Loads, and executes, a remote JavaScript file using an HTTP GET request.
	/// </summary>
	/// <returns type="XMLHttpRequest"></returns>
	/// <param name="url">The URL of the page to load.</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the data is loaded successfully.</param>
};

$.getJSON = function (url, params, callback) {
	/// <summary>
	/// Load JSON data using an HTTP GET request.
	/// </summary>
	/// <returns type="XMLHttpRequest"></returns>
	/// <param name="url">The URL of the page to load.</param>
	/// <param name="params" optional="true">(optional) Key/value pairs that will be sent to the server.</param>
	/// <param name="callback">A function to be executed whenever the data is loaded successfully.</param>
};

$.post = function (url, params, callback) {
	/// <summary>
	/// Load a remote page using an HTTP POST request.
	/// </summary>
	/// <returns type="XMLHttpRequest"></returns>
	/// <param name="url">The URL of the page to load.</param>
	/// <param name="params" optional="true">(optional) Key/value pairs that will be sent to the server.</param>
	/// <param name="callback" optional="true">(optional) A function to be executed whenever the data is loaded successfully.</param>
};

$.ajaxSetup = function (settings) {
	/// <summary>
	/// Setup global settings for AJAX requests.
	/// </summary>
	/// <param name="settings">Key/value pairs to use for all AJAX requests</param>
};

$.ajaxSettings = {};

$.lastModified = {};

$.ajax = function (properties) {
	/// <summary>
	/// Load a remote page using an HTTP request.
	/// </summary>
	/// <returns type="XMLHttpRequest"></returns>
	/// <param name="properties">Key/value pairs to initialize the request with.</param>
};

$.handleError = function (s, xhr, status, e) {};

$.active = {};

$.httpSuccess = function (xhr) {};

$.httpNotModified = function (xhr, url) {};

$.httpData = function (xhr, type, filter) {};

$.param = function (a) {};

$.speed = function (speed, easing, fn) {};

$.easing = {};

$.timers = {};

$.timerId = {};

$.fx = function (elem, options, prop) {};

$.easing.linear = function (p, n, firstNum, diff) {};

$.easing.swing = function (p, n, firstNum, diff) {};

$.ajaxSettings.url = {};

$.ajaxSettings.global = {};

$.ajaxSettings.type = {};

$.ajaxSettings.timeout = {};

$.ajaxSettings.contentType = {};

$.ajaxSettings.processData = {};

$.ajaxSettings.async = {};

$.ajaxSettings.data = {};

$.ajaxSettings.username = {};

$.ajaxSettings.password = {};

$.ajaxSettings.accepts = {};

$.ajaxSettings.accepts.xml = {};

$.ajaxSettings.accepts.html = {};

$.ajaxSettings.accepts.script = {};

$.ajaxSettings.accepts.json = {};

$.ajaxSettings.accepts.text = {};

$.ajaxSettings.accepts._default = {};

$.props.float = {};

$.props.cssFloat = {};

$.props.styleFloat = {};

$.props.readonly = {};

$.props.maxlength = {};

$.props.cellspacing = {};

$.browser.version = {};

$.browser.safari = {};

$.browser.opera = {};

$.browser.msie = {};

$.browser.mozilla = {};

$.className.add = function (elem, classNames) {};

$.className.remove = function (elem, classNames) {};

$.className.has = function (elem, className) {};

