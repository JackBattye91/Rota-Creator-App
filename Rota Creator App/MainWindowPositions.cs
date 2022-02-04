using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

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

            Positions.Add(new Position() { Name = newPositionsName, Site = Sites[0] });
        }
    }
}
