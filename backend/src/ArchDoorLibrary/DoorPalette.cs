using Autodesk.AutoCAD.ApplicationServices;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArchDoorLibrary.Bridge;
using ArchDoorLibrary.Services;

namespace ArchDoorLibrary
{
    public class DoorPalette : UserControl
    {
        private readonly WebView2 _webView;
        private bool _initialized;

        public DoorPalette()
        {
            Dock = DockStyle.Fill;
            _webView = new WebView2 { Dock = DockStyle.Fill };
            Controls.Add(_webView);
            Load += async (s, e) => await InitializeWebViewAsync();
        }

        private async Task InitializeWebViewAsync()
        {
            if (_initialized) return;
            try
            {
                string root = ProjectPaths.FindProjectRoot();
                string frontend = Path.Combine(root, "frontend", "index.html");
                string userData = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ArchDoorLibrary", "WebView2");
                Directory.CreateDirectory(userData);

                var environment = await CoreWebView2Environment.CreateAsync(null, userData);
                await _webView.EnsureCoreWebView2Async(environment);
                _webView.CoreWebView2.WebMessageReceived += WebMessageRouter.Handle;
                _webView.CoreWebView2.Navigate(new Uri(frontend).AbsoluteUri);
                _initialized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo giao diện ArchDoorLibrary:\n" + ex.Message,
                    "ArchDoorLibrary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
