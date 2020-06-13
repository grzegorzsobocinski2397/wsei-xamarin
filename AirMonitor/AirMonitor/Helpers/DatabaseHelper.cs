using AirMonitor.Models;
using SQLite;

namespace AirMonitor.Helpers
{
	/// <summary>
	/// Helper for the SQLite database. Used for writing/reading data.
	/// </summary>
	public class DatabaseHelper
	{
		#region Private Fields

		private Database Database { get; set; }

		#endregion Private Fields

		private SQLiteConnection DBConnection { get; set; }

		#region Constructor

		public DatabaseHelper()
		{
			Database = new Database("AirlyDB");
			InitializeDatabase();
		}

		#endregion Constructor

		#region Private Methods
		/// <summary>
		/// Connect to database and create tables.
		/// </summary>
		private void InitializeDatabase()
		{
			DBConnection = new SQLiteConnection(Database.Path, Database.Flags);

			DBConnection.CreateTable<InstallationEntity>();
			DBConnection.CreateTable<MeasurementEntity>();
			DBConnection.CreateTable<MeasurementItemEntity>();
			DBConnection.CreateTable<MeasurementValue>();
			DBConnection.CreateTable<AirQualityIndex>();
			DBConnection.CreateTable<AirQualityStandard>();
		}

		#endregion Private Methods
	}
}