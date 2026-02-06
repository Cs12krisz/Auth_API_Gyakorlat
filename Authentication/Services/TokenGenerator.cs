using Authentication.Models;
using Authentication.Services.AuthInterfaces;

namespace Authentication.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            throw new NotImplementedException();
        }
    }
}
