using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMDB_Movie_API.Entities
{
    [Table("Producers")]
    public class Producer
    {
        [Key]
        public long ProducerId { get; set; }
        [Required]
        public string ProducerName { get; set; }
    }
}
 