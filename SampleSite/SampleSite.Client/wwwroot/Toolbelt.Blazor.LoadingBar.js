var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        var LoadingBarClass = /** @class */ (function () {
            function LoadingBarClass() {
                var _this = this;
                this.loadingBarTemplate = '<div id="loading-bar"><div class="bar"><div class="peg"></div></div></div>';
                this.startSize = 0.02;
                this.started = false;
                this.latencyThreshold = 100;
                this.reqsTotal = 0;
                this.reqsCompleted = 0;
                this.startTimer = { handle: null };
                this.incrementTimer = { handle: null };
                this.completeTimer = { handle: null };
                document.addEventListener('readystatechange', function () {
                    if (document.readyState === 'interactive') {
                        document.body.insertAdjacentHTML('afterbegin', _this.loadingBarTemplate);
                        _this.loadingBarContainer = document.getElementById('loading-bar');
                        _this.loadingBar = _this.loadingBarContainer.getElementsByClassName('bar')[0];
                    }
                });
            }
            LoadingBarClass.prototype.beforeSend = function () {
                var _this = this;
                if (this.reqsTotal === 0) {
                    this.setTimeout(this.startTimer, function () { return _this.start(); }, this.latencyThreshold);
                }
                this.reqsTotal++;
                this.setProgress(this.reqsCompleted / this.reqsTotal);
            };
            LoadingBarClass.prototype.afterSend = function () {
                this.reqsCompleted++;
                if (this.reqsCompleted >= this.reqsTotal) {
                    this.complete();
                }
                else {
                    this.setProgress(this.reqsCompleted / this.reqsTotal);
                }
            };
            LoadingBarClass.prototype.start = function () {
                this.clearTimeout(this.completeTimer);
                if (this.started)
                    return;
                this.started = true;
                // visible loading bar UI.
                this.loadingBar.classList.add('in-progress');
                this.setProgress(this.startSize, { enableTransition: false });
            };
            LoadingBarClass.prototype.complete = function () {
                var _this = this;
                this.reqsTotal = 0;
                this.reqsCompleted = 0;
                this.clearTimeout(this.startTimer);
                this.setProgress(1);
                // Attempt to aggregate any start/complete calls within 500ms:
                this.setTimeout(this.completeTimer, function () {
                    _this.loadingBar.classList.remove('in-progress');
                    _this.progress = 0;
                    _this.started = false;
                }, 500);
            };
            LoadingBarClass.prototype.setProgress = function (progress, option) {
                var _this = this;
                if (option === void 0) { option = { enableTransition: true }; }
                if (!this.started)
                    return;
                this.progress = progress;
                if (option.enableTransition)
                    this.loadingBar.classList.remove('no-trans');
                else
                    this.loadingBar.classList.add('no-trans');
                this.loadingBar.style.width = (progress * 100) + '%';
                this.setTimeout(this.incrementTimer, function () { return _this.incrementProgress(); }, 250);
            };
            LoadingBarClass.prototype.incrementProgress = function () {
                if (this.progress >= 1)
                    return;
                var randomDelta = 0;
                var currentProgress = this.progress;
                if (currentProgress >= 0 && currentProgress < 0.25) {
                    // Start out between 3 - 6% increments
                    randomDelta = (Math.random() * (5 - 3 + 1) + 3) / 100;
                }
                else if (currentProgress >= 0.25 && currentProgress < 0.65) {
                    // increment between 0 - 3%
                    randomDelta = (Math.random() * 3) / 100;
                }
                else if (currentProgress >= 0.65 && currentProgress < 0.9) {
                    // increment between 0 - 2%
                    randomDelta = (Math.random() * 2) / 100;
                }
                else if (currentProgress >= 0.9 && currentProgress < 0.99) {
                    // finally, increment it .5 %
                    randomDelta = 0.005;
                }
                else {
                    // after 99%, don't increment:
                    randomDelta = 0;
                }
                var newProgress = this.progress + randomDelta;
                this.setProgress(newProgress);
            };
            LoadingBarClass.prototype.setTimeout = function (timer, handler, timeout) {
                if (timer.handle !== null)
                    clearTimeout(timer.handle);
                timer.handle = setTimeout(function () {
                    timer.handle = null;
                    handler();
                }, timeout);
            };
            LoadingBarClass.prototype.clearTimeout = function (timer) {
                if (timer.handle !== null) {
                    clearTimeout(timer.handle);
                    timer.handle = null;
                }
            };
            return LoadingBarClass;
        }());
        Blazor.LoadingBarClass = LoadingBarClass;
        Blazor.loadingBar = new LoadingBarClass();
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));
//# sourceMappingURL=Toolbelt.Blazor.LoadingBar.js.map