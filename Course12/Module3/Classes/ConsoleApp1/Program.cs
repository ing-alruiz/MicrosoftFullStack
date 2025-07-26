using System;

class Program
{
    public class Person
    {
        public string Name { get; }
        public int Age { get; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return $"{Name}, {Age} years old";
        }

        public string ToJson()
        {
            return $"{{ \"Name\": \"{Name}\", \"Age\": {Age} }}";
        }
    }

    static void Main(string[] args)
    {
        Person person = new Person("Alice", 30);
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");
        Console.WriteLine(person.ToString());
        Console.WriteLine(person.ToJson());
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
