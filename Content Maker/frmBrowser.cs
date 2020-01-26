using System;
using System.Windows.Forms;

namespace Content_Maker
{
    public partial class FrmBrowser : Form
    {
        public FrmBrowser()
        {
            InitializeComponent();

            GoTo("http://www.vorlof.com");
        }


        public void GoTo(string url)
        {
            webBrowser1.Navigate(url);
        }

        private void FrmBrowser_Load(object sender, EventArgs e)
        {

        }
    }
}
