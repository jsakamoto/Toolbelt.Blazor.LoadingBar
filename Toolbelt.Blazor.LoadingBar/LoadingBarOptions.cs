namespace Toolbelt.Blazor
{
    /// <summary>
    /// Provides programmatic configuration for the loading bar.
    /// </summary>
    public class LoadingBarOptions
    {
        /// <summary>
        /// Gets or sets the flag that disables client script auto-injection.
        /// </summary>
        public bool DisableClientScriptAutoInjection { get; set; }

        /// <summary>
        /// Gets or sets the flag that disables style sheet auto-injection.
        /// </summary>
        public bool DisableStyleSheetAutoInjection { get; set; }

        /// <summary>
        /// Get or set the color of the loading bar by CSS color descriptor.
        /// </summary>
        public string LoadingBarColor { get; set; } = "#29d";
    }
}
