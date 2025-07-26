using System;
using System.Collections.Generic;

class LibraryManagement
{
    // List to store up to 5 book titles
    static List<string> books = new List<string>();
    static readonly int MaxBooks = 5;

    static void Main(string[] args)
    {
        Console.WriteLine("=== Welcome to the Simple Library Management System ===\n");
        Console.WriteLine("You can manage up to 5 books in this library.\n");

        // Main program loop
        while (true)
        {
            DisplayMenu();
            string choice = GetUserChoice();

            switch (choice.ToLower())
            {
                case "1":
                case "add":
                    AddBook();
                    break;
                case "2":
                case "remove":
                    RemoveBook();
                    break;
                case "3":
                case "display":
                    DisplayBooks();
                    break;
                case "4":
                case "exit":
                    Console.WriteLine("Thank you for using the Library Management System. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.\n");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("=== Library Management System ===");
        Console.WriteLine("1. Add a book");
        Console.WriteLine("2. Remove a book");
        Console.WriteLine("3. Display all books");
        Console.WriteLine("4. Exit");
        Console.Write("\nEnter your choice (1-4): ");
    }

    static string GetUserChoice()
    {
        return Console.ReadLine()?.Trim() ?? "";
    }

    static void AddBook()
    {
        Console.WriteLine("\n--- Add a Book ---");
        
        // Check if library is full
        if (books.Count >= MaxBooks)
        {
            Console.WriteLine("Sorry, the library is full! No more books can be added.");
            Console.WriteLine("You need to remove a book first to make space.");
            return;
        }

        Console.Write("Enter the book title to add: ");
        string bookTitle = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(bookTitle))
        {
            Console.WriteLine("Invalid input. Book title cannot be empty.");
            return;
        }

        // Check if book already exists
        if (books.Exists(book => book.Equals(bookTitle, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine($"The book '{bookTitle}' already exists in the library.");
            return;
        }

        // Add the book to the list
        books.Add(bookTitle);
        Console.WriteLine($"Book '{bookTitle}' has been added to slot {books.Count}.");
    }

    static void RemoveBook()
    {
        Console.WriteLine("\n--- Remove a Book ---");
        
        // Check if library is empty
        if (books.Count == 0)
        {
            Console.WriteLine("The library is empty. There are no books to remove.");
            return;
        }

        Console.WriteLine("Current books in the library:");
        DisplayBooks();

        Console.Write("\nEnter the exact title of the book to remove: ");
        string bookToRemove = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(bookToRemove))
        {
            Console.WriteLine("Invalid input. Book title cannot be empty.");
            return;
        }

        // Find and remove the book
        int bookIndex = books.FindIndex(book => book.Equals(bookToRemove, StringComparison.OrdinalIgnoreCase));
        
        if (bookIndex >= 0)
        {
            books.RemoveAt(bookIndex);
            Console.WriteLine($"Book '{bookToRemove}' has been removed from the library.");
        }
        else
        {
            Console.WriteLine($"Book '{bookToRemove}' was not found in the library.");
            Console.WriteLine("Please check the spelling and try again.");
        }
    }

    static void DisplayBooks()
    {
        Console.WriteLine("\n--- Current Books in Library ---");
        
        if (books.Count == 0)
        {
            Console.WriteLine("The library is currently empty.");
        }
        else
        {
            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {books[i]}");
            }
            
            Console.WriteLine($"\nTotal books: {books.Count}/{MaxBooks}");
            Console.WriteLine($"Available slots: {MaxBooks - books.Count}");
        }
    }

    static bool IsLibraryFull()
    {
        return books.Count >= MaxBooks;
    }

    static bool IsLibraryEmpty()
    {
        return books.Count == 0;
    }

    static bool BookExists(string title)
    {
        return books.Exists(book => book.Equals(title, StringComparison.OrdinalIgnoreCase));
    }
}
          