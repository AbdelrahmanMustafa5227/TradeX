using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Subscriptions;

namespace TradeX.Infrastructure.Persistance.Configurations
{
    internal class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.subscriptionTier).HasConversion(x => (int)x, x => (SubscriptionTier)x);
            builder.Property(x => x.subscriptionPlan).HasConversion(x => (int)x, x => (SubscriptionPlan)x);

            builder.Property(x => x.UserId);
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.IsVaild);
            builder.Property(x => x.ComulativeTradingVolume24H).HasPrecision(18, 4);

            builder.OwnsMany(x => x.Alerts, b =>
            {
                b.ToTable("Alerts");
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedNever();
                b.WithOwner().HasForeignKey("SubscriptionId");

                b.Property(x => x.Price).HasPrecision(18, 4);
                b.Property(x => x.CryptoId);
                b.Property(x => x.IsActive);
            });


        }
    }
}
