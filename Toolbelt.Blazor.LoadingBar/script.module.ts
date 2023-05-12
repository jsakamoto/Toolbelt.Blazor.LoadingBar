export namespace Toolbelt.Blazor {

    interface TimerHandle {
        handle: number | null;
    }

    export class LoadingBarClass {

        private loadingBarTemplate = '<div id="loading-bar" style="display:none;"><div class="bar" style="width:0;"><div class="peg"></div></div></div>';

        private loadingBarContainer: HTMLElement | null = null;

        private loadingBar: HTMLElement | null = null;

        private startSize: number = 0.02;

        private started: boolean = false;

        private progress: number = 0;

        private latencyThreshold: number = 100;

        private reqsTotal: number = 0;

        private reqsCompleted: number = 0;

        private startTimer: TimerHandle = { handle: null };

        private incrementTimer: TimerHandle = { handle: null };

        private completeTimer: TimerHandle = { handle: null };

        constructor() {
        }

        public constructDOM(barColor: string, cssPath: string, versionText: string, containerSelector: string | null | undefined, latencyThreshold: number): void {
            this.latencyThreshold = latencyThreshold;
            const doc = document;

            let cssAwaiter: Promise<void> | null = null;
            if (cssPath !== '') {
                const head = doc.head;
                let cssLinkElement: HTMLLinkElement | null = head.querySelector(`link[href^=\"${cssPath}\"]`);
                if (cssLinkElement === null) {
                    cssLinkElement = doc.createElement('link');
                    cssLinkElement.rel = 'stylesheet';
                    cssLinkElement.href = cssPath + '?v=' + versionText;
                    head.insertAdjacentElement('afterbegin', cssLinkElement);
                    const cle = cssLinkElement;
                    cssAwaiter = new Promise<void>(resolve => cle.onload = () => { resolve(); });
                }
            }
            if (cssAwaiter === null) cssAwaiter = new Promise<void>(resolve => resolve());

            doc.documentElement.style.setProperty('--toolbelt-loadingbar-color', barColor);

            const container = doc.querySelector(containerSelector || 'body');
            if (container === null) throw new Error('The container element could not found by selector "' + containerSelector + '"');
            container.insertAdjacentHTML('afterbegin', this.loadingBarTemplate);

            this.loadingBarContainer = doc.getElementById('loading-bar');
            if (this.loadingBarContainer != null) {
                this.loadingBar = this.loadingBarContainer.getElementsByClassName('bar')[0] as HTMLElement;
                const lbc = this.loadingBarContainer;
                cssAwaiter.then(() => { lbc.style.display = 'block'; });
            }
        }

        public beginLoading(): void {
            if (this.reqsTotal === 0) {
                this.setTimeout(this.startTimer, () => this.start(), this.latencyThreshold);
            }
            this.reqsTotal++;
            this.setProgress(this.reqsCompleted / this.reqsTotal);
        }

        public endLoading(): void {
            this.reqsCompleted++;
            if (this.reqsCompleted >= this.reqsTotal) {
                this.complete();
            } else {
                this.setProgress(this.reqsCompleted / this.reqsTotal);
            }
        }

        private start(): void {

            this.clearTimeout(this.completeTimer);
            if (this.started) return;
            this.started = true;

            // visible loading bar UI.
            if (this.loadingBar != null) this.loadingBar.classList.add('in-progress');

            this.setProgress(this.startSize, { enableTransition: false });
        }

        public complete(): void {
            this.reqsTotal = 0;
            this.reqsCompleted = 0;
            this.clearTimeout(this.startTimer);

            this.setProgress(1);

            // Attempt to aggregate any start/complete calls within 500ms:
            this.setTimeout(this.completeTimer, () => {
                if (this.loadingBar != null) this.loadingBar.classList.remove('in-progress');
                this.progress = 0;
                this.started = false;
            }, 500);
        }

        private setProgress(progress: number, option: { enableTransition: boolean } = { enableTransition: true }): any {
            if (!this.started) return;

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

        private incrementProgress(): any {
            if (this.progress >= 1) return;

            let randomDelta = 0;
            const currentProgress = this.progress;
            if (currentProgress >= 0 && currentProgress < 0.25) {
                // Start out between 3 - 6% increments
                randomDelta = (Math.random() * (5 - 3 + 1) + 3) / 100;
            } else if (currentProgress >= 0.25 && currentProgress < 0.65) {
                // increment between 0 - 3%
                randomDelta = (Math.random() * 3) / 100;
            } else if (currentProgress >= 0.65 && currentProgress < 0.9) {
                // increment between 0 - 2%
                randomDelta = (Math.random() * 2) / 100;
            } else if (currentProgress >= 0.9 && currentProgress < 0.99) {
                // finally, increment it .5 %
                randomDelta = 0.005;
            } else {
                // after 99%, don't increment:
                randomDelta = 0;
            }

            const newProgress = this.progress + randomDelta;
            this.setProgress(newProgress);
        }

        private setTimeout(timer: TimerHandle, handler: () => void, timeout: number): void {
            if (timer.handle !== null) clearTimeout(timer.handle);
            timer.handle = setTimeout(() => {
                timer.handle = null;
                handler();
            }, timeout);
        }

        private clearTimeout(timer: TimerHandle): void {
            if (timer.handle !== null) {
                clearTimeout(timer.handle);
                timer.handle = null;
            }
        }
    }

    export var loadingBar = new LoadingBarClass();
}
