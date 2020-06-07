using AirMonitor.Models;
using System;
using System.Linq;

namespace AirMonitor.ViewModels
{
	internal class DetailsViewModel : BaseViewModel
	{
		#region Properties

		public PM PM25 { get; set; }
		public PM PM10 { get; set; }
		public int Caqi { get; set; }
		public string CaqiDescription { get; set; }
		public string CaqiLabel { get; set; }
		public double MoistureLevel { get; set; }
		public int PressureLevel { get; set; }
		public Measurement Measurement { set { AssignMeasurement(value); } }

		#endregion Properties

		#region Public Methods

		/// <summary>
		/// Assign the values from measurement.
		/// </summary>
		public void AssignMeasurement(Measurement measurement)
		{
			MeasurementItem current = measurement.Current;
			AirQualityIndex index = current.Indexes.FirstOrDefault(c => c.Name == "AIRLY_CAQI");
			MeasurementValue[] values = current.Values;
			AirQualityStandard[] standards = current.Standards;

			PM10 = GetPM("PM10", current);
			PM25 = GetPM("PM25", current);

			Caqi = (int)Math.Round(index.Value);
			CaqiLabel = index.Description;
			CaqiDescription = index.Advice;

			MoistureLevel = (int)Math.Round(values.FirstOrDefault(value => value.Name == "HUMIDITY").Value);
			PressureLevel = (int)Math.Round(values.FirstOrDefault(value => value.Name == "PRESSURE").Value);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Get the PM values and percentage.
		/// </summary>
		private PM GetPM(string name, MeasurementItem current)
		{
			int value = (int)Math.Round(current.Values.FirstOrDefault(val => val.Name == name).Value);
			int percentage = (int)Math.Round(current.Standards.FirstOrDefault(standard => standard.Pollutant == name).Percent);
			return new PM(value, percentage);
		}

		#endregion Private Methods
	}
}