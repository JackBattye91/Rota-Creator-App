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
using System.Threading;
using System.Data.SQLite;
using System.IO;

namespace Rota_Creator_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread statusTextThread;
        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists("rotacreator.db"))
            {
                SQLiteConnection.CreateFile("rotacreator.db");

                using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
                {
                    connection.Open();
                    
                    SQLiteCommand createSiteTable = connection.CreateCommand();
                    createSiteTable.CommandText = "CREATE TABLE sites(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT)";
                    createSiteTable.ExecuteNonQuery();
                    
                    SQLiteCommand createDefaultSite = connection.CreateCommand();
                    createDefaultSite.CommandText = "INSERT INTO sites(id, name) VALUES (0, 'Default')";
                    createDefaultSite.ExecuteNonQuery();
 
                    SQLiteCommand createPoitionTable = connection.CreateCommand();
                    createPoitionTable.CommandText = "CREATE TABLE positions(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, site TEXT, duration INTEGER)";
                    createPoitionTable.ExecuteNonQuery();
                    
                    SQLiteCommand createOfficerTable = connection.CreateCommand();
                    createOfficerTable.CommandText = "CREATE TABLE officers(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, abbreviation TEXT, team TEXT )";
                    createOfficerTable.ExecuteNonQuery();
                    
                }
            }

            // Create a thread to reset the status text
            statusTextThread = new Thread(new ParameterizedThreadStart((obj) => {
                try
                {
                    // wait 10 seconds
                    Thread.Sleep(10000);
                    // change status text
                    Dispatcher.Invoke(new Action(() => { statusText.Text = "Okay"; }), System.Windows.Threading.DispatcherPriority.Normal);
                }
                catch(Exception e)
                {
                }
            }));

            initializeSites();
            initializePositions();
            initializeOfficers();
            initializeRota();
        }

        private void updateStatusText(string text)
        {
            statusText.Text = text;
            statusTextThread.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // cancel update status text thread
            statusTextThread.Abort();
        }
    }
}
