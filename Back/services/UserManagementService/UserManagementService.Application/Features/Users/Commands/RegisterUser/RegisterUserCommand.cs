using MediatR;

namespace UserManagementService.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<Guid>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int? TimeZone { get; set; }
}