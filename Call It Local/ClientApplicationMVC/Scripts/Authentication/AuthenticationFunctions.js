
function validateNewAccount() {
    var phoneReg = /\d\d\d\d\d\d\d\d\d\d/;
    var emailReg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (!signupForm.username.value)
        return false;
    else if (!signupForm.email.value || !emailReg.test(String(signupForm.email.value).toLowerCase()))
        return false;
    else if (!signupForm.password.value || signupForm.password.value.toString().length < 6)
        return false;
    else if (!signupForm.address.value)
        return false;
    else if (!signupForm.phone.value || !phoneReg.test(signupForm.phone.value))
        return false;
    else if (!signupForm.type.value)
        return false;
    else
        return true;
}