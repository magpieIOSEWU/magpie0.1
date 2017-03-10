using System;
using CoreLocation;
using MapKit;

namespace MagpieIOS
{
	public class AnnotationModel : MKAnnotation
	{

		private string _title;
		//private string _subtitle;
		private int id;
		private bool claimed;

		public AnnotationModel(CLLocationCoordinate2D coordinate, string title, int id, bool claimed)
		{
			this.Coords = coordinate;
			_title = title;
			this.id = id;
			this.claimed = claimed;
		}


		public override string Title { get { return _title; } }

		public CLLocationCoordinate2D Coords;
		CLLocationCoordinate2D cLLocationCoordinate2D;
		string name;
		int index;

		public override CLLocationCoordinate2D Coordinate { get { return this.Coords; } }

		public bool isClaimed()
		{
			return claimed;
		}
	}
}
