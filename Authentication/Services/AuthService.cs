using Authentication.Datas;
using Authentication.Models;
using Authentication.Models.Dtos;
using Authentication.Services.AuthInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Services
{
    public class AuthService : IAuth
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGenerator _tokenGenerator;
        public ResponseDto responseDto = new();

        public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenGenerator tokenGenerator, ResponseDto responseDto)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
            this.responseDto = responseDto;
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

        public async Task<object> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.NormalizedUserName == loginRequestDto.UserName.ToUpper());

                bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (isValid) 
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var jwtToken = _tokenGenerator.GenerateToken(user, roles);

                    responseDto.Message = "Sikeres belépés";
                    responseDto.Result = jwtToken;
                    return responseDto;
                }

                responseDto.Message = "Sikertelen belépés";
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
