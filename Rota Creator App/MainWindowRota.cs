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
            string[] startTimeParts = cmbStartTime.SelectedItem.ToString().Split(':');
            string[] finishTimeParts = cmbFinishTime.SelectedItem.ToString().Split(':');

            int startHour = 0;
            int finishHour = 0;
            if (!int.TryParse(startTimeParts[0], out startHour) || !int.TryParse(finishTimeParts[0], out finishHour))
            {
                updateStatusText("Unable to parse start and finish time");
                return;
            }

            DateTime startTime = DateTime.Now.Date + new TimeSpan(startHour, 0, 0);
            DateTime finishTime = DateTime.Now.Date + new TimeSpan(finishHour, 0, 0);

            // if shift finishes the next day
            if (startTime < finishTime)
                finishTime.AddDays(1);

            RotaWindow rotaWindow = new RotaWindow(activeOfficers, Positions, startTime, finishTime);
            rotaWindow.ShowDialog();
        }

        private void txtAvailableOfficerSearch_Changed(object sender, RoutedEventArgs e)
        {

            if (txtAvailableOfficerSearch.Text == "")
            {
                lstAvailableOfficers.ItemsSource = availableOfficers;
                return;
            }


            List<Officer> searchResults = availableOfficers.Where(o => o.Name.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower()) ||
                                                                                        o.Team.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower()) ||
                                                                                        o.Abbreviation.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower())).ToList();
            lstAvailableOfficers.ItemsSource = searchResults;
        }

        private void txtActiveOfficerSearch_Changed(object sender, RoutedEventArgs e)
        {
            if (txtActiveOfficerSearch.Text == "")
            {
                lstActiveOfficers.ItemsSource = activeOfficers;
                return;
            }

            List<Officer> searchResults = activeOfficers.Where(o => o.Name.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower()) || 
                                                                                    o.Team.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower()) ||
                                                                                    o.Abbreviation.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower())).ToList();
            lstActiveOfficers.ItemsSource = searchResults;
        }
    }
}
