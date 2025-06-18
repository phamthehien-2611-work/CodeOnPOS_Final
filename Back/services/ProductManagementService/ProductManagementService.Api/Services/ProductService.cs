using System.Net;
using Grpc.Core;
using Shared.GrpcContracts.Product;

namespace ProductManagementService.Api.Services
{
    public class ProductService : ProductConfigService.ProductConfigServiceBase
    {
        private readonly IConfiguration _configuration;

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
