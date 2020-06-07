using AirMonitor.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AirMonitor.Services
{
	/// <summary>
	/// Service responsible for sending out requests to the Airly endpoints.
	/// </summary>
	public class AirlyDataService
	{
		#region Public Properties

		/// <summary>
		/// Number of requests left for current day.
		/// </summary>
		public int RequestsCount { get; private set; }

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Send a request for measurements for a specific station.
		/// </summary>
		/// <param name="id">Id of the station.</param>
		public async Task<Measurement> GetMeasurements(string id)
		{
			Dictionary<string, string> queryString = new Dictionary<string, string>() { { "installationId", id } };

			string uri = CreateURI(App.ApiMeasurementsUrl, queryString);

			return await SendRequest<Measurement>(uri);
		}

		/// <summary>
		/// Send a request for nearest airly station based on the latitude and longitude.
		/// </summary>
		/// <param name="id">Current user's location.</param>
		/// <param name="maxResults">The maximum amount of stations.</param>
		public async Task<IEnumerable<Station>> GetNearestData(Location location, string maxResults = "1")
		{
			Dictionary<string, string> queryString = new Dictionary<string, string>() { { "lat", location.Latitude.ToString() }, { "lng", location.Longitude.ToString() }, { "maxResults", maxResults } };

			string uri = CreateURI(App.ApiNearestUrl, queryString);

			return await SendRequest<IEnumerable<Station>>(uri);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Send a request to Airly endpoints and deserialize the response.
		/// </summary>
		/// <typeparam name="T">Type of the endpoint response.</typeparam>
		/// <param name="uri">Address to the Airly endpoint with params specified.</param>
		private async Task<T> SendRequest<T>(string uri)
		{
			using (HttpClient client = GetHttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				LogRequestsLeft(response.Headers);
				string content = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(content);
			}
		}

		/// <summary>
		/// Save the information about requests left for a day.
		/// </summary>
		private void LogRequestsLeft(HttpResponseHeaders headers)
		{
			headers.TryGetValues("X-RateLimit-Remaining-day", out var limits);
			RequestsCount = int.Parse(limits.FirstOrDefault());
			System.Diagnostics.Debug.WriteLine($"Requests left: {RequestsCount}");
		}

		/// <summary>
		/// Create a base HtppClient with autharization header set.
		/// </summary>
		private HttpClient GetHttpClient()
		{
			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Add("apikey", App.ApiKey);

			return client;
		}

		/// <summary>
		/// Create URI based on the endpoint and queries specified.
		/// </summary>
		/// <param name="endpoint">Endpoint of the Airly Developer.</param>
		/// <param name="queries">Parameters required for the endpoint.</param>
		private string CreateURI(string endpoint, Dictionary<string, string> queries)
		{
			UriBuilder uri = new UriBuilder(App.ApiUrl);
			uri.Path += endpoint;
			uri.Port = -1;
			return QueryHelpers.AddQueryString(uri.ToString(), queries);
		}

		#endregion Private Methods
	}
}