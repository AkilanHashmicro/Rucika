using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SalesApp.DBModel;
using SalesApp.models;
using SalesApp.Persistance;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrmTabbedPage : TabbedPage
    {
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    MessagingCenter.Subscribe<string, string>("MyApp", "Login_auto", async (sender, arg) =>
        //    {
        //        await Task.Run(() =>
        //        {
        //            var user_details = (from y in App._connection.Table<UserModelDB>() select y).ToList();
        //            if (App.cusList.Count == 0 && user_details.Count == 0 && Settings.UserId != 0)
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


        //        });
        //    });
        //}


        public CrmTabbedPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            BarBackgroundColor = Color.FromHex("#363E4B");

            var crmLeadPage = new NavigationPage(new CrmLeadPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            crmLeadPage.Icon = "lead.png";
            Children.Add(crmLeadPage);

            App.load_rpc = false;

            var crmOppurtunityPage = new NavigationPage(new OppurtunityPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            crmOppurtunityPage.Icon = "oppurtunity.png";
            //crmOppurtunityPage.Title = "Opportunity";
            Children.Add(crmOppurtunityPage);

            var crmquotPage = new NavigationPage(new DraftQuotationsPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            crmquotPage.Icon = "draftquot.png";
            //crmOppurtunityPage.Title = "Opportunity";
            Children.Add(crmquotPage);


            var quotationPage = new NavigationPage(new QuotationPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            quotationPage.Icon = "quotations.png";
            Children.Add(quotationPage);

            var salesOrderPage = new NavigationPage(new SalesOrderPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            salesOrderPage.Icon = "salesorder.png";
            //salesOrderPage.Title = "Sales Order";
            Children.Add(salesOrderPage);

            this.CurrentPageChanged += CurrentPageHasChanged;

        }


        public CrmTabbedPage(string tabs)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            BarBackgroundColor = Color.FromHex("#363E4B");

            var crmLeadPage = new NavigationPage(new CrmLeadPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            crmLeadPage.Icon = "lead.png";
            Children.Add(crmLeadPage);

            var crmOppurtunityPage = new NavigationPage(new OppurtunityPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            crmOppurtunityPage.Icon = "oppurtunity.png";
            //crmOppurtunityPage.Title = "Opportunity";
            Children.Add(crmOppurtunityPage);

            var crmquotPage = new NavigationPage(new DraftQuotationsPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            crmquotPage.Icon = "draftquot.png";
            //crmOppurtunityPage.Title = "Opportunity";
            Children.Add(crmquotPage);

            var quotationPage = new NavigationPage(new QuotationPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            quotationPage.Icon = "quotations.png";
            Children.Add(quotationPage);

            var salesOrderPage = new NavigationPage(new SalesOrderPage()) { BarBackgroundColor = Color.FromHex("#363E4B") };
            salesOrderPage.Icon = "salesorder.png";
            //salesOrderPage.Title = "Sales Order";
            Children.Add(salesOrderPage);

            var tabPage = this as TabbedPage;

            if (tabs == "tab1")
            {
                tabPage.CurrentPage = tabPage.Children[0];
            }

            else if (tabs == "tab2")
            {
                tabPage.CurrentPage = tabPage.Children[1];
            }

            else if (tabs == "tab3")
            {
                tabPage.CurrentPage = tabPage.Children[2];
            }

            else if (tabs == "tab4")
            {
                tabPage.CurrentPage = tabPage.Children[3];
            }

            else if (tabs == "tab5")
            {
                tabPage.CurrentPage = tabPage.Children[4];
            }

            this.CurrentPageChanged += CurrentPageHasChanged;

        }

        protected void CurrentPageHasChanged(object sender, EventArgs e)
        {

            // ImageSource icon = CurrentPage.IconImageSource;
            string icon_name = CurrentPage.Icon;

            if (icon_name == "oppurtunity.png")
            {
                MessagingCenter.Send<string, string>("MyApp", "opp_swipped", "true");
            }

            else if (icon_name == "draftquot.png")
            {
                MessagingCenter.Send<string, string>("MyApp", "dq_swipped", "true");
            }
            else if (icon_name == "quotations.png")
            {
                MessagingCenter.Send<string, string>("MyApp", "sq_swipped", "true");
            }
            else if (icon_name == "salesorder.png")
            {
                MessagingCenter.Send<string, string>("MyApp", "so_swipped", "true");
            }
        }

    }
}