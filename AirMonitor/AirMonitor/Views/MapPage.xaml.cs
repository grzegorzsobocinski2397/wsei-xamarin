using AirMonitor.Models.Map;
using AirMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirMonitor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
		public MapPage()
		{
			InitializeComponent();

			BindingContext = new HomeViewModel(Navigation);
		}

		private void Pin_InfoWindowClicked(object sender, Xamarin.Forms.Maps.PinClickedEventArgs e)
		{
			Xamarin.Forms.Maps.Pin pin = (sender as Xamarin.Forms.Maps.Pin);
			MapLocation mapLocation = (MapLocation)pin.BindingContext;
			(BindingContext as HomeViewModel).NavigatePinToDetialsCommand.Execute(mapLocation.InstallationId);
		}
	}
}