using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Users;
using TradeX.Infrastructure.Persistance.Extensions;

namespace TradeX.Infrastructure.Persistance.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.Email).HasMaxLength(50);

            builder.OwnsOne(x => x.balance , b =>
            {
                b.Property(x => x.TotalBalance).HasPrecision(18, 4);
                b.Property(x => x.FreezedBalance).HasPrecision(18, 4);
            });

            builder.OwnsOne(x => x.performanceMetrics , b =>
            {
                b.Property(x => x.TotalProfit).HasPrecision(18, 4);
                b.Property(x => x.TradingVolume).HasPrecision(18, 4);
            });


            builder.OwnsMany(x => x.Assets, assetsBuilder =>
            {
                assetsBuilder.ToTable("Assets");
                assetsBuilder.HasKey(x => x.Id);
                assetsBuilder.Property(x => x.Id).ValueGeneratedNever();
                assetsBuilder.WithOwner().HasForeignKey("UserId");
                assetsBuilder.Property(x => x.Amount).HasPrecision(18, 4);
                assetsBuilder.Property(x => x.Freezed).HasPrecision(18, 4);
            })
            .Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);



            builder.Property<List<Guid>>("_spotOrders")
            .HasColumnName("SpotOrderIds")
            .ConvertListOfIds();


            builder.Property<List<Guid>>("_futureOrders")
            .HasColumnName("FutureOrderIds")
            .ConvertListOfIds();
        }
    }
}
