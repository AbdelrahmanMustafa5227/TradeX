using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Cryptos;

namespace TradeX.Infrastructure.Persistance.Configurations
{
    internal class CryptoConfiguration : IEntityTypeConfiguration<Crypto>
    {
        public void Configure(EntityTypeBuilder<Crypto> builder)
        {
            builder.ToTable("Cryptos");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Symbol).HasMaxLength(5);

            builder.Property(x => x.Name);
            builder.Property(x => x.Price).HasPrecision(18, 4);
            builder.Property(x => x.PriceLast24H).HasPrecision(18, 4);
            builder.Property(x => x.TradingVolume24H).HasPrecision(18, 4);

        }
    }
}
