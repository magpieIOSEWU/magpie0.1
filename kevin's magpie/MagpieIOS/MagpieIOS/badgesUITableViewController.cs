using Foundation;
using System;
using UIKit;

namespace MagpieIOS
{
    public partial class badgesUITableViewController : UITableViewController
    {
        public badgesUITableViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			/* code for the database information
			 * 
			 * connedt to local sqlite database
			 * var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var dbPath = Path.Combine(documents, "db_sqlite-net.db");
				var db = new SQLiteConnection(dbPath);

				//example of using specific query
				var startInfo = db.Query<MagpieBadge>("SELECT * FROM MagpieBadge WHERE bid = 1");
			SculptureTitle.Text = startInfo[0].bname;

				//example of getting whole table, use for each to get to items
				var table = db.Table<MagpieBadge>();
				foreach (MagpieBadge badge in table)
			{
				double lat = table.ElementAt(index).lat;
				double lon = table.ElementAt(index).lon;
				String name = table.ElementAt(index).bname;

			 * 
			 */

       //Tuple<string, string> s0 = new Tuple<title, artist and year>;
            Tuple<string, string> s1 = new Tuple<string, string>("Alive, Lively, Living", "Jim Hodges. 2008");
            Tuple<string, string> s2 = new Tuple<string, string>("Light Reading", "Peter Reiquam. 2006");
            Tuple<string, string> s3 = new Tuple<string, string>("Cooperation", "Michiro Kosuge. 1998");
            Tuple<string, string> s4 = new Tuple<string, string>("Riverpoint Observatory", "Montana artist, Patrick Zentz. 2002");
            Tuple<string, string> s5 = new Tuple<string, string>("Shamil", "Anatoli Abgudaev. 1991");
            Tuple<string, string> s6 = new Tuple<string, string>("East-West Arbor", "Jody Pinto. 1990");
            Tuple<string, string> s7 = new Tuple<string, string>("From this Earth", "Glenn Emmons. 1994");
            Tuple<string, string> s8 = new Tuple<string, string>("Michael P.Anderson Memorial", "Dorothy Fowler. 2006");
            Tuple<string, string> s9 = new Tuple<string, string>("The Call and the Challenge", "Ken Spiering. 1986");
            Tuple<string, string> s10 = new Tuple<string, string>("Untitled (lantern)", "Harold Balazs. 1974");
            Tuple<string, string> s11 = new Tuple<string, string>("Moon Crater", "Glen Michaels. Expo ‘74");
            Tuple<string, string> s12 = new Tuple<string, string>("Aluminum Fountain", "George Tsutakawa. 1974");
            Tuple<string, string> s13 = new Tuple<string, string>("Centennial Sculpture", "Harold Balazs. 1978");
            Tuple<string, string> s14 = new Tuple<string, string>("The Childhood Express", "Ken Spiering. 1990");
            Tuple<string, string> s15 = new Tuple<string, string>("Goat", "Sister Paula Turnbull");
            Tuple<string, string> s16 = new Tuple<string, string>("Vietnam Veteran's Memorial", "Deborah Copenhaver. 1984");
            Tuple<string, string> s17 = new Tuple<string, string>("Australian Sundial", "Sister Paula Turnbull");
            Tuple<string, string> s18 = new Tuple<string, string>("Mountain Sheep", "Ken Spiering and students");
            Tuple<string, string> s19 = new Tuple<string, string>("Rotary Riverfront Fountain", "Harold Balazs & Bob Perron. 2005");
            Tuple<string, string> s20 = new Tuple<string, string>("The Joy of Running Together", "David Govedare. 1984");
            Tuple<string, string> s21 = new Tuple<string, string>("The Place Where Ghosts of Salmon Jump", "Sherman Alexie");
            Tuple<string, string> s22 = new Tuple<string, string>("", "");

       //tuple array of each statue's <title, artist and year>
            Tuple<string, string>[] statues = new Tuple<string, string>[22] { /*s0,*/ s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15, s16, s17, s18, s19, s20, s21, s22 };


            this.badgeTableView = new UITableView(new System.Drawing.RectangleF(0,0, (float)UIScreen.MainScreen.Bounds.Width, (float)(UIScreen.MainScreen.Bounds.Height)));
            this.badgeTableView.RowHeight = 100f;
            badgeTableView.Source = new TableSource(statues, this);
            Add(badgeTableView);

        }
    }
}