using Gecko;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snow.Edu
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            String XulStartUpPath = Application.StartupPath + @"\XulRunner";
            
            Xpcom.Initialize(XulStartUpPath);
            //GeckoWebBrowser.UseCustomPrompt();
            //Gecko.PromptFactory.PromptServiceCreator();
            //GeckoWebBrowser.UseCustomPrompt();
            //GeckoPreferences.User["browser.xul.error_pages.enabled"] = false;
            //Gecko.GeckoPreferences.User["network.proxy.http"] = host;
            //Gecko.GeckoPreferences.User["network.proxy.http_port"] = port;
            //Gecko.GeckoPreferences.User["network.proxy.ssl"] = host; 
            //Gecko.GeckoPreferences.User["network.proxy.ssl_port"] = port;
            //Gecko.GeckoPreferences.User["network.proxy.type"] = 1;

            Xpcom.Initialize(XULRunnerLocator.GetXULRunnerLocation());
            
            GeckoPreferences.User["browser.xul.error_pages.enabled"] = true;

            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true;

            GeckoPreferences.User["full-screen-api.enabled"] = true;

            Application.ApplicationExit += (sender, e) =>
            {
                Xpcom.Shutdown();
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new frmMain();
           

           // Gecko.LauncherDialog.Download += (s, e) => LauncherDialog_Download(mainForm, s, e);

           Application.Run(mainForm);
        }

        static void LauncherDialog_Download(IWin32Window owner, object sender, LauncherDialogEvent e)
        {
            uint flags = (uint)nsIWebBrowserPersistConsts.PERSIST_FLAGS_NO_CONVERSION |
                (uint)nsIWebBrowserPersistConsts.PERSIST_FLAGS_REPLACE_EXISTING_FILES |
                (uint)nsIWebBrowserPersistConsts.PERSIST_FLAGS_BYPASS_CACHE;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = e.Filename;
            if (sfd.ShowDialog(owner) == DialogResult.OK)
            {
                // the part that do the download, may be used for automation, or when the source URI is known, or after a parse of the dom :
                string url = e.Url;  //url to download
                string fullpath = sfd.FileName; //destination file absolute path
                nsIWebBrowserPersist persist = Xpcom.GetService<nsIWebBrowserPersist>("@mozilla.org/embedding/browser/nsWebBrowserPersist;1");
                nsIURI source = IOService.CreateNsIUri(url);
                nsIURI dest = IOService.CreateNsIUri(new Uri(fullpath).AbsoluteUri);
                persist.SetPersistFlagsAttribute(flags);
                persist.SaveURI(source, null, null, null, null, (nsISupports)dest, null);
                // file is saved - asynchronous call
                // need to try to have a temp name while the file is downloaded eg filename.ext.geckodownload (one of the SaveURI option)
            }
        }
    }
}
