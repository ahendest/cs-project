using cs_project.Core.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;


namespace cs_project.Infrastructure.Data.Auditing
{
    public class AuditWriterService : BackgroundService
    {
        private readonly IDbContextFactory<AppDbContext> _factory;
        private static readonly TimeSpan _flushInterval = TimeSpan.FromSeconds(2);

        public static readonly Channel<AuditLog> Queue =
            Channel.CreateBounded<AuditLog>(new BoundedChannelOptions(1000)
            {
                SingleReader = true,
                FullMode = BoundedChannelFullMode.Wait
            });

        public AuditWriterService(IDbContextFactory<AppDbContext> factory)
            => _factory = factory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var batch = new List<AuditLog>(capacity: 256);

            while (!stoppingToken.IsCancellationRequested)
            {
                while (Queue.Reader.TryRead(out var log))
                    batch.Add(log);

                if (batch.Count > 0)
                {
                    // Create a fresh DbContext instance per batch
                    await using var db = await _factory.CreateDbContextAsync(stoppingToken);
                    db.AuditLogs.AddRange(batch);
                    await db.SaveChangesAsync(stoppingToken);
                    batch.Clear();
                }

                await Task.Delay(_flushInterval, stoppingToken);
            }
        }
    }
}
