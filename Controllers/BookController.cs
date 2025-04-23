using Microsoft.AspNetCore.Mvc;
using BookLibrary.Data;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _db;

        // Konstruktorn injicerar AppDbContext
        public BookController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var list = _db.Books.ToList();
            return View(list);
        }
    }
}
