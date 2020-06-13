namespace AirMonitor.Models
{
	/// <summary>
	/// PM Measurement representation.
	/// </summary>
	public struct PM
	{
		public int Value { get; set; }
		public int Percentage { get; set; }

		public PM(int value, int percentage)
		{
			Value = value;
			Percentage = percentage;
		}
	}
}