using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * This is a custom descendant of the List class for implementing the INotifyCollectionChanged interface and 
 * to allow for a Contains method for checking if the collection contains a book with a given ISBN.
*/

namespace BookstoreTracker
{
    internal class ListBookInventory: List<Book>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        // Implement custom overloaded behaviour for searching for specific ISBN
        public bool Contains(int isbn)
        {
            bool containsIsbn = false;
            Action<Book> GenerateContainsISBNAction()
            {
                void SearchForISBNAction(Book book) 
                { 
                    if (book.ISBN == isbn)
                    {
                        containsIsbn = true;
                    }
                }
                return SearchForISBNAction;
            }

            this.ForEach(GenerateContainsISBNAction());
            return containsIsbn;
        }

        // Use base List behaviour for adding Book, but also send NotifyCollection
        // changed event to update the WPF GUI lists
        public new void Add(Book book) 
        { 
            base.Add(book);
            CollectionChanged?.Invoke(this, new 
                NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, book));
        }

        // Use base List behaviour for removing Book, but also send NotifyCollection
        // changed event to update the WPF GUI lists
        public new void Remove(Book book) 
        { 
            base.Remove(book);
            CollectionChanged?.Invoke(this, new 
                NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, book));
        }
    }
}
