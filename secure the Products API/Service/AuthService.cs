
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(
        IUserRepository users,
        IConfiguration configuration)
    {
        _users = users;
        _configuration = configuration;
    }

    public async Task<RegisterResponseDto?> RegisterAsync(
        AuthRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
            return null;

        var username = request.Username.Trim();

        var existing = await _users.GetByUsernameAsync(username);

        if (existing != null)
            return null;

        var user = new User
        {
            Username = username,
            Role = username.Equals(
                "admin",
                StringComparison.OrdinalIgnoreCase)
                ? "Admin"
                : "User"
        };

        user.PasswordHash = _hasher.HashPassword(
            user,
            request.Password
        );

        await _users.AddAsync(user);

        return new RegisterResponseDto(user.Id, user.Username);
    }

    public async Task<AuthResponseDto?> LoginAsync(
        AuthRequestDto request)
    {
        var user = await _users.GetByUsernameAsync(
            request.Username.Trim()
        );

        if (user == null)
            return null;

        var result = _hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
            return null;

        var jwt = _configuration.GetSection("Jwt");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwt["ExpiryMinutes"])
            ),
            signingCredentials: credentials
        );

        return new AuthResponseDto(
            new JwtSecurityTokenHandler().WriteToken(token),
            user.Username,
            user.Role
        );
    }
}
