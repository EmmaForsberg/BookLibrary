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

        //Visa formuläret med bokdata
        //när anv klickar på redigera körs get.
        // hittar boken i db med rätt id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _db.Books.FindAsync(id);

            if (book == null) { return NotFound(); }

            var viewModel = new CreateBookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                IsRead = book.IsRead,
                GenreId = book.GenreId,  // Här sätter du GenreId så att den kan visas i formuläret
                Genres = await _db.Genres.ToListAsync() // Hämtar alla genrer till dropdown
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateBookViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                // Detta ger mer information om fel i ModelState
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                var book = await _db.Books.FindAsync(id);

                if (book == null)
                {
                    return NotFound();
                }

                // Uppdatera boken med de nya värdena från ViewModel
                book.Title = viewModel.Title;
                book.Author = viewModel.Author;
                book.Description = viewModel.Description;
                book.IsRead = viewModel.IsRead;
                book.GenreId = viewModel.GenreId;

                // Spara ändringarna
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Om modelstate inte är giltig, skicka tillbaka vyn med valideringsfel
            viewModel.Genres = await _db.Genres.ToListAsync();
            return View(viewModel);  // Här skickas ModelState med till vyn
        }

        //hämta boken och visa vy där anv kan bekräfta att bok ska tas bort
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //hämta boken från db med findasync(id)
            var book = await _db.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var viewmodel = new DeleteBookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
            };
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, DeleteBookViewModel deleteBookViewModel)
        {
            if (id != deleteBookViewModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var book = await _db.Books.FindAsync(id);

                if (book == null)
                {
                    return NotFound();
                }

                _db.Books.Remove(book);

                await _db.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }
    }
}

