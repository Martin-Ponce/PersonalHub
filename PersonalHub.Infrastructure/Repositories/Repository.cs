using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PersonalHub.Domain.Contracts;
using System.Data;

namespace PersonalHub.Infrastructure.Repositories
{
    public class Repository<TContext> : IRepository where TContext : DbContext
    {
        protected readonly TContext _context;
        public Repository(TContext context)
        {
            _context = context;
        }
        public virtual async Task SaveChanges(string transactionMetadata = "", CancellationToken ct = default)
        {
            await _context.SaveChangesAsync();
        }
        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }
        public async Task CloseTransaction()
        {
            if (_context.Database.CurrentTransaction != null) await _context.Database.CurrentTransaction.DisposeAsync();
        }
    }
}
