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
        private void initializeRota()
        {
            lstAvailableOfficers.ItemsSource = Officers;
        }

    }
}
