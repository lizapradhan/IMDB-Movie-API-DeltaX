using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace IMDB_Movie_API.Entities
{
    [Table("Actors")]
    public class Actor
    {
        [Key]
        public long ActorId { get; set; }
        [Required]
        public string ActorName { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Bio { get; set; }

        public DateOnly DOB { get; set; }
    }
}
