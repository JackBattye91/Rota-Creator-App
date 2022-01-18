using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public partial class MainWindow
    {
        List<Officer> Officers = new List<Officer>();

        private void btnAddOfficer_Clicked(object sender, RoutedEventArgs e)
        {
            Officer newOfficer = Officer.Create(txtOfficerName.Text, txtOfficerTeam.Text, null);

            Officers.Add(newOfficer);
        }

        private void btnUpdateOfficer_Clicked(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnDeleteOfficer_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnClearOfficer_Clicked(object sender, RoutedEventArgs e)
        {
            txtOfficerName.Text = "";
            txtOfficerTeam.Text = "";
            officerPositionsList.Items.Clear();
        }
    }
}
