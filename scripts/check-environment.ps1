$ErrorActionPreference = "Continue"
Write-Host "=== ArchDoorLibrary environment check ==="
$msbuild = Get-Command msbuild -ErrorAction SilentlyContinue
if ($msbuild) { Write-Host "MSBuild: $($msbuild.Source)" } else { Write-Warning "msbuild is not in PATH. Use the full MSBuild.exe path or add its Bin folder to PATH." }
$paths = @(
  "C:\Program Files\Autodesk\AutoCAD 2023",
  "C:\Program Files\Autodesk\AutoCAD 2024",
  "C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8"
)
foreach ($p in $paths) { Write-Host ("{0} : {1}" -f $p, (Test-Path $p)) }
$wv = Get-Command msedgewebview2.exe -ErrorAction SilentlyContinue
if ($wv) { Write-Host "WebView2 executable: $($wv.Source)" } else { Write-Host "WebView2 runtime executable not found in PATH (this alone does not prove runtime is absent)." }
