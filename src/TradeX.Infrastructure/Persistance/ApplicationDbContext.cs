using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Infrastructure.BackgroundJobs.Outbox;

namespace TradeX.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeProvider dateTimeProvider) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                AddOutboxMessages();
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException("Concurrency exception occurred.", ex);
            }
        }



        private void AddOutboxMessages()
        {
            var outboxMessages =  ChangeTracker.Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .SelectMany(x =>
                {
                    var events = x.GetDomainEvent();
                    x.ClearDomainEvents();
                    return events;
                })
                .Select(x => new OutboxMessage(
                    Guid.NewGuid(),
                    _dateTimeProvider.UtcNow,
                    x.GetType().Name,
                    JsonConvert.SerializeObject(x, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All})
                )).ToList();

            
            Set<OutboxMessage>().AddRange(outboxMessages);
        }
    }
}
