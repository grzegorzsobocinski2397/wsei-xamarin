using AirMonitor.Models;
using AirMonitor.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace AirMonitor.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage(Measurement measurement)
        {
            InitializeComponent();

            BindingContext = new DetailsViewModel();
            (BindingContext as DetailsViewModel).Measurement = measurement;
        }

        private void Help_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Co to jest CAQI?", "Lorem ipsum.", "Zamknij");
        }
    }
}
