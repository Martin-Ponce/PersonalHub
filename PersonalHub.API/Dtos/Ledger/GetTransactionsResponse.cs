using PersonalHub.API.Dtos.Pagination;
using PersonalHub.Application.Dtos.Ledger;

namespace PersonalHub.API.Dtos.Ledger
{
    public record GetTransactionsResponse(PagedList<TransactionDto> Transactions, decimal Total);
}
