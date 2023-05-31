using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Xamarin.Essentials;
using System.Net;

namespace network_info
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private string _ipv4;
        public string Ipv4 {
            get {
                return _ipv4;
            }
            set {
                _ipv4 = value;
                OnPropertyChanged(nameof(Ipv4));
            }
        }
        private string _ipv6;
        public string Ipv6 {
            get {
                return _ipv6;
            }
            set {
                _ipv6 = value;
                OnPropertyChanged(nameof(Ipv6));
            }
        }
        private string _debugEvent;
        public string debugEvent
        {
            get
            {
                return _debugEvent;
            }
            set
            {
                _debugEvent = value;
                OnPropertyChanged(nameof(debugEvent));
            }
        }
        private string _versionStatus;
        public string versionStatus
        {
            get
            {
                return _versionStatus;
            }
            set
            {
                _versionStatus = value;
                OnPropertyChanged(nameof(versionStatus));
            }
        }

        public MainPage() {
            InitializeComponent();

            debugEvent = "app started";

            // check version
            UpdaterFunction();

            // refresh button
            Button refreshButton = new Button
            {
                Text = "Data refresh"
            };

            refreshButton.Clicked += RefreshButton_Clicked;
            
            //Content = new StackLayout
            //{
            //    Children = { refreshButton }
            //};

            debugEvent = "button created";

            // get data
            BindingContext = this;
            //MainThread.InvokeOnMainThreadAsync(() => Ipv4Function());
            //MainThread.InvokeOnMainThreadAsync(() => Ipv6Function());

            debugEvent = "end of MainPage()";
        }
        private async Task UpdaterFunction()
        {
            debugEvent = "Checking version from server";
            
            int yourVersion = 1;
            int latestVersion = yourVersion;
            using (WebClient client = new WebClient())
            {
                try
                {
                    latestVersion = Int32.Parse(client.DownloadString("https://raw.githubusercontent.com/filip2cz/network-info/main/ver"));
                }
                catch (Exception)
                {
                }
            }
            if (yourVersion == latestVersion)
            {
                versionStatus = "You have latest version published on github.";
            }
            else
            {
                versionStatus = "You do not have latest version of app, consider update.";
            }
            
            debugEvent = "Checking version from server done";
        }
        public async Task Ipv4Function() {

            debugEvent = "Ipv4Function() started";
            
            Ipv4 = "getting data";

            debugEvent = "Ipv4 = \"getting data\"";

            // get IPv4
            string Ipv4Url = "https://v4.ipv6-test.com/api/myip.php";
            string responseipv4 = string.Empty;
            
            int i = 0;
            while (i < 5)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        responseipv4 = client.DownloadString(Ipv4Url);
                        i = 5;
                    }
                    catch (Exception)
                    {
                        Ipv4 = "unknown";
                        debugEvent = "Ipv4 = \"unknown\"";
                        i++;
                    }
                }
            }
            debugEvent = "getting IPv4 done";
            Ipv4 = responseipv4;
            debugEvent = "Ipv4 = ip";
        }
        public async Task Ipv6Function()
        {

            debugEvent = "Ipv6Function() started";

            Ipv6 = "getting data";

            debugEvent = "Ipv6 = \"getting data\"";

            // get IPv6
            string Ipv6Url = "https://v6.ipv6-test.com/api/myip.php";
            string responseipv6 = string.Empty;

            int i = 0;
            while (i < 5)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        responseipv6 = client.DownloadString(Ipv6Url);
                        i = 5;
                    }
                    catch (Exception ex)
                    {
                        Ipv6 = "unknown";
                        debugEvent = "Ipv6 = \"unknown\"";
                        i++;
                    }
                }
            }
            debugEvent = "getting IPv6 done";
            Ipv6 = responseipv6;
            debugEvent = "Ipv6 = ip";
        }
        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            // Volání funkce DataAsync pro načtení nových dat
            debugEvent = "reload button pressed";
            await Ipv4Function();
            await Ipv6Function();
            await UpdaterFunction();
        }
    }
}
