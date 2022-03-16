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
            cmbOfficerStartPos.ItemsSource = Positions;
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
            availableOfficers.Add(newOfficer);
            SQLiteDatabase.Global?.Insert<Officer>(newOfficer);
            // select new item
            lstOfficers.SelectedIndex = lstOfficers.Items.Count() - 1;
        }

        private void btnUpdateOfficer_Click(object sender, RoutedEventArgs e)
        {
            if (Officers.Count(o => o.Name == txtOfficerName.Text) > 0 && Officers[lstOfficers.SelectedIndex].Name != txtOfficerName.Text)
            {
                updateStatusText("There is already an officer named " + txtOfficerName.Text);
                return;
            }

            if (Officers.Count(o => o.Abbreviation == txtOfficerAbbr.Text) > 0 && Officers[lstOfficers.SelectedIndex].Abbreviation != txtOfficerAbbr.Text && txtOfficerAbbr.Text.Length != 0)
            {
                updateStatusText("There is already a officer with the abbreviation " + txtOfficerAbbr.Text);
                return;
            }
            
            int id = Officers[lstOfficers.SelectedIndex].ID;
            Officer newOfficer = new Officer() { ID = id, Name = txtOfficerName.Text, Abbreviation = txtOfficerAbbr.Text, Team = txtOfficerTeam.Text, WorkablePositions = Officers[lstOfficers.SelectedIndex].WorkablePositions };
            Officers[lstOfficers.SelectedIndex] = newOfficer;
            
            for(int o = 0; o < availableOfficers.Count; o++)
            {
                if (availableOfficers[o].ID == id)
                {
                    availableOfficers[o] = newOfficer;
                    break;
                }
            }
            for (int o = 0; o < activeOfficers.Count; o++)
            {
                if (activeOfficers[o].ID == id)
                {
                    activeOfficers[o] = newOfficer;
                    break;
                }
            }

            SQLiteDatabase.Global?.Update<Officer>(newOfficer);

            updateStatusText("Officer " + newOfficer.Name + " updated");
        }

        private void btnDeleteOfficer_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete officer: " + (lstOfficers.SelectedItem as Officer).Name, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                updateStatusText("Officer " + (lstOfficers.SelectedItem as Officer).Name + " deleted");

                Officer officer = Officers[lstOfficers.SelectedIndex];

                SQLiteDatabase.Global?.Delete<Officer>(officer);
                Officers.Remove(officer);

                activeOfficers.Remove(officer);
                availableOfficers.Remove(officer);
            }
        }

        private void btnAddOfficerPosition_Click(object sender, RoutedEventArgs e)
        {
            PositionsWindow positionsWindow = new PositionsWindow(Positions);
            bool? results = positionsWindow.ShowDialog();
            switch(results)
            {
                case true:
                    foreach (Position pos in positionsWindow.PositionsToAdd)
                    {
                        if (!Officers[lstOfficers.SelectedIndex].WorkablePositions.Contains(pos))
                            Officers[lstOfficers.SelectedIndex].WorkablePositions.Add(pos);
                    }

                    lstOfficerPositions.ItemsSource = Officers[lstOfficers.SelectedIndex].WorkablePositions;

                    break;
                case false:
                    return;
                default:
                    return;
            }
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
