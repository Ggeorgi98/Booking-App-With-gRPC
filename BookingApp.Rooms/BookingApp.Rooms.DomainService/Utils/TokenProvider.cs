using BookingApp.Rooms.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace BookingApp.Rooms.DomainService.Utils
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTokenAsync()
        {
            return _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        }
    }
}
