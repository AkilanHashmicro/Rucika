using Newtonsoft.Json.Linq;
using SalesApp.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using SalesApp.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.DBModel;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        bool crmFlag = false;

        IDictionary<string, int> menuClick = new Dictionary<string, int>();

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();


        //    // var user_details = App._connection.Query<UserModelDB>("SELECT * from UserModelDB");
        //    //  var user_details = (from y in App._connection.Table<UserModelDB>() select y).ToList();


           
        //        Device.StartTimer(new TimeSpan(0, 0, 1), () =>
        //        {


        //          //  var user_details = (from y in App._connection.Table<UserModelDB>() select y).ToList();
        //            if (App.cusList.Count == 0  && Settings.UserId != 0)
        //            {
        //                JObject obj = Controller.InstanceCreation().GetMastersList();

        //                App.product_PriceList = obj["product_pricelist_data"].ToObject<List<Product_PriceList>>();
        //                App.nextActivityList = obj["crm_activity_data"].ToObject<List<next_activity>>();
        //                App.reasondict = obj["crm_lost_reason_data"].ToObject<Dictionary<int, string>>();
        //                App.stageList = obj["crm_stage_data"].ToObject<List<stages>>();
        //                App.paytermList = obj["payment_term_data"].ToObject<List<paytermList>>();
        //                App.taxList = obj["tax_data"].ToObject<List<taxes>>();

        //                App.salesteam = obj["crm_team_data"].ToObject<Dictionary<int, string>>();
        //                App.salespersons = obj["res_user_data"].ToObject<Dictionary<int, string>>();
        //                App.crmleadtags = obj["crm_lead_tag_data"].ToObject<Dictionary<int, string>>();

        //                App.analayticList = obj["analytic_account_data"].ToObject<List<analytic>>();
        //                App.commisiongroupList = obj["target_group_data"].ToObject<List<commisiongroupList>>();
        //                App.all_delivery_method = obj["delivery_carrier_data"].ToObject<List<all_delivery_method>>();
        //                App.locationsList = obj["stock_location_data"].ToObject<List<LocationsList>>();
        //                App.warehousList = obj["stock_warehouse_data"].ToObject<List<warehouse>>();
        //                App.branchList = obj["res_branch_data"].ToObject<List<branch>>();

        //                App.cus_address = obj["customer_address"].ToObject<Dictionary<dynamic, dynamic>>();


        //                App.cusList = Controller.InstanceCreation().GetCustomersList();
        //                App.productList = Controller.InstanceCreation().GetProductssList();
        //                JObject sales_persons = Controller.InstanceCreation().GetSalespersonsList();

        //                App.cusList = Controller.InstanceCreation().GetCustomersList();
        //                App.cusList = Controller.InstanceCreation().GetCustomersList();
        //                App.cusList = Controller.InstanceCreation().GetCustomersList();
        //                App.cusList = Controller.InstanceCreation().GetCustomersList();
        //                App.cusList = Controller.InstanceCreation().GetCustomersList();
        //                App.cusList = Controller.InstanceCreation().GetCustomersList();

        //                MessagingCenter.Send<string, string>("MyApp", "FieldsListUpdated", "true");
        //            }

        //            return true;

        //        });

        //}



        public MasterPage(Page pageRef)
        {
            try
            {
                InitializeComponent();

                ObservableCollection<MasterPageItem> otherGroups = new ObservableCollection<MasterPageItem>();
                MasterPageItem crmPage = new MasterPageItem { Title = "CRM",  Icon = "crmnavy.png", TargetType = typeof(CrmTabbedPage) };
              //  MasterPageItem draftQuotationPage = new MasterPageItem { Title = "Draft Quotation", Icon = "draftimg.png", TargetType = typeof(DraftQuotationsPage) };
                MasterPageItem cusPage = new MasterPageItem { Title = "Customer", Icon = "customersnavy.png", TargetType = typeof(CustomersPage) };
                MasterPageItem calendarPage = new MasterPageItem { Title = "Calendar", Icon = "calendarnavy.png", TargetType = typeof(CalendarPage) };
                MasterPageItem activityPage = new MasterPageItem { Title = "Activities", Icon = "activitynavy.png", TargetType = typeof(ActivitiesPage) };
                MasterPageItem stockPage = new MasterPageItem { Title = "Stock Count", Icon = "stocknavy.png", TargetType = typeof(StockCountPage) };
              //  MasterPageItem targetPage = new MasterPageItem { Title = "Sales Commission Target", Icon = "targetnavy.png", TargetType = typeof(SalesTargetPage) };
                MasterPageItem salestargetPage = new MasterPageItem { Title = "Sales Target", Icon = "salestarget.png", TargetType = typeof(NewSalesTargetPage) };
                MasterPageItem profilePage = new MasterPageItem { Title = "Profile", Icon = "profilemenu.png", TargetType = typeof(ProfilePage) };
                MasterPageItem signoutPage = new MasterPageItem { Title = "Sign Out", Icon = "exit.png", TargetType = typeof(LogoutPage) };
                otherGroups.Add(crmPage);
              //  otherGroups.Add(draftQuotationPage);
                otherGroups.Add(cusPage);
                otherGroups.Add(calendarPage);
                otherGroups.Add(activityPage);
                otherGroups.Add(stockPage);
               // otherGroups.Add(targetPage);
                otherGroups.Add(salestargetPage);
                otherGroups.Add(profilePage);
                otherGroups.Add(signoutPage);
                otherDrawerList.ItemsSource = otherGroups;

            }
            catch (Exception ea)
            {
                System.Diagnostics.Debug.WriteLine(ea.Message);
            }

            menuList = new List<MasterPageItem>();
            string tmpImage = App.partner_image;
            this.masterPageName.Text = App.partner_name;
            this.masterPageRole.Text = App.partner_email;

            if (App.NetAvailable == true)
            {

                if (tmpImage.Equals("False"))
                {
                    userImage.Source = "unknownPerson.png";
                }
                else
                {
                    byte[] imageAsBytes = Encoding.UTF8.GetBytes(tmpImage);
                    byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
                    var stream = new MemoryStream(decodedByteArray);
                    userImage.Source = ImageSource.FromStream(() => stream);
                }

            }

            else if(App.NetAvailable == false)
            {
                foreach (var obj in App.UserListDb)
                {
                    this.masterPageName.Text = obj.user_name;
                    this.masterPageRole.Text = App.partner_email;

                    if (tmpImage.Equals("False"))
                    {
                        userImage.Source = "unknownPerson.png";
                    }
                    else
                    {
                        byte[] imageAsBytes = Encoding.UTF8.GetBytes(obj.user_image_medium);
                        byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
                        var stream = new MemoryStream(decodedByteArray);
                        userImage.Source = ImageSource.FromStream(() => stream);
                    }
                }

            }

            Page page = pageRef;

            //Detail = new NavigationPage((page)) { BarBackgroundColor = Color.FromHex("#414141") };

             Detail = new NavigationPage((page)) { BarBackgroundColor = Color.FromHex("#363E4B") };

          //  { BarBackgroundColor = Color.FromHex("#414141") }

            this.IsPresented = false;

        }

        private async void OnMenuItemTappedAsync(object sender, ItemTappedEventArgs ea)
        {
          
            try
            {

                MasterPageItem masterItemObj = (MasterPageItem)ea.Item;
                string name = masterItemObj.Title;         
                if (name == "Sign Out")
                {
                    var data = await DisplayAlert("Logout Alert", "Are you sure you want to log out?", "OK", "Cancel");
                    if (data)
                    {
                        Settings.UserName = "";
                        Settings.UserPassword = "";
                        Settings.PrefKeyUserDetails = "";
                        App.userid = 0;
                        Settings.UserId = 0;
                        App.cusList.Clear();
                        App.oppo_swipped = true;
                        App.draftquot_swipped = true;
                        App.quot_swipped = true;
                        App.so_swipped = true;
                        App._connection.CreateTable<UserModelDB>();
                        App._connection.Query<UserModelDB>("DELETE from UserModelDB");
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    else
                    {

                    }
                }

                else
                {
                    Type page = masterItemObj.TargetType;
                    act_ind.IsRunning = true;
                    await Task.Run(() =>  Detail = new NavigationPage((Page)Activator.CreateInstance(page)) { BarBackgroundColor = Color.FromHex("#363E4B") });
                    act_ind.IsRunning = false;
                    IsPresented = false;

                }
            }

            catch(Exception exc)
            {
                if (App.NetAvailable == false)
                {
                    await DisplayAlert("Alert", "Need Internet Connection", "Ok");
                   
                }

            }

        }


        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#F0EEEF");
          //  m_title.TextColor = Color.Red;
        }


    }
}
