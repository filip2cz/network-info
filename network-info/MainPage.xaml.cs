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
        public string Ipv4
        {
            get
            {
                return _ipv4;
            }
            set
            {
                _ipv4 = value;
                OnPropertyChanged(nameof(Ipv4));
            }
        }
        private string _ipv6;
        public string Ipv6
        {
            get
            {
                return _ipv6;
            }
            set
            {
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
        private string _country4;
        public string Country4
        {
            get
            {
                return _country4;
            }
            set
            {
                _country4 = value;
                OnPropertyChanged(nameof(Country4));
            }
        }
        private string _country6;
        public string Country6
        {
            get
            {
                return _country6;
            }
            set
            {
                _country6 = value;
                OnPropertyChanged(nameof(Country6));
            }
        }
        private string _isp4;
        public string Isp4
        {
            get
            {
                return _isp4;
            }
            set
            {
                _isp4 = value;
                OnPropertyChanged(nameof(Isp4));
            }
        }
        private string _isp6;
        public string Isp6
        {
            get
            {
                return _isp6;
            }
            set
            {
                _isp6 = value;
                OnPropertyChanged(nameof(Isp6));
            }
        }
        private string _vpn4;
        public string Vpn4
        {
            get
            {
                return _vpn4;
            }
            set
            {
                _vpn4 = value;
                OnPropertyChanged(nameof(Vpn4));
            }
        }
        private string _vpn6;
        public string Vpn6
        {
            get
            {
                return _vpn6;
            }
            set
            {
                _vpn6 = value;
                OnPropertyChanged(nameof(Vpn6));
            }
        }
        bool ipv4avaible = false;
        bool ipv6avaible = false;
        string json4 = string.Empty;
        string json6 = string.Empty;
        public MainPage()
        {
            InitializeComponent();

            // basic bool variables

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
            //MainThread.InvokeOnMainThreadAsync(() => IpInfoFunction());

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
        public async Task Ipv4Function()
        {

            Debug.WriteLine("Ipv4Function() started");

            Ipv4 = "getting data";

            Debug.WriteLine("Ipv4 = \"getting data\"");

            // get IPv4
            string Ipv4Url = "https://v4.ipv6-test.com/api/myip.php";
            string responseipv4 = string.Empty;

            int i = 0;
            while (i < 3)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        responseipv4 = client.DownloadString(Ipv4Url);
                        Ipv4 = responseipv4;
                        ipv4avaible = true;
                        i = 3;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("IPv4 request failed");
                        Debug.WriteLine(ex);
                        i++;
                        if (i < 3)
                        {
                            Debug.WriteLine("Trying IPv4 again");
                        }
                        Ipv4 = "unknown";
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
            int maxTries = 1;
            while (i < maxTries)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        responseipv6 = client.DownloadString(Ipv6Url);
                        Ipv6 = responseipv6;
                        ipv6avaible = true;
                        i = maxTries;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("IPv6 request failed");
                        Debug.WriteLine(ex);
                        i++;
                        if (i < maxTries)
                        {
                            Debug.WriteLine("Trying IPv6 again");
                        }
                        Ipv6 = "unknown";
                    }
                }
            }
            Debug.WriteLine("getting IPv6 done");
            Debug.WriteLine($"Ipv6 = {Ipv6}");
        }
        public async Task IpInfoFunction()
        {
            Debug.WriteLine("getting info about IP started");

            
        if (ipv4avaible)
        {
            Debug.WriteLine("getting data about IPv4");
                /*int i = 0;
                    while (i < 3)
                    {
                            using (WebClient client = new WebClient())
                            {
                                try
                                {
                                    json4 = client.DownloadString($"http://ip-api.com/json/{Ipv4}?fields=country,isp,proxy");

                                    Debug.WriteLine("request done");
                                    Debug.WriteLine(json4);

                                    var jsonObject4 = JsonSerializer.Deserialize<dynamic>(json4);

                                    Debug.WriteLine("jsonObject4:");
                                    //Debug.WriteLine(jsonObject4);

                                    Debug.WriteLine("JSON parsing done");
                                    Country4 = jsonObject4.country;
                                    Isp4 = jsonObject4.isp;
                                    Vpn4 = jsonObject4.proxy;
                                    Debug.WriteLine("Variables set properly");

                                    i = 3;
                                    Debug.WriteLine("Geting data about IPv4 done");
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Data about IPv4 request failed");
                                    Debug.WriteLine(ex);
                                    i++;
                                    Country4 = "unknown";
                                    Isp4 = "unknown";
                                    Vpn4 = "unknown";
                                    if (i < 3)
                                    {
                                        Debug.WriteLine("Trying IPv4 data again");
                                    }
                                }
                            }
                }*/
                Debug.WriteLine("starting Country4 request");
            int i = 0;
            while (i < 3)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        Country4 = client.DownloadString($"http://ip-api.com/line/{Ipv4}?fields=country");
                        Country4 = Country4.Replace("\n", "").Replace("\r", "");
                        i = 3;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Country4 request failed");
                        Debug.WriteLine(ex);
                        i++;
                        Country4 = "unknown";
                        if (i < 3)
                        {
                            Debug.WriteLine("Trying Country4 again");
                        }
                    }
                }
            }
            Debug.WriteLine("starting Isp4 request");
            i = 0;
            while (i < 3)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        Isp4 = client.DownloadString($"http://ip-api.com/line/{Ipv4}?fields=isp");
                        Isp4 = Isp4.Replace("\n", "").Replace("\r", "");
                        i = 3;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Isp4 request failed");
                        Debug.WriteLine(ex);
                        i++;
                        Isp4 = "unknown";
                        if (i < 3)
                        {
                            Debug.WriteLine("Trying Isp4 again");
                        }
                    }
                }
            }
            Debug.WriteLine("starting Vpn4 request");
            i = 0;
            while (i < 3)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        Vpn4 = client.DownloadString($"http://ip-api.com/line/{Ipv4}?fields=proxy");
                        Vpn4 = Vpn4.Replace("\n", "").Replace("\r", "");
                        i = 3;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Vpn4 request failed");
                        Debug.WriteLine(ex);
                        i++;
                        Vpn4 = "unknown";
                        if (i < 3)
                        {
                            Debug.WriteLine("Trying Vpn4 again");
                        }
                    }
                }
            }
        }
            else
            {
                Debug.WriteLine("getting data about IPv4 skipped because IPv4 is not avaible");
                Country4 = "unknown";
                Isp4 = "unknown";
                Vpn4 = "unknown";
            }
            
            if (ipv6avaible)
            {
                Debug.WriteLine("getting data about IPv6");
                Debug.WriteLine("starting Country6 request");
                int i = 0;
                while (i < 3)
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            Country6 = client.DownloadString($"http://ip-api.com/line/{Ipv6}?fields=country");
                            Country6 = Country6.Replace("\n", "").Replace("\r", "");
                            i = 3;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Country6 request failed");
                            Debug.WriteLine(ex);
                            i++;
                            Country6 = "unknown";
                            if (i < 3)
                            {
                                Debug.WriteLine("Trying Country6 again");
                            }
                        }
                    }
                }
                Debug.WriteLine("starting Isp6 request");
                i = 0;
                while (i < 3)
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            Isp6 = client.DownloadString($"http://ip-api.com/line/{Ipv6}?fields=isp");
                            Isp6 = Isp6.Replace("\n", "").Replace("\r", "");
                            i = 3;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Isp6 request failed");
                            Debug.WriteLine(ex);
                            i++;
                            Isp6 = "unknown";
                            if (i < 3)
                            {
                                Debug.WriteLine("Trying Isp6 again");
                            }
                        }
                    }
                }
                Debug.WriteLine("starting Vpn6 request");
                i = 0;
                while (i < 3)
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            Vpn6 = client.DownloadString($"http://ip-api.com/line/{Ipv6}?fields=proxy");
                            Vpn6 = Vpn6.Replace("\n", "").Replace("\r", "");
                            i = 3;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Vpn6 request failed");
                            Debug.WriteLine(ex);
                            i++;
                            Vpn6 = "unknown";
                            if (i < 3)
                            {
                                Debug.WriteLine("Trying Vpn6 again");
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.WriteLine("getting data about IPv6 skipped because IPv6 is not avaible");
                Country6 = "unknown";
                Isp6 = "unknown";
                Vpn6 = "unknown";
            }
            Debug.WriteLine("getting info about IP done");
        }
        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("reload button pressed");
            await Ipv4Function();
            await Ipv6Function();
            await IpInfoFunction();
            await UpdaterFunction();
        }
    }
}
