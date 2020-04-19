using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Rg.Plugins.Popup.Services;
using SalesApp.models;
using SalesApp.Pages;
using SalesApp.views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

using static SalesApp.models.CRMModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Geolocator;
using SQLite;
using System.Collections.ObjectModel;
using SalesApp.Persistance;
using SalesApp.DBModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesApp.OdooRpc.Model;
//using Android.Locations;


namespace SalesApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public static SQLiteConnection _connection;
        public static List<ProductsList> ProductListDb = new List<ProductsList>();
        public static List<SalesQuotationDB> SalesQuotationListDb = new List<SalesQuotationDB>();
        public static List<SalesOrderDB> SalesOrderListDb = new List<SalesOrderDB>();
        public static List<CRMOpportunitiesDB> CRMOpportunitiesListDb = new List<CRMOpportunitiesDB>();

        public static List<taxes> taxListdb = new List<taxes>();

        public static List<UserModelDB> UserListDb = new List<UserModelDB>();


        public static List<CRMLeadDB> crmListDb = new List<CRMLeadDB>();
        public static Dictionary<int, string> cusdictDb = new Dictionary<int, string>();

        public static List<Customers> cusListDB = new List<Customers>();
       
        public static int userid_db = 0;
       // public static List<SalesOrder> SalesOrderDb = new List<SalesOrder>();

       // public static ObservableCollection<DbTable> _samp;

        private static Stopwatch stopWatch = new Stopwatch();
        private const int defaultTimespan = 1;
        //  public static var calattnList = "";

        public static List<next_activity> nextActivityList = new List<next_activity>();
        public static List<ProductsList> productList = new List<ProductsList>();
        public static List<LocationsList> locationsList = new List<LocationsList>();
       
        public static List<branch> branchList = new List<branch>();
        public static List<warehouse> warehousList = new List<warehouse>();
        public static List<analytic> analayticList = new List<analytic>();

        public static List<taxes> taxList = new List<taxes>();

        public static List<ContactsCreationList> concreationList = new List<ContactsCreationList>();

        public static List<paytermList> paytermList = new List<paytermList>();

        public static List<commisiongroupList> commisiongroupList = new List<commisiongroupList>();

        public static List<taxes> taxListRemove = new List<taxes>();
        public static Dictionary<int, string> cusdict = new Dictionary<int, string>();
        public static Dictionary<int, string> reasondict = new Dictionary<int, string>();
        public static Dictionary<int, string> tagsDict = new Dictionary<int, string>();
        public static List<Customers> cusList = new List<Customers>();
        public static List<stages> stageList = new List<stages>();

        public static List<CRMLead> crmList = new List<CRMLead>();
        public static List<CRMOpportunities> crmOpprList = new List<CRMOpportunities>();
        public static List<SalesQuotation> salesQuotList = new List<SalesQuotation>();
        public static List<SalesQuotation> draftQuotList = new List<SalesQuotation>();
        public static List<SalesOrder> salesOrderList = new List<SalesOrder>();

        public static List<OrderLine> orderLineList = new List<OrderLine>();

        public static List<Product_PriceList> product_PriceList = new List<Product_PriceList>();
        public static List<all_delivery_method> all_delivery_method = new List<all_delivery_method>();


        public static int userid = 0;
        public static int partner_id = 0;
        public static string partner_name = "";
        public static string partner_image = "";
        public static string partner_email = "";

        public static string cpstring = "";
        public static string calselecteddate = "";
        public static string filterstring = "Month";
        public static Dictionary<string, dynamic> filterdict = new Dictionary<string, dynamic>();

        public static Dictionary<int, string> salesteam = new Dictionary<int, string>();
        public static Dictionary<int, string> salespersons = new Dictionary<int, string>();
        public static Dictionary<int, string> crmleadtags = new Dictionary<int, string>();
        public static Dictionary<int, string> countrydict = new Dictionary<int, string>();
        public static Dictionary<int, string> statedict = new Dictionary<int, string>();

        public static Dictionary<dynamic, dynamic> cus_address = new Dictionary<dynamic, dynamic>();

      

        public static  TimeSpan ts = stopWatch.Elapsed;

        public static Boolean user_gps_enabled = false;
        public static int user_gps_time = 0;
        public static String user_gps_period = "";

        public static bool NetAvailable = false;
        public static bool responseState = false;

        public static string StockFilterProduct = "";
        public static string StockFilterWarehouse = "";

        public static  bool tap_plusbtncheck = true;

        public static string user_location_string = "";

        public static bool load_rpc = true;

        public static int createContact_Id = 0;

        public App()
        {
            InitializeComponent();

          //  App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
           
          //  App._connection.CreateTable<SalesOrderDB>();
          //  try
          //  {

          //         var details = (from y in App._connection.Table<SalesOrderDB>() select y).ToList();                   App.SalesOrderListDb = details;

          //  }

          //  catch (Exception e)
          //  {
          //      int j = 0;
          //  }


          //  App._connection.CreateTable<CRMOpportunitiesDB>();
          //  try
          //  {
          //      var details = (from y in App._connection.Table<CRMOpportunitiesDB>() select y).ToList();
          //      App.CRMOpportunitiesListDb = details;
          //  }
          //  catch(Exception ex)
          //  {
          //      int i = 0;
          //  }


          //  App._connection.CreateTable<CRMLeadDB>();
          //  try
          //  {
          //      var details = (from y in App._connection.Table<CRMLeadDB>() select y).ToList();
          //      App.crmListDb = details;
          //  }
          //  catch(Exception ex)
          //  {
          //      int i = 0;
          //  }


          //  App._connection.CreateTable<SalesQuotationDB>();
          //  try
          //  {
          //      var details = (from y in App._connection.Table<SalesQuotationDB>() select y).ToList();
          //      App.SalesQuotationListDb = details;
          //  }
          //  catch(Exception ex)
          //  {
          //      int i = 0;
          //  }


          //  App._connection.CreateTable<UserModelDB>();
          //  try
          //  {
          //      var details = (from y in App._connection.Table<UserModelDB>() select y).ToList();
          //      App.UserListDb = details;
          //  }
          //  catch (Exception ex)
          //  {
          //      int i = 0;
          //  }

          
          //  List<ProductsList> productslistdb = new List<ProductsList>();
          //// Dictionary<int, string> cusdictDbapp = new Dictionary<int, string>();
            //foreach (var item in App.UserListDb)
            //{
            //    productslistdb = JsonConvert.DeserializeObject<List<ProductsList>>(item.products);
            //    App.cusdictDb = JsonConvert.DeserializeObject<Dictionary<int, string>>(item.customers_list);
            //    App.ProductListDb = productslistdb;
            //    App.userid_db = item.userid;
            //  //  App.cusdictDb = cusdictDbapp;
            //}
     



          //  var details1 = (from x in App._connection.Table<SalesQuotation>() select x).ToList();  

            if (Settings.UserName.Length > 0 && Settings.UserPassword.Length > 0)
            {
              // App.Current.MainPage = new MasterPage(new LogoutPage());

                String res = "";

                    try
                    {
                       res = Controller.InstanceCreation().login(Settings.UserUrlName, Settings.UserDbName, Settings.UserName, Settings.UserPassword);
                       App.Current.MainPage = new MasterPage(new CrmTabbedPage());

                     //   PopupNavigation.PushAsync( new CalendarPopupPage());
                    }

                   catch(Exception ea)
                    {
                        if (ea.Message.Contains("(Network is unreachable)") || ea.Message.Contains("NameResolutionFailure"))
                        {
                            App.NetAvailable = false;
                        }

                        else if (ea.Message.Contains("(503) Service Unavailable"))
                        {
                            App.responseState = false;
                        }
                    }

                    if (App.NetAvailable == false)
                    {
                    
                        App.Current.MainPage = new MasterPage(new CrmTabbedPage());
                    }
                

              

                //   Loadingalertcall();
            }

            else
            {
                //  MainPage = new MainPage();
                MainPage = new SalesApp.views.LoginPage();
                //  MainPage = new SalesApp.views.GoogleMapScreen();
            }


          
        }

  
//        async Task MainRefreshData()
//        {
//            if (!stopWatch.IsRunning)
//            {
//                stopWatch.Start();
//            }


//            try
//            {
                
//                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
//             {

//             if (App.NetAvailable == false)
//                 {
//                     List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();
//                 }

//                 var sq_details = App._connection.Query<SalesQuotationDB>("SELECT * from SalesQuotationDB where yellowimg_string = ?", "yellowcircle.png");

//                 if (sq_details.Count() != 0 && App.NetAvailable == true)
//                 {
//                     foreach (var dbobj in sq_details)
//                     {
//                         Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

//                         vals["order_date"] = dbobj.order_date;
//                         vals["expiration_date"] = dbobj.expiration_date;

//                            vals["payment_terms"] = dbobj.payment_term_id;
//                            vals["commission_group"] = dbobj.commission_group_id;


//                         if (dbobj.payment_term_id == 0)
//                         {
//                             vals["payment_terms"] = false;
//                         }

//                            if (dbobj.commission_group_id == 0)
//                         {
//                                vals["commission_group"] = false;
//                         }


//                         vals["user_id"] = dbobj.user_id;

//                     // var cusid = App.cusdict.FirstOrDefault(x => x.Value == cuspicker1.SelectedItem.ToString()).Key;
//                     vals["customer"] = dbobj.customer_id;

//                         vals["state"] = "draft";

//                         List<OrderLinesList> orderLineList1 = new List<OrderLinesList>();

//                     //  orderLineList1 = JsonConvert.DeserializeObject<List<OrderLinesList>>(dbobj.order_line);

//                     JArray output = JsonConvert.DeserializeObject<JArray>(dbobj.order_line);

//                         foreach (JObject obj in output)
//                         {
//                         //   object[] tax_idList = new object[2];

//                      //  List<int> TaxesIdList = new List<int>();

//                             string product = obj["product_name"].ToString();
//                             double ordered_qty = Convert.ToDouble(obj["product_uom_qty"].ToString());
//                             double unit_price = Convert.ToDouble(obj["price_subtotal"].ToString());
//                              string discount = obj["discount"].ToString();
//                              string multi_discount = obj["multi_discount"].ToString();

//                                List<int> TaxesIdList = obj["taxesid"].ToObject<List<int>>();


//                         //  TaxesIdList = 
//                         string description = "";

//                                OrderLinesList od = new OrderLinesList(product, ordered_qty, unit_price, TaxesIdList, description,"",discount,multi_discount);

//                             orderLineList1.Add(od);
//                         }

//                         vals["order_lines"] = orderLineList1;

//                         string updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_sale_quotation", vals);

//                         if (updated == "true")
//                         {
                        
//                         App._connection.Query<SalesQuotationDB>("UPDATE SalesQuotationDB set yellowimg_string=? Where Dbid=?", "", dbobj.Dbid);

//                             App._connection.CreateTable<SalesQuotationDB>();
//                             var details = (from y in App._connection.Table<SalesQuotationDB>() select y).ToList();
//                             App.SalesQuotationListDb = details;

//                             List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

//                             MessagingCenter.Send<string, String>("MyApp", "QuotListUpdated", "true");


//                         }

//                     }
//                 }


//                 var lead_details = App._connection.Query<CRMLeadDB>("SELECT * from CRMLeadDB where yellowimg_string = ?", "yellowcircle.png");


//                 if (lead_details.Count() != 0 && App.NetAvailable == true)
//                 {

//                     foreach (var dbobj in lead_details)
//                     {
//                         Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

//                         int stateid = 0;
//                         int countryid = 0;

//                         vals["contact_name"] = dbobj.customer;

//                         vals["name"] = dbobj.name;

//                         vals["email_from"] = dbobj.email;
//                         vals["phone"] = dbobj.phone;
//                         vals["function"] = dbobj.function;
//                         vals["street"] = dbobj.street;
//                         vals["street2"] = dbobj.street2;
//                         vals["zip"] = dbobj.zip;

//                     //   var salespersonid = App.salespersons.FirstOrDefault(x => x.Value == salesperson_Picker.SelectedItem.ToString()).Key;
//                     vals["user_id"] = dbobj.user_id;

//                     //   var salesteamid = App.salesteam.FirstOrDefault(x => x.Value == salesteam_Picker.SelectedItem.ToString()).Key;
//                     vals["team_id"] = dbobj.team_id;

//                         if (dbobj.state_id == 0)
//                         {
//                             vals["state_id"] = false;
//                         }
//                         else
//                         {
//                         //  stateid = App.statedict.FirstOrDefault(x => x.Value == state_picker.SelectedItem.ToString()).Key;
//                         vals["state_id"] = dbobj.state_id;
//                         }

//                         if (dbobj.country_id == 0)
//                         {
//                             vals["country_id"] = false;
//                         }
//                         else
//                         {
//                         //  countryid = App.countrydict.FirstOrDefault(x => x.Value == country_picker.SelectedItem.ToString()).Key;
//                         vals["country_id"] = dbobj.country_id;
//                         }


//                         vals["description"] = dbobj.description;

//                         vals["type"] = "lead";

//                           vals["priority"] = dbobj.priority.ToString();

//                          vals["next_activity_id"] = dbobj.nextact_id;

//                          vals["date_deadline"] = dbobj.expected_closing;


//                     //bool updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_crm_quotations", vals);



//                     string updated = Controller.InstanceCreation().UpdateLeadCreationData("crm.lead", "create", vals);

//                         if (updated == "Odoo Error")
//                         {
//                         // App.Current.MainPage = new MasterPage(new CrmTabbedPage());
//                         // Navigation.PushPopupAsync(new MasterPage(  );

//                     }
//                         else
//                         {

//                         //   App._connection.Query<CRMLeadDB>("UPDATE CRMLeadDB SET yellowimg_string = empty " + "WHERE Dbid = ?", dbobj.Dbid);
//                         App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();

//                             try
//                             {
//                                 App._connection.Query<CRMLeadDB>("UPDATE CRMLeadDB set yellowimg_string=? Where Dbid=?", "", dbobj.Dbid);

//                                 App._connection.CreateTable<CRMLeadDB>();
//                                 var details = (from y in App._connection.Table<CRMLeadDB>() select y).ToList();
//                                 App.crmListDb = details;

//                                 List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

//                                 MessagingCenter.Send<string, String>("MyApp", "leadListUpdated", "true");
//                             }

//                             catch (Exception ex)
//                             {

//                             }

//                         }
//                     }

//                 }


//                 var oppor_details = App._connection.Query<CRMOpportunitiesDB>("SELECT * from CRMOpportunitiesDB where yellowimg_string = ?", "yellowcircle.png");

//                 if (oppor_details.Count() != 0 && App.NetAvailable == true)
//                 {

//                     foreach (var dbobj in oppor_details)
//                     {
//                         Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

//                         vals["customer"] = dbobj.cusid;
//                         vals["oppurtunity_title"] = dbobj.oppurtunity_title;
//                         vals["email"] = dbobj.email;
//                         vals["phone"] = dbobj.phone;

//                         vals["next_activity_date"] = dbobj.next_activity_date;

//                         vals["next_activity"] = dbobj.next_activity;
//                         vals["next_activity_summary"] = dbobj.next_activity_summary;

//                         vals["expected_revenue"] = dbobj.expected_revenue;

//                         vals["rating"] = dbobj.rating;
//                         vals["stage"] = dbobj.stage;
//                         vals["internal_notes"] = dbobj.internal_notes;

//                         vals["state"] = "new";

//                         vals["next_activity_id"] = dbobj.nextact_id;

//                         vals["date_deadline"] = dbobj.expected_closing;

//                         List<MeetingLinesList> meetingLineList = new List<MeetingLinesList>();

//                         meetingLineList = JsonConvert.DeserializeObject<List<MeetingLinesList>>(dbobj.meetings);

//                         vals["meetings"] = meetingLineList;

//                         string updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_crm_quotations", vals);

//                         if (updated == "true")
//                         {
//                             App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();

//                             try
//                             {
//                                 App._connection.Query<CRMOpportunitiesDB>("UPDATE CRMOpportunitiesDB set yellowimg_string=? Where Dbid=?", "", dbobj.Dbid);

//                                 App._connection.CreateTable<CRMOpportunitiesDB>();
//                                 var details = (from y in App._connection.Table<CRMOpportunitiesDB>() select y).ToList();
//                                 App.CRMOpportunitiesListDb = details;

//                                 List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

//                                 MessagingCenter.Send<string, String>("MyApp", "oppListUpdated", "true");
//                             }

//                             catch (Exception ex)
//                             {

//                             }

//                         }
//                         else
//                         {

//                         }
//                     }
//                 }


//                 if (!stopWatch.IsRunning && App.userid != 0)
//                 {
//                     stopWatch.Start();
//                 }


//                 if (stopWatch.IsRunning && App.user_gps_enabled == true && stopWatch.Elapsed.Minutes >= App.user_gps_time && App.userid != 0)
//                 {
//                 //prepare to perform your data pull here as we have hit the 1 minute mark   

//                 // Perform your long running operations here.

//                 Device.BeginInvokeOnMainThread(async () =>
//                 {
//                        // If you need to do anything with your UI, you need to wrap it in this.

//                        //Stopwatch stopWatch = new Stopwatch();
//                        //stopWatch.Start();

//                        try
//                            {
//                                var locator = CrossGeolocator.Current;
//                                locator.DesiredAccuracy = 10;
//                                var position = await locator.GetPositionAsync(timeout: new TimeSpan(20000));



//                                var updated = Controller.InstanceCreation().UpdateGPSData1("gps.sales.person.location", "create_sales_person_gps_location", (float)position.Latitude, (float)position.Longitude);
//                            }


//                            catch (Exception ex)
//                            {
//                                if (!App.NetAvailable)
//                                {
//                                    int i = 0;
//                                // await  DisplayAlert("Alert", "Your Internet Connection has been lost.", "Ok");
//                                }

//                                else if (!App.responseState)
//                                {
//                                //  await  DisplayAlert("Alert", "The server is temporarily unavailable", "Ok");
//                                }
//                            }

//                        //  System.Diagnostics.Debug.WriteLine("USER>>>>>>"+ App.userid);

//                    });

//                     stopWatch.Restart();
//                 }

//             // Always return true as to keep our device timer running.
//             return true;
//             });

//            }
//            catch(Exception exce)
//            {
//                System.Diagnostics.Debug.WriteLine(exce.Message);
//            }
           
//        }

//// Back Thread Ends

        protected  override async void OnStart()
        {
            
            //  Task.Run(async () => timer());

         //   await MainRefreshData();


            // Handle when your app starts
        }

        private async void timer()
        {

            // ***********SalesQuotationDB

          //  await MainRefreshData();



           

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps

            stopWatch.Reset();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            stopWatch.Reset();
        }


    }

}
