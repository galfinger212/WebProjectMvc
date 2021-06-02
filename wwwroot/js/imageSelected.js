$(function () {
    $('#liImage1').click(function () {
        $(".image_selected").html("");
        $(".image_selected").append($('#image1').clone());
    });
    $('#liImage2').click(function () {
        $(".image_selected").html("");
        $(".image_selected").append($('#image2').clone());
    });
    $('#liImage3').click(function () {
        $(".image_selected").html("");
        $(".image_selected").append($('#image3').clone());
    });
});
//changed the selected image in the details page