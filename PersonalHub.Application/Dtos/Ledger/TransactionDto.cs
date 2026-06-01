using PersonalHub.Domain.Helpers;

namespace PersonalHub.Application.Dtos.Ledger
{
    public record TransactionDto(
        Guid Id,
        short Month,
        short Year,
        TransactionType Type,
        TransactionCategory CategoryId,
        string Description,
        decimal Amount,
        DateTime CreatedAt
    );
}
