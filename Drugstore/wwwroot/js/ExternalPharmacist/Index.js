
function initialize() {

    const uploadForm = document.querySelector(".external-pharm form");
    const uploadInput = uploadForm.querySelector("input");
    const uploadButton = uploadForm.querySelector("#upload-button");
    uploadButton.disabled = true;

    uploadInput.onchange = function (event) {
        if (uploadInput.value == '') {
            uploadButton.disabled = true;
        }
        else {
            uploadButton.disabled = false;
        }
    }
}

initialize();