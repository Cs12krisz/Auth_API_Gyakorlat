using Authentication.Models;

namespace Authentication.Services.AuthInterfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
