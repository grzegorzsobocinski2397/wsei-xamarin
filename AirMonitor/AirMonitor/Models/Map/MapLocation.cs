using Xamarin.Forms.Maps;

namespace AirMonitor.Models.Map
{
	/// <summary>
	/// Location of the pin on the map.
	/// </summary>
	public class MapLocation
	{
		public string InstallationId { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }
		public Position Position { get; set; }
	}
}
