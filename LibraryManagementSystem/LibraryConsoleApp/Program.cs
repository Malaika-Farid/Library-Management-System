using LibraryDAL;

DataAccess M = new();
M.GetDataFromFiles(); // getting all data from files into lists

Console.WriteLine("____PUCIT LIBRARY____");
Console.WriteLine("___ M E N U ___");
int option = 0;
int id;
string title,author,genre,name, email; // initializing variables to temporarily store user inputs
while (option != 12)
{
    Console.WriteLine("\n\t1. Add a new book\r\n" +
    "\t2. Remove a book\r\n" +
    "\t3. Update a book\r\n" +
    "\t4. Register a new borrower\r\n" +
    "\t5. Update a borrower\r\n" +
    "\t6. Delete a borrower\r\n" +
    "\t7. Borrow a book\r\n" +
    "\t8. Return a book\r\n" +
    "\t9. Search for books by title, author, or genre\r\n" +
    "\t10.View all books\r\n" +
    "\t11.View borrowed books by a specific borrower\r\n" +
    "\t12.Exit the application\r\n");

    Console.Write("Select an Option:");
    option = Convert.ToInt32(Console.ReadLine()); // reads user's option
    id = 0;
    title = author = genre = name = email = "";
    switch (option)
    {
        case 1: // add a book
            {
                bool available = false;
                while (id <= 0 || available) // validated inputs
                {
                    Console.Write("Enter the BOOK ID:");
                    id = Convert.ToInt32(Console.ReadLine());
                    available = (M.GetBookById(id) != null); // only if there's no book with same ID already
                    if (available)
                        Console.WriteLine("Book with this ID already exists...");
                }
                while (title == "")
                {
                    Console.Write("Enter the BOOK TITLE:");
                    title = Console.ReadLine();
                }
                while (author == "")
                {
                    Console.Write("Enter the BOOK AUTHOR:");
                    author = Console.ReadLine();
                }
                while (genre == "")
                {
                    Console.Write("Enter the BOOK GENRE:");
                    genre = Console.ReadLine();
                }
                M.AddBook(new BOOK(id, title, author, genre, true));
            }
            break;
        case 2: // remove book
            {
                while(M.GetBookById(id) == null || id<=0)
                {
                    Console.Write("Enter a valid ID : ");
                    id = int.Parse(Console.ReadLine());
                }
                M.RemoveBook(id);
            }
            break;
        case 3: //update book
            {
                while (M.GetBookById(id) == null || id <= 0)
                {
                    Console.Write("Enter the valid BOOK ID :");
                    id = int.Parse(Console.ReadLine());
                }
                BOOK foundBook = M.GetBookById(id);
                while (title == "")
                {
                    Console.Write("Enter the BOOK TITLE:");
                    title = Console.ReadLine();
                }
                while (author == "")
                {
                    Console.Write("Enter the BOOK AUTHOR:");
                    author = Console.ReadLine();
                }
                while (genre == "")
                {
                    Console.Write("Enter the BOOK GENRE:");
                    genre = Console.ReadLine();
                }
                M.UpdateBook(foundBook,title, author, genre);
            }
            break;
        case 4:  // add a borrower
            {
                id = 0;
                bool available = false;
                while (id <= 0 || available)
                {
                    Console.Write("Enter the Borrower's ID:");
                    id = int.Parse(Console.ReadLine());
                    available = (M.GetBorrowerById(id) != null); // only if there's no borrower with same ID already
                    if (available)
                        Console.WriteLine("Borrower with this ID already exists...");
                }
                while (name == "")
                {
                    Console.Write("Enter the name:");
                    name = Console.ReadLine();
                }
                available = false;
                while (!available)
                {
                    Console.Write("Enter the e-mail:");
                    email = Console.ReadLine();
                    available = ((M.GetBorrowerByEmail(email) == null) && (M.IsValidMail(email))); // only if there's no borrower with same email ID already or email is not valid
                    if (!available)
                        Console.WriteLine("Borrower with this Email ID already exists OR Email ID is not valid..");
                }
                M.RegisterBorrower(new Borrower(id, name, email));
            }
            break;
        case 5: //update a borrower
            {
                Console.Write("Enter the Borrower ID :");
                id = int.Parse(Console.ReadLine());
                Borrower found = M.GetBorrowerById(id);
                if (found != null)
                {
                    Console.Write("Update the NAME:");
                    name = Console.ReadLine();
                    while (!M.IsValidMail(email)) //while email is not valid, repititive inputs
                    {
                        Console.Write("Update the E-MAIL:");
                        email = Console.ReadLine();
                    }
                    M.UpdateBorrower(found, name , email);
                }
            }
            break;
        case 6: // delete a borrower
            {
                Console.Write("Enter the BORROWER ID :");
                id = int.Parse(Console.ReadLine());
                while (M.GetBorrowerById(id) == null)
                {
                    Console.Write("Enter a valid ID : ");
                    id = int.Parse(Console.ReadLine());
                }
                M.DeleteBorrower(id);
            }
            break;
        case 7: // borrow a book
            {
                Console.Write("Enter the BOOK ID :");
                int bookId = int.Parse(Console.ReadLine());
                BOOK book = M.GetBookById(bookId);
                while (book == null || book.IsAvailable==false)
                {
                    Console.Write("Enter a valid ID : ");
                    bookId = int.Parse(Console.ReadLine());
                    book = M.GetBookById(bookId);
                }
                Console.Write("Enter the BORROWER ID :");
                int borrowerId = int.Parse(Console.ReadLine());
                while (M.GetBorrowerById(borrowerId) == null)
                {
                    Console.Write("Enter a valid ID : ");
                    borrowerId = int.Parse(Console.ReadLine());
                }
                book.IsAvailable = false;
                TRANSACTION T = new TRANSACTION(M.NextTransactionNumber(),borrowerId,bookId,true,DateTime.Now);
                M.RecordTransaction(T);
            }
            break;
        case 8: // return a book
            {
                Console.Write("Enter the BOOK ID :");
                int bookId = int.Parse(Console.ReadLine());
                BOOK book = M.GetBookById(bookId);
                while (book == null || book.IsAvailable == true)
                {
                    Console.Write("Enter a valid ID : ");
                    bookId = int.Parse(Console.ReadLine());
                    book = M.GetBookById(bookId);
                }
                Console.Write("Enter the BORROWER ID :");
                int borrowerId = int.Parse(Console.ReadLine());
                while (M.GetBorrowerById(borrowerId) == null)
                {
                    Console.Write("Enter a valid ID : ");
                    borrowerId = int.Parse(Console.ReadLine());
                }
                book.IsAvailable = true;
                TRANSACTION T = new TRANSACTION(M.NextTransactionNumber(), borrowerId, bookId, false, DateTime.Now);
                M.RecordTransaction(T);
            }
            break;
        case 9: //search a book
            {
                int choice = 0;
                Console.WriteLine("1. Search By Title.");
                Console.WriteLine("2. Search By Author.");
                Console.WriteLine("3. Search By Genre.");
                string query = "";
                while(choice<1 || choice>3)
                {
                    Console.Write("Choice :");
                    choice = int.Parse(Console.ReadLine());
                }
                if (choice == 1)
                {
                    Console.Write("Enter the BOOK TITLE:");
                }
                else if(choice == 2)
                {
                    Console.Write("Enter the BOOK AUTHOR:");
                }
                else
                {
                    Console.Write("Enter the BOOK GENRE:");
                }
                query = Console.ReadLine();
                while (query=="")
                {
                    Console.WriteLine("Enter again : ");
                    query = Console.ReadLine();
                }
                List<BOOK> searchedBooks = M.SearchBooks(choice,query);
                //display founded books
                if (searchedBooks.Count > 0)
                {
                    Console.WriteLine("ID , BOOK TITLE , BOOK GENRE , BOOK AUTHOR , AVAILABILITY");
                    foreach (BOOK book in searchedBooks)
                    {
                        Console.WriteLine(book.BookId + " , " + book.BookTitle + " , " + book.Genre + " , " + book.Author + " , " + book.IsAvailable);
                    }
                }
                else
                {
                    Console.WriteLine("\n\nNot found...\n\n\n");
                }
            }
            break;
        case 10: //display all books
            List<BOOK> B = M.GetAllBooks();
            Console.WriteLine("ID , BOOK TITLE , BOOK GENRE , BOOK AUTHOR , AVAILABILITY");
            foreach (BOOK book in B)
            {
                Console.WriteLine(book.BookId + " , " + book.BookTitle + " , " + book.Genre + " , " + book.Author + " , " + book.IsAvailable);
            }
            break;
        case 11: // get books borrowed by a specific borrower
            {
                Console.Write("Enter the Borrower ID :");
                id = int.Parse(Console.ReadLine());
                while (M.GetBorrowerById(id) == null)
                {
                    Console.Write("Enter a valid ID : ");
                    id = int.Parse(Console.ReadLine());
                }
                Borrower found = M.GetBorrowerById(id);
                if (found != null)
                {
                    List<TRANSACTION> borrowedBooks = M.GetBorrowedBooksByBorrower(id);
                    Console.WriteLine("\n\nBooks borrowed by " + M.GetBorrowerById(id).Name);
                    if (borrowedBooks.Count() > 0)
                    {
                        Console.WriteLine("Book ID , Title , Author , Genre");
                        foreach (TRANSACTION T in borrowedBooks)
                        {
                            BOOK b = M.GetBookById(T.BookId);
                            if (b != null)
                                Console.WriteLine(b.BookId + " , " + b.BookTitle + " , " + b.Author + " , " + b.Genre);
                        }
                        Console.WriteLine("\n\n");
                    }
                    else
                    {
                        Console.WriteLine("NOT FOUND...");
                    }
                }
                else
                {
                    Console.WriteLine("No Book Borrowed");
                }
            }
            break;
        case 12:
            {
                Console.WriteLine("Exiting the system...");
                M.WriteDataToFiles();
                return;
            }
    }
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    Console.Clear();
}