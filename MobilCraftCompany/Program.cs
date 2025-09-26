using MobilCraftCompany.Infrastructure;
namespace MobilCraftCompany
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            
            //Подключаем в конфигурацию файл appsettings
            IConfigurationBuilder configBuild=new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange:true)
                .AddEnvironmentVariables();
            
            //Оборачиваем секцию Project в объектную форму
            IConfiguration configuration = configBuild.Build();
            AppConfig config=configuration.GetSection("Project").Get<AppConfig>()!;

            //Подключаем функционал контролеров
            builder.Services.AddControllersWithViews();

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
