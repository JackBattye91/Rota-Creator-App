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
            Officers = Officer.Load();
            lstOfficers.ItemsSource = Officers;
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

            Officer newOfficer = new Officer() { Name = newOfficerName };
            Officers.Add(newOfficer);
            SQLiteDatabase.Global?.Insert<Officer>(newOfficer);
        }

        private void btnUpdateOfficer_Click(object sender, RoutedEventArgs e)
        {
            if (Officers.Count(o => o.Name == txtOfficerName.Text) > 0 && Officers[lstOfficers.SelectedIndex].Name != txtOfficerName.Text)
            {
                updateStatusText("There is already an officer named " + txtOfficerName.Text);
                return;
            }

            if (Officers.Count(o => o.Abbreviation == txtOfficerAbbr.Text) > 0)
            {
                updateStatusText("There is already a officer with the abbreviation " + txtOfficerAbbr.Text);
                return;
            }
            
            int id = Officers[lstOfficers.SelectedIndex].ID;
            Officer newOfficer = new Officer() { ID = id, Name = txtOfficerName.Text, Abbreviation = txtOfficerAbbr.Text, Team = txtOfficerTeam.Text };
            Officers[lstOfficers.SelectedIndex] = newOfficer;
            SQLiteDatabase.Global?.Update<Officer>(newOfficer);

            updateStatusText("Officer " + newOfficer.Name + " updated");
        }

        private void btnDeleteOfficer_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete officer: " + (lstOfficers.SelectedItem as Officer).Name, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                updateStatusText("Officer " + (lstOfficers.SelectedItem as Officer).Name + " deleted");
                
                SQLiteDatabase.Global?.Delete<Officer>(Officers[lstOfficers.SelectedIndex]);
                Officers.RemoveAt(lstOfficers.SelectedIndex);
            }
        }

        private void btnAddOfficerPosition_Click(object sender, RoutedEventArgs e)
        {
            /*
            PositionsWindow positionsWindow = new PositionsWindow(Positions);
            bool? results = positionsWindow.ShowDialog();
            switch(results)
            {
                case true:
                    Officers[lstOfficers.SelectedIndex].WorkablePositions.Add(positionsWindow.PositionsToAdd);
                    break;
                case false:
                    return;
                default:
                    return;
            }
            */
        }
        private void btnDeleteOfficerPosition_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove " + (lstOfficerPositions.SelectedItem as Position).Name + " from this officer", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Officers[lstOfficers.SelectedIndex].WorkablePositions.RemoveAt(lstOfficerPositions.SelectedIndex);
        }

        private void lstOfficers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdateOfficer.IsEnabled = btnDeleteOfficer.IsEnabled = txtOfficerName.IsEnabled = txtOfficerAbbr.IsEnabled = txtOfficerTeam.IsEnabled = lstOfficerPositions.IsEnabled = lstOfficers.SelectedIndex != -1;

            if (lstOfficers.SelectedIndex != -1)
            {
                txtOfficerName.Text = (lstOfficers.SelectedItem as Officer).Name;
                txtOfficerAbbr.Text = (lstOfficers.SelectedItem as Officer).Abbreviation;
                txtOfficerTeam.Text = (lstOfficers.SelectedItem as Officer).Team;
                lstOfficerPositions.ItemsSource = (lstOfficers.SelectedItem as Officer).WorkablePositions;
            }
            else
            {
                txtOfficerName.Text = "";
                txtOfficerAbbr.Text = "";
                txtOfficerTeam.Text = "";
                lstOfficerPositions.ItemsSource = null;
            }
        }
    }
}
