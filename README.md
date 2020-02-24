# Blazor WebAssembly (client-side) LoadingBar [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.LoadingBar.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.LoadingBar/)

## Summary

This is a class library that inserts loading bar UI automatically into a client side Blazor WebAssembly application.

![movie.1](https://github.com/jsakamoto/Toolbelt.Blazor.LoadingBar/blob/master/.assets/movie-001.gif?raw=true)

This is a porting from [**angular-loading-bar**](https://github.com/chieffancypants/angular-loading-bar) (except spinner UI).

## Supported Blazor versions

"Blazor WebAssembly App (client-side) LoadingBar" ver.10.x supports Blazor WebAssembly App version **3.2 Preview 1**.

## How to install and use?

**Step.1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.LoadingBar
```

**Step.2** Register "LoadingBar" service into the DI container, and declare contruct loading bar UI, at `Main()` method in the `Program` class of your Blazor application.

```csharp
using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Open namespace, and...
...
public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("app");
    builder.Services.AddLoadingBar(); // <-- register the service, and...

    await builder
        .Build()
        .UseLoadingBar() // <!-- declare construct loading bar UI.
        .RunAsync();
    ...
```

That's all.

After doing those 3 step, you can see a loading bar effect on your Blazor application UI, during HttpClient request going on.

### Configuration

The calling of `AddLoadingBar()` and `UseLoadingBar()` injects the references of JavaScript file (.js) and style sheet file (.css) - which are bundled with this package - into your page automatically.

If you don't want this behavior, you can disable these automatic injections, please call `AddLoadingBar()` with configuration action like this:

```csharp
builder.Services.AddLoadingBar(options =>
{
  // If you don't want automatic injection of js file, add below;
  options.DisableClientScriptAutoInjection = true;

  // If you don't want automatic injection of css file, add bllow;
  options.DisableStyleSheetAutoInjection = true;
});
```

You can inject those helper files manually. The URLs are below:

- **.js file** - _content/Toolbelt.Blazor.LoadingBar/script.min.js
- **.css file** - _content/Toolbelt.Blazor.LoadingBar/style.min.css

## Credits

Credit goes to [chieffancypants](https://github.com/chieffancypants) for his great works [**angular-loading-bar**](https://github.com/chieffancypants/angular-loading-bar).

This library includes many codes, style sheet definition, and algorithms derived from angular-loading-bar.

## Relese Note

- **v.10.0.0** - BREAKING CHANGE: Support Blazor v.3.2.0 Preview 1 (not compatible with v.3.1.0 Preview 4 or before.)
- **v.9.0.0** - BREAKING CHANGE: Support Blazor v.3.1.0 Preview 4 (not compatible with v.3.1.0 Preview 3 or before.)
- **v.8.0.0** - BREAKING CHANGE: Support Blazor v.3.1.0 Preview 3 (not compatible with v.3.1.0 Preview 2 or before.)
- **v.7.0.0** - BREAKING CHANGE: Support Blazor v.3.0.0 Preview 9 (not compatible with v.3.0.0 Preview 8 or before.)
- **v.6.0.0** - BREAKING CHANGE: Support Blazor v.3.0.0 Preview 6 (not compatible with v.3.0.0 Preview 5 or before.)
- **v.5.0.0** - BREAKING CHANGE: Support Blazor v.3.0.0 Preview 4 (not compatible with v.0.9.0 or before.)
- **v.4.0.0** - BREAKING CHANGE: Support Blazor v.0.9.0 (not compatible with v.0.8.0 or before.)
- **v.3.0.0** - BREAKING CHANGE: Support Blazor v.0.8.0 (not compatible with v.0.7.0 or before.)
- **v.2.1.0** - Support Blazor v.0.6.0 - it was signed strong name.
- **v.2.0.0** - BREAKING CHANGE: Fix namespace of LoadingBarExtension class.
- **v.1.0.0** - 1st release.

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.LoadingBar/blob/master/LICENSE)
