using PersonalHub.Domain.Entities.Security;
using System.Linq.Expressions;

namespace PersonalHub.Domain.Contracts
{
    public interface ISecurityRepository
    {
        Task<User> GetUser(Expression<Func<User, bool>> filter);
    }
}
