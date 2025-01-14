using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using System.Windows.Forms;

namespace FirstToolWindow
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class FindServicesCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4131;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("95d716c7-31e3-460b-87d4-133ed42d75a0");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindServicesCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private FindServicesCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static FindServicesCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in FindServicesCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new FindServicesCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        //private void Execute(object sender, EventArgs e)
        //{
        //    SettingsManager settingsManager = new ShellSettingsManager((IServiceProvider)ServiceProvider);
        //    SettingsStore configurationSettingsStore = settingsManager.GetReadOnlySettingsStore(SettingsScope.Configuration);
        //    string message = "Available services:\n";
        //    IEnumerable<string> collection = configurationSettingsStore.GetSubCollectionNames("Services");
        //    int n = 0;
        //    foreach (string service in collection)
        //    {
        //        message += configurationSettingsStore.GetString("Services\\" + service, "Name", "Unknown") + "\n";
        //    }

        //    MessageBox.Show(message);
        //}

        private void Execute(object sender, EventArgs e)
        {
            SettingsManager settingsManager = new ShellSettingsManager((IServiceProvider)ServiceProvider);
            SettingsStore configurationSettingsStore = settingsManager.GetReadOnlySettingsStore(SettingsScope.Configuration);
            string helpServiceGUID = typeof(SVsHelpService).GUID.ToString("B").ToUpper();
            bool hasHelpService = configurationSettingsStore.CollectionExists("Services\\" + helpServiceGUID);
            string message = "Help Service Available: " + hasHelpService;

            var myServiceTask = ServiceProvider.GetServiceAsync(typeof(MyService));
            myServiceTask.Wait(TimeSpan.FromSeconds(10));
            MyService myService = (MyService)myServiceTask.Result;

            message += " and MyService is ok? " + myService.IsOK();

            MessageBox.Show(message);
        }
    }
}
