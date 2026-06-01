using PersonalHub.Domain.Entities.Base;

namespace PersonalHub.Domain.Entities.Ledger
{
    public class Summary : Entity
    {
        public short Month { get; set; }
        public short Year { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetBalance => TotalIncome - TotalExpense;
        public Summary(short month, short year, Guid userId, decimal totalIncome, decimal totalExpense)
        {
            Month = month;
            Year = year;
            UserId = userId;
            TotalIncome = totalIncome;
            TotalExpense = totalExpense;
        }
    }
}
