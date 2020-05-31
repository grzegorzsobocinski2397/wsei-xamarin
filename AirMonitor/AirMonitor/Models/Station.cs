using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace AirMonitor.Models
{
	/// <summary>
	/// Representation of an airly station.
	/// </summary>
	public struct Station
	{
		public string Id { get; set; }
		public Location Location { get; set; }
		public Address Address { get; set; }
		public double Elevation { get; set; }
	}
}
