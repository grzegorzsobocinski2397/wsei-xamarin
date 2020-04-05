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
		public double MoistureLevel { get; set; }
		public int PressureLevel { get; set; }

		public DetailsViewModel()
		{
			PM25 = new PM(10, 130);
			PM10 = new PM(25, 150);
			MoistureLevel = 0.58;
			PressureLevel = 1026;
			Caqi = 10;
		}
	}
}
