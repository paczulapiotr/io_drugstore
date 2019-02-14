
function prescription() {

    // Patients elements
    const patientPanel = document.querySelector('.new-prescription-panel .patient-panel');
    const patientInput = patientPanel.querySelector('.patient-search input');
    const patientButton = patientPanel.querySelector('.patient-search button');
    const patientTable = patientPanel.querySelector('table tbody');

    // Medicine elements
    const medicinePanel = document.querySelector('.new-prescription-panel .medicine-panel');
    const medicineInput = medicinePanel.querySelector('.medicine-search input');
    const medicineButton = medicinePanel.querySelector('.medicine-search button');
    const medicineTable = medicinePanel.querySelector('table tbody');

    // Prescription elemets
    const prescriptionPanel = document.querySelector('.new-prescription-panel .prescription-panel');
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
                    td2.innerText = m.pricePerOne.toFixed(2)
                    tr.appendChild(td2);
                    td3.innerText = (m.refundation*100).toFixed(2);
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
						console.log('searched id',id)
						let orderedMeds = context.medicines.filter(m=>m.stockId==id);
						if(orderedMeds.length !== 0) {
							console.log('med before',orderedMeds[0])
							orderedMeds[0].quantity++;
							console.log('med after',orderedMeds[0])
						}
						else {
                            let filteredMed = acquiredMedicines.filter(m => m.stockId == id)[0];
							let copy = Object.assign({}, filteredMed);
							copy.quantity = 1;

							context.medicines.push(copy)
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
                    td1.innerText = p.fullName;

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
        let { fullName } = context.patient;
        prescriptionPatient.innerText = fullName||"";

        context.medicines.forEach(m => {
            let tr = document.createElement('tr');
            let td1 = document.createElement('td');
            let td2 = document.createElement('td');
            let td3 = document.createElement('td');
			let td4 = document.createElement('td');

			td1.innerText = m.name;
            tr.appendChild(td1);
            td2.innerText = m.pricePerOne.toFixed(2);
            tr.appendChild(td2);
            td3.innerText = m.quantity;
            tr.appendChild(td3);
            tr.appendChild(td4);


            let button = document.createElement('button');
            button.classList.add('btn');
            button.value = m.stockId;
            button.innerText = 'Usuń';
            button.onclick = (event) => {
                let id = event.target.value;
                context.medicines = context.medicines.filter(m => m.stockId != id);
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
				if(data.succes){

					alert("Dodano receptę!");
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
					alert("Błąd przy dodawaniu recepty! "+data.message);
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

	prescriptionSaveButton.onclick = ({target}) => {
		savePrescription(target);
	}
};
prescription();