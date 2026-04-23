using E_Commerce.Common;
using E_Commerce.DAL.Data.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Commerce.BLL
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<UserLoginDto> _loginValidator;
        private readonly JwtSettings _jwtSettings;

        public AuthManager(UserManager<ApplicationUser> userManager, IValidator<RegisterDto> registerValidator, IValidator<UserLoginDto> loginValidator, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<GeneralResult> RegisterAsync(RegisterDto registerDto)
        {
            var validation = await _registerValidator.ValidateAsync(registerDto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());
                return GeneralResult.Failure(errors);
            }

            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                return GeneralResult.Failure("User with this email already exists.");
            }

            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToList());
                return GeneralResult.Failure(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToList());
                return GeneralResult.Failure(errors);
            }

            return GeneralResult.Success("Registered successfully.");
        }

        public async Task<GeneralResult<TokenDto>> LoginAsync(UserLoginDto userLoginDto)
        {
            var validation = await _loginValidator.ValidateAsync(userLoginDto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());
                return GeneralResult<TokenDto>.Failure(errors);
            }

            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                return GeneralResult<TokenDto>.Failure("Invalid email or password.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);
            if (!isPasswordValid)
            {
                return GeneralResult<TokenDto>.Failure("Invalid email or password.");
            }

            var token = await GenerateTokenAsync(user);

            return GeneralResult<TokenDto>.Success(token, "Login Successful.");
        }

        private async Task<TokenDto> GenerateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var keyBytes = Convert.FromBase64String(_jwtSettings.SecretKey);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);

            var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: expiry,
                    signingCredentials: creds
                );

            return new TokenDto(
                    new JwtSecurityTokenHandler().WriteToken(token),
                    _jwtSettings.DurationInMinutes
                );
        }
    }
}
