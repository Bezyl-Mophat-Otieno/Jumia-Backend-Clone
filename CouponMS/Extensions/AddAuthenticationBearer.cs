
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CouponMS.Extensions
{
    public static class AddAuthenticationBearer
    {



        public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
        {
            // Working with the Bearer token for authentication
            builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    // What should be validated 
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,


                    // How the fields should be validated

                    ValidAudience = builder.Configuration.GetSection("JWToptions:Audience").Value,
                    ValidIssuer = builder.Configuration.GetSection("JWToptions:Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWToptions:SecretKey").Value))


                };
            });

            return builder;

        }
    }
}
