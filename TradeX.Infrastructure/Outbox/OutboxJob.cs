using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Infrastructure.Migrations;
using TradeX.Infrastructure.Persistance;

namespace TradeX.Infrastructure.Outbox
{
    [DisallowConcurrentExecution]
    internal class OutboxJob : IJob
    {
        private readonly IPublisher _publisher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly OutboxOptions _outboxOptions;
        private readonly ILogger<OutboxJob> _logger;
        private readonly ApplicationDbContext _db;

        public OutboxJob(IPublisher publisher, IDateTimeProvider dateTimeProvider,
            IOptions<OutboxOptions> outboxOptions, ILogger<OutboxJob> logger, ApplicationDbContext context)
        {
            _publisher = publisher;
            _dateTimeProvider = dateTimeProvider;
            _outboxOptions = outboxOptions.Value;
            _logger = logger;
            _db = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Beginning to process outbox messages");  
            var outboxMessages = await GetOutboxMessagesAsync();

            if(outboxMessages.Count > 0)
            {
                using var transaction = _db.Database.BeginTransaction();

                foreach (var outboxMessage in outboxMessages)
                {
                    Exception? exception = null;
                    try
                    {
                        var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                            outboxMessage.Content,
                            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ContractResolver = new PrivateResolver() });

                        await _publisher.Publish(domainEvent!, context.CancellationToken);
                    }
                    catch (Exception caughtException)
                    {
                        _logger.LogError(caughtException, $"Exception while processing outbox message {outboxMessage.Id}");
                        exception = caughtException;
                    }

                    UpdateOutboxMessageAsync(outboxMessage, exception);
                }

                transaction.Commit();
            }

            _logger.LogInformation("Completed processing outbox messages");
        }

        private Task<List<OutboxMessage>> GetOutboxMessagesAsync()
        {     
            var outboxMessages = _db.Set<OutboxMessage>()
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(_outboxOptions.BatchSize)
                .ToListAsync();

            return outboxMessages;
        }

        private void UpdateOutboxMessageAsync(OutboxMessage outboxMessage,  Exception? exception)
        {
            outboxMessage.ProcessedOnUtc = _dateTimeProvider.UtcNow;
            outboxMessage.Error = exception?.Message;
            _db.Set<OutboxMessage>().Update(outboxMessage);
            _db.SaveChanges();
        }
    }
}
