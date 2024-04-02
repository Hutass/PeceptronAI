using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Peceptron
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Perceptron perceptron;

        public MainWindow()
        {
            InitializeComponent();
            perceptron = new Perceptron(64, 10.5, 0.03);

            //datasetGenerator.CreateSymbol("4", 3000, 64, 2, 1, 0.05, 0, 0, new string[] { "Times New Roman" });
            //datasetGenerator.CreateSymbol("5", 3000, 64, 2, 1, 0.05, 0, 0, new string[] { "Times New Roman" });
            //datasetGenerator.CreateSymbol("6", 3000, 64, 2, 1, 0.05, 0, 0, new string[] { "Times New Roman" });
            //datasetGenerator.CreateSymbol("7", 3000, 64, 2, 1, 0.05, 0, 0, new string[] { "Times New Roman" });
            //datasetGenerator.CreateSymbol("8", 3000, 64, 2, 1, 0.05, 0, 0, new string[] { "Times New Roman" });
            //datasetGenerator.CreateSymbol("9", 3000, 64, 2, 1, 0.05, 0, 0, new string[] { "Times New Roman" });

            //perceptron = new Perceptron(64, 3, 0.03);
            //perceptron.Train(@"D:\DATASET_AILAB", 100002, false, 64, 64, true, 1, 20000);
            //Debug.WriteLine("Процент распознавания 4 равен " + perceptron.CheckAccuracy("D:\\Test\\4"));
            //Debug.WriteLine("Процент распознавания 5 равен " + perceptron.CheckAccuracy("D:\\Test\\5"));
            //Debug.WriteLine("Процент распознавания 6 равен " + perceptron.CheckAccuracy("D:\\Test\\6"));
            //Debug.WriteLine("Процент распознавания 7 равен " + perceptron.CheckAccuracy("D:\\Test\\7"));
            //Debug.WriteLine("Процент распознавания 8 равен " + perceptron.CheckAccuracy("D:\\Test\\8"));
            //Debug.WriteLine("Процент распознавания 9 равен " + perceptron.CheckAccuracy("D:\\Test\\9"));

            //perceptron = new Perceptron(64, 3, 0.03);
            //perceptron.Train(@"D:\DATASET_AILAB", 100002, true, 64, 64, true, 1, 20000);
            //Debug.WriteLine("Процент распознавания 4 равен " + perceptron.CheckAccuracy("D:\\Test\\4"));
            //Debug.WriteLine("Процент распознавания 5 равен " + perceptron.CheckAccuracy("D:\\Test\\5"));
            //Debug.WriteLine("Процент распознавания 6 равен " + perceptron.CheckAccuracy("D:\\Test\\6"));
            //Debug.WriteLine("Процент распознавания 7 равен " + perceptron.CheckAccuracy("D:\\Test\\7"));
            //Debug.WriteLine("Процент распознавания 8 равен " + perceptron.CheckAccuracy("D:\\Test\\8"));
            //Debug.WriteLine("Процент распознавания 9 равен " + perceptron.CheckAccuracy("D:\\Test\\9"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DatasetGenerator datasetGenerator = new(@"D:\DATASET_AILAB");

            //datasetGenerator.CreateSymbol("+", 5000, 64, 20, 0, 1, 0.05, -0.2, -0.2, 1.5, new string[] { "Arial" });
            //datasetGenerator.CreateSymbol("-", 5000, 64, 20, 0, 1, 0.05, -0.2, -0.2, 1.5, new string[] { "Arial" });

            datasetGenerator.CreateSymbol("4", 500, 64, 20, 0, 1, 0.05, 0, 0, 1, new string[] { "Times New Roman" });
            datasetGenerator.CreateSymbol("5", 500, 64, 20, 0, 1, 0.05, 0, 0, 1, new string[] { "Times New Roman" });
            datasetGenerator.CreateSymbol("6", 500, 64, 20, 0, 1, 0.05, 0, 0, 1, new string[] { "Times New Roman" });
            datasetGenerator.CreateSymbol("7", 500, 64, 20, 0, 1, 0.05, 0, 0, 1, new string[] { "Times New Roman" });
            datasetGenerator.CreateSymbol("8", 500, 64, 20, 0, 1, 0.05, 0, 0, 1, new string[] { "Times New Roman" });
            datasetGenerator.CreateSymbol("9", 500, 64, 20, 0, 1, 0.05, 0, 0, 1, new string[] { "Times New Roman" });

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            perceptron.Train(@"D:\DATASET_AILAB", 10002, false, 256, 256, true, 1, 1000);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach(string f in Directory.GetDirectories("D:\\TEST_AILAB"))
            Debug.WriteLine("процент распознавания " + f + " " + perceptron.CheckAccuracy(f));
        }
    }
}