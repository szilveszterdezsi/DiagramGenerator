/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-11-20
/// Modified: n/a
/// ---------------------------

using System.Linq;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Partial presentation class that handles button input interaction with the user.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Configures diagram title, max and min limit values as well as tick intervals based in text inputs.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void BtnApplySettings_Click(object sender, RoutedEventArgs e)
        {
            if (InputSettingsCheck())
            {
                controller.ConfigureDiagram(tbDiagramTitle.Text, new Point(double.Parse(tbXMaxValue.Text), double.Parse(tbYMaxValue.Text)), new Point(double.Parse(tbXMinValue.Text), double.Parse(tbYMinValue.Text)), new Point(double.Parse(tbXTick.Text), double.Parse(tbYTick.Text)));
                diagram.Configure(diagramTitle, maxValue, minValue, tickInterval);
            }
        }

        /// <summary>
        /// Adds a new point to dataset based in text inputs.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void BtnAddPoint_Click(object sender, RoutedEventArgs e)
        {
            if (InputCoordCheck())
            {
                dataSet.Add(new Point(double.Parse(tbXCoord.Text), double.Parse(tbYCoord.Text)));
                dataSet.ResetBindings();
                tbXCoord.Text = "";
                tbYCoord.Text = "";
            }
        }

        /// <summary>
        /// Removes selected point(s) from dataset if user confirms.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void BtnRemovePoints_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to remove selected point(s)?", "Remove", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                lvDataSet.SelectedItems.Cast<Point>().ToList().ForEach(p => dataSet.Remove(p));
                dataSet.ResetBindings();
            }
        }

        /// <summary>
        /// Clears all points from dataset if user confirms.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void BtnClearAllPoints_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to clear all points?", "Clear", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                dataSet.Clear();
            }
        }
    }
}
