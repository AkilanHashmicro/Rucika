using SalesApp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using SalesApp.Pages;
using static SalesApp.models.CRMModel;
using Syncfusion.SfBusyIndicator.XForms;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private Controller controllerObj = Controller.InstanceCreation();

        public LoginPage()
        {
            InitializeComponent();
           // loginEntry.Text = "admin";
           // passwordEntry.Text = "admin";

            NavigationPage.SetHasNavigationBar(this, false);
            sign_color.BackgroundColor = Color.FromHex("#f7dede");
           // signInButton.IsEnabled = false;
            login_frame.BackgroundColor = Color.FromHex("#f7dede");
            pass_frame.BackgroundColor = Color.FromHex("#f7dede");

            var eye_viewImgRecognizer = new TapGestureRecognizer();
            eye_viewImgRecognizer.Tapped += async (s, e) =>
            {
                // handle the tap              
                eye_view_layout.IsVisible = false;
                eye_close_layout.IsVisible = true;
                passwordEntry.IsPassword = false;
            };
            eye_view.GestureRecognizers.Add(eye_viewImgRecognizer);

            var eye_closeImgRecognizer = new TapGestureRecognizer();
            eye_closeImgRecognizer.Tapped += async (s, e) =>
            {
                // handle the tap              
                eye_view_layout.IsVisible = true;
                eye_close_layout.IsVisible = false;

                passwordEntry.IsPassword = true;

            };
            eye_close.GestureRecognizers.Add(eye_closeImgRecognizer);

        }

        private void login_Focused(object sender, EventArgs e)
        {
            //  Navigation.PushPopupAsync(new PickerSelectionPage());

            // searchprod.Unfocus();
            login_frame.BackgroundColor = Color.White;
            loginEntry.BackgroundColor = Color.White;

            if(loginEntry.Text !="" && passwordEntry.Text!="")
            {
                sign_color.BackgroundColor = Color.White;
            }

            else
            {
                sign_color.BackgroundColor = Color.FromHex("#f7dede");
            }

        }
        private void login_Unfoucsed(object sender, EventArgs e)
        {
            //  Navigation.PushPopupAsync(new PickerSelectionPage());

            // searchprod.Unfocus();
            login_frame.BackgroundColor = Color.FromHex("#f7dede");
            loginEntry.BackgroundColor = Color.FromHex("#f7dede");

            if (loginEntry.Text != "" && passwordEntry.Text != "")
            {
                sign_color.BackgroundColor = Color.White;
            }

            else
            {
                sign_color.BackgroundColor = Color.FromHex("#f7dede");
            }

        }


        private void pass_Focused(object sender, EventArgs e)
        {
            //  Navigation.PushPopupAsync(new PickerSelectionPage());

            // searchprod.Unfocus();
           pass_frame.BackgroundColor = Color.White;

            if (loginEntry.Text != "" && passwordEntry.Text != "")
            {
                sign_color.BackgroundColor = Color.White;
            }

            else
            {
                sign_color.BackgroundColor = Color.FromHex("#f7dede");
            }

        }
        private void pass_Unfoucsed(object sender, EventArgs e)
        {
            //  Navigation.PushPopupAsync(new PickerSelectionPage());

            // searchprod.Unfocus();
            pass_frame.BackgroundColor = Color.FromHex("#f7dede");

            if (loginEntry.Text != "" && passwordEntry.Text != "")
            {
                sign_color.BackgroundColor = Color.White;
            }

            else
            {
                sign_color.BackgroundColor = Color.FromHex("#f7dede");
            }

        }

        public void URLTextChanged(object sender, EventArgs e)
        {
            string urlText = ((Entry)sender).Text;
            dbPicker.Items.Clear();
            dbPicker.IsVisible = false;

            if (Util.Net.ValidateURL(urlText) && urlText.Length > 10)
            {

                try
                {
                    String[] dbData = controllerObj.getDatabases(urlText);


                    foreach (String db in dbData)
                    {
                        dbPicker.Items.Add(db);
                    }

                    if(dbData == null )
                    {
                      //  url_tickimg.IsVisible = false;

                        db_layout.IsVisible = false;
                      //  db_layout1.IsVisible = false;
                       // db_layout2.IsVisible = false;
                    }

                    else
                    {
                      //  url_tickimg.IsVisible = true;
                        db_layout.IsVisible = true;
                      //  db_layout1.IsVisible = true;
                       // db_layout2.IsVisible = true;
                    }
                                       
                    dbPicker.SelectedIndex = 0;
                    if (dbData.Length >=1)
                    {
                        //urlAlert.Text = "Invalid URL";
                       dbPicker.IsVisible = true;
                     //   urlAlert.IsVisible = false;
                    }
                                      
                }
                catch (Exception ex)
                {
                    //url_tickimg.IsVisible = false;

                    db_layout.IsVisible = false;
                  //  db_layout1.IsVisible = false;
                    //db_layout2.IsVisible = false;
                }
            }
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        public async void SignInActionAsync(object sender, EventArgs ea)
        {
         
            try
            {
                
                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);

                Settings.UserName = loginEntry.Text;
                Settings.UserPassword = passwordEntry.Text;

            // Settings.UserUrlName = "http://laborindo.equip-sapphire.com";

                Settings.UserUrlName = "https://laborindo.hashmicro.com";
              //  dbPicker.SelectedItem = "laborindo";
                dbPicker.SelectedItem = "test25";

                  //Settings.UserUrlName = "http://beta-dev1.hashmicro.com";
                  //dbPicker.SelectedItem = "MBTurssco";

                Settings.UserDbName = dbPicker.SelectedItem.ToString();


                //controllerObj.login("http://salesapp.hashmicro.com", "salesapp", "admin", "admin");

            String res =    await Task.Run(() => controllerObj.login(Settings.UserUrlName, Settings.UserDbName, Settings.UserName, Settings.UserPassword));

                //    await Task.Run(() => controllerObj.login("http://beta-dev2.hashmicro.com", "PNM", "admin", "admin"));

                if (res == "false")
                {
                    loginfailedAlert.Text = "Invalid Username or Password.";
                    loginfailedAlert.IsVisible = true;

                    Loadingalertcall();
                }

                else
                {
                   // loginfailedAlert.Text = "Invalid Username or Password.";
                    loginfailedAlert.IsVisible = false;

                    Page pageRef = new CrmTabbedPage();
                    App.Current.MainPage = new MasterPage(pageRef);

                    //await Task.Run(() =>
                    //{

                    //    Device.BeginInvokeOnMainThread(() =>
                    //    {

                    //        indi.IsVisible = true;
                    //        indi.IsRunning = true;
                           
                    //        Page pageRef = new CrmTabbedPage();
                    //        App.Current.MainPage = new MasterPage(pageRef);
                    //        indi.IsVisible = false;
                    //        indi.IsRunning = false;
                    //    });
                    //});

                   Loadingalertcall();
                     
                   
                }


            }
            catch
            {

                loginfailedAlert.Text = "Invalid Username or Password.";
                loginfailedAlert.IsVisible = true;

                Loadingalertcall();
            }


        }


    }
}
