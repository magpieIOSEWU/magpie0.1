using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagpieIOS
{
	class MagpieBadge
	{
		public string bid { get; set; }
		public string bname { get; set; }
		public double lat { get; set; }
		public double lon { get; set; }
		public string desc { get; set; }
		public string badge_artist { get; set; }
		public string badge_year { get; set; }

		public MagpieBadge()
		{
			bid = "";
			bname = "";
			lat = 0.0;
			lon = 0.0;
			desc = "";
			badge_artist = "";
			badge_year = "";
		}


	}
}
