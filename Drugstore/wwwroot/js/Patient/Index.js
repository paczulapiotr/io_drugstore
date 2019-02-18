function initialize() {
	
	const tableBody = document.querySelector('.patient-menu table tbody');
	const totalCost = document.querySelector('.patient-menu p span');
	const button = document.querySelector('.patient-menu .date-picker button');
    const prev = document.querySelector('.patient-menu .navigation .previous');
    const next = document.querySelector('.patient-menu .navigation .next');

	let initialData = {
		start: moment().startOf('hour'),
		end: moment().startOf('hour').add(7, 'day')
	}
	let context = {
		start: initialData.start.format('YYYY/MM/DD'),
        end: initialData.end.format('YYYY/MM/DD'),
        canSearch: false,
		page: 1,
		totalPages: 1
	};

	function updateTable(page = 1) {
		const { start, end } = context;
		context.canSearch = false;
		fetch(`/Patient/TreatmentData?start=${start}&end=${end}&page=${page}`,{ method: "POST"})
		.then(response=>response.json())
		.then(data=>{
			
			if(data.isValid){
				totalCost.innerHTML = data.totalCost.toFixed(2);
				context.page = data.currentPage;
				context.totalPages = data.totalPages;

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

				context.canSearch = true;
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
        updateTable();
        context.canSearch = true;
    }

    prev.onclick = (event) => {
        const { canSearch, page, totalPages } = context;
		if(canSearch && page > 1 && page <= totalPages) {
			updateTable(page - 1);
		}
    }

    next.onclick = (event) => {
		const { canSearch, page, totalPages } = context;
        if(canSearch && page > 0 && page < totalPages) {
			updateTable(page + 1);
		}
    }
}
initialize();