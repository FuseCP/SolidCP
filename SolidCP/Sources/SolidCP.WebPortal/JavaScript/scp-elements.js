$(document).ready(function(){

	if( $('body').hasClass('fancy-form-elements') ) {
		//*******************************************
		/*	MASKED INPUT
		/********************************************/

		$('#phone').mask('(999) 999-9999');
		$('#phone-ex').mask('(999) 999-9999? x99999');
		$('#tax-id').mask('99-9999999');
		$('#ssn').mask('999-99-9999');
		$('#product-key').mask('a*-999-a999');


		//*******************************************
		/*	RANGE SLIDER INPUT
		/********************************************/

		$('.basic-slider').rangeSlider({
			arrows: false
		});

		$('.basic-slider').on('valuesChanging', 
			function(e, data) {
				$('#slider-output')
				.text('Value min: ' + data.values.min + ', ' + 'max: ' + data.values.max)
				.slideDown(300);
			}
		);

		$('.date-slider').dateRangeSlider({
			arrows: false
		});
		
		$('.editable-slider').editRangeSlider({
			arrows: false,
			bounds: {min: 5, max: 50},
			defaultValues: {min: 12, max: 40}
		});

		$('.basic-step-slider').rangeSlider({
			arrows: false,
			step: 10
		});

		$('.basic-label-slider').rangeSlider({
			valueLabels: "change"
		});


		//*******************************************
		/*	MULTISELECT
		/********************************************/

		$('.multiselect-single-lg').multiselect({
			templates: {
				li: '<li><a href="javascript:void(0);"><label><i></i></label></a></li>' // mandatory for single selection
			}
		});

		$('.multiselect-single').multiselect({
			templates: {
				li: '<li><a href="javascript:void(0);"><label><i></i></label></a></li>' // mandatory for single selection
			}
		});

		$('.multiselect-single-sm').multiselect({
			buttonClass: 'btn btn-default btn-sm',
			templates: {
				li: '<li><a href="javascript:void(0);"><label><i></i></label></a></li>' // mandatory for single selection
			}
		});

		$('#multiselect1, #multiselect5, #multiselect6').multiselect({
			maxHeight: 200
		});

		$('#multiselect3-all').multiselect({
			includeSelectAllOption: true,
			buttonClass: 'btn btn-warning',
		});
		
		$('#multiselect4-filter').multiselect({
			enableFiltering: true,
			enableCaseInsensitiveFiltering: true,
			buttonClass: 'btn btn-success',
			maxHeight: 200
		});

		$('#multiselect-size').multiselect({
			buttonClass: 'btn btn-primary btn-sm'
		});

		$('#multiselect-link').multiselect({
			buttonClass: 'btn btn-link'
		});


		//*******************************************
		/*	DATE PICKER
		/********************************************/

		var dtp = $('#datepicker').datepicker()
		.on('changeDate', function(e) {
			dtp.datepicker('hide');
		});

		$('#daterange-default').daterangepicker({
			timePicker: true,
			timePickerIncrement: 10,
			format: 'MM/DD/YYYY h:mm A'
		});
		
		$('#reportrange').daterangepicker({
			startDate: moment().subtract('days', 29),
			endDate: moment(),
			minDate: '01/01/2012',
			maxDate: '12/31/2032',
			dateLimit: { days: 60 },
			showDropdowns: true,
			showWeekNumbers: true,
			timePicker: false,
			timePickerIncrement: 1,
			timePicker12Hour: true,
			ranges: {
				'Today': [moment(), moment()],
				'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
				'Last 7 Days': [moment().subtract('days', 6), moment()],
				'Last 30 Days': [moment().subtract('days', 29), moment()],
				'This Month': [moment().startOf('month'), moment().endOf('month')],
				'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
				},
			opens: 'left',
			applyClass: 'btn btn-small btn-primary',
			cancelClass: 'btn-small',
			format: 'MM/DD/YYYY',
			separator: ' to ',
			locale: {
					applyLabel: 'Submit',
					fromLabel: 'From',
					toLabel: 'To',
					customRangeLabel: 'Custom Range',
					daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr','Sa'],
					monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
					firstDay: 1
				}
			},

			function(start, end) {
				console.log("Callback has been called!");
				$('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
			
			}
		);

		// set the initial state of the picker label
		$('#reportrange span').html(moment().subtract('days', 29).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));


		//*******************************************
		/*	SLIDER INPUT
		/********************************************/

		var sliderChanged = function() {
			$('.label-slider').text( theSlider.getValue() );
		}

		var theSlider = $('.bootstrap-slider')
			.slider({
				min: 0,
				max: 500,
				value: 120,
				tooltip: 'hide',
				handle: 'square'
			}).on('slide', sliderChanged).data('slider');

		$('.label-slider').text( theSlider.getValue() );

		var theStepSlider = $('.bootstrap-slider-step')
			.slider({
				min: 0,
				max: 500,
				value: 150,
				step: 50,
				handle: 'square'
			});

		$('.bootstrap-slider-vertical').each( function() {
			$(this).slider({
				min: 0,
				max: 100,
				value: Math.floor((Math.random() * 100) + 1),
				orientation: 'vertical',
				selection: 'after',
				tooltip: 'hide',
				handle: 'square'
			});
		});


		//*******************************************
		/*	COLOR PICKER
		/********************************************/

		$('#colorpicker1').colorpicker();
		$('#colorpicker2').colorpicker({
			format: 'rgba'
		});

		$('#colorpicker3').colorpicker();


		//*******************************************
		/*	APPENDABLE INPUT, DYNAMIC FORM FIELD
		/********************************************/

		$( "form.input-append" ).keypress(function(e) {
			if ( e.which == 13 ) {
				e.preventDefault();
			}
		});

		$('.input-group-appendable .add-more').click(function(){
			$wrapper = $(this).parents('.input-appendable-wrapper');
			$lastItem = $wrapper.find('.input-group-appendable').last(); 

			$newInput = $lastItem.clone(true);

			// change attribute for new item
			$count = $wrapper.find('#count').val();
			$count++;

			// change text input and the button
			$newInput.attr('id', 'input-group-appendable' + $count);
			$newInput.find('input[type="text"]').attr({
				id: "field" + $count,
				name: "field" + $count
			});

			$newInput.find('.btn').attr('id', 'btn' + $count);
			$newInput.appendTo($wrapper);

			//change the previous button to remove
			$lastItem.find('.btn')
			.removeClass('add-more btn-primary')
			.addClass('btn-danger')
			.text('-')
			.off()
			.on('click', function(){
				$(this).parents('.input-group-appendable').remove();
			});

			$wrapper.find('#count').val($count);

		});


		//*******************************************
		/*	SPINNER INPUT
		/********************************************/

		$("#touchspin1").TouchSpin();
		$("#touchspin2").TouchSpin({
			min: 0,
			max: 100,
			step: 0.1,
			decimals: 2,
			boostat: 5,
			maxboostedstep: 10,
			postfix: '%'
		});

		$("#touchspin3").TouchSpin({
			min: -1000000000,
			max: 1000000000,
			stepinterval: 50,
			maxboostedstep: 10000000,
			prefix: '$',
			buttondown_class: 'btn btn-primary',
			buttonup_class: 'btn btn-warning'
		});

		$("#touchspin4").TouchSpin({
			postfix: "Submit",
			postfix_extraclass: "btn btn-default"
		});


		//*******************************************
		/*	TEXTAREA
		/********************************************/

		$('.textarea-msg span').html($('.textarea-with-counter').attr('maxlength') + ' characters remaining');

		$('.textarea-with-counter').keyup(function() {
			var textMax = $(this).attr('maxlength');
			var textLength = $(this).val().length;
			var textRemaining = textMax - textLength;
			$(this).next().find('span').html(textRemaining + ' characters remaining');
		});

	}

	if( $('body').hasClass('buttons') ) {

		//*******************************************
		/*	BUTTON WITH LOADING STATE
		/********************************************/

		if( $('#loading-example-btn').length > 0 ) {
			$('#loading-example-btn').click( function() {
				var btn = $(this);

				$.ajax({
					url: 'php/widget-ajax.php',
					type: 'POST',
					dataType: 'json',
					cache: false,
					beforeSend: function(){
						btn.button('loading');
					},
					success: function(  data, textStatus, XMLHttpRequest  ) {
						// dummy delay for demo purpose only
						setTimeout( function() {
							$('#server-message').text(data['msg']).addClass('green-font');
							btn.button('reset');
						}, 1000 );
					},
					error: function( XMLHttpRequest, textStatus, errorThrown ) {
						console.log("AJAX ERROR: \n" + errorThrown);
					}
				});

				
			});
		}
	}

	//*******************************************
	/*	GENERAL UI ELEMENTS
	/********************************************/

	// if general ui elements page
	if( $('body').hasClass('general-ui-elements') ) {

		//*******************************************
		/*	BOOTSTRAP PROGRESS BAR BY @MINDDUST
		/********************************************/

		if( $('.progress .progress-bar').length > 0 ) {
			$('.progress .progress-bar').progressbar({
				display_text: 'fill'
			});
			
			$('.progress.no-percentage .progress-bar').progressbar({
				display_text: 'fill',
				use_percentage: false
			});

			$('.progress.custom-format .progress-bar').progressbar({
				display_text: 'fill',
				use_percentage: false,
				amount_format: function(p, t) {return p + ' of ' + t;}
			});

			$('.progress.vertical .progress-bar').progressbar();
			$('.progress.vertical .progress-bar').progressbar({
				display_text: 'fill'
			});
		}


		//*******************************************
		/*	NOTIFICATION
		/********************************************/

		// global setting override
		$.extend( $.gritter.options, {
			/* NOTE: 
			/* you can use these params to set global variable that affect all the notications behaviour
			/* class_name: 'gritter-light',
			/* position: 'bottom-right' // possibilities: bottom-left, bottom-right, top-left, top-right */

			fade_in_speed: 500,
			fade_out_speed: 500,
			time: 2000,
			tpl_close: '<a class="gritter-close" href="#" tabindex="1"></a>' // option added in customized version
		});

		$('#gritter-regular').click( function() {
			$.gritter.add({
				title: '<i class="icon ion-information-circled"></i> Info Notification',
				text: 'Fades out automatically in amount of time. Link <a href="#">goes here</a>',
				class_name: 'gritter-info',
				after_close: function() {

					$.gritter.add({
						title: '<i class="icon ion-alert-circled"></i> Warning Notification',
						text: 'Fades out automatically in amount of time. Link <a href="#">goes here</a>',
						class_name: 'gritter-warning',

						after_close: function() {
							$.gritter.add({
								title: '<i class="icon ion-close-circled"></i> Danger Notification',
								text: 'Fades out automatically in amount of time. Link <a href="#">goes here</a>',
								class_name: 'gritter-danger'
							});
						}
					});
				}
			});
		});

		$('#gritter-sticky').click( function() {
			$.gritter.add({
				title: '<i class="icon ion-information-circled"></i> Sticky Alert!',
				text: 'You have <a href="#">new support message</a>. Click (x) on the top right to close this message.',
				sticky: true,
			});
		});

		$('#gritter-image').click( function() {
			$.gritter.add({
				title: 'Jane Doe',
				text: 'Online',
				image: '../Images/user3.png',
				time: 2500,
				after_close: function() {
					$.gritter.add({
						title: 'Jordan Smith',
						text: 'Offline',
						image: '../Images/user5.png',
						time: 2500,
					});
				}
			});
		});

		$('.btn-gritter-position').click( function() {

			// clean the wrapper position class
			$('#gritter-notice-wrapper').attr('class', '');

			// global setting override
			$.extend( $.gritter.options, {
				position: '' + $(this).attr('id') + '' // possibilities: bottom-left, bottom-right, top-left, top-right
			});

			$.gritter.add({
				title: $(this).find('span.title').text(), // could be simpler, just for demo purposes
				text: 'Hi, I\'m on the  ' + $.gritter.options.position + ''
			});
		});

		$('#gritter-callback').click( function() {
			$.gritter.add({
				title: 'Callback',
				text: 'Provide several callback features',
				time: 1000,
				before_open: function() {
					alert('before_open callback');
				},
				after_open: function() {
					alert('after_open callback');
				},
				before_close: function() {
					alert('before_close callback');
				},
				after_close: function() {
					alert('after_close callback');
				},
			});
		});

		$('#gritter-max').click( function() {
			$.gritter.add({
				title: 'Limit Notifications',
				text: 'This is a notice with a max of 3 on screen at one time!',
				before_open: function() {
					if( $('.gritter-item-wrapper').length == 3 ) {
						// Returning false prevents a new gritter from opening
						return false;
					}
				}
			});

			return false;
		});
	} // end if general ui elements


	//*******************************************
	/*	MULTISELECT VALIDATION
	/********************************************/

	if( $('body').hasClass('form-validation') ) {

		$('.multiselect-validation').multiselect({
			checkboxName: 'food'
		});
	}


	//*******************************************
	/*	SUMMERNOTE AND MARKDOWN TEXT EDITOR
	/********************************************/

	// if text editor page
	if( $('body').hasClass('text-editor') ) {
		/* summernote */
		$('.summernote').summernote({
			height: 300,
			focus: true,
			onpaste: function() {
				alert('You have pasted something to the editor');
			}
		});

		/* markdown */
		var initContent = '<h4>Hello there</h4> ' +
			'<p>How are you? I have below task for you :</p> ' + 
				'<p>Select from this text... Click the bold on THIS WORD and make THESE ONE italic, ' +
				'link GOOGLE to google.com, ' +
				'test to insert image (and try to tab after write the image description)</p>' + 
				'<p>Test Preview And ending here...</p> ' + 
				'<p>Click "List"</p> Enjoy!';

		$('#markdown-editor').text(toMarkdown(initContent));
		
	}


	//*******************************************
	/*	X-EDITABLE IN-PLACE EDITING
	/********************************************/

	if( $('body').hasClass('inplace-editing') ) {
		
		//defaults
		$.fn.editable.defaults.url = '/post';

		// editable fields
		$('#username').editable();
		$('#lastname').editable({
			validate: function(value) {
				if($.trim(value) == '') return 'This field is required';
			}
		});

		$('#sex').editable({
			prepend: "not selected",
			source: [
				{value: 1, text: 'Male'},
				{value: 2, text: 'Female'}
			],
			display: function(value, sourceData) {
				 var colors = {"": "gray", 1: "green", 2: "red"},
					 elem = $.grep(sourceData, function(o){return o.value == value;});
					 
				 if(elem.length) {
					 $(this).text(elem[0].text).css("color", colors[value]);
				 } else {
					 $(this).empty();
				 }
			}
		});

		$('#group').editable({
			showbuttons: false
		});

		$('#status').editable();
		
		$('#dob').editable({
			format: 'yyyy-mm-dd',
				viewformat: 'dd/mm/yyyy',
				datepicker: {
					weekStart: 1
				}
		});

		$('#combodate').editable();

		$('#event').editable({
			placement: 'right',
			combodate: {
				firstItem: 'name'
			}
		});

		$('#comments').editable({
			showbuttons: 'bottom'
		});

		$('#state2').editable({
			value: 'California',
			typeahead: {
				name: 'state',
				local: ["Alabama","Alaska","Arizona","Arkansas","California","Colorado","Connecticut","Delaware","Florida","Georgia","Hawaii","Idaho","Illinois","Indiana","Iowa","Kansas","Kentucky","Louisiana","Maine","Maryland","Massachusetts","Michigan","Minnesota","Mississippi","Missouri","Montana","Nebraska","Nevada","New Hampshire","New Jersey","New Mexico","New York","North Dakota","North Carolina","Ohio","Oklahoma","Oregon","Pennsylvania","Rhode Island","South Carolina","South Dakota","Tennessee","Texas","Utah","Vermont","Virginia","Washington","West Virginia","Wisconsin","Wyoming"]
			}
		});

		$('#fruits').editable({
			pk: 1,
			limit: 3,
			source: [
				{value: 1, text: 'banana'},
				{value: 2, text: 'peach'},
				{value: 3, text: 'apple'},
				{value: 4, text: 'watermelon'},
				{value: 5, text: 'orange'}
			]
		});

		$('#tags').editable({
			inputclass: 'input-large',
			select2: {
			tags: ['html', 'javascript', 'css', 'ajax'],
				tokenSeparators: [",", " "]
			}
		});

		var countries = [];
		$.each({"BD": "Bangladesh", "BE": "Belgium", "BF": "Burkina Faso", "BG": "Bulgaria", "BA": "Bosnia and Herzegovina", "BB": "Barbados", "WF": "Wallis and Futuna", "BL": "Saint Bartelemey", "BM": "Bermuda", "BN": "Brunei Darussalam", "BO": "Bolivia", "BH": "Bahrain", "BI": "Burundi", "BJ": "Benin", "BT": "Bhutan", "JM": "Jamaica", "BV": "Bouvet Island", "BW": "Botswana", "WS": "Samoa", "BR": "Brazil", "BS": "Bahamas", "JE": "Jersey", "BY": "Belarus", "O1": "Other Country", "LV": "Latvia", "RW": "Rwanda", "RS": "Serbia", "TL": "Timor-Leste", "RE": "Reunion", "LU": "Luxembourg", "TJ": "Tajikistan", "RO": "Romania", "PG": "Papua New Guinea", "GW": "Guinea-Bissau", "GU": "Guam", "GT": "Guatemala", "GS": "South Georgia and the South Sandwich Islands", "GR": "Greece", "GQ": "Equatorial Guinea", "GP": "Guadeloupe", "JP": "Japan", "GY": "Guyana", "GG": "Guernsey", "GF": "French Guiana", "GE": "Georgia", "GD": "Grenada", "GB": "United Kingdom", "GA": "Gabon", "SV": "El Salvador", "GN": "Guinea", "GM": "Gambia", "GL": "Greenland", "GI": "Gibraltar", "GH": "Ghana", "OM": "Oman", "TN": "Tunisia", "JO": "Jordan", "HR": "Croatia", "HT": "Haiti", "HU": "Hungary", "HK": "Hong Kong", "HN": "Honduras", "HM": "Heard Island and McDonald Islands", "VE": "Venezuela", "PR": "Puerto Rico", "PS": "Palestinian Territory", "PW": "Palau", "PT": "Portugal", "SJ": "Svalbard and Jan Mayen", "PY": "Paraguay", "IQ": "Iraq", "PA": "Panama", "PF": "French Polynesia", "BZ": "Belize", "PE": "Peru", "PK": "Pakistan", "PH": "Philippines", "PN": "Pitcairn", "TM": "Turkmenistan", "PL": "Poland", "PM": "Saint Pierre and Miquelon", "ZM": "Zambia", "EH": "Western Sahara", "RU": "Russian Federation", "EE": "Estonia", "EG": "Egypt", "TK": "Tokelau", "ZA": "South Africa", "EC": "Ecuador", "IT": "Italy", "VN": "Vietnam", "SB": "Solomon Islands", "EU": "Europe", "ET": "Ethiopia", "SO": "Somalia", "ZW": "Zimbabwe", "SA": "Saudi Arabia", "ES": "Spain", "ER": "Eritrea", "ME": "Montenegro", "MD": "Moldova, Republic of", "MG": "Madagascar", "MF": "Saint Martin", "MA": "Morocco", "MC": "Monaco", "UZ": "Uzbekistan", "MM": "Myanmar", "ML": "Mali", "MO": "Macao", "MN": "Mongolia", "MH": "Marshall Islands", "MK": "Macedonia", "MU": "Mauritius", "MT": "Malta", "MW": "Malawi", "MV": "Maldives", "MQ": "Martinique", "MP": "Northern Mariana Islands", "MS": "Montserrat", "MR": "Mauritania", "IM": "Isle of Man", "UG": "Uganda", "TZ": "Tanzania, United Republic of", "MY": "Malaysia", "MX": "Mexico", "IL": "Israel", "FR": "France", "IO": "British Indian Ocean Territory", "FX": "France, Metropolitan", "SH": "Saint Helena", "FI": "Finland", "FJ": "Fiji", "FK": "Falkland Islands (Malvinas)", "FM": "Micronesia, Federated States of", "FO": "Faroe Islands", "NI": "Nicaragua", "NL": "Netherlands", "NO": "Norway", "NA": "Namibia", "VU": "Vanuatu", "NC": "New Caledonia", "NE": "Niger", "NF": "Norfolk Island", "NG": "Nigeria", "NZ": "New Zealand", "NP": "Nepal", "NR": "Nauru", "NU": "Niue", "CK": "Cook Islands", "CI": "Cote d'Ivoire", "CH": "Switzerland", "CO": "Colombia", "CN": "China", "CM": "Cameroon", "CL": "Chile", "CC": "Cocos (Keeling) Islands", "CA": "Canada", "CG": "Congo", "CF": "Central African Republic", "CD": "Congo, The Democratic Republic of the", "CZ": "Czech Republic", "CY": "Cyprus", "CX": "Christmas Island", "CR": "Costa Rica", "CV": "Cape Verde", "CU": "Cuba", "SZ": "Swaziland", "SY": "Syrian Arab Republic", "KG": "Kyrgyzstan", "KE": "Kenya", "SR": "Suriname", "KI": "Kiribati", "KH": "Cambodia", "KN": "Saint Kitts and Nevis", "KM": "Comoros", "ST": "Sao Tome and Principe", "SK": "Slovakia", "KR": "Korea, Republic of", "SI": "Slovenia", "KP": "Korea, Democratic People's Republic of", "KW": "Kuwait", "SN": "Senegal", "SM": "San Marino", "SL": "Sierra Leone", "SC": "Seychelles", "KZ": "Kazakhstan", "KY": "Cayman Islands", "SG": "Singapore", "SE": "Sweden", "SD": "Sudan", "DO": "Dominican Republic", "DM": "Dominica", "DJ": "Djibouti", "DK": "Denmark", "VG": "Virgin Islands, British", "DE": "Germany", "YE": "Yemen", "DZ": "Algeria", "US": "United States", "UY": "Uruguay", "YT": "Mayotte", "UM": "United States Minor Outlying Islands", "LB": "Lebanon", "LC": "Saint Lucia", "LA": "Lao People's Democratic Republic", "TV": "Tuvalu", "TW": "Taiwan", "TT": "Trinidad and Tobago", "TR": "Turkey", "LK": "Sri Lanka", "LI": "Liechtenstein", "A1": "Anonymous Proxy", "TO": "Tonga", "LT": "Lithuania", "A2": "Satellite Provider", "LR": "Liberia", "LS": "Lesotho", "TH": "Thailand", "TF": "French Southern Territories", "TG": "Togo", "TD": "Chad", "TC": "Turks and Caicos Islands", "LY": "Libyan Arab Jamahiriya", "VA": "Holy See (Vatican City State)", "VC": "Saint Vincent and the Grenadines", "AE": "United Arab Emirates", "AD": "Andorra", "AG": "Antigua and Barbuda", "AF": "Afghanistan", "AI": "Anguilla", "VI": "Virgin Islands, U.S.", "IS": "Iceland", "IR": "Iran, Islamic Republic of", "AM": "Armenia", "AL": "Albania", "AO": "Angola", "AN": "Netherlands Antilles", "AQ": "Antarctica", "AP": "Asia/Pacific Region", "AS": "American Samoa", "AR": "Argentina", "AU": "Australia", "AT": "Austria", "AW": "Aruba", "IN": "India", "AX": "Aland Islands", "AZ": "Azerbaijan", "IE": "Ireland", "ID": "Indonesia", "UA": "Ukraine", "QA": "Qatar", "MZ": "Mozambique"}, function(k, v) {
			countries.push({id: k, text: v});
		}); 
		$('#country').editable({
				source: countries,
				select2: {
				width: 200,
				placeholder: 'Select country',
				allowClear: true
			} 
		});

		/* please refer to address.custom.js for the address template */
		$('#address').editable({
			url: '/post',
			value: {
				city: "Moscow", 
				street: "Lenina", 
				building: "12"
			},
			validate: function(value) {
				if(value.city == '') return 'city is required!'; 
			},
			display: function(value) {
				if(!value) {
					$(this).empty();
					return; 
				}
				var html = '<b>' + $('<div>').text(value.city).html() + '</b>, ' + $('<div>').text(value.street).html() + ' st., bld. ' + $('<div>').text(value.building).html();
				$(this).html(html); 
			}
		});

		/* demo controls */
		//enable / disable in-place editing
		$('#enable').click(function() {
			$('#user .editable').editable('toggleDisabled');
		});

		// x-editable inline/popup input mode
		if( localStorage.getItem('xmode') == 'inline' ) {
			$('#user .editable').editable('option', 'mode', 'inline');
			$('#switch-inline-input').prop('checked', true);
		}else {
			$('#user .editable').editable('option', 'mode', 'popup');
			$('#switch-inline-input').prop('checked', false);
		}

		$('#switch-inline-input').on('change', function() {
			if( $(this).prop('checked') == true) {
				localStorage.setItem('xmode', 'inline');
				window.location.href="?mode=inline";
			}else {
				localStorage.setItem('xmode', 'popup');
				window.location.href="?mode=popup";
			}
		});
	}

	/* button with loading state */
	if( $('#loading-example-btn').length > 0 ) {
		$('#loading-example-btn').click( function() {
			var btn = $(this);

			$.ajax({
				url: 'php/widget-ajax.php',
				type: 'POST',
				dataType: 'json',
				cache: false,
				beforeSend: function(){
					btn.button('loading');
				},
				success: function(  data, textStatus, XMLHttpRequest  ) {
					// dummy delay for demo purpose only
					setTimeout( function() {
						$('#server-message').text(data['msg']).addClass('green-font');
						btn.button('reset');
					}, 1000 );


				},
				error: function( XMLHttpRequest, textStatus, errorThrown ) {
					console.log("AJAX ERROR: \n" + errorThrown);
				}
			});
		});
	}


	//*******************************************
	/*	BOOTSTRAP TOUR
	/********************************************/

	if( $('.tour-demo').length > 0 ) {
		var queenTour = new Tour({
			steps: [
				{
					element: "#btn-nav-sidebar-minified",
					title: "Minified Navigation",
					content: "Minified button toggle, provides <strong>more space</strong> for main content. Try it now",
					placement: "right"
				},
				{
					element: "#tour-collapse-toggle",
					title: "Widget Controls",
					content: "Expand/collapse toggle, give your attention for other widgets",
					placement: "left"
				},
				{
					element: "#toggle-right-sidebar",
					title: "Chat",
					content: "Show/hide chat contacts sidebar",
					placement: "left"
				}
			],
			template: "<div class='popover tour'> " +
						"<div class='arrow'></div> " +
						"<h3 class='popover-title'></h3>" +
						"<div class='popover-content'></div>" +
						"<div class='popover-navigation'>" +
							"<div class='btn-group'>" +
								"<button class='btn btn-default' data-role='prev'>« Prev</button>" +
								"<button class='btn btn-primary' data-role='next'>Next »</button>" +
								"<button class='btn btn-warning' data-role='end'>End tour</button>" +
							"</div>" +
						"</div>" +
					"</div>",
			onEnd: function(tour) {
				$('#btn-restart-tour').prop('disabled', false);
			}
		});

		queenTour.init();
		queenTour.start();

		$('#btn-restart-tour').click( function() {
			queenTour.restart();
			$(this).prop('disabled', true);
		});
	}


	//*******************************************
	/*	WIZARD
	/********************************************/

	if( $('#demo-wizard').length > 0 ) {
		var btnNext, btnPrev;

		btnNext = $('.wizard-wrapper').find('.btn-next');
		btnPrev = $('.wizard-wrapper').find('.btn-prev');

		$('#demo-wizard').on('change', function(e, data) {
			// validation
			if( $('#form'+data.step).length > 0 ) {
				var parsleyForm = $('#form'+data.step).parsley();
				parsleyForm.validate();

				if( !parsleyForm.isValid() )
					return false;
			}
			
			// last step button
			if(data.step === 3 && data.direction == 'next') {
				btnNext.text(' Create My Account')
				.prepend('<i class="icon ion-checkmark-circled"></i>')
				.removeClass('btn-primary').addClass('btn-success');
			}else{
				btnNext.text('Next ').
				append('<i class="icon ion-arrow-right-c"></i>')
				.removeClass('btn-success').addClass('btn-primary');
			}

		}).on('finished', function(){
			alert('Your account has been created.');
		});

		btnNext.click( function(){
			$('#demo-wizard').wizard('next');
		});

		btnPrev.click( function(){
			$('#demo-wizard').wizard('previous');
		});
	}

	if( $('#demo-wizard2').length > 0 ) {
		$('#demo-wizard2').bootstrapWizard({
			'tabClass': 'nav nav-pills',
			onTabClick: function() {
				return false;
			},
			onNext: function(tab, navigation, index) {
				var total = navigation.find('li').length;
				var current = index+1;
				var parsleyForm = $('#form-circle-wizard'+index).parsley();

				// if not last tab
				if(current <= total) {
					tab.addClass('done');

					// form validation
					parsleyForm.validate();

					if( !parsleyForm.isValid() )
						return false;
				}
			},
			onTabShow: function(tab, navigation, index) {
				var total = navigation.find('li').length;
				var current = index+1;

				// if last button
				if(current >= total ) {
					$('#demo-wizard2').find('.pager .next').hide();
					$('#demo-wizard2').find('.pager .last').show().removeClass('disabled');

					// show confirmation info
					$('#outputEmail').text($('#email2').val());
					$('#outputName').text($('#name2').val());
					$('#outputNickname').text($('#nickname').val());
					$('#outputBio').text($('#bio').val());

				} else {
					$('#demo-wizard2').find('.pager .next').show();
					$('#demo-wizard2').find('.pager .last').hide();
				}

				tab.removeClass('done');
			},
			onLast: function(tab, navigation, index) {
				alert('Your account has been created.');
			},
		});
	}


	//*******************************************
	/*	REAL TIME STAT
	/********************************************/

	if($('#cpu-usage').length > 0) {
		var cpuUsage = $('#cpu-usage').easyPieChart({
			size: 130,
			barColor: function(percent) {
				return "rgb(" + Math.round(200 * percent / 100) + ", " + Math.round(200 * (1.1 - percent / 100)) + ", 0)";
			},
			trackColor: 'rgba(73, 73, 73, 0.2)',
			scaleColor: false,
			lineWidth: 5,
			lineCap: "square",
			animate: 800
		});

		setInterval( function() {
			var randomVal;
			randomVal = getRandomInt(0, 100);

			cpuUsage.data('easyPieChart').update(randomVal);
			cpuUsage.find('.percent').text(randomVal + '%');
		}, 3000);

		function getRandomInt(min, max) {
			return Math.floor(Math.random() * (max - min + 1)) + min;
		}
	}



	//*******************************************
	/*	DASHBOARD PAGE
	/********************************************/
	
	if( $('.completeness-meter').length > 0 ) {
		var cPbar = $('.completeness-progress');

		if( $('.progress-bar').length > 0 ) {
			cPbar.progressbar({
				display_text: 'fill',
				update: function(current_percentage) {
					$('.completeness-percentage').text(current_percentage+'%');

					if(current_percentage == 100) {
						$('.complete-info').addClass('text-success').html('<i class="ion ion-checkmark-circled"></i> Hooray, it\'s done!');
						cPbar.removeClass('progress-bar-info').addClass('progress-bar-success');
						$('.completeness-meter .editable').editable('disable');
					}
				}
			});
		}

		$.fn.editable.defaults.mode = 'inline';

		$('#complete-phone-number').on('shown', function(e, editable) {
			editable.input.$input.mask('(999) 999-9999');
		}).on('hidden', function(e, reason) {
			if(reason == 'save') {
				$(this).parent().prepend('Phone: ');
				updateProgressBar(cPbar, 10);
			}
		});
		$('#complete-sex').on('hidden', function(e, reason) {
			if(reason == 'save') {
				$(this).parent().prepend('Sex: ');
				updateProgressBar(cPbar, 10);
			}
		});
		$('#complete-birthdate').on('hidden', function(e, reason) {
			if(reason == 'save') {
				$(this).parent().prepend('Birthdate: ');
				updateProgressBar(cPbar, 10);
			}
		});
		$('#complete-nickname').on('shown', function(e, editable) {
			editable.input.$input.val('');
		}).on('hidden', function(e, reason) {
			if(reason == 'save') {
				$(this).parent().prepend('Nickname: ');
				updateProgressBar(cPbar, 10);
			}
		});

		$('.completeness-meter #complete-phone-number').editable();
		$('#complete-sex').editable({
			source: [
				{value: 1, text: 'Male'},
				{value: 2, text: 'Female'}
			]
		});
		$('#complete-birthdate').editable();
		$('#complete-nickname').editable();
	}

	function updateProgressBar(pbar, valueAdded) {
		pbar.attr('data-transitiongoal', parseInt(pbar.attr('data-transitiongoal'))+valueAdded).progressbar();
	}

});