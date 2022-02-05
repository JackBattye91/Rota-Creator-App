﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rota_Creator_App
{
    public partial class MainWindow : Window
    {
        ObservableCollection<string> Sites = new ObservableCollection<string>();

        private void initializeSites()
        {
            // ********** Sites
            Sites.Add("Default");
            lstSites.ItemsSource = Sites;
        }

        private void btnAddSite_Click(object sender, RoutedEventArgs e)
        {
            string newSiteName = "New Site";
            int counter = 1;
            while(Sites.Contains(newSiteName))
            {
                newSiteName = "New Site " + counter.ToString();
                counter++;
            }

            Sites.Add(newSiteName);
        }
        private void btnUpdateSite_Click(object sender, RoutedEventArgs e)
        {
            if (!Sites.Contains(txtSiteName.Text) || Sites[lstSites.SelectedIndex] == txtSiteName.Text)
                Sites[lstSites.SelectedIndex] = txtSiteName.Text;
            else
                updateStatusText("There is already a site named " + txtSiteName.Text);
        }
        private void btnDeleteSite_Click(object sender, RoutedEventArgs e)
        {
            if (lstSites.Items.Count <= 1)
                return;

            if (MessageBox.Show("Are you sure you want to delete site: " + lstSites.SelectedItem.ToString(), "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Sites.RemoveAt(lstSites.SelectedIndex);
        }

        private void lstSites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdateSite.IsEnabled = btnDeleteSite.IsEnabled = txtSiteName.IsEnabled = lstSites.SelectedIndex != -1;

            if (lstSites.SelectedIndex != -1)
            {
                txtSiteName.Text = (e.Source as ListView).SelectedItem.ToString();
            }
        }
    }
}
