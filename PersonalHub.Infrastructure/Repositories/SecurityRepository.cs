using PersonalHub.Domain.Contracts;
using PersonalHub.Domain.Entities.Security;
using PersonalHub.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace PersonalHub.Infrastructure.Repositories
{
    public class SecurityRepository : Repository<PersonalHubContext>, ISecurityRepository
    {
        public SecurityRepository(PersonalHubContext dbContext) : base(dbContext) { }

        public Task<User> GetUser(Expression<Func<User, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
