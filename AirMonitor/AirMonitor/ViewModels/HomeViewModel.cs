using AirMonitor.Models;
using AirMonitor.Services;
using AirMonitor.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirMonitor.ViewModels
{
	internal class HomeViewModel : BaseViewModel
	{
		#region Properties

		/// <summary>
		/// List of measurements for nearby stations.
		/// </summary>
		public List<Measurement> Measurements { get; private set; }

		/// <summary>
		/// Service responsible for sending requests to Airly endpoints.
		/// </summary>
		private readonly AirlyDataService airlyDataService = new AirlyDataService();

		/// <summary>
		/// Xamarin navigation.
		/// </summary>
		private readonly INavigation navigation;

		#endregion Properties

		#region Constructor

		public HomeViewModel(INavigation navigation)
		{
			this.navigation = navigation;

			Initialize();
		}

		#endregion Constructor

		#region Public Methods

		/// <summary>
		/// Move the user to Details page.
		/// </summary>
		public void NavigateToDetailsPage(Measurement measurement)
		{
			navigation.PushAsync(new DetailsPage(measurement));
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Send requests to Airly endpoints with current location.
		/// </summary>
		private async void Initialize()
		{
			Location location = await GetLocation();
			IEnumerable<Station> stations = await GetStations(location);
			IEnumerable<Measurement> measurements = await GetMeasurements(stations);
			Measurements = new List<Measurement>(measurements);
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

		#endregion Private Methods
	}
}