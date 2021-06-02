var ok=false;
$('#LogIn_button').click(function () {
    const url = 'http://localhost:21474/api/users';
    var password = document.getElementById("LoginPassword");
    var username = document.getElementById("LoginUsername");
    var loadList = function () {
        jQuery.ajaxSetup({ async: false });
        $.get(url, function (data) {
            var loadData = function () {
                $(data).each(function (index, item) {
                    if (item.userName == username.value && item.password == password.value) {
                        ok = true;
                        return true;
                    }
                });
                return ok;
            }
            Date.prototype.addHours = function (h) {
                this.setHours(this.getHours() + h);
                return this;
            }
            return loadData();
        });
    }
    async function asyncCall() {
        const result = await loadList();
        return result;
    }
    if (asyncCall()) {
        if (ok == false && username.value != "" && password.value != "") {
            username.value = "";
            password.value = "";
            alert("User name or password is not correct");
            return ok;
        }
        else {
            return true;
        }
    }
});