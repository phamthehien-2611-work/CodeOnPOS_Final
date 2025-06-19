using MediatR;
using Microsoft.IdentityModel.Tokens;
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
        if (existingUserName != null)
        {
            throw new Exception($"Tên đăng nhập '{request.UserName}' đã tồn tại.");
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            var existingEmail = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                throw new Exception($"Email '{request.Email}' đã được sử dụng.");
            }
        }

        // 2. Mã hoá mật khẩu
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3. Tạo user mới
        var user = new tbl_User
        {
            UserId = Guid.NewGuid(),
            UserName = request.UserName,
            PasswordHash = passwordHash,
            SecurityKey = "",
            License = "",
            FullName = request.FullName,
            BirthDate = request.BirthDate,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address,
            TimeZone = request.TimeZone,
            IsActive = true,
            IsSystem = false,
            CreatedUser = "",
            CreatedTime = DateTime.UtcNow,
            LastChangedUser = "",
            LastChangedTime = DateTime.UtcNow
        };

        // 4. Gọi Repository để lưu vào DB
        var newUser = await _userRepository.AddAsync(user);

        // 5. Trả về response
        return newUser.UserId;
    }
}