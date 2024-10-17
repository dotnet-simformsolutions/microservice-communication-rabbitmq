using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using APIGetway;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((host, config) =>
            {
                config.AddJsonFile("ocelot.json",optional:false,reloadOnChange:true);
                config.AddJsonFile($"appsettings.json", false, false);
            }).UseStartup<Startup>();

}