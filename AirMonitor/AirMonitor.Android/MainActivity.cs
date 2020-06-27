using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace AirMonitor.Droid
{
	[Activity(Label = "AirMonitor", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		private const int RequestLocationId = 0;

		private readonly string[] locationPermissions =
		{
			Manifest.Permission.AccessCoarseLocation,
			Manifest.Permission.AccessFineLocation
		};

		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			Xamarin.FormsMaps.Init(this, savedInstanceState);
			LoadApplication(new App());
		}

		protected override void OnStart()
		{
			base.OnStart();

			if ((int)Build.VERSION.SdkInt >= 23)
			{
				if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
				{
					RequestPermissions(locationPermissions, RequestLocationId);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Localization already permission granted.");
				}
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			if (requestCode == RequestLocationId)
			{
				if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
				{
					System.Diagnostics.Debug.WriteLine("Localization permission granted.");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Localization permission not granted.");
				}
			}
			else
			{
				base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			}
		}
	}
}