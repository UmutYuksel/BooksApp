using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksApp.Models
{
    public class Product{

        [Display(Name = "Ürün Id")]
        public int ProductID { get; set; }

        [Display(Name = "Ürün Adı")]
        [Required(ErrorMessage = "Ürün Adı Eksik")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Display(Name = "Sayfa Sayısı")]
        [Required(ErrorMessage ="Sayfa Sayısı Eksik")]
        [Range(0,5000)]
        public decimal Pages {get;set;}

        [Display(Name = "Ürün Görseli")]
        public string? Image {get;set;} = string.Empty ;

        public bool isActive {get;set;}

        [Display(Name = "Kategori")]
        [Required(ErrorMessage ="Ürün Kategorisi Eksik")]
        public int? CategoryId {get;set;}
    }
}