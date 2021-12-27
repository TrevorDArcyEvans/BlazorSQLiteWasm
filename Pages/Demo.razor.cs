namespace BlazorSQLiteWasm.Pages;

using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

public partial class Demo
{
  private string _version = "unknown";

  private async Task SQLiteVersion()
  {
    await using var db = new SqliteConnection("Data Source=DemoData.db");
    await db.OpenAsync();
    await using var cmd = new SqliteCommand("SELECT SQLITE_VERSION()", db);
    _version = (await cmd.ExecuteScalarAsync())?.ToString();
  }
}
