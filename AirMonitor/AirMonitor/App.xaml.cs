using AirMonitor.Helpers;
using AirMonitor.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AirMonitor
{
	public partial class App : Application
	{
		/// <summary>
		/// Helper that reads the configuration file for Airly API keys and endpoints.
		/// </summary>
		public static ApiHelper ApiHelper { get; private set; }

		/// <summary>
		/// Helper that connects to the SQLite database.
		/// </summary>
		public static DatabaseHelper DatabaseHelper { get; private set; }

		public App()
		{
			DatabaseHelper = new DatabaseHelper();
			ApiHelper = new ApiHelper();

			InitializeComponent();

			InitializeApp();
		}

		/// <summary>
		/// Load the configuration and set the main page.
		/// </summary>
		private async Task InitializeApp()
		{
			await ApiHelper.LoadConfig();

			MainPage = new TabPage();
		}

		protected override void OnStart()
		{
			if (DatabaseHelper == null)
			{
				DatabaseHelper = new DatabaseHelper();
			}
		}

		protected override void OnSleep()
		{
			if (DatabaseHelper != null)
			{
				DatabaseHelper.Dispose();
				DatabaseHelper = null;
			}
		}

		protected override void OnResume()
		{
			if (DatabaseHelper == null)
			{
				DatabaseHelper = new DatabaseHelper();
			}
		}
	}
}