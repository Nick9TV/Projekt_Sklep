document.getElementById('registerForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const formData = new FormData(event.target);
    function isPasswordValid(password) {

        const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$/;
        return passwordRegex.test(password);
    }
    if (formData.get('password') !== formData.get('confirmPassword')) {

        const successMessage = document.createElement('div');
        successMessage.innerHTML = 'Passwords do not match!';
        successMessage.style.color = 'white';
        successMessage.style.fontSize = '14px';
        successMessage.style.fontFamily = 'Montserrat';

        setTimeout(() => {
            successMessage.style.display = 'none';
        }, 4000);

        return;
    }
    if (!isPasswordValid(formData.get('password'))) {

        const successMessage = document.createElement('div');
        successMessage.innerHTML = 'Password must be at least 8 characters long and contain at least one uppercase letter, one special character, and one digit!';
        successMessage.style.color = 'white';
        successMessage.style.fontSize = '14px';
        successMessage.style.fontFamily = 'Montserrat';


        setTimeout(() => {
            successMessage.style.display = 'none';
        }, 4000);

        return;
    }

    fetch('../api/User/Register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            name: formData.get('Name'),
            surname: formData.get('Surname'),
            phone: formData.get('Phone'),
            email: formData.get('Email'),
            password: formData.get('password'),
            confirmPassword: formData.get('confirmPassword'),
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
            console.log('Registration successful:', data);

            const successMessage = document.createElement('div');


            setTimeout(() => {
                successMessage.style.display = 'none';
                window.location.href = 'logowanie.html';
            }, 4000);
        })
        .catch(error => {
            console.error('Registration failed:', error.message);
        });
});


