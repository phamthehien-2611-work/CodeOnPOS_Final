using System.Net;
using Grpc.Core;
using Shared.GrpcContracts.User;

namespace UserManagementService.Api.Services
{
    public class UserService : UserConfigService.UserConfigServiceBase
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
