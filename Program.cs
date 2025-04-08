
using CC_Karriarpartner.Data;
using CC_Karriarpartner.Endpoints.AdminPanelEndpoints;
using CC_Karriarpartner.Endpoints.LoginEndpoints;
using CC_Karriarpartner.Endpoints.SearchEndpoints;
using CC_Karriarpartner.Endpoints.UserEndpoints;
using CC_Karriarpartner.Services.AdminPanel;
using CC_Karriarpartner.Services.AuthServices;
using CC_Karriarpartner.Services.IAdminServices;
using CC_Karriarpartner.Services.IUserServices;
using CC_Karriarpartner.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace CC_Karriarpartner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Rmove later
            }).AddJwtBearer(options =>
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

            builder.Services.AddAuthorizationBuilder().AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin")); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Testing only
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "CC Karriärpartner API",
                    Version = "v1"
                });

                // Lägg till autentiseringsstöd i Swagger UI
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            // Scoped services
            builder.Services
                .AddScoped<IUserService, UserRegisterService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IAdminPanel, AdminPanelService>();

            builder.Services.AddDbContext<KarriarPartnerDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
 
            builder.Services.AddRateLimiter(option =>
            {
                option.AddPolicy("login", httpcontext => RateLimitPartition.GetFixedWindowLimiter( // Rate limiting for login endpoint
                    partitionKey: httpcontext.Connection.RemoteIpAddress?.ToString() ?? "UNKNOWN",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true, // Enable auto-replenishment so that the rate limiter will automatically replenish the limit
                        PermitLimit = 5, // Allow 5 requests
                        QueueLimit = 0, // No queue limit so requests are rejected if limit is reached and give a 429 code
                        Window = TimeSpan.FromMinutes(10) // Only 5 requests per 10 minutes

                    }));
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
            app.UseRateLimiter();
            app.UseStaticFiles();

            LoginEndpoint.LoginEndpointAsync(app);
            UserRegisterEndpoint.RegisterUserEndpoints(app);
            CourseSearchEndpoint.RegisterCourseSearchEnpoints(app);
            TemplateSearchEndpoint.RegisterTemplateSearchEndpoint(app);
            GetPurchasesEndpoint.PurchasesEndpoints(app);

            app.Run();
        }
    }
}
