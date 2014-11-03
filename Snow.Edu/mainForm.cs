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
using System.IO;

namespace Snow.Edu
{
    public partial class frmMain : Form
    {
        private GeckoWebBrowser _browser = new GeckoWebBrowser();
        private string _appPath = Application.StartupPath;

        public frmMain()
        {
            InitializeComponent();

            _browser_init();
        }
        string getFilePath(string filename) {
            return Path.Combine(_appPath, filename);
        }

        void _browser_init() { 
            this.Controls.Add(_browser);
            _browser.Dock = DockStyle.Fill;
            // 禁用弹出菜单
            _browser.NoDefaultContextMenu = true;
            // 禁用弹出窗口
            _browser.CreateWindow2 += _browser_CreateWindow2;
            //
            _browser.Navigating += _browser_Navigating;
            //
            _browser.DocumentCompleted += _browser_DocumentCompleted;
            //
            _browser.DomClick += _browser_DomClick;

            _browser.Navigate(getFilePath("index.html"));
        }

        void _browser_Navigating(object sender, Gecko.Events.GeckoNavigatingEventArgs e)
        {
            _browser.Navigating -= _browser_Navigating;
            _browser.AddMessageEventListener("applicationExit",
                                             ((string p) =>
                                              MessageBox.Show(String.Format("C# : Got Message '{0}' from javascript", p))));
        }

        void _browser_Validating(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _browser_DomClick(object sender, DomMouseEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;
            if (e.Target == null) return;

            var element = e.Target.CastToGeckoElement();

            GeckoHtmlElement clicked = element as GeckoHtmlElement;
            if (clicked == null) return;

            switch (clicked.Id)
            {
                case "applicationExit":
                    e.Handled = true;
                    this.Exit();
                    break;
                case "popup_message":
                    ShowHtmlMessage(clicked.TextContent);
                    break;
                default:
                    return;
            }

        }

        private void ShowHtmlMessage(string p)
        {
            MessageBox.Show(p, "来自页面的消息");
        }

        void _browser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            
        }
        /// <summary>
        /// 禁止弹出窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _browser_CreateWindow2(object sender, GeckoCreateWindow2EventArgs e)
        {
            e.Cancel = true;
        }

        void Exit()
        {
            _browser.Dispose();
            Application.Exit();
        }

    }
}
