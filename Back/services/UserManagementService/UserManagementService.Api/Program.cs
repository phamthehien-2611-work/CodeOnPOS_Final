using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserManagementService.Api.Services;
using UserManagementService.Application.Contracts.Infrastructure;
using UserManagementService.Application.Contracts.Persistence;
using UserManagementService.Infrastructure.Persistence.Data;
using UserManagementService.Infrastructure.Persistence.Repositories;
using UserManagementService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

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

// Configure the HTTP request pipeline.
app.MapGrpcService<UserService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
