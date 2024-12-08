using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;

namespace TradeX.Infrastructure.Persistance.Configurations
{
    internal class FutureOrderConfiguration : IEntityTypeConfiguration<FutureOrder>
    {
        public void Configure(EntityTypeBuilder<FutureOrder> builder)
        {
            builder.ToTable("FutureOrders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Type).HasConversion(x => (int)x, x => (FutureOrderType)x);


            builder.Property(x => x.UserId);
            builder.Property(x => x.CryptoId);
            builder.Property(x => x.Amount).HasPrecision(18, 4);
            builder.Property(x => x.EntryPrice).HasPrecision(18, 4);
            builder.Property(x => x.ExitPrice).HasPrecision(18, 4);
            builder.Property(x => x.StopLossPrice).HasPrecision(18, 4);
            builder.Property(x => x.TakeProfitPrice).HasPrecision(18, 4);
            builder.Property(x => x.IsActive);
            builder.Property(x => x.Fees).HasPrecision(18, 4);
            builder.Property(x => x.Total).HasPrecision(18, 4);
            builder.Property(x => x.OpenedOnUtc);
            builder.Property(x => x.ClosedOnUtc);
        }
    }
}
