using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirMonitor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Move the user to the Home Page.
		/// </summary>
		private void NavigateToHomePage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new DetailsPage());
		}
	}
}