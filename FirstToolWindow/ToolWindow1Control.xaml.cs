//using CefSharp.Wpf;
using CefSharp;
using CefSharp.Wpf;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
//using CefSharp.DevTools.Autofill;

namespace FirstToolWindow
{
    /// <summary>
    /// Interaction logic for ToolWindow1Control.
    /// </summary>
    public partial class ToolWindow1Control : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1Control"/> class.
        /// </summary>
        public ToolWindow1Control()
        {
            this.InitializeComponent();

            //// Ensure CEF is initialized
            //var settings = new CefSettings();
            //if (!Cef.IsInitialized.HasValue || !Cef.IsInitialized.Value)
            //{
            //    Cef.Initialize(settings);
            //}

            //// Set the initial URL
            Browser.Address = "https://digma.ai/";

            //var browser = new ChromiumWebBrowser();
            //browser.LoadUrl("https://google.com");
            //this.Add(browser);
            //browser.Dock = DockStyle.Fill;

            //ChromiumWebBrowser chromeBrowser = new ChromiumWebBrowser("https://digma.ai/");
            //// Add it to the form and fill it to the form window.
            //this.WrapPanel1.Children.Add(chromeBrowser);
            //chromeBrowser.Dock = DockStyle.Fill;


        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "ToolWindow1");
        }


        public System.Windows.Controls.MediaElement MediaPlayer
        {
            get { return mediaElement1; }
        }

        //public StackPanel StackPanel1
        //{
        //    get { return stackPanel1; }
        //}
    }
}