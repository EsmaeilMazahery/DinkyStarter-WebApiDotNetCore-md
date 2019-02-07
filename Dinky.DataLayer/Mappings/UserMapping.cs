
using Dinky.Domain.Models;
using Dinky.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dinky.DataLayer.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(b => b.mobile)
                    .IsUnique()
                    .HasName("UI_Mobile");

            builder.HasIndex(b => b.userName)
                     .IsUnique()
                     .HasName("UI_Username");
        }
    }
}