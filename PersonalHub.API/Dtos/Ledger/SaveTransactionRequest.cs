using PersonalHub.Domain.Helpers;

namespace PersonalHub.API.Dtos.Ledger
{
    public record SaveTransactionRequest(
        short Month,
        short Year,
        TransactionType Type,
        TransactionCategory CategoryId,
        string Description,
        decimal Amount
    );
}
