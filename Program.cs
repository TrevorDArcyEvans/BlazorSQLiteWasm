using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorSQLiteWasm
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
#if DEBUG
      // Allow some time for debugger to attach to Blazor framework debugging proxy
      await Task.Delay(TimeSpan.FromSeconds(2));
#endif

      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("#app");

      builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

      SQLitePCL.Batteries.Init();

      await builder.Build().RunAsync();
    }
  }
}
