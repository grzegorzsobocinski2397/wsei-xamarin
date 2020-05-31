namespace AirMonitor.Models
{
	/// <summary>
	/// Air quality standard model based on the Airly response.
	/// </summary>
	public struct AirQualityStandard
	{
		public string Name { get; set; }
		public string Pollutant { get; set; }
		public double Limit { get; set; }
		public double Percent { get; set; }
		public string Averaging { get; set; }
	}
}