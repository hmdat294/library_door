(() => {
  const $ = (id) => document.getElementById(id);
  const pending = new Map();

  function log(message) {
    $("log").textContent = `${new Date().toLocaleTimeString()}  ${message}\n` + $("log").textContent;
  }

  function send(action, data = {}) {
    const requestId = (crypto.randomUUID ? crypto.randomUUID() : `${Date.now()}-${Math.random()}`);
    const message = { requestId, action, data };
    if (!window.chrome || !window.chrome.webview) {
      log(`Không có AutoCAD WebView bridge. Action: ${action}`);
      return;
    }
    pending.set(requestId, action);
    window.chrome.webview.postMessage(message);
  }

  function renderLibrary(library) {
    const doors = Array.isArray(library.doors) ? library.doors : [];
    $("libraryStatus").textContent = `Có ${doors.length} bộ cửa trong thư viện project.`;
    $("doorList").innerHTML = "";
    if (!doors.length) {
      $("doorList").textContent = "Chưa có bộ cửa nào. Hãy lưu bản ghi mẫu bên dưới.";
      return;
    }
    doors.forEach((door) => {
      const item = document.createElement("div");
      item.className = "door-item";
      const title = document.createElement("strong");
      title.textContent = `${door.id || ""} · ${door.name || "Chưa đặt tên"}`;
      const path = document.createElement("small");
      path.textContent = door.path || "";
      item.append(title, path);
      $("doorList").appendChild(item);
    });
  }

  $("reloadBtn").addEventListener("click", () => send("GET_LIBRARY"));
  $("selectBtn").addEventListener("click", () => send("SELECT_SOURCE"));
  $("saveBtn").addEventListener("click", () => {
    const id = $("doorId").value.trim();
    if (!id) { log("Mã cửa không được để trống."); return; }
    send("SAVE_DOOR", { 
      id,
      name: $("doorName").value.trim(),
      units: "mm",
      dimensions: {
        width: Number($("doorWidth").value),
        height: Number($("doorHeight").value)
      },
      arch: { type: "circular", rise: 300, note: "Thông số minh họa; cần xác nhận theo bản vẽ nguồn." },
      structure: { leafCount: 2 },
      members: [],
      sourceGeometry: [],
      constraints: [],
      schemaVersion: 1
    });
  });

  if (window.chrome && window.chrome.webview) {
    window.chrome.webview.addEventListener("message", (event) => {
      const response = event.data || {};
      const action = pending.get(response.requestId) || response.action || "RESPONSE";
      pending.delete(response.requestId);
      if (response.status === "error") log(`Lỗi ${action}: ${response.message}`);
      else {
        if (action === "GET_LIBRARY") renderLibrary(response.data || {});
        log(response.message || `${action}: thành công`);
      }
    });
    $("connection").textContent = "Đã kết nối WebView";
    send("GET_LIBRARY");
  } else {
    $("connection").textContent = "Chạy ngoài AutoCAD";
    $("libraryStatus").textContent = "Mở giao diện qua lệnh ADL trong AutoCAD để kết nối dữ liệu.";
  }
})();
