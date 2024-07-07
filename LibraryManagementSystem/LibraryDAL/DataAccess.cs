using LibraryDAL;
using System.Net;
using System.Transactions;

namespace LibraryDAL
{
    public class DataAccess
    {
        internal List<BOOK> books = new List<BOOK>();
        internal List<Borrower> borrowers = new List<Borrower>();
        internal List<TRANSACTION> transactions = new List<TRANSACTION>();
        public void GetDataFromFiles()
        {
            // reading data from each file to respective list
            StreamReader reader = new StreamReader("Book.txt");
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                books.Add(new BOOK(int.Parse(parts[0]), parts[1], parts[2], parts[3], Convert.ToBoolean(parts[4])));
            }
            reader.Close();
            reader = new StreamReader("Borrower.txt");
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                borrowers.Add(new Borrower(int.Parse(parts[0]), parts[1], parts[2]));
            }
            reader.Close();
            reader = new StreamReader("Transaction.txt");
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                transactions.Add(new TRANSACTION(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), bool.Parse(parts[3]), DateTime.Parse(parts[4])));
            }
            reader.Close();
        }
        public void WriteDataToFiles()
        {
            //on exiting,writing data from lists to files
            StreamWriter writer = new StreamWriter("Book.txt");
            foreach (BOOK book in books)
            {
                writer.WriteLine(book.BookId + "," + book.BookTitle + "," + book.Author + "," + book.Genre + "," + book.IsAvailable);
            }
            writer.Close();
            writer = new StreamWriter("Borrower.txt");
            foreach (Borrower borrower in borrowers)
            {
                writer.WriteLine(borrower.BorrowerId + "," + borrower.Name + "," + borrower.Email);
            }
            writer.Close();
            writer = new StreamWriter("Transaction.txt");
            foreach (TRANSACTION transaction in transactions)
            {
                writer.WriteLine(transaction.TransactionId + "," + transaction.BorrowerId + "," + transaction.BookId + "," + transaction.IsBorrowed + "," + transaction.Date);
            }
            writer.Close();
        }
        public List<BOOK> GetAllBooks() // Retrieves all books in the library
        {
            return books;
        }

        public BOOK GetBookById(int bookId) // Retrieves a specific book by ID
        {
            foreach (BOOK book in books)
            {
                if (book.BookId==bookId)
                    return book;
            }
            return null;
        }

        public void RemoveBook(int bookId) // Removes a book from the library
        {
            books.Remove(GetBookById(bookId));
        }

        public void AddBook(BOOK book) // Adds a new book to the library
        {
             books.Add(book);
        }

        public void UpdateBook(BOOK book,string title,string author,string genre)
        {
            book.Author = author;
            book.Genre = genre;
            book.BookTitle = title;
        }

        public List<BOOK> SearchBooks(int choice , string query) // Searches for books by title, author, or genre
        {
            List<BOOK> found = new List<BOOK>();   
            if (choice==1)
            {
                foreach(BOOK book in books)
                {
                    if(book.BookTitle==query)
                    {
                        found.Add(book);
                    }
                }
            }
            else if (choice == 2)
            {
                foreach (BOOK book in books)
                {
                    if (book.Author == query)
                    {
                        found.Add(book);
                    }
                }
            }
            else
            {
                foreach (BOOK book in books)
                {
                    if (book.Genre == query)
                    {
                        found.Add(book);
                    }
                }
            }
            return found;
        }
        public void RegisterBorrower(Borrower borrower) // Registers a new borrower
        {
            bool flag = false;
            foreach(Borrower b in borrowers) // check if not any with same id already
            {
                if(b.BorrowerId==borrower.BorrowerId)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                borrowers.Add(borrower);
            }
        }
        public Borrower GetBorrowerById(int id)
        {
            foreach (Borrower b in borrowers)
            {
                if (b.BorrowerId == id)
                    return b;
            }
            return null;
        }
        public Borrower GetBorrowerByEmail(string email)
        {
            foreach (Borrower b in borrowers)
            {
                if (b.Email == email)
                    return b;
            }
            return null;
        }
        public void UpdateBorrower(Borrower b,string name,string email)
        {
            b.Name = name;
            b.Email = email;
        }
        public void DeleteBorrower(int borrowerId) // Deletes a borrower
        {
            borrowers.Remove(GetBorrowerById(borrowerId));
        }
        public List<TRANSACTION> GetBorrowedBooksByBorrower(int borrowerId) // Retrieves borrowed by a specific borrower
        {
            List<int> returnedBookIds = new List<int>();
            List<TRANSACTION> borrowedBooks = new List<TRANSACTION>();

            // Collect borrowed books and returned book IDs
            foreach (TRANSACTION T in transactions)
            {
                if (T.BorrowerId == borrowerId)
                {
                    if (T.IsBorrowed)
                    {
                        borrowedBooks.Add(T);
                    }
                    else
                    {
                        returnedBookIds.Add(T.BookId);
                    }
                }
            }

            // Create a new list to avoid modifying the collection while iterating
            List<TRANSACTION> finalBorrowedBooks = new List<TRANSACTION>();

            foreach (TRANSACTION T in borrowedBooks)
            {
                if (!returnedBookIds.Contains(T.BookId))
                {
                    finalBorrowedBooks.Add(T);
                }
            }

            return finalBorrowedBooks;
        }
        public int NextTransactionNumber()
        {
            if (transactions.Any())
            {
                return transactions.Max(t => t.TransactionId) + 1;
            }
            else
            {
                return 1; // If no transactions exist, start with 1
            }
        }

        public void RecordTransaction(TRANSACTION transaction) // Records a borrowing or returning transaction.
        {
            transactions.Add(transaction);
        }

        public bool IsValidMail(string email)
        {
            string[] parts = email.Split('@');
            if (parts.Length != 2)
                return false;
            string username = parts[0];
            string domain = parts[1];
            // Check username part
            if (username=="")
                return false;

            foreach (char c in username)
            {
                // Check if username contains valid characters
                if (!(char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '%' || c == '+' || c == '-'))
                    return false;
            }

            // Check domain part
            if (domain=="")
                return false;

            string[] domainParts = domain.Split('.');

            // Check if there are at least two parts in the domain (domain and top-level domain)
            if (domainParts.Length < 2)
                return false;

            foreach (string part in domainParts)
            {
                foreach (char c in part)
                {
                    // Check if domain parts contain valid characters
                    if (!(char.IsLetterOrDigit(c) || c == '-'))
                        return false;
                }
            }
            return true;
        }
    }
}