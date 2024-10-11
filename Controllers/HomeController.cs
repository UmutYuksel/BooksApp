/*using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BooksApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.Controllers;

public class HomeController : Controller
{
   public HomeController()
    {

    }

    public IActionResult Index(string searchString, string category)
    {
        var products = Repository.Products;

        if (!string.IsNullOrEmpty(searchString))
        {
            ViewBag.SearchString = searchString;
            products = products.Where(p => p.Name!.ToLower().Contains(searchString)).ToList();
        }

        if (!string.IsNullOrEmpty(category) && category != "0")
        {
            products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
        }

        // ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId","Name",category);

        var model = new ProductViewModel
        {
            Products = products,
            Categories = Repository.Categories,
            SelectedCategory = category
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View();
    }

    [HttpPost]
public async Task<IActionResult> Create(Product model, IFormFile imageFile)
{
    var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
    long maxFileSize = 2 * 1024 * 1024; // 2 MB

    if (imageFile != null)
    {
        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError("", "Geçerli bir resim türü seçiniz.");
        }
        else if (imageFile.Length > maxFileSize)
        {
            ModelState.AddModelError("", "Dosya boyutu 2 MB'den büyük olamaz.");
        }
        else
        {
            var randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.Image = randomFileName;
            }
            catch
            {
                ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu!");
            }
        }
    }
    else
    {
        ModelState.AddModelError("", "Bir resim seçiniz!");
    }

    if (ModelState.IsValid)
    {
        model.ProductID = Repository.Products.Count + 1;
        Repository.CreateProduct(model);
        return RedirectToAction("Index");
    }

    ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
    return View(model);
}


    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = Repository.Products.FirstOrDefault(p => p.ProductID == id);
        if (entity == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(entity);
    }

    [HttpPost]
public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
{
    if (id != model.ProductID)
    {
        return NotFound();
    }

    var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
    long maxFileSize = 2 * 1024 * 1024; // 2 MB

    if (imageFile != null)
    {
        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError("", "Geçerli bir resim türü seçiniz.");
        }
        else if (imageFile.Length > maxFileSize)
        {
            ModelState.AddModelError("", "Dosya boyutu 2 MB'den büyük olamaz.");
        }
        else
        {
            var randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.Image = randomFileName;
            }
            catch
            {
                ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu!");
            }
        }
    }

    if (ModelState.IsValid)
    {
        Repository.EditProduct(model);
        return RedirectToAction("Index");
    }

    ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
    return View(model);
}


    public IActionResult Delete(int? id){
        if(id == null){
            return NotFound();
        }
        var entity = Repository.Products.FirstOrDefault(p=>p.ProductID == id);
        if(entity == null){
            return NotFound();
        }
        Repository.DeleteProduct(entity);
        return RedirectToAction("index");
    }
    
    public IActionResult ETicaret(){
        return View();
    }
} */

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BooksApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BooksApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string category)
        {
            var bookDataList = await _context.Books.ToListAsync(); // BookData listesini al
            var products = ConvertToProductList(bookDataList); // Dönüştür

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name!.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(category) && category != "0")
            {
                products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
            }

            var model = new ProductViewModel
            {
                Products = products,
                Categories = Repository.Categories,
                SelectedCategory = category
            };
            return View(model);
        }

        public async Task<IActionResult> UserList(string searchString, string category)
        {
            var bookDataList = await _context.Books.ToListAsync(); // BookData listesini al
            var activeBooks = ConvertToProductList(bookDataList).Where(b => b.isActive).ToList(); // Dönüştür ve filtrele

            if (!string.IsNullOrEmpty(searchString))
            {
                activeBooks = activeBooks.Where(p => p.Name!.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(category) && category != "0")
            {
                activeBooks = activeBooks.Where(p => p.CategoryId == int.Parse(category)).ToList();
            }

            var model = new ProductViewModel
            {
                Products = activeBooks,
                Categories = Repository.Categories
            };

            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile imageFile)
        {
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            long maxFileSize = 2 * 1024 * 1024; // 2 MB

            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim türü seçiniz.");
                }
                else if (imageFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("", "Dosya boyutu 2 MB'den büyük olamaz.");
                }
                else
                {
                    var randomFileName = $"{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        model.Image = randomFileName;
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu!");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Bir resim seçiniz!");
            }

            if (ModelState.IsValid)
            {
                var bookData = new BookData
                {
                    BookID = model.ProductID,
                    BookName = model.Name,
                    BookPage = model.Pages,
                    BookImage = model.Image,
                    CategoryID = model.CategoryId,
                    IsActive = model.isActive
                };

                await _context.Books.AddAsync(bookData); // DB'ye ekleme
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _context.Books.FirstOrDefault(p => p.BookID == id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new Product
            {
                ProductID = entity.BookID,
                Name = entity.BookName,
                Pages = entity.BookPage,
                Image = entity.BookImage,
                CategoryId = entity.CategoryID,
                isActive = entity.IsActive
            };

            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
        {
            if (id != model.ProductID)
            {
                return NotFound();
            }

            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            long maxFileSize = 2 * 1024 * 1024; // 2 MB

            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim türü seçiniz.");
                }
                else if (imageFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("", "Dosya boyutu 2 MB'den büyük olamaz.");
                }
                else
                {
                    var randomFileName = $"{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        model.Image = randomFileName;
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu!");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var bookData = new BookData
                {
                    BookID = model.ProductID,
                    BookName = model.Name,
                    BookPage = model.Pages,
                    BookImage = model.Image,
                    CategoryID = model.CategoryId,
                    IsActive = model.isActive
                };

                _context.Books.Update(bookData); // Güncellemeyi yap
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _context.Books.FirstOrDefault(p => p.BookID == id);
            if (entity == null)
            {
                return NotFound();
            }
            _context.Books.Remove(entity); // DB'den sil
            _context.SaveChanges(); // Değişiklikleri kaydet
            return RedirectToAction("Index");
        }

        private List<Product> ConvertToProductList(List<BookData> bookDataList)
        {
            return bookDataList.Select(book => new Product
            {
                ProductID = book.BookID,
                Name = book.BookName,
                Pages = book.BookPage,
                Image = book.BookImage,
                CategoryId = book.CategoryID,
                isActive = book.IsActive
            }).ToList();
        }

        public IActionResult ETicaret()
        {
            return View();
        }
    }
}
