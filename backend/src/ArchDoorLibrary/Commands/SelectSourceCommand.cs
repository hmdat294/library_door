using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using System.Text;

namespace ArchDoorLibrary.Commands
{
    public class SelectSourceCommand
    {
        [CommandMethod("ADL_SELECTSOURCE", CommandFlags.Modal)]
        public void SelectSource()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            Editor ed = doc.Editor;
            var options = new PromptSelectionOptions
            {
                MessageForAdding = "\nChọn các đối tượng nguồn của bộ cửa: "
            };
            var result = ed.GetSelection(options);
            if (result.Status != PromptStatus.OK)
            {
                ed.WriteMessage("\nKhông có đối tượng nào được chọn.");
                return;
            }

            var sb = new StringBuilder();
            sb.Append("\nArchDoorLibrary - các đối tượng đã chọn:");
            using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
            {
                foreach (SelectedObject selected in result.Value)
                {
                    if (selected == null) continue;
                    Entity entity = tr.GetObject(selected.ObjectId, OpenMode.ForRead) as Entity;
                    if (entity == null) continue;
                    sb.AppendFormat("\nHandle={0}, Type={1}, Layer={2}",
                        entity.Handle, entity.GetType().Name, entity.Layer);
                }
                tr.Commit();
            }
            ed.WriteMessage(sb.ToString());
            ed.WriteMessage("\nBước tiếp theo: trích xuất hình học chuẩn hóa và gán vai trò/profile.");
        }
    }
}
