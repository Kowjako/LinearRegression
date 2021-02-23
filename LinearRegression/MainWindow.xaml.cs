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
        private Functionality functionality;

        public ObservableCollection<MyPoint> collection { get; set; }
        public ObservableCollection<DataPoint> collectionDataPoint { get; set; }
        public List<DataPoint> trendLineCollection { get; set; }

        private static readonly string  FAQMessage = "1. Przyjmwowana baza jest baza gdzie pomiedzy koordynatami stoi symbol spacji! Jezeli baza posiada inny separator to najpierw wybieramy Change splitters i tylko po tym wzucamy baze do programu\n2. Wykres mozna przesuwac/powiekszac za pomoca myszki\n3. Dane wejsciowe mozna redagowac za pomoca tabelki";

        public MainWindow()
        {
            InitializeComponent();
            collection = new ObservableCollection<MyPoint>();
            trendLineCollection = new List<DataPoint>();
            collectionDataPoint = new ObservableCollection<DataPoint>();
            functionality = new Functionality(this);
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
            functionality.SolveRegression();
        }

        private void authorBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(FAQMessage, "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            functionality.ActivateButtons();
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            functionality.Clear();
            dataPath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            if (dataPath.Substring(dataPath.LastIndexOf(".")) != ".txt")
            {
                MessageBox.Show($"Baza danych musi byc plikiem tekstowym .txt", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                functionality.FillDataGrid(dataPath);
            }
        }

        private void splittersBtn_Click(object sender, RoutedEventArgs e)
        {
            functionality.ChangeSplitters();
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            functionality.Clear();
            resultatField.Text = "Y = ";
        }

        private void exportPlotBtn_Click(object sender, RoutedEventArgs e)
        {
            functionality.ExportPlotToPng();       
        } 
    }
}
