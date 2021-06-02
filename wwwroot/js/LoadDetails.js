$(function () {
	const url = 'http://localhost:21474/api/users';
	function getList() {
		var cookie = getCookie("id");
		var currentUrl = url + "/" + cookie;
		$.get(currentUrl, function (data) {
			$(data).each(function (index, item) {
				$('#userNamesign').val(item.userName);
				$('#btnSub').text('Update User');
				$('#subForm').prop("action", "http://localhost:38564/User/Update_Submit");
				$('#userNamesign').prop("disabled", true);
				$('#password').val(item.password);
				$('#confirm_password').val(item.password);
				$('#firstNamesign').val(item.firstName);
				$('#lastName').val(item.lastName);
				$('#email').val(item.email);
				$('#IdUserUpdate').val(item.id);
				let date = new Date(item.birthDate);
				date = new Date(date.getTime() - date.getTimezoneOffset() * 60000)
				date = date.toISOString().substring(0,10);
				$('#BirthDate').val(date);
			});
		});
	}
	getList();
});
function getCookie(name) {
	const value = `; ${document.cookie}`;
	const parts = value.split(`; ${name}=`);
	if (parts.length === 2) return parts.pop().split(';').shift();
}

//load all the details of the user on the updateUser page 