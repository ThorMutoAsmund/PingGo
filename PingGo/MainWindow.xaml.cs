using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.NetworkInformation;
using System.Windows.Input;

namespace PingGo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan CheckEvery = new TimeSpan(0, 1, 0);

        private ObservableCollection<Watch> AllWatches { get; } = new ObservableCollection<Watch>();
        private System.Windows.Threading.DispatcherTimer PingTimer { get;  } = new System.Windows.Threading.DispatcherTimer();
        private string Who { get; } = "localhost";

        public MainWindow()
        {
            InitializeComponent();

            // Read watches and update window
            ReadWatches();
            this.DataContext = this.AllWatches;

            // Set timer            
            this.PingTimer.Tick += PingTimer_Tick;
            this.PingTimer.Interval = new TimeSpan(0, 0, 10);
            this.PingTimer.Start();
        }

        private void WatchInfo_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.watchListView.SelectedIndex > -1)
            {
                var watch = (Watch)watchListView.SelectedItem;
                MessageBox.Show(String.IsNullOrEmpty(watch.LastResult) ? "No info yet" : watch.LastResult, "Watch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void PingTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            foreach (var watch in this.AllWatches.Where(w => w.LastCheck == DateTime.MinValue || (now - w.LastCheck) >= this.CheckEvery))
            {
                Ping pingSender = new Ping();
                pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                // Wait 12 seconds for a reply.
                int timeout = 12000;

                // Set options for transmission:
                // The data can go through 64 gateways or routers
                // before it is destroyed, and the data packet
                // cannot be fragmented.
                PingOptions options = new PingOptions(64, true);

                Console.WriteLine("Time to live: {0}", options.Ttl);
                Console.WriteLine("Don't fragment: {0}", options.DontFragment);

                // Send the ping asynchronously.
                // Use the waiter as the user token.
                // When the callback completes, it can wake up this thread.
                pingSender.SendAsync(this.Who, timeout, buffer, options, watch);
            }
        }

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            var watch = (Watch)e.UserState;

            watch.Status = WatchStatus.OK;

            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled.");

                // Let the main thread resume. 
                // UserToken is the AutoResetEvent object that the main thread 
                // is waiting for.
                //((AutoResetEvent)e.UserState).Set();
                watch.Status = WatchStatus.Failed;
            }

            // If an error occurred, display the exception to the user.
            if (e.Error != null)
            {
                Console.WriteLine("Ping failed:");
                Console.WriteLine(e.Error.ToString());

                // Let the main thread resume. 
                //((AutoResetEvent)e.UserState).Set();
                watch.Status = WatchStatus.Failed;
            }

            PingReply reply = e.Reply;

            //DisplayReply(reply);
            StringBuilder status = new StringBuilder();
            if (reply != null)
            {
                status.AppendLine(String.Format("ping status: {0}", reply.Status));
                if (reply.Status == IPStatus.Success)
                {
                    status.AppendLine(String.Format("Address: {0}", reply.Address.ToString()));
                    status.AppendLine(String.Format("RoundTrip time: {0}", reply.RoundtripTime));
                    if (reply.Options != null)
                    {
                        status.AppendLine(String.Format("Time to live: {0}", reply.Options.Ttl));
                        status.AppendLine(String.Format("Don't fragment: {0}", reply.Options.DontFragment));
                    }
                    if (reply.Buffer != null)
                    {
                        status.AppendLine(String.Format("Buffer size: {0}", reply.Buffer.Length));
                    }
                }
            }


            watch.LastResult = status.ToString();
            
            // Let the main thread resume.
            //((AutoResetEvent)e.UserState).Set();
        }

        private void ReadWatches()
        {
            var settings = new ConfigSettings();

            foreach (var service in settings.Services)
            {
                this.AllWatches.Add(new Watch() { Name = service.Name, Url = service.Url, Status = !service.IsActive ? WatchStatus.Inactive : WatchStatus.Waiting });
            }
        }
    }
}
