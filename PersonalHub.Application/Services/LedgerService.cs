using PersonalHub.Application.Dtos.Ledger;
using PersonalHub.Domain.Contracts;
using PersonalHub.Domain.Entities.Ledger;

namespace PersonalHub.Application.Services
{
    public class LedgerService(ILedgerRepository ledgerRepository)
    {
        private readonly ILedgerRepository _ledgerRepository = ledgerRepository;
        public IQueryable<TransactionDto> GetTransactions(short month, short year, CancellationToken ct = default)
        {
            return _ledgerRepository.GetTransactions(month, year, ct).Select(t => new TransactionDto(
                t.Id,
                t.Month,
                t.Year,
                t.Type,
                t.CategoryId,
                t.Description,
                t.Amount,
                t.CreatedAt));
        }
        public async Task<TransactionResponse> CreateTransaction(TransactionRequest request, CancellationToken ct = default)
        {
            var transaction = new Transaction(
                request.Month,
                request.Year,
                request.Id,
                request.Type,
                request.CategoryId,
                request.Description,
                request.Amount
            );
            await _ledgerRepository.CreateTransactionAsync(transaction, ct);
            return new TransactionResponse(
                new TransactionDto(
                    transaction.Id,
                    transaction.Month,
                    transaction.Year,
                    transaction.Type,
                    transaction.CategoryId,
                    transaction.Description,
                    transaction.Amount,
                    transaction.CreatedAt
                ),
                await _ledgerRepository.GetTotalByMonthAsync(request.Month, request.Year, ct)
            );
        }
        public async Task DeleteTransaction(Guid id, CancellationToken ct = default)
        {
            await _ledgerRepository.DeleteTransactionAsync(id, ct);
        }
        public async Task<TransactionResponse> UpdateTransaction(TransactionRequest request, CancellationToken ct = default)
        {
            var transaction = await _ledgerRepository.GetTransactionByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Transaction not found");
            transaction.Update(request.Type, request.CategoryId, request.Description, request.Amount);
            await _ledgerRepository.SaveChanges(ct: ct);
            return new TransactionResponse(
                new TransactionDto(
                    transaction.Id,
                    transaction.Month,
                    transaction.Year,
                    transaction.Type,
                    transaction.CategoryId,
                    transaction.Description,
                    transaction.Amount,
                    transaction.CreatedAt
                ),
                await _ledgerRepository.GetTotalByMonthAsync(transaction.Month, transaction.Year, ct)
            );
        }
    }
}
