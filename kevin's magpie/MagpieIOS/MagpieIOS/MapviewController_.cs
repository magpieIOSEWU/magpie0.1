using System;
using UIKit;
using MapKit;
using CoreLocation;
using SQLite;
using System.IO;
using System.Collections.Generic;
using ObjCRuntime;
using System.Linq;
using System.Drawing;


namespace MagpieIOS
{
    public partial class MapviewController : UIViewController
    {

		private UIButton _annotationDetailButton;

		protected string annotationIdentifier = "AnnotationIdentifier";
		IMKAnnotation[] annoList = new IMKAnnotation[22];
		List<CLLocationCoordinate2D> annotationScrollList;
		int currentAnno;
		CLLocationCoordinate2D currAnnoLocation;
		bool locationUPDATED = false;

        public MapviewController (IntPtr handle) : base (handle)
        {
			annotationScrollList = new List<CLLocationCoordinate2D>();
			currentAnno = 0;
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);
			var table = db.Table<MagpieBadge>();

			foreach (MagpieBadge badge in table)
			{
				CLLocationCoordinate2D c = new CLLocationCoordinate2D(badge.lat, badge.lon);
				annotationScrollList.Add(c);
			}

			annotationScrollList.Sort((x, y) => x.Longitude.CompareTo(y.Longitude));
			addAnnotations(table);
			initializeMap();

			mapToggle.ValueChanged += (s, e) =>
			{
				switch (mapToggle.SelectedSegment)
				{
					case 0:
						map.MapType = MKMapType.Standard;
						break;
					case 1:
						map.MapType = MKMapType.Satellite;
						break;
					case 2:
						map.MapType = MKMapType.Hybrid;
						break;
				}
			};

			SculptureInfo.Layer.BorderColor = UIColor.Gray.CGColor;
			SculptureInfo.Layer.BorderWidth = (System.nfloat).5;
			CollectionDistanceTime.Layer.BorderColor = UIColor.Gray.CGColor;
			CollectionDistanceTime.Layer.BorderWidth = (System.nfloat).5;
			CollectionDistance.Layer.BorderColor = UIColor.Gray.CGColor;
			CollectionDistance.Layer.BorderWidth = (System.nfloat).5;

		}


		private void initializeMap()
		{
			CLLocationManager locationManager = new CLLocationManager();
			locationManager.RequestWhenInUseAuthorization();
			locationManager.RequestAlwaysAuthorization(); //requests permission for access to location data while running in the background

			map.ShowsUserLocation = true;

			map.DidUpdateUserLocation += (sender, e) =>
			{
				if (map.UserLocation != null && !locationUPDATED)
				{
					//this would constantly move map back to user location
					CLLocationCoordinate2D coords = map.UserLocation.Coordinate;
					MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(coords, 100, 100);
					map.Region = mapRegion;
					locationUPDATED = true;
				}

			};

			//if user location not accesible start at this location
			if (!map.UserLocationVisible)
			{
				CLLocationCoordinate2D mapCenter = new CLLocationCoordinate2D(47.659554, -117.426290);
				MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(mapCenter, 100, 100);
				map.CenterCoordinate = mapCenter;
				map.Region = mapRegion;
			}

			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);

			//set default pin information on display
			var startInfo = db.Query<MagpieBadge>("SELECT * FROM MagpieBadge WHERE bid = 1");
			SculptureTitle.Text = startInfo[0].bname;

			map.GetViewForAnnotation = GetViewForAnnotation;

			map.DidSelectAnnotationView += (object sender, MKAnnotationViewEventArgs e) =>
			{
				annoList = map.SelectedAnnotations;
				foreach (IMKAnnotation a in annoList)
				{
					SculptureTitle.Text = a.GetTitle();
					getDistanceTime(a.Coordinate, map.UserLocation.Coordinate);
					currAnnoLocation = new CLLocationCoordinate2D(a.Coordinate.Latitude, a.Coordinate.Longitude);
				}
			};

			LeftAnnotation.TouchUpInside += (sender, e) =>
			{
				CLLocationCoordinate2D anno = annotationScrollList[currentAnno];

				map.SetCenterCoordinate(anno, true);

				getDistanceTime(anno, map.UserLocation.Coordinate);

				var table = db.Query<MagpieBadge>("SELECT bname FROM MagpieBadge WHERE lon = " + anno.Longitude);
				SculptureTitle.Text = table[0].bname;

				if (currentAnno == 0)
				{
					currentAnno = annotationScrollList.Count - 1;
				}
				else
				{
					currentAnno--;
				}
			};

			RightAnnotation.TouchUpInside += (sender, e) =>
			{
				CLLocationCoordinate2D anno = annotationScrollList[currentAnno];
				;
				map.SetCenterCoordinate(anno, true);

				getDistanceTime(anno, map.UserLocation.Coordinate);

				var table = db.Query<MagpieBadge>("SELECT bname FROM MagpieBadge WHERE lon = " + anno.Longitude);
				SculptureTitle.Text = table[0].bname;

				if (currentAnno == annotationScrollList.Count - 1)
				{
					currentAnno = 0;
				}
				else
				{
					currentAnno++;
				}
			};


			_btnCurrentLocation.TouchUpInside += (sender, e) =>
			{
				map.SetCenterCoordinate(map.UserLocation.Location.Coordinate, true);
			};
			View.AddSubview(_btnCurrentLocation);
		}


		private void addAnnotations(SQLite.TableQuery<MagpieIOS.MagpieBadge> table)
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine(documents, "db_sqlite-net.db");
			var db = new SQLiteConnection(dbPath);


			int index = 0;

			foreach (MagpieBadge badge in table)
			{
				double lat = table.ElementAt(index).lat;
				double lon = table.ElementAt(index).lon;
				String name = table.ElementAt(index).bname;

				//if iscollected check
				String q = "SELECT * FROM MagpieUser WHERE bid = " + table.ElementAt(index).bid;
				var userTuple = db.Query<MagpieUser>(q);

				if (userTuple.ElementAt(0).isClaimed.Equals(""))
				{
					var annotation = new AnnotationModel(new CLLocationCoordinate2D(lat, lon), name, index, false);
					map.AddAnnotation(annotation);
				}
				else
				{
					var annotation = new AnnotationModel(new CLLocationCoordinate2D(lat, lon), name, index, true);
					map.AddAnnotation(annotation);
				}

				index++;

			}

		}


		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(annotationIdentifier);
			// Set current location and location of annotation
			CLLocationCoordinate2D currentLocation = mapView.UserLocation.Coordinate;
			CLLocationCoordinate2D annotationLocation = annotation.Coordinate;

			// We don't want a special annotation for the user location
			if (currentLocation.Latitude == annotationLocation.Latitude && currentLocation.Longitude == annotationLocation.Longitude)
				return null;

			if (annotationView == null)
			{
				annotationView = new MKPinAnnotationView(annotation, annotationIdentifier);
			}
			else
				annotationView.Annotation = annotation;


			annotationView.CanShowCallout = true;
			(annotationView as MKPinAnnotationView).AnimatesDrop = false; // Set to true if you want to animate the pin dropping

			if ((annotation as AnnotationModel).isClaimed())
			{
				(annotationView as MKPinAnnotationView).PinColor = MKPinAnnotationColor.Green;
			}
			else
			{
				(annotationView as MKPinAnnotationView).PinColor = MKPinAnnotationColor.Red;
			}

			annotationView.SetSelected(true, false);

			annotationView.Annotation.GetTitle();
			_annotationDetailButton = UIButton.FromType(UIButtonType.DetailDisclosure);
			_annotationDetailButton.TouchUpInside += (sender, e) =>
			{
				this.PerformSegue("ShowView", this);
			};

			annotationView.Image = UIImage.FromFile("images/Icon-Small.png");

			annotationView.RightCalloutAccessoryView = _annotationDetailButton;

			// Annotation icon may be specified like this, in case you want it.
			annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromBundle("Icon-Small.png"));
			return annotationView;
		}

		public void getDistanceTime(CLLocationCoordinate2D destination, CLLocationCoordinate2D current)
		{
			CLLocation dest = new CLLocation(destination.Latitude, destination.Longitude);
			CLLocation cur = new CLLocation(current.Latitude, current.Longitude);

			//get and display distance to location
			double meters = cur.DistanceFrom(dest);
			double feet = meters * 3.28084;
			double miles = feet / 5280;
			String distance = String.Format("{0:0.00}", miles);
			milesTo.Text = distance + " mi";

			//estimate and display time to location, assuming 1.4 m/s average pace
			double seconds = meters * 1.4;
			double minutes = seconds / 60;
			string time = String.Format("{0:0.00}", minutes);
			DistanceAway.Text = time + " min";
		}


		/* I dont know what view to point this to?
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			var transferdata = segue.DestinationViewController as BadgeController;
			if (transferdata != null)
			{
				transferdata.loc = currAnnoLocation;
			}
		}
		*/
	}
}