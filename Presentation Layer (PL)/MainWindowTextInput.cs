/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-11-20
/// Modified: n/a
/// ---------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL
{
    /// <summary>
    /// Partial presentation class that handles text input interaction with the user.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Detects when user inputs values for max and min limits and only allows negative and positive digits.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">TextCompositionEventArgs.</param>
        private void TextBox_PreviewTextInput_Values(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[-]{0,1}[0-9]*$|^[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        /// <summary>
        /// Detects when user inputs values for tick intervals and only allows positive digits greater or equal to 1.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">TextCompositionEventArgs.</param>
        private void TextBox_PreviewTextInput_Ticks(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[1-9][0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        /// <summary>
        /// Detects when user inputs values for x and y coordinates and only allows valid double type digits.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">TextCompositionEventArgs.</param>
        private void TextBox_PreviewTextInput_Point(object sender, TextCompositionEventArgs e)
        {
            string nds = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            Regex regex = new Regex("^[-][" + nds + "][0-9]+$|^[" + nds + "][0-9]+$|^[-][0-9]*[" + nds + "]{0,1}[0-9]*$|^[0-9]*[" + nds + "]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        /// <summary>
        /// Detects when user inputs "space"-value and filters it out.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">TextCompositionEventArgs.</param>
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        /// <summary>
        /// Checks/tests if any setting inputs are either empty, not valid, max is lesser than min or min is greater than max.
        /// If any test fails an error message is displayed.
        /// </summary>
        /// <returns>True is all tests pass, otherwise false.</returns>
        private bool InputSettingsCheck()
        {
            string title = "Incorrect Input";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image = MessageBoxImage.Error;
            if (string.IsNullOrEmpty(tbXMaxValue.Text))
                MessageBox.Show("X-axis max value is empty!", title, button, image);
            else if (!double.TryParse(tbXMaxValue.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out double xMax))
                MessageBox.Show("X-axis max value is not valid!", title, button, image);
            else if (string.IsNullOrEmpty(tbXMinValue.Text))
                MessageBox.Show("X-axis min value is empty!", title, button, image);
            else if (!double.TryParse(tbXMinValue.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out double xMin))
                MessageBox.Show("X-axis min value is not valid!", title, button, image);
            else if (string.IsNullOrEmpty(tbXTick.Text))
                MessageBox.Show("X-axis tick interval is empty!", title, button, image);
            else if (!double.TryParse(tbXTick.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out double xTick))
                MessageBox.Show("X-axis tick interval is not valid!", title, button, image);
            else if (string.IsNullOrEmpty(tbYMaxValue.Text))
                MessageBox.Show("Y-axis max value is empty!", title, button, image);
            else if (!double.TryParse(tbYMaxValue.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out double yMax))
                MessageBox.Show("Y-axis max value is not valid!", title, button, image);
            else if (string.IsNullOrEmpty(tbYMinValue.Text))
                MessageBox.Show("Y-axis min value is empty!", title, button, image);
            else if (!double.TryParse(tbYMinValue.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out double yMin))
                MessageBox.Show("Y-axis min value is not valid!", title, button, image);
            else if (string.IsNullOrEmpty(tbYTick.Text))
                MessageBox.Show("Y-axis tick interval is empty!", title, button, image);
            else if (!double.TryParse(tbYTick.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out double yTick))
                MessageBox.Show("Y-axis tick interval is not valid!", title, button, image);
            else if (xMax <= xMin)
                MessageBox.Show("X-axis max value is lesser than or equal to min value!", title, button, image);
            else if (xMin >= xMax)
                MessageBox.Show("X-axis min value is greater than or equal to max value!", title, button, image);
            else if (yMax <= yMin)
                MessageBox.Show("Y-axis max value is lesser than or equal to min value!", title, button, image);
            else if (yMin >= yMax)
                MessageBox.Show("Y-axis min value is greater than or equal to max value!", title, button, image);
            else
                return true;
            return false;
        }

        /// <summary>
        /// Checks/tests if any coordinate inputs are either empty or not valid.
        /// If any test fails an error message is displayed.
        /// </summary>
        /// <returns>True is all tests pass, otherwise false.</returns>
        private bool InputCoordCheck()
        {
            string title = "Incorrect Input";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image = MessageBoxImage.Error;
            if (string.IsNullOrEmpty(tbXCoord.Text))
                MessageBox.Show("X-coordinate is empty!", title, button, image);
            else if (string.IsNullOrEmpty(tbYCoord.Text))
                MessageBox.Show("Y-coordinate is empty!", title, button, image);
            else if (!double.TryParse(tbXCoord.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out double x))
                MessageBox.Show("X-coordinate is not valid!", title, button, image);
            else if (!double.TryParse(tbYCoord.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out double y))
                MessageBox.Show("Y-coordinate is not valid!", title, button, image);
            else
                return true;
            return false;
        }
    }
}
