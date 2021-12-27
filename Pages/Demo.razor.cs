namespace BlazorSQLiteWasm.Pages;

using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

public partial class Demo
{
  private async Task TestSQLite()
  {
    var connStr = "Data Source=/local/MyData.db";
    var db = new SqliteConnection(connStr);
    await db.OpenAsync();
  }
}
