using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public partial class MainWindow
    {
        private void btnAddPosition_Clicked(object sender, RoutedEventArgs e)
        {
            Position newPosition = new Position();

            if (txtPositionName.Text != "")
            {
                txtPositionName.ToolTip = new ToolTip() { Content = "Please enter a name" ShowDuration = "5000", Foreground = Brushes.Red };
                break;
            }
            newPosition.Name = txtPositionName.Text;

            if (!int.TryParse(txtPositionDuration.Text, newPosition.Duration))
            {
               txtPositionDuration.ToolTip = new ToolTip() { Content = "Please enter a valid duration" ShowDuration = "5000", Foreground = Brushes.Red }
               break;
            }

            Positions.Add(newPosition);
        }

        private void btnUpdatePosition_Clicked(object sender, RoutedEventArgs e)
        {
            Position selectedPos = e.AddedItems[0] as Positions;

            if (txtPositionName.Text != "")
            {
                txtPositionName.ToolTip = new ToolTip() { Content = "Please enter a name" ShowDuration = "5000", Foreground = Brushes.Red };
                break;
            }
            selectedPos.Name = txtPositionName.Text;

            if (!int.TryParse(txtPositionDuration.Text, selectedPos.Duration))
            {
               txtPositionDuration.ToolTip = new ToolTip() { Content = "Please enter a valid duration" ShowDuration = "5000", Foreground = Brushes.Red }
               break;
            }
        }

        private void btnDeletePosition_Clicked(object sender, RoutedEventArgs e)
        {
            Position delPos = positionList.SelectedItem as Position;
            if (MessageBox.Show("Are you sure you want to delete position: " + delPos.Name, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Positions.Remove(delPos);
                clearPostionForm();
            }
        }

        private void btnClearPosition_Clicked(object sender, RoutedEventArgs e)
        {
            clearPostionForm();
        }

        private void positionList_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            Position selectedPos = e.AddedItems[0] as Positions;
            txtPositionName.SetBinding(TextBox.TextProperty, new Binding("Name") { Source = selectedPos });
            txtPositionName.SetBinding(TextBox.TextProperty, new Binding("Duration") { Source = selectedPos });
        }

        private void clearPostionForm()
        {
            BindingOperations.ClearBinding(txtPositionName, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtPositionDuration, TextBox.TextProperty);

            txtPositionName.Text = "";
            txtPositionDuration.Text = "";
        }
    }
}
