using System;
using System.Collections.Generic;
using System.ComponentModel;
using SmartBrewApp.Models;
using SmartBrewApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartBrewApp.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}