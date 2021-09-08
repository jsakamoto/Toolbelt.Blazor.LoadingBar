using Microsoft.AspNetCore.Components;

namespace Toolbelt.Blazor
{
    internal class ScriptInjectorComponent : ComponentBase
    {
        [Inject] private LoadingBar LoadingBar { get; set; } = null!;

        protected override void OnInitialized()
        {
            this.LoadingBar.ConstructDOM();
        }
    }
}
