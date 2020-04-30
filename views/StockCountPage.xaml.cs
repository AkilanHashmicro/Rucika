using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Extensions;
using SalesApp.models;
using SalesApp.Pages;
using Xamarin.Forms;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    public partial class StockCountPage : ContentPage
    {
        int product_id = 0;
        int location_id = 0;

        public StockCountPage()
        {
            InitializeComponent();

            // stockcountListView.ItemsSource = App.productList;

            if (App.StockFilterProduct == "" && App.StockFilterWarehouse == "")
            {
                //searchBar.IsVisible = false;
                //warehousesBar.IsVisible = false;
                //wh_stock.IsVisible = false;
                //pro_stock.IsVisible = false;

                searchBar.IsVisible = true;
                warehousesBar.IsVisible = true;
                wh_stock.IsVisible = false;
                pro_stock.IsVisible = false;
            }

            else if (App.StockFilterWarehouse == "Ware" && App.StockFilterProduct == "")
            {
                searchBar.IsVisible = false;
                warehousesBar.IsVisible = true;
                //product_layout.IsVisible = false;
                //location_layout.IsVisible = true;
                pro_stock.IsVisible = false;
                wh_stock.IsVisible = true;
            }

            else if (App.StockFilterWarehouse == "" && App.StockFilterProduct == "Product")
            {
                searchBar.IsVisible = true;
                warehousesBar.IsVisible = false;
                pro_stock.IsVisible = true;
                wh_stock.IsVisible = false;
            }

            else if (App.StockFilterWarehouse == "Ware" && App.StockFilterProduct == "Product")
            {
                searchBar.IsVisible = true;
                warehousesBar.IsVisible = true;
            }
            else
            {

            }

        }

        //private void Toolbar_Filter_Activated(object sender, EventArgs e)
        //{
        //    Navigation.PushPopupAsync(new StockFilterPage());
        //}

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                productlabel.IsVisible = true;
                pro_stock.IsVisible = false;
                stockcountListView.ItemsSource = App.productList;
                stockcountListView.HeightRequest = 60 * App.productList.Count;
                locationlabel.IsVisible = false;
                wh_stock.IsVisible = false;

                if (searchBar.Text != "" || warehousesBar.Text != "")
                {
                    btn_layout.IsVisible = true;
                }

                else if(searchBar.Text == "" && warehousesBar.Text == "")
                {
                    btn_layout.IsVisible = false;
                }
            }
            else
            {
                productlabel.IsVisible = true;
                pro_stock.IsVisible = true;
                var data = App.productList.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()));
                stockcountListView.HeightRequest = 60 * data.Count();
                stockcountListView.ItemsSource = data;
                locationlabel.IsVisible = false;
                wh_stock.IsVisible = false;

              
                btn_layout.IsVisible = false;

            }

        }

        private void warehousesBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                wh_stock.IsVisible = false;
                locationlabel.IsVisible = true;
                warehouseListview.ItemsSource = App.locationsList;
                warehouseListview.HeightRequest = 45 * App.locationsList.Count;
                pro_stock.IsVisible = false;
                productlabel.IsVisible = false;

                if (searchBar.Text != "" || warehousesBar.Text != "")
                {
                    btn_layout.IsVisible = true;
                }

                else if (searchBar.Text == "" && warehousesBar.Text == "")
                {
                    btn_layout.IsVisible = false;
                }
              //  btn_layout.IsVisible = false;
            }

            else
            {
                wh_stock.IsVisible = true;
                locationlabel.IsVisible = true;
                var data = App.locationsList.Where(x => x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                warehouseListview.HeightRequest = 45 * data.Count();
                warehouseListview.ItemsSource = data;
                pro_stock.IsVisible = false;
                productlabel.IsVisible = false;
                btn_layout.IsVisible = false;
            }
        }



        private void stockcountListView_ItemTapped(object sender, ItemTappedEventArgs ea)
        {

            ProductsList pd = ea.Item as ProductsList;
            product_id = pd.Id;

            searchBar.Text = pd.Name;

            pro_stock.IsVisible = false;
            wh_stock.IsVisible = false;

            btn_layout.IsVisible = true;
        }

        private void warehouseListview_ItemTapped(object sender, ItemTappedEventArgs ea)
        {
            LocationsList pd = ea.Item as LocationsList;
            location_id = pd.id;

            warehousesBar.Text = pd.name;
            wh_stock.IsVisible = false;
            pro_stock.IsVisible = false;

            btn_layout.IsVisible = true;
        }


        private void cancel_Clicked(object sender, EventArgs e)
        {

          //  pro_stock.IsVisible = false;
           // wh_stock.IsVisible = false;
            btn_layout.IsVisible = false;

            searchBar.Text = "";
            warehousesBar.Text = "";
            //searchBar.IsVisible = false;
            //warehousesBar.IsVisible = false;
            locationlabel.IsVisible = false;

            searchBar.IsVisible = true;
            warehousesBar.IsVisible = true;
            wh_stock.IsVisible = false;
            pro_stock.IsVisible = false;
        }


        private void search_Clicked(object sender, EventArgs e)
        {
           // if (App.StockFilterWarehouse == "Ware" && App.StockFilterProduct == "")

            if (searchBar.Text == "" && warehousesBar.Text != "")
            {
                try
                {
                    List<StockWareHouseList> data = Controller.InstanceCreation().GetStockWareHouseData(location_id);
                    if (data[0].location == null)
                    {
                        DisplayAlert("Alert", "Not available", "Ok");
                    }
                    else
                    {
                        Navigation.PushPopupAsync(new StockCountDetailPage(data));
                    }
                }
                catch (Exception exce)
                {
                    DisplayAlert("Alert", "Product not available", "Ok");
                }

            }

            //  else if (App.StockFilterProduct == "Product" && App.StockFilterWarehouse == "")
            else if (searchBar.Text != "" && warehousesBar.Text == "")

             {
                try
                {
                    List<StockWareHouseList> data = Controller.InstanceCreation().GetStockProductData(product_id);
                    Navigation.PushPopupAsync(new StockCountDetailPage(data));
                }

                catch (Exception exce)
                {
                    DisplayAlert("Alert", "Product not available", "Ok");
                }

            }

            else if (searchBar.Text != "" && warehousesBar.Text != "")
            {
                if (product_id != 0 && location_id != 0)
                {
                    List<StockWareHouseList> data = Controller.InstanceCreation().GetStockAllData(product_id, location_id);
                    Navigation.PushPopupAsync(new StockCountDetailPage(data));
                }

                else
                {
                    DisplayAlert("Alert", "Please Select Both", "Ok");
                }
            }
        }


        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    MessagingCenter.Subscribe<string, string>("MyApp", "NotifyMsg", (sender, arg) =>
        //    {
        //        // HideLbl.Text = "New Quotation Creation";
        //        searchBar.IsVisible = true;

        //    });


        //}

    }
}