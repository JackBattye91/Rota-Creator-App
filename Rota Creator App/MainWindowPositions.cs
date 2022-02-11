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

        private void initializePositions()
        {
            Positions = Position.Load();

            lstPositions.ItemsSource = Positions;
            cmbPositionSite.ItemsSource = Sites;
            cmbPositionSite.SelectedIndex = 0;
        }

        private void btnAddPosition_Click(object sender, RoutedEventArgs e)
        {
            string newPositionsName = "New Position";
            int counter = 1;
            while (Positions.Count(p => p.Name == newPositionsName) > 0)
            {
                newPositionsName = "New Position " + counter.ToString();
                counter++;
            }

            Position newPosition = new Position() { Name = newPositionsName, Duration = 1, Site = Sites[0] };
            Positions.Add(newPosition);
            SQLiteDatabase.Global?.Insert<Position>(newPosition);
        }

        private void btnUpdatePosition_Click(object sender, RoutedEventArgs e)
        {
            if (Positions.Count(p => p.Name == txtPositionName.Text) > 0 && Positions[lstPositions.SelectedIndex].Name != txtPositionName.Text)
            {
                updateStatusText("There is already a position named " + txtPositionName.Text);
                return;
            }

            int duration = 1;

            if (!int.TryParse(txtPositionDuration.Text, out duration))
            {
                updateStatusText("Unable to parse duration");
                return;
            }

            int id = Positions[lstPositions.SelectedIndex].ID;
            Positions[lstPositions.SelectedIndex] = new Position() { ID = id, Name = txtPositionName.Text, Duration = duration, Site = Sites[cmbPositionSite.SelectedIndex] };
            SQLiteDatabase.Global?.Update<Position>(Positions[lstPositions.SelectedIndex]);
        }

        private void btnDeletePosition_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete position: " + (lstPositions.SelectedItem as Position).Name, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SQLiteDatabase.Global?.Delete<Position>(Positions[lstPositions.SelectedIndex]);
                Positions.RemoveAt(lstPositions.SelectedIndex);
            }
        }

        private void lstPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdatePosition.IsEnabled = btnDeletePosition.IsEnabled = txtPositionName.IsEnabled = txtPositionDuration.IsEnabled = cmbPositionSite.IsEnabled = lstPositions.SelectedIndex != -1;

            if (lstPositions.SelectedIndex != -1)
            {
                txtPositionName.Text = ((e.Source as ListView).SelectedItem as Position).Name;
                txtPositionDuration.Text = ((e.Source as ListView).SelectedItem as Position).Duration.ToString();
                cmbPositionSite.SelectedIndex = Sites.IndexOf(((e.Source as ListView).SelectedItem as Position).Site);
            }
        }
    }
}
