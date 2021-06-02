$(function () {
	const url = 'http://localhost:21474/api/users';
	const value = `; ${document.cookie}`;
	const parts = value.split(`; id=`);
	if (parts.length === 2) {
		$('#Id').val(parts.pop().split(';').shift());
		return;
	}
	else {
		var userName = parts.pop().split('; UserName=')[1].replace("%20"," ");
		$.get(url, function (data) {
			$(data).each(function (index, item) {
				if (item.userName == userName) {
					document.cookie = `id=${item.id}; expires=${new Date().addHours(-1)};`;
					$('#Id').val(item.id);
                }
			});
			
		});
		Date.prototype.addHours = function (h) {
			this.setHours(this.getHours() + h);
			return this;
		}
    }
});
//get the id of that belong the current user name