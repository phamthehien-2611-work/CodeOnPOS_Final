using UserManagementService.Domain.Entities;

namespace UserManagementService.Application.Contracts.Infrastructure
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(tbl_User user);
    }
}