var btn = document.getElementById("LogOut_button");
function LogOutFunc() {
    var answer = confirm("Are you sure you want to log out?");
    if (answer == true) {
        document.cookie = "UserName=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        document.cookie = `id=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
        if (window.location.pathname == "/Home/PostAdd") {
            window.location.href = "http://localhost:38564/Home";
        }
        else {
            window.location.reload();
        }
    }
}
if (btn) {
    btn.onclick = LogOutFunc;
}
//delete the user name and id from the cookies