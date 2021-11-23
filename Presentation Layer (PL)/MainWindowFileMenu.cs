/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-20
/// Modified: n/a
/// ---------------------------

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Partial presentation class that handles File-menu events and I/O interaction with the user.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Checks "save status" and lets user choose "Yes, No or Cancel" if status is "unsaved" and content is not empty.
        /// If user chooses "Yes" the method SaveCommand_Executed is raised as if user clicked "Save" in File-menu.
        /// If user chooses "No" or "Cancel" nothing happens.
        /// </summary>
        /// <returns>True if status is "saved" and if user chooses "Yes" or "No", otherwise false.</returns>
        private bool SaveCheck()
        {
            if (!controller.SessionSaved() && lvDataSet.Items.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Would you like to save current diagram?", "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveCommand_Executed(null, null);
                    return true;
                }
                else if (result == MessageBoxResult.No)
                    return true;
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Detects when "New" is clicked in the File-menu and performs a SaveCheck.
        /// If SaveCheck returns true controller will attempt to reset to start-up defaults.
        /// If attempt fails an error info message is displayed.
        /// </summary>
        /// <param name="sender">Component clicked.</param>
        /// <param name="e">Routed event.</param>
        private void NewCommand_Executed(object sender, RoutedEventArgs e)
        {
            if (SaveCheck())
            {
                try
                {
                    controller.New();
                    diagram.Configure(diagramTitle, maxValue, minValue, tickInterval);
                    tbDiagramTitle.Text = diagramTitle;
                    tbXMaxValue.Text = maxValue.X.ToString();
                    tbYMaxValue.Text = maxValue.Y.ToString();
                    tbXMinValue.Text = minValue.X.ToString();
                    tbYMinValue.Text = minValue.Y.ToString();
                    tbXTick.Text = tickInterval.X.ToString();
                    tbYTick.Text = tickInterval.Y.ToString();
                    miSaveAs.IsEnabled = false;
                    MessageBox.Show("New diagram initialized.", "New", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Detects when "Open..." is clicked in the File-menu and performs a 'SaveCheck'.
        /// If 'SaveCheck' returns true, a dialog to select file opens and attempt to load data from selected file is made.
        /// If attempt fails an error info message is displayed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void OpenCommand_Executed(object sender, RoutedEventArgs e)
        {
            if (SaveCheck())
            {
                OpenFileDialog op = new OpenFileDialog { Title = "Open", Filter = "Data files (*.dat)|*.dat" };
                if (op.ShowDialog() == true)
                {
                    try
                    {
                        controller.Open(op.FileName);
                        diagram.Configure(diagramTitle, maxValue, minValue, tickInterval);
                        tbDiagramTitle.Text = diagramTitle;
                        tbXMaxValue.Text = maxValue.X.ToString();
                        tbYMaxValue.Text = maxValue.Y.ToString();
                        tbXMinValue.Text = minValue.X.ToString();
                        tbYMinValue.Text = minValue.Y.ToString();
                        tbXTick.Text = tickInterval.X.ToString();
                        tbYTick.Text = tickInterval.Y.ToString();
                        miSaveAs.IsEnabled = true;
                        MessageBox.Show("Diagram successfully loaded from file!", "Open", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Detects when "Save" is clicked in the File-menu and checks if diagram has ever been saved.
        /// If 'neverSaved' returns false the method 'SaveAsCommand_Executed' is raised as if user clicked "Save as..." in File-menu.
        /// If 'neverSaved' returns true attempt to save current diagram to default save file is made.
        /// If attempt fails an error info message is displayed.
        /// </summary>
        /// <param name="sender">Component clicked.</param>
        /// <param name="e">Routed event.</param>
        private void SaveCommand_Executed(object sender, RoutedEventArgs e)
        {
            if (controller.NeverSaved())
                SaveAsCommand_Executed(sender, e);
            else
            {
                try
                {
                    controller.Save();
                    MessageBox.Show("Diagram successfully saved to file!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Detects when "Save As..." is clicked in the File-menu.
        /// A dialog to select file opens and  attempt to save current diagram to selected save file is made.
        /// If attempt fails an error info message is displayed.
        /// </summary>
        /// <param name="sender">Component clicked.</param>
        /// <param name="e">Routed event.</param>
        private void SaveAsCommand_Executed(object sender, RoutedEventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog { Title = "Save As...", Filter = "Data files (*.dat)|*.dat" };
            if (op.ShowDialog() == true)
            {
                try
                {
                    controller.SaveAs(op.FileName);
                    miSaveAs.IsEnabled = true;
                    MessageBox.Show("Diagram successfully saved to file!", "Save Game", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Detects when "Exit" is clicked in the File-menu and exits.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void ExitCommand_Executed(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Overrides and detects when any shutdown event is raised and performs a 'SaveCheck'.
        /// If SaveCheck returns true application exits.
        /// If SaveCheck returns false exit is aborted.
        /// </summary>
        /// <param name="e">CancelEventArgs.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!SaveCheck())
                e.Cancel = true;
        }
    }
}
