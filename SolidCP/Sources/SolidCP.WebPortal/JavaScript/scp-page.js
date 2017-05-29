$(document).ready(function(){

	//*******************************************
	/*	PRINT BUTTON
	/********************************************/

	$('.print-btn').click( function(){
		window.print();
	});


	//*******************************************
	/*	INBOX PAGE
	/********************************************/

	// star icon toggle
	$('.inbox .message-list-table td i').clickToggle( 
		function(){
			$(this).removeClass('ion-ios7-star-outline').addClass('ion-ios7-star');
		},
		function(){
			$(this).removeClass('ion-ios7-star').addClass('ion-ios7-star-outline');
		}
	);

	var anyChecked = false;
	var activated = false;

	// inbox checkbox toggle function
	$('.inbox .message-list-table .fancy-checkbox').change( function(){
		if( $(this).find(':checkbox').is(':checked') ){
			$(this).parents('tr').addClass('highlighted');
		} else {
			$(this).parents('tr').removeClass('highlighted');
		}

		// show/hide top menu
		$('.inbox .message-list-table .fancy-checkbox').each( function() {
			if( $(this).find(':checkbox').is(':checked') ) {
				$('.inbox .top-menu-group1').removeClass('hide');
				$('.inbox .top-menu-label').removeClass('hide');
				anyChecked = true;

				return false;
			}else {
				$('.inbox .top-menu-group1').addClass('hide');
				$('.inbox .top-menu-label').addClass('hide');
				anyChecked = false;
			}
		});

		if( anyChecked && !activated ) {
			$('.inbox .top-menu-more ul li').toggleMenuItem();
			activated = true;
		}else if( !anyChecked ) {
			$('.inbox .top-menu-more ul li').toggleMenuItem();
			activated = false;
		}

	});

	$.fn.toggleMenuItem = function() {
		$(this).each( function() {
			if( $(this).hasClass('hide') ) {
				$(this).removeClass('hide')
			}else {
				$(this).addClass('hide')
			}
		});
	}

	// inbox check all message
	$('.inbox .top-menu .select-checkbox-all').change( function() {
		if( $(this).find(':checkbox').is(':checked') ) {
			$('.inbox .message-list-table .fancy-checkbox').find(':checkbox').prop('checked', true);
			$('.inbox .message-list-table tr').addClass('highlighted');

			$('.inbox .top-menu-group1').removeClass('hide');
			$('.inbox .top-menu-label').removeClass('hide');
			$('.inbox .top-menu-more ul li').toggleMenuItem();
		}else {
			$('.inbox .message-list-table .fancy-checkbox').find(':checkbox').prop('checked', false);
			$('.inbox .message-list-table tr').removeClass('highlighted');

			$('.inbox .top-menu-group1').addClass('hide');
			$('.inbox .top-menu-label').addClass('hide');
			$('.inbox .top-menu-more ul li').toggleMenuItem();
		}

	});

	// inbox responsive left nav
	$('.inbox-nav-toggle').click( function() {
		$('.inbox-left-menu').toggleClass('active');
	});


	//*******************************************
	/*  INBOX
	/********************************************/

	// reply from view single message
	$('.reply-box, .btn-reply').click( function() {

		// divided by two, so we can see half of the sender message and text editor
		$('html, body').animate({
			scrollTop: $("#reply-section").offset().top / 2
		}, 1000);

		$('.reply-box').summernote({
			focus: true,
			height: 300,
		}).code('');
	});

	// create/compose new message
	if( $('.new-message-editor').length > 0 ) {
		$('.new-message-editor').summernote({
			height: 300
		});
	}


	//*******************************************
	/*  SEARCH RESULTS
	/********************************************/

	if( $('body.search-results-page').length > 0 ) {
		$('.multiselect-single-lg').multiselect({
			buttonClass: 'btn btn-default btn-lg',
			templates: {
				li: '<li><a href="javascript:void(0);"><label><i></i></label></a></li>' // mandatory for single selection
			}
		});

		var sliderChanged = function() {
			$('#result-label').text( theSlider.getValue() );
		}

		var theSlider = $('#settings-result-slider').slider({
						min: 5,
						max: 30,
						value: 10,
						step: 5,
						tooltip: 'hide',
						handle: 'square'
					}).on('slide', sliderChanged).data('slider');

		$('#result-label').text( theSlider.getValue() );
	}


	//*******************************************
	/*  PROJECT PAGE: PROGRESS
	/********************************************/

	if( $('.project-progress .pie-chart').length > 0 ) {
		$('.project-progress .pie-chart').easyPieChart({
			size: 100,
			barColor: '#7bae16',
			trackColor: 'rgba(73, 73, 73, 0.2)',
			scaleColor: false,
			lineWidth: 5,
			lineCap: "square",
			animate: 800
		});
	}


	//*******************************************
	/*  PROJECT LIST PAGE: PROGRESS
	/********************************************/

	if( $('.progress .progress-bar').length > 0 ) {
		$('.progress .progress-bar').progressbar({
			display_text: 'fill',
		});
	}

	//*******************************************
	/*  PRICING TABLES
	/********************************************/

	/* pricing table plan help popover */
	if( $('.btn-plan-popover').length > 0 ) {
		$('.btn-plan-popover').popover({
			container: 'body',
			placement: 'right',
			trigger: 'hover',
			html: true,
			content: 'This is custom styled popover, also support HTML content by enabling <code>html: true</code>',
			template: '<div class="popover popover-custom" role="tooltip"><div class="arrow"></div><h5 class="popover-title"></h5><div class="popover-content"></div></div>'
		});
	}

});