using Newtonsoft.Json;

namespace AirMonitor.Models
{
	/// <summary>
	/// Measurement provided by Airly endpoint.
	/// </summary>
	public class Measurement
	{
		#region Public Properties

		public int CurrentDisplayValue { get; set; }
		public MeasurementItem Current { get; set; }
		public MeasurementItem[] History { get; set; }
		public MeasurementItem[] Forecast { get; set; }
		public Installation Installation { get; set; }

		#endregion Public Properties

		#region Constructor

		public Measurement()
		{
		}

		public Measurement(MeasurementEntity measurement, MeasurementItem current, Installation installation)
		{
			Current = current;
			History = JsonConvert.DeserializeObject<MeasurementItem[]>(measurement.History);
			Forecast = JsonConvert.DeserializeObject<MeasurementItem[]>(measurement.Forecast);
			Installation = installation;
		}

		#endregion Constructor
	}
}