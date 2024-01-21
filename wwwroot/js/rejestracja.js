document.addEventListener("DOMContentLoaded", function () {
    const registrationForm = document.getElementById('registrationForm');

    registrationForm.addEventListener('submit', function (event) {
        event.preventDefault();

        const formData = new FormData(registrationForm);
        const userData = {
            Name: formData.get('Name'),
            Surname: formData.get('Surname'),
            Phone: formData.get('Phone'),
            Email: formData.get('Email'),
            password: formData.get('password'),
            passwordHash: formData.get('passwordHash'),
            // Dodaj pozostałe pola zgodnie z Twoim formularzem i modelem użytkownika
        };

        fetch('https://localhost:7157/api/User/Register', { // Zmień URL na odpowiedni endpoint
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userData),
        })
            .then(response => response.json())
            .then(data => {
                console.log('Success:', data);
                // Obsługa pozytywnej odpowiedzi, np. przekierowanie lub wyświetlenie komunikatu
            })
            .catch((error) => {
                console.error('Error:', error);
                // Obsługa błędów, np. wyświetlenie komunikatu o błędzie
            });
    });
});
