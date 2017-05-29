$(document).ready(function(){

	//*******************************************
	/*	DATA TABLE
	/********************************************/

	if( $('.datatable').length > 0 ) {
		/* basic */
		$('#datatable-basic').dataTable({
			sDom:
				"t"+
				"<'row'<'col-sm-6'i><'col-sm-6'p>>"
		});

		$('#datatable-basic-scrolling').dataTable({
			scrollY: "300px",
			scrollCollapse: true,
			paging: false
		});

		/* column filters */
		var dtTable = $('#datatable-column-filter').DataTable({ // use DataTable, not dataTable
			sDom: // redefine sDom without lengthChange and default search box
				"t"+
				"<'row'<'col-sm-6'i><'col-sm-6'p>>"
		}); 

		$('#datatable-column-filter thead').append('<tr class="row-filter"><th></th><th></th><th></th><th></th><th></th></tr>');
		$('#datatable-column-filter thead .row-filter th').each( function() {
			$(this).html('<input type="text" class="form-control input-sm" placeholder="Search...">');
		});

		$('#datatable-column-filter .row-filter input').on('keyup change', function() {
			dtTable
				.column($(this).parent().index()+':visible')
				.search(this.value)
				.draw();
		});

		/* column interactive */
		$('#datatable-column-interactive').dataTable({
			sDom: "RC<'clearfix'>" +
				"<'row'<'col-sm-6'l><'col-sm-6'f>r>"+
				"t"+
				"<'row'<'col-sm-6'i><'col-sm-6'p>>",
			colVis: {
				buttonText: 'Show / Hide Columns',
				restore: "Restore",
				showAll: "Show all"
			},
		});

		/* data export */
		var exportTable = $('#datatable-data-export').DataTable({
			sDom: "T<'clearfix'>" +
				"<'row'<'col-sm-6'l><'col-sm-6'f>r>"+
				"t"+
				"<'row'<'col-sm-6'i><'col-sm-6'p>>",
				"tableTools": {
					"sSwfPath": "assets/js/plugins/datatable/exts/swf/copy_csv_xls_pdf.swf"
				}
		});

	}


	//*******************************************
	/*	CUSTOMER SUPPORT TICKET TABLE
	/********************************************/

	if( $('#ticket-table').length > 0 ) {
		$('#ticket-table').dataTable({
			sDom: "<'row'<'col-sm-6'l><'col-sm-6'f>r>"+
					"t"+
					"<'row'<'col-sm-6'i><'col-sm-6'p>>"
		});
	}

}); // end ready function
