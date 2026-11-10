# Development

- Edit frontend assets under `frontend/`.
- Edit AutoCAD integration under `backend/src/ArchDoorLibrary/`.
- JSON data lives under `data/`.
- Build via VS Code Tasks (`Ctrl+Shift+B` for default 2024 task; use Terminal > Run Task for 2023).
- Attach debugger to `acad.exe` after starting AutoCAD and NETLOADing the DLL.

Use the exact managed DLLs from the AutoCAD version being targeted. `Copy Local=False` prevents Autodesk assemblies from being copied into the plugin output.
