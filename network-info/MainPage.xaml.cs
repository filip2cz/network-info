using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace network_info
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        #region props
        private string _ipv4;
        public string Ipv4 {
            get => _ipv4;
            set {
                _ipv4 = value;
                OnPropertyChanged(nameof(Ipv4));
            }
        }
        private string _ipv4Local;
        public string Ipv4Local {
            get => _ipv4Local;
            set {
                _ipv4Local = value;
                OnPropertyChanged(nameof(Ipv4Local));
            }
        }
        private string _ipv6;
        public string Ipv6 {
            get => _ipv6;
            set {
                _ipv6 = value;
                OnPropertyChanged(nameof(Ipv6));
            }
        }
        private string _ipv6Local;
        public string Ipv6Local {
            get => _ipv6Local;
            set {
                _ipv6Local = value;
                OnPropertyChanged(nameof(Ipv6Local));
            }
        }
        private string _connectionType;
        public string ConnectionType {
            get => _connectionType;
            set {
                _connectionType = value;
                OnPropertyChanged(nameof(ConnectionType));
            }
        }
        private string _versionStatus;
        public string VersionStatus {
            get => _versionStatus;
            set {
                _versionStatus = value;
                OnPropertyChanged(nameof(VersionStatus));
            }
        }
        private string _country4;
        public string Country4 {
            get => _country4;
            set {
                _country4 = value;
                OnPropertyChanged(nameof(Country4));
            }
        }
        private string _country6;
        public string Country6 {
            get => _country6;
            set {
                _country6 = value;
                OnPropertyChanged(nameof(Country6));
            }
        }
        private string _isp4;
        public string Isp4 {
            get => _isp4;
            set {
                _isp4 = value;
                OnPropertyChanged(nameof(Isp4));
            }
        }
        private string _isp6;
        public string Isp6 {
            get => _isp6;
            set {
                _isp6 = value;
                OnPropertyChanged(nameof(Isp6));
            }
        }
        private string _vpn4;
        public string Vpn4 {
            get => _vpn4;
            set {
                _vpn4 = value;
                OnPropertyChanged(nameof(Vpn4));
            }
        }
        private string _vpn6;
        public string Vpn6 {
            get => _vpn6;
            set {
                _vpn6 = value;
                OnPropertyChanged(nameof(Vpn6));
            }
        }
        #endregion

        private bool ipv4available;
        private bool ipv6available;
        private string json4;
        private string json6;

        public MainPage() {
            InitializeComponent();

            // basic bool variables

            Debug.WriteLine("app started");

            // check version
            Task.Run(UpdaterFunction);

            Debug.WriteLine("button created");

            // get data
            BindingContext = this;

            Debug.WriteLine("end of MainPage()");
        }

        private async Task UpdaterFunction() {
            Debug.WriteLine("Checking version from server");

            int yourVersion = 3;
            int latestVersion = yourVersion;
            using (WebClient client = new WebClient()) {
                try {
                    latestVersion = int.Parse(await client.DownloadStringTaskAsync("https://raw.githubusercontent.com/filip2cz/network-info/main/ver"));
                }
                catch (Exception) {
                    // ignored
                }
            }
            VersionStatus = yourVersion == latestVersion ? "You have latest version." : "You do not have latest version of app, consider update.";

            Debug.WriteLine("Checking version from server done");
        }

        private async Task<T> GetWithRetries<T>(Func<Task<T>> func, int maxRetries = 3, Action<Exception> onError = null) where T : class {
            for (int retries = 0; retries < maxRetries; retries++) {
                try {
                    return await func();
                }
                catch (Exception ex) {
                    onError?.Invoke(ex);
                }
            }

            return null;
        }

        // thanks chatgpt for GetLocalIPv* functions
        public static string GetLocalIPv4() {
            string ipAddress = null;
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces()) {
                foreach (var addressInfo in netInterface.GetIPProperties().UnicastAddresses) {
                    if (addressInfo.Address.AddressFamily != AddressFamily.InterNetwork) continue;

                    ipAddress = addressInfo.Address.ToString();
                    break;
                }

                if (ipAddress != null)
                    break;
            }
            return ipAddress;
        }

        public static string GetLocalIPv6() {
            string ipAddress = null;
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces()) {
                foreach (var addressInfo in netInterface.GetIPProperties().UnicastAddresses) {
                    if (addressInfo.Address.AddressFamily != AddressFamily.InterNetworkV6) continue;

                    ipAddress = addressInfo.Address.ToString();
                    break;
                }
                if (ipAddress != null)
                    break;
            }
            return ipAddress;
        }

        public async Task Ipv4Function() {

            Debug.WriteLine("Ipv4Function() started");

            Ipv4 = "getting data";

            Debug.WriteLine("Ipv4 = \"getting data\"");

            const string Ipv4Url = "https://v4.ipv6-test.com/api/myip.php";

            // get IPv4
            using (WebClient client = new TimedWebClient { Timeout = 4000 }) {
                string response = (await GetWithRetries(async () => await client.DownloadStringTaskAsync(Ipv4Url),
                    onError: ex => {
                        Debug.WriteLine("IPv4 request failed");
                        Debug.WriteLine(ex);
                    }));
                Ipv4 = response ?? "unknown";
                ipv4available = response != null;
            }

            Debug.WriteLine("getting IPv4 done");
            Debug.WriteLine($"Ipv4 = {Ipv4}");
        }

        public async Task Ipv6Function() {
            Debug.WriteLine("Ipv6Function() started");

            Ipv6 = "getting data";

            Debug.WriteLine("Ipv6 = \"getting data\"");

            const string Ipv6Url = "https://v6.ipv6-test.com/api/myip.php";

            // get IPv6
            using (WebClient client = new TimedWebClient { Timeout = 4000 }) {
                string response = (await GetWithRetries(async () => await client.DownloadStringTaskAsync(Ipv6Url),
                    onError: ex => {
                        Debug.WriteLine("IPv6 request failed");
                        Debug.WriteLine(ex);
                    }));
                Ipv6 = response ?? "unknown";
                ipv6available = response != null;
            }

            Debug.WriteLine("getting IPv6 done");
            Debug.WriteLine($"Ipv6 = {Ipv6}");
        }

        public async Task Ipv4Info() {
            Debug.WriteLine("getting info about IPv4 started");

            if (!ipv4available) {
                Debug.WriteLine("getting data about IPv4 skipped because IPv4 is not available");
                Country4 = "unknown";
                Isp4 = "unknown";
                Vpn4 = "unknown";
                return;
            }

            Debug.WriteLine("getting data about IPv4");
            Debug.WriteLine("starting Country4 request");
            using (WebClient client = new TimedWebClient()) {
                string url = $"http://ip-api.com/line/{Ipv4}?fields=country";
                Country4 = (await GetWithRetries(async () => await client.DownloadStringTaskAsync(url),
                        onError: ex => {
                            Debug.WriteLine("Country4 request failed");
                            Debug.WriteLine(ex);
                        })
                    )?.Replace("\n", "").Replace("\r", "") ?? "unknown";
            }

            Debug.WriteLine("starting Isp4 request");
            using (WebClient client = new TimedWebClient()) {
                string url = $"http://ip-api.com/line/{Ipv4}?fields=isp";
                Isp4 = (await GetWithRetries(async () => await client.DownloadStringTaskAsync(url),
                        onError: ex => {
                            Debug.WriteLine("Isp4 request failed");
                            Debug.WriteLine(ex);
                        })
                    )?.Replace("\n", "").Replace("\r", "") ?? "unknown";
            }

            Debug.WriteLine("starting Vpn4 request");
            using (WebClient client = new TimedWebClient()) {
                string url = $"http://ip-api.com/line/{Ipv4}?fields=proxy";
                Vpn4 = (await GetWithRetries(async () => await client.DownloadStringTaskAsync(url),
                        onError: ex => {
                            Debug.WriteLine("Vpn4 request failed");
                            Debug.WriteLine(ex);
                        })
                    )?.Replace("\n", "").Replace("\r", "") ?? "unknown";
            }

            Debug.WriteLine("getting info about IPv4 done");
        }

        public async Task Ipv6Info() {
            Debug.WriteLine("getting info about IPv6 started");

            if (!ipv6available) {
                Debug.WriteLine("getting data about IPv6 skipped because IPv6 is not available");
                Country6 = "unknown";
                Isp6 = "unknown";
                Vpn6 = "unknown";
                return;
            }

            Debug.WriteLine("getting data about IPv6");
            Debug.WriteLine("starting Country6 request");
            using (WebClient client = new TimedWebClient()) {
                string url = $"http://ip-api.com/line/{Ipv6}?fields=country";
                Country6 = (await GetWithRetries(async () => await client.DownloadStringTaskAsync(url),
                        onError: ex => {
                            Debug.WriteLine("Country6 request failed");
                            Debug.WriteLine(ex);
                        })
                    )?.Replace("\n", "").Replace("\r", "") ?? "unknown";
            }

            Debug.WriteLine("starting Isp6 request");
            using (WebClient client = new TimedWebClient()) {
                string url = $"http://ip-api.com/line/{Ipv6}?fields=isp";
                Isp6 = (await GetWithRetries(async () => await client.DownloadStringTaskAsync(url),
                        onError: ex => {
                            Debug.WriteLine("Isp6 request failed");
                            Debug.WriteLine(ex);
                        })
                    )?.Replace("\n", "").Replace("\r", "") ?? "unknown";
            }

            Debug.WriteLine("starting Vpn6 request");
            using (WebClient client = new TimedWebClient()) {
                string url = $"http://ip-api.com/line/{Ipv6}?fields=proxy";
                Vpn6 = (await GetWithRetries(async () => await client.DownloadStringTaskAsync(url),
                       onError: ex => {
                           Debug.WriteLine("Vpn6 request failed");
                           Debug.WriteLine(ex);
                       })
                    )?.Replace("\n", "").Replace("\r", "") ?? "unknown";
            }

            Debug.WriteLine("getting info about IPv4 done");
        }


        private async void RefreshButton_Clicked(object sender, EventArgs e) {
            RefreshButton.IsEnabled = false;

            Debug.WriteLine("reload button pressed");
            Ipv4Local = GetLocalIPv4();
            Ipv6Local = GetLocalIPv6();
            ConnectionType = "work in progress";
            if (Ipv4Local == null) {
                Ipv4 = "unknown";
            } else {
                await Ipv4Function();
            }
            if (Ipv6Local == null) {
                Ipv6 = "unknown";
            } else {
                await Ipv6Function();
            }

            await Ipv4Info();
            await Ipv6Info();

            await UpdaterFunction();

            RefreshButton.IsEnabled = true;
        }
    }
}
