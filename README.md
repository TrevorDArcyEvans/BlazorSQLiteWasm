# Blazor + SQLite + WebAssembly

## Prerequisites
* .NET Core 6
* Microsoft Visual Studio 2022
* Google Chrome
  * should work in other browsers which support wasm

**Debugging of wasm does not work on _JetBrains Rider 2021.3_**

## Getting Started

<details>

### Building

```bash
$ dotnet build
```

### Debugging
* open _BlazorSQLiteWasm.sln_ in _Visual Studio 2022_
* _F5_ to run will open a browser and load wasm

</details>

## Further Information
* [Sqlite database for WebAssembly](https://github.com/unoplatform/Uno.Samples/tree/master/UI/SQLiteSample)
* [BlazeOrbital](https://github.com/SteveSandersonMS/BlazeOrbital)
* [sqlite](https://github.com/cloudmeter/sqlite)
* [emscripten](https://emscripten.org/)
