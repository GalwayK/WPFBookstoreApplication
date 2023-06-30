# What is this application?

This is a Graphical User Interface (GUI) application coded in C# using the Windows Presentation Foundation, written with the Visual Studio code editor. This application emulates the function of an inventory tracking program for a Bookstore, tracking the information of books stocked by the store. 

# What information is tracked by this application?

Each book in this application is represented by a Book object. Each object has five main fields: title, author, genre, price, and ISBN. To track these fields, the Books are contained in an extended List object, which is used to collect the objects for display.

# What is the main design pattern used by this application? 

This application is composed of three primary collections of classes, which altogether function as the view, model, and controller for the application. They are as follows: 

<ol> 
	<li> 
The Book objects function as the model and contain the data for the program. They are contained inside a customized List collection. This collection and the Book objects implement the INotifyCollectionChanged and INotifyPropertyChanged interfaces, which update the GUI automatically when a contained value changes or a new object is added to the list. 
	</li> 
	<li> 
The BookStore class functions as the main controller of the program Through its public methods, it allows the view class to input data received from the user and to output data retrieved from the model. This class also provides a number of status codes and messages for use in the view, and is modular in design, functioning regardless of the structure of the view, be it GUI, console, or another method of output.
	</li> 
	<li> 
The MainWindow and BookManagement classes function as the views of the application. The BookManagement class contains a single instance of the BookStore class, which allows it to access the model data. The GUI is created with Extensible Application Markup Language (XAML) and groups the main functionalities into the View, Search, Delete, Edit, and Insert tabs.
	</li> 
</ol>

# How does this application track the book information in the GUI?

This application uses binding to bind the Book objects and the BookList collection to the GUI in the form of the DataGrid controls with the INotifyCollectionChanged and INotifyPropertyChanged interfaces. This allows the display to automatically update its fields whenever a new Book is inserted or an existing book is deleted or updated. 
