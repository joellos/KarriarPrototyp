
using CC_Karriarpartner.Data;
using CC_Karriarpartner.Endpoints.SearchEndpoints;
using CC_Karriarpartner.Endpoints.UserEndpoints;
using CC_Karriarpartner.Services.IUserServices;
using CC_Karriarpartner.Services.UserServices;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddScoped<IUserService, UserRegisterService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddDbContext<KarriarPartnerDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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

            UserRegisterEndpoint.RegisterUserEndpoints(app);
            CourseSearchEndpoint.RegisterCourseSearchEnpoints(app);
            TemplateSearchEndpoint.RegisterTemplateSearchEndpoint(app);

            app.Run();
        }
    }
}
