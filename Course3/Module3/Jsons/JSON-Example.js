const jsonString = '{"name": "Alice", "age": 25, "isStudent": false}';
let jsonObject = JSON.parse(jsonString);
console.log(jsonObject);
console.log(jsonObject.name); // "Alice"
console.log(jsonObject.age); // 25


const student = {
  name: "Bob",
  age: 30,
  isEnrolled: true,
  courses: ["Math", "Science", "Art"]
};
const jsonString2 = JSON.stringify(student);
console.log(jsonString2);