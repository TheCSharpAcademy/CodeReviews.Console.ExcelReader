using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelReader.Forser.Models
{
    public class HockeyModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Team { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Weight { get; set; }
        public string Height { get; set; }
        public string DateOfBirth { get; set; }
        public string HomeTown { get; set; }
        public string Provinces { get; set; }
        public string Position { get; set; }
        public int Age { get; set; }
        public string HeightFt { get; set; }
        public float Htln { get; set; }
        public int BMI { get; set; }
    }
}