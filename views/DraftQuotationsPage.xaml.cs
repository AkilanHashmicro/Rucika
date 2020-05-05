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

            MessagingCenter.Subscribe<string, string>("MyApp", "dq_backbtn", async (sender, arg) =>
            {
                act_ind.IsRunning = true;

                await Task.Run(() => App.draftQuotList = Controller.InstanceCreation().GetdraftQuotations());
                draftQuotationListView.ItemsSource = App.draftQuotList;

                act_ind.IsRunning = false;
            });

            MessagingCenter.Subscribe<string, string>("MyApp", "dq_swipped", async (sender, arg) =>
            {
                if (App.draftquot_swipped)
                {
                    act_ind.IsRunning = true;


                    await Task.Run(() => App.draftQuotList = Controller.InstanceCreation().GetdraftQuotations());
                    draftQuotationListView.ItemsSource = App.draftQuotList;
                    App.filterdict.Clear();
                    App.draftquot_swipped = false;

                    act_ind.IsRunning = false;
                }
                else
                {
                    draftQuotationListView.ItemsSource = App.draftQuotList;
                }


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
                draftQuotationListView.ItemsSource = App.draftQuotList;
                App.filterdict.Clear();
                App.draftquot_rpc = false;
            }
            else
            {
                draftQuotationListView.ItemsSource = App.draftQuotList;
            }


            var plusRecognizer = new TapGestureRecognizer();
            plusRecognizer.Tapped += (s, e) => {

                Navigation.PushPopupAsync(new DraftQuotationCreationPage());


            };
            plus.GestureRecognizers.Add(plusRecognizer);


            draftQuotationListView.Refreshing += this.RefreshRequested;
        }

        private async void OnMenuItemTappedAsync(object sender, ItemTappedEventArgs ea)
        {
            act_ind.IsRunning = true;
      
            await Task.Run(()=> Navigation.PushPopupAsync(new DraftQuotationsDetailPage(ea.Item as SalesQuotation)));

            act_ind.IsRunning = false;
         //  PopupNavigation.PopAsync();
     
        }

        //async Task RefreshData()
        //{
        //    List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

        //  //  List<SalesQuotation> crmdraftData = Controller.InstanceCreation().GetDraftQuotationData();
        //}

        private async void RefreshRequested(object sender, object e)
        {
            draftQuotationListView.IsRefreshing = true;
            App.draftQuotList = Controller.InstanceCreation().GetdraftQuotations();
            App.filterdict.Clear();
            draftQuotationListView.ItemsSource = App.draftQuotList;
            draftQuotationListView.IsRefreshing = false;
        
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


                draftQuotationListView.ItemsSource = App.draftQuotList;
            }

            else
            {

                draftQuotationListView.ItemsSource = App.draftQuotList.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));

               
            }
        }

        private void Toolbar_Filter_Activated(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new FilterPopupPage("tab3"));
            //Navigation.PushPopupAsync(new CrmFilterWizard());
        }
    }
}