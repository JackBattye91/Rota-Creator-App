using System;
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

        private void lstSites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSiteName.Text = (e.Source as ListView).SelectedItem.ToString();
        }
    }
}
