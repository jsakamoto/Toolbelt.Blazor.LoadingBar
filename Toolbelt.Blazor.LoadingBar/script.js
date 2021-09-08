"use strict";
var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var _a, _b;
        const searchParam = ((_b = (_a = document.currentScript) === null || _a === void 0 ? void 0 : _a.getAttribute('src')) === null || _b === void 0 ? void 0 : _b.split('?')[1]) || '';
        const ready = import('./script.module.min.js?' + searchParam).then(m => {
            Object.assign(Blazor, m.Toolbelt.Blazor);
        });
        function loadingBarReady() { return ready; }
        Blazor.loadingBarReady = loadingBarReady;
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
