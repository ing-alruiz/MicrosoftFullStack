using System;
using System.Collections.Generic;
using System.Linq;

class Book
{
    public string Title { get; set; }
    public bool IsCheckedOut { get; set; }
    public string BorrowedBy { get; set; }
    public DateTime CheckoutDate { get; set; }

    public Book(string title)
    {
        Title = title;
        IsCheckedOut = false;
        BorrowedBy = "";
        CheckoutDate = DateTime.MinValue;
    }
}

class LibraryManagement
{
    // List to store books
    static List<Book> books = new List<Book>();
    static readonly int MaxBooksPerUser = 3;

    static void Main(string[] args)
    {
        Console.WriteLine("=== Welcome to the Simple Library Management System ===\n");
        Console.WriteLine("This library can store unlimited books.\n");
        Console.WriteLine($"Each user can borrow up to {MaxBooksPerUser} books at a time.\n");

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
                case "search":
                    SearchBook();
                    break;
                case "5":
                case "checkout":
                    CheckoutBook();
                    break;
                case "6":
                case "return":
                    ReturnBook();
                    break;
                case "7":
                case "status":
                    ShowBorrowingStatus();
                    break;
                case "8":
                case "exit":
                    Console.WriteLine("Thank you for using the Library Management System. Goodbye!");
                    return;
                case "9":
                case "toggle":
                    ToggleBookStatus();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1-9.\n");
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
        Console.WriteLine("4. Search for a book");
        Console.WriteLine("5. Checkout a book");
        Console.WriteLine("6. Return a book");
        Console.WriteLine("7. Show borrowing status");
        Console.WriteLine("8. Exit");
        Console.WriteLine("9. Toggle book checkout status");
        Console.Write("\nEnter your choice (1-9): ");
    }

    static string GetUserChoice()
    {
        return Console.ReadLine()?.Trim() ?? "";
    }

    static void AddBook()
    {
        Console.WriteLine("\n--- Add a Book ---");

        Console.Write("Enter the book title to add: ");
        string bookTitle = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(bookTitle))
        {
            Console.WriteLine("Invalid input. Book title cannot be empty.");
            return;
        }

        // Check if book already exists
        if (BookExists(bookTitle))
        {
            Console.WriteLine($"The book '{bookTitle}' already exists in the library.");
            return;
        }

        // Add the book to the list
        books.Add(new Book(bookTitle));
        Console.WriteLine($"Book '{bookTitle}' has been added to slot {books.Count}.");
    }

    static void RemoveBook()
    {
        Console.WriteLine("\n--- Remove a Book ---");
        
        // Check if library is empty
        if (IsLibraryEmpty())
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
        int bookIndex = books.FindIndex(book => book.Title.Equals(bookToRemove, StringComparison.OrdinalIgnoreCase));
        
        if (bookIndex >= 0)
        {
            Book book = books[bookIndex];
            if (book.IsCheckedOut)
            {
                Console.WriteLine($"Cannot remove '{bookToRemove}' - it is currently checked out by {book.BorrowedBy}.");
                return;
            }
            
            books.RemoveAt(bookIndex);
            Console.WriteLine($"Book '{bookToRemove}' has been removed from the library.");
        }
        else
        {
            Console.WriteLine($"Book '{bookToRemove}' was not found in the library.");
            Console.WriteLine("Please check the spelling and try again.");
        }
    }

    static void SearchBook()
    {
        Console.WriteLine("\n--- Search for a Book ---");
        
        // Check if library is empty
        if (IsLibraryEmpty())
        {
            Console.WriteLine("The library is empty. There are no books to search.");
            return;
        }

        Console.Write("Enter book title or partial title to search: ");
        string searchTerm = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(searchTerm))
        {
            Console.WriteLine("Invalid input. Search term cannot be empty.");
            return;
        }

        // Find books that contain the search term
        List<Book> matchingBooks = books.FindAll(book => 
            book.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);

        if (matchingBooks.Count == 0)
        {
            Console.WriteLine($"No books found containing '{searchTerm}'.");
        }
        else
        {
            Console.WriteLine($"\nFound {matchingBooks.Count} book(s) matching '{searchTerm}':");
            for (int i = 0; i < matchingBooks.Count; i++)
            {
                int originalIndex = books.FindIndex(book => book.Title.Equals(matchingBooks[i].Title, StringComparison.OrdinalIgnoreCase));
                string status = matchingBooks[i].IsCheckedOut ? $" (Checked out by {matchingBooks[i].BorrowedBy})" : " (Available)";
                Console.WriteLine($"{originalIndex + 1}. {matchingBooks[i].Title}{status}");
            }
        }
    }

    static void CheckoutBook()
    {
        Console.WriteLine("\n--- Checkout a Book ---");
        
        if (IsLibraryEmpty())
        {
            Console.WriteLine("The library is empty. There are no books to checkout.");
            return;
        }

        Console.Write("Enter your name: ");
        string userName = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Invalid input. Name cannot be empty.");
            return;
        }

        // Check borrowing limit
        int currentBorrowedCount = GetBorrowedBookCount(userName);
        if (currentBorrowedCount >= MaxBooksPerUser)
        {
            Console.WriteLine($"Sorry, {userName} has already borrowed the maximum number of books ({MaxBooksPerUser}).");
            Console.WriteLine("Please return a book before borrowing another one.");
            return;
        }

        Console.WriteLine("\nAvailable books:");
        DisplayAvailableBooks();

        Console.Write("\nEnter the title of the book to checkout: ");
        string bookTitle = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(bookTitle))
        {
            Console.WriteLine("Invalid input. Book title cannot be empty.");
            return;
        }

        Book book = books.Find(b => b.Title.Equals(bookTitle, StringComparison.OrdinalIgnoreCase));

        if (book == null)
        {
            Console.WriteLine($"Book '{bookTitle}' was not found in the library.");
            return;
        }

        if (book.IsCheckedOut)
        {
            Console.WriteLine($"Book '{bookTitle}' is already checked out by {book.BorrowedBy}.");
            return;
        }

        // Checkout the book
        book.IsCheckedOut = true;
        book.BorrowedBy = userName;
        book.CheckoutDate = DateTime.Now;

        Console.WriteLine($"Book '{bookTitle}' has been checked out to {userName}.");
        Console.WriteLine($"Books borrowed by {userName}: {currentBorrowedCount + 1}/{MaxBooksPerUser}");
    }

    static void ReturnBook()
    {
        Console.WriteLine("\n--- Return a Book ---");

        Console.Write("Enter your name: ");
        string userName = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("Invalid input. Name cannot be empty.");
            return;
        }

        List<Book> userBooks = books.FindAll(b => b.IsCheckedOut && b.BorrowedBy.Equals(userName, StringComparison.OrdinalIgnoreCase));

        if (userBooks.Count == 0)
        {
            Console.WriteLine($"{userName} has no books currently checked out.");
            return;
        }

        Console.WriteLine($"\nBooks checked out by {userName}:");
        for (int i = 0; i < userBooks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {userBooks[i].Title} (Checked out: {userBooks[i].CheckoutDate:MM/dd/yyyy})");
        }

        Console.Write("\nEnter the title of the book to return: ");
        string bookTitle = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(bookTitle))
        {
            Console.WriteLine("Invalid input. Book title cannot be empty.");
            return;
        }

        Book book = userBooks.Find(b => b.Title.Equals(bookTitle, StringComparison.OrdinalIgnoreCase));

        if (book == null)
        {
            Console.WriteLine($"You don't have '{bookTitle}' checked out.");
            return;
        }

        // Return the book
        book.IsCheckedOut = false;
        book.BorrowedBy = "";
        book.CheckoutDate = DateTime.MinValue;

        Console.WriteLine($"Book '{bookTitle}' has been returned by {userName}.");
        Console.WriteLine($"Books borrowed by {userName}: {GetBorrowedBookCount(userName)}/{MaxBooksPerUser}");
    }

    static void ShowBorrowingStatus()
    {
        Console.WriteLine("\n--- Borrowing Status ---");

        if (IsLibraryEmpty())
        {
            Console.WriteLine("The library is empty.");
            return;
        }

        var borrowers = books.Where(b => b.IsCheckedOut)
                            .GroupBy(b => b.BorrowedBy)
                            .ToList();

        if (borrowers.Count == 0)
        {
            Console.WriteLine("No books are currently checked out.");
        }
        else
        {
            Console.WriteLine("Current borrowers:");
            foreach (var borrower in borrowers)
            {
                Console.WriteLine($"\n{borrower.Key} ({borrower.Count()}/{MaxBooksPerUser} books):");
                foreach (var book in borrower)
                {
                    Console.WriteLine($"  - {book.Title} (Checked out: {book.CheckoutDate:MM/dd/yyyy})");
                }
            }
        }

        int availableBooks = books.Count(b => !b.IsCheckedOut);
        Console.WriteLine($"\nAvailable books: {availableBooks}/{books.Count}");
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
                string status = books[i].IsCheckedOut ? $" (Checked out by {books[i].BorrowedBy})" : " (Available)";
                Console.WriteLine($"{i + 1}. {books[i].Title}{status}");
            }
            
            int availableCount = books.Count(b => !b.IsCheckedOut);
            Console.WriteLine($"\nTotal books: {books.Count}");
            Console.WriteLine($"Available books: {availableCount}/{books.Count}");
        }
    }

    static void DisplayAvailableBooks()
    {
        var availableBooks = books.Where(b => !b.IsCheckedOut).ToList();
        
        if (availableBooks.Count == 0)
        {
            Console.WriteLine("No books are currently available for checkout.");
        }
        else
        {
            for (int i = 0; i < availableBooks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableBooks[i].Title}");
            }
        }
    }

    static int GetBorrowedBookCount(string userName)
    {
        return books.Count(b => b.IsCheckedOut && b.BorrowedBy.Equals(userName, StringComparison.OrdinalIgnoreCase));
    }

    static void ToggleBookStatus()
    {
        Console.WriteLine("\n--- Toggle Book Checkout Status ---");
        
        if (IsLibraryEmpty())
        {
            Console.WriteLine("The library is empty. There are no books to toggle.");
            return;
        }

        Console.WriteLine("Current books in the library:");
        DisplayBooks();

        Console.Write("\nEnter the title of the book to toggle: ");
        string bookTitle = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrEmpty(bookTitle))
        {
            Console.WriteLine("Invalid input. Book title cannot be empty.");
            return;
        }

        Book book = books.Find(b => b.Title.Equals(bookTitle, StringComparison.OrdinalIgnoreCase));

        if (book == null)
        {
            Console.WriteLine($"Book '{bookTitle}' was not found in the library.");
            return;
        }

        if (book.IsCheckedOut)
        {
            // Check the book back in
            string previousBorrower = book.BorrowedBy;
            book.IsCheckedOut = false;
            book.BorrowedBy = "";
            book.CheckoutDate = DateTime.MinValue;
            Console.WriteLine($"Book '{bookTitle}' has been checked in (was borrowed by {previousBorrower}).");
        }
        else
        {
            // Check the book out
            Console.Write("Enter borrower's name: ");
            string borrowerName = Console.ReadLine()?.Trim() ?? "";
            
            if (string.IsNullOrEmpty(borrowerName))
            {
                Console.WriteLine("Invalid input. Borrower name cannot be empty.");
                return;
            }

            // Check borrowing limit
            int currentBorrowedCount = GetBorrowedBookCount(borrowerName);
            if (currentBorrowedCount >= MaxBooksPerUser)
            {
                Console.WriteLine($"Sorry, {borrowerName} has already borrowed the maximum number of books ({MaxBooksPerUser}).");
                Console.WriteLine("Cannot check out this book.");
                return;
            }

            book.IsCheckedOut = true;
            book.BorrowedBy = borrowerName;
            book.CheckoutDate = DateTime.Now;
            Console.WriteLine($"Book '{bookTitle}' has been checked out to {borrowerName}.");
            Console.WriteLine($"Books borrowed by {borrowerName}: {currentBorrowedCount + 1}/{MaxBooksPerUser}");
        }
    }

    static bool IsLibraryEmpty()
    {
        return books.Count == 0;
    }

    static bool BookExists(string title)
    {
        return books.Exists(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }
}
