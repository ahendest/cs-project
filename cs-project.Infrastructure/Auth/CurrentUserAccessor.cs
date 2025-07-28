using cs_project.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace cs_project.Infrastructure.Auth
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _http;

        public CurrentUserAccessor(IHttpContextAccessor http) => _http = http;

        public long GetCurrentUserId()
        {
            var claim = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return long.TryParse(claim?.Value, out var id) ? id : throw new UnauthorizedAccessException("User ID not found.");
        }
    }
}
