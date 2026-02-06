using Authentication.Models.Dtos;

namespace Authentication.Services.AuthInterfaces
{
    public interface IAuth
    {
        Task<object> Register(RegisterRequestDto registerRequestDto);
        Task<object> AssignRole(string userName, string RoleName);
        Task<object> Login(LoginRequestDto loginRequestDto);


    }
}
