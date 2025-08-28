// Fetch data from an API and display it
fetch('https://jsonplaceholder.typicode.com/users')
    .then(response => response.json())
    .then(data => {
    const container = document.getElementById('users-container');
    data.forEach(user => {
        const userElement = document.createElement('p');
        userElement.textContent = `${user.name} - ${user.email}`;
        container.appendChild(userElement);
    });
    })
    .catch(error => console.error('Error fetching data:', error));
// Parse a JSON string
const jsonString = '[{"name": "Alice", "age": 25}, {"name": "Bob", "age": 30}]';
const users = JSON.parse(jsonString);
console.log(users[0].name);  // Output: Alice
console.log(users[1].age);   // Output: 30
// Convert a JavaScript object to JSON
const user = {
    name: "Charlie",
    age: 28,
    isActive: true
};
const jsonUserString = JSON.stringify(user);
console.log(jsonUserString); // Output: {"name":"Charlie","age":28,"isActive":true}
// Store and retrieve data using localStorage
const settings = {
    theme: "dark",
    language: "en"
};
localStorage.setItem('userSettings', JSON.stringify(settings));
const storedSettings = JSON.parse(localStorage.getItem('userSettings'));
console.log(storedSettings.theme); // Output: dark
console.log(storedSettings.language); // Output: en