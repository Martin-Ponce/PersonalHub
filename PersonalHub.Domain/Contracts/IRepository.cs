using System.Data;

namespace PersonalHub.Domain.Contracts
{
    public interface IRepository
    {
        Task SaveChanges(string transactionMetadata = "", CancellationToken ct = default);
        IDbTransaction BeginTransaction();
        Task CloseTransaction();
    }
}
