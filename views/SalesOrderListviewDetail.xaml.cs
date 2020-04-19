using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.DBModel;
using SalesApp.models;
using SalesApp.Pages;
using Xamarin.Forms;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    public partial class SalesOrderListviewDetail : PopupPage
    {

        List<Attachments> attachres = new List<Attachments>();

        int saleoder_id = 0;

        public SalesOrderListviewDetail( SalesOrder item )
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //    App.orderLineList = item.order_line;
            total_cal.IsVisible = true;

            Cus.Text = item.customer;
            CD.Text = item.DateOrder;
            PT.Text = item.payment_term;
            SP.Text = item.sales_person;
            ST.Text = item.sales_team;
            CR.Text = item.client_order_ref;
            FP.Text = item.fiscal_position;

            branch_name.Text = item.branch_id;
            WH.Text = item.warehouse_id;
            shipping_policy.Text = item.picking_policy;
            analytic_account.Text = item.project_id;

            orderListview.ItemsSource = item.order_line;

            saleoder_id = item.id;


            attachres = Controller.InstanceCreation().getfileAtachment(saleoder_id);

            if (attachres.Count == 0)
            {
                attach_name.Text = "Attachment(s)";
            }

            else if (attachres.Count > 0)
            {
                attach_name.Text = attachres.Count + " " + "Attachment(s)";
            }

            orderListview.HeightRequest = item.order_line.Count * 50;

            amt_untax.Text = item.amount_untaxed;
            amt_tax.Text = item.amount_tax;
            amt_total.Text = item.amount_total;

            quotref.Text = item.quotation_reference;
            order_date.Text = item.DateOrder;
            expir_date.Text = item.validity_date;
            delidead_date.Text = item.delivery_deadline;
            con_person.Text = item.contact_person;
            pricelist.Text = item.pricelist;
            invaddr.Text = item.invoice_address;
            deladdr.Text = item.delivery_deadline;
            taxterm.Text = item.tax_term;
            ponum_ref.Text = item.po_number_reference;
            podate_date.Text = item.po_date;
            franco_lbl.Text = item.franco;
            pofile_lbl.Text = item.file_name;
            delmethod_lbl.Text = item.carrier;
            comments.Text = item.special_notes;
           
           

            //var backImgRecognizer = new TapGestureRecognizer();
            //backImgRecognizer.Tapped +=  (s, e) => {
            //    // handle the tap    

            //    Navigation.PopAllPopupAsync();

            //};
            //backImg.GestureRecognizers.Add(backImgRecognizer);

        }


        protected override bool OnBackButtonPressed()
        {

            //   Navigation.PopAllPopupAsync();
            PopupNavigation.PopAsync();
            return true;
        }

        public SalesOrderListviewDetail(SalesOrderDB item)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //    App.orderLineList = item.order_line;



            Cus.Text = item.customer;
            CD.Text = item.DateOrder;
            PT.Text = item.payment_term;
            SP.Text = item.sales_person;
            ST.Text = item.sales_team;
            CR.Text = item.customer_reference;
            FP.Text = item.fiscal_position;

                   List<OrderLine> or_linelistdb = new List<OrderLine>();

           
                    var json_orderline = JsonConvert.SerializeObject(item.order_line);

                    String convertstring = json_orderline.ToString();

                    //  "\"[{\\\"customer_lead\\\":\\\"0\\\",\\\"price_unit\\\":\\\"10000\\\",\\\"product_uom_qty\\\":\\\"10\\\",\\\"price_subtotal\\\":\\\"100000\\\",\\\"taxes\\\":[],\\\"product_name\\\":\\\"Floordeck 1000x060 MM\\\"},{\\\"customer_lead\\\":\\\"0\\\",\\\"price_unit\\\":\\\"1\\\",\\\"product_uom_qty\\\":\\\"1\\\",\\\"price_subtotal\\\":\\\"1\\\",\\\"taxes\\\":[\\\"Sales Tax N/A SRCA-S\\\"],\\\"product_name\\\":\\\"Floordeck 1000x060 MM\\\"}]\""

                    String finstring = convertstring.Replace("\\", "");

                    finstring = finstring.Substring(1);

                    finstring = finstring.Remove(finstring.Length - 1);

                    JArray stringres = JsonConvert.DeserializeObject<JArray>(finstring);

                    //  OrderLine stringres = JsonConvert.DeserializeObject<OrderLine>(json_orderline)

                    int cus_lead = 0;
                    string prod_name = "";




                    foreach (JObject obj in stringres)
                    {
                        OrderLine or_line = new OrderLine();


                        or_line.product_name = obj["product_name"].ToString();
                        or_line.product_uom_qty = obj["product_uom_qty"].ToString();
                        or_line.price_subtotal = obj["price_subtotal"].ToString();
                try
                {
                    or_line.tax_names = obj["tax_names"].ToString();
                }
                catch
                {
                    or_line.tax_names = "";
                }

                try
                {
                    or_line.discount = obj["discount"].ToString();
                }

                catch
                {
                    or_line.discount = "";
                }

                        or_linelistdb.Add(or_line);
                    }


             


            orderListview.ItemsSource = or_linelistdb;


            //var backImgRecognizer = new TapGestureRecognizer();
            //backImgRecognizer.Tapped += async (s, e) => {
            //    // handle the tap    

            //    var currentpage = new LoadingAlert();
            //    await PopupNavigation.PushAsync(currentpage);

            //    // Navigation.PopAllPopupAsync();
            //    App.Current.MainPage = new MasterPage(new CrmTabbedPage());
            //    //  orderListview.ItemsSource = null;
            //    Loadingalertcall();

            //};
            //backImg.GestureRecognizers.Add(backImgRecognizer);
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        private void Tab1Clicked(object sender, EventArgs ea)
        {
            total_cal.IsVisible = true;

            tab1stack.BackgroundColor = Color.FromHex("#363E4B");
            tab1.BackgroundColor = Color.FromHex("#363E4B");
            tab2stack.BackgroundColor = Color.White;
            tab2.BackgroundColor = Color.White;
            tab2frame.BackgroundColor = Color.FromHex("#363E4B");
            tab2borderstack.BackgroundColor = Color.White;
            orderLineList.IsVisible = true;
            OtherInfoStack1.IsVisible = false;
            OtherInfoStack2.IsVisible = false;
            OtherInfoStack3.IsVisible = false;
            tab1frame.BackgroundColor = Color.FromHex("#363E4B");
            tab1borderstack.BackgroundColor = Color.FromHex("#363E4B");
            OrderLineList1.IsVisible = true;

            tab1.TextColor = Color.White;
            tab2.TextColor = Color.Black;
        }

        private void Tab2Clicked(object sender, EventArgs ea)
        {
            total_cal.IsVisible = false;

            //tab2stack.BackgroundColor = Color.FromHex("#363E4B");
            //tab2.BackgroundColor = Color.FromHex("#363E4B");
            //tab1stack.BackgroundColor = Color.White;
            //tab1.BackgroundColor = Color.White;
            //tab2borderstack.BackgroundColor = Color.FromHex("#363E4B");
            //tab2frame.BackgroundColor = Color.FromHex("#363E4B");
            //orderLineList.IsVisible = false;
            //OtherInfoStack1.IsVisible = true;
            //OtherInfoStack2.IsVisible = true;
            //tab1frame.BackgroundColor = Color.White;
            //tab1borderstack.BackgroundColor = Color.FromHex("#363E4B");
            //OrderLineList1.IsVisible = false;
            total_cal.IsVisible = false;

            tab2stack.BackgroundColor = Color.FromHex("#363E4B");
            tab2.BackgroundColor = Color.FromHex("#363E4B");
            tab1stack.BackgroundColor = Color.White;
            tab1.BackgroundColor = Color.White;
            tab2borderstack.BackgroundColor = Color.FromHex("#363E4B");
            tab2frame.BackgroundColor = Color.FromHex("#363E4B");
            orderLineList.IsVisible = false;
            OtherInfoStack1.IsVisible = true;
            OtherInfoStack2.IsVisible = true;
            OtherInfoStack3.IsVisible = true;
            tab1frame.BackgroundColor = Color.FromHex("#363E4B");
            tab1borderstack.BackgroundColor = Color.White;
            OrderLineList1.IsVisible = false;

          

            tab1.TextColor = Color.Black;
            tab2.TextColor = Color.White;


        }

        private async void attachStackClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AttachmentPopupPage(attachres, saleoder_id));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<string, List<Attachments>>("MyApp", "attachUpdated", (sender, arg) =>
            {
                // List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

                //   List<Attachments> attch = new List<Attachments>();
                //  crmLeadListView.ItemsSource = App.crmList;

                attachres = arg;

                if (attachres.Count == 0)
                {
                    attach_name.Text = "Attachment(s)";
                }

                else if (attachres.Count > 0)
                {
                    attach_name.Text = attachres.Count + " " + "Attachment(s)";
                }

                //    attach_name.Text = attachres.Count + " " + "Attachment(s)";


            });
        }

    }
}   
