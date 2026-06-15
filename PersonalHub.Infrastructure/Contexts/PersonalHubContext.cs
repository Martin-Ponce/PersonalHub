using Microsoft.EntityFrameworkCore;
using PersonalHub.Domain.Entities.Ledger;
using PersonalHub.Domain.Entities.Security;
using PersonalHub.Infrastructure.Helpers.Factories;

namespace PersonalHub.Infrastructure.Contexts
{
    public class PersonalHubContext : DbContext
    {
        public PersonalHubContext(DbContextOptions<PersonalHubContext> options) : base(options)
        {
        }
        internal DbSet<Transaction> Transactions { get; set; }
        internal DbSet<Summary> Summaries { get; set; }
        internal DbSet<User> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var strategy = DbManagerStrategyFactory.GetConfigurationStrategy(Database.ProviderName ?? throw new NotImplementedException("Invalid context configuration"));
            modelBuilder = strategy.ApplyConfigurations(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
