using AirMonitor.Helpers;
using AirMonitor.Views;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AirMonitor
{
	public partial class App : Application
	{
		/// <summary>
		/// API key to the Airly endpoint. Provided in the config.json
		/// </summary>
		public static string ApiKey { get; private set; }

		/// <summary>
		/// URL exposed for the developers by Airly team. Provided in the config.json
		/// </summary>
		public static string ApiUrl { get; private set; }

		/// <summary>
		/// Endpoint for the nearest station. Provided in the config.json
		/// </summary>
		public static string ApiNearestUrl { get; private set; }

		/// <summary>
		/// Endpoint for the measurements based on the station id. Provided in the config.json
		/// </summary>
		public static string ApiMeasurementsUrl { get; private set; }

		/// <summary>
		/// Helper that connects to the SQLite database.
		/// </summary>
		public static DatabaseHelper DatabaseHelper { get; private set; }

		public App()
		{
			DatabaseHelper = new DatabaseHelper();

			InitializeComponent();

			InitializeApp();
		}

		/// <summary>
		/// Load the configuration and set the main page.
		/// </summary>
		private async Task InitializeApp()
		{
			await LoadConfig();

			MainPage = new TabPage();
		}

		/// <summary>
		/// Load the configuration with URLs.
		/// </summary>
		private static async Task LoadConfig()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(App));
			string[] resourceNames = assembly.GetManifestResourceNames();
			string configName = resourceNames.FirstOrDefault(s => s.Contains("config.json"));

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

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}