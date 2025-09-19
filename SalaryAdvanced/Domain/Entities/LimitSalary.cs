using System.ComponentModel.DataAnnotations;

namespace SalaryAdvanced.Domain.Entities
{
    public class LimitSalary
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }

        //[Required(ErrorMessage = "ObjectId is required")]
        public int? ObjectId { get; set; }

        [Range(0, 100, ErrorMessage = "Max Once % must be between 0 and 100")]
        public double? MaxOncePercent { get; set; }

        //[Range(0, 10, ErrorMessage = "Max Times/Month must be between 0 and 10")]
        public int? MaxTimesPerMonth { get; set; }

        [Range(0, 100, ErrorMessage = "Max Monthly % must be between 0 and 100")]
        public double? MaxMonthlyPercent { get; set; }

        [Range(1, 31, ErrorMessage = "Valid From Day must be between 1 and 31")]
        public int? ValidFromDay { get; set; }

        [Range(1, 31, ErrorMessage = "Valid To Day must be between 1 and 31")]
        public int? ValidToDay { get; set; }
        public DateTime create_date { get; set; } = DateTime.UtcNow;
        public DateTime last_change { get; set; } = DateTime.UtcNow;
    }


}
