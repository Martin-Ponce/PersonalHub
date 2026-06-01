
using PersonalHub.Domain.Entities.Ledger;

namespace PersonalHub.Domain.Contracts
{
    public interface ILedgerRepository : IRepository
    {
        IQueryable<Transaction> GetTransactions(short month, short year, CancellationToken ct = default);
        Task CreateTransactionAsync(Transaction transaction, CancellationToken ct = default);
        Task DeleteTransactionAsync(Guid id, CancellationToken ct = default);
        Task<decimal> GetTotalByMonthAsync(short month, short year, CancellationToken ct = default);
        Task<decimal> GetTotalByYearAsync(short year, CancellationToken ct = default);
        Task<Transaction> GetTransactionByIdAsync(Guid id, CancellationToken ct = default);
    }
}
