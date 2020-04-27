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
using Newtonsoft.Json.Linq;
using SalesApp.DBModel;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private Controller controllerObj = Controller.InstanceCreation();




        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string, string>("MyApp", "Login", async (sender, arg) =>
            {
                await Task.Run(() =>
                {
                    var user_details = (from y in App._connection.Table<UserModelDB>() select y).ToList();
                    if (App.cusList.Count == 0 && user_details.Count == 0 && Settings.UserId != 0)
                    {
                        App.cusList = Controller.InstanceCreation().GetCustomersList();
                        JObject sales_persons = Controller.InstanceCreation().GetSalespersonsList();
                        App.salespersons = sales_persons.ToObject<Dictionary<int, string>>();                       
                        App.taxList = Controller.InstanceCreation().GettaxList();
                        App.warehousList = Controller.InstanceCreation().GetwarehouseList();
                        App.productList = Controller.InstanceCreation().GetProductssList();


                        App.warehousList = Controller.InstanceCreation().GetwarehouseList();
                        App.productList = Controller.InstanceCreation().GetProductssList(); App.warehousList = Controller.InstanceCreation().GetwarehouseList();
                        App.productList = Controller.InstanceCreation().GetProductssList(); App.warehousList = Controller.InstanceCreation().GetwarehouseList();
                        App.productList = Controller.InstanceCreation().GetProductssList(); App.warehousList = Controller.InstanceCreation().GetwarehouseList();
                        App.productList = Controller.InstanceCreation().GetProductssList(); App.warehousList = Controller.InstanceCreation().GetwarehouseList();
                        App.productList = Controller.InstanceCreation().GetProductssList(); App.warehousList = Controller.InstanceCreation().GetwarehouseList();
                        App.productList = Controller.InstanceCreation().GetProductssList(); App.warehousList = Controller.InstanceCreation().GetwarehouseList();
                        App.productList = Controller.InstanceCreation().GetProductssList();




                        MessagingCenter.Send<string, string>("MyApp", "FieldsListUpdated", "true");

                    }
                });
            });
        }


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
 
                        db_layout.IsVisible = false;
                    }

                    else
                    {
                        db_layout.IsVisible = true;
                    }
                                       
                    dbPicker.SelectedIndex = 0;
                    if (dbData.Length >=1)
                    {
                       dbPicker.IsVisible = true;
                    }
                                      
                }
                catch (Exception ex)
                {    
                    db_layout.IsVisible = false;
                }
            }
        }


        public async void SignInActionAsync(object sender, EventArgs ea)
        {
           try
            {
                act_ind.IsRunning = true;  
              
                Settings.UserName = loginEntry.Text;
                Settings.UserPassword = passwordEntry.Text;

                Settings.UserUrlName = "https://laborindo.hashmicro.com";
              //  dbPicker.SelectedItem = "laborindo";
                dbPicker.SelectedItem = "test25";
                Settings.UserDbName = dbPicker.SelectedItem.ToString();

            String res =  await Task.Run(() => controllerObj.login(Settings.UserUrlName, Settings.UserDbName, Settings.UserName, Settings.UserPassword));

                if (res == "false")
                {
                    loginfailedAlert.Text = "Invalid Username or Password.";
                    loginfailedAlert.IsVisible = true;
                    act_ind.IsRunning = false; 
                }

                else
                {
                    MessagingCenter.Send<string, string>("MyApp", "Login", "true");
                    JObject obj = controllerObj.getuserdata("res.users", "get_user_data");
                    App.partner_id = obj["partner_id"].ToObject<int>();
                    App.partner_name = obj["user_name"].ToObject<string>();
                    App.partner_image = obj["image_medium"].ToObject<string>();
                    App.partner_email = obj["user_email"].ToObject<string>();

                    loginfailedAlert.IsVisible = false;
                    App.lead_rpc = true;
                    Page pageRef = new CrmTabbedPage();
                    App.Current.MainPage = new MasterPage(pageRef);
                      act_ind.IsRunning = false;
                   
                }
            }
            catch
            {


                loginfailedAlert.Text = "Invalid Username or Password.";
                loginfailedAlert.IsVisible = true;

                act_ind.IsRunning = false; 
            }


        }


    }
}
