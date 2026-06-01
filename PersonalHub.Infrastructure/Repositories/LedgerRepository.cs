using Microsoft.EntityFrameworkCore;
using PersonalHub.Domain.Contracts;
using PersonalHub.Domain.Entities.Ledger;
using PersonalHub.Infrastructure.Contexts;

namespace PersonalHub.Infrastructure.Repositories
{
    public class LedgerRepository(PersonalHubContext context) : Repository<PersonalHubContext>(context), ILedgerRepository
    {
        public IQueryable<Transaction> GetTransactions(short month, short year, CancellationToken ct = default)
        {
            return _context.Transactions.Where(t => t.Month == month && t.Year == year);
        }
        public async Task CreateTransactionAsync(Transaction transaction, CancellationToken ct = default)
        {
            await _context.Transactions.AddAsync(transaction, ct);
            await _context.SaveChangesAsync(ct);
        }
        public async Task DeleteTransactionAsync(Guid id, CancellationToken ct = default)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id, ct) ?? throw new KeyNotFoundException("Transaction not found");
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync(ct);
        }
        public async Task<decimal> GetTotalByMonthAsync(short month, short year, CancellationToken ct = default)
        {
            return await _context.Transactions
                .Where(t => t.Month == month && t.Year == year)
                .SumAsync(t => t.Amount, ct);
        }
        public async Task<decimal> GetTotalByYearAsync(short year, CancellationToken ct = default)
        {
            return await _context.Transactions
                .Where(t => t.Year == year)
                .SumAsync(t => t.Amount, ct);
        }
        public async Task<Transaction> GetTransactionByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id, ct) ?? throw new KeyNotFoundException("Transaction not found");
        }
    }
}
