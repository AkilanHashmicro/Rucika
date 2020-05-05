using Rg.Plugins.Popup.Extensions;
using SalesApp.models;
using SalesApp.wizard;
using SalesApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SalesApp.models.CRMModel;
using SalesApp.DBModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuotationPage : ContentPage
    {
        int tap_count = 0;

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
            MessagingCenter.Subscribe<string, string>("MyApp", "QuotListUpdated", (sender, arg) =>
            {
               
            });

            MessagingCenter.Subscribe<string, string>("MyApp", "sq_swipped", async (sender, arg) =>
            {
                if (App.quot_swipped)
                {
                    act_ind.IsRunning = true;
                   
                    await Task.Run(() => App.salesQuotList = Controller.InstanceCreation().GetSalesQuotations());
                    salesQuotationListView.ItemsSource = App.salesQuotList;
                    App.filterdict.Clear();
                    App.quot_swipped = false;

                    act_ind.IsRunning = false;
                }
                else
                {
                    salesQuotationListView.ItemsSource = App.salesQuotList;
                }

            });

            MessagingCenter.Subscribe<string, string>("MyApp", "sq_backbtn", async (sender, arg) =>
            {
                act_ind.IsRunning = true;

                await Task.Run(() => App.salesQuotList = Controller.InstanceCreation().GetSalesQuotations());
                salesQuotationListView.ItemsSource = App.salesQuotList;
             
                act_ind.IsRunning = false;
            });
        }


      
        public  QuotationPage()
        {
            Title = "Sales Quotations";

            BackgroundColor = Color.White;
            InitializeComponent();
                                   
            if (App.salequot_rpc)
            {
                
                App.salesQuotList = Controller.InstanceCreation().GetSalesQuotations();
                salesQuotationListView.ItemsSource = App.salesQuotList;
                App.filterdict.Clear();
                App.salequot_rpc = false;
            }
            else
            {
                salesQuotationListView.ItemsSource = App.salesQuotList;
            }

           
            var plusRecognizer = new TapGestureRecognizer();
            plusRecognizer.Tapped += (s, e) => {

                try
                {
                    Navigation.PushPopupAsync(new SalesQuotationCreationPage());
                }

                catch(Exception excep)
                {
                    int i = 0;
                }
                  
            };
            plus.GestureRecognizers.Add(plusRecognizer);


            salesQuotationListView.Refreshing += this.RefreshRequested;
        }

        private async void OnMenuItemTappedAsync(object sender, ItemTappedEventArgs ea)
        {
            act_ind.IsRunning = true;


            await Task.Run(() => Navigation.PushPopupAsync(new SalesQuotationsListviewDetail(ea.Item as SalesQuotation)));

            act_ind.IsRunning = false;
        }

      
        private void RefreshRequested(object sender, object e)
        {
            salesQuotationListView.IsRefreshing = true;
           
            App.salesQuotList = Controller.InstanceCreation().GetSalesQuotations();
            salesQuotationListView.ItemsSource = App.salesQuotList;
            App.filterdict.Clear();
            salesQuotationListView.IsRefreshing = false;
        }

        private void Toolbar_Search_Activated(object sender, EventArgs e)
        {
            if (searchBar.IsVisible)
            {
                searchBar.IsVisible = false;
            }   
            else { searchBar.IsVisible = true; }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                var result1 = from y in App.SalesQuotationListDb
                              where y.yellowimg_string == "yellowcircle.png"
                              select y;

                if (result1.Count() == 0)
                {
                    salesQuotationListView.ItemsSource = App.salesQuotList;
                }

                else
                {
                    salesQuotationListView.ItemsSource = App.SalesQuotationListDb;
                }
            }

            else
            {

                var result1 = from y in App.SalesQuotationListDb
                              where y.yellowimg_string == "yellowcircle.png"
                              select y;

                if (result1.Count() == 0)
                {
                    salesQuotationListView.ItemsSource = App.salesQuotList.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                }

                else
                {
                    salesQuotationListView.ItemsSource = App.SalesOrderListDb.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }
        }

        private void Toolbar_Filter_Activated(object sender, EventArgs e)
        {            
                Navigation.PushPopupAsync(new FilterPopupPage("tab4"));
        }
    }
}