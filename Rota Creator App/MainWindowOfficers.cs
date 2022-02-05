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
        ObservableCollection<Officer> Officers = new ObservableCollection<Officer>();

        private void initializeOfficers()
        {
            lstOfficers.ItemsSource = Officers;
            lstOfficerPositions.ItemsSource = Positions;
        }

        private void btnAddOfficer_Click(object sender, RoutedEventArgs e)
        {
            string newOfficerName = "New Officer";
            int counter = 1;

            while (Officers.Count(p => p.Name == newOfficerName) > 0)
            {
                newOfficerName = "New Officer " + counter.ToString();
                counter++;
            }

            Officers.Add(new Officer() { Name = newOfficerName });
        }

        private void btnUpdateOfficer_Click(object sender, RoutedEventArgs e)
        {
            if (Officers.Count(o => o.Name == txtOfficerName.Text) > 0 && Officers[lstOfficers.SelectedIndex].Name != txtOfficerName.Text)
            {
                updateStatusText("There is already a officer named " + txtPositionName.Text);
                return;
            }

            if (Officers.Count(o => o.Abbreviation == txtOfficerAbbr.Text) > 0)
            {
                updateStatusText("There is already a officer with the abbreviation " + txtOfficerAbbr.Text);
                return;
            }
            
            Officers[lstOfficers.SelectedIndex] = new Officer() { Name = txtOfficerName.Text, Abbreviation = txtOfficerAbbr.Text, Team = txtOfficerTeam.Text, WorkablePositions = new List<Position>() };
        }

        private void btnDeleteOfficer_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete officer: " + (lstOfficers.SelectedItem as Officer).Name, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Officers.RemoveAt(lstOfficers.SelectedIndex);
        }

        private void lstOfficers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdateOfficer.IsEnabled = btnDeleteOfficer.IsEnabled = txtOfficerName.IsEnabled = txtOfficerAbbr.IsEnabled = txtOfficerTeam.IsEnabled = lstOfficerPositions.IsEnabled = lstOfficers.SelectedIndex != -1;

            if (lstOfficers.SelectedIndex != -1)
            {
                txtOfficerName.Text = (lstOfficers.SelectedItem as Officer).Name;
                txtOfficerAbbr.Text = (lstOfficers.SelectedItem as Officer).Abbreviation;
                txtOfficerTeam.Text = (lstOfficers.SelectedItem as Officer).Team;
            }
        }

        private void officerPosition_Checked(object sender, RoutedEventArgs e)
        {
            (lstOfficers.SelectedItem as Officer).WorkablePositions.Add((e.Source as CheckBox).DataContext as Position);
        }
        private void officerPosition_Unchecked(object sender, RoutedEventArgs e)
        {
            (lstOfficers.SelectedItem as Officer).WorkablePositions.Remove((e.Source as CheckBox).DataContext as Position);
        }
    }
}
