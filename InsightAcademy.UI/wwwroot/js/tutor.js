"use strict"
function searchTutor() {
    let subject = $('#selectv8').val();
    window.location.href = '/tutor/list?SubjectId=' + subject
}
function filter() {
    showLoader();
    var urlParams = new URLSearchParams(window.location.search);
    var subjectValue = urlParams.get('subject');
   

    let location = $('#location').val();
    let order = $('#order').val();
    let minPrice = $('#tu-min-value').val();
    let maxPrice = $('#tu-max-value').val();


    let service;
    if ($('#service').is(':checked')) {
        service = $('#service').val();
    }
    var selectedSubjects = $('.subject:checked').map(function () {
        return $(this).val();
    }).get();

    console.log(selectedSubjects); 
   
    //let price = $('#price').val();
    $.ajax({
        url: '/tutor/Filter',
        type: 'POST',
        data: { Order: order, Location: location, SubjectId: subjectValue, MinPrice: minPrice, MaxPrice: maxPrice, service: service, SubjectIds: selectedSubjects },
        contentype: "json",
        dataType: "html",
        success: function (response) {
            $('#tutor').empty();
            $('#tutor').html(response);
        },
        error: function (xhr, status, error) {
            console.error("Error uploading image: " + error);
        }, complete: function () {
            hideLoader();
        }
    });
}
function gridView() {
    
    $.ajax({
        url: '/tutor/TutorGridView',
        type: 'GET',
        contentype: "json",
        dataType: "html",
        success: function (response) {
            console.log(response);
            $('#list-view').remove();
            $('#grid').html(response);
        },
        error: function (xhr, status, error) {
            console.error("Error uploading image: " + error);
        }, complete: function () {
            hideLoader();
        }
    });
}


