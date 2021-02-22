using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinearRegression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string dataPath;

        public ObservableCollection<MyPoint> collection { get; set; }
        public ObservableCollection<DataPoint> collectionDataPoint { get; set; }
        public List<DataPoint> trendLineCollection { get; set; }

        private static readonly string  FAQMessage = "1. Przyjmwowana baza jest baza gdzie pomiedzy koordynatami stoi symbol spacji! Jezeli baza posiada inny separator to najpierw wybieramy Change splitters i tylko po tym wzucamy baze do programu\n2. Wykres mozna przesuwac/powiekszac za pomoca myszki\n3. Dane wejsciowe mozna redagowac za pomoca tabelki";

        string[] splitters = { ",", "/", ":" };

        public MainWindow()
        {
            InitializeComponent();
            collection = new ObservableCollection<MyPoint>();
            trendLineCollection = new List<DataPoint>();
            collectionDataPoint = new ObservableCollection<DataPoint>();
            DataContext = this;
        }

        private void titlePanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void closeImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void minimizeImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void solveBtn_Click(object sender, RoutedEventArgs e)
        {
            collectionDataPoint.Clear();
            trendLineCollection.Clear();
            foreach(MyPoint singleMyPoint in collection)
            {
                collectionDataPoint.Add(MyPoint.ConvertToDataPoint(singleMyPoint));
            }

            LinearSolver solver = new LinearSolver(collection);
            resultatField.Text = solver.functionExample();
            trendLineCollection.AddRange(solver.startValues());
        }

        private void authorBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(FAQMessage, "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            ActivateButtons();
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            Clear();
            dataPath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            if (dataPath.Substring(dataPath.LastIndexOf(".")) != ".txt")
            {
                MessageBox.Show($"Baza danych musi byc plikiem tekstowym .txt", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                using (StreamReader dataReader = new StreamReader(dataPath))
                {
                    string singlePoint;
                    while ((singlePoint = dataReader.ReadLine()) != null)
                        collection.Add(new MyPoint((singlePoint.Split(' ')[0]), (singlePoint.Split(' ')[1])));
                }
            }
        }

        private void splittersBtn_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show(text);
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

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            resultatField.Text = "Y = ";
        }

        private void exportPlotBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savePlotDialog = new SaveFileDialog();
            savePlotDialog.Filter = "PNG files (*.png) | *.png";
            if (savePlotDialog.ShowDialog() == true)
            {
                var pngExporter = new PngExporter { Width = 500, Height = 500, Background = OxyColors.White };
                pngExporter.ExportToFile(oxyPlot.ActualModel, savePlotDialog.FileName);
            }        
        }
        
        /*Function to clear all collections used in program */
        private void Clear()
        {
            collectionDataPoint.Clear();
            trendLineCollection.Clear();
            collection.Clear();
        }

        private void ActivateButtons()
        {
            solveBtn.IsEnabled = true;
            exportPlotBtn.IsEnabled = true;
            splittersBtn.IsEnabled = true;
            clearBtn.IsEnabled = true;
            dataGrid.IsEnabled = true;
            dropArea.IsEnabled = true;
        }
    }
}
