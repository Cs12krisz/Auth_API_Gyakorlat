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
        private readonly RoleManager<IdentityRole> _roleManager;
        public ResponseDto responseDto = new();

        public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<object> AssignRole(string userName, string roleName)
        {
            try
            {
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(user => user.NormalizedUserName == userName.ToUpper());

                if (user != null) 
                {
                    if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                    {
                        _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                    }


                    await _userManager.AddToRoleAsync(user, roleName);
                    responseDto.Message = "Sikeres hozzárendelés";
                    responseDto.Result = user;
                    return responseDto;
                }

                responseDto.Message = "Sikertelen hozzárendelés";
                return responseDto;

            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.Result = ex.HResult;
                return responseDto;
            }
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
