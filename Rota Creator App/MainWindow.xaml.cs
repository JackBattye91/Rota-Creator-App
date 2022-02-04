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
        public MainWindow()
        {
            InitializeComponent();

            for (int h = 0; h < 24; h++)
            {
                cmbStartTime.Items.Add(h.ToString("00") + ":00");
                cmbFinishTime.Items.Add(h.ToString("00") + ":00");
            }
            cmbStartTime.SelectedIndex = 6;
            cmbFinishTime.SelectedIndex = 18;

            // ********** Positions
            lstPositions.ItemsSource = Positions;
            cmbPositionSite.ItemsSource = Sites;
            cmbPositionSite.SelectedIndex = 0;

            // ********** Sites
            Sites.Add("Default");
            lstSites.ItemsSource = Sites;
        }

        
    }
}
