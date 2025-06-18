using MediatR;
using UserManagementService.Application.Contracts.Infrastructure;
using UserManagementService.Application.Contracts.Persistence;

namespace UserManagementService.Application.Features.Users.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // 1. Lấy user từ DB
            var user = await _userRepository.GetUserByUserNameAsync(request.UserName);
            if (user == null)
            {
                throw new Exception("Invalid username or password."); // Unauthorized
            }

            // 2. Kiểm tra mật khẩu
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid username or password."); // Unauthorized
            }

            // 3. Tạo JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return token;
        }
    }
}