namespace BookLibrary.Models;
using System.ComponentModel.DataAnnotations;


public class Book
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Author { get; set; }
    public bool IsRead { get; set; }
    [Required]
    public int GenreId { get; set; }
    public Genre Genre { get; set; }

}

