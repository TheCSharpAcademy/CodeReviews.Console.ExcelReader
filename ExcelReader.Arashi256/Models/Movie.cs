using System.ComponentModel.DataAnnotations;

namespace ExcelReader.Arashi256.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Year { get; set; } = string.Empty;
        [Required]
        public string Runtime { get; set; } = string.Empty;
        [Required]
        public decimal ImdbRating { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Name} ({Year}) Runtime: {Runtime} IMDB Rating: {ImdbRating}";
        }
    }
}
