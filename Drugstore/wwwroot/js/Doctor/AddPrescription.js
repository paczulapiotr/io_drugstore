
function prescription() {

    // Patients elements
    const patientPanel = document.querySelector('.newPrescriptionPanel .patientPanel');
    const patientInput = patientPanel.querySelector('.patientSearch input');
    const patientButton = patientPanel.querySelector('.patientSearch button');
    const patientTable = patientPanel.querySelector('table tbody');

    // Medicine elements
    const medicinePanel = document.querySelector('.newPrescriptionPanel .medicinePanel');
    const medicineInput = medicinePanel.querySelector('.medicineSearch input');
    const medicineButton = medicinePanel.querySelector('.medicineSearch button');
    const medicineTable = medicinePanel.querySelector('table tbody');

    // Prescription elemets
    const prescriptionPanel = document.querySelector('.newPrescriptionPanel .prescriptionPanel');
    const prescriptionPatient = prescriptionPanel.querySelector('h5 span');
	const prescriptionTable = prescriptionPanel.querySelector('table tbody');
	const prescriptionSaveButton = prescriptionPanel.querySelector('.create');

    let context = {
        patient: {
            id: 0,
            name: ""
        },
        medicines: []
	}
	
    let acquiredMedicines = [];

    let acquiredPatients = [];


    medicine = (search) => {
        fetch('/Doctor/FindMedicine?search=' + search)
            .then(response => response.json())
            .then(data => {

                acquiredMedicines = data;

                while (medicineTable.firstChild) {
                    medicineTable.removeChild(medicineTable.firstChild);
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
                    td2.innerText = m.pricePerOne
                    tr.appendChild(td2);
                    td3.innerText = m.isRefunded;
                    tr.appendChild(td3);
                    td4.innerText = m.quantity
                    tr.appendChild(td4);
                    tr.appendChild(td5);

                    let button = document.createElement('button');
                    button.classList.add('btn');
                    button.value = m.id;
                    button.innerText = 'Wybierz';
                    button.onclick = (event) => {
						let id = event.target.value;
						let orderedMeds = context.medicines.filter(m=>m.stockMedicine.id==id);
						if(orderedMeds.length !== 0) {
							orderedMeds[0].assignedQuantity++;
						}
						else {
							let filteredMed = acquiredMedicines.filter(m => m.id == id)[0];
							let copy = Object.assign({}, filteredMed);
							context.medicines.push({
								stockMedicine:copy,
								assignedQuantity: 1
							})
						}
                        console.log(context);
                        updatePrescription();
                    }
                    td5.appendChild(button);

                    medicineTable.appendChild(tr);
                })
            })
    }

    patients = (search) => {
        fetch('/Doctor/FindPatient?search=' + search)
            .then(response => response.json())
            .then(data => {

                acquiredPatients = data;
                while (patientTable.firstChild) {
                    patientTable.removeChild(patientTable.firstChild);
                }
                data.forEach(p => {
                    console.log(p);
                    let tr = document.createElement('tr');

                    let td1 = document.createElement('td');
                    td1.innerText = p.firstName + ' ' + p.secondName;

                    let td2 = document.createElement('td');

                    let button = document.createElement('button');
                    button.classList.add('btn');
                    button.innerText = 'Wybierz';
                    button.value = p.id;
                    button.onclick = (event) => {
                        let id = event.target.value;
                        let filteredPatient = acquiredPatients.filter(p => p.id == id)[0];
                        let copy = Object.assign({}, filteredPatient);
                        context.patient = copy;
                        console.log(context);
                        updatePrescription();
                    };
                    td2.appendChild(button);

                    tr.appendChild(td1);
                    tr.appendChild(td2);

                    patientTable.appendChild(tr);
                });

            })
            .catch(error => console.info('error', error));


    }

    updatePrescription = () => {
        while (prescriptionTable.firstChild) {
            prescriptionTable.removeChild(prescriptionTable.firstChild);
        }
        let { firstName, secondName } = context.patient;
        prescriptionPatient.innerText = (firstName||"") + " " + (secondName||"");

        context.medicines.forEach(m => {
            let tr = document.createElement('tr');
            let td1 = document.createElement('td');
            let td2 = document.createElement('td');
            let td3 = document.createElement('td');
            let td4 = document.createElement('td');
            td1.innerText = m.stockMedicine.name;
            tr.appendChild(td1);
            td2.innerText = m.stockMedicine.pricePerOne
            tr.appendChild(td2);
            td3.innerText = m.assignedQuantity;
            tr.appendChild(td3);
            tr.appendChild(td4);


            let button = document.createElement('button');
            button.classList.add('btn');
            button.value = m.stockMedicine.id;
            button.innerText = 'Usuń';
            button.onclick = (event) => {
                let id = event.target.value;
                context.medicines = context.medicines.filter(m => m.stockMedicine.id != id);
                console.log(context);
                updatePrescription();
            }
            td4.appendChild(button);

            prescriptionTable.appendChild(tr);
        })



    }


    savePrescription = (button) => {

		button.disabled = true;
        var url = '/Doctor/AddPrescription';
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
				if(data.valid){

					alert("Dodano recepte!");
					context = {
						patient: {
							id: 0,
							name: ""
						},
						medicines: []
					}
					updatePrescription();
					button.disabled = false;
				}
				else {
					alert("Błąd przy dodawaniu recepty!");
					button.disabled = false;
				}

			})
            .catch(error => {
				console.error('Error:', error)
				alert("Błąd przy dodawaniu recepty!");
				button.disabled = false;
			});
    }

    patientButton.onclick = (event) => {
        const search = patientInput.value;
        patients(search);
	}
	
    medicineButton.onclick = (event) => {
        let search = medicineInput.value;
        medicine(search);
    }

	prescriptionSaveButton.onclick = (event) => {
		savePrescription(event.target);
	}
};
prescription();