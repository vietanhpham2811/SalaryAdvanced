namespace SalaryAdvanced.Domain.Entities
{
    public class LimitSalary
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int ObjectId { get; set; }

        public decimal MaxOncePercent { get; set; }

        public int MaxTimesPerMonth { get; set; }

        public decimal MaxMonthlyPercent { get; set; }

        public int ValidFromDay { get; set; }

        public int ValidToDay { get; set; }
    }


}
