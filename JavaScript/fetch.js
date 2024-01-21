await fetch('https://reqres.in/api/user')
    .then(res => {
        return res.json());
    };
fetch('https://localhost:7157/api/user')
    .then(response => response.json())
    .then(data => {
        console.log(data);
        // Dodatkowe operacje na danych
    })
    .catch(error => console.error('Error:', error));
