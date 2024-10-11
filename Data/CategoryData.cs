using System.ComponentModel.DataAnnotations;

namespace BooksApp.Data
{
    public class CategoryData
    {
        [Key]
        public int CategoryID { get; set; }

        [MaxLength(100)]
        public string? CategoryName { get; set; }
    }
}
