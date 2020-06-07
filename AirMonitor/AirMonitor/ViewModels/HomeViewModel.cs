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
		#region Properties

		/// <summary>
		/// Service responsible for sending requests to Airly endpoints.
		/// </summary>
		private readonly AirlyDataService airlyDataService = new AirlyDataService();

		/// <summary>
		/// Xamarin navigation.
		/// </summary>
		private readonly INavigation navigation;

		/// <summary>
		/// Command that will navigate the user to a Home Page.
		/// </summary>
		public ICommand NavigateToDetailsCommand { get; set; }

		#endregion Properties

		#region Constructor

		public HomeViewModel(INavigation navigation)
		{
			this.navigation = navigation;

			Initialize();

			NavigateToDetailsCommand = new RelayCommand(() => NavigateToDetailsPage());
		}

		#endregion Constructor

		#region Private Methods

		/// <summary>
		/// Send requests to Airly endpoints with current location.
		/// </summary>
		private async void Initialize()
		{
			var location = await GetLocation();
			var stations = await GetStations(location);
			var measurements = await GetMeasurements(stations);
		}

		/// <summary>
		/// Get current user's location.
		/// </summary>
		private async Task<Location> GetLocation()
		{
			return await Geolocation.GetLastKnownLocationAsync();
		}

		/// <summary>
		/// Send a request to Airly for nearest stations.
		/// </summary>
		/// <param name="location">Current user's location</param>
		private async Task<IEnumerable<Station>> GetStations(Location location)
		{
			return await airlyDataService.GetNearestData(location);
		}

		/// <summary>
		/// Get the measurements for available stations.
		/// </summary>
		/// <param name="stations">Stations near the user's location.</param>
		private async Task<IEnumerable<Measurement>> GetMeasurements(IEnumerable<Station> stations)
		{
			List<Measurement> measurements = new List<Measurement>();

			foreach (Station station in stations)
			{
				Measurement measurement = await airlyDataService.GetMeasurements(station.Id);

				measurements.Add(measurement);
			}

			return measurements;
		}

		/// <summary>
		/// Move the user to Details page.
		/// </summary>
		private void NavigateToDetailsPage()
		{
			navigation.PushAsync(new DetailsPage());
		}

		#endregion Private Methods
	}
}