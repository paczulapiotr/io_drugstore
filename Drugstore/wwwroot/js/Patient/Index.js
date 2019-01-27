function manage() {
	
	function updateTable(startDate, endDate) {

		fetch(`/Patient/TreatmentData?start=${startDate}&end=${endDate}`,{ method: "POST"})
		.then(response=>response.json())
		.then(data=>console.log(data));
	}

	$('.date-picker').daterangepicker({
		"autoApply": true,
	  startDate: moment().startOf('hour'),
	  endDate: moment().startOf('hour').add(7, 'day')
	},
	function(start, end, label) {
		const startDate = start.format('YYYY/MM/DD');
		const endDate = end.format('YYYY/MM/DD');
		updateTable(startDate,endDate);
	  });
}
manage();