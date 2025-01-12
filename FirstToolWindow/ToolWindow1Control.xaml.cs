using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

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

            ThreadHelper.JoinableTaskFactory.Run(async delegate {
                InitializeWebViewAsync();
            });
        }


        private async void InitializeWebViewAsync()
        {
            webView = new WebView2
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            };

            await webView.EnsureCoreWebView2Async(null);
            //task.Wait(System.TimeSpan.FromSeconds(10));

            //webView.NavigationStarting += EnsureHttps;
            //webView.NavigationCompleted += NavigationCompleted;

            //webView.CoreWebView2.WebResourceRequested += WebResourceRequested;
            //webView.CoreWebView2.AddWebResourceRequestedFilter("https*",
            //    CoreWebView2WebResourceContext.All, CoreWebView2WebResourceRequestSourceKinds.All);

            webView.Source = new Uri("https://www.google.com");
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


        //public System.Windows.Controls.MediaElement MediaPlayer
        //{
        //    get { return mediaElement1; }
        //}

    }
}