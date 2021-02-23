using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRegression
{
    class Functionality
    {
        private string[] splitters = { ",", "/", ":" };
        private MainWindow window;

        public Functionality(MainWindow window)
        {
            this.window = window;
        }

        public void SolveRegression()
        {
            foreach (MyPoint singleMyPoint in window.collection)
            {
                window.collectionDataPoint.Add(MyPoint.ConvertToDataPoint(singleMyPoint));
            }
            LinearSolver solver = new LinearSolver(window.collection);
            window.resultatField.Text = solver.functionExample();
            window.trendLineCollection.AddRange(solver.startValues());
        }

        public void Clear()
        {
            window.collectionDataPoint.Clear();
            window.trendLineCollection.Clear();
            window.collection.Clear();
        }

        public void ActivateButtons()
        {
            window.solveBtn.IsEnabled = true;
            window.exportPlotBtn.IsEnabled = true;
            window.splittersBtn.IsEnabled = true;
            window.clearBtn.IsEnabled = true;
            window.dataGrid.IsEnabled = true;
            window.dropArea.IsEnabled = true;
        }

        public void FillDataGrid(string dataPath)
        {
            using (StreamReader dataReader = new StreamReader(dataPath))
            {
                string singlePoint;
                while ((singlePoint = dataReader.ReadLine()) != null)
                    window.collection.Add(new MyPoint((singlePoint.Split(' ')[0]), (singlePoint.Split(' ')[1])));
            }
        }

        public void ExportPlotToPng()
        {
            SaveFileDialog savePlotDialog = new SaveFileDialog();
            savePlotDialog.Filter = "PNG files (*.png) | *.png";
            if (savePlotDialog.ShowDialog() == true)
            {
                var pngExporter = new PngExporter { Width = 500, Height = 500, Background = OxyColors.White };
                pngExporter.ExportToFile(window.oxyPlot.ActualModel, savePlotDialog.FileName);
            }
        }

        public void ChangeSplitters()
        {
            string readerDatapath = null;
            string text = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt) | *.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                readerDatapath = openFileDialog.FileName;
                using (StreamReader dataReader = new StreamReader(readerDatapath))
                {
                    text = dataReader.ReadToEnd();
                    foreach (string splitter in splitters)
                    {
                        text = text.Replace(splitter, " ");
                    }
                }
                using (StreamWriter dataWriter = new StreamWriter(readerDatapath, false))
                {
                    dataWriter.Write(text);
                }
            }
        }
    }
}
