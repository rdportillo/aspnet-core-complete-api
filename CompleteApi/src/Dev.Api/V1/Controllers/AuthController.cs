using Dev.Api.Controllers;
using Dev.Api.DTO;
using Dev.Api.Extensions;
using Dev.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dev.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/account")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApiSettings _apiSettings;

        public AuthController(INotifier notifier,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<ApiSettings> apiSettings,
                              IUser user) : base(notifier, user)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _apiSettings = apiSettings.Value;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> RegisterUser(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (result.Succeeded)
            {
                // isPersistent as false to avoid remember logged user in the future, since the log in action is being done automatically
                await _signInManager.SignInAsync(user, isPersistent: false);
                return CustomResponse(await GenerateJwt(user.Email));
            }

            foreach (var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return CustomResponse(registerUserDto);
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult> LoginUser(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return CustomResponse(await GenerateJwt(loginUserDto.Email));
            }
            if (result.IsLockedOut)
            {
                NotifyError("User has been blocked due to sign-in failed attempts");
                return CustomResponse(loginUserDto);
            }

            NotifyError("Incorrect user or password");
            return CustomResponse(loginUserDto);
        }

        private async Task<LoginResponseDto> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await ConfigureUserClaims(user);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apiSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _apiSettings.Issuer,
                Audience = _apiSettings.ValidTo,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_apiSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return BuildLoginResponseObject(encodedToken, user, claims);
        }

        private static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<IEnumerable<Claim>> ConfigureUserClaims(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            return claims;
        }

        private LoginResponseDto BuildLoginResponseObject(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            var loginResponseDto = new LoginResponseDto
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_apiSettings.ExpirationHours).TotalSeconds,
                UserInformation = new UserInformationDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value })
                }
            };

            return loginResponseDto;
        }
    }
}
