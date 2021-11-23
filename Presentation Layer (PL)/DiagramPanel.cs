/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-11-20
/// Modified: n/a
/// ---------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Globalization;

namespace PL
{
    /// <summary>
    /// Panel class that draws a digram grid with x and y axises.
    /// Limits are determined by max and min points and tick interval is generated based on tick point.
    /// Line plot is generated based on points in data list.
    /// Inherits behavior from Canvas and Panel classes.
    /// </summary>
    public class DiagramPanel : Canvas
    {
        private BindingList<Point> data;
        private string title;
        private Point max, min, tick, zero, scale;
        private Rectangle mX, mY, back;
        private Label mC;
        private Brush ln, bg, fg, gr;
        private Pen pC, pG, pN, pB;
        private DoubleCollection sda;

        /// <summary>
        /// Constructor with parameters for line data and color.
        /// </summary>
        /// <param name="dataSet">Line data.</param>
        /// <param name="lineColor">Line color.</param>
        public DiagramPanel(BindingList<Point> dataSet, Brush lineColor)
        {
            ln = lineColor;
            bg = Brushes.Transparent;
            fg = Brushes.Black;
            gr = Brushes.LightGray;
            pC = new Pen(ln, 1);
            pG = new Pen(gr, 0.5);
            pN = new Pen(fg, 0.5);
            pB = new Pen(fg, 1);
            sda = new DoubleCollection() { 2 };
            max = new Point();
            min = new Point();
            tick = new Point();
            mX = new Rectangle() { Stroke = gr, Width = 1, StrokeDashArray = sda, Visibility = Visibility.Hidden };
            mY = new Rectangle() { Stroke = gr, Width = 1, StrokeDashArray = sda, Visibility = Visibility.Hidden };
            back = new Rectangle() { Fill = bg };
            mC = new Label() { BorderThickness = new Thickness(0), Visibility = Visibility.Hidden, FontSize = 10, FontFamily = new FontFamily("Arial") };
            Children.Add(mX);
            Children.Add(mY);
            Children.Add(back);
            Children.Add(mC);
            back.MouseMove += MouseMoves;
            back.MouseEnter += MouseEnters;
            back.MouseLeave += MouseLeaves;
            back.MouseLeftButtonDown += MouseClicks;
            data = dataSet;
            title = "";
        }

        /// <summary>
        /// Override of panel object method that draws the content of the drawing context object.
        /// Purpose of override is to obtain actual dimention of the panel after it has been rendered.
        /// Generates diagram title, grid, axises, ticks and plots line based on dataset.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        protected override void OnRender(DrawingContext dc)
        {
            Point start, end, pos;
            FormattedText ft = new FormattedText(title, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 20, fg, 1.5);
            ft.MaxTextWidth = ActualWidth;
            ft.MaxLineCount = 1;
            ft.Trimming = TextTrimming.WordEllipsis;
            Margin = new Thickness(40, string.IsNullOrEmpty(title) ? 40 : 60, 40, 40);
            zero = new Point(-min.X, -min.Y);
            scale = new Point(ActualWidth / (max.X - min.X), ActualHeight / (max.Y - min.Y));
            back.Width = ActualWidth;
            back.Height = ActualHeight;
            mX.Width = ActualWidth;
            mY.Height = ActualHeight;
            dc.DrawText(ft, new Point(ActualWidth / 2 - ft.Width / 2, -30 - ft.Height / 2));
            for (double i = min.X; i <= max.X; i++)
            {
                if (i % tick.X == 0)
                {
                    dc.DrawLine(i == 0 ? pB : pG, diagramToActual(i, min.Y), diagramToActual(i, max.Y));
                    start = diagramToActual(i, min.Y < 0 ? max.Y > 0 ? 0 : max.Y : min.Y);
                    end = diagramToActual(i, min.Y < 0 ? max.Y > 0 ? 0 : max.Y : min.Y);
                    start.Offset(0, max.Y < 0 ? 0 : 3);
                    end.Offset(0, min.Y <= 0 ? -3 : 0);
                    dc.DrawLine(pB, start, end);
                    if (i != 0)
                    {
                        ft = new FormattedText(i.ToString().Replace(",", "."), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 10, fg, 1.5);
                        pos = diagramToActual(i, min.Y < 0 ? max.Y > 0 ? 0 : max.Y : min.Y);
                        pos.Offset(-ft.Width / 2, (max.Y < 0 ? -ft.Height : ft.Height) - 5);
                        dc.DrawText(ft, pos);
                    }
                }
            }
            for (double i = min.Y; i <= max.Y; i++)
            {
                if (i % tick.Y == 0)
                {
                    dc.DrawLine(i == 0 ? pB : pG, diagramToActual(min.X, i), diagramToActual(max.X, i));
                    start = diagramToActual(min.X < 0 ? max.X > 0 ? 0 : max.X : min.X, i);
                    end = diagramToActual(min.X < 0 ? max.X > 0 ? 0 : max.X : min.X, i);
                    start.Offset(min.X <= 0 ? 3 : 0, 0);
                    end.Offset(max.X < 0 ? 0 : -3, 0);
                    dc.DrawLine(pB, start, end);
                    if (i != 0)
                    {
                        ft = new FormattedText(i.ToString().Replace(",", "."), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 10, fg, 1.5);
                        pos = diagramToActual(min.X < 0 ? max.X > 0 ? 0 : max.X : min.X, i);
                        pos.Offset(max.X < 0 ? 5 : -ft.Width -5, -ft.Height / 2);
                        dc.DrawText(ft, pos);
                    }
                }
            }
            dc.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
            for (int i = 0; i < data.Count; i++)
            {
                if (i < data.Count - 1)
                {
                    dc.DrawEllipse(ln, pC, diagramToActual(data[i].X, data[i].Y), 1.5, 1.5);
                    dc.DrawLine(pC, diagramToActual(data[i].X, data[i].Y), diagramToActual(data[i + 1].X, data[i + 1].Y));
                }
                else
                {
                    dc.DrawEllipse(fg, pC, diagramToActual(data[i].X, data[i].Y), 1.5, 1.5);
                }
            }
        }

        /// <summary>
        /// Returns point as actual canvas coordinates translated from scaled diagram coordinates.
        /// </summary>
        /// <param name="x">Sacled diagram x-coordinate.</param>
        /// <param name="y">Sacled diagram y-coordinate.</param>
        /// <returns>Point as actual canvas coordinates.</returns>
        private Point diagramToActual(double x, double y)
        {
            return new Point((x + zero.X) * scale.X, ActualHeight - (y * scale.Y) - (zero.Y * scale.Y));
        }

        /// <summary>
        /// Returns point as scaled diagram coordinates translated from actual canvas coordinates.
        /// </summary>
        /// <param name="x">Sacled diagram x-coordinate.</param>
        /// <param name="y">Sacled diagram y-coordinate.</param>
        /// <returns>Point as scaled diagram coordinates.</returns>
        private Point actualToDiagram(double x, double y)
        {
            return new Point((x / scale.X) - zero.X, ((ActualHeight - y) / scale.Y) - zero.Y);
        }

        /// <summary>
        /// Configures diagram title, max and min limit values as well as tick intervals.
        /// </summary>
        /// <param name="diagramTitle">Diagram title.</param>
        /// <param name="maxValue">Max limit values.</param>
        /// <param name="minValue">Min limit values.</param>
        /// <param name="tickInterval">Tick intervals.</param>
        public void Configure(string diagramTitle, Point maxValue, Point minValue, Point tickInterval)
        {
            title = diagramTitle;
            max = maxValue;
            min = minValue;
            tick = tickInterval;
            InvalidateVisual();
        }

        /// <summary>
        /// Detects when mouse leaves diagram as hides horizontal and vertical crosshair lines as well as coordinate numbers.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">MouseEventArgs.</param>
        private void MouseLeaves(object sender, MouseEventArgs e)
        {
            mX.Visibility = Visibility.Hidden;
            mY.Visibility = Visibility.Hidden;
            mC.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Detects when mouse enters diagram as shows horizontal and vertical crosshair lines as well as coordinate numbers.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">MouseEventArgs.</param>
        private void MouseEnters(object sender, MouseEventArgs e)
        {
            mX.Visibility = Visibility.Visible;
            mY.Visibility = Visibility.Visible;
            mC.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Detects when mouse moves over diagram as updates horizontal and vertical crosshair lines as well as coordinate numbers.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">MouseEventArgs.</param>
        private void MouseMoves(object sender, MouseEventArgs e)
        {
            mX.Margin = new Thickness(0, e.GetPosition(this).Y, 0, 0);
            mY.Margin = new Thickness(e.GetPosition(this).X, 0, 0, 0);
            Point diagram = actualToDiagram(e.GetPosition(this).X, e.GetPosition(this).Y);
            mC.Content = $"({diagram.X:0.00}: {diagram.Y:0.00})".Replace(",", ".").Replace(":", ",");
            mC.Margin = new Thickness(e.GetPosition(back).X + 2, e.GetPosition(back).Y - mC.ActualHeight, 0, 0);
        }

        /// <summary>
        /// Detects when mouse clicks on diagram and ask if user wants to add point to dataset at target coordinates.
        /// If user answers "Yes" point is added to dataset at target coordinates, otherwise nothing happens.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">MouseButtonEventArgs.</param>
        public void MouseClicks(object sender, MouseButtonEventArgs e)
        {
            Point graph = actualToDiagram(e.GetPosition(this).X, e.GetPosition(this).Y);
            MessageBoxResult result = MessageBox.Show("Do you want insert point at \n(" + graph.X.ToString().Replace(",",".") + ", " + graph.Y.ToString().Replace(",", ".") + ")?", "Insert", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                data.Add(graph);
                data.ResetBindings();
                InvalidateVisual();
            }
        }
    }
}
