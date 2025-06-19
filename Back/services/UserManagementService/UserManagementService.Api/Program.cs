using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserManagementService.Api.Services;
using UserManagementService.Application.Contracts.Infrastructure;
using UserManagementService.Application.Contracts.Persistence;
using UserManagementService.Infrastructure.Persistence.Data;
using UserManagementService.Infrastructure.Persistence.Repositories;
using UserManagementService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Định nghĩa một chính sách CORS cho phép client truy cập
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy
        (
            name: MyAllowSpecificOrigins,
            policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // Địa chỉ của server
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Grpc-Status", "Grpc-Message"); // Cho phép client đọc header lỗi của gRPC
                }
        );
});

// Thêm MediatR và chỉ định nó quét các handler trong project Application
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.Load("UserManagementService.Application")));

// Đăng ký Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

// Configure the database context
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Cấu hình HTTP request pipeline
app.UseRouting();

// Sử dụng CORS - PHẢI đặt trước UseGrpcWeb và MapGrpcService
app.UseCors(MyAllowSpecificOrigins);

// Kích hoạt middleware cho gRPC-Web - PHẢI đặt sau UseRouting và UseCors
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

// Configure the HTTP request pipeline. Áp dụng chính sách CORS cho service
app.MapGrpcService<UserService>()
   .RequireCors(MyAllowSpecificOrigins);

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
