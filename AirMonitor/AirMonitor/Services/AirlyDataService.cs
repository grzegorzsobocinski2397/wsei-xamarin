using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AirMonitor.Services
{
	public class AirlyDataService
	{

		/// <summary>
		/// Send a request for nearest airly station based on the latitude and longitude.
		/// </summary>
		public async void GetNearestData(string latitude, string longitude, string maxResults = "1")
		{
			using (HttpClient client = new HttpClient())
			{
				Dictionary<string, string> queryString = new Dictionary<string, string>()
				{  { "lat", latitude },  { "lng", longitude }, { "maxResults", maxResults} };

				client.BaseAddress = new Uri(App.ApiNearestUrl);

				string requestUri = QueryHelpers.AddQueryString(App.ApiUrl, queryString);

				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				client.DefaultRequestHeaders.Add("apikey", App.ApiKey);

				HttpResponseMessage response = await client.GetAsync(requestUri);

				if (response.IsSuccessStatusCode)
				{
					Console.WriteLine(response.Content);
				}
				else
				{
					Console.WriteLine("Internal server Error");
				}
			}
		}
	}
}