namespace BlazorSQLiteWasm;

using Data;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pages;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

public static class Program
{
  /// <summary>
  /// FIXME: This is required for EF Core 6.0 as it is not compatible with trimming.
  ///
  /// For more information:
  ///   [.NET 6] Migrate API - Could not find method 'AddYears' on type 'System.DateOnly'
  ///   https://github.com/dotnet/efcore/issues/26860
  /// </summary>
  [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
  private static Type _keepDateOnly = typeof(DateOnly);

  public static async Task Main(string[] args)
  {
#if DEBUG
    // Allow some time for debugger to attach to Blazor framework debugging proxy
    await Task.Delay(TimeSpan.FromSeconds(2));
#endif

    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");

    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

    // Sets up EF Core with Sqlite
    builder.Services.AddDbContextFactory<ClientSideDbContext>(options =>
      options
        .UseSqlite($"Filename={Demo.SqliteDbFilename}")
        .EnableSensitiveDataLogging());

    await builder.Build().RunAsync();
  }
}
