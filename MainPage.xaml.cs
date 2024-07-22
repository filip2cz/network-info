using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace network_info
{
    public partial class MainPage : ContentPage
    {
        public bool ipv4avaible = false;
        public bool ipv6avaible = false;

        public MainPage()
        {
            InitializeComponent();

            RefreshInfo();
        }
        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            RefreshInfo();
        }
        public async void RefreshInfo()
        {
            Ipv4.Text = "getting data";
            Ipv6.Text = "getting data";

            Country4.Text = "getting data";
            Country6.Text = "getting data";

            Isp4.Text = "getting data";
            Isp6.Text = "getting data";

            Vpn4.Text = "getting data";
            Vpn6.Text = "getting data";

            string versionString = "";
            string Ipv4LocalString = "";
            string Ipv6LocalString = "";
            string Ipv4String = "";
            string Ipv6String = "";
            string Country4String = "";
            string Country6String = "";
            string Isp4String = "";
            string Isp6String = "";
            string Vpn4String = "";
            string Vpn6String = "";

            // Díky Rakočević za návod na await Task.Run
            await Task.Run(async () =>
            {
                Ipv4LocalString = await GetLocalIPv4();
                Ipv6LocalString = await GetLocalIPv6();
            });
            Ipv4Local.Text = Ipv4LocalString;
            Ipv6Local.Text = Ipv6LocalString;

            await Task.Run(async() =>
            {
                versionString = await UpdaterFunction();

                Ipv4String = await GetIPv4();
            });

            versionStatus.Text = versionString;
            Ipv4.Text = Ipv4String;

            if (ipv4avaible)
            {
                await Task.Run(async () =>
                {
                    Country4String = await GetCountry(Ipv4String);
                    Isp4String = await GetIsp(Ipv4String);
                    Vpn4String = await GetVpn(Ipv4String);
                });
            }
            else
            {
                Country4String = "unknown";
                Isp4String = "unknown";
                Vpn4String = "unknown";
            }

            Country4.Text = Country4String;
            Isp4.Text = Isp4String;
            Vpn4.Text = Vpn4String;

            await Task.Run(async () =>
            {
                Ipv6String = await GetIPv6();
            });

            Ipv6.Text = Ipv6String;

            if (ipv6avaible)
            {
                await Task.Run(async () =>
                {
                    Country6String = await GetCountry(Ipv6String);
                    Isp6String = await GetIsp(Ipv6String);
                    Vpn6String = await GetVpn(Ipv6String);
                });
            }
            else
            {
                Country6String = "unknown";
                Isp6String = "unknown";
                Vpn6String = "unknown";
            }

            Country6.Text = Country6String;
            Isp6.Text = Isp6String;
            Vpn6.Text = Vpn6String;

            Debug.WriteLine("Refreshing done");
        }
        public async Task<string> UpdaterFunction()
        {
            Debug.WriteLine("Checking version from server");

            string yourVersion = AppInfo.BuildString;
            string latestVersion = yourVersion;

            latestVersion = await MakeWebRequest("https://raw.githubusercontent.com/filip2cz/network-info/main/ver");

            if (yourVersion == latestVersion)
            {
                return "You have latest version.";
            }
            else if (latestVersion == "unknown")
            {
                return "Failed to check for updates.";
            }
            else
            {
                return "You do not have latest version of app, consider update.";
            }
        }
        // thanks chatgpt for GetLocalIPv* functions
        public async Task<string> GetLocalIPv4()
        {
            string ipAddress = null;
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var addressInfo in netInterface.GetIPProperties().UnicastAddresses)
                {
                    if (addressInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = addressInfo.Address.ToString();
                        break;
                    }
                }
                if (ipAddress != null)
                    break;
            }
            return ipAddress;
        }
        public async Task<string> GetLocalIPv6()
        {
            string ipAddress = null;
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var addressInfo in netInterface.GetIPProperties().UnicastAddresses)
                {
                    if (addressInfo.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        ipAddress = addressInfo.Address.ToString();
                        break;
                    }
                }
                if (ipAddress != null)
                    break;
            }
            return ipAddress;
        }
        public async Task<string> GetIPv4()
        {
            string Ipv4Url = "https://v4.ipv6-test.com/api/myip.php";

            string responseipv4 = await MakeWebRequest(Ipv4Url);

            if (responseipv4 == "unknown")
            {
                ipv4avaible = false;
            }
            else
            {
                ipv4avaible = true;
            }

            Debug.WriteLine("getting IPv4 done");
            Debug.WriteLine($"Ipv4 = {responseipv4}");

            return responseipv4;
        }
        public async Task<string> GetIPv6()
        {
            string Ipv6Url = "https://v6.ipv6-test.com/api/myip.php";

            string responseipv6 = await MakeWebRequest(Ipv6Url);

            if (responseipv6 == "unknown")
            {
                ipv6avaible = false;
            }
            else
            {
                ipv6avaible = true;
            }

            Debug.WriteLine("getting IPv6 done");
            Debug.WriteLine($"Ipv6 = {responseipv6}");

            return responseipv6;
        }
        public async Task<string> GetCountry(string ip)
        {
            string url = $"http://ip-api.com/line/{ip}?fields=country";

            string response = await MakeWebRequest(url);

            return response;
        }
        public async Task<string> GetIsp(string ip)
        {
            string url = $"http://ip-api.com/line/{ip}?fields=isp";

            string response = await MakeWebRequest(url);

            return response;
        }
        public async Task<string> GetVpn(string ip)
        {
            string url = $"http://ip-api.com/line/{ip}?fields=proxy";

            string response = await MakeWebRequest(url);

            return response;
        }
        public async Task<string> MakeWebRequest(string url)
        {
            string response = string.Empty;
            int i = 0;
            while (i < 3)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        // to do https://stackoverflow.com/questions/1789627/how-to-change-the-timeout-on-a-net-webclient-object
                        response = client.DownloadString(url);
                        i = 3;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("HTTPS request failed");
                        Debug.WriteLine(ex);
                        i++;
                        if (i < 3)
                        {
                            Debug.WriteLine("Trying HTTPS request again");
                        }
                        else
                        {
                            response = "unknown";
                        }
                    }
                }
            }
            response = response.Replace("\n", "").Replace("\r", "");
            return response;
        }
    }
}
