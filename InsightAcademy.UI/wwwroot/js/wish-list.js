
"use strict"
function addToWishList(tutorId) {
    $.ajax({
        url: '/tutor/addwishlist',
        type: 'POST',
        data: { tutorId: tutorId },
        contentype:"json",
        success: function (response) {
            if (response == true) {
                alert("added to wishlist")
            }

        },
        error: function (xhr, status, error) {
            console.error("Error uploading image: " + error);
        }
    });
}
