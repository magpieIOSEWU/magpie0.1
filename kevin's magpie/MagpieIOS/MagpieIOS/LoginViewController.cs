﻿using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace MagpieIOS
{
    public partial class LoginViewController : UIViewController
    {
		private string request = "";
		private string sLine = "";
		private WebRequest wrequest = null;
		private Stream objStream = null;
		private StreamReader objReader = null;

		//work around for error on getting being called twice;
		private int alertCount = 2;

        public LoginViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


       //Style login text fields and button
            //this.logoImg;
            this.titleLogin.Font = UIFont.FromName("Montserrat-Medium", 40f);
            this.loginBtn.Font = UIFont.FromName("Montserrat-Medium", 26f);
            this.emailTextField.Font = UIFont.FromName("Montserrat-Medium", 24f);
            this.passwordTextField.Font = UIFont.FromName("Montserrat-Medium", 24f);

			var btnImage = UIImage.FromBundle("magpieBtn1.png");
			loginBtn.SetBackgroundImage(btnImage, UIControlState.Normal);

			loginBtn.TouchUpInside += (sender, e) => { 
				//enter login check here
				ShouldPerformSegue("ShowMenu", this);
			};

			AddAccountbtn.TouchUpInside += (sender, e) => { 
				string username = emailTextField.Text;
				request = "http://www.zjohns.com/magpie/login.php?User_ID=" + username;
				wrequest = WebRequest.Create(request);
				objStream = wrequest.GetResponse().GetResponseStream();
				objReader = new StreamReader(objStream);
				sLine = objReader.ReadLine();

				if (sLine != "[]")
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Try Again",
						Message = "This User Already Exists. Choose a different name"
					};
					alert.AddButton("OK");
				}
				else
				{
					request = "http://www.zjohns.com/magpie/create.php?User_ID=" + username;
					wrequest = WebRequest.Create(request);
					objStream = wrequest.GetResponse().GetResponseStream();
					UIAlertView alert = new UIAlertView()
					{
						Title = "Success!",
						Message = "Enjoy the application"
					};
					alert.AddButton("OK");
					PerformSegue("ShowMenu", this);
				}
			};



        }

		public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
		{
			string username = emailTextField.Text;
			request = "http://www.zjohns.com/magpie/login.php?User_ID=" + username;
			wrequest = WebRequest.Create(request);
			objStream = wrequest.GetResponse().GetResponseStream();
			objReader = new StreamReader(objStream);
			sLine = objReader.ReadLine();

			if (sLine == "[]")
			{
				UIAlertView alert = new UIAlertView()
				{
					Title = "Invalid Name or Password",
					Message = "There is no user with this user name or password"
				};
				alert.AddButton("OK");
				if (alertCount % 2 == 0)
				{
					alert.Show();
				}
				alertCount++;
				return false;
			}
			else{
				return true;
			}
		}
    }
}