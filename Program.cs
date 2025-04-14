using CC_Karriarpartner.Data;
using CC_Karriarpartner.Endpoints.AdminPanelEndpoints;
using CC_Karriarpartner.Endpoints.GuestPurchaseEndpoints;
using CC_Karriarpartner.Endpoints.CourseEndpoints;
using CC_Karriarpartner.Endpoints.LoginEndpoints;
using CC_Karriarpartner.Endpoints.SearchEndpoints;
using CC_Karriarpartner.Endpoints.TemplateEndpoints;
using CC_Karriarpartner.Endpoints.UserEndpoints;
using CC_Karriarpartner.Services.AdminPanel;
using CC_Karriarpartner.Services.AuthServices;
using CC_Karriarpartner.Services.CourseServices;
using CC_Karriarpartner.Services.IAdminServices;
using CC_Karriarpartner.Services.ICourseServices;
using CC_Karriarpartner.Services.ITemplateServices;
using CC_Karriarpartner.Services.IUserServices;
using CC_Karriarpartner.Services.TemplateServices;
using CC_Karriarpartner.Services.UserServices;
using CC_Karriarpartner.Services.ValidationServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using DinkToPdf;
using DinkToPdf.Contracts;
using CC_Karriarpartner.Services.CustomAssembly;
using CC_Karriarpartner.Endpoints.GenerateDiplomaEndpoints;


namespace CC_Karriarpartner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // This is a comment.
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Remove later
            }).AddJwtBearer(options =>
            {
                var token = builder.Configuration.GetValue<string>("Appsettings:Token");
                if (string.IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Appsettings:Token", "Token value is missing in configuration.");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Appsettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Appsettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token)),
                    ValidateIssuerSigningKey = true
                };
                options.Events = new JwtBearerEvents // this override to look for tookens in cookies
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["accessToken"];
                        return Task.CompletedTask;
                    }
                };
            });

            // Laddar ner native DinkToPdf och Registrerar den
            var context = new CustomAssemblyLoadContext();
            var dllPath = Path.Combine(Directory.GetCurrentDirectory(), "DinkToPdfNative", "libwhtmltox.dll");
            context.LoadUnmanagedLibrary(dllPath);
            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

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
                .AddScoped<IAdminPanel, AdminPanelService>()
                .AddScoped<ITemplateService, TemplateService>()
                .AddScoped<ICourseService, CourseService>()
                .AddScoped<IValidationService, ValidationService>();


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

            //customisable error messages
            app.UseStatusCodePages(async statusCodeContext =>
            {
                var response = statusCodeContext.HttpContext.Response;

                if (response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    response.ContentType = "application/json";
                    await response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = 401,
                        message = "Not authorized to access this resource"
                    }));
                }
                else if (response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    response.ContentType = "application/json";
                    await response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = 403,
                        message = "You do not have permission to access this resource"
                    }));
                }
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseRateLimiter();
            app.UseStaticFiles();

            LoginEndpoint.LoginEndpointAsync(app);
            UserRegisterEndpoint.RegisterUserEndpoints(app);
            CourseSearchEndpoint.RegisterCourseSearchEnpoints(app);
            TemplateSearchEndpoint.RegisterTemplateSearchEndpoint(app);
            GetPurchasesEndpoint.PurchasesEndpoints(app);
            GuestPurchaseStartEndpoint.RegisterGuestPurchaseStartEndpoint(app);
            GuestPurchasePaymentEndpoint.RegisterGuestPurchasePaymentEndpoint(app);
            CourseDiplomaEndpoint.RegisterCourseDiplomaEndpoint(app);

            CourseEndpoint.RegisterCourseEndpoints(app);
            TemplateEndpoint.RegisterTemplateEndpoints(app);

            app.Run();
        }

    }
}
