using System;

namespace AirMonitor.Models
{
	/// <summary>
	/// Contains measurement for given time period.
	/// </summary>
	public class MeasurementItem
	{
		#region Public Properties

		public DateTime FromDateTime { get; set; }
		public DateTime TillDateTime { get; set; }
		public MeasurementValue[] Values { get; set; }
		public AirQualityIndex[] Indexes { get; set; }
		public AirQualityStandard[] Standards { get; set; }
		public int FirstOrDefault { get; internal set; }

		#endregion Public Properties

		#region Constructor

		public MeasurementItem()
		{
		}

		public MeasurementItem(MeasurementItemEntity measurementEntity, MeasurementValue[] values, AirQualityIndex[] indexes, AirQualityStandard[] standards)
		{
			FromDateTime = measurementEntity.FromDateTime;
			TillDateTime = measurementEntity.TillDateTime;
			Values = values;
			Indexes = indexes;
			Standards = standards;
		}

		#endregion Constructor
	}
}