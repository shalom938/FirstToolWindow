using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstToolWindow
{

    [Guid("00000000-0000-0000-0000-000000000000")]
    [ComVisible(true)]
    public class OptionPageCustom : DialogPage
    {
        private string optionValue = "alpha";

        public string OptionString
        {
            get { return optionValue; }
            set { optionValue = value; }
        }

        protected override IWin32Window Window
        {
            get
            {
                UserControl1 page = new UserControl1();
                page.optionsPage = this;
                page.Initialize();
                return page.optionsPage;
            }
        }
    }


    [ComVisible(true)]
    internal class OptionPageGrid : DialogPage
    {

        private int optionInt = 256;
        private string optionString = "My string";

        [Category("Digma Category")]
        [DisplayName("My Integer Option display name")]
        [Description("My integer option descryption")]
        public int OptionInteger
        {
            get { return optionInt; }
            set { optionInt = value; }
        }

        [Category("Digma Category")]
        [DisplayName("My String Option display name")]
        [Description("My String option descryption")]
        public string OptionString
        {
            get { return optionString; }
            set { optionString = value; }
        }

    }
}
