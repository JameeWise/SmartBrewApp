using System.ComponentModel;
using SmartBrewApp.ViewModels;
using Xamarin.Forms;

namespace SmartBrewApp.Views
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