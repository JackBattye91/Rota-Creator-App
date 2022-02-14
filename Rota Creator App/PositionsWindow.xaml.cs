using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rota_Creator_App
{
    /// <summary>
    /// Interaction logic for PositionsWindow.xaml
    /// </summary>
    public partial class PositionsWindow : Window
    {
        ObservableCollection<Position> Positions;
        public ObservableCollection<Position> PositionsToAdd { get; protected set; } = new ObservableCollection<Position>();
        public PositionsWindow()
        {
            InitializeComponent();
        }
        public PositionsWindow(ObservableCollection<Position> positions)
        {
            InitializeComponent();

            Positions = positions;
            lstPositions.ItemsSource = Positions;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            foreach(Position pos in lstPositions.SelectedItems)
            {
                PositionsToAdd.Add(pos);
            }
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
