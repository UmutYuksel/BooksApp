using System.ComponentModel.DataAnnotations;

namespace BooksApp.Data {

    public class BookData {

        [Key]
        public int BookID {get;set;}
        public string? BookName {get;set;}
        public decimal BookPage {get;set;}
        public string? BookImage {get;set;}

        public int? CategoryID {get;set;}

        public bool IsActive {get;set;}
    }
}