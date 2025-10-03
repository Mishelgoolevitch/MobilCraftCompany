using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MobilCraftCompany.Domain;
using MobilCraftCompany.Domain.Repositories.Abstract;
using MobilCraftCompany.Domain.Repositories.EntityFramework;
using MobilCraftCompany.Infrastructure;
using Serilog;
namespace MobilCraftCompany
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            //���������� � ������������ ���� appsettings
            IConfigurationBuilder configBuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //����������� ������ Project � ��������� �����
            IConfiguration configuration = configBuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            //���������� �������� ���� ������
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString)
            .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            builder.Services.AddTransient<IServiceCategoriesRepository, EFServiceCategoriesRepository>();
            builder.Services.AddTransient<IServicesRepository, EFServicesRepository>();
            builder.Services.AddTransient<DataManager>();

            //����������� Identity �������
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit=false;

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //����������� Auth cookie
            builder.Services.ConfigureApplicationCookie(options => 
            {
                options.Cookie.Name = "mobilCraftAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath= "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            //���������� ���������� �����������
            builder.Services.AddControllersWithViews();

            builder.Host.UseSerilog((context, configuration)=>configuration.ReadFrom.Configuration(context.Configuration));

            //�������� ������������
            WebApplication app = builder.Build();

            //���������� �����������
            app.UseSerilogRequestLogging();

            //���������� ��������� ����������
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //���������� ������������� ��������� ������ (js, css)
            app.UseStaticFiles();

            //���������� ������� �������������
            app.UseRouting();

            //���������� �������������� � �����������
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
           

            //������������ ������ ��� ��������
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
           await app.RunAsync();
        }
    }
}
