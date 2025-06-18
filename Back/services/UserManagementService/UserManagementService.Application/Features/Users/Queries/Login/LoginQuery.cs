using MediatR;

namespace UserManagementService.Application.Features.Users.Queries.Login
{
    public class LoginQuery : IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}