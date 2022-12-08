using System.ComponentModel.DataAnnotations;

namespace IMDB_Movie_API.Entities
{
    public class Movie
    {
        [Key]
        public long MovieId { get; set; }
        public string Title { get; set; }
        public DateOnly? DateOfRelease { get; set; }
        public List<ActorMovies> Actors { get; set; }
        public long Producer { get; set; }

    }
}
