using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLSanBong_API.Data;
using QLSanBong_API.Services;
using QLSanBong_API.Services.IService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext
builder.Services.AddDbContext<QlsanBongContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QlsanBongContext")));

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCors", builder =>
    {
        builder.WithOrigins("https://localhost:7198", "https://localhost:7182") // URL của ứng dụng client
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
// Cấu hình Authentication với JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],       // Cấu hình trong appsettings.json
        ValidAudience = builder.Configuration["Jwt:Audience"],   // Cấu hình trong appsettings.json
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Khóa từ appsettings.json
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole",
        policy => policy.RequireRole("Admin")); // Cần phải có vai trò admin

    options.AddPolicy("RequireEmployeeRole",
        policy => policy.RequireRole("NhanVien")); // Cần phải có vai trò nhân viên
    options.AddPolicy("RequireCustomerRole",
        policy => policy.RequireRole("KhachHang")); // Cần phải có vai trò nhân viên
});

builder.Services.AddHttpContextAccessor(); // Đăng ký IHttpContextAccessor
// Đăng ký các dịch vụ
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ISanBongService, SanBongService>();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<IGiaGioThueService, GiaGioThueService>();
builder.Services.AddScoped<IPhieuDatSanService, PhieuDatSanService>();
builder.Services.AddScoped<IYeuCauDatSanService, YeuCauDatSanService>();

// Đăng ký dịch vụ MVC
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "QLSanBong API", Version = "v1" });


    // Cấu hình để sử dụng Bearer Token
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập Bearer token vào đây.",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLSanBong API v1");
        c.RoutePrefix = string.Empty; // Đặt Swagger ở trang chính
    });
}

// Áp dụng chính sách CORS
app.UseCors("MyCors");
app.UseHttpsRedirection();

// Sử dụng Authentication và Authorization
app.UseAuthentication(); // Quan trọng: Sử dụng authentication trước authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
