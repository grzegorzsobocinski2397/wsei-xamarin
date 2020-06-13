using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AirMonitor.Helpers
{
	public class ApiHelper
	{
		#region Properties

		/// <summary>
		/// API key to the Airly endpoint. Provided in the config.json
		/// </summary>
		public string ApiKey { get; private set; }

		/// <summary>
		/// URL exposed for the developers by Airly team. Provided in the config.json
		/// </summary>
		public string ApiUrl { get; private set; }

		/// <summary>
		/// Endpoint for the nearest station. Provided in the config.json
		/// </summary>
		public string ApiNearestUrl { get; private set; }

		/// <summary>
		/// Endpoint for the measurements based on the station id. Provided in the config.json
		/// </summary>
		public string ApiMeasurementsUrl { get; private set; }

		#endregion Properties

		#region Private Fields

		/// <summary>
		/// File name that contains all the api keys.
		/// </summary>
		private const string CONFIGURATION_FILE_NAME = "config.json";

		#endregion Private Fields

		#region Public Methods

		/// <summary>
		/// Load the configuration with URLs.
		/// </summary>
		public async Task LoadConfig()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(App));
			string[] resourceNames = assembly.GetManifestResourceNames();
			string configName = resourceNames.FirstOrDefault(s => s.Contains(CONFIGURATION_FILE_NAME));

			using (Stream stream = assembly.GetManifestResourceStream(configName))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					string json = await reader.ReadToEndAsync();
					JObject dynamicJson = JObject.Parse(json);

					ApiKey = dynamicJson["apiKey"].Value<string>();
					ApiUrl = dynamicJson["apiUrl"].Value<string>();
					ApiNearestUrl = dynamicJson["apiNearest"].Value<string>();
					ApiMeasurementsUrl = dynamicJson["apiMeasurements"].Value<string>();
				}
			}
		}

		#endregion Public Methods
	}
}