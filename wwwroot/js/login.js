function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)).join(''));
        return JSON.parse(jsonPayload);
    } catch (error) {
        console.error('Błąd podczas parsowania tokena JWT:', error);
        return {};
    }
}
document.getElementById('loginForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const formData = new FormData(event.target);

    fetch('../api/user/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',

        },
        body: JSON.stringify({
            email: formData.get('email'),
            password: formData.get('password'),
        }),
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(errorData => {
                    const errorMessage = document.createElement('div');

                    setTimeout(() => {
                        errorMessage.style.display = 'none';
                    }, 4000);

                    throw new Error(`HTTP error! Status: ${response.status}, Error: ${errorData}`);
                });
            }
            return response.text();
        })
        .then(data => {
            console.log('Login successful:', data);

            localStorage.setItem('jwtToken', data);

            const parsedJwt = parseJwt(data);
            console.log(parsedJwt);

            const userRole = parsedJwt['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

            console.log(userRole);
            setTimeout(() => {

                // Check user's role and redirect accordingly
                if (userRole === 'Admin') {
                    window.location.href = 'Admin/adminindex.html';
                }
                if (userRole === 'User') {
                    window.location.href = 'User/userindex.html';
                }
            }, 4000);
        })
        .catch(error => {

            console.error('Login failed:', error.message);
        });


});