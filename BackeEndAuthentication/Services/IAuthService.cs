using BackeEndAuthentication.DTO;

namespace BackeEndAuthentication.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
    }
}
