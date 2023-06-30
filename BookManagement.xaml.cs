using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/*
 * This is the main view class for outputting BookStore data and recieving user input from the WPF GUI
*/

namespace BookstoreTracker
{
    /// <summary>
    /// Interaction logic for BookManagement.xaml
    /// </summary>
    public partial class BookManagement : Window
    {
        // Initial field for determining start window behaviour when window closed
        public bool continueApplication = false;

        // Create bookStore variable to access singleton controller
        private static BookStore bookStore = BookStore.BookStoreFactory();
        public BookManagement()
        {
            InitializeComponent();
        }

        // Function for initializing all default values in window.
        private void InitializeWindow(object sender, RoutedEventArgs e)
        {
            continueApplication = false;
            gridData.ItemsSource = bookStore.GetInventory();
            gridSearchedData.ItemsSource = null;
        }

        // Method for handling search by genre events when search button clicked
        private void SearchByGenre(object sender, RoutedEventArgs e)
        {
            string strGenre = txtSearchInputGenre.Text;

            List<Book> searchedBooks = bookStore.SearchInventoryByGenre(strGenre);

            string status = $"Could not find books of genre {strGenre}";
            if (searchedBooks.Count > 0)
            {
                gridSearchedData.ItemsSource = searchedBooks;
                status = $"Found books of genre {strGenre}";
            }
            lblStatus.Content = status;
            ClearSearchText();
        }

        // Method for handling search by author events when a search event is submitted
        private void SearchByAuthor()
        {
            string strAuthor = txtSearchInputAuthor.Text.ToLower();
            string status = "Error: Author name not valid";

            if (strAuthor.Length > 0)
            {
                List<Book> searchedBooks = bookStore.SearchInventoryByAuthor(strAuthor);
                status = $"Could not find books by {strAuthor}";
                if (searchedBooks.Count > 0)
                {
                    gridSearchedData.ItemsSource = searchedBooks;
                    status = $"Found books by {strAuthor}";
                }
                gridSearchedData.ItemsSource = searchedBooks;
            }
            lblStatus.Content = status;
            ClearSearchText();
        }

        // Event method for routing mouse click search by author button events
        private void SearchByAuthor(object sender, RoutedEventArgs e)
        {
            SearchByAuthor();
        }

        // Event method for routing key down search by author button events
        private void SearchByAuthor(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                SearchByAuthor();
            }
        }
        
        // Event method for handling book insertion events
        private void InsertNewBook(object sender, RoutedEventArgs e)
        {
            string strTitle = "";
            string strAuthor = "";
            string strGenre = "";
            decimal monPrice = 0.0M;
            int isbn = 0;

            bool IsInputValid()
            {
                bool isInputValid = false;
                try
                {
                    strTitle = txtInsertTitle.Text;
                    strAuthor = txtInsertAuthor.Text;
                    strGenre = txtInsertGenre.Text;
                    monPrice = Convert.ToDecimal(txtInsertPrice.Text);
                    isbn = Convert.ToInt32(txtInsertISBN.Text);

                    bool isTitleValid = strTitle.Length > 0;
                    bool isAuthorValid = strAuthor.Length > 0;
                    bool isGenreValid = strGenre.Length > 0;
                    bool isPriceValid = monPrice > BookStore.BOOK_MIN_PRICE && monPrice < BookStore.BOOK_MAX_PRICE;
                    bool isISBNValid = isbn > 0;

                    isInputValid = isTitleValid && isAuthorValid && isGenreValid && isPriceValid && isISBNValid;

                }
                catch (Exception) { }
                return isInputValid;
            }

            BookStore.StatusNumber statusNumber;

            if (IsInputValid() ) 
            {
                Console.WriteLine("Trying to Insert!");
                statusNumber = bookStore.AddBookToInventory(strTitle, strAuthor, strGenre, isbn, monPrice);
            }
            else
            {
                Console.WriteLine("Can't insert for some reason!");
                statusNumber = BookStore.StatusNumber.INSERT_ERROR;
            }
            lblStatus.Content = BookStore.GetStatusMessage(statusNumber);
            ClearInsertText();
        }

        // Routing method for handling edit form reveal events on button click
        private void RevealEditInformationMouseEvent(object sender, RoutedEventArgs e)
        {
            RevealEditInformation();
        }

        // Routing method for handling edit form reveal events on keydown events
        private void RevealEditInformationKeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                RevealEditInformation();
            }
        }

        // Retrieve book information to reveal edit form when ISBN enter into Edit tab 
        private void RevealEditInformation()
        {
            EditInnerGrid.Opacity = 0;
            bool isISBNValid = false;
            string status = "No matching book found";
            int isbn = 0;

            void RevealEditInformation(Book book)
            {
                if (book != null)
                {
                    txtEditAuthor.Text = book.Author;
                    txtEditTitle.Text = book.Title;
                    txtEditGenre.Text = book.Genre;
                    txtEditPrice.Text = book.Price.ToString();

                    status = $"Found book with ISBN {isbn}";
                    EditInnerGrid.Opacity = 1;
                }
            }

            try
            {
                isbn = Convert.ToInt32(txtEditISBN.Text);
                isISBNValid = isbn > 0;
            }
            catch
            {
                status = "ISBN must be a number";
            }

            if (isISBNValid)
            {
                Book searchedBook = bookStore.GetBookByISBN(isbn);
                RevealEditInformation(searchedBook);
            }
            else
            {
                status = "ISBN invalid";
            }
            lblStatus.Content = status;
        }

        // Event handling method for handling update book events
        private void UpdateBook(object sender, RoutedEventArgs e)
        {
            string status = "Invalid input";
            int? isbn = 0;
            decimal? monPrice = 0.0M;
            bool IsInputValid()
            {
                bool isPriceValid = false;
                bool isISBNValid = false;
                bool isInputValid = false;
                monPrice = null;
                try
                {
                    monPrice = Convert.ToDecimal(txtEditPrice.Text);
                }
                catch (Exception ex) { }
                
                isPriceValid = monPrice != null && monPrice > BookStore.BOOK_MIN_PRICE && monPrice < BookStore.BOOK_MAX_PRICE;

                isbn = null;
                try
                {
                    isbn = Convert.ToInt32(txtEditISBN.Text);
                }
                catch (Exception ex) { }

                isISBNValid = isbn != null && isbn > 0;

                isInputValid = isPriceValid && isISBNValid;

                return isInputValid;
            }

            BookStore.StatusNumber statusNumber;
            if (IsInputValid()) 
            {
                statusNumber = bookStore.UpdateBook((int)isbn, (decimal)monPrice);
                status = BookStore.GetStatusMessage(statusNumber);
            }
            else
            {
                statusNumber = BookStore.StatusNumber.UPDATE_ERROR;
                status = BookStore.GetStatusMessage(statusNumber);
            }
            lblStatus.Content = status;
            ClearEditText();
        }

        // Event handling method for handling delete book events
        private void DeleteBook(object sender, RoutedEventArgs e)
        {
            string status = "ISBN Invalid";
            BookStore.StatusNumber statusNumber;

            if (IsTextValidISBN(txtDeleteISBN))
            {
                int? isbn = -1;
                try
                {
                    isbn = Convert.ToInt32(txtDeleteISBN.Text);
                }
                catch (Exception ex) { }
                
                statusNumber = bookStore.DeleteBook((int)isbn);
                status = BookStore.GetStatusMessage(statusNumber);
            }
            else
            {
                statusNumber = BookStore.StatusNumber.DELETION_ERROR;
                status = BookStore.GetStatusMessage(statusNumber);
            }
            lblStatus.Content = status;
            ClearDeleteText();
        }

        // Utility method for checking validlity of inputted ISBN
        private bool IsTextValidISBN(TextBox txtIsbn)
        {
            int? isbn = null;
            try
            {
                isbn = Convert.ToInt32(txtIsbn.Text);
            }
            catch (Exception ex) 
            {

            }
            return isbn != null && isbn > 0;
        }

        // Utility method for checking validity of inputted price
        private bool IsTextValidPrice(TextBox txtPrice)
        {

            decimal? monPrice = null;
            try
            {
                monPrice = Convert.ToDecimal(txtPrice.Text);
            }
            catch (Exception ex)
            {

            }
            return monPrice != null && monPrice > BookStore.BOOK_MIN_PRICE && monPrice < BookStore.BOOK_MAX_PRICE;
        }

        // Method for clearing all leftover text on tab change
        private void FocusShowTab(object sender, RoutedEventArgs e)
        {
            TabShow.Focus();
            ClearAllText();
        }

        // Method for clearing all leftover text on tab change
        private void FocusSearchTab(object sender, RoutedEventArgs e)
        {
            TabSearch.Focus();
            ClearAllText();
        }

        // Method for clearing all leftover text on tab change
        private void FocusInsertTab(object sender, RoutedEventArgs e)
        {
            TabInsert.Focus();
            ClearAllText();
        }

        // Method for clearing all leftover text on tab change
        private void FocusEditTab(object sender, RoutedEventArgs e)
        {
            TabEdit.Focus();
            ClearAllText();
        }

        // Method for clearing all leftover text on tab change
        private void FocusDeleteTab(object sender, RoutedEventArgs e)
        {
            TabDelete.Focus();
            ClearAllText();
        }

        // Method for returning to start page
        private void Return(object sender, RoutedEventArgs e)
        {
            continueApplication = true;
            this.Close();
        }

        // Method for closing application
        private void QuitApplication(object sender, RoutedEventArgs e)
        {
            continueApplication = false;
            this.Close();
        }

        // Method for clearing all text in search tab
        void ClearSearchText()
        {
            txtSearchInputAuthor.Text = string.Empty;
            txtSearchInputGenre.Text = string.Empty;
        }

        // Method for clearing all text in edit tab
        void ClearEditText()
        {
            txtDeleteISBN.Text = string.Empty;
            txtEditAuthor.Text = string.Empty;
            txtEditPrice.Text = string.Empty;
            txtEditTitle.Text = string.Empty;
            txtEditISBN.Text = string.Empty;
        }

        // Method for clearing all text in delete tab
        void ClearDeleteText()
        {
            txtDeleteISBN.Text = string.Empty;
        }

        // Method for clearing all text in insert tab
        void ClearInsertText()
        {
            txtInsertAuthor.Text = string.Empty;
            txtInsertPrice.Text = string.Empty;
            txtInsertISBN.Text = string.Empty;  
            txtInsertTitle.Text = string.Empty;
            txtInsertGenre.Text = string.Empty;
        }

        // Method for clearing all text in application
        void ClearAllText()
        {
            ClearEditText();
            ClearDeleteText();
            ClearInsertText();
            ClearSearchText();
        }

        // Method for handling tab change events in menu
        private void TabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl) 
            {
                ClearAllText();
            }
        }
    }
}
