using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

/*
 * This is the main controller class for accessing Book data
*/

namespace BookstoreTracker
{   
    internal class BookStore
    {
        // Constant to define store decided max and min prices
        internal const decimal BOOK_MIN_PRICE = 0.0M;
        internal const decimal BOOK_MAX_PRICE = 9999.99M;

        // Static BookStore instance to act as application controller
        private static readonly BookStore singletonBookStore = new BookStore();

        // Custom List<Book> Child class to define additional List<Book> behaviours
        private readonly ListBookInventory inventoryBooks;

        // Private constructor the public creation of any BookStore instances
        private BookStore()
        {
            inventoryBooks = new ListBookInventory();
        }

        // Function for returning read-only copy of Inventory List
        public ListBookInventory GetInventory()
        {
            // To prevent external modification, this method returns a copy of the inventory
            return inventoryBooks;
        }

        // Function for returning sorted read-only copy of Inventory List
        public List<Book> GetSortedInventory()
        {
            List<Book> copyBooks = GetInventory();
            copyBooks.Sort();
            return copyBooks;
        }

        // Disallow creation of more than one BookStore instance as controller. 
        static public BookStore BookStoreFactory()
        {
            return singletonBookStore;
        }

        // Kyle Galway 
        // 991418738
        // Function for searching through inventory by genre between max and min price ranges.
        public List<Book> SearchInventoryByAuthor(string author, decimal minPrice = BOOK_MIN_PRICE, 
            decimal maxPrice = BOOK_MAX_PRICE)
        {
            List<Book> searchedInventory = new List<Book>(from book in inventoryBooks
                                                          where book.Price >= minPrice
                                                          && book.Price <= maxPrice
                                                          && book.Author.ToLower().Equals(author)
                                                          select book);

            return searchedInventory;
        }

        // Function for searching through inventory by author between max and min price ranges.
        public List<Book> SearchInventoryByGenre(string genre, decimal? minPrice = BOOK_MIN_PRICE, 
            decimal? maxPrice = BOOK_MAX_PRICE)
        {
            List<Book> searchedInventory = new List<Book>(from book in inventoryBooks
                                                       where book.Price >= minPrice 
                                                       && book.Price <= maxPrice
                                                       && book.Genre.Equals(genre)
                                                       select book);

            return searchedInventory;
        }

        // Function for getting book by ISBN to display on main page
        public Book GetBookByISBN(int isbn)
        {
            Book searchedBook = null;
            Action<Book> CreateISBNSearchAction()
            {
                void SearchForBook(Book book)
                {
                    if (book.ISBN == isbn)
                    {
                        searchedBook = book;
                    }
                }
                return SearchForBook;
            }

            inventoryBooks.ForEach(CreateISBNSearchAction());

            return searchedBook;
        }

        // Function for updating a book's price by searching for it's matching ISBN
        public StatusNumber UpdateBook(int isbn, decimal price)
        {
            StatusNumber statusNumber = StatusNumber.UPDATE_ERROR;
            Book book = GetBookByISBN(isbn);
            if (price >= BOOK_MIN_PRICE && price <= BOOK_MAX_PRICE && book != null)
            {
                book.Price = price;
                statusNumber = StatusNumber.SUCCESS;
            }
            return statusNumber;
        }

        // My name is Kyle Galway 
        // Function for deleting book by it's matching ISBN
        public StatusNumber DeleteBook(int isbn)
        {
            StatusNumber statusNumber = StatusNumber.DELETION_ERROR;
            Book book = GetBookByISBN(isbn);

            if (book != null)
            {
                inventoryBooks.Remove(book);
                statusNumber = StatusNumber.SUCCESS;
            }

            return statusNumber;
        }

        // Function for adding single book to inventory
        public StatusNumber AddBookToInventory(string title, string author, string 
            genre, int isbn, decimal price)
        {
            StatusNumber statusNumber = StatusNumber.INSERT_ERROR;
            if (price >= BOOK_MIN_PRICE && price <= BOOK_MAX_PRICE && !inventoryBooks.Contains(isbn))
            {
                Book book = new Book(title, author, genre, isbn, price);
                inventoryBooks.Add(book);
                statusNumber = StatusNumber.SUCCESS;
            }
            return statusNumber;
        }

        // Create Dictionary for retrieving status BookStore status messages
        private static readonly Dictionary<StatusNumber, string> BOOKSTORE_STATUS_CODES = GenerateStatusCodes();

        // Enum to hold onto status code numbers for allowing for semantic meaning
        public enum StatusNumber
        {
            SUCCESS = 0,
            DELETION_ERROR = -1,
            UPDATE_ERROR = -2,
            INSERT_ERROR = 3,
            STATUS_ERROR = -4
        }

        // Function for generating status codes of BookStore operations
        private static Dictionary<StatusNumber, string> GenerateStatusCodes()
        {
            Dictionary<StatusNumber, string> statusCodes = new Dictionary<StatusNumber, string>();
            statusCodes.Add(StatusNumber.SUCCESS, "Operation Success!");
            statusCodes.Add(StatusNumber.DELETION_ERROR, "Deletion Unsuccessful!");
            statusCodes.Add(StatusNumber.UPDATE_ERROR, "Update Unsuccessful!");
            statusCodes.Add(StatusNumber.INSERT_ERROR, "Insertion Unsuccessful!");
            statusCodes.Add(StatusNumber.STATUS_ERROR, "Status Message Not Found!");
            return statusCodes;
        }

        // Function for retrieving status message of matching status number
        public static string GetStatusMessage(StatusNumber statusNumber)
        {
            string statusMessage = BOOKSTORE_STATUS_CODES[StatusNumber.STATUS_ERROR];

            if (BOOKSTORE_STATUS_CODES.ContainsKey(statusNumber))
            {
                statusMessage = BOOKSTORE_STATUS_CODES[statusNumber];
            }
            return statusMessage;
        }
    }
}
