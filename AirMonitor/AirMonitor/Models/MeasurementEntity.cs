using System;
using System.Collections.Generic;
using System.Text;

namespace AirMonitor.Models
{
	/// <summary>
	/// Model of the <see cref="Measurement"/> for the SQLite database.
	/// </summary>
	public struct MeasurementEntity
	{
		public int CurrentId { get; set; }
		public MeasurementItem Current { get; set; }
		public MeasurementItem[] History { get; set; }
		public MeasurementItem[] Forecast { get; set; }
		public int InstallationId { get; set; }
	}
}
