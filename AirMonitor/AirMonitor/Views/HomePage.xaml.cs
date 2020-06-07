using AirMonitor.Models;
using AirMonitor.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirMonitor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			InitializeComponent();

			BindingContext = new HomeViewModel(Navigation);
		}

		private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			(BindingContext as HomeViewModel).NavigateToDetailsPage((Measurement)e.Item);
		}
	}
}