using AirMonitor.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
		/// <summary>
		/// Send a request for measurements for a specific station.
		/// </summary>
		/// <param name="id">Id of the station.</param>
		public async Task<Measurement> GetMeasurements(string id)
		{
			using (HttpClient client = GetHttpClient())
			{
				Dictionary<string, string> queryString = new Dictionary<string, string>() { { "installationId", id } };

				string uri = CreateURI(App.ApiMeasurementsUrl, queryString);
				HttpResponseMessage response = await client.GetAsync(uri);

				string content = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<Measurement>(content);
			}
		}

		/// <summary>
		/// Send a request for nearest airly station based on the latitude and longitude.
		/// </summary>
		/// <param name="id">Current user's location.</param>
		/// <param name="maxResults">The maximum amount of stations.</param>
		public async Task<IEnumerable<Station>> GetNearestData(Location location, string maxResults = "1")
		{
			using (HttpClient client = GetHttpClient())
			{
				Dictionary<string, string> queryString = new Dictionary<string, string>() { { "lat", location.Latitude.ToString() }, { "lng", location.Longitude.ToString() }, { "maxResults", maxResults } };

				string uri = CreateURI(App.ApiNearestUrl, queryString);
				HttpResponseMessage response = await client.GetAsync(uri);

				string content = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<Station>>(content);
			}
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
	}
}