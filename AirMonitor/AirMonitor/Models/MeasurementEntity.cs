namespace AirMonitor.Models
{
	/// <summary>
	/// Model of the <see cref="Measurement"/> for the SQLite database.
	/// </summary>
	public class MeasurementEntity
	{
		#region Public Properties

		public int CurrentId { get; set; }
		public MeasurementItem Current { get; set; }
		public MeasurementItem[] History { get; set; }
		public MeasurementItem[] Forecast { get; set; }
		public int InstallationId { get; set; }

		#endregion Public Properties

		#region Constructor

		public MeasurementEntity()
		{
		}

		#endregion Constructor
	}
}