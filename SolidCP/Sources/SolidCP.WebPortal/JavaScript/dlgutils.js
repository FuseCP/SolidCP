var estop = function (e) {
	if (!e) e = window.event;
	e.cancelBubble = true;
	if (e.stopPropagation) e.stopPropagation();
	return e;
}

var CPopupDialog = function (el, e) {
	if (typeof el == 'string')
		el = document.getElementById(el);
	e = estop(e);

	var oldclick = document.body.onclick;
	el.onclick = estop;

	function close() {
		el.style.display = "none";
		document.body.onclick = oldclick;
	}

	function show(x, y) {
		el.style.left = x ? x : e.clientX + document.documentElement.scrollLeft + "px";
		el.style.top = y ? y : e.clientY + document.documentElement.scrollTop + "px";
		el.style.display = "block";
		document.body.onclick = close;
	}

	show();
}