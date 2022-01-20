using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public partial class MainWindow
    {
        private void btnAddOfficer_Clicked(object sender, RoutedEventArgs e)
        {
            Officer newOfficer = Officer.Create(txtOfficerName.Text, txtOfficerTeam.Text, null);
            Officers.Add(newOfficer);
        }

        private void btnUpdateOfficer_Clicked(object sender, RoutedEventArgs e)
        {
            Officer selectedItem = e.AddedItems[0] as Officer;

            if (txtPositionName.Text != "")
            {
                txtOfficerName.ToolTip = new ToolTip() { Content = "Please enter a name" ShowDuration = "5000", Foreground = Brushes.Red };
                break;
            }
            selectedItem.Name = txtPositionName.Text;
            SelectedItem.Team = txtOfficerTeam.Text;
        }

        private void btnDeleteOfficer_Clicked(object sender, RoutedEventArgs e)
        {
            Officer delOff= officerList.SelectedItem as Officer;
            if (MessageBox.Show("Are you sure you want to delete officer: " + delOff.Name, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Officers.Remove(delOff);
                clearOfficerForm();
            }
        }

        private void btnClearOfficer_Clicked(object sender, RoutedEventArgs e)
        {
            clearOfficerForm();
        }

        private void clearOfficerForm()
        {
            BindingOperations.ClearBinding(txtOfficerName, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtOfficerTeam, TextBox.TextProperty);

            txtOfficerName.Text = "";
            txtOfficerTeam.Text = "";
        }
    }
}
