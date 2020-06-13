using SQLite;
using System;

namespace AirMonitor.Models
{
	/// <summary>
	/// Model of the <see cref="MeasurementItem"/> for the SQLite database.
	/// </summary>
	public class MeasurementItemEntity
	{
		#region Public Properties

		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }

		public DateTime FromDateTime { get; set; }
		public DateTime TillDateTime { get; set; }
		public string Values { get; set; }
		public string Indexes { get; set; }
		public string Standards { get; set; }
		public int FirstOrDefault { get; internal set; }

		#endregion Public Properties

		#region Constructor

		public MeasurementItemEntity()
		{
		}

		#endregion Constructor
	}
}