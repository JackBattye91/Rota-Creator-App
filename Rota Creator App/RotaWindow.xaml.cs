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
using System.Threading;

namespace Rota_Creator_App
{
    /// <summary>
    /// Interaction logic for Rota.xaml
    /// </summary>
    public partial class RotaWindow : Window
    {
        Rota rota;

        ObservableCollection<Officer> Officers;
        ObservableCollection<Position> Positions;
        DateTime StartTime, FinishTime;

        public RotaWindow()
        {
            InitializeComponent();
        }

        public RotaWindow(ObservableCollection<Officer> officers, ObservableCollection<Position> positions, DateTime startTime, DateTime finishTime)
        {
            InitializeComponent();

            Officers = new ObservableCollection<Officer>(officers);
            Positions = new ObservableCollection<Position>(positions);
            StartTime = startTime;
            FinishTime = finishTime;

            ThreadStart threadStart = new ThreadStart(generateRota);
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        private void generateRota()
        {
            rota = Rota.Create(Officers.ToList(), Positions.ToList(), StartTime, FinishTime);


            Dispatcher.Invoke(new Action(() => {
                // add columns to grid
                rotaGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                foreach (Position pos in rota.Positions)
                    rotaGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                // add rows to grid
                rotaGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                for (int time = 0; time < (FinishTime - StartTime).Hours; time++)
                    rotaGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                for (int p = 0; p < rota.Positions.Count; p++)
                {
                    TextBlock label = new TextBlock() { Text = rota.Positions[p].Name, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 12, FontWeight = FontWeights.Bold, Padding = new Thickness(5) };

                    Border border = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1), Child = label };
                    Grid.SetRow(border, 0);
                    Grid.SetColumn(border, p + 1);
                    rotaGrid.Children.Add(border);
                }

                for (int h = 0; h < (rota.FinishTime - rota.StartTime).Hours; h++)
                {
                    DateTime time = rota.StartTime + new TimeSpan(h, 0, 0);
                    TextBlock label = new TextBlock() { Text = time.ToString("HH:00") + " - " + (time + new TimeSpan(1, 0, 0)).ToString("HH:00"), Padding = new Thickness(5), TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };

                    Border border = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1), Child = label };
                    Grid.SetRow(border, h + 1);
                    Grid.SetColumn(border, 0);
                    rotaGrid.Children.Add(border);
                }

                foreach (Rota.RotaTimePosition tp in rota.RotaTimePositions)
                {
                    Binding officerBinding = new Binding("Abbreviation") { Source = tp.officer };
                    TextBlock label = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Padding = new Thickness(5) };
                    label.SetBinding(TextBlock.TextProperty, officerBinding);

                    Border border = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1), Child = label };
                    int column = Positions.IndexOf(tp.position) + 1;
                    int row = (tp.time - rota.StartTime).Hours + 1;
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, column);

                    // create context Menu
                    ContextMenu context = new ContextMenu();
                    context.UseLayoutRounding = true;
                    MenuItem blankPosition = new MenuItem() { Header = "Empty" };
                    blankPosition.Click += (object sender, RoutedEventArgs e) =>
                    {
                        tp.officer = null;
                        label.Text = "Empty";
                    };

                    foreach (Officer off in Officers)
                    {
                        if (!off.CanWorkPosition(tp.position))
                        {
                            continue;
                        }

                        MenuItem menuItem = new MenuItem() { Header = off.Name };
                        context.Items.Add(menuItem);

                        menuItem.Click += (object sener, RoutedEventArgs e) => {
                            tp.officer = off;

                            Binding textBind = new Binding("Name") { Source = off };
                            label.SetBinding(TextBlock.TextProperty, textBind);

                            foreach(Rota.RotaTimePosition rotaTime in rota.RotaTimePositions)
                            {
                                if (rotaTime.time == tp.time && rotaTime.officer == off)
                                {

                                }
                            }

                            e.Handled = true;
                        };
                    }
                    rotaGrid.Children.Add(border);
                    border.ContextMenu = context;
                }

                progLoading.Visibility = Visibility.Collapsed;
                btnPrint.IsEnabled = true;
            }));
        }

        private void btnPrint_Click(Object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(rotaGrid, "Rota");
            }
        }

        private void btnClose_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
