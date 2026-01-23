using Authentication.Models.Dtos;

namespace Authentication.Services
{
    public interface IAuth
    {
        Task<object> Register(RegisterRequestDto registerRequestDto);
    }
}
