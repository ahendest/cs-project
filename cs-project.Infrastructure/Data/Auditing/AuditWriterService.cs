using cs_project.Core.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;


namespace cs_project.Infrastructure.Data.Auditing
{
    public class AuditWriterService : BackgroundService
    {
        //private readonly IDbContextFactory<AppDbContext> _factory;

        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly TimeSpan _flushInterval = TimeSpan.FromSeconds(2);

        public static readonly Channel<AuditLog> Queue =
            Channel.CreateBounded<AuditLog>(new BoundedChannelOptions(1000)
            {
                SingleReader = true,
                FullMode = BoundedChannelFullMode.Wait
            });

        public AuditWriterService(IServiceScopeFactory scopeFactory)
            => _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var batch = new List<AuditLog>(capacity: 256);

            while (!stoppingToken.IsCancellationRequested)
            {
                while (Queue.Reader.TryRead(out var log))
                    batch.Add(log);

                if (batch.Count > 0)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    
                    db.AuditLogs.AddRange(batch);
                    await db.SaveChangesAsync(stoppingToken);
                    batch.Clear();
                }

                await Task.Delay(_flushInterval, stoppingToken);
            }
        }
    }
}
