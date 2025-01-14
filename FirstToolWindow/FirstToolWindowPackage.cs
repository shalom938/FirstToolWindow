using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FirstToolWindow
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(FirstToolWindowPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(ToolWindow1), Style = Microsoft.VisualStudio.Shell.VsDockStyle.Tabbed, Window = "3ae79031-e1bc-11d0-8f78-00a0c9110057")]
    [ProvideOptionPage(typeof(OptionPageGrid), "Digma Category", "Digma Page", 0, 0, true)]
    //OptionPageCustom doesn't work yet
    //[ProvideOptionPage(typeof(OptionPageCustom), "Digma Custom Category", "Digma Custom Page", 0, 0, true)]
    [ProvideService(typeof(MyService), IsAsyncQueryable = true)]

    public sealed class FirstToolWindowPackage : AsyncPackage
    {
        /// <summary>
        /// FirstToolWindowPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "62113088-801a-46dc-86da-c40e7f6c5deb";

        public int OptionInteger
        {
            get
            {
                OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
                return page.OptionInteger;
            }
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {

            this.AddService(typeof(MyService), CreateMyServiceAsync, true);
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await ToolWindow1Command.InitializeAsync(this);

            base.InitializeAsync(cancellationToken, progress);
            await MyToolsOptionsCommand.InitializeAsync(this);
            await OpenPageCommand.InitializeAsync(this);
            await SettingsStoreCommand.InitializeAsync(this);
            await FindServicesCommand.InitializeAsync(this);
            await Task.FromResult<object>(null);
        }

        private Task<object> CreateMyServiceAsync(IAsyncServiceContainer container, CancellationToken token, Type serviceType)
        {
            // Create and return the service instance
            return Task.FromResult<object>(new MyService());
        }

        #endregion


    }


    
}
