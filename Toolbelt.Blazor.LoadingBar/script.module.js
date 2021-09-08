export var Toolbelt;
(function (Toolbelt) {
    var Blazor;
    (function (Blazor) {
        class LoadingBarClass {
            constructor() {
                this.loadingBarTemplate = '<div id="loading-bar"><div class="bar"><div class="peg"></div></div></div>';
                this.loadingBarContainer = null;
                this.loadingBar = null;
                this.startSize = 0.02;
                this.started = false;
                this.progress = 0;
                this.latencyThreshold = 100;
                this.reqsTotal = 0;
                this.reqsCompleted = 0;
                this.startTimer = { handle: null };
                this.incrementTimer = { handle: null };
                this.completeTimer = { handle: null };
            }
            constructDOM(barColor) {
                document.documentElement.style.setProperty('--toolbelt-loadingbar-color', barColor);
                document.body.insertAdjacentHTML('afterbegin', this.loadingBarTemplate);
                this.loadingBarContainer = document.getElementById('loading-bar');
                if (this.loadingBarContainer != null) {
                    this.loadingBar = this.loadingBarContainer.getElementsByClassName('bar')[0];
                }
            }
            beginLoading() {
                if (this.reqsTotal === 0) {
                    this.setTimeout(this.startTimer, () => this.start(), this.latencyThreshold);
                }
                this.reqsTotal++;
                this.setProgress(this.reqsCompleted / this.reqsTotal);
            }
            endLoading() {
                this.reqsCompleted++;
                if (this.reqsCompleted >= this.reqsTotal) {
                    this.complete();
                }
                else {
                    this.setProgress(this.reqsCompleted / this.reqsTotal);
                }
            }
            start() {
                this.clearTimeout(this.completeTimer);
                if (this.started)
                    return;
                this.started = true;
                if (this.loadingBar != null)
                    this.loadingBar.classList.add('in-progress');
                this.setProgress(this.startSize, { enableTransition: false });
            }
            complete() {
                this.reqsTotal = 0;
                this.reqsCompleted = 0;
                this.clearTimeout(this.startTimer);
                this.setProgress(1);
                this.setTimeout(this.completeTimer, () => {
                    if (this.loadingBar != null)
                        this.loadingBar.classList.remove('in-progress');
                    this.progress = 0;
                    this.started = false;
                }, 500);
            }
            setProgress(progress, option = { enableTransition: true }) {
                if (!this.started)
                    return;
                this.progress = progress;
                if (this.loadingBar != null) {
                    if (option.enableTransition)
                        this.loadingBar.classList.remove('no-trans');
                    else
                        this.loadingBar.classList.add('no-trans');
                    this.loadingBar.style.width = (progress * 100) + '%';
                }
                this.setTimeout(this.incrementTimer, () => this.incrementProgress(), 250);
            }
            incrementProgress() {
                if (this.progress >= 1)
                    return;
                let randomDelta = 0;
                const currentProgress = this.progress;
                if (currentProgress >= 0 && currentProgress < 0.25) {
                    randomDelta = (Math.random() * (5 - 3 + 1) + 3) / 100;
                }
                else if (currentProgress >= 0.25 && currentProgress < 0.65) {
                    randomDelta = (Math.random() * 3) / 100;
                }
                else if (currentProgress >= 0.65 && currentProgress < 0.9) {
                    randomDelta = (Math.random() * 2) / 100;
                }
                else if (currentProgress >= 0.9 && currentProgress < 0.99) {
                    randomDelta = 0.005;
                }
                else {
                    randomDelta = 0;
                }
                const newProgress = this.progress + randomDelta;
                this.setProgress(newProgress);
            }
            setTimeout(timer, handler, timeout) {
                if (timer.handle !== null)
                    clearTimeout(timer.handle);
                timer.handle = setTimeout(() => {
                    timer.handle = null;
                    handler();
                }, timeout);
            }
            clearTimeout(timer) {
                if (timer.handle !== null) {
                    clearTimeout(timer.handle);
                    timer.handle = null;
                }
            }
        }
        Blazor.LoadingBarClass = LoadingBarClass;
        Blazor.loadingBar = new LoadingBarClass();
    })(Blazor = Toolbelt.Blazor || (Toolbelt.Blazor = {}));
})(Toolbelt || (Toolbelt = {}));