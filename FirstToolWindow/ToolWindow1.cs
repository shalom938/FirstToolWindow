using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

using System.ComponentModel.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Community.VisualStudio.Toolkit;

namespace FirstToolWindow
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("e8c0fc66-316d-4b44-ac37-98086f3e1766")]
    public class ToolWindow1 : ToolWindowPane
    {

        public ToolWindow1Control control;
        private WebView2 myWebView;


        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1"/> class.
        /// </summary>
        public ToolWindow1() : base(null)
        {
            this.Caption = "My ToolWindow 1";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            //this.Content = new ToolWindow1Control();

            //control = new ToolWindow1Control();
            //base.Content = control;
            //        this.ToolBar = new CommandID(new Guid(ToolWindow1Command.guidFirstToolWindowPackageCmdSet),
            //ToolWindow1Command.ToolbarID);
            //        this.ToolBarLocation = (int)VSTWT_LOCATION.VSTWT_TOP;




            myWebView = new WebView2
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            };
            this.Content = myWebView;
            InitWebView();
            
            
        }

        private async Task InitWebView()
        {

            myWebView.CreationProperties = new CoreWebView2CreationProperties();
            myWebView.CreationProperties.UserDataFolder = "C:\\Temp\\myfolder";

            await myWebView.EnsureCoreWebView2Async(null);

            myWebView.NavigationStarting += EnsureHttps;
            myWebView.NavigationCompleted += NavigationCompleted;



            myWebView.CoreWebView2.WebMessageReceived += ProcessWebMessage;

            myWebView.CoreWebView2.WebResourceRequested += WebResourceRequested;
            myWebView.CoreWebView2.AddWebResourceRequestedFilter("http*",
                CoreWebView2WebResourceContext.All, CoreWebView2WebResourceRequestSourceKinds.All);

            //myWebView.Source = new Uri("https://www.google.com");
            myWebView.Source = new Uri("https://main/index.html");
            
        }



        void ProcessWebMessage(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String messageFromWebView = args.TryGetWebMessageAsString();
            Console.Out.WriteLine("Got message from react: "+messageFromWebView);
            myWebView.CoreWebView2.PostWebMessageAsString("Got message from react : " + messageFromWebView);
            //webView.CoreWebView2.PostWebMessageAsJson("{\"message\": \"Got message from react "+messageFromWebView+"\"}");
            VS.StatusBar.ShowMessageAsync("digma ProcessWebMessage "+ messageFromWebView).FireAndForget();
        }

        private void WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            VS.StatusBar.ShowMessageAsync("digma WebResourceRequested "+ e.Request.Uri).FireAndForget();

            if (e.Request.Uri.EndsWith("main/index.html"))
            {

                e.Response = myWebView.CoreWebView2.Environment.CreateWebResourceResponse(
                    getIndex(),
                    (int)200,
                    "OK",   // HTTP status message
                    "Content-Type: text/html; charset=utf-8" // Headers
                );
            }

        }

        private Stream getIndex()
        {
            var index = File.ReadAllText("D:\\workspace\\digma\\embedded-react-samples-1.html");
            //string index = "<html>\r\n<head>\r\n  <title>Hello World React App</title>\r\n</head>\r\n<body>\r\n  <div id=\"root\"></div>\r\n  <!-- Include React -->\r\n  <script src=\"https://unpkg.com/react@18/umd/react.development.js\" crossorigin></script>\r\n  <script src=\"https://unpkg.com/react-dom@18/umd/react-dom.development.js\" crossorigin></script>\r\n  <!-- Include Babel Standalone -->\r\n  <script src=\"https://unpkg.com/babel-standalone@6/babel.min.js\"></script>\r\n  <!-- Your React Code -->\r\n  <script type=\"text/babel\">\r\n    function HelloWorld() {\r\n      return <h1>Hello World from react fun!</h1>;\r\n    }\r\n\r\n    ReactDOM.render(<HelloWorld />, document.getElementById('root'));\r\n\r\n<div>\r\n      <h1>React App</h1>\r\n      <button onClick={sendMessageToWinForms}>Send Message to Wpf</button>\r\n    </div>\r\n\r\n  </script>\r\n</body>\r\n</html>";
            //string index = "                <!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <title>Hello World React App</title>\r\n</head>\r\n<body>\r\n  <div id=\"root\"></div>\r\n  <!-- Include React -->\r\n  <script src=\"https://unpkg.com/react@18/umd/react.development.js\" crossorigin></script>\r\n  <script src=\"https://unpkg.com/react-dom@18/umd/react-dom.development.js\" crossorigin></script>\r\n  <!-- Include Babel Standalone -->\r\n  <script src=\"https://unpkg.com/babel-standalone@6/babel.min.js\"></script>\r\n  <!-- Your React Code -->\r\n  <script type=\"text/babel\">\r\n    function HelloWorld() {\r\n      return <h1>Hello World from react fun!</h1>;\r\n    }\r\n\r\n    ReactDOM.render(<HelloWorld />, document.getElementById('root'));\r\n  </script>\r\n</body>\r\n</html>";
            //string index = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <title>Hello World React App</title>\r\n</head>\r\n</html>";
            //string index = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <title>Hello World React App</title>\r\n</head>\r\n<body>\r\n                        <h1>Hello, World!</h1>\r\n                        <p>This is a custom HTML response served from WebView2.</p>\r\n                    </body>\r\n</html>";

            byte[] byteArray = Encoding.UTF8.GetBytes(index);

            // Create a MemoryStream from the byte array
            MemoryStream stream = new MemoryStream(byteArray);

            // Optionally, reset the position to the start of the stream
            stream.Position = 0;

            return stream;
        }



        private void NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            e.ToString();
        }


        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            Console.WriteLine($"Navigating to: {args.Uri}");
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                myWebView.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
                args.Cancel = true;
            }
        }
    }
}
