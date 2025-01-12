using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

using System.ComponentModel.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell.Interop;
using CefSharp.Wpf;

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

    //        control = new ToolWindow1Control();
    //        base.Content = control;
    //        this.ToolBar = new CommandID(new Guid(ToolWindow1Command.guidFirstToolWindowPackageCmdSet),
    //ToolWindow1Command.ToolbarID);
    //        this.ToolBarLocation = (int)VSTWT_LOCATION.VSTWT_TOP;


            ChromiumWebBrowser chromeBrowser = new ChromiumWebBrowser("https://digma.ai/");
            base.Content = chromeBrowser;
        }
    }
}
