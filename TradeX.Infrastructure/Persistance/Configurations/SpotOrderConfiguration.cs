using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.SpotOrders;

namespace TradeX.Infrastructure.Persistance.Configurations
{
    internal class SpotOrderConfiguration : IEntityTypeConfiguration<SpotOrder>
    {
        public void Configure(EntityTypeBuilder<SpotOrder> builder)
        {
            builder.ToTable("SpotOrders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.OrderType).HasConversion(x => (int)x, x => (SpotOrderType)x);


            builder.Property(x => x.UserId);
            builder.Property(x => x.CryptoId);
            builder.Property(x => x.Amount).HasPrecision(18, 4);
            builder.Property(x => x.EntryPrice).HasPrecision(18, 4);
            builder.Property(x => x.Fees).HasPrecision(18, 4);
            builder.Property(x => x.Total).HasPrecision(18, 4);
            builder.Property(x => x.ExecutedOnUtc);
        }
    }
}
