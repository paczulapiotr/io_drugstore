
function initialize() {

	let medicineSearchButton = document.querySelector('.search-bar button');
	let medicineSearchInput = document.querySelector('.search-bar input');
    let tableBody = document.querySelector('.main-section .edit-prescription .assigned-medicines tbody');
	let medicineSearchTable = document.querySelector('.main-section .edit-prescription .medicine-search-panel table tbody');
	let saveButton = document.querySelector('#save-button');
	let id = saveButton.value;

    let context = [];
	let acquiredMedicines = [];

    renderContext = () => {
		console.info('context',context);
		console.info('acquiredMedicines',acquiredMedicines);
		while (tableBody.firstChild) {
			tableBody.removeChild(tableBody.firstChild);
		}

        context.forEach(medicine => {
            let tr = document.createElement('tr');
            let td1 = document.createElement('td');
            td1.innerText = medicine.name;

            let td2 = document.createElement('td');
            td2.innerText = medicine.pricePerOne.toFixed(2);

            let td3 = document.createElement('td');
            td3.innerText = (medicine.refundation * 100).toFixed(2);

            let td4 = document.createElement('td');
            td4.innerText = medicine.quantity;

            let td5 = document.createElement('td');
            let button = document.createElement('button');
            button.innerText = 'Usuń';
			button.value = medicine.stockId;
			button.classList.add('btn');
            button.onclick = (event) => {
                context = context.filter(m => m.stockId != event.target.value);
                renderContext();
            }
            td5.appendChild(button);

            tr.appendChild(td1);
            tr.appendChild(td2);
            tr.appendChild(td3);
            tr.appendChild(td4);
            tr.appendChild(td5);
            tableBody.appendChild(tr);
        });
	}
	
    getMedicines = (prescriptionId) => {
        fetch('/Doctor/GetPrescriptionMedicines?prescriptionId=' + prescriptionId)
            .then(response => response.json())
            .then(json => {
				context = json.data;
				console.log(context);
				renderContext();
            })
    }
	getMedicines(id);
	

	medicineSearch = (search) => {
        fetch('/Doctor/FindMedicine?search=' + search)
            .then(response => response.json())
            .then(data => {

                acquiredMedicines = data;

                while (medicineSearchTable.firstChild) {
                    medicineSearchTable.removeChild(medicineSearchTable.firstChild);
                }

                data.forEach(m => {
                    let tr = document.createElement('tr');
                    let td1 = document.createElement('td');
                    let td2 = document.createElement('td');
                    let td3 = document.createElement('td');
                    let td4 = document.createElement('td');
                    let td5 = document.createElement('td');
                    td1.innerText = m.name;
                    tr.appendChild(td1);
                    td2.innerText = m.pricePerOne.toFixed(2)
                    tr.appendChild(td2);
                    td3.innerText = (m.refundation * 100).toFixed(2);
                    tr.appendChild(td3);
                    td4.innerText = m.quantity
                    tr.appendChild(td4);
                    tr.appendChild(td5);

                    let button = document.createElement('button');
                    button.classList.add('btn');
                    button.value = m.stockId;
                    button.innerText = 'Wybierz';
                    button.onclick = (event) => {
						let id = event.target.value;
						let orderedMeds = context.filter(m=>m.stockId==id);
						if(orderedMeds.length !== 0) {
							orderedMeds[0].quantity++;
						}
						else {
                            let filteredMed = acquiredMedicines.filter(m => m.stockId == id)[0];
							let copy = Object.assign({}, filteredMed);
							copy.quantity = 1;

							context.push(copy)
						}
                        renderContext();
                    }
                    td5.appendChild(button);
                    medicineSearchTable.appendChild(tr);
                })
            })
    }



	updatePrescription = (button) => {
		
		button.disabled = true;
        var url = '/Doctor/EditMedicines?prescriptionId='+button.value;
        var data = JSON.stringify(context);
		console.log(data);
        fetch(url, {
            method: 'POST', 
            body: data,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
			.then(data=>{
				console.log('Success:',data)
				if(data.succes){

					alert("Dodano recepte!");
					button.disabled = false;
				}
				else {
					alert("Błąd przy dodawaniu recepty!\n"+data.message);
					button.disabled = false;
				}
			})
            .catch(error => {
				console.error('Error:', error)
				alert("Błąd przy dodawaniu recepty!");
				button.disabled = false;
			});
	}

	medicineSearchButton.onclick = (event) => {
        let search = medicineSearchInput.value;
        medicineSearch(search);
	}
	
	
	saveButton.onclick = ({ target }) => {
		updatePrescription(target);
	}



}
initialize();