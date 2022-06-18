using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace API 
{
  public class MigrationHelper
  {
    public static async Task Init(WebApplication app)
    {
      using (var scope = app.Services.CreateScope()) 
      {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
          var context = services.GetRequiredService<StoreContext>();
          await context.Database.MigrateAsync();
          await StoreContextSeed.SeedAsync(context, loggerFactory);
        }
        catch (Exception ex) 
        {
          var logger = loggerFactory.CreateLogger<MigrationHelper>();
          logger.LogError(ex,"An exception occured during migration");
        }
      }
    }
  }
}