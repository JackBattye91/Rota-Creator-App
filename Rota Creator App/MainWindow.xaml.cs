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

            SQLiteDatabase.CreateDatabase("rotacreator.db");
            SQLiteDatabase database = new SQLiteDatabase("Data Source=rotacreator.db");

            if (!database.TableExists("Site"))
            {
                database.CreateTable("Site", "ID INTEGER PRIMARY KEY, Name TEXT");
                database.Insert<Site>(new Site() { ID = 1, Name = "Default" });
            }
            if (!database.TableExists("Position"))
                database.CreateTable("Position", "ID INTEGER PRIMARY KEY, Name TEXT, Site INTEGER, Duration INTEGER");
            if (!database.TableExists("Officer"))
                database.CreateTable("Officer", "ID INTEGER PRIMARY KEY, Name TEXT, Abbreviation TEXT, Team TEXT, WorkablePositions TEXT");

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
                    SystemLog.Add(e);
                }
            }));

            initializeSites();
            initializePositions();
            initializeOfficers();
            initializeRota();
        }

        private void updateStatusText(string text)
        {
            SystemLog.Add($"Status: {text}");
            statusText.Text = text;

            //if (statusTextThread.ThreadState & (state1 | state2) > 0)
            //statusTextThread.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // cancel update status text thread
            statusTextThread.Abort();
            SystemLog.Dump();
        }
    }
}
