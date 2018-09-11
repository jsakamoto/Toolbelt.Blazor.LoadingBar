var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var LoadingBar = /** @class */ (function () {
            function LoadingBar() {
                var _this = this;
                this.loadingBarTemplate = '<div id="loading-bar"><div class="bar"><div class="peg"></div></div></div>';
                this.latencyThreshold = 100;
                document.addEventListener('readystatechange', function () {
                    if (document.readyState == 'interactive') {
                        document.body.insertAdjacentHTML('afterbegin', _this.loadingBarTemplate);
                        _this.loadingBarContainer = document.getElementById('loading-bar');
                        _this.loadingBar = _this.loadingBarContainer.getElementsByClassName('bar')[0];
                    }
                });
            }
            LoadingBar.prototype.beforeSend = function () {
                console.log("BEFORE SEND");
            };
            LoadingBar.prototype.afterSend = function () {
                console.log("AFTER SEND");
            };
            LoadingBar.prototype.setWidth = function (pct) {
                this.loadingBar.style.width = pct + '%';
                this.loadingBarContainer.className = 'active';
            };
            LoadingBar.prototype.complete = function () {
                var _this = this;
                this.loadingBarContainer.className = '';
                setTimeout(function () {
                    _this.loadingBar.style.width = '0';
                }, 350);
            };
            return LoadingBar;
        }());
        Blazor.LoadingBar = LoadingBar;
        Blazor.loadingBar = new LoadingBar();
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
//# sourceMappingURL=Toolbelt.Blazor.LoadingBar.js.map