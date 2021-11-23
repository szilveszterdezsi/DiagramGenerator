/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-20
/// Modified: -
/// ---------------------------

using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using DAL;

namespace BLL
{
    /// <summary>
    /// Controller class of the Diagram Generator that serves the presentation layer.
    /// </summary>
    public class Controller
    {
        private List<Point> backing;
        public BindingList<Point> dataSet;
        public string diagramTitle, defaultFilePath;
        public Point maxValue, minValue, tickInterval;        
        private bool sessionSaved, neverSaved;
        private Action sortDataSet;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Controller()
        {
            backing = new List<Point>();
            dataSet = new BindingList<Point>(backing);
            sortDataSet = SortDataSetByXAxis;
            dataSet.ListChanged += SortDataSet;
            diagramTitle = "Default Title";
            defaultFilePath = "default.dat";
            maxValue = new Point(100, 100);
            minValue = new Point(-100, -100);
            tickInterval = new Point(10, 10);
            sessionSaved = false;
            neverSaved = true;
        }

        /// <summary>
        /// Configures diagram title, max and min limit values as well as tick intervals.
        /// </summary>
        /// <param name="diagramTitle">Diagram title.</param>
        /// <param name="maxValue">Max limit values.</param>
        /// <param name="minValue">Min limit values.</param>
        /// <param name="tickInterval">Tick intervals.</param>
        public void ConfigureDiagram(string diagramTitle, Point maxValue, Point minValue, Point tickInterval)
        {
            this.diagramTitle = diagramTitle;
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.tickInterval = tickInterval;
        }

        /// <summary>
        /// Detects changes to dataset and sorts it.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">ListChangedEventArgs.</param>
        private void SortDataSet(object sender, ListChangedEventArgs e)
        {
            sortDataSet();
            sessionSaved = false;
        }

        /// <summary>
        /// Sorts dataset ordered by X-value of points.
        /// </summary>
        private void SortDataSetByXAxis()
        {
            backing.Sort((a, b) => { return a.X.CompareTo(b.X); });
        }

        /// <summary>
        /// Sorts dataset ordered by Y-value of points.
        /// </summary>
        private void SortDataSetByYAxis()
        {
            backing.Sort((a, b) => { return a.Y.CompareTo(b.Y); });
        }

        /// <summary>
        /// Sets axis for sorting dataset.
        /// </summary>
        /// <param name="axis">Order by axis, "X" or "Y".</param>
        public void SetDataSetSortingAxis(string axis)
        {
            if (axis.Equals("X"))
            {
                sortDataSet = SortDataSetByXAxis;
            }
            else if (axis.Equals("Y"))
            {
                sortDataSet = SortDataSetByYAxis;
            }
            sortDataSet();
            dataSet.ResetBindings();
        }

        /// <summary>
        /// Gets save status about the current diagram.
        /// </summary>
        /// <returns>True if session is saved, false if unsaved.</returns>
        public bool SessionSaved()
        {
            return sessionSaved;
        }

        /// <summary>
        /// Gets status on whether current diagram has ever been saved.
        /// </summary>
        /// <returns>True if session has never been saved, otherwise false.</returns>
        public bool NeverSaved()
        {
            return neverSaved;
        }

        /// <summary>
        /// Inizializes a new diagram with default values.
        /// </summary>
        public void New()
        {
            dataSet.Clear();
            diagramTitle = "Default Title";
            maxValue = new Point(100, 100);
            minValue = new Point(-100, -100);
            tickInterval = new Point(10, 10);
            neverSaved = true;
            sessionSaved = false;
        }

        /// <summary>
        /// Inizializes a new diagram from selected file.
        /// </summary>
        /// <param name="filePath">Path of the selected file.</param>
        public void Open(string filePath)
        {
            defaultFilePath = filePath;
            List<dynamic> items = Serialization.BinaryDeserializeFromFile<List<dynamic>>(filePath);
            dataSet.Clear();
            foreach (Point p in items[0])
                dataSet.Add(p);
            diagramTitle = items[1];
            maxValue = items[2];
            minValue = items[3];
            tickInterval = items[4];
            neverSaved = false;
            sessionSaved = true;
        }

        /// <summary>
        /// Saves the currect diagram to the default file.
        /// </summary>
        public void Save()
        {
            List<dynamic> items = new List<dynamic>() { dataSet, diagramTitle, maxValue, minValue, tickInterval };
            Serialization.BinarySerializeToFile(items, defaultFilePath);
            neverSaved = false;
            sessionSaved = true;
        }

        /// <summary>
        /// Saves the currect diagram to the selected file.
        /// </summary>
        /// <param name="filePath">Path of the selected file.</param>
        public void SaveAs(string filePath)
        {
            defaultFilePath = filePath;
            Save();
        }
    }
}
