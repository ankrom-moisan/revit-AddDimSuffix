using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DimSuffix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Document doc, UIDocument uiDoc)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            btnOk.Click += (sender, e) => btnOk_Click(sender, e, doc, uiDoc);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e, Document doc, UIDocument uidoc)
        {
            // The suffix to add:
            string suffix = textBox.Text;

            // As long as the box was filled out:
            if (suffix != null || suffix != "")
            {
                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Transaction Name");

                    // Get all currently selected elements
                    IList<ElementId> elemIdList = uidoc.Selection.GetElementIds().ToList();

                    // Filter the selected elements down to only include dimensions
                    // -2000260 is the element ID of the dimension category
                    IList<Element> filteredList = new List<Element>();
                    foreach (ElementId elemId in elemIdList)
                    {
                        Element selectedElem = uidoc.Document.GetElement(elemId);
                        if (selectedElem.Category.Id.IntegerValue == -2000260)
                        {
                            filteredList.Add(selectedElem);
                        }
                    }

                    // cast the selected elements as dimensions and change their suffix
                    // if the dimension has multiple segments, iterate through segments
                    // and change the suffix on each.
                    foreach (var item in filteredList)
                    {
                        var itemDim = item as Dimension;
                        var dimsegs = itemDim.Segments;
                        if(dimsegs.Size == 0)
                        {
                            itemDim.Suffix = suffix;
                        }
                        else
                        {
                            for (int i = 0; i < dimsegs.Size; i++)
                            {
                                var segment = dimsegs.get_Item(i);
                                segment.Suffix = suffix;
                            }
                        }
                    }
                    tx.Commit();
                }
            }
            Close();
        }
    }
}
