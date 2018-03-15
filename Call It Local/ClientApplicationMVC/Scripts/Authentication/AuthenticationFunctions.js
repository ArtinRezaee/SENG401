function validateNewAccount() {
    var success = false;
    var phoneReg = /\d\d\d\d\d\d\d\d\d\d/;
    var emailReg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (!signupForm.username.value)
        success = false;
    else if (!signupForm.email.value || !emailReg.test(String(signupForm.email.value).toLowerCase()))
        success = false;
    else if (!signupForm.password.value || signupForm.password.value.toString().length < 6)
        success = false;
    else if (!signupForm.address.value)
        success = false;
    else if (!signupForm.phone.value || !phoneReg.test(signupForm.phone.value))
        success = false;
    else if (!signupForm.type.value)
        success = false;
    else
        success = true;

    if (!success) {
        $('.error').html("Invalid input. Please verify your input and try again.");
    }

    return success;
}

var validateLoginForm = function(){
    if ($('#username').val() && $('#password').val()) {
        return true;
    } else {
        $('.error').html("Invalid input. Please verify your input and try again.");
        return false;
    }
};