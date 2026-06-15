using Microsoft.EntityFrameworkCore;
using PersonalHub.Domain.Entities.Ledger;
using static PersonalHub.Infrastructure.InfrastructureConstants;

namespace PersonalHub.Infrastructure.Mappings.SQLServer.Ledger
{
    internal class TransactionSQLServerConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions", "Ledger")
                .HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnType(SqlServerColumnTypes.NVARCHAR)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(t => t.UserId)
                .HasColumnType(SqlServerColumnTypes.INT)
                .HasColumnName("UserId")
                .IsRequired();

            builder.Property(t => t.Description)
                .HasColumnType(SqlServerColumnTypes.NVARCHAR)
                .HasColumnName("Description")
                .IsRequired();

            builder.Property(t => t.Amount)
                .HasColumnType(SqlServerColumnTypes.DECIMAL)
                .HasColumnName("Amount")
                .IsRequired();

            builder.Property(t => t.Month)
                .HasColumnType(SqlServerColumnTypes.SMALLINT)
                .HasColumnName("Month")
                .IsRequired();

            builder.Property(t => t.Year)
                .HasColumnType(SqlServerColumnTypes.INT)
                .HasColumnName("Year")
                .IsRequired();

            builder.Property(t => t.Type)
                .HasColumnType(SqlServerColumnTypes.INT)
                .HasColumnName("Type")
                .IsRequired();

            builder.Property(t => t.CategoryId)
                .HasColumnType(SqlServerColumnTypes.INT)
                .HasColumnName("CategoryId")
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .HasColumnType(SqlServerColumnTypes.DATETIME)
                .HasColumnName("CreatedAt")
                .IsRequired();
        }
    }
}
