$(function () {
	const url = 'http://localhost:21474/api/users';

	function getUser() {
		const div = $('#userDiv');
		div.html('');
		id =  $('#Id').val();
		let url2 = url + "/" + id;
		$.get(url2, function (data) {
			console.log(data);
			div.append(`The publisher ${data.firstName} ${data.lastName} was born in ${(new Date(data.birthDate)).toISOString().substring(0, 10)}, Email: ${data.email} `);
		});
	}
	getUser();
});
//pull the user details from the database