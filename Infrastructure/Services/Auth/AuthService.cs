using HouseRentalApplication.Common.DTOs.Auth;
using HouseRentalApplication.Common.Interfaces.Auth;
using HouseRentalDomain.Entities.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace HouseRentalInfrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    Address = model.Address,
                    NIDNo = model.NIDNo,
                    NormalizedEmail=model.Email,
                    NormalizedUserName = model.Email,
                    IsOwner = model.IsOwner,
                    IsAdmin = model.IsAdmin
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                if (!await _roleManager.RoleExistsAsync("Owner")) await _roleManager.CreateAsync(new IdentityRole("Owner"));
                if (!await _roleManager.RoleExistsAsync("Tenant")) await _roleManager.CreateAsync(new IdentityRole("Tenant"));

                var role = model.IsOwner ? "Owner" : "Tenant";

                await AssignRoleAsync(user.Id, role);

                return new AuthResponseDTO
                {
                    Success = true,
                    Message = "Registration Successfully Done!!"
                };
            }
            catch(Exception ex)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = ex.Message
                };
            }

            
        }
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO model)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return new AuthResponseDTO { Success = false, Message = "Invalid credentials" };
                }

                string role = user.IsOwner == true ? "Owner" : "Tenant";
                // JWT generation
                var token = GenerateJwtToken(user);

                return new AuthResponseDTO
                {
                    Success = true,
                    Token = token,
                    Role = role,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<UniqueCheckResponseDTO> CheckUniqueFieldsAsync(UniqueCheckRequestDTO dto)
        {
            var response = new UniqueCheckResponseDTO();

            if (!string.IsNullOrEmpty(dto.Email))
            {
                var emailUser = await _userManager.FindByEmailAsync(dto.Email);
                response.EmailExists = emailUser != null;
            }

            if (!string.IsNullOrEmpty(dto.Phone))
            {
                var phoneUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Phone == dto.Phone);

                response.PhoneExists = phoneUser != null;
            }

            if (!string.IsNullOrEmpty(dto.NidNo))
            {
                var nidUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.NIDNo == dto.NidNo);

                response.NidExists = nidUser != null;
            }

            response.Message = "Validation completed";

            return response;
        }


        public async Task<bool> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiresMinutes = int.Parse(_config["Jwt:TokenExpiryMinutes"] ?? "60");

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim("UserID", user.UserID.ToString() ?? ""),
            new Claim("isOwner", user.IsOwner.ToString()),
            new Claim("UserName", user.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // add roles as claims
            var rolesTask = _userManager.GetRolesAsync(user);
            rolesTask.Wait();
            foreach (var r in rolesTask.Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddMinutes(expiresMinutes), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
