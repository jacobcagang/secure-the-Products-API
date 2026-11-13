
public interface IAuthService
{
    Task<RegisterResponseDto?> RegisterAsync(AuthRequestDto request);

    Task<AuthResponseDto?> LoginAsync(AuthRequestDto request);
}
