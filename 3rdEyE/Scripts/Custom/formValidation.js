
var formValidity = true;
var validatorArray = [];

function TrySubmit() {
    formValidity = true;
    $('b[data-category=errorMessage]').html("");
    for (var i = 0; i < validatorArray.length; i++) {
        validatorArray[i]();
    }
    if (formValidity) {
        $('form[name=ThisForm]').submit();
    } else {
        console.log("form validation failed");
    }
}

