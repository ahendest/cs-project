using cs_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MySqlConnector;
using System.Data.Common;

namespace cs_project.Infrastructure.Data.Auditing
{
    public class MySqlAuditConnectionInterceptor(ICurrentUserAccessor userAccessor) : DbConnectionInterceptor
    {
        private readonly ICurrentUserAccessor _userAccessor = userAccessor;
        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            var cmd = connection.CreateCommand(); ;
            cmd.CommandText = @"
                SET @CurrentUserId = @uid;
                SET @AuditCorrelationId = @cid";
            cmd.Parameters.Add(new MySqlParameter("@uid", _userAccessor.GetCurrentUserId()));
            cmd.Parameters.Add(new MySqlParameter("@cid", Guid.NewGuid().ToString()));
            cmd.ExecuteNonQuery();

            base.ConnectionOpened(connection, eventData);
        }
    }
}
