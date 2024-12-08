using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Infrastructure.Persistance.Extensions
{
    public static class PropertyConfigurationExtensions
    {
        public static PropertyBuilder<T> ConvertListOfIds<T>(this PropertyBuilder<T> builder)
        {
            var converter = new ValueConverter<List<Guid>, string>(
                x => string.Join(',', x),
                x => x.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList()
            );

            var comparer = new ValueComparer<List<Guid>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );

            builder.HasConversion(converter,comparer);
            return builder;
        }
    }
}
