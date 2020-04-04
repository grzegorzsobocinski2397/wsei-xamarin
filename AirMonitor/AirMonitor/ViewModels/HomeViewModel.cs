using AirMonitor.ViewModels.Base;
using AirMonitor.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace AirMonitor.ViewModels
{
	class HomeViewModel : BaseViewModel
	{
		private readonly INavigation navigation;

		/// <summary>
		/// Command that will navigate the user to a Home Page.
		/// </summary>
		public ICommand NavigateToDetailsCommand { get; set; }

		public HomeViewModel(INavigation navigation)
		{
			this.navigation = navigation;

			NavigateToDetailsCommand = new RelayCommand(() => NavigateToDetailsPage());
		}

		/// <summary>
		/// Move the user to Details page.
		/// </summary>
		private void NavigateToDetailsPage()
		{
			navigation.PushAsync(new DetailsPage());
		}
	}
}
