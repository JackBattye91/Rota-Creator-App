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
                    TextBlock label = new TextBlock() { Text = rota.Positions[p].Name, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 12, FontWeight = FontWeights.Bold };

                    Border border = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1), Child = label };
                    Grid.SetRow(border, 0);
                    Grid.SetColumn(border, p + 1);
                    rotaGrid.Children.Add(border);
                }

                for (int h = 0; h < (rota.FinishTime - rota.StartTime).Hours; h++)
                {
                    DateTime time = rota.StartTime + new TimeSpan(h, 0, 0);
                    TextBlock label = new TextBlock() { Text = time.ToString("HH:00") + " - " + (time + new TimeSpan(1, 0, 0)).ToString("HH:00"), Padding = new Thickness(5, 0, 5, 0), TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };

                    Border border = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1), Child = label };
                    Grid.SetRow(border, h + 1);
                    Grid.SetColumn(border, 0);
                    rotaGrid.Children.Add(border);
                }

                for (int tp = 0; tp < rota.RotaTimePositions.Count; tp++)
                {
                    Rota.RotaTimePosition timePos = rota.RotaTimePositions[tp];
                    Binding officerBinding = new Binding("Name") { Source = rota.RotaTimePositions[tp].officer };
                    TextBlock label = new TextBlock() { TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                    label.SetBinding(TextBlock.TextProperty, officerBinding);

                    Border border = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1), Child = label };
                    int column = Positions.IndexOf(timePos.position) + 1;
                    int row = (timePos.time - rota.StartTime).Hours + 1;
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, column);
                    rotaGrid.Children.Add(border);


                    // create context Menu
                    ContextMenu context = new ContextMenu();
                    foreach(Officer off in Officers)
                    {
                        if (!off.CanWorkPosition(timePos.position))
                        {
                            continue;
                        }

                        Binding binding = new Binding("Name") { Source = off };

                        TextBlock textBlock = new TextBlock();
                        textBlock.SetBinding(TextBlock.TextProperty, binding);

                        textBlock.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
                        {
                            timePos.officer = off;

                            Binding offTextBind = new Binding("Name") { Source = off };
                            label.SetBinding(TextBlock.TextProperty, offTextBind);
                        };

                        context.Items.Add(textBlock);
                    }
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
