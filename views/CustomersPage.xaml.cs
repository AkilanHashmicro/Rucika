using FFImageLoading.Forms;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SalesApp.models;
using SalesApp.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomersPage : ContentPage
    {
        Image Pic = new Image();
        List<CustomersModel> customerdata = new List<CustomersModel>();
        public CustomersPage()
        {
            InitializeComponent();

           // customerdata = Controller.InstanceCreation().GetCustomerData();
            Customerlist.ItemsSource = App.cusList;
             
            //var plusRecognizer = new TapGestureRecognizer();
            //plusRecognizer.Tapped += (s, e) =>
            //{
            //  //  App.Current.MainPage = new MasterPage(new CRMLeadCreationPage());

            //    Navigation.PushPopupAsync( new CRMLeadCreationPage());
            //};
            //plus.GestureRecognizers.Add(plusRecognizer);

        }

        private async void CustomerListView_ItemTappedAsync(object sender, ItemTappedEventArgs e)
        {

            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);

            CustomersModel modelObj = e.Item as CustomersModel;
            await Navigation.PushPopupAsync(new CustomerListviewDetailPage(modelObj.id));
          

         //   Navigation.PushPopupAsync(new CustomerListviewDetailPage(modelObj));
        //    Navigation.PushAsync(new CustomerListviewDetailPage(modelObj));

        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
           // obj.View.BackgroundColor = Color.FromHex("#414141");  
            obj.View.BackgroundColor = Color.White;
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

                Customerlist.ItemsSource = customerdata;
                // Customerlist.HeightRequest = 60 * customerdata.Count;


                if (searchBar.Text != "")
                {
                    // btn_layout.IsVisible = true;
                }

                else if (searchBar.Text == "")
                {
                    //  btn_layout.IsVisible = false;
                }
            }
            else
            {

                var data = customerdata.Where(x => x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                // Customerlist.HeightRequest = 60 * data.Count();
                Customerlist.ItemsSource = data;


            }

        }

    }
}
