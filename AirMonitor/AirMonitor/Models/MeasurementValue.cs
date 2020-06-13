using SQLite;

namespace AirMonitor.Models
{
	/// <summary>
	/// Measurement value like PM.
	/// </summary>
	public struct MeasurementValue
	{
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public double Value { get; set; }
	}
}
