using BackeEndAuthentication.Models;
using Microsoft.EntityFrameworkCore;
using BackeEndAuthentication.Data;
using BackeEndAuthentication.DTO;
using BackeEndAuthentication.AuotoMapper;
using BackeEndAuthentication.Repository;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using BackeEndAuthentication.CustomExceptions;
namespace BackeEndAuthentication.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly AuthenticationDbContext _context;
        public UserRepository(AuthenticationDbContext context)
        {
            _context = context;
        }


        public AuthenticationDbContext GetDbContext()
        {
            return _context;
        }
        public async Task AddAsync(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            try
            {
                Console.WriteLine($"Getting user by email: {email}");

                var user = await _context.Users
                    .Include(u=>u.Role)
                    .FirstOrDefaultAsync(u => u.Email == email);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                Console.WriteLine($"STACKTRACE: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Users> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
        public async Task<int> GetRoleIdByNameAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
                throw new ConflictException($"Role '{roleName}' not found.");
            return role.RoleId;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

      

        
    }
}
