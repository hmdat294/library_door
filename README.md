# ArchDoorLibrary

Starter project for an AutoCAD 2023/2024 aluminum-door library.

## Stack
- AutoCAD .NET API, .NET Framework 4.8
- WinForms `PaletteSet` hosting WebView2
- HTML/CSS/JavaScript frontend
- JSON data stored inside this project

## Important
This is a runnable **foundation**, not a finished parametric door generator. The starter supports loading the palette, reading/writing the JSON library, and requesting entity selection from AutoCAD. Profile geometry, constraints, resizing, door generation and takeoff are intentionally the next implementation stage. Sample profile dimensions are illustrative only and are not verified manufacturer data.

## Prerequisites
1. AutoCAD 2023 and/or 2024 installed.
2. Visual Studio Build Tools with .NET desktop build tools.
3. .NET Framework 4.8 Developer Pack / Targeting Pack.
4. WebView2 Runtime (Evergreen).
5. VS Code.

## Configure AutoCAD references
The project expects the AutoCAD managed DLLs in one of these locations:
- `C:\Program Files\Autodesk\AutoCAD 2023`
- `C:\Program Files\Autodesk\AutoCAD 2024`

If your installation differs, set `AutoCAD2023Path` / `AutoCAD2024Path` in `.vscode/tasks.json` or pass the property to MSBuild.

## Build
Open this folder in VS Code. Choose one task:
- `Build: AutoCAD 2024`
- `Build: AutoCAD 2023`

The default output is `backend/src/ArchDoorLibrary/bin/Debug/ArchDoorLibrary.dll`.

You can also run in PowerShell from the project root (using the Build Tools 18 path you reported):

```powershell
& "C:\Program Files (x86)\Microsoft Visual Studio\18\BuildTools\MSBuild\Current\Bin\MSBuild.exe" .\ArchDoorLibrary.sln /restore /t:Build /p:Configuration=Debug /p:AutoCADVersion=2024
```

For AutoCAD 2023 change the last property to `/p:AutoCADVersion=2023`. If your actual `MSBuild.exe` is in a different subfolder, update `.vscode/tasks.json` accordingly.

## Load into AutoCAD
1. Build the DLL.
2. In AutoCAD, run `NETLOAD`.
3. Select `backend\src\ArchDoorLibrary\bin\Debug\ArchDoorLibrary.dll`.
4. Run command `ADL`.
5. The ArchDoorLibrary palette should open.
6. Click **Reload library** to read `data/library.json`.
7. Click **Select CAD objects** to start AutoCAD's selection command. Select objects and press Enter. The current starter reports selected handles in the AutoCAD command line; the next stage will persist normalized geometry and role assignments.

If WebView2 initialization fails, install the WebView2 Evergreen Runtime and restart AutoCAD.

## Data
- `data/library.json`: index of door records
- `data/doors/AD001/door.json`: illustrative door record
- `data/profiles/*.json`: illustrative profile schema/examples

Do not treat sample profile dimensions as actual Xingfa extrusion specifications.

## AutoCAD 2023/2024
Build separately against each AutoCAD installation. Do not load a DLL compiled against one AutoCAD release into the other release unless you have validated API compatibility. This starter uses the `AutoCADVersion` MSBuild property to choose managed references.

## Troubleshooting
- `MSB... AcMgd.dll not found`: correct the AutoCAD install path or pass `/p:AutoCAD2024Path="..."`.
- `NETLOAD` says a dependency is missing: ensure WebView2 Runtime is installed and the NuGet packages restored during build.
- `msbuild` not recognized: use the full path to `MSBuild.exe`, or add its `MSBuild\Current\Bin` directory to PATH.
- AutoCAD blocks loading: check `SECURELOAD` / trusted paths according to your organization's security policy; do not globally weaken security settings.
