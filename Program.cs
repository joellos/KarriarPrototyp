
using CC_Karriarpartner.Data;
using CC_Karriarpartner.Endpoints.UserEndpoints;
using CC_Karriarpartner.Services.AuthServices;
using CC_Karriarpartner.Services.IUserServices;
using CC_Karriarpartner.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CC_Karriarpartner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Scoped services
            builder.Services
                .AddScoped<IUserService, UserRegisterService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IAuthService, AuthService>();


            builder.Services.AddDbContext<KarriarPartnerDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Appsettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Appsettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Appsettings:Token")!)),
                    ValidateIssuerSigningKey = true
                };
            });

            var app = builder.Build();

            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseStaticFiles();

            LoginEndpoint.LoginEndpointAsync(app);
            UserRegisterEndpoint.RegisterUserEndpoints(app);

            app.Run();
        }
    }
}
