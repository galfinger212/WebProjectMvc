$(function () {
	$('#Signup_button').click(function () {
		window.location.href = "http://localhost:38564/User/Signup";
	});
});
function OnLoad(title) {
	$(`#${title.replaceAll(" ","")}`).addClass("disabled");
}
