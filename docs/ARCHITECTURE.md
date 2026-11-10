# Architecture

```text
AutoCAD command ADL
  -> PaletteSet (WinForms)
      -> WebView2
          -> frontend/index.html (HTML/CSS/JS)
              -> window.chrome.webview.postMessage(...)
                  -> WebMessageRouter (C#)
                      -> LibraryService -> project/data/*.json
                      -> AutoCAD Editor commands / AutoCAD .NET API
```

JavaScript never calls AutoCAD APIs directly. It sends structured messages to C#. The bridge validates each action and returns a response containing the same `requestId`.

## Planned geometry pipeline
1. Select source entities in an existing DWG.
2. Extract geometry into normalized local coordinates.
3. Assign semantic member roles and profile codes.
4. Define anchors, constraints and resizing rules.
5. Generate the door from dimensions and arch parameters.
6. Draw profile cross-sections without scaling their extrusion dimensions.
7. Calculate glass, aluminum and accessory takeoff.
