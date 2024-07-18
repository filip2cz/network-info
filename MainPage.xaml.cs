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
            Isp4.Text = "getting data";

            Country6.Text = "getting data";
            Isp6.Text = "getting data";

            versionStatus.Text = await UpdaterFunction();
            Ipv4Local.Text = await GetLocalIPv4();
            Ipv6Local.Text = await GetLocalIPv6();

            Ipv4.Text = await GetIPv4();
            Ipv6.Text = await GetIPv6();

            if (ipv4avaible)
            {
                Country4.Text = await GetCountry(Ipv4.Text);
                Isp4.Text = await GetIsp(Ipv4.Text);
            }
            else
            {
                Country4.Text = "unknown";
                Isp4.Text = "unknown";
            }

            if (ipv6avaible)
            {
                Country6.Text = await GetCountry(Ipv6.Text);
                Isp6.Text = await GetIsp(Ipv6.Text);
            }
            else
            {
                Country6.Text = "unknown";
                Isp6.Text = "unknown";
            }
        }
        public async Task<string> UpdaterFunction()
        {
            Debug.WriteLine("Checking version from server");

            int yourVersion = 3;
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
                return "You have latest version.";
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
        public async Task<string> MakeWebRequest(string url)
        {
            string response = string.Empty;
            int i = 0;
            while (i < 3)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        response = await client.GetStringAsync(url);
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
