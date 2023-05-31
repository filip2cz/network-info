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

        public MainPage() {
            InitializeComponent();

            debugEvent = "app started";
            
            // refresh button
            Button refreshButton = new Button
            {
                Text = "Data refresh"
            };

            refreshButton.Clicked += RefreshButton_Clicked;
            /*
            Content = new StackLayout
            {
                Children = { refreshButton }
            };*/

            debugEvent = "button created";

            // get data
            BindingContext = this;
            MainThread.InvokeOnMainThreadAsync(() => Ipv4Function());

            debugEvent = "end of MainPage()";
        }
        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            // Volání funkce DataAsync pro načtení nových dat
            debugEvent = "reload button pressed";
            await Ipv4Function();
        }
        public async Task Ipv4Function() {

            debugEvent = "Ipv4Function() started";
            
            Ipv4 = "getting data";

            debugEvent = "Ipv4 = \"getting data\"";

            // get IPv4
            // this is temporary both IPv4 and IPv6
            string Ipv4Url = "https://v4v6.ipv6-test.com/api/myip.php";
            string responseipv4 = string.Empty;

            bool i = true;
            while (i)
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        responseipv4 = client.DownloadString(Ipv4Url);
                        i = false;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            debugEvent = "getting IPv4 done";
            Ipv4 = responseipv4;
            debugEvent = "Ipv4 = ip";
            /*
            else {
                Ipv4 = "unknown";
                debugEvent = "Ipv4 = \"unknown\"";
            }
            */
        }
        public async Task Ipv6Function()
        {
            Ipv6 = "getting data";
            // zjištění a IPv6
            HttpClient client = new HttpClient();
            HttpResponseMessage responseipv6 = await client.GetAsync("https://v6.ipv6-test.com/api/myip.php");
            if (responseipv6.IsSuccessStatusCode)
            {
                Ipv6 = await responseipv6.Content.ReadAsStringAsync();
            }
            else
            {
            Ipv6 = "unknown";
            }
        }
    }
}
