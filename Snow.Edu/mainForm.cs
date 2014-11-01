using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using System.Runtime.InteropServices;

namespace Snow.Edu
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            GeckoWebBrowser web = new GeckoWebBrowser();
            this.Controls.Add(web);
            web.Dock = DockStyle.Fill;
            // 禁用弹出菜单
            web.NoDefaultContextMenu = true;
            // 禁用弹出窗口
            web.CreateWindow2 += web_CreateWindow2;
            //
            web.DocumentCompleted += web_DocumentCompleted;

            web.EnableJavascriptDebugger();
            web.EnableConsoleMessageNotfication();

            web.Navigate(Application.StartupPath + @"\index.html");
            web.AddMessageEventListener("applicationExit", ((string s) => this.Exit(s)));
        }

        void web_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            //(sender as GeckoWebBrowser).AddMessageEventListener("applicationExit", ((string s) => this.Exit(s)));
        }

        void web_CreateWindow2(object sender, GeckoCreateWindow2EventArgs e)
        {
            e.Cancel = true;
        }

        void Exit(string s) {
            MessageBox.Show(s);
            Application.Exit();
        }

    }
}
