using System.Data.Common;
using CoWorkApi.Infraestructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<AppDbContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);




            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {

                var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "database", "app.db");

                var databaseFolder = Path.GetDirectoryName(databasePath);
                if (!Directory.Exists(databaseFolder))
                {
                    Directory.CreateDirectory(databaseFolder ?? "");
                }

                if (!File.Exists(databasePath))
                {
                    Console.WriteLine("Database file not found. Creating a new database...");
                    // Create an empty database file
                    File.Create(databasePath).Dispose();
                }

                var connection = new SqliteConnection($"Data Source={databasePath}");
                connection.Open();

                return connection;
            });

            services.AddDbContext<AppDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });

        builder.UseEnvironment("Development");
    }
}