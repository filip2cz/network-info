using network_info.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace network_info.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}