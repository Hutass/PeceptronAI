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
        public MainWindow()
        {
            InitializeComponent();
            DatasetGenerator datasetGenerator = new(@"D:\DATASET_AILAB");
            datasetGenerator.CreateSymbol("0", 200, 30,7);
            datasetGenerator.CreateSymbol("1", 200, 30,7);
        }
    }
}