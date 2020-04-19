using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SalesApp.models.CRMModel;

namespace SalesApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerSelectionPage : PopupPage
    {
        List<Customers> cusresult = new List<Customers>();
        public CustomerSelectionPage()
        {
            InitializeComponent();
            if (App.NetAvailable == true)
            {
                cusresult = App.cusList;
                pickerListView.ItemsSource = cusresult;
            }

            if (App.NetAvailable == false)
            {

                cusresult = App.cusListDB;
                pickerListView.ItemsSource = cusresult;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                pickerListView.ItemsSource = App.cusList;
            }


            else
            {
                pickerListView.ItemsSource = App.cusList.Where(x => x.name.ToLower().Contains(e.NewTextValue.ToLower()));
                // pickerListView.ItemsSource = App.productList.Where(x => x.customer.ToLower().Contains(e.NewTextValue.ToLower()) || x.name.ToLower().Contains(e.NewTextValue.ToLower()));
            }

        }

        private void pickerListView_ItemTapped(object sender, ItemTappedEventArgs ea)
        {
            Customers masterItemObj = (Customers)ea.Item;

            MessagingCenter.Send<string, int>("MyApp", "CusPickerMsg", masterItemObj.id);

            Navigation.PopPopupAsync();
            // Navigation.PushPopupAsync(new CrmLeadDetailWizard(masterItemObj, "Lead"));
        }


        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#f0eaea");
            //  m_title.TextColor = Color.Red;
        }

    }
}
