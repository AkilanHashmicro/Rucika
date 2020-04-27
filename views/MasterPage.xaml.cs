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
