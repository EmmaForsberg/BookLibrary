using BookLibrary.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookLibrary.ViewModel
{
    public class CreateBookViewModel
    {
        public int Id { get; set; }
        public int GenreId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public bool IsRead { get; set; }
        [ValidateNever]
        public List<Genre> Genres { get; set; } // Inget [BindNever], bara testa

    }
}
