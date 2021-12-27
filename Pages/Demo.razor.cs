namespace BlazorSQLiteWasm.Pages;

using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

public partial class Demo
{
  private async Task TestSQLite()
  {
    var db = new SqliteConnection("Data Source=DemoData.db");

    await db.OpenAsync();
  }
}
