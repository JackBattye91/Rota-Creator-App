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

            foreach(Officer off in Officers)
            {
                availableOfficers.Add(off);
            }

            lstAvailableOfficers.ItemsSource = availableOfficers;
            lstActiveOfficers.ItemsSource = activeOfficers;
            cmbRotaSite.ItemsSource = Sites;
            cmbRotaSite.SelectedIndex = 0;
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as TabControl).SelectedItem == null)
                return;

            if (((sender as TabControl).SelectedItem as TabItem).Header.ToString() == "Rota")
            {
                /*foreach(Officer off in Officers)
                {
                    if (!availableOfficers.Contains(off))
                        availableOfficers.Add(off);
                }*/
                
                //lstAvailableOfficers.ItemsSource = availableOfficers;
            }
        }

        

        private void btnMoveOfficerRight_Click(object sender, RoutedEventArgs e)
        {
            while(lstAvailableOfficers.SelectedItems.Count != 0)
            {
                Officer off = lstAvailableOfficers.SelectedItems[0] as Officer;

                activeOfficers.Add(off);
                availableOfficers.Remove(off);
            }
        }
        private void btnMoveAllOfficersRight_Click(object sender, RoutedEventArgs e)
        {
            while (lstAvailableOfficers.Items.Count != 0)
            {
                Officer off = lstAvailableOfficers.Items[0] as Officer;

                activeOfficers.Add(off);
                availableOfficers.Remove(off);
            }
        }
        private void btnMoveOfficerLeft_Click(object sender, RoutedEventArgs e)
        {
            while (lstActiveOfficers.SelectedItems.Count != 0)
            {
                Officer off = lstActiveOfficers.SelectedItems[0] as Officer;

                activeOfficers.Remove(off);
                availableOfficers.Add(off);
            }
        }
        private void btnMoveAllOfficersLeft_Click(object sender, RoutedEventArgs e)
        {
            while (lstActiveOfficers.Items.Count != 0)
            {
                Officer off = lstActiveOfficers.Items[0] as Officer;

                activeOfficers.Remove(off);
                availableOfficers.Add(off);
            }
        }

        private void lstAvailableOfficers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;
            Officer off = listView.SelectedItem as Officer;

            if (!activeOfficers.Contains(off))
                activeOfficers.Add(off);

            availableOfficers.Remove(off);
        }

        private void lstActiveOfficers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;
            Officer off = listView.SelectedItem as Officer;

            if (!availableOfficers.Contains(off))
                availableOfficers.Add(off);

            activeOfficers.Remove(off);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string[] startTimeParts = cmbStartTime.SelectedItem.ToString().Split(':');
            string[] finishTimeParts = cmbFinishTime.SelectedItem.ToString().Split(':');

            if (!int.TryParse(startTimeParts[0], out int startHour) || !int.TryParse(finishTimeParts[0], out int finishHour))
            {
                updateStatusText("Unable to parse start and finish time");
                return;
            }

            DateTime startTime = DateTime.Now.Date + new TimeSpan(startHour, 0, 0);
            DateTime finishTime = DateTime.Now.Date + new TimeSpan(finishHour, 0, 0);

            // if shift finishes the next day
            if (startTime < finishTime)
                finishTime.AddDays(1);

            ObservableCollection<Position> sitePositions = new ObservableCollection<Position>(Positions.Where(p => p.Site.ID == (cmbRotaSite.SelectedItem as Site).ID).ToList());

            RotaWindow rotaWindow = new RotaWindow(activeOfficers, sitePositions, startTime, finishTime);
            rotaWindow.ShowDialog();
        }

        private void txtAvailableOfficerSearch_Changed(object sender, RoutedEventArgs e)
        {
            if (txtAvailableOfficerSearch.Text == "")
            {
                lstAvailableOfficers.ItemsSource = availableOfficers;
                return;
            }

            ObservableCollection<Officer> searchResults = new ObservableCollection<Officer>(availableOfficers.Where(o =>  o.Name.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower()) ||
                                                                                        o.Team.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower()) ||
                                                                                        o.Abbreviation.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower())).ToList());
            lstAvailableOfficers.ItemsSource = searchResults;
        }

        private void txtActiveOfficerSearch_Changed(object sender, RoutedEventArgs e)
        {
            if (txtActiveOfficerSearch.Text == "")
            {
                lstActiveOfficers.ItemsSource = activeOfficers;
                return;
            }

            ObservableCollection<Officer> searchResults = new ObservableCollection<Officer>(activeOfficers.Where(o => o.Name.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower()) || 
                                                                                    o.Team.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower()) ||
                                                                                    o.Abbreviation.ToLower().Contains(txtAvailableOfficerSearch.Text.ToLower())));
            lstActiveOfficers.ItemsSource = searchResults;
        }
    }
}
