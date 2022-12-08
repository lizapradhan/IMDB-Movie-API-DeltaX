using System.ComponentModel.DataAnnotations;

namespace IMDB_Movie_API.Entities
{
    public class ActorMovies
    {
        [Key]
        public long MappingId { get; set; }
        [Required]
        public long actorId { get; set; }
        
        public long? movieId { get; set; }
    }
}
