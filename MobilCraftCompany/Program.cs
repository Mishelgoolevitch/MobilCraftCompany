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

            //Подключаем в конфигурацию файл appsettings
            IConfigurationBuilder configBuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //Оборачиваем секцию Project в объектную форму
            IConfiguration configuration = configBuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            //Подключаем контекст базы данных
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString)
            .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));
            //Подключаем функционал контролеров
            builder.Services.AddControllersWithViews();

            //Настраиваем Identity систему
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit=false;

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //Настраиваем Auth cookie
            builder.Services.ConfigureApplicationCookie(options => 
            {
                options.Cookie.Name = "mobilCraftAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/admin/login";
                options.AccessDeniedPath= "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            //Собираем конфигурацию
            WebApplication app = builder.Build();

            //Подключаем использование статичных файлов (js, css)
            app.UseStaticFiles();

            //Подключаем систему маршрутизации
            app.UseRouting();

            //Регистрируем нужные нам маршруты
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
           await app.RunAsync();
        }
    }
}
