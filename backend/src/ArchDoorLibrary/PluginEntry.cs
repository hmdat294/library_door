using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Windows;
using System;

[assembly: CommandClass(typeof(ArchDoorLibrary.PluginEntry))]
[assembly: CommandClass(typeof(ArchDoorLibrary.Commands.SelectSourceCommand))]

namespace ArchDoorLibrary
{
    public class PluginEntry : IExtensionApplication
    {
        private static PaletteSet _palette;

        public void Initialize()
        {
            Application.DocumentManager.MdiActiveDocument?.Editor.WriteMessage(
                "\nArchDoorLibrary loaded. Run ADL to open the library palette.");
        }

        public void Terminate() { }

        [CommandMethod("ADL", CommandFlags.Modal)]
        public void ShowPalette()
        {
            if (_palette == null)
            {
                _palette = new PaletteSet("ArchDoorLibrary")
                {
                    Style = PaletteSetStyles.ShowAutoHideButton |
                            PaletteSetStyles.ShowCloseButton |
                            PaletteSetStyles.ShowPropertiesMenu,
                    MinimumSize = new System.Drawing.Size(420, 560),
                    Size = new System.Drawing.Size(520, 720)
                };
                _palette.Add("Door Library", new DoorPalette());
            }
            _palette.Visible = true;
        }
    }
}
