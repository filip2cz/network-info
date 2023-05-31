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
using System.Diagnostics;
using System.Xml;

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
            
            Debug.WriteLine("app started");

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

            Debug.WriteLine("button created");

            // get data
            BindingContext = this;
            //MainThread.InvokeOnMainThreadAsync(() => Ipv4Function());
            //MainThread.InvokeOnMainThreadAsync(() => Ipv6Function());
            
            Debug.WriteLine("end of MainPage()");
        }
        private async Task UpdaterFunction()
        {
            Debug.WriteLine("Checking version from server");

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

            Debug.WriteLine("Checking version from server done");
        }
        public async Task Ipv4Function() {
            
            Debug.WriteLine("Ipv4Function() started");

            Ipv4 = "getting data";
            
            Debug.WriteLine("Ipv4 = \"getting data\"");

            // get IPv4
            string Ipv4Url = "https://v4.ipv6-test.com/api/myip.php";
            string responseipv4 = string.Empty;
            
            int i = 0;
            Ipv4 = "unknown";
            while (i < 3)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        responseipv4 = client.DownloadString(Ipv4Url);
                        Ipv4 = responseipv4;
                        i = 5;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("IPv4 request failed");
                        Debug.WriteLine(ex);
                        if (i < 5)
                        {
                            Debug.WriteLine("Trying IPv4 again");
                        }
                        i++;
                    }
                }
            }
            Debug.WriteLine("getting IPv4 done");
            Debug.WriteLine($"Ipv4 = {Ipv4}");
        }
        public async Task Ipv6Function()
        {
            Debug.WriteLine("Ipv6Function() started");

            Ipv6 = "getting data";
            
            Debug.WriteLine("Ipv6 = \"getting data\"");

            // get IPv6
            string Ipv6Url = "https://v6.ipv6-test.com/api/myip.php";
            string responseipv6 = string.Empty;

            int i = 0;
            Ipv6 = "unknown";
            while (i < 3)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        responseipv6 = client.DownloadString(Ipv6Url);
                        Ipv6 = responseipv6;
                        i = 5;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("IPv6 request failed");
                        Debug.WriteLine(ex);
                        i++;
                        if (i < 5)
                        {
                            Debug.WriteLine("Trying IPv6 again");
                        }
                    }
                }
            }
            Debug.WriteLine("getting IPv6 done");
            Debug.WriteLine($"Ipv6 = {Ipv6}");
        }
        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("reload button pressed");
            await Ipv4Function();
            await Ipv6Function();
            await UpdaterFunction();
        }
    }
}
