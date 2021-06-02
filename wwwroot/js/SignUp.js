$('#btnSub').click(function () {
    var ok = true;
    const url = 'http://localhost:21474/api/users';
    var password = document.getElementById("password");
    var confirm_password = document.getElementById("confirm_password");
    if (password.value != confirm_password.value) {
        confirm_password.value = "";
        alert("Passwords Don't Match");
        ok = false;
    } else {
        confirm_password.setCustomValidity('');
    }
    var date = new Date($('#BirthDate').val());
    var dateNow = new Date();

    if (date.getTime() >=dateNow.getTime()) {
        alert("Date is not valid!");
        ok = false;
        $('#BirthDate').val(null);
    }
    var firstname = $("#firstNamesign").val()
    if (firstname == null || firstname?.trimEnd().trimStart() == "") {
        alert("Firstname must not be empty or in spaces!!");
        ok = false;
    }
    var lastName = $("#lastName").val()
    if (lastName == null || lastName?.trimEnd().trimStart() == "") {
        alert("LastName must not be empty or in spaces!!");
        ok = false;
    }
    var userNamesign = $("#userNamesign").val()
    if (userNamesign == null || userNamesign?.trimEnd().trimStart() == "") {
        alert("UserName must not be empty or in spaces!!");
        ok = false;
    }
    //signup
    var id = $('#IdUserUpdate').val();
    if (id == "") {
        var loadList = function () {
            jQuery.ajaxSetup({ async: false });
            $.get(url, function (data) {
                var loadData = function () {
                    $(data).each(function (index, item) {
                        var username = document.getElementById("userNamesign");
                        var name = username.value;
                        if (item.userName == name) {
                            username.value = "";
                            alert("There is already a user name with this name!");
                            ok = false;
                            return false;
                        }
                    });
                    return ok;
                }
                return loadData();
            });
        }
        async function asyncCall() {
            const result = await loadList();
            return result;
        }
        if (asyncCall()) {
            return ok;
        }
    }
    else {
        return ok;
    }
});