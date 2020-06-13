using SQLite;

namespace AirMonitor.Models
{
	/// <summary>
	/// Model of the air quality provided by the Airly team.
	/// </summary>
	public struct AirQualityIndex
	{
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public double Value { get; set; }
		public string Level { get; set; }
		public string Description { get; set; }
		public string Advice { get; set; }
		public string Color { get; set; }
	}
}