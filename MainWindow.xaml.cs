using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/*
 * This is first window for displaying my student name, ID, and the basic application information.
*/

namespace BookstoreTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Use singleton bookStore to access application controller
        static BookStore bookStore = BookStore.BookStoreFactory();

        // Initialize window and disallow window resizing
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }

        // Close application when window closed or Quit Button clicked
        private void QuitApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Start application if Start Button clicked
        private void StartApplication(object sender, RoutedEventArgs e)
        {
            BookManagement bookManagement = new BookManagement();
            bookManagement.ResizeMode = ResizeMode.NoResize;
            this.Hide();
            // Show Book Management Window and pause current window 
            bookManagement.ShowDialog();
            if (bookManagement.continueApplication)
            {
                // If return button clicked, return to start window
                this.Show();
            }
            else
            {
                // If quit button clicked, close application
                this.Close();
            }
        }
    }
}
