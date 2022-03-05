using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Schedule : UserControl
    {
        protected string[] dayOfWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public Schedule()
        {
            InitializeComponent();

            for(int r = 0; r < 8; r++)
                gridSchedule.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            for(int c = 0; c < 25; c++)
                gridSchedule.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            // add day of the week labels
            for(int d = 0; d < dayOfWeek.Length; d++)
            {
                TextBlock dayLabel = new TextBlock() { Text = dayOfWeek[d], TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                Grid.SetRow(dayLabel, d + 1);
                Grid.SetColumn(dayLabel, 0);
                gridSchedule.Children.Add(dayLabel);
            }

            // add time labels
            for (int t = 0; t < 24; t++)
            {
                TextBlock timeLabel = new TextBlock() { Text = t.ToString("00") + ":00", TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                Grid.SetRow(timeLabel, 0);
                Grid.SetColumn(timeLabel, t + 1);
                gridSchedule.Children.Add(timeLabel);
            }

            
        }
    }
}
