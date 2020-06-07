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
		/// Send a request for measurements for specific station.
		/// </summary>
		/// <param name="id">Id of the station.</param>
		public async Task<Measurement> GetMeasurements(string id)
		{
			using (HttpClient client = new HttpClient())
			{
				Dictionary<string, string> queryString = new Dictionary<string, string>()
				{  {"installationId", id } };

				client.BaseAddress = new Uri(App.ApiUrl);

				string requestUri = QueryHelpers.AddQueryString(App.ApiMeasurementsUrl, queryString);

				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				client.DefaultRequestHeaders.Add("apikey", App.ApiKey);

				HttpResponseMessage response = await client.GetAsync(requestUri);

				var content = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<Measurement>(content);
				return result;
			}
		}

		/// <summary>
		/// Send a request for nearest airly station based on the latitude and longitude.
		/// </summary>
		public async Task<IEnumerable<Station>> GetNearestData(Location location, string maxResults = "1")
		{
			using (HttpClient client = new HttpClient())
			{
				Dictionary<string, string> queryString = new Dictionary<string, string>()
				{  { "lat", location.Latitude.ToString() },  { "lng", location.Longitude.ToString() }, { "maxResults", maxResults} };

				client.BaseAddress = new Uri(App.ApiUrl);

				string requestUri = QueryHelpers.AddQueryString(App.ApiNearestUrl, queryString);

				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				client.DefaultRequestHeaders.Add("apikey", App.ApiKey);

				HttpResponseMessage response = await client.GetAsync(requestUri);

				var content = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<IEnumerable<Station>>(content);
				return result;
			}
		}
	}
}