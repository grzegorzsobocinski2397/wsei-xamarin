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

			string uri = CreateURI(App.ApiHelper.ApiMeasurementsUrl, queryString);

			return await SendRequest<Measurement>(uri);
		}

		/// <summary>
		/// Send a request for nearest airly station based on the latitude and longitude.
		/// </summary>
		/// <param name="id">Current user's location.</param>
		/// <param name="maxResults">The maximum amount of stations.</param>
		public async Task<IEnumerable<Installation>> GetNearestData(Location location, string maxResults = "3")
		{
			if (location == null)
			{
				System.Diagnostics.Debug.WriteLine("Location not found.");
				return null;
			}

			Dictionary<string, string> queryString = new Dictionary<string, string>() { { "lat", location.Latitude.ToString() }, { "lng", location.Longitude.ToString() }, { "maxResults", maxResults } };

			string uri = CreateURI(App.ApiHelper.ApiNearestUrl, queryString);

			return await SendRequest<IEnumerable<Installation>>(uri);
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
				try
				{
					HttpResponseMessage response = await client.GetAsync(uri);
					LogRequestsLeft(response.Headers);

					switch ((int)response.StatusCode)
					{
						case 200:
							string content = await response.Content.ReadAsStringAsync();
							return JsonConvert.DeserializeObject<T>(content);

						case 429:
							System.Diagnostics.Debug.WriteLine("Hit daily limit for requests.");
							break;

						default:
							string errorContent = await response.Content.ReadAsStringAsync();
							System.Diagnostics.Debug.WriteLine($"Error: {errorContent}");
							return default;
					}
				}
				catch (ArgumentNullException ane)
				{
					System.Diagnostics.Debug.WriteLine(ane.Message);
				}
				catch (HttpRequestException hre)
				{
					System.Diagnostics.Debug.WriteLine(hre.Message);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}

				return default;
			}
		}

		/// <summary>
		/// Save the information about requests left for a day.
		/// </summary>
		private void LogRequestsLeft(HttpResponseHeaders headers)
		{
			headers.TryGetValues("X-RateLimit-Remaining-day", out var limits);
			try
			{
				RequestsCount = int.Parse(limits.FirstOrDefault());
				System.Diagnostics.Debug.WriteLine($"Requests left: {RequestsCount}");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error during reading X-RateLimit header. ${ex.Message}");
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
			client.DefaultRequestHeaders.Add("apikey", App.ApiHelper.ApiKey);

			return client;
		}

		/// <summary>
		/// Create URI based on the endpoint and queries specified.
		/// </summary>
		/// <param name="endpoint">Endpoint of the Airly Developer.</param>
		/// <param name="queries">Parameters required for the endpoint.</param>
		private string CreateURI(string endpoint, Dictionary<string, string> queries)
		{
			UriBuilder uri = new UriBuilder(App.ApiHelper.ApiUrl);
			uri.Path += endpoint;
			uri.Port = -1;
			return QueryHelpers.AddQueryString(uri.ToString(), queries);
		}

		#endregion Private Methods
	}
}