using MediatR;

namespace UserManagementService.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<Guid>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}