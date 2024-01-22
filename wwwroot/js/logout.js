document.addEventListener('DOMContentLoaded', function () {
    checkLoginStatus();
});

function checkLoginStatus() {
    const jwtToken = localStorage.getItem('jwtToken');


    if (!jwtToken) {
        window.location.href = '../../index.html';
    }
}

document.getElementById('logoutButton').addEventListener('click', function (event) {
    event.preventDefault();


    localStorage.removeItem('jwtToken');


    window.location.href = '../../index.html';
});
