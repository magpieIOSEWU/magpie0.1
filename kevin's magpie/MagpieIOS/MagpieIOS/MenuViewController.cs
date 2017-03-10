using Foundation;
using System;
using UIKit;
using ZXing;

namespace MagpieIOS
{
    public partial class MenuViewController : UIViewController
    {
        public MenuViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

      //Style Menu buttons
            this.menuTitle.Font = UIFont.FromName("Montserrat-Medium", 35f);
            this.badgeBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            this.mapBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            this.qrBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            this.settingsBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            this.legalBtn.Font = UIFont.FromName("Montserrat-Medium", 20f);


        }



       
    }
}