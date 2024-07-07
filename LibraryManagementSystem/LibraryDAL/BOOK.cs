using LibraryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace LibraryDAL
{
    public class BOOK
    {
        static void Main() { }
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public bool IsAvailable { get; set; }
        public BOOK(int bookId, string bookTitle, string author, string genre, bool isAvailable)
        {
            BookId = bookId;
            BookTitle = bookTitle;
            Author = author;
            Genre = genre;
            IsAvailable = isAvailable;
        }
    }
}
