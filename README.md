# Blazor WebAssembly (client-side) LoadingBar [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.LoadingBar.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.LoadingBar/)

## Summary

This is a class library that inserts loading bar UI automatically into a client side Blazor WebAssembly application.

![movie.1](https://raw.githubusercontent.com/jsakamoto/Toolbelt.Blazor.LoadingBar/master/.assets/movie-001.gif)

This is a porting from [**angular-loading-bar**](https://github.com/chieffancypants/angular-loading-bar) (except spinner UI).

Any HTTP requests to servers from HttpClient will cause appearing loading bar effect if the request takes over 100 msec.

The live demo site is here:

- [https://demo-blazor-loadingbar.azurewebsites.net/fetchdata](https://demo-blazor-loadingbar.azurewebsites.net/fetchdata)

## Supported Blazor versions

"Blazor WebAssembly App (client-side) LoadingBar" ver.12.x or later supports Blazor WebAssembly App versions below.

- .NET Core 3.1 / Blazor Wasm 3.2
- .NET 5.0, 6.0, 7.0 or later

## How to install and use?

**Step.1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.LoadingBar
```

**Step.2** Register the "LoadingBar" service into the DI container, and declare the construct loading bar UI at the startup of your Blazor WebAssembly application (usually, you will need to do it in the "Program.cs").

```csharp
using Toolbelt.Blazor.Extensions.DependencyInjection; // 👈 Open namespace, and...
...
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.AddLoadingBarService(); // 👈 register the service, and...
...
builder.UseLoadingBar(); // 👈 declare construct loading bar UI.
...
await builder.Build().RunAsync();
...
```

**Step.3** Add invoking `EnableIntercept(IServiceProvider)` extension method at the registration of `HttpClient` to DI container.

```csharp
...
builder.Services.AddScoped(sp => new HttpClient { 
  BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
}.EnableIntercept(sp)); // 👈 Add this!
...
```

That's all.

After doing those 3 steps, you can see a loading bar effect on your Blazor application UI, during an HttpClient request going on.

### Configuration

#### Configure the color of the loading bar

If you want to customize the color of the loading bar, please call `AddLoadingBarService()` with a configuration action like this:

```csharp
builder.Services.AddLoadingBarService(options =>
{
  // Specify the color of the loading bar
  // by CSS color descriptor.
  options.LoadingBarColor = "yellow";
});
```

And also, the color of the loading bar is defined as a [CSS variable](https://developer.mozilla.org/en-US/docs/Web/CSS/Using_CSS_custom_properties), and the variable name is `--toolbelt-loadingbar-color`.

So you can change the color of the loading bar anytime by using JavaScript like this:

```js
document.documentElement.style.setProperty('--toolbelt-loadingbar-color', '#ff00dc')
```

#### Configure the threshold time for the loading bar will appear

The threshold time to appearing the loading bar after sending an HTTP request is 100 msec by default. You can configure that threshold time by calling `AddLoadingBarService()` with a configuration action like this:

```csharp
builder.Services.AddLoadingBarService(options =>
{
  // Specify the threshold time for the loading bar will appear by msec.
  options.LatencyThreshold = 200;
});
```

#### Configure injection of CSS and JavaScript

The calling of `AddLoadingBarService()` and `UseLoadingBar()` injects the references of JavaScript file (.js) and style sheet file (.css) - which are bundled with this package - into your page automatically.

If you don't want this behavior, you can disable these automatic injections, please call `AddLoadingBarService()` with a configuration action like this:

```csharp
builder.Services.AddLoadingBarService(options =>
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

#### Configure the container element what the loading bar contents inject into

By default, the "Loading bar" injects its DOM contents to the inside of the body element.

If you want to specify the element where the "Loading Bar" 's contents are injected, you can do that by setting a query selector to the `ContainerSelector` option, like this.

```csharp
builder.Services.AddLoadingBarService(options =>
{
  options.ContainerSelector = "#selector-of-container";
});
```


## Credits

Credit goes to [chieffancypants](https://github.com/chieffancypants) for his great works [**angular-loading-bar**](https://github.com/chieffancypants/angular-loading-bar).

This library includes many codes, style sheet definition, and algorithms derived from angular-loading-bar.

## Release Notes

Release notes is [here.](https://github.com/jsakamoto/Toolbelt.Blazor.LoadingBar/blob/master/RELEASE-NOTES.txt)

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.LoadingBar/blob/master/LICENSE)
