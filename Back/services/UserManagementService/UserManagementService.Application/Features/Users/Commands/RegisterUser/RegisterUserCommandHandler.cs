using MediatR;
using UserManagementService.Application.Contracts.Persistence;
using UserManagementService.Domain.Entities;

namespace UserManagementService.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra username, email đã tồn tại chưa
        var existingUserName = await _userRepository.GetUserByUserNameAsync(request.UserName);
        var existingEmail = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUserName != null || existingEmail != null)
        {
            // Có thể trả về lỗi cụ thể hơn
            throw new Exception($"Username '{request.UserName}' already exists.");
        }

        // 2. Mã hoá mật khẩu
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3. Tạo user mới
        var user = new tbl_User
        {
            UserId = Guid.NewGuid(),
            UserName = request.UserName,
            PasswordHash = passwordHash,
            FullName = request.FullName,
            Email = request.Email,
            Address = request.Address,
            CreatedTime = DateTime.UtcNow
        };

        // 4. Gọi Repository để lưu vào DB
        var newUser = await _userRepository.AddAsync(user);

        // 5. Trả về response
        return newUser.UserId;
    }
}