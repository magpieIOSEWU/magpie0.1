using CoreGraphics;
using Foundation;
using System;
using System.Text.RegularExpressions;
using UIKit;

namespace MagpieIOS
{
    internal class TableSource : UITableViewSource
    {
        private Tuple<string,string>[] statues; //<title, artist and year>
        string CellIdentifier = "badgeCell";
        badgesUITableViewController owner;

        public TableSource(Tuple<string, string>[] statues, badgesUITableViewController owner)
        {
            this.owner = owner;
            this.statues = statues;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return statues.Length;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
//creates each table cell dynamically

            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = statues[indexPath.Row].Item1;
            string sub = statues[indexPath.Row].Item2;

        //if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier); }

        //parse the statue's title into a file name
            string parsed = Regex.Replace(item, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            //if (){/*user has badge*/
                parsed = "badges/SSW_" + parsed + ".png";
            //}
            //else {//user does not have badge... load grayscale icon
            //    parsed = "graybadges/SSW_" + parsed + ".png";
            //}

        //create badge icon
            var img = UIImage.FromFile(parsed);
            cell.ImageView.Image = img;


            cell.TextLabel.Text = item;
            cell.DetailTextLabel.Text = sub;

        //title text styling *
            cell.TextLabel.Font = UIFont.FromName("Montserrat-Light", 17f);
            cell.DetailTextLabel.Font = UIFont.FromName("Montserrat-Bold", 12f);
            cell.DetailTextLabel.TextColor = UIColor.LightGray;

            cell.ImageView.SizeToFit();

            return cell;

            
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
//Event when a badge table row is selected

      //Creates badge detail image -- need assets
            UIImageView image = new UIImageView(UIImage.FromFile("defaultBadge.png"));


            UIViewController badgeDetailView = new UIViewController();
            badgeDetailView.EdgesForExtendedLayout = UIRectEdge.None;
            badgeDetailView.Add(image);

      //Create badge title bar
            UILabel label = new UILabel(new System.Drawing.RectangleF(0, 230, (float)UIScreen.MainScreen.Bounds.Width, 60));
            label.Text = statues[indexPath.Row].Item1;
            if (label.Text.Length < 30)
            {
                label.Font = UIFont.FromName("Montserrat-Light", 24f);
            }
            else
            {
                label.Font = UIFont.FromName("Montserrat-Light", 18f);
            }
            label.TextColor = UIColor.White;
            label.BackgroundColor = UIColor.LightGray;
            label.TextAlignment = UITextAlignment.Center;
            badgeDetailView.Add(label);
      //end badge title bar

      //Holds the view map button and share button
            UIView buttonBar = new UIView(new System.Drawing.RectangleF(0, 290, (float)UIScreen.MainScreen.Bounds.Width, 90));
            buttonBar.BackgroundColor = UIColor.DarkGray;

      //Build map button
            UIButton viewInMapBtn = new UIButton(new System.Drawing.RectangleF(50, 15, 40, 40));
            viewInMapBtn.SetImage(UIImage.FromFile("mapIcon.png"), UIControlState.Normal);

            UILabel mapBtnLabel = new UILabel(new System.Drawing.RectangleF(20, 60, 100, 20));
            mapBtnLabel.Text = "VIEW IN MAP";
            mapBtnLabel.TextAlignment = UITextAlignment.Center;
            mapBtnLabel.Font = UIFont.FromName("Montserrat-Bold", 12f);
            mapBtnLabel.TextColor = UIColor.White;
            buttonBar.Add(mapBtnLabel);

            buttonBar.Add(viewInMapBtn);
      //end map button

      //Build share button
            UIButton shareBtn = new UIButton(new System.Drawing.RectangleF((float)(UIScreen.MainScreen.Bounds.Width - 90), 15, 40, 40));
            shareBtn.SetImage(UIImage.FromFile("shareIcon.png"), UIControlState.Normal);

            UILabel shareBtnLabel = new UILabel(new System.Drawing.RectangleF((float)(UIScreen.MainScreen.Bounds.Width - 100), 60, 60, 20));
            shareBtnLabel.Text = "SHARE";
            shareBtnLabel.TextAlignment = UITextAlignment.Center;
            shareBtnLabel.Font = UIFont.FromName("Montserrat-Bold", 12f);
            shareBtnLabel.TextColor = UIColor.White;
            buttonBar.Add(shareBtnLabel);

            buttonBar.Add(shareBtn);
     //end share button

     //Create badge icon
            string parsed = Regex.Replace(statues[indexPath.Row].Item1, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            //if (){/*user has badge*/
                parsed = "badges/SSW_" + parsed + ".png";
            //}
            //else {//user does not have badge... load grayscale icon
            //    parsed = "graybadges/SSW_" + parsed + ".png";
            //}

            UIImageView icon = new UIImageView(UIImage.FromFile(parsed));
            icon.Frame = new CoreGraphics.CGRect((float)(UIScreen.MainScreen.Bounds.Width / 2) - 75, 270, 150, 128.89221556886);

            badgeDetailView.Add(buttonBar);

      //Controls the scroll view of the description and artist
            UIScrollView textScroll = new UIScrollView(new System.Drawing.RectangleF(0, 380, (float)UIScreen.MainScreen.Bounds.Width, (float)UIScreen.MainScreen.Bounds.Height - 460));
            textScroll.ScrollEnabled = true;
            textScroll.BackgroundColor = UIColor.White;

      //Build artist and year description
            UILabel subLabel = new UILabel(new System.Drawing.RectangleF(5, 0, (float)textScroll.Bounds.Width, 40));
            subLabel.Text = "ARTIST: " + statues[indexPath.Row].Item2.ToUpper();
            subLabel.TextColor = UIColor.LightGray;
            subLabel.Font = UIFont.FromName("Montserrat-Bold", 16f);

      //Builds the badge description
            UITextView description = new UITextView(new System.Drawing.RectangleF(20, 20, (float)(textScroll.Bounds.Width - 40), (float)textScroll.Bounds.Height));
            description.Add(subLabel);
            description.Text = "\n\nSpicy jalapeno bacon ipsum dolor amet frankfurter chicken tail pig shankle. Meatloaf swine cupim pork loin."
                            + "Cupim prosciutto andouille ham hock cow corned beef, shank pastrami turkey beef ribs biltong frankfurter rump."
                            + "Drumstick pig porchetta shank, pork meatball meatloaf pork belly pastrami beef ribs pork chop chuck. Salami cow "
                            + "strip steak short ribs swine bacon ham venison chuck, andouille drumstick. Burgdoggen sausage turkey bacon, pancetta "
                            + "strip steak meatball jerky. Tail shoulder hamburger salami biltong cupim frankfurter swine pork belly.\n"
                            + "Swine porchetta t - bone, chuck beef corned beef tenderloin ribeye rump alcatra. Jowl leberkas tail capicola. "
                            + "Ribeye pork loin andouille frankfurter sausage pork rump short ribs. Jerky prosciutto tri - tip, strip steak leberkas "
                            + "sausage ham salami flank tenderloin ground round frankfurter meatloaf bresaola tongue."
                            + "Jerky landjaeger pork chop, swine frankfurter ribeye fatback filet mignon capicola doner chicken. Landjaeger "
                            + "ground round capicola turkey strip steak prosciutto t - bone picanha cow spare ribs pork bacon turducken kevin.\n "
                            + "Shankle short ribs salami, ham pig turkey fatback jowl spare ribs porchetta. Jowl shoulder shankle doner landjaeger,"
                            + "ball tip salami fatback. Ham sausage turducken shank, shoulder frankfurter ground round cupim chicken.Prosciutto "
                            + "kielbasa tongue flank porchetta, picanha leberkas chicken.Boudin salami sirloin filet mignon ball tip shankle ham ribeye "
                            + "jowl beef. Beef ribs bresaola filet mignon kielbasa leberkas tongue sausage pork loin. Jerky tenderloin pig prosciutto "
                            + "drumstick swine. Biltong tri-tip strip steak ham, pancetta sausage short loin landjaeger prosciutto. Biltong sausage shank "
                            + "fatback ham hock. Porchetta shoulder bresaola fatback biltong. Jowl jerky tongue short loin meatloaf.\n";

            description.TextColor = UIColor.Black;
            description.Font = UIFont.FromName("Montserrat-Regular", 14f);

            textScroll.AddSubview(description);

            badgeDetailView.Add(textScroll);
     //End badge description build

            badgeDetailView.Add(icon);

            //show badge detail view controller
            owner.NavigationController.PushViewController(badgeDetailView, true);

            /*
             * creates popup for debugging
             * 
            UIAlertController okAlertController = UIAlertController.Create("Row Selected", statues[indexPath.Row].Item1 + "\n" + statues[indexPath.Row].Item2, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            owner.PresentViewController(okAlertController, true, null);
            */

            tableView.DeselectRow(indexPath, true);
        }
    }
}
