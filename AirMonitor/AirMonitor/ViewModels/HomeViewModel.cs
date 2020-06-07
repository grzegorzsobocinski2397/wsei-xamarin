using AirMonitor.Models;
using AirMonitor.Services;
using AirMonitor.ViewModels.Base;
using AirMonitor.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirMonitor.ViewModels
{
	internal class HomeViewModel : BaseViewModel
	{
		private readonly INavigation navigation;

		/// <summary>
		/// Command that will navigate the user to a Home Page.
		/// </summary>
		public ICommand NavigateToDetailsCommand { get; set; }

		public HomeViewModel(INavigation navigation)
		{
			this.navigation = navigation;

			Initialize();

			NavigateToDetailsCommand = new RelayCommand(() => NavigateToDetailsPage());
		}

		private async void Initialize()
		{
			var location = await GetLocation();
			var stations = await GetStations(location);
			var measurements = await GetMeasurements(stations);
		}

		private async Task<IEnumerable<Measurement>> GetMeasurements(IEnumerable<Station> stations)
		{
			var measurements = new List<Measurement>();
			AirlyDataService airlyDataService = new AirlyDataService();
			foreach (var station in stations)
			{
				var measurement = await airlyDataService.GetMeasurements(station.Id);

				measurements.Add(measurement);
			}

			return measurements;
		}

		private async Task<IEnumerable<Station>> GetStations(Location location)
		{
			AirlyDataService airlyDataService = new AirlyDataService();
			return await airlyDataService.GetNearestData(location);
		}

		private async Task<Location> GetLocation()
		{
			return await Geolocation.GetLastKnownLocationAsync();
		}

		/// <summary>
		/// Move the user to Details page.
		/// </summary>
		private void NavigateToDetailsPage()
		{
			navigation.PushAsync(new DetailsPage());
		}
	}
}