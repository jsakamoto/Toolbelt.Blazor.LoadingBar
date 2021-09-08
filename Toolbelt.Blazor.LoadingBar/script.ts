namespace Toolbelt.Blazor {
    const searchParam = document.currentScript?.getAttribute('src')?.split('?')[1] || '';
    const ready = import('./script.module.min.js?' + searchParam).then(m => {
        Object.assign(Blazor, m.Toolbelt.Blazor);
    });

    export function loadingBarReady(): Promise<void> { return ready; }
}
