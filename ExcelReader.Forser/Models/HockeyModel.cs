using System.ComponentModel.DataAnnotations;

namespace ExcelReader.Forser.Models
{
    public class HockeyModel
    {
        [Key]
        public int Id { get; set; }
        public string Team { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Weight { get; set; }
        public string Height { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string HomeTown { get; set; }
        public string Provinces { get; set; }
        public string Position { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
        public string HeightFt { get; set; }
        public float Htln { get; set; }
        public int BMI { get; set; }
    }
}