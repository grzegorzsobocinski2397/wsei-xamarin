using System;

namespace AirMonitor.Models
{
	/// <summary>
	/// Contains measurement for given time period.
	/// </summary>
	public struct MeasurementItem
	{
		public DateTime FromDateTime { get; set; }
		public DateTime TillDateTime { get; set; }
		public MeasurementValue[] Values { get; set; }
		public AirQualityIndex[] Indexes { get; set; }
		public AirQualityStandard[] Standards { get; set; }
		public int FirstOrDefault { get; internal set; }
	}
}
