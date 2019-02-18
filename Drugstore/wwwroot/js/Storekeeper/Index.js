
function manage() {

    const uploadForm = document.querySelector(".storekeeper-panel .update-store-panel form");
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

manage();