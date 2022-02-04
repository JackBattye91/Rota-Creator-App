using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Rota_Creator_App
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Position> Positions = new ObservableCollection<Position>();

        private void btnAddPosition_Click(object sender, RoutedEventArgs e)
        {
            string newPositionsName = "New Position";
            int counter = 1;
            while (Positions.Count(p => p.Name == newPositionsName) > 0)
            {
                newPositionsName = "New Position " + counter.ToString();
                counter++;
            }

            Positions.Add(new Position() { Name = newPositionsName, Duration = 1, Site = Sites[0] });
        }

        private void btnUpdatePosition_Click(object sender, RoutedEventArgs e)
        {
            int duration = 1;

            if (!int.TryParse(txtPositionDuration.Text, out duration))
            {
                updateStatusText("Unable to parse duration");
                return;
            }

            Positions[lstPositions.SelectedIndex] = new Position() { Name = txtPositionName.Text, Duration = duration, Site = Sites[cmbPositionSite.SelectedIndex] };
        }

        private void btnDeletePosition_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete position: " + (lstPositions.SelectedItem as Position).Name, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Positions.RemoveAt(lstPositions.SelectedIndex);
        }

        private void lstPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdateSite.IsEnabled = btnDeleteSite.IsEnabled = lstSites.SelectedIndex != -1;

            if (lstPositions.SelectedIndex != -1)
            {
                txtPositionName.Text = ((e.Source as ListView).SelectedItem as Position).Name;
                txtPositionDuration.Text = ((e.Source as ListView).SelectedItem as Position).Duration.ToString();
                cmbPositionSite.SelectedIndex = Sites.IndexOf(((e.Source as ListView).SelectedItem as Position).Site);
            }
        }
    }
}
