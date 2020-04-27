using Rg.Plugins.Popup.Extensions;
using SalesApp.models;
using SalesApp.wizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SalesApp.Pages;
using static SalesApp.models.CRMModel;
using Plugin.Messaging;
using SalesApp.DBModel;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OppurtunityPage : ContentPage
    {

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<string, string>("MyApp", "oppListUpdated", (sender, arg) =>
            {
                // List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();
                oppurtunityListView.ItemsSource = App.crmOpprList;

            });

            MessagingCenter.Subscribe<string, string>("MyApp", "opp_swipped", async (sender, arg) =>
            {
                if (App.oppo_swipped)
                {
                    act_ind.IsRunning = true;

                    await Task.Run(() => App.crmOpprList = Controller.InstanceCreation().GetOpportunityList());
                    oppurtunityListView.ItemsSource = App.crmOpprList;
                    App.filterdict.Clear();
                    App.oppo_swipped = false;

                    act_ind.IsRunning = false;
                }
                else
                {
                    oppurtunityListView.ItemsSource = App.crmOpprList;
                }
                             
            });
        }

    

        public OppurtunityPage()
        {
            Title = "CRM Opportunities";

            BackgroundColor = Color.White;
            InitializeComponent();
             
            if (App.oppo_rpc)
            {
               
                App.crmOpprList = Controller.InstanceCreation().GetOpportunityList();
                oppurtunityListView.ItemsSource = App.crmOpprList;
                App.filterdict.Clear();

                App.oppo_rpc = false;
            }
            else
            {
                oppurtunityListView.ItemsSource = App.crmOpprList;
            }
                          
            oppurtunityListView.Refreshing += this.RefreshRequested;

            var plusRecognizer = new TapGestureRecognizer();
            plusRecognizer.Tapped += (s, e) => {

               Navigation.PushPopupAsync(new CRMOpportunityCreationPage1());
                                              
            };
            plus.GestureRecognizers.Add(plusRecognizer);
         }

        private void OnMenuItemTapped(object sender, ItemTappedEventArgs ea)
        {

            if (App.NetAvailable == true)
            {

                var result1 = from y in App.CRMOpportunitiesListDb
                              where y.yellowimg_string == "yellowcircle.png"
                              select y;

                if (result1.Count() == 0)
                {
                    try
                    {
                        CRMOpportunities masterItemObj = (CRMOpportunities)ea.Item;
                        Navigation.PushPopupAsync(new CrmOppDetailWizard(masterItemObj));
                    }

                    catch
                    {
                        CRMOpportunitiesDB masterItemObj = (CRMOpportunitiesDB)ea.Item;
                        Navigation.PushPopupAsync(new CrmOppDetailWizard(masterItemObj));
                    }
                }

                else
                {
                    try
                    {
                        CRMOpportunitiesDB masterItemObj = (CRMOpportunitiesDB)ea.Item;
                        Navigation.PushPopupAsync(new CrmOppDetailWizard(masterItemObj));
                    }

                    catch
                    {
                        CRMOpportunities masterItemObj = (CRMOpportunities)ea.Item;
                        Navigation.PushPopupAsync(new CrmOppDetailWizard(masterItemObj));
                    }
                }

            }

            else if (App.NetAvailable == false)
            {
                try
                {
                    CRMOpportunitiesDB masterItemObj = (CRMOpportunitiesDB)ea.Item;
                    Navigation.PushPopupAsync(new CrmOppDetailWizard(masterItemObj));
                }

                catch
                {
                    CRMOpportunities masterItemObj = (CRMOpportunities)ea.Item;
                    Navigation.PushPopupAsync(new CrmOppDetailWizard(masterItemObj));
                }
            }

        }

        private async void RefreshRequested(object sender, object e)
        {

        
                oppurtunityListView.IsRefreshing = true;
            //   await Task.Delay(200);                
                App.crmOpprList = Controller.InstanceCreation().GetOpportunityList();
                oppurtunityListView.ItemsSource = App.crmOpprList;
                App.filterdict.Clear();

            oppurtunityListView.IsRefreshing = false;
        }

        private void Toolbar_Search_Activated(object sender, EventArgs e)
        {
            if (searchBar.IsVisible)
            {
                searchBar.IsVisible = false;
            }
            else { searchBar.IsVisible = true; }
        }

        private void Toolbar_Filter_Activated(object sender, EventArgs e)
        {
            // Navigation.PushPopupAsync(new CrmFilterWizard());
            Navigation.PushPopupAsync(new FilterPopupPage("tab2")); 
        }



        private void meetingClicked(object sender, EventArgs e1)
        {
            try
            {
                App.Current.MainPage = new MasterPage(new CalendarPage());
            }

            catch
            {
                if (App.NetAvailable == false)
                {
                    DisplayAlert("Alert", "Need Internet Connection", "Ok");
                }
            }
        }

        async void phoneClicked(object sender, EventArgs e1)
        {
            // taxes myobj = sender as taxes;
            var args = (TappedEventArgs)e1;

            try
            {

                CRMOpportunities myobj = args.Parameter as CRMOpportunities;
                var phoneDialer = CrossMessaging.Current.PhoneDialer;
                if (phoneDialer.CanMakePhoneCall && myobj.phone != "")
                {
                    phoneDialer.MakePhoneCall(myobj.phone);
                }
                else
                {
                    await Navigation.PushPopupAsync(new PhoneWizard());
                }

            }

            catch
            {
                CRMOpportunitiesDB myobj = args.Parameter as CRMOpportunitiesDB;
                var phoneDialer = CrossMessaging.Current.PhoneDialer;
                if (phoneDialer.CanMakePhoneCall && myobj.phone != "")
                {
                    phoneDialer.MakePhoneCall(myobj.phone);
                }
                else
                {
                    await Navigation.PushPopupAsync(new PhoneWizard());
                }
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                var result1 = from y in App.CRMOpportunitiesListDb
                              where y.yellowimg_string == "yellowcircle.png"
                              select y;

                if (result1.Count() == 0)
                {
                    oppurtunityListView.ItemsSource = App.crmOpprList;
                }

                else
                {
                    oppurtunityListView.ItemsSource = App.CRMOpportunitiesListDb;
                }
            }

            else
            {

                var result1 = from y in App.CRMOpportunitiesListDb
                              where y.yellowimg_string == "yellowcircle.png"
                              select y;

                if (result1.Count() == 0)
                {
                    oppurtunityListView.ItemsSource = App.crmOpprList.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                }

                else
                {
                    oppurtunityListView.ItemsSource = App.CRMOpportunitiesListDb.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                }


            }
        }
    }
}