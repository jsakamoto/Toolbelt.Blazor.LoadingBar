using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Toolbelt.Blazor.LoadingBar
{
    public class ExampleJsInterop
    {
        public static Task<string> Prompt(string message)
        {
            // Implemented in exampleJsInterop.js
            return JSRuntime.Current.InvokeAsync<string>(
                "exampleJsFunctions.showPrompt",
                message);
        }
    }
}
