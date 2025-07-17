using AutoMapper;
using BackeEndAuthentication.Authentication;
using BackeEndAuthentication.DTO;
using BackeEndAuthentication.Helpers;
using BackeEndAuthentication.Repository;
using BackeEndAuthentication.Exceptions;

namespace BackeEndAuthentication.Services
{
    public class AuthService:IAuthService
    {
        private readonly JwtTokenGeneration _jwt;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;


        public AuthService(JwtTokenGeneration jwt, IUserRepository userRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _jwt = jwt;
            _userRepository = userRepository;
            _mapper = mapper;
            _env = env;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null || !user.IsActive)
            {
                //custom exetion which is writen in Excetion/invalidEmalExcetion fILE
                throw new InvalidEmailException();

            }
            // Account locked?
            if (user.LockOutEnd.HasValue && user.LockOutEnd.Value > DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Try Again Later!");
            }
            //Password Match

            if (!PasswordHelpers.VerifyPassword(dto.Password, user.PasswordHash))
            {
                //count failed login attepts
                user.FailedLoginAttempts += 1;
                //Lock account after 5 failed attempts
                if (user.FailedLoginAttempts >= 5)
                {
                    //login again after 2 minutes 
                    user.LockOutEnd = DateTime.UtcNow.AddMinutes(2);
                }
                await _userRepository.SaveChangesAsync();
                throw new InvalidPasswordException();
            }
            // if login is valid then Reset login attempts

            user.FailedLoginAttempts = 0;
            user.LastLoginAt = DateTime.UtcNow;
            //save all in databse 
            await _userRepository.SaveChangesAsync();

            var token = _jwt.GenerateToken(user);

            var responseDto = _mapper.Map<LoginResponseDTO>(user);
            if (_env.IsDevelopment())
            {
                responseDto.Token = token;

            }
            else
            {
                responseDto.Token = null;
            }
                return responseDto;

        }
    }
}