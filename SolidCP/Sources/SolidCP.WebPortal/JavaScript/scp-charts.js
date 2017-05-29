$(document).ready(function(){

	/* general chart parameters for headline on index page */
	var headlineChart = {
		pointsFillColor: '#fff',
		pointsLineWidth: 4,
		linesLineWidth: 2,
		gridTickColor: 'rgba(255, 255, 255, 0.5)',
		axisFontColor: '#fff',
		xTickColor: '#fff'
	}

	//*******************************************
	/*	HEADLINE CHART
	/********************************************/

	/* manually show/hide tab content to avoid chart visual problems */
	if( $('.headline').length > 0 ) {

		/* init chart */
		$('.headline a:first').tab('show');
		headlineSummaryChart('#headline-summary-chart');

		$('.headline a').click( function(e) {
			e.preventDefault();

			$tabId = $(this).attr('href');
			$chartId = $(this).attr('data-cid');
			
			/* adjust tab nav status */
			$('.headline li').removeClass('active');
			$(this).parent().addClass('active');

			/* show/hide tab pane */
			$('.headline .tab-pane').css('opacity', 0).removeClass('active');
			$('.headline .tab-pane'+$tabId).addClass('active').animate({
				opacity: 1
			}, 300);

			if( $chartId == '#headline-summary-chart' ) {
				headlineSummaryChart($chartId);
			}else if( $chartId == '#headline-sales-chart' ) {
				headlineSalesChart($chartId);
			}else if( $chartId == '#headline-bar-chart' ) {
				headlineBarChart($chartId);
			}else if( $chartId == '#headline-mini-chart' ) {
				headlineMiniChart();
			}else if( $chartId == '#headline-pie-chart' ) {
				headlinePieChart();
			}
		});
	}

	/* general chart parameters for demo charts page and dashboard charts */
	var demoChart = {
		linesFill: true,
		pointsFillColor: '#fff',
		pointsLineWidth: 2,
		linesLineWidth: 3,
		gridTickColor: '#e4e4e4',
		axisFontColor: '#555',
		xTickColor: '#fff'
	}

	//*******************************************
	/*	DASHBOARD CHARTS
	/********************************************/

	$('#dashboard-stat-tab a:first').tab('show');
	if( $('#dashboard-sales-chart').length > 0 )
		chartBarVertical( $('#dashboard-sales-chart') );

	$('#dashboard-stat-tab a').click( function(e) {
		e.preventDefault();

		$tabId = $(this).attr('href');
		$chartId = $(this).attr('data-cid');

		$('#dashboard-stat-tab li').removeClass('active');
		$(this).parents('li').addClass('active');

		/* show/hide tab pane */
		$('#dashboard-stat-tab-content .tab-pane').css('opacity', 0).removeClass('active');
		$('#dashboard-stat-tab-content .tab-pane'+$tabId).addClass('active').animate({
			opacity: 1
		}, 300);

		if( $chartId == '#dashboard-sales-chart' ) {
			chartBarVertical($chartId);
		}else if($chartId == '#dashboard-visits-chart') {
			demoChart.linesFill = false;
			chartWeek($chartId);
		}
	});

	//*******************************************
	/*	CHART DEMO PAGE
	/********************************************/

	if($('.basic-charts').length > 0) {

		if( $('#demo-line-chart').length > 0 ) 
			chartYear( $('#demo-line-chart') );
		if( $('#demo-area-chart').length > 0 )
			chartWeek( $('#demo-area-chart') );
		if( $('#demo-vertical-bar-chart').length > 0 )
			chartBarVertical( $('#demo-vertical-bar-chart') );
		if( $('#demo-horizontal-bar-chart').length > 0 )
			chartBarHorizontal( $('#demo-horizontal-bar-chart') );
		if( $('#demo-multi-types-chart').length > 0 )
			chartMonth( $('#demo-multi-types-chart') );


		//*******************************************
		/*	DONUT CHART
		/********************************************/

		if( $('#demo-donut-chart').length > 0 ) {
			var data = [
				{ label: "Direct",  data: 65},
				{ label: "Referral",  data: 20},
				{ label: "Others", data: 15}
			];

			$.plot('#demo-donut-chart', data, {
				series: {
					pie: {
						show: true,
						innerRadius: .4,
						stroke: {
							width: 4,
							color: "#F9F9F9"
						},
						label: {
							show: true,
							radius: 3/4,
							formatter: donutLabelFormatter
						}
					},
				},
				legend: {
					show: false
				},
				grid: {
					hoverable: true
				},
				colors: ["#d9d9d9", "#5399D6", "#d7ea2b"],
			});
		}


		//*******************************************
		/*	MINI BAR CHART
		/********************************************/

		var values = getRandomValues();
		var params = {
			type: 'bar',
			barWidth: 5,
			height: 25
		}

		params.barColor = '#CE7B11';
		$('#mini-bar-chart1').sparkline(values[0], params);
		params.barColor = '#1D92AF';
		$('#mini-bar-chart2').sparkline(values[1], params);
		params.barColor = '#3F7577';
		$('#mini-bar-chart3').sparkline(values[2], params);


		//*******************************************
		/*	MINI PIE CHART
		/********************************************/
		
		var visitData = [[30, 15, 55], [65, 25, 10], [70, 30]];
		var params = {
			type: "pie",
			sliceColors: ["#ff9800", "#5399D6", "#d7ea2b"],
		}

		$('#mini-pie-chart1').sparkline(visitData[0], params);
		$('#mini-pie-chart2').sparkline(visitData[1], params);
		$('#mini-pie-chart3').sparkline(visitData[2], params);


		//*******************************************
		/*	INLINE SPARKLINE WIDGET
		/********************************************/
	
		var values1 = getRandomValues();
		var sparklineWidget = function() {

			var params = {
				width: '60px',
				height: '30px',
				lineWidth: '2',
				lineColor: '#1D92AF',
				fillColor: 'rgba(29,146,175,0.2) ',
				spotRadius: '2',
				highlightLineColor: '#aedaff',
				highlightSpotColor: '#71aadb',
				spotColor: false,
				minSpotColor: false,
				maxSpotColor: false,
				disableInteraction: false
			}

			$('#mini-line-chart1').sparkline(values1[0], params);
			params.lineColor = '#ef2020';
			params.fillColor = 'rgba(239,32,32,0.2)';
			$('#mini-line-chart2').sparkline(values1[1], params);
			params.lineColor = '#ff9800';
			params.fillColor = 'rgba(255,152,0,0.2)';
			$('#mini-line-chart3').sparkline(values1[2], params);
		}

		sparklineWidget();
	}


	//*******************************************
	/*	INTERACTIVE CHARTS DEMO PAGE
	/********************************************/

	if($('.interactive-charts').length > 0) {
		chartToggleSeries( $('#demo-toggle-series-chart') );
		chartSelectZoomSeries( $('#demo-select-zoom-chart') );
	}

	//*******************************************
	/*	CHART FUNCTIONS
	/********************************************/

	// init headline flot chart: summary
	function headlineSummaryChart(placeholder) {

		var currentWeek = [
			[gt(2013, 10, 21), 78], [gt(2013, 10, 22), 215], [gt(2013, 10, 23), 250], [gt(2013, 10, 24), 230], [gt(2013, 10, 25), 245], 
			[gt(2013, 10, 26), 260], [gt(2013, 10, 27), 340]
		];

		var lastWeek = [
			[gt(2013, 10, 21), 123], [gt(2013, 10, 22), 180], [gt(2013, 10, 23), 285], [gt(2013, 10, 24), 250], [gt(2013, 10, 25), 205], 
			[gt(2013, 10, 26), 198], [gt(2013, 10, 27), 195]
		];

		var plot = $.plot(placeholder, 
			[
				{
					data: currentWeek,
					label: "Current Week",
					points: {
						fillColor: headlineChart.pointsFillColor
					},
					lines: {
						fill: true,
						fillColor: { colors: [ {opacity: 0.1}, {opacity: 0.8} ] }
					}
				},
				{
					data: lastWeek,
					label: "Last Week",
					points: {
						fillColor: headlineChart.pointsFillColor
					},
				}
			], 

			{
			series: {
				lines: {
					show: true,
					lineWidth: headlineChart.lineWidth,
				},
				points: {
					show: true,
					lineWidth: headlineChart.pointsLineWidth,
					fill: true
				},

				shadowSize: 0
			},
			grid: {
				hoverable: true, 
				clickable: true,
				borderWidth: 0,
				tickColor: headlineChart.gridTickColor
			},
			colors: ["#238C47", "#A7F43D"],
			yaxis: {
				font: { color: headlineChart.axisFontColor },
				ticks: 5
			},
			xaxis: {
				mode: "time",
				timezone: "browser",
				minTickSize: [1, "day"],
				timeformat: "%a",
				font: { color: headlineChart.axisFontColor },
				tickColor: "transparent",
				autoscaleMargin: 0.02
			},
			legend: {
				labelBoxBorderColor: "transparent",
				backgroundColor: "transparent"
			},
			tooltip: true,
			tooltipOpts: {
				content: '%s: %y'
			}

		});

		return plot;
	}

	// init headline flot chart: sales area chart
	function headlineSalesChart(placeholder) {

		var currentWeek = [
			[gt(2013, 10, 21), 50], [gt(2013, 10, 22), 75], [gt(2013, 10, 23), 205], [gt(2013, 10, 24), 170], [gt(2013, 10, 25), 205], 
			[gt(2013, 10, 26), 198], [gt(2013, 10, 27), 195]
		];

		var plot = $.plot(placeholder, 
			[
				{
					data: currentWeek,
					label: "Current Week",
				},
			], 

			{
			series: {
				lines: {
					show: true,
					lineWidth: headlineChart.linesLineWidth,
					fill: true,
				},
				points: {
					show: true,
					lineWidth: headlineChart.pointsLineWidth,
					fill: true,
					fillColor: headlineChart.pointsFillColor
				},

				shadowSize: 0
			},
			grid: {
				hoverable: true, 
				clickable: true,
				borderWidth: 0,
				tickColor: headlineChart.gridTickColor
			},
			colors: ["#78AF2C"],
			yaxis: {
				font: { color: headlineChart.axisFontColor },
			},
			xaxis: {
				mode: "time",
				timezone: "browser",
				minTickSize: [1, "day"],
				timeformat: "%a",
				font: { color: headlineChart.axisFontColor },
				tickColor: "transparent",
				autoscaleMargin: 0.02
			},
			legend: {
				labelBoxBorderColor: "transparent",
				backgroundColor: "transparent"
			},
			tooltip: true,
			tooltipOpts: {
				content: '%s: %y'
			}

		});

		return plot;
	}

	// init headline flot chart: visit bar chart
	function headlineBarChart(placeholder) {
		var basic = [
			[gt(2013, 10, 21), 188], [gt(2013, 10, 22), 205], [gt(2013, 10, 23), 250], [gt(2013, 10, 24), 230], [gt(2013, 10, 25), 245], [gt(2013, 10, 26), 260], [gt(2013, 10, 27), 290]
		];

		var gold = [
			[gt(2013, 10, 21), 100], [gt(2013, 10, 22), 110], [gt(2013, 10, 23), 155], [gt(2013, 10, 24), 120], [gt(2013, 10, 25), 135], [gt(2013, 10, 26), 150], [gt(2013, 10, 27), 175]
		];

		var platinum = [
			[gt(2013, 10, 21), 75], [gt(2013, 10, 22), 65], [gt(2013, 10, 23), 80], [gt(2013, 10, 24), 60], [gt(2013, 10, 25), 65], [gt(2013, 10, 26), 80], [gt(2013, 10, 27), 110]
		];

		var plot = $.plot(placeholder, 
			[
				{
					data: basic,
					label: "Basic"
				},
				{
					data: gold,
					label: "Gold"
				},
				{
					data: platinum,
					label: "Platinum"
				}
			], 
			{
				bars: {
					show: true,
					barWidth: 15*60*60*300,
					fill: true,
					order: true,
					lineWidth: 0,
					fillColor: { colors: [ { opacity: 0.8 }, { opacity: 0.1 } ] }
				},
				grid: {
					hoverable: true, 
					clickable: true,
					borderWidth: 0,
					tickColor: headlineChart.gridTickColor,
				},
				colors: ["#7bae16", "#d7ea2b", "#5399D6"],
				yaxis: {
					font: { color: headlineChart.axisFontColor },
				},
				xaxis: {
					mode: "time",
					timezone: "browser",
					minTickSize: [1, "day"],
					timeformat: "%a",
					font: { color: headlineChart.axisFontColor },
					tickColor: "transparent",
					autoscaleMargin: 0.2
				},
				legend: {
					labelBoxBorderColor: "transparent",
					backgroundColor: "transparent"
				},
				tooltip: true,
				tooltipOpts: {
					content: '%s: %y'
				}
			});

		return plot;
	}

	/* mini stat on headline, social mini stat */
	function headlineMiniChart() {

		/* mini line charts */
		var statValues = getRandomValues();
		var params = {
			width: '120px',
			height: '30px',
			lineWidth: '2',
			fillColor: false,
			spotRadius: '2',
			lineColor: '#1D92AF',
			highlightLineColor: '#1D92AF',
			highlightSpotColor: '#1D92AF',
			spotColor: false,
			minSpotColor: false,
			maxSpotColor: false,
			disableInteraction: false
		}

		$('#mini-stat-likes').sparkline(statValues[0], params);
		
		params.lineColor = '#3F7577';
		params.highlightLineColor = '#3F7577';
		params.highlightSpotColor = '#3F7577';
		$('#mini-stat-subscribers').sparkline(statValues[1], params);
		
		params.lineColor = '#CE7B11';
		params.highlightLineColor = '#CE7B11';
		params.highlightSpotColor = '#CE7B11';
		$('#mini-stat-followers').sparkline(statValues[2], params);

		/* mini bar charts */ 
		var statBarValues = getRandomValues();
		var params = {
			type: 'bar',
			barWidth: 15,
			height: 30
		}

		params.barColor = '#1D92AF';
		$('#mini-stat-users').sparkline(statBarValues[0], params);
		params.barColor = '#3F7577';
		$('#mini-stat-customers').sparkline(statBarValues[1], params);
		params.barColor = '#CE7B11';
		$('#mini-stat-contributors').sparkline(statBarValues[2], params);
	}

	/* init pie chart on headline */
	function headlinePieChart() {
		var barColors = ['#78AF2C', '#1D92AF', '#238C47', '#CE7B11'];

		$('.headline .pie-chart').each( function(index) {
			$(this).easyPieChart({
				size: 130,
				barColor: barColors[index],
				trackColor: 'rgba(255, 255, 255, 0.2)',
				scaleColor: false,
				lineWidth: 5,
				lineCap: "square",
				animate: 1000
			});
		});
	}

	// helper function, get day
	function gt(y, m, d) {
		return new Date(y, m-1, d).getTime();
	}

	function chartWeek(placeholder) {

		var currentWeek = [
			[gt(2013, 10, 21), 208], [gt(2013, 10, 22), 215], [gt(2013, 10, 23), 250], [gt(2013, 10, 24), 230], [gt(2013, 10, 25), 245], 
			[gt(2013, 10, 26), 260], [gt(2013, 10, 27), 300]
		];

		var lastWeek = [
			[gt(2013, 10, 21), 50], [gt(2013, 10, 22), 75], [gt(2013, 10, 23), 205], [gt(2013, 10, 24), 170], [gt(2013, 10, 25), 205], 
			[gt(2013, 10, 26), 198], [gt(2013, 10, 27), 195]
		];

		var plot = $.plot(placeholder, 
			[
				{
					data: currentWeek,
					label: "Current Week",
				},
				{
					data: lastWeek,
					label: "Last Week",
				}
			], 

			{
			series: {
				lines: {
					show: true,
					lineWidth: demoChart.linesLineWidth,
					fill: demoChart.linesFill,
				},
				points: {
					show: true,
					lineWidth: demoChart.pointsLineWidth,
					fill: true,
					fillColor: demoChart.pointsFillColor
				},

				shadowSize: 0
			},
			grid: {
				hoverable: true, 
				clickable: true,
				borderWidth: 0,
				tickColor: demoChart.gridTickColor
			},
			colors: ["#ff9800", "#ccc"],
			yaxis: {
				font: { color: demoChart.axisFontColor },
				ticks: 5
			},
			xaxis: {
				mode: "time",
				timezone: "browser",
				minTickSize: [1, "day"],
				timeformat: "%a",
				font: { color: demoChart.axisFontColor },
				tickColor: "transparent",
				autoscaleMargin: 0.02
			},
			legend: {
				labelBoxBorderColor: "transparent",
				backgroundColor: "transparent"
			},
			tooltip: true,
			tooltipOpts: {
				content: '%s: %y'
			}

		});
	}

	// init flot chart: current month
	function chartMonth(placeholder) {

		var visit = [
			[gt(2013, 10, 1), 100], [gt(2013, 10, 2), 140], [gt(2013, 10, 3), 160], [gt(2013, 10, 4),190], [gt(2013, 10, 5),170], [gt(2013, 10, 6), 200], [gt(2013, 10, 7), 220],
			[gt(2013, 10, 8), 250], [gt(2013, 10, 9),280], [gt(2013, 10, 10), 240], [gt(2013, 10, 11), 250], [gt(2013, 10, 12), 260], [gt(2013, 10, 13), 300], [gt(2013, 10, 14), 320],
			[gt(2013, 10, 15), 330], [gt(2013, 10, 16), 370], [gt(2013, 10, 17), 390], [gt(2013, 10, 18), 350], [gt(2013, 10, 19), 340], [gt(2013, 10, 20), 320], [gt(2013, 10, 21), 370],
			[gt(2013, 10, 22), 400], [gt(2013, 10, 23), 440], [gt(2013, 10, 24), 450], [gt(2013, 10, 25), 470], [gt(2013, 10, 26), 450], [gt(2013, 10, 27), 500], [gt(2013, 10, 28), 540],
			[gt(2013, 10, 29), 600], [gt(2013, 10, 30), 580], [gt(2013, 10, 31), 620]
		];

		var val = [
			[gt(2013, 10, 1), 20], [gt(2013, 10, 2), 28], [gt(2013, 10, 3), 32], [gt(2013, 10, 4), 40], [gt(2013, 10, 5), 35], [gt(2013, 10, 6), 40], [gt(2013, 10, 7), 45],
			[gt(2013, 10, 8), 25], [gt(2013, 10, 9), 60], [gt(2013, 10, 10), 48], [gt(2013, 10, 11), 53], [gt(2013, 10, 12), 58], [gt(2013, 10, 13), 60], [gt(2013, 10, 14), 65],
			[gt(2013, 10, 15), 66], [gt(2013, 10, 16), 60], [gt(2013, 10, 17), 79], [gt(2013, 10, 18), 75], [gt(2013, 10, 19), 34], [gt(2013, 10, 20), 32], [gt(2013, 10, 21), 75],
			[gt(2013, 10, 22), 88], [gt(2013, 10, 23), 99], [gt(2013, 10, 24), 86], [gt(2013, 10, 25), 83], [gt(2013, 10, 26), 45], [gt(2013, 10, 27), 50], [gt(2013, 10, 28), 100],
			[gt(2013, 10, 29), 125], [gt(2013, 10, 30), 110], [gt(2013, 10, 31), 130]
		];

		var plot = $.plot(placeholder, 
			[
				{
					data: visit,
					label: "Visits",
					bars: {
						show: true,
						fill: false,
						barWidth: 0.1,
						align: "center",
						lineWidth: 25
					}
				},
				{
					data: val,
					label: "Sales"
				}
			], 

			{
				series: {
					lines: {
						show: true,
						lineWidth: 2, 
						fill: false
					},
					points: {
						show: true, 
						lineWidth: 3,
						fill: true,
						fillColor: demoChart.pointsFillColor
					},
					shadowSize: 0
				},
				grid: {
					hoverable: true, 
					clickable: true,
					borderWidth: 0,
					tickColor: demoChart.gridTickColor,
					
				},
				colors: ["rgba(217,217,217, 0.8)", "#238C47"],
				yaxis: {
					font: { color: demoChart.axisFontColor },
					ticks: 8,
				},
				xaxis: {
					mode: "time",
					timezone: "browser",
					minTickSize: [1, "day"],
					font: { color: demoChart.axisFontColor },
					tickColor: demoChart.xTickColor,
					autoscaleMargin: 0.02
				},
				legend: {
					labelBoxBorderColor: "transparent",
					backgroundColor: "transparent"
				},
				tooltip: true,
				tooltipOpts: {
					content: '%s: %y'
				}
			}
		); 

	}

	// init flot chart: current year
	function chartYear(placeholder) {

		var visit = [
			[gt(2013, 1, 1), 200], [gt(2013, 2, 1), 300], [gt(2013, 3, 1), 360], [gt(2013, 4, 1), 340], [gt(2013, 5, 1), 440], [gt(2013, 6, 1), 600], [gt(2013, 7, 1), 1050],
			[gt(2013, 8, 1), 1700], [gt(2013, 9, 1), 1100], [gt(2013, 10, 1), 1200], [gt(2013, 11, 1), 1300], [gt(2013, 12, 1), 1500]
		];

		var val = [
			[gt(2013, 1, 1), 100], [gt(2013, 2, 1), 155], [gt(2013, 3, 1), 180], [gt(2013, 4, 1), 172], [gt(2013, 5, 1), 222], [gt(2013, 6, 1), 300], [gt(2013, 7, 1), 550],
			[gt(2013, 8, 1), 452], [gt(2013, 9, 1), 552], [gt(2013, 10, 1), 600], [gt(2013, 11, 1), 680], [gt(2013, 12, 1), 750]
		];

		var plot = $.plot(placeholder, 
			[
				{
					data: visit,
					label: "Visits"
				},
				{
					data: val,
					label: "Sales"

				}
			], 

			{
				series: {
					lines: {
						show: true,
						lineWidth: demoChart.linesLineWidth, 
						fill: false
					},
					points: {
						show: true, 
						lineWidth: demoChart.pointsLineWidth,
						fill: true,
						fillColor: demoChart.pointsFillColor
					},
					shadowSize: 0
				},
				grid: {
					hoverable: true, 
					clickable: true,
					borderWidth: 0,
					tickColor: demoChart.gridTickColor,
					
				},
				colors: ["#919191", "#59b7de"],
				yaxis: {
					font: { color: demoChart.axisFontColor },
					ticks: 8,
				},
				xaxis: {
					mode: "time",
					timezone: "browser",
					minTickSize: [1, "month"],
					font: { color: demoChart.axisFontColor },
					tickColor: demoChart.xTickColor,
					autoscaleMargin: 0.02
				},
				legend: {
					labelBoxBorderColor: "transparent",
					backgroundColor: "transparent"
				},
				tooltip: true,
				tooltipOpts: {
					content: '%s: %y'
				}
			}
		);
	}

	// init flot chart: vertical bar chart
	function chartBarVertical(placeholder) {
		var basic = [
			[gt(2013, 10, 21), 188], [gt(2013, 10, 22), 205], [gt(2013, 10, 23), 250], [gt(2013, 10, 24), 230], [gt(2013, 10, 25), 245], [gt(2013, 10, 26), 260], [gt(2013, 10, 27), 290]
		];

		var gold = [
			[gt(2013, 10, 21), 100], [gt(2013, 10, 22), 110], [gt(2013, 10, 23), 155], [gt(2013, 10, 24), 120], [gt(2013, 10, 25), 135], [gt(2013, 10, 26), 150], [gt(2013, 10, 27), 175]
		];

		var platinum = [
			[gt(2013, 10, 21), 75], [gt(2013, 10, 22), 65], [gt(2013, 10, 23), 80], [gt(2013, 10, 24), 60], [gt(2013, 10, 25), 65], [gt(2013, 10, 26), 80], [gt(2013, 10, 27), 110]
		];

		var plot = $.plot(placeholder, 
			[
				{
					data: basic,
					label: "Basic"
				},
				{
					data: gold,
					label: "Gold"
				},
				{
					data: platinum,
					label: "Platinum"
				}
			], 
			{
				bars: {
					show: true,
					barWidth: 15*60*60*300,
					fill: true,
					order: true,
					lineWidth: 0,
					fillColor: { colors: [ { opacity: 1 }, { opacity: 1 } ] }
				},
				grid: {
					hoverable: true, 
					clickable: true,
					borderWidth: 0,
					tickColor: demoChart.gridTickColor,
					
				},
				colors: ["#919191", "#5399D6", "#d7ea2b"],
				yaxis: {
					font: { color: demoChart.axisFontColor },
				},
				xaxis: {
					mode: "time",
					timezone: "browser",
					minTickSize: [1, "day"],
					timeformat: "%a",
					font: { color: demoChart.axisFontColor },
					tickColor: demoChart.xTickColor,
					autoscaleMargin: 0.2
				},
				legend: {
					labelBoxBorderColor: "transparent",
					backgroundColor: "transparent"
				},
				tooltip: true,
				tooltipOpts: {
					content: '%s: %y'
				}
			}
		);
	}

	// init flot chart: horizontal bar chart
	function chartBarHorizontal(placeholder) {
		var basic = [
			[188, 1], [200, 2], [225, 3], [230, 4], [250, 5]
		];

		var gold = [
			[200, 1], [220, 2], [210, 3], [240, 4], [240, 5]
		];

		var platinum = [
			[100, 1], [90, 2], [150, 3], [200, 4], [235, 5]
		];

		var plot = $.plot(placeholder, 
			[
				{
					data: basic,
					label: "Basic"
				},
				{
					data: gold,
					label: "Gold"
				},
				{
					data: platinum,
					label: "Platinum"
				}
			], 
			{
				bars: {
					show: true,
					horizontal: true,
					barWidth: 0.2,
					fill: true,
					order: true,
					lineWidth: 0,
					fillColor: { colors: [ { opacity: 1 }, { opacity: 1 } ] }
				},
				grid: {
					hoverable: true, 
					clickable: true,
					borderWidth: 0,
					tickColor: "#E4E4E4",
					
				},
				colors: ["#d9d9d9", "#5399D6", "#d7ea2b"],
				xaxis: {
					autoscaleMargin: 0.2
				},
				legend: {
					labelBoxBorderColor: "transparent",
					backgroundColor: "transparent"
				},
				tooltip: true,
				tooltipOpts: {
					content: '%s: %x'
				}
			}
		);
	}

	// init interactive flot chart: toggle on/off data series
	function chartToggleSeries(placeholder) {

		var datasets = {
			"usa": {
				label: "USA",
				data: [[1988, 483994], [1989, 479060], [1990, 457648], [1991, 401949], [1992, 424705], [1993, 402375], [1994, 377867], [1995, 357382], [1996, 337946], [1997, 336185], [1998, 328611], [1999, 329421], [2000, 342172], [2001, 344932], [2002, 387303], [2003, 440813], [2004, 480451], [2005, 504638], [2006, 528692]]
			},
			"russia": {
				label: "Russia",
				data: [[1988, 218000], [1989, 203000], [1990, 171000], [1992, 42500], [1993, 37600], [1994, 36600], [1995, 21700], [1996, 19200], [1997, 21300], [1998, 13600], [1999, 14000], [2000, 19100], [2001, 21300], [2002, 23600], [2003, 25100], [2004, 26100], [2005, 31100], [2006, 34700]]
			},
			"uk": {
				label: "UK",
				data: [[1988, 62982], [1989, 62027], [1990, 60696], [1991, 62348], [1992, 58560], [1993, 56393], [1994, 54579], [1995, 50818], [1996, 50554], [1997, 48276], [1998, 47691], [1999, 47529], [2000, 47778], [2001, 48760], [2002, 50949], [2003, 57452], [2004, 60234], [2005, 60076], [2006, 59213]]
			},
			"germany": {
				label: "Germany",
				data: [[1988, 55627], [1989, 55475], [1990, 58464], [1991, 55134], [1992, 52436], [1993, 47139], [1994, 43962], [1995, 43238], [1996, 42395], [1997, 40854], [1998, 40993], [1999, 41822], [2000, 41147], [2001, 40474], [2002, 40604], [2003, 40044], [2004, 38816], [2005, 38060], [2006, 36984]]
			},
			"denmark": {
				label: "Denmark",
				data: [[1988, 3813], [1989, 3719], [1990, 3722], [1991, 3789], [1992, 3720], [1993, 3730], [1994, 3636], [1995, 3598], [1996, 3610], [1997, 3655], [1998, 3695], [1999, 3673], [2000, 3553], [2001, 3774], [2002, 3728], [2003, 3618], [2004, 3638], [2005, 3467], [2006, 3770]]
			},
			"sweden": {
				label: "Sweden",
				data: [[1988, 6402], [1989, 6474], [1990, 6605], [1991, 6209], [1992, 6035], [1993, 6020], [1994, 6000], [1995, 6018], [1996, 3958], [1997, 5780], [1998, 5954], [1999, 6178], [2000, 6411], [2001, 5993], [2002, 5833], [2003, 5791], [2004, 5450], [2005, 5521], [2006, 5271]]
			},
			"norway": {
				label: "Norway",
				data: [[1988, 4382], [1989, 4498], [1990, 4535], [1991, 4398], [1992, 4766], [1993, 4441], [1994, 4670], [1995, 4217], [1996, 4275], [1997, 4203], [1998, 4482], [1999, 4506], [2000, 4358], [2001, 4385], [2002, 5269], [2003, 5066], [2004, 5194], [2005, 4887], [2006, 4891]]
			}
		};

		// hard-code color indices to prevent them from shifting as countries are turned on/off
		var i = 0;
		$.each(datasets, function(key, val) {
			val.color = i;
			++i;
		});

		// insert checkboxes 
		var choiceContainer = $("#choices");
		$.each( datasets, function( key, val ) {

			choiceContainer.append(
				"<label class='fancy-checkbox'><input type='checkbox' name='" + key +
				"' checked='checked' id='id" + key + "'>" +
				"<span>" + val.label + "</span></label>"
			);
		});

		choiceContainer.find("input").click( function() {
			plotAccordingToChoices(placeholder, datasets);
		});		

		plotAccordingToChoices(placeholder, datasets);
	}

	function plotAccordingToChoices(placeholder, datasets) {

		var data = [];

		$("#choices").find("input:checked").each(function () {
			var key = $(this).attr("name");
			if (key && datasets[key]) {
				data.push(datasets[key]);
			}
		});

		if (data.length > 0) {
			$.plot( placeholder, data, {
				series: {
					lines: {
						show: true,
						lineWidth: 2, 
						fill: false
					},
					points: {
						show: true, 
						lineWidth: 3,
						fill: true,
						fillColor: demoChart.pointsFillColor
					},
					shadowSize: 0
				},

				grid: {
					hoverable: true, 
					clickable: true,
					borderWidth: 0,
					tickColor: demoChart.gridTickColor,
				},
				xaxis: {
					tickDecimals: 0,
					autoscaleMargin: 0.1,
					tickColor: demoChart.xTickColor
				},
				yaxis: {
					min: 0,
					max: 700000
				},
				colors: ["#919191", "#59b7de", "#d7ea2b", "#f30", "#E7A13D", "#3F7577", "#7bae16"],
				legend: {
					labelBoxBorderColor: "transparent",
					backgroundColor: "transparent",
					noColumns: 4
				},
				tooltip: true,
				tooltipOpts: {
					content: '%s: $%y'
				}
			});
		}
	}

	// init flot chart: select and zoom
	function chartSelectZoomSeries(placeholder) {

		var data = [{
			label: "United States",
			data: [[1990, 18.9], [1991, 18.7], [1992, 18.4], [1993, 19.3], [1994, 19.5], [1995, 19.3], [1996, 19.4], [1997, 20.2], [1998, 19.8], [1999, 18.9], [2000, 19.4], [2001, 17.6], [2002, 18.0], [2003, 16.8], [2004, 19.4]]
		}, {
			label: "Russia", 
			data: [[1992, 13.4], [1993, 12.2], [1994, 10.6], [1995, 10.2], [1996, 10.1], [1997, 9.7], [1998, 9.5], [1999, 9.7], [2000, 9.9], [2001, 9.9], [2002, 9.9], [2003, 10.3], [2004, 10.5]]
		}, {
			label: "United Kingdom",
			data: [[1990, 10.0], [1991, 11.3], [1992, 9.9], [1993, 9.6], [1994, 9.5], [1995, 9.5], [1996, 9.9], [1997, 9.3], [1998, 9.2], [1999, 9.2], [2000, 9.5], [2001, 9.6], [2002, 9.3], [2003, 9.4], [2004, 9.79]]
		}, {
			label: "Germany",
			data: [[1990, 12.4], [1991, 11.2], [1992, 10.8], [1993, 10.5], [1994, 10.4], [1995, 10.2], [1996, 10.5], [1997, 10.2], [1998, 10.1], [1999, 9.6], [2000, 9.7], [2001, 10.0], [2002, 9.7], [2003, 9.8], [2004, 9.79]]
		}, {
			label: "Denmark",
			data: [[1990, 9.7], [1991, 12.1], [1992, 10.3], [1993, 11.3], [1994, 11.7], [1995, 10.6], [1996, 12.8], [1997, 10.8], [1998, 10.3], [1999, 9.4], [2000, 8.7], [2001, 9.0], [2002, 8.9], [2003, 10.1], [2004, 9.80]]
		}, {
			label: "Sweden",
			data: [[1990, 5.8], [1991, 6.0], [1992, 5.9], [1993, 5.5], [1994, 5.7], [1995, 5.3], [1996, 6.1], [1997, 5.4], [1998, 5.4], [1999, 5.1], [2000, 5.2], [2001, 5.4], [2002, 6.2], [2003, 5.9], [2004, 5.89]]
		}];

		var options = {
			series: {
				lines: {
					show: true,
					lineWidth: 2, 
					fill: false
				},
				points: {
					show: true, 
					lineWidth: 3,
					fill: true,
					fillColor: demoChart.pointsFillColor
				},
				shadowSize: 0
			},
			grid: {
				hoverable: true, 
				clickable: true,
				borderWidth: 0,
				tickColor: demoChart.gridTickColor,
				
			},
			legend: {
				noColumns: 3,
				labelBoxBorderColor: "transparent",
				backgroundColor: "transparent"
			},
			xaxis: {
				tickDecimals: 0,
				tickColor: demoChart.xTickColor
			},
			yaxis: {
				min: 0
			},
			colors: ["#919191", "#59b7de", "#d7ea2b", "#f30", "#E7A13D", "#3F7577"],
			tooltip: true,
			tooltipOpts: {
				content: '%s: %y'
			},
			selection: {
				mode: "x"
			}
		};


		var plot = $.plot(placeholder, data, options);

		placeholder.bind("plotselected", function (event, ranges) {

			plot = $.plot(placeholder, data, $.extend(true, {}, options, {
				xaxis: {
					min: ranges.xaxis.from,
					max: ranges.xaxis.to
				}
			}));
		});

		$('#reset-chart-zoom').click( function() {
			plot.setSelection({
				xaxis: {
					from: 1990,
					to: 2004
				}
			});
		});
	}

	function getRandomValues() {
		// data setup
		var values = new Array(20);
		
		for (var i = 0; i < values.length; i++){
			values[i] = [5 + randomVal(), 10 + randomVal(), 15 + randomVal(), 20 + randomVal(), 30 + randomVal(),
				35 + randomVal(), 40 + randomVal(), 45 + randomVal(), 50 + randomVal()]
		}

		return values;
	}

	function randomVal(){
		return Math.floor( Math.random() * 80 );
	}

	function donutLabelFormatter(label, series) {
		return "<div class=\"donut-label\">" + label + "<br/>" + Math.round(series.percent) + "%</div>";
	}

});