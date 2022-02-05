﻿using System;
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
        ObservableCollection<Officer> Officers = new ObservableCollection<Officer>();

        private void initializeOfficers()
        {
            lstOfficers.ItemsSource = Officers;
        }

    }
}
