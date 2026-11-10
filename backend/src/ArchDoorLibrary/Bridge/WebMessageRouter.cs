using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using ArchDoorLibrary.Services;
using System;

namespace ArchDoorLibrary.Bridge
{
    public static class WebMessageRouter
    {
        public static void Handle(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string requestId = Guid.NewGuid().ToString("N");
            JObject request = null;
            try
            {
                request = JObject.Parse(e.WebMessageAsJson);
                requestId = (string)request["requestId"] ?? requestId;
                string action = (string)request["action"];

                switch (action)
                {
                    case "GET_LIBRARY":
                        Send(sender, new { requestId, status = "ok", action,
                            data = LibraryService.ReadLibrary() });
                        break;

                    case "SAVE_DOOR":
                        var door = request["data"] as JObject;
                        if (door == null) throw new InvalidOperationException("Thiếu dữ liệu cửa.");
                        LibraryService.SaveDoor(door);
                        Send(sender, new { requestId, status = "ok", action,
                            message = "Đã lưu dữ liệu cửa vào thư viện project." });
                        break;

                    case "SELECT_SOURCE":
                        var doc = Application.DocumentManager.MdiActiveDocument;
                        if (doc == null) throw new InvalidOperationException("Không có bản vẽ đang mở.");
                        doc.SendStringToExecute("ADL_SELECTSOURCE ", true, false, false);
                        Send(sender, new { requestId, status = "ok", action,
                            message = "Đã gọi lệnh chọn đối tượng trong AutoCAD." });
                        break;

                    default:
                        throw new InvalidOperationException("Action không được hỗ trợ: " + action);
                }
            }
            catch (Exception ex)
            {
                Send(sender, new { requestId, status = "error",
                    message = ex.Message });
            }
        }

        private static void Send(object sender, object payload)
        {
            var webView = sender as CoreWebView2;
            if (webView != null)
                webView.PostWebMessageAsJson(JsonConvert.SerializeObject(payload));
        }
    }
}
