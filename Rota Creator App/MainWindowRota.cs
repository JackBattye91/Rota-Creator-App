using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Rota_Creator_App
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Officer> availableOfficers = new ObservableCollection<Officer>();
        ObservableCollection<Officer> activeOfficers = new ObservableCollection<Officer>();

        private void initializeRota()
        {
            lstAvailableOfficers.ItemsSource = availableOfficers;
            lstActiveOfficers.ItemsSource = activeOfficers;
        }

        private void lstAvailableOfficers_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                activeOfficers.Add(availableOfficers[lstAvailableOfficers.SelectedIndex]);
                availableOfficers.RemoveAt(lstAvailableOfficers.SelectedIndex);
            }
        }

        private void lstActiveOfficers_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {

            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
