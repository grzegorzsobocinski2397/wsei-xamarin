using AirMonitor.Models;
using AirMonitor.Services;
using AirMonitor.ViewModels.Base;
using AirMonitor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
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
		/// Whether the application is currently loading data.
		/// </summary>
		public bool IsLoading { get; private set; } = true;

		/// <summary>
		/// Whether the application is requesting/reading new data after user had used refresh command.
		/// </summary>
		public bool IsRefreshing { get; private set; } = false;

		/// <summary>
		/// List of measurements for nearby stations.
		/// </summary>
		public List<Measurement> Measurements { get; private set; }

		/// <summary>
		/// Request new measurements.
		/// </summary>
		public ICommand RefreshMeasurementsCommand { get; set; }

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

			RefreshMeasurementsCommand = new RelayCommand(async () => await RefreshData());
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
		/// Send requests to Airly endpoints with current location or read data from database .
		/// </summary>
		private async void Initialize()
		{
			IsLoading = true;

			if (App.DatabaseHelper.IsDataValid())
			{
				await Task.Run(() => Measurements = App.DatabaseHelper.ReadMeasurements().ToList());
			}
			else
			{
				await Task.Run(() => RequestData());
			}

			IsLoading = false;
		}

		private async Task RequestData()
		{
			Location location = await GetLocation();
			IEnumerable<Installation> stations = await GetStations(location);
			App.DatabaseHelper.SaveInstallations(stations);

			IEnumerable<Measurement> measurements = await GetMeasurements(stations);
			Measurements = new List<Measurement>(measurements);
			App.DatabaseHelper.SaveMeasurements(Measurements);
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
		private async Task<IEnumerable<Installation>> GetStations(Location location)
		{
			return await airlyDataService.GetNearestData(location);
		}

		/// <summary>
		/// Get the measurements for available stations.
		/// </summary>
		/// <param name="stations">Stations near the user's location.</param>
		private async Task<IEnumerable<Measurement>> GetMeasurements(IEnumerable<Installation> stations)
		{
			List<Measurement> measurements = new List<Measurement>();

			foreach (Installation station in stations)
			{
				Measurement measurement = await airlyDataService.GetMeasurements(station.Id);
				if (measurement == null)
				{
					continue;
				}

				measurement.Installation = station;
				measurement.CurrentDisplayValue = Convert.ToInt32(Math.Round(measurement.Current.Indexes.FirstOrDefault().Value));

				measurements.Add(measurement);
			}

			return measurements;
		}

		/// <summary>
		/// Request new data from the Airly endpoints.
		/// </summary>
		/// <returns></returns>
		private async Task RefreshData()
		{
			IsRefreshing = true;

			await Task.Run(() => RequestData());

			IsRefreshing = false;
		}

		#endregion Private Methods
	}
}