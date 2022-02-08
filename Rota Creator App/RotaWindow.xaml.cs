﻿using System;
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
    /// Interaction logic for Rota.xaml
    /// </summary>
    public partial class RotaWindow : Window
    {
        Rota rota = new Rota();

        public RotaWindow()
        {
            InitializeComponent();
        }

        public RotaWindow(ObservableCollection<Officer> officers, ObservableCollection<Position> positions, DateTime startTime, DateTime finishTime)
        {
            InitializeComponent();

            rota.Generate(officers, positions, startTime, finishTime);

            // add columns to grid
            rotaGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            foreach (Position pos in rota.Positions)
                rotaGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            // add rows to grid
            rotaGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            foreach (Officer off in rota.Officers)
                rotaGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            for (int p = 0; p < rota.Positions.Count; p++)
            {
                TextBlock label = new TextBlock() { Text = rota.Positions[p].Name };
                Grid.SetRow(label, 0);
                Grid.SetColumn(label, p + 1);
                rotaGrid.Children.Add(label);
            }

            for (int h = 0; h < (rota.FinishTime - rota.StartTime).Hours; h++)
            {
                DateTime time = rota.StartTime + new TimeSpan(0, 0, 0);

                TextBlock label = new TextBlock() { Text = time.ToString("HH:00") + " - " + (time + new TimeSpan(1, 0, 0)).ToString("HH:00") };
                Grid.SetRow(label, 0 + 1);
                Grid.SetColumn(label, 0);
                rotaGrid.Children.Add(label);
            }

            foreach (Rota.RotaTimePosition timePos in rota.rotaTimePositions)
            {
                Binding officerBinding = new Binding("officer.Name") { Source = timePos };

                TextBlock label = new TextBlock() { };
                label.SetBinding(TextBlock.TextProperty, officerBinding);
                
                Grid.SetRow(label, 0 + 1);
                Grid.SetColumn(label, 0);
                rotaGrid.Children.Add(label);
            }
        }
    }
}