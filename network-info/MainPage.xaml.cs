using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Xamarin.Essentials;

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

        public MainPage() {
            InitializeComponent();
            BindingContext = this;
            MainThread.InvokeOnMainThreadAsync(() => DataAsync());
        }
        public async Task DataAsync() {
            Ipv4 = "getting data";
            Ipv6 = "getting data";
            // zjištění IPv4 a IPv6
            HttpClient client = new HttpClient();
            HttpResponseMessage responseipv4 = await client.GetAsync("https://v4v6.ipv6-test.com/api/myip.php");
            //HttpResponseMessage responseipv6 = await client.GetAsync("https://v6.ipv6-test.com/api/myip.php");

            if (responseipv4.IsSuccessStatusCode) {
                Ipv4 = await responseipv4.Content.ReadAsStringAsync();
            }
            else {
                Ipv4 = "unknown";
            }
            /*if (responseipv6.IsSuccessStatusCode)
            {
                Ipv6 = await responseipv6.Content.ReadAsStringAsync();
            }
            else
            {*/
                Ipv6 = "is not supported yet";
            //}
        }
    }
}
