using Microsoft.EntityFrameworkCore;
using BackeEndAuthentication.Models;

using BackeEndAuthentication.Data;
using BackeEndAuthentication.DTO;
using BackeEndAuthentication.AuotoMapper;
using BackeEndAuthentication.Repository;
namespace BackeEndAuthentication.Repository
{
    public interface IUserRepository
    {
        Task AddAsync(Users user);
        Task<Users> GetUserByEmailAsync(string email);
        Task<Users> GetUserByPhoneNumber(string phoneNumber);
        Task<int> GetRoleIdByNameAsync(string roleName);
        Task SaveChangesAsync();

     
         AuthenticationDbContext GetDbContext();
    }
}
