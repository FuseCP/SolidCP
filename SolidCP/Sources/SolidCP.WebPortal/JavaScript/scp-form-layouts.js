$(document).ready(function(){

	/**********************************
	/*	MULTISELECT ON FORM
	/**********************************/

	if( $('.select-ticket-type').length > 0 || $('.select-ticket-priority').length > 0) {
		$('.select-ticket-type, .select-ticket-priority').multiselect({
			templates: {
				li: '<li><a href="javascript:void(0);"><label><i></i></label></a></li>'
			}
		});
	}

	if( $('.btn-login-help').length > 0 ) {
		$('.btn-login-help').popover({
			container: 'body',
			placement: 'right',
			html: true,
			title: 'Problem Signing?',
			content: 'Please find helpful links below that match best with your signing problems.' +
			'<ul>' +
			'<li><a href="#">I forgot my login email</a></li>' +
			'<li><a href="#">How to secure your account?</a></li>' +
			'<li><a href="#">Go to help center</a></li>' +
			'</ul>'
		});
	}


	/**********************************
	/*	FORM WITH FANCY ELEMENTS
	/**********************************/

	if( $('.form-layouts').length > 0 ) {
		$('#input-slider').slider({
			min: 0,
			max: 500,
			value: 120,
			handle: 'square'
		});
	}
	
	
});