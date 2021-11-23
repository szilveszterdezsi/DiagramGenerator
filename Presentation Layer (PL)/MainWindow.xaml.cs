/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-11-20
/// Modified: n/a
/// ---------------------------

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BLL;

namespace PL
{
    /// <summary>
    /// Partial presentation class that initializes and handles GUI components.
    /// </summary>
    public partial class MainWindow : Window
    {
        public BindingList<Point> dataSet { get { return controller.dataSet; } }
        public string diagramTitle { get { return controller.diagramTitle; } }
        public Point maxValue { get { return controller.maxValue; } }
        public Point minValue { get { return controller.minValue; } }
        public Point tickInterval { get { return controller.tickInterval; } }
        private Controller controller;
        private DiagramPanel diagram;

        /// <summary>
        /// Constructor that initializes GUI components.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            controller = new Controller();
            InitializeGUI();
        }

        /// <summary>
        /// Initializes the GUI.
        /// </summary>
        private void InitializeGUI()
        {
            diagram = new DiagramPanel(dataSet, Brushes.Blue);
            diagram.Configure(diagramTitle, maxValue, minValue, tickInterval);
            gPlot.Children.Add(diagram);
            dataSet.ListChanged += DataSetListChanged_Refresh;
            btnRemovePoints.IsEnabled = false;
            btnClearAllPoints.IsEnabled = false;
        }

        /// <summary>
        /// Detects changes to dataset and refreshes diagram, listview columns and enable/disables clear all point button.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">ListChangedEventArgs.</param>
        private void DataSetListChanged_Refresh(object sender, ListChangedEventArgs e)
        {
            diagram.InvalidateVisual();
            AutoResizeListViewColumns(lvDataSet);
            if (lvDataSet.Items.Count > 0)
                btnClearAllPoints.IsEnabled = true;
            else
                btnClearAllPoints.IsEnabled = false;
        }

        /// <summary>
        /// Detects when listview column header is clicked and sets axis dataset is ordered by.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void LvDataSetHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = e.OriginalSource as GridViewColumnHeader;
            if (column == null || column.Content == null)
            {
                return;
            }
            string axis = column.Content.Equals("X-Coordinate") ? "X" : "Y";
            controller.SetDataSetSortingAxis(axis);
        }

        /// <summary>
        /// Detects when listview selection changes and enable/disables remove point(s) button.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">SelectionChangedEventArgs.</param>
        private void LvDataSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDataSet.SelectedItems.Count > 0)
                btnRemovePoints.IsEnabled = true;
            else
                btnRemovePoints.IsEnabled = false;
        }

        /// <summary>
        /// Forces listview to resize column widths to fit current content.
        /// </summary>
        private void AutoResizeListViewColumns(ListView lv)
        {
            GridView gridView = lv.View as GridView;
            if (gridView != null)
            {
                foreach (GridViewColumn gridViewColumn in gridView.Columns)
                {
                    if (double.IsNaN(gridViewColumn.Width))
                    {
                        gridViewColumn.Width = gridViewColumn.ActualWidth;
                    }
                    gridViewColumn.Width = double.NaN;
                }
            }
        }
    }
}
