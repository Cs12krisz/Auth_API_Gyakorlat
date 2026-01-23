using Authentication.Datas;
using Authentication.Models;
using Authentication.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Services
{
    public class AuthService : IAuth
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ResponseDto responseDto = new();

        public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        public async Task<object> Register(RegisterRequestDto registerRequestDto)
        {
			try
			{
                var user = new ApplicationUser
                {
                    UserName = registerRequestDto.UserName,
                    Email = registerRequestDto.Email,
                    FullName = registerRequestDto.FullName,
                };

                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

                if (result.Succeeded)
                {
                    var userReturn = await _context.ApplicationUsers.FirstOrDefaultAsync(user => user.UserName == registerRequestDto.UserName);

                    if (userReturn != null)
                    {
                        responseDto.Message = "Sikeres regisztráció";
                        responseDto.Result = userReturn;
                        return responseDto;
                    }
                }


                responseDto.Message = result.Errors.FirstOrDefault().Description;
                return responseDto;
			}
			catch (Exception ex)
			{
                responseDto.Message = ex.Message;
                responseDto.Result = ex.HResult;
                return responseDto;
			}
        }
    }
}
