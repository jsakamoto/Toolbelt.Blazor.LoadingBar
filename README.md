# Blazor WebAssembly (client-side) LoadingBar [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.LoadingBar.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.LoadingBar/)

## Summary

This is a class library that inserts loading bar UI automatically into a client side Blazor WebAssembly application.

![movie.1](https://github.com/jsakamoto/Toolbelt.Blazor.LoadingBar/blob/master/.assets/movie-001.gif?raw=true)

This is a porting from [**angular-loading-bar**](https://github.com/chieffancypants/angular-loading-bar) (except spinner UI).

Any HTTP requests to servers from HttpClient will cause appearing loading bar effect if the request takes over 100 msec.

## Supported Blazor versions

"Blazor WebAssembly App (client-side) LoadingBar" ver.12.x supports Blazor WebAssembly App version **3.2 Preview 2~5,  Release Candidates, and 3.2 Final Release!**

## How to install and use?

**Step.1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.LoadingBar
```

**Step.2** Register "LoadingBar" service into the DI container, and declare construct loading bar UI, at `Main()` method in the `Program` class of your Blazor application.

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
    ...

    await builder
        .Build()
        .UseLoadingBar() // <!-- declare construct loading bar UI.
        .RunAsync();
    ...
```

**Step.3** Add invoking `EnableIntercept(IServiceProvider)` extension method when registration of `HttpClient` as a transient service to DI container.

```csharp
public class Program
{
  public static async Task Main(string[] args)
  {
    ...
    builder.Services.AddTransient(sp => new HttpClient { 
      BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    }.EnableIntercept(sp)); // <- Add this!
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

## Release Notes

Release notes is [here.](https://github.com/jsakamoto/Toolbelt.Blazor.LoadingBar/blob/master/RELEASE-NOTES.txt)

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.LoadingBar/blob/master/LICENSE)
