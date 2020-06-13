using Newtonsoft.Json;

namespace AirMonitor.Models
{
	/// <summary>
	/// Model of the <see cref="Measurement"/> for the SQLite database.
	/// </summary>
	public class MeasurementEntity
	{
		#region Public Properties

		public int CurrentId { get; set; }
		public string History { get; set; }
		public string Forecast { get; set; }
		public string InstallationId { get; set; }

		#endregion Public Properties

		#region Constructor

		public MeasurementEntity(Measurement measurement, MeasurementItemEntity current)
		{
			CurrentId = current.Id;
			History = JsonConvert.SerializeObject(measurement.History);
			Forecast = JsonConvert.SerializeObject(measurement.Forecast);
			InstallationId = measurement.Installation.Id;
		}

		#endregion Constructor
	}
}