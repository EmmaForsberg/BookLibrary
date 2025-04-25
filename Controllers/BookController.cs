using BookLibrary.Data;
using BookLibrary.Models;
using BookLibrary.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var list = _db.Books.Include(b => b.Genre).ToList();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //get list of genres
            var genres = await _db.Genres.ToListAsync();
            if (genres == null || genres.Count == 0)
            {
                // Kontrollera om det finns några genrer
                Console.WriteLine("Inga genrer finns i databasen");
            }
            var viewModel = new CreateBookViewModel
            {
                Genres = genres
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel viewModel)
        {
            // Logga värdet på GenreId för felsökning
            Console.WriteLine($"GenreId: {viewModel.GenreId}");

            if (!ModelState.IsValid)
            {
                // Ladda genrer på nytt om model binding misslyckas
                viewModel.Genres = await _db.Genres.ToListAsync();
                return View(viewModel);
            }

            var book = new Book
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                Author = viewModel.Author,
                IsRead = viewModel.IsRead,
                GenreId = viewModel.GenreId
            };

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

