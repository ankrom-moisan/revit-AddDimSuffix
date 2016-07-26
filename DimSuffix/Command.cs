using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DimSuffix
{
    [Transaction(TransactionMode.Manual)]
    class Command : IExternalCommand
    {
        public string suffix = "";
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            MainWindow mWindow = new MainWindow(doc, uidoc);
            mWindow.ShowDialog();


            

            return Result.Succeeded;
        }
    }
}
