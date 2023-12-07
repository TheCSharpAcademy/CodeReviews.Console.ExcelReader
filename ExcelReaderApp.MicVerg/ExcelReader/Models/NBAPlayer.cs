using System.ComponentModel.DataAnnotations;

namespace ExcelReader.Models
{
    internal class NBAPlayer
    {
        [Key]
        public int Id { get; set; }
        public string playerName { get; set; }
        public int gamesPlayed { get; set; }
        public double pointsPerGame { get; set; }
        public double minutesPerGame { get; set; }
        public double reboundsPerGame { get; set; }
        public double assistsPerGame { get; set; }
    }
}
