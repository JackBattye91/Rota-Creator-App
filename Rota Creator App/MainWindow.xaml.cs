using System;
using System.Collections.Generic;
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

namespace Rota_Creator_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Officer> Officers;
        List<Position> Positions;

        public MainWindow()
        {
            InitializeComponent();

            

            Officers = Officer.Load("");
            Positions = Position.Load("");
            officerList.ItemsSource = Officers;
            positionList.ItemsSource = Positions;
        }

        private void btnGenerate_Clicked(object sender, RoutedEventArgs e)
        {
            List<Officer> currOfficers = new List<Officer>();
            DateTime startTime = new DateTime();
            DateTime finishTime = new DateTime();
            Rota rota = Rota.Create(currOfficers, Positions, startTime, finishTime);
        }
    }
}
