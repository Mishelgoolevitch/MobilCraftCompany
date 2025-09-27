using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MobilCraftCompany.Domain;
using MobilCraftCompany.Infrastructure;
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
            //���������� ���������� �����������
            builder.Services.AddControllersWithViews();

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
                options.LoginPath = "/admin/login";
                options.AccessDeniedPath= "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            //�������� ������������
            WebApplication app = builder.Build();

            //���������� ������������� ��������� ������ (js, css)
            app.UseStaticFiles();

            //���������� ������� �������������
            app.UseRouting();

            //������������ ������ ��� ��������
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
           await app.RunAsync();
        }
    }
}
