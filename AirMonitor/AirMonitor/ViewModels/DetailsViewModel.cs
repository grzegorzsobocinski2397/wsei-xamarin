using AirMonitor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirMonitor.ViewModels
{
	class DetailsViewModel : BaseViewModel
	{
		public PM PM25 { get; set; }
		public PM PM10 { get; set; }
		public int Caqi { get; set; }

		public DetailsViewModel()
		{
			PM25 = new PM(10, 130);
			PM10 = new PM(25, 150);
		}
	}
}
