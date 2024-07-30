
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
        [CommandMethod("U_83_LISTATT", CommandFlags.Redraw)]
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
                //TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.Subclass, "AcDbBlockReference") };

                TypedValue[] filList = new TypedValue[2]
                {   new TypedValue((int)DxfCode.Start, "INSERT"),
                    new TypedValue((int)DxfCode.HasSubentities, 1) };

                SelectionFilter filter = new SelectionFilter(filList);

                PromptSelectionOptions opts = new PromptSelectionOptions();

                opts.MessageForAdding = "Select block references: ";

                PromptSelectionResult res = ed.GetSelection(opts, filter);
                // PromptSelectionResult res = ed.selecti(opts, ObjectTypeAttribute);
                // PromptSelectionResult res = ed.SelectAll();
                // Do nothing if selection is unsuccessful
                if (res.Status != PromptStatus.OK)
                    return;

                SelectionSet selSet = res.Value;
                ObjectId[] idArray = selSet.GetObjectIds();
                // проверим по типу обькты
                foreach (var e in res.Value)
                {
                    //ed.WriteMessage(e.GetType().ToString() + "\n");
                }

                // создаем список, потом в массив преобразуем и выделим в модели 24-07-2024
                List<ObjectId> pid = new List<ObjectId>();
                foreach (ObjectId blkId in idArray)
                {
                    // проверка по типу обьекта, берем только блоки
                    // Autodesk.AutoCAD.DatabaseServices.MText
                    //Autodesk.AutoCAD.DatabaseServices.BlockReference

                    BlockReference blkRef =
                          (BlockReference)tr.GetObject(blkId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead);
                    //ed.WriteMessage("\nBlock: " + btr.Name + btr.AcadObject);
                    //btr.Dispose();
                    AttributeCollection attCol = blkRef.AttributeCollection;

                    int countBlock = 0;
                    foreach (ObjectId attId in attCol)
                    {
                        for (int i = 0; i < WinCloseTwo.massSeach.Length; i++)
                        {
                            AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForRead);
                            if (attRef.TextString == WinCloseTwo.massSeach[i])
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
                        //countBlock++;
                    }
                    ed.WriteMessage("количество - " + countBlock + "\n");
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


