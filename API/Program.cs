using HouseRentalApplication.Common.Interfaces.Auth;
using HouseRentalApplication.Common.Interfaces.Properties;
using HouseRentalDomain.Entities.Auth;
using HouseRentalInfrastructure.Data;
using HouseRentalInfrastructure.Services.Auth;
using HouseRentalInfrastructure.Services.PropertyService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("appCon")));

//Application DI(dependency Injection)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IImageService, FileSystemImageService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Authentication - JWT(recommended for API)
    var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//    if (!await roleMgr.RoleExistsAsync("Owner")) await roleMgr.CreateAsync(new IdentityRole("Owner"));
//    if (!await roleMgr.RoleExistsAsync("Tenant")) await roleMgr.CreateAsync(new IdentityRole("Tenant"));

//    // create sample owner if not exists
//    var ownerEmail = "owner@example.com";
//    var owner = await userMgr.FindByEmailAsync(ownerEmail);
//    if (owner == null)
//    {
//        owner = new ApplicationUser
//        {
//            UserID=1,
//            UserName = ownerEmail,
//            Email = ownerEmail,
//            FirstName = "Sample Owner",
//            IsOwner = true,
//            PhoneNumber = "01700000000"
//        };
//        var res = await userMgr.CreateAsync(owner, "P@ssword1!");
//        if (res.Succeeded) await userMgr.AddToRoleAsync(owner, "Owner");
//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.UseCors("MyPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
