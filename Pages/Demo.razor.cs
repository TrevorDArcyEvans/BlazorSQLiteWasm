namespace BlazorSQLiteWasm.Pages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Microsoft.Data.Sqlite;

public partial class Demo
{
  private string _version = "unknown";

  private string _newBrand = "no brand";
  private int _newPrice = 22;

  private List<Car> _cars = new()
  {
    new Car { Id = 0, Brand = "Audi", Price = 21000 },
    new Car { Id = 1, Brand = "Volvo", Price = 11000 },
    new Car { Id = 2, Brand = "Range Rover", Price = 135000 },
    new Car { Id = 3, Brand = "Ford", Price = 8995 }
  };

  private async Task SQLiteVersion()
  {
    await using var db = new SqliteConnection("Data Source=DemoData.db");
    await db.OpenAsync();
    await using var cmd = new SqliteCommand("SELECT SQLITE_VERSION()", db);
    _version = (await cmd.ExecuteScalarAsync())?.ToString();
  }

  private async Task Add(Car upCar)
  {
    var idx = _cars.Max(c => c.Id) + 1;
    upCar.Id = idx;
    _cars.Add(upCar);
    StateHasChanged();
  }

  private async Task Update(Car upCar)
  {
    var car = _cars.Single(c => c.Id == upCar.Id);
    car.Brand = upCar.Brand;
    car.Price = upCar.Price;
    StateHasChanged();
  }

  private async Task Delete(int id)
  {
    var car = _cars.SingleOrDefault(c => c.Id == id);
    var idx = _cars.IndexOf(car);
    _cars.RemoveAt(idx);
    StateHasChanged();
  }
}
