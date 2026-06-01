using PersonalHub.Domain.Helpers;

namespace PersonalHub.Application.Dtos.Ledger
{
    public record TransactionRequest(Guid Id, short Month, short Year, TransactionType Type, TransactionCategory CategoryId, string Description, decimal Amount);
}
