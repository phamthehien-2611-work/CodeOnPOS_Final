using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.Domain.Entities;

namespace UserManagementService.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<tbl_User?> GetUserByUserNameAsync(string username);
        Task<tbl_User?> GetUserByEmailAsync(string email);
        Task<tbl_User> AddAsync(tbl_User user);
    }
}
