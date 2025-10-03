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

            builder.Services.AddTransient<IServiceCategoriesRepository, EFServiceCategoriesRepository>();
            builder.Services.AddTransient<IServicesRepository, EFServicesRepository>();
            builder.Services.AddTransient<DataManager>();

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
                options.LoginPath = "/account/login";
                options.AccessDeniedPath= "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            //Подключаем функционал контролеров
            builder.Services.AddControllersWithViews();

            builder.Host.UseSerilog((context, configuration)=>configuration.ReadFrom.Configuration(context.Configuration));

            //Собираем конфигурацию
            WebApplication app = builder.Build();

            //Используем логирование
            app.UseSerilogRequestLogging();

            //Подключаем обработку исключений
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //Подключаем использование статичных файлов (js, css)
            app.UseStaticFiles();

            //Подключаем систему маршрутизации
            app.UseRouting();

            //Подключаем аутентификацию и авторизацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
           

            //Регистрируем нужные нам маршруты
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
           await app.RunAsync();
        }
    }
}
