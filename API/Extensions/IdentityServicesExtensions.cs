using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions;
public static class IdentityServiceExtension
{
    const string ADMIN_ID = "1";
    const string STAFF_ID = "2";
    const string MEMBER_ID = "3";
    public static IServiceCollection IdentityServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
        {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        //pending, intend to use RoleEnum but not working as expected
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
                      policy.RequireClaim("RoleId", ADMIN_ID));
            options.AddPolicy("Staff", policy =>
                      policy.RequireClaim("RoleId", STAFF_ID));
            options.AddPolicy("Member", policy =>
                      policy.RequireClaim("RoleId", MEMBER_ID));
            options.AddPolicy("AdminAndStaff", policy =>
                        policy.RequireClaim("RoleId", ADMIN_ID, STAFF_ID));
        });
        return services;
    }

}
