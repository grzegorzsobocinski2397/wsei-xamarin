namespace AirMonitor.Models
{
	/// <summary>
	/// Measurement value like PM.
	/// </summary>
	public struct MeasurementValue
	{
		public string Name { get; set; }
		public double Value { get; set; }
	}
}
