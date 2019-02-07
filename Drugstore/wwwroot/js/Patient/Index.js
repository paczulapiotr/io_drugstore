function manage() {
	
	const tableBody = document.querySelector('.patient-menu table tbody');
	const totalCost = document.querySelector('.patient-menu p span');
	const button = document.querySelector('.patient-menu .date-picker button');

	let initialData = {
		start: moment().startOf('hour'),
		end: moment().startOf('hour').add(7, 'day')
	}
	let context = {
		start: initialData.start.format('YYYY/MM/DD'),
		end: initialData.end.format('YYYY/MM/DD')
	};

	function updateTable(startDate, endDate) {

		fetch(`/Patient/TreatmentData?start=${startDate}&end=${endDate}`,{ method: "POST"})
		.then(response=>response.json())
		.then(data=>{
			
			if(data.isValid){
				totalCost.innerHTML = data.totalCost.toFixed(2);

				while (tableBody.firstChild) {
					tableBody.removeChild(tableBody.firstChild);
				}

				data.prescriptions.map(p=>{
					let tr = document.createElement('tr');
					
					let td1 = document.createElement('td');  
					td1.innerText = p.date;
					
					let td2 = document.createElement('td');  
					td2.innerText = p.doctor;
					
					let td3 = document.createElement('td');  
					td3.innerText = p.price.toFixed(2);
					
					let td4 = document.createElement('td');  
					let form = document.createElement('form');
					form.method='get';
					form.action='/Patient/Prescription'
					let input = document.createElement('input')
					input.hidden = true;
					input.name = 'prescriptionId';
					input.value = p.id;
					let button = document.createElement('button');
					button.innerText = 'Pokaż';
					button.classList.add('btn');
					form.appendChild(input);
					form.appendChild(button);
					td4.appendChild(form);

					tr.appendChild(td1);
					tr.appendChild(td2);
					tr.appendChild(td3);
					tr.appendChild(td4);

					tableBody.appendChild(tr);
				});
			}		
			else {
				console.log(data.error);
			}
		})
		.catch(error=>console.log(error));
	}

	$('.date-picker input').daterangepicker({
		"autoApply": true,
	  startDate: initialData.start,
	  endDate: initialData.end
	},
	function(start, end, label) {
		context.start = start.format('YYYY/MM/DD');
		context.end = end.format('YYYY/MM/DD');
	});

	button.onclick = (event) => {
		console.log(context);
		updateTable(context.start, context.end);
	}
}
manage();