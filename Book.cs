using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

/*
 * This is the main controller class for accessing Book data
*/

namespace BookstoreTracker
{
    /*
     * IComparable: Needed to compare books 
     * INotifyPropertyChanged: Needed to update the WPF GUI when a property changes
    */

    internal class Book: IComparable<Book>, INotifyPropertyChanged
    {
        private string _author;
        private string _title;
        private string _genre;
        private decimal _price;
        private int _isbn;

        // PropertyChangedEventHandler for notifying WPF GUI when a property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor
        public Book(string title, string author, string genre, int isbn, decimal price)
        {
            ISBN = isbn;
            Price = price;
            Title = title;
            Author = author;
            Genre = genre;
        }

        // Standard setter and getter for _title
        public string Title
        {
            get;
            set;
        }

        // Standard setter and getter for _author
        public string Author
        {
            get;
            set;
        }

        // Standard setter and getter for _genre
        public string Genre
        {
            get;
            set;    
        }

        // Standard setter and custom getter to disallow negative ISBN
        public int ISBN
        {
            get => _isbn;
            set
            {
                // If ISBN negative, throw Exception
                if (value >= 0) 
                { 
                    _isbn  = value;
                }
                else
                {
                    throw new ArgumentException("Error: ISBN cannot be negative!");
                }
            }
        }

        // Standard Getter and Custom Setter to send notify propery changed exception
        public decimal Price
        {
            get => _price; 
            set
            {
                if (value >= 0)
                {
                    _price = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                }
                else
                {
                    throw new ArgumentException("Error: Price cannot be negative!");
                }
            }
        }

        // Bool operators for coparing Books using book prices

        // Greater than operator implementation
        public static bool operator > (Book bookOne, Book bookTwo)
        {
            return bookOne.Price > bookTwo.Price;
        }

        // Less than operator implementation
        public static bool operator < (Book bookOne, Book bookTwo)
        {
            return bookOne.Price < bookTwo.Price;
        }

        // Custom Equals method implementation for compare Book prices
        public override bool Equals(object obj)
        {
            if (obj is Book)
            {
                Book book = obj as Book;
                return book.Price == this.Price;
            }
            return false;
        }

        // Custom Greater than or Equal to operator 
        public static bool operator >= (Book bookOne, Book bookTwo)
        {
            return bookOne.Price >= bookTwo.Price;
        }

        // Custom Less than or Equal to operator
        public static bool operator <= (Book bookOne, Book bookTwo)
        {
            return bookOne.Price <= bookTwo.Price;
        }


        // CompareTo Method for IComparable implementation by comparing Book prices
        public int CompareTo(Book book)
        {
            return this > book 
                ? 1
                : this < book 
                    ? -1 
                    : 0;
        }

        // Custom method for checking if two Book objects reference the same Object
        public bool IsSameBook(Book that)
        {
            return this.Author.Equals(that.Author) && this.Author.Equals(that.Author);
        }

        // Custom ToString implementation
        public override string ToString()
        {
            return $"Title: {Title} Author: {Author} Genre: {Genre} ISBN: {ISBN}";
        }


        // Use base object GetHashCode implementation
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
