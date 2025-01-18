# Blazor + EF Core + SQLite + WebAssembly

Showing how to integrate _SQLite_ into a _Blazor_ wasm app which runs completely
inside the browser!

![index](index.png)

## Demo
[BlazorSQLiteWasm](https://trevordarcyevans.github.io/BlazorSQLiteWasm/)

## Changes in this Fork
This fork addresses issues with the original project:
* .NET 6 supports the Module object in JS Interop, but .NET 8 and above do not
* .NET 8 and above use the Blazor.runtime object to provide access to the Mono and Emscripten APIs
* The Sqlite Nuget had to be updated for .NET 8+ compatibility
* The Javascript code for syncing the file actually *is** necessary, so I fixed it as well and kept it in
* More info about the .NET breaking changes can be found here: https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/9.0/legacy-apis

## Prerequisites
* .NET Core 8 or 9
* `wasm-tools` workload
  <details>
  
    ```bash
    $ sudo dotnet workload install wasm-tools
    ```
  </details>

* Microsoft Visual Studio 2022
* JetBrains Rider
* Visual Studio Code
* Google Chrome or Microsoft Edge
  * should work in other browsers which support wasm

## Getting Started

<details>

### Building

```bash
$ git clone https://github.com/TrevorDArcyEvans/BlazorSQLiteWasm.git
$ cd BlazorSQLiteWasm
$ dotnet build
$ dotnet run
```
open [BlazorSQLiteWasm](http://localhost:5000)

### Debugging
* open _BlazorSQLiteWasm.sln_ in _Visual Studio 2022_
* _F5_ to run will open a browser and load wasm

Debugging _should_ also work with the latest _JetBrains Rider_

</details>

## How It Works

<details>

This project is largely based on demo code from [BlazeOrbital](https://github.com/SteveSandersonMS/BlazeOrbital)
and requires a number of coordinated and cooperating parts.

### C# .NET
* application code
* Entity Framework Core
* EF Core SQlite provider

is all compiled into the final wasm file.

### Javascript
A small piece of [helper code](./wwwroot/dbstorage.js) is required to create an _SQLite_ database file in the browser.
This is called from C# application code via .NET-javascript interop, _IJSRuntime_.  The database file is created on a
per-user-per-url basis and is persistent between browser sessions.
<p/>

![sqlite-storage](sqlite-storage.png)

There is some additional code:

```javascript
    setInterval(() => {
      const path = `/${filename}`;
      if (FS.analyzePath(path).exists) {
        const mtime = FS.stat(path).mtime;
        if (mtime.valueOf() !== lastModifiedTime.valueOf()) {
          lastModifiedTime = mtime;
          const data = FS.readFile(path);
          db.result.transaction('Files', 'readwrite').objectStore('Files').put(data, 'file');
        }
      }
    }, 1000);
```

which runs every second.  This is an artefact from the original [BlazeOrbital](https://github.com/SteveSandersonMS/BlazeOrbital)
project which required the data to be synchronised every second. **This code is actually required otherwise all data can be lost on reload.**

### SQLite
_SQLite_ driver is provided by _SQLitePCLRaw.bundle_e_sqlite3_ nuget package and is linked
into the final wasm file.

### Schema updates
If more properties are added to _Car_ class, the application will throw EF Core exception.  This is because the class and
and underlying database schema are now mismatched.  The database needs to be rebuilt and, during testing and development, 
this can be done by running:

```csharp
await db.Database.EnsureDeletedAsync();
await db.Database.EnsureCreatedAsync();
```

For production, you could follow the guide
[here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime)

### Publishing
There are several workarounds required to successfully publish this:
* various options for _emscripten_
  * `AllowUnsafeBlocks`
  * `EmccExtraLDFlags`
* force all types to be included in wasm file
  * [ [.NET 6] Migrate API - Could not find method 'AddYears' on type 'System.DateOnly'](https://github.com/dotnet/efcore/issues/26860)

Once published, it can be served by any webserver capable of serving static content
eg python test server

```bash
$ dotnet publish
$ cd cd bin/Debug/net6.0/publish/wwwroot/
$ python3 -m http.server
```

 open [BlazorSQLiteWasm](http://localhost:8000)

Note that there are specific workarounds in [index.html](wwwroot/index.html) for
hosting on GitHub.io pages

</details>

## Further Information

<details>

* [BlazeOrbital](https://github.com/SteveSandersonMS/BlazeOrbital)
* [`WasmBuildNative`](https://github.com/dotnet/AspNetCore.Docs/issues/24443)
* [webassembly native dependencies](https://docs.microsoft.com/en-us/aspnet/core/blazor/webassembly-native-dependencies?view=aspnetcore-6.0)
* [Uno Platform based SQLitePCLRaw provider for WebAssembly](https://github.com/unoplatform/Uno.SQLitePCLRaw.Wasm)
* [How do I call SQLitePCL.Batteries.Init().?](https://stackoverflow.com/questions/50746465/how-do-i-call-sqlitepcl-batteries-init)
* [SQLite-net](https://github.com/praeclarum/sqlite-net)
* [Sqlite database for WebAssembly](https://github.com/unoplatform/Uno.Samples/tree/master/UI/SQLiteSample)
* [sqlite](https://github.com/cloudmeter/sqlite)
* [emscripten](https://emscripten.org/)

</details>

## Acknowledgements
* <div>Icons made by <a href="https://www.freepik.com" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>
* <div>Icons made by <a href="https://www.flaticon.com/authors/eucalyp" title="Eucalyp">Eucalyp</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>

