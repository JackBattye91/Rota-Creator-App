﻿using System;
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
            int index = 0;
            while (Positions.Count(p => p.Index() == index) > 0)
            {
                index++;
            }

            string newPositionsName = "New Position";
            int counter = 1;
            while (Positions.Count(p => p.Name() == newPositionsName) > 0)
            {
                newPositionsName = "New Position " + counter.ToString();
                counter++;
            }

            Position newPosition = new Position() { index = index, name = newPositionsName, duration = 1, site = Sites[0] };
            Positions.Add(newPosition);
            SQLiteDatabase.Global?.Insert<Position>(newPosition);
        }

        private void btnUpdatePosition_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            if (!int.TryParse(txtPositionIndex.Text, out index))
            {
                updateStatusText("Unable to parse index");
                return;
            }

            if (Positions.Count(p => p.Name() == txtPositionName.Text) > 0 && Positions[lstPositions.SelectedIndex].Name() != txtPositionName.Text)
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

            int id = Positions[lstPositions.SelectedIndex].ID();
            Position updatedPosition = new Position() { id = id, index = index, name = txtPositionName.Text, duration = duration, site = Sites[cmbPositionSite.SelectedIndex] };
            Positions[lstPositions.SelectedIndex] = updatedPosition;
            SQLiteDatabase.Global?.Update<Position>(updatedPosition);
        }

        private void btnDeletePosition_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete position: " + (lstPositions.SelectedItem as Position).Name(), "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SQLiteDatabase.Global?.Delete<Position>(Positions[lstPositions.SelectedIndex]);
                Positions.RemoveAt(lstPositions.SelectedIndex);
            }
        }

        private void lstPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdatePosition.IsEnabled = btnDeletePosition.IsEnabled = txtPositionIndex.IsEnabled = txtPositionName.IsEnabled = txtPositionDuration.IsEnabled = cmbPositionSite.IsEnabled = lstPositions.SelectedIndex != -1;

            if (lstPositions.SelectedIndex != -1)
            {
                txtPositionIndex.Text = ((e.Source as ListView).SelectedItem as Position).Index().ToString();
                txtPositionName.Text = ((e.Source as ListView).SelectedItem as Position).Name();
                txtPositionDuration.Text = ((e.Source as ListView).SelectedItem as Position).Duration().ToString();
                cmbPositionSite.SelectedItem = Sites.First(s => s.ID == ((e.Source as ListView).SelectedItem as Position).site.ID);
            }
        }

        private void lstPositions_Sort(object sender, RoutedEventArgs e)
        {
            string selection = "";
            if (selection == "Name(A-Z)")
            {
            }
            else if (selection == "Name(Z-A)")
            {
            }
            else if (selection == "Site(A-Z)")
            {   
            }
            else if (selection == "Site(Z-A)")
            {
            }
            else if (selection == "Index(1-9)")
            {
            }
            else if (selection == "Index(9-1)")
            {
            }
        }
    }
}
