document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.getElementById('loginForm');

    loginForm.addEventListener('submit', function (event) {
        event.preventDefault();

        const formData = new FormData(loginForm);
        const loginData = {
            email: formData.get('email'),
            password: formData.get('password')
        };

        fetch('https://localhost:7157/api/User/Login', { // Zmień URL na odpowiedni endpoint
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(loginData),
        })
            .then(response => response.json())
            .then(data => {
                console.log('Login successful:', data);
                // Obsługa pozytywnej odpowiedzi, np. zapisanie tokena sesji, przekierowanie
            })
            .catch((error) => {
                console.error('Login error:', error);
                // Obsługa błędów, np. wyświetlenie komunikatu o błędzie logowania
            });
    });
});

