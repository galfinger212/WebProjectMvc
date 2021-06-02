$(function () {
    $('#OwnerId').val($('#Id').val());
    $('#btnSubAdd').click(function () {
        var ok = true;
        var title = $("#title").val()
        if (title == null || title?.trimEnd().trimStart() == "") {
            alert("Title must not be empty or in spaces!!");
            ok = false;
        }
        var ShortDescription = $("#ShortDescription").val()
        if (ShortDescription == null || ShortDescription?.trimEnd().trimStart() == "") {
            alert("Short Description must not be empty or in spaces!!");
            ok = false;
        }
        var LongDescription = $("#LongDescription").val()
        if (LongDescription == null || LongDescription?.trimEnd().trimStart() == "") {
            alert("Long Description must not be empty or in spaces!!");
            ok = false;
        }
        if (ok) return true;
        else return false;
    });
    
});
function getCookie(name) {
	const value = `; ${document.cookie}`;
	const parts = value.split(`; ${name}=`);
	if (parts.length === 2) return parts.pop().split(';').shift();
}

//fill the input hidden with the current owner id
//client validation