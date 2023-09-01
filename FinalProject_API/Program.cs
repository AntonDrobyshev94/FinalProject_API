using FinalProject_API.ContextFolder;
using FinalProject_API.Data;
using Microsoft.AspNetCore;

namespace FinalProject_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var init = BuildWebHost(args);
            using (var scope = init.Services.CreateScope())
            {
                var s = scope.ServiceProvider;
                var c = s.GetRequiredService<DataContext>();
                DbInitializer.Initialize(c);
                DbInitializer.InitializeContacts(c);
            }
            init.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(log => log.AddConsole())
                .Build();
    }
}