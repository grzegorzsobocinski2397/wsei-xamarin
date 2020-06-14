using SQLite;

namespace AirMonitor.Models
{
	/// <summary>
	/// Measurement value like PM.
	/// </summary>
	public class MeasurementValue
	{
		#region Public Properties

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }
		public double Value { get; set; }

		#endregion Public Properties

		#region Constructor

		public MeasurementValue()
		{
		}

		#endregion Constructor
	}
}