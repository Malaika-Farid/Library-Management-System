using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL
{
    public class TRANSACTION
    {
        public int TransactionId { get; set; }
        public int BorrowerId { get; set; }
        public int BookId { get; set; }
        public bool IsBorrowed { get; set; }
        public DateTime Date { get; set; }

        public TRANSACTION(int transactionId, int borrowerId, int bookId, bool isBorrowed, DateTime date)
        {
            TransactionId = transactionId;
            BorrowerId = borrowerId;
            BookId = bookId;
            IsBorrowed = isBorrowed;
            Date = date;
        }
    }
}
