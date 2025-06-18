using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.GrpcContracts.User;
using System.Net;
using UserManagementService.Application.Features.Users.Commands.RegisterUser;
using UserManagementService.Application.Features.Users.Queries.Login;

namespace UserManagementService.Api.Services
{
    public class UserService : UserConfigService.UserConfigServiceBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserService> _logger;

        public UserService(IMediator mediator, ILogger<UserService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public override async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, ServerCallContext context)
        {
            try
            {
                var command = new RegisterUserCommand
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    FullName = request.FullName,
                    Email = request.Email,
                    Address = request.Address
                };
                var userId = await _mediator.Send(command);

                return new RegisterUserResponse
                {
                    Success = true,
                    Message = "User registered successfully.",
                    UserId = userId.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration.");
                // Trả về lỗi cho client
                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            try
            {
                var query = new LoginQuery
                {
                    UserName = request.UserName,
                    Password = request.Password
                };
                var token = await _mediator.Send(query);

                return new LoginResponse
                {
                    AccessToken = token
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed.");
                throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
            }
        }
    }
}
