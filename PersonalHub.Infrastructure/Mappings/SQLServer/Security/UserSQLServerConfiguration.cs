using Microsoft.EntityFrameworkCore;
using PersonalHub.Domain.Entities.Security;
using static PersonalHub.Infrastructure.InfrastructureConstants;

namespace PersonalHub.Infrastructure.Mappings.SQLServer.Security
{
    internal class UserSQLServerConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "Security")
                .HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnType(SqlServerColumnTypes.NVARCHAR)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(t => t.Username)
                .HasColumnType(SqlServerColumnTypes.NVARCHAR)
                .HasColumnName("Username")
                .IsRequired();

            builder.Property(t => t.Password)
                .HasColumnType(SqlServerColumnTypes.NVARCHAR)
                .HasColumnName("Password")
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .HasColumnType(SqlServerColumnTypes.DATETIME)
                .HasColumnName("CreatedAt")
                .IsRequired();
        }
    }
}
