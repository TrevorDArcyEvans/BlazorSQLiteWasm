namespace BlazorSQLiteWasm.Pages;

using Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public partial class Demo
{
  public const string SqliteDbFilename = "DemoData.db";

  private string _version = "unknown";

  private string _newBrand = "no brand";
  private int _newPrice = 999;

  private List<Car> _cars = new();

  [Inject]
  private IJSRuntime _js { get; set; }

  [Inject]
  private IDbContextFactory<ClientSideDbContext> _dbContextFactory { get; set; }

  protected override async Task OnInitializedAsync()
  {
    var module = await _js.InvokeAsync<IJSObjectReference>("import", "./dbstorage.js");

    if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
    {
      await module.InvokeVoidAsync("synchronizeFileWithIndexedDb", SqliteDbFilename);
    }

    await using var db = await _dbContextFactory.CreateDbContextAsync();
    await db.Database.EnsureCreatedAsync();

    if (!db.Cars.Any())
    {
      var cars = new[]
      {
        new Car { Brand = "Audi", Price = 21000 },
        new Car { Brand = "Volvo", Price = 11000 },
        new Car { Brand = "Range Rover", Price = 135000 },
        new Car { Brand = "Ford", Price = 8995 }
      };

      await db.Cars.AddRangeAsync(cars);
      await db.SaveChangesAsync();
    }

    _cars.AddRange(db.Cars);

    await base.OnInitializedAsync();
  }

  private async Task SQLiteVersion()
  {
    await using var db = new SqliteConnection($"Data Source={SqliteDbFilename}");
    await db.OpenAsync();
    await using var cmd = new SqliteCommand("SELECT SQLITE_VERSION()", db);
    _version = (await cmd.ExecuteScalarAsync())?.ToString();
  }

  private async Task Add(Car upCar)
  {
    var db = await _dbContextFactory.CreateDbContextAsync();
    await db.Cars.AddAsync(upCar);
    await db.SaveChangesAsync();
    _cars.Clear();
    _cars.AddRange(db.Cars);
    StateHasChanged();
  }

  private async Task Update(Car upCar)
  {
    var db = await _dbContextFactory.CreateDbContextAsync();
    var car = db.Cars.SingleOrDefault(c => c.Id == upCar.Id);
    car.Brand = upCar.Brand;
    car.Price = upCar.Price;
    db.Cars.Update(car);
    await db.SaveChangesAsync();
    _cars.Clear();
    _cars.AddRange(db.Cars);
    StateHasChanged();
  }

  private async Task Delete(int id)
  {
    var db = await _dbContextFactory.CreateDbContextAsync();
    var car = db.Cars.SingleOrDefault(c => c.Id == id);
    db.Cars.Remove(car);
    await db.SaveChangesAsync();
    _cars.Clear();
    _cars.AddRange(db.Cars);
    StateHasChanged();
  }
}
