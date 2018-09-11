namespace Toolbelt.Blazor {

    export class LoadingBar {

        private loadingBarTemplate = '<div id="loading-bar"><div class="bar"><div class="peg"></div></div></div>';

        private loadingBarContainer: HTMLElement;

        private loadingBar: HTMLElement;

        private latencyThreshold: number = 100;

        constructor() {
            document.addEventListener('readystatechange', () => {
                if (document.readyState == 'interactive') {
                    document.body.insertAdjacentHTML('afterbegin', this.loadingBarTemplate);
                    this.loadingBarContainer = document.getElementById('loading-bar');
                    this.loadingBar = this.loadingBarContainer.getElementsByClassName('bar')[0] as HTMLElement;
                }
            });
        }

        public beforeSend(): void {
            console.log(`BEFORE SEND`);
        }

        public afterSend(): void {
            console.log(`AFTER SEND`);
        }

        public setWidth(pct: number): void {
            this.loadingBar.style.width = pct + '%';
            this.loadingBarContainer.className = 'active';
        }

        public complete(): void {
            this.loadingBarContainer.className = '';
            setTimeout(() => {
                this.loadingBar.style.width = '0';
            }, 350);
        }
    }

    export var loadingBar = new LoadingBar();
}
