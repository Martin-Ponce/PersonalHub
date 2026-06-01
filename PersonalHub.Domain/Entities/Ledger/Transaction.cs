using PersonalHub.Domain.Entities.Base;
using PersonalHub.Domain.Helpers;
using PersonalHub.Domain.Validators;

namespace PersonalHub.Domain.Entities.Ledger
{
    public class Transaction : Entity
    {
        public short Month { get; private set; }
        public short Year { get; private set; }
        public Guid UserId { get; private set; }
        public TransactionType Type { get; private set; }
        public TransactionCategory CategoryId { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; }

        public Transaction(short month, short year, Guid userId, TransactionType type, TransactionCategory categoryId, string description, decimal amount)
        {
            Month = month;
            Year = year;
            UserId = userId;
            Type = type;
            CategoryId = categoryId;
            Description = description.FormatDescription();
            Amount = amount;
            CreatedAt = DateTime.UtcNow;
        }
        public void Update(TransactionType type, TransactionCategory categoryId, string description, decimal amount)
        {
            Type = type;
            CategoryId = categoryId;
            Description = description.FormatDescription();
            Amount = amount;
        }
    }
}
