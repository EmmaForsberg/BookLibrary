namespace BookLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool IsRead { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

    }
}
