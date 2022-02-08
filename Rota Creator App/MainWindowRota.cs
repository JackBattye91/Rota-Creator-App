using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace Rota_Creator_App
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Officer> availableOfficers = new ObservableCollection<Officer>();
        ObservableCollection<Officer> activeOfficers = new ObservableCollection<Officer>();

        private void initializeRota()
        {
            // Add Times to start and finish times
            for (int h = 0; h < 24; h++)
            {
                cmbStartTime.Items.Add(h.ToString("00") + ":00");
                cmbFinishTime.Items.Add(h.ToString("00") + ":00");
            }
            // select default times
            cmbStartTime.SelectedIndex = 6;
            cmbFinishTime.SelectedIndex = 18;

            lstAvailableOfficers.ItemsSource = Officers;// availableOfficers;
            lstActiveOfficers.ItemsSource = activeOfficers;
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as TabControl).SelectedItem == null)
                return;

            if (((sender as TabControl).SelectedItem as TabItem).Header.ToString() == "Rota")
            {
                availableOfficers.Clear();
                activeOfficers.Clear();

                availableOfficers = new ObservableCollection<Officer>(Officers);
                lstAvailableOfficers.ItemsSource = availableOfficers;
            }
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
                availableOfficers.Add(activeOfficers[lstActiveOfficers.SelectedIndex]);
                activeOfficers.RemoveAt(lstActiveOfficers.SelectedIndex);
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            RotaWindow rotaWindow = new RotaWindow(Officers, Positions, DateTime.Now, DateTime.Now.AddHours(12));
            rotaWindow.ShowDialog();
        }
    }
}
