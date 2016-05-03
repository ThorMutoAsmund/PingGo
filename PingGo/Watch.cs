using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PingGo
{
    public class Watch : INotifyPropertyChanged
    {
        /// <summary>
        /// Name of the watch to identify it in the list view
        /// </summary>
        private string name;

        /// <summary>
        /// The url of the site to be watched
        /// </summary>
        private string url;

        /// <summary>
        /// Last time the watch was checked
        /// </summary>
        private DateTime lastCheck;

        /// <summary>
        /// Textual represenation of the result of the last check
        /// </summary>
        private string lastResult;

        /// <summary>
        /// The status of a watch, e.g. inactive, waiting, ok
        /// </summary>
        private WatchStatus status;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Watch()
        {
            this.LastCheck = DateTime.MinValue;
        }

        /// <summary>
        /// INotifyPropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The status of a watch, e.g. inactive, waiting, ok
        /// </summary>
        public WatchStatus Status
        {
            get { return this.status; }
            set { SetField(ref this.status, value, "Color", "StatusText"); }
        }

        /// <summary>
        /// Name of the watch to identify it in the list view
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { SetField(ref this.name, value); }
        }

        /// <summary>
        /// The url of the site to be watched
        /// </summary>
        public string Url
        {
            get { return this.url; }
            set { SetField(ref this.url, value); }
        }

        /// <summary>
        /// Last time the watch was checked
        /// </summary>
        public DateTime LastCheck
        {
            get { return this.lastCheck; }
            set { SetField(ref this.lastCheck, value); }
        }

        /// <summary>
        /// Textual represenation of the result of the last check
        /// </summary>
        public string LastResult
        {
            get { return this.lastResult; }
            set { SetField(ref this.lastResult, value); }
        }

        /// <summary>
        /// Textual version of the status to be displayed in the listview
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.Status.ToString();
            }
        }

        /// <summary>
        /// Color representing the status to be shown in the listview
        /// </summary>
        public string Color
        {
            get
            {
                switch (this.Status)
                {
                    case WatchStatus.OK:
                        return "LimeGreen";
                    case WatchStatus.Failed:
                        return "Crimson";
                    case WatchStatus.Testing:
                        return "Gold";
                    case WatchStatus.Inactive:
                        return "LightGray";
                    case WatchStatus.Waiting:
                        return "AliceBlue";
                    default:
                        return "Black";
                }
            }
        }

        /// <summary>
        /// Invokes PropertyChanged event if defined
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field if the value has changed and calls OnPropertyChanged for the calling property and any additional
        /// properties specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, params string[] additionalNotificationProperties)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);

                foreach (var otherPropertyName in additionalNotificationProperties)
                {
                    OnPropertyChanged(otherPropertyName);
                }
            }
        }
    }
}
