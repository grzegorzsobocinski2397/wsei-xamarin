using AirMonitor.Models;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirMonitor.Helpers
{
	/// <summary>
	/// Helper for the SQLite database. Used for writing/reading data.
	/// </summary>
	public class DatabaseHelper : IDisposable
	{
		#region Private Fields

		/// <summary>
		/// Representation of SQLite database with basic options like path or name.
		/// </summary>
		private Database Database { get; set; }

		/// <summary>
		/// Name of the database on the device.
		/// </summary>
		private const string DATABASE_NAME = "AirlyDB";

		/// <summary>
		/// Connection to the database.
		/// </summary>
		private SQLiteConnection DBConnection { get; set; }

		#endregion Private Fields

		#region Constructor

		/// <summary>
		/// Establish connection to the database.
		/// </summary>
		public DatabaseHelper()
		{
			Database = new Database(DATABASE_NAME);
			InitializeDatabase();
		}

		#endregion Constructor

		#region Public Methods

		/// <summary>
		/// Dispose of the connection to the database.
		/// </summary>
		public void Dispose()
		{
			DBConnection.Dispose();
			DBConnection = null;
		}

		/// <summary>
		/// Map the existing installations and save them in the database. Erase all table data before.
		/// </summary>
		/// <param name="installations">Current installations of Airly sensors.</param>
		public void SaveInstallations(IEnumerable<Installation> installations)
		{
			List<InstallationEntity> installationEntities = new List<InstallationEntity>();

			foreach (Installation installation in installations)
			{
				InstallationEntity installationEnitity = new InstallationEntity(installation);
				installationEntities.Add(installationEnitity);
			}

			DBConnection.RunInTransaction(() =>
			{
				DBConnection.DeleteAll<InstallationEntity>();
				DBConnection.InsertAll(installationEntities);
			}
			);
		}

		/// <summary>
		/// Map the measurements into entites and write them into the database. Wipe data before.
		/// </summary>
		/// <param name="measurements"></param>
		public void SaveMeasurements(IEnumerable<Measurement> measurements)
		{
			DBConnection.RunInTransaction(() =>
			{
				DBConnection.DeleteAll<MeasurementEntity>();
				DBConnection.DeleteAll<MeasurementItemEntity>();
				DBConnection.DeleteAll<MeasurementValue>();
				DBConnection.DeleteAll<AirQualityIndex>();
				DBConnection.DeleteAll<AirQualityStandard>();

				foreach (Measurement measurement in measurements)
				{
					DBConnection.InsertAll(measurement.Current.Values, false);
					DBConnection.InsertAll(measurement.Current.Indexes, false);
					DBConnection.InsertAll(measurement.Current.Standards, false);

					MeasurementItemEntity measurementCurrentEntity = new MeasurementItemEntity(measurement.Current);
					DBConnection.Insert(measurementCurrentEntity);

					MeasurementEntity measurementEntity = new MeasurementEntity(measurement, measurementCurrentEntity);
					DBConnection.Insert(measurementEntity);
				}
			});
		}

		/// <summary>
		/// Read the database for <see cref="InstallationEntity"/>, deserialize them and return.
		/// </summary>
		public IEnumerable<Installation> ReadInstallations()
		{
			List<InstallationEntity> installationEnitities = DBConnection.Table<InstallationEntity>().ToList();
			List<Installation> installations = new List<Installation>();

			foreach (InstallationEntity installationEntity in installationEnitities)
			{
				installations.Add(new Installation(installationEntity));
			}

			return installations;
		}

		/// <summary>
		/// Read the database for <see cref="MeasurementEntity"/>, deserialize them and return.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Measurement> ReadMeasurements()
		{
			List<MeasurementEntity> measurementEntities = DBConnection.Table<MeasurementEntity>().ToList();
			List<Measurement> measurements = new List<Measurement>();

			foreach (MeasurementEntity measurementEntity in measurementEntities)
			{
				Measurement measurement = DeserializeMeasurement(measurementEntity);
				measurements.Add(measurement);
			}

			return measurements;
		}

		/// <summary>
		/// Whether any measurement isn't expired.
		/// </summary>
		/// <returns></returns>
		public bool IsDataValid()
		{
			double MINUS_ONE_HOUR = -1;
			DateTime expiringDate = DateTime.Now.ToUniversalTime().AddHours(MINUS_ONE_HOUR);

			IEnumerable<Measurement> measurements = ReadMeasurements();

			bool isAnyMeasurementValid = measurements.Any(m => m.Current.TillDateTime > expiringDate);

			return isAnyMeasurementValid;
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Deserialize measurement entity taken from the database.
		/// </summary>
		private Measurement DeserializeMeasurement(MeasurementEntity measurementEntity)
		{
			InstallationEntity installationEntity = DBConnection.Get<InstallationEntity>(measurementEntity.InstallationId);
			Installation installationModel = new Installation(installationEntity);
			MeasurementItemEntity measurementCurrentEntity = DBConnection.Get<MeasurementItemEntity>(measurementEntity.CurrentId);

			JArray valuesArray = JArray.Parse(measurementCurrentEntity.Values);
			JArray indexesArray = JArray.Parse(measurementCurrentEntity.Indexes);
			JArray standardsArray = JArray.Parse(measurementCurrentEntity.Standards);

			string ID_COLUMN = "Id";
			int[] valuesIds = DeserializeJsonToIntArray(valuesArray, ID_COLUMN);
			int[] indexesIds = DeserializeJsonToIntArray(indexesArray, ID_COLUMN);
			int[] standardsIds = DeserializeJsonToIntArray(standardsArray, ID_COLUMN);

			MeasurementValue[] measurementValues = DBConnection.Table<MeasurementValue>().Where(v => valuesIds.Contains(v.Id)).ToArray();
			AirQualityIndex[] measurementIndexes = DBConnection.Table<AirQualityIndex>().Where(i => indexesIds.Contains(i.Id)).ToArray();
			AirQualityStandard[] measurementStandards = DBConnection.Table<AirQualityStandard>().Where(s => standardsIds.Contains(s.Id)).ToArray();

			MeasurementItem measurementCurrent = new MeasurementItem(measurementCurrentEntity, measurementValues, measurementIndexes, measurementStandards);
			Measurement measurement = new Measurement(measurementEntity, measurementCurrent, installationModel);
			return measurement;
		}

		/// <summary>
		/// Deserialize JSON into an array of int. Used for deserializing ids.
		/// </summary>
		private int[] DeserializeJsonToIntArray(JArray jArray, string key)
		{
			int[] JObjectArray = new int[jArray.Count];

			for (int i = 0; i < jArray.Count; i++)
			{
				JObject JObj = JObject.Parse(jArray[i].ToString());
				JObjectArray[i] = (int)JObj[key];
			}

			return JObjectArray;
		}

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