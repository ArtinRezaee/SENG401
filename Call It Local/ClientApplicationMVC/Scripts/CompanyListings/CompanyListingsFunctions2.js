function validateReviewForm() {
    if (reviewForm.comment.value && reviewForm.name.value) {
        return true;
    } else {
        $('.error').html("Invalid input. Please verify your input and try again.");
        return false;
    }
};


function validateSearchForm() {
    if (searchForm.textCompanyName.value)
        return true;
    else {
        $('.error').html("Invalid input. Please verify your input and try again.");
        return false;
    }
}