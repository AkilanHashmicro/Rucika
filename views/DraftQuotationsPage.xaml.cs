using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SalesApp.DBModel;
using SalesApp.models;
using SalesApp.Pages;
using Xamarin.Forms;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    public partial class DraftQuotationsPage : ContentPage
    {
        List<SalesQuotation> crmdraftData = new List<SalesQuotation>();


        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<string, string>("MyApp", "dq_swipped", async (sender, arg) =>
            {
                if (App.draftquot_swipped)
                {
                    act_ind.IsRunning = true;


                    await Task.Run(() => App.draftQuotList = Controller.InstanceCreation().GetdraftQuotations());
                    salesQuotationListView.ItemsSource = App.draftQuotList;
                    App.filterdict.Clear();
                    App.draftquot_swipped = false;

                    act_ind.IsRunning = false;
                }
                else
                {
                    salesQuotationListView.ItemsSource = App.draftQuotList;
                }

                //    salesQuotationListView.ItemsSource = App.salesQuotList;
            });
        }

        public DraftQuotationsPage()
        {
            InitializeComponent();

            Title = "Draft Quotations";
            BackgroundColor = Color.White;
           
            if (App.draftquot_rpc)
            {
                
                App.draftQuotList = Controller.InstanceCreation().GetdraftQuotations();
                salesQuotationListView.ItemsSource = App.draftQuotList;
                App.filterdict.Clear();
                App.draftquot_rpc = false;
            }
            else
            {
                salesQuotationListView.ItemsSource = App.draftQuotList;
            }


            var plusRecognizer = new TapGestureRecognizer();
            plusRecognizer.Tapped += (s, e) => {

                Navigation.PushPopupAsync(new DraftQuotationCreationPage());


            };
            plus.GestureRecognizers.Add(plusRecognizer);


            salesQuotationListView.Refreshing += this.RefreshRequested;
        }

        private async void OnMenuItemTappedAsync(object sender, ItemTappedEventArgs ea)
        {

      
            await Navigation.PushPopupAsync(new DraftQuotationsDetailPage(ea.Item as SalesQuotation));

     
        }

        //async Task RefreshData()
        //{
        //    List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

        //  //  List<SalesQuotation> crmdraftData = Controller.InstanceCreation().GetDraftQuotationData();
        //}

        private async void RefreshRequested(object sender, object e)
        {
            salesQuotationListView.IsRefreshing = true;
            App.draftQuotList = Controller.InstanceCreation().GetdraftQuotations();
            App.filterdict.Clear();
            salesQuotationListView.ItemsSource = App.draftQuotList;
        
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
                //var result1 = from y in App.SalesQuotationListDb
                //              where y.yellowimg_string == "yellowcircle.png"
                //              select y;

                //if (result1.Count() == 0)
                //{
                //    salesQuotationListView.ItemsSource = App.salesQuotList;
                //}

                //else
                //{
                //    salesQuotationListView.ItemsSource = App.SalesQuotationListDb;
                //}

                salesQuotationListView.ItemsSource = App.draftQuotList;
            }

            else
            {

                salesQuotationListView.ItemsSource = App.draftQuotList.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));

                //var result1 = from y in App.SalesQuotationListDb
                //              where y.yellowimg_string == "yellowcircle.png"
                //              select y;

                //if (result1.Count() == 0)
                //{
                //    salesQuotationListView.ItemsSource = App.salesQuotList.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                //}

                //else
                //{
                //    salesQuotationListView.ItemsSource = App.SalesOrderListDb.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                //}

                //  salesQuotationListView.ItemsSource = App.salesQuotList.Where(x => x.name.ToLower().StartsWith(e.NewTextValue.ToLower()));

            }
        }

        private void Toolbar_Filter_Activated(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new FilterPopupPage("tab3"));
            //Navigation.PushPopupAsync(new CrmFilterWizard());
        }
    }
}