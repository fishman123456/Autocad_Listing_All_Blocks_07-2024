
using Autodesk.AutoCAD;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Collections.Generic;
using Autocad_Listing_All_Blocks_07_2024;
namespace MyApplication
{
    public class DumpAttributes
    {
        [CommandMethod("LISTATT", CommandFlags.Redraw)]
        public void ListAttributes()
        {
            CheckDateWork.CheckDate();
            UserControl1 windowSeach;
            // проверяем на существования окна
            if (WinCloseTwo.countWin == 0)
            {
                windowSeach = new UserControl1();
                windowSeach.Show();
                windowSeach.Activate();
                WinCloseTwo.countWin = 1;
            }
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            var doc = Application.DocumentManager.MdiActiveDocument;
            Transaction tr = db.TransactionManager.StartTransaction();
            // Start the transaction

            try
            {
                // Build a filter list so that only
                // block references are selected
                TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.Start, "INSERT") };

                SelectionFilter filter = new SelectionFilter(filList);

                PromptSelectionOptions opts = new PromptSelectionOptions();

                opts.MessageForAdding = "Select block references: ";

                //PromptSelectionResult res = ed.GetSelection(opts, filter);
                PromptSelectionResult res = ed.SelectAll();
                // Do nothing if selection is unsuccessful
                if (res.Status != PromptStatus.OK)
                    return;

                SelectionSet selSet = res.Value;
                ObjectId[] idArray = selSet.GetObjectIds();

                // создаем список, потом в массив преобразуем и выделим в модели 24-07-2024
                List<ObjectId> pid = new List<ObjectId>();
                foreach (ObjectId blkId in idArray)
                {
                    BlockReference blkRef =
                      (BlockReference)tr.GetObject(blkId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead);
                    ed.WriteMessage("\nBlock: " + btr.Name);
                    btr.Dispose();
                    AttributeCollection attCol = blkRef.AttributeCollection;

                    foreach (ObjectId attId in attCol)
                    {
                        AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForRead);
                        if (attRef.TextString == WinCloseTwo.massSeach[0])
                        {
                            // добавляем id блока в список ---- повторяю блока а не аттрибута 24-07-2024 01-21 ночи
                            pid.Add(blkId);
                            //Add ObjectIds to the pid
                            SelectionSet ss1 = SelectionSet.FromObjectIds(pid.ToArray());
                            ed.SetImpliedSelection(ss1);
                            string str = ("\n Attribute Tag: " + attRef.Tag + "\n Attribute String: " + attRef.TextString);
                            ed.WriteMessage(str);
                        }
                    }
                }
                tr.Commit();

            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage(("Exception: " + ex.Message));
            }
            finally
            {
                //tr.Dispose();
            }
        }
    }
}


