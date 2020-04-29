using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.DBModel;
using SalesApp.models;
using SalesApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    public partial class DraftQuotationsDetailPage : PopupPage
    {

        string taxname = "";
        string overper_string = "";
        String file_uploadstring = "";
        String file_uploadname = "";

        Dictionary<int, string> cus_selectdict = new Dictionary<int, string>();

        List<Attachments> attachres = new List<Attachments>();

        bool editbtn_clicked = false;

        bool add_new_orderline = false;

        bool tap2_clicked = false;

        List<int> taxidList = new List<int>();

        object[] taxidListObj = new object[10];

        int saleoder_id = 0;

        int orderline_id = 0;

        SalesQuotationDB dbobj = new SalesQuotationDB();

        SalesQuotation obj = new SalesQuotation();

        List<OrderLine> final_listview = new List<OrderLine>();

        List<taxes> taxList_edit = new List<taxes>();

        // List<OrderLine> tax_orderline = new List<OrderLine>();

        List<OrderLine> final_listviewnew = new List<OrderLine>();

        List<OrderLinesListForUpdate> orderLinelist = new List<OrderLinesListForUpdate>();



        protected override bool OnBackButtonPressed()
        {

            //  Loading();

          //  App.Current.MainPage = new MasterPage(new DraftQuotationsPage());

            PopupNavigation.PopAllAsync();

          //  Loadingalertcall();

            return true;

        }

        async void Loading()
        {
            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        private async void set_to_Clicked(object sender, EventArgs ea)
        {

            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);

            try
            {
                string res = Controller.InstanceCreation().SetToConfirm("sale.order", "SetToQuotation", saleoder_id);

                if (res == "true")

                {
                    await DisplayAlert("Alert", "Moved Succssfully", "Ok");
                    App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab3"));
                }

                else
                {
                    await DisplayAlert("Alert", "Try Again", "Ok");
                }

            }

            catch (Exception e)
            {
                // int j = 0;
            }

            Loadingalertcall();

        }


        public DraftQuotationsDetailPage(SalesQuotation item)
        {
            InitializeComponent();

            total_cal.IsVisible = true;



           

            obj = item;

            Cus.Text = item.customer;
            // CD.Text = item.DateOrder;
            PT.Text = item.payment_term;
            CG.Text = item.commission_group;
            SP.Text = item.sales_person;
            ST.Text = item.sales_team;
            CR.Text = item.client_order_ref;
            FP.Text = item.fiscal_position;
            saleoder_id = item.id;
        //    userlocation.Text = App.user_location_string;

            quot_ref.Text = item.quotation_reference;

            branch_name.Text = item.branch_id;
            analytic_account.Text = item.project_id;
            WH.Text = item.warehouse_id;
            shipping_policy.Text = item.picking_policy;


         attachres = Controller.InstanceCreation().getfileAtachment(saleoder_id);

            if(attachres.Count == 0)
            {
                attach_name.Text = "Attachment(s)"; 
            }

            else if(attachres.Count>0)
            {
                attach_name.Text = attachres.Count + " " + "Attachment(s)"; 
            }

            final_listview = item.order_line;

            //OrderLine ite = new OrderLine();

            //final_listview.Add(ite);

            orderListview.ItemsSource = null;
            orderListview.ItemsSource = item.order_line;

            amt_untax.Text = item.amount_untaxed;
            amt_tax.Text = item.amount_tax;
            amt_total.Text = item.amount_total;
            ponumref_lbl.Text = item.po_number_reference;

            order_date.Text = item.DateOrder;
            expir_date.Text = item.validity_date;
            delidead_date.Text = item.delivery_deadline;
            contperson.Text = item.contact_person;
            pricelist.Text = item.pricelist;
            invaddr.Text = item.invoice_address;
            deladdr.Text = item.delivery_deadline;
            taxterm.Text = item.tax_term;
            podate_date.Text = item.po_date;
            franco_lbl.Text = item.franco;
            filename_lbl.Text = item.file_name;
            delmethod_lbl.Text = item.carrier;
            comments.Text = item.special_notes;

            file_uploadstring = item.po_file;
            file_uploadname = item.file_name;

            specialnotes_noedit.Text = item.special_notes;

            // quotref_lbl.Text = item.quotation_reference;

            // tax_listview.ItemsSource = item.order_line;

            orderListview.HeightRequest = item.order_line.Count * 55;

            var sq_editRecognizer = new TapGestureRecognizer();
            sq_editRecognizer.Tapped += async (s, e) =>
            {
                // handle the tap              
                // noedit_layout.IsVisible = false;

                // List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

                total_cal.IsVisible = false;


                //try
                //{
                //    String res = Controller.InstanceCreation().SaleOrderConfirm("sale.order", "confirm_sale_quotation", saleoder_id);
                //}

                //catch
                //{
                //    int j = 0;
                //}

                if (App.NetAvailable == false)
                {
                    await DisplayAlert("Alert", "Need Internet Connection", "Ok");
                }

                else
                {
                    sq_editbtn.IsVisible = false;

                    updatebtn.IsVisible = false;

                    savebtn_layout.IsVisible = true;

                    editbtn_clicked = true;

                    cus_edit.IsVisible = true;
                    // con_dateedit.IsVisible = true;
                    ptpicker_edit.IsVisible = true;
                    commissionpicker_edit.IsVisible = true;
                    sales_teamedit.IsVisible = true;
                    sales_personsedit.IsVisible = true;


                    branch_noedit.IsVisible = false;
                    analytic_noedit.IsVisible = false;
                    wh_noedit.IsVisible = false;
                    sp_noedit.IsVisible = false;


                    cr_edit.IsVisible = true;
                    fp_edit.IsVisible = true;
                    is_edit.IsVisible = true;

                    //   quotref_edit.IsVisible = true;
                    orderdate_dateedit.IsVisible = true;
                    expir_dateedit.IsVisible = true;
                    delidead_dateedit.IsVisible = true;
                    contperson_edit.IsVisible = true;
                    pricelist_edit.IsVisible = true;
                    invaddr_edit.IsVisible = true;
                    deladdr_edit.IsVisible = true;
                    ponumref_edit.IsVisible = true;
                    taxterm_edit.IsVisible = true;
                    podate_dateedit.IsVisible = true;
                    franco_edit.IsVisible = true;
                    pofile_edit.IsVisible = true;
                    delmethod_edit.IsVisible = true;

                    branch_edit.IsVisible = true;
                    analytic_edit.IsVisible = true;
                    wh_edit.IsVisible = true;
                    sp_edit.IsVisible = true;


                    cus_noedit.IsVisible = false;
                    //  con_datenoedit.IsVisible = false;
                    ptpicker_noedit.IsVisible = false;
                    commissionpicker_noedit.IsVisible = false;
                    sales_teamnoedit.IsVisible = false;
                    sales_personsnoedit.IsVisible = false;

                    cr_noedit.IsVisible = false;
                    fp_noedit.IsVisible = false;
                    is_noedit.IsVisible = false;

                    // quotref_noedit.IsVisible = false;
                    orderdate_datenoedit.IsVisible = false;
                    expir_datenoedit.IsVisible = false;
                    delidead_datenoedit.IsVisible = false;
                    contperson_noedit.IsVisible = false;
                    pricelist_noedit.IsVisible = false;
                    invaddr_noedit.IsVisible = false;
                    deladdr_noedit.IsVisible = false;
                    ponumref_noedit.IsVisible = false;
                    taxterm_noedit.IsVisible = false;
                    podate_datenoedit.IsVisible = false;
                    franco_noedit.IsVisible = false;
                    pofile_noedit.IsVisible = false;
                    delmethod_noedit.IsVisible = false;

                 //   br_analytic.IsVisible = false;

                    try

                    {

                        //  od_Picker.Date = item.order_date;
                        if (item.dateOrder != "")
                        {
                            DateTime oDate = DateTime.Parse(item.dateOrder);
                            od_Picker.Date = oDate;
                        }
                      
                        if(item.validity_date !="")
                        {
                            DateTime oDate = DateTime.Parse(item.validity_date);
                            expir_Picker.Date = oDate;
                        }
                        salesteam_picker.ItemsSource = App.salesteam.Select(x => x.Value).ToList();
                        salesteam_picker.SelectedItem = item.sales_team;
                        salesteam_picker.SelectedIndex = 0;


                        salesperson_picker.ItemsSource = App.salespersons.Select(x => x.Value).ToList();
                        salesperson_picker.SelectedItem = item.sales_person;
                        salesperson_picker.SelectedIndex = 0;

                        cuspicker1.ItemsSource = App.cusList.Select(x => x.name).ToList();
                        cuspicker1.SelectedItem = item.customer;

   //newly added starts here

                        if (item.branch_id != "")
                        {
                            branch_picker.ItemsSource = App.branchList.Select(x => x.name).ToList();
                            branch_picker.SelectedItem = item.branch_id;
                        }

                        else 
                        {
                            branch_picker.ItemsSource = App.branchList.Select(x => x.name).ToList();
                            branch_picker.SelectedIndex = -1;
                        }


                        if (item.project_id != "")
                        {
                            analytic_picker.ItemsSource = App.analayticList.Select(x => x.name).ToList();
                            analytic_picker.SelectedItem = item.project_id;
                        }

                        else
                        {
                            analytic_picker.ItemsSource = App.analayticList.Select(x => x.name).ToList();
                            analytic_picker.SelectedIndex = -1;
                        }

                        if (item.warehouse_id == "")
                        {
                            warehouse_picker.ItemsSource = App.warehousList.Select(x => x.name).ToList();
                            warehouse_picker.SelectedIndex = -1;

                        }

                        else
                        {
                            warehouse_picker.ItemsSource = App.warehousList.Select(x => x.name).ToList();
                            warehouse_picker.SelectedItem = item.warehouse_id;
                        }


                        if (item.picking_policy == "")
                        {
                            shipping_picker.Items.Add("Deliver each product when available");
                            shipping_picker.Items.Add("Deliver all products at once");
                            shipping_picker.SelectedIndex = -1;
                        }

                        else
                        {
                            shipping_picker.Items.Add("Deliver each product when available");
                            shipping_picker.Items.Add("Deliver all products at once");
                            shipping_picker.SelectedItem = item.picking_policy;
                        }
                          
                        cr_entry.Text = item.client_order_ref;

     //newly added ends here





                        var cusid = App.cusList.FirstOrDefault(x => x.name == cuspicker1.SelectedItem.ToString()).id;

                        JObject con_dict = App.cus_address.FirstOrDefault(x => x.Key == cusid.ToString()).Value;

                        cus_selectdict = con_dict.ToObject<Dictionary<int, string>>();

                        invaddr_Picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
                        invaddr_Picker.SelectedItem = item.invoice_address;
                        // invaddr_Picker.SelectedIndex = 0;

                        deladdr_Picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
                        deladdr_Picker.SelectedItem = item.delivery_address;
                        // deladdr_Picker.SelectedIndex = 0;

                        contperson_Picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
                        contperson_Picker.SelectedIndex = -1;


                        pricelist_Picker.ItemsSource = App.product_PriceList.Select(x => x.name).ToList();
                        pricelist_Picker.SelectedItem = item.pricelist;

                        ponumref_entry.Text = item.po_number_reference;
                        franco_entry.Text = item.franco;


                        taxterm_Picker.Items.Add("Free tax");
                        taxterm_Picker.Items.Add("Pay via PT. LABORINDO SARANA");
                        taxterm_Picker.Items.Add("Pay via Customer");
                        taxterm_Picker.SelectedIndex = 0;

                        delmethod_Picker.ItemsSource = App.all_delivery_method.Select(x => x.name).ToList();
                        delmethod_Picker.SelectedItem = item.carrier;

                        filename_lbl.Text = item.file_name;
                        pofile_noedit.IsVisible = true;

                        specialnotes_edit.IsVisible = true;
                        specialnotes_noedit.IsVisible = false;
                        comments.Text = item.special_notes;

                    }


                    catch
                    {

                        await DisplayAlert("Alert-1", "Please Try Again", "Ok");
                    }


                    try
                    {

                        DateTime enteredDate = DateTime.Parse(item.DateOrder);
                        //  cd_Picker.Date = enteredDate;
                    }

                    catch
                    {

                        // await  DisplayAlert("Alert-2", "Please Try Again","Ok");

                        String enteredDate = DateTime.Now.ToString("yyyy-MM-dd");

                        DateTime dt = DateTime.Parse(enteredDate);

                        //  cd_Picker.Date = dt;

                        //  cd_Picker.Date= cd_Picker.Date;
                    }

                    try
                    {

                        ptpicker.ItemsSource = App.paytermList.Select(x => x.name).ToList();
                        ptpicker.SelectedItem = item.payment_term;


                        comgroup_picker.ItemsSource = App.commisiongroupList.Select(x => x.name).ToList();
                        comgroup_picker.SelectedItem = item.commission_group;

                    }

                    catch
                    {

                        await DisplayAlert("Alert-3", "Please Try Again", "Ok");
                    }



                    orderLineGrid_ol.IsVisible = false;
                    discount_grid_ol.IsVisible = false;
                    taxlistviewGrid_ol.IsVisible = false;

                    addbtn_orderline.IsVisible = true;

                    if (tap2_clicked == true)
                    {
                        addbtn_orderline.IsVisible = false;
                    }

                }

            };
            sq_editbtn.GestureRecognizers.Add(sq_editRecognizer);


            var addbtn_orderlineRecognizer = new TapGestureRecognizer();
            addbtn_orderlineRecognizer.Tapped += (s, e) =>
            {
                orderLineGrid_ol.IsVisible = true;
                discount_grid_ol.IsVisible = true;
                taxlistviewGrid_ol.IsVisible = true;
                addbtn_orderline.IsVisible = false;
                savebtn_layout.IsVisible = true;
                updatebtn.IsVisible = false;

                searchprod_ol.Text = "";
                orderline_des_ol.Text = "";
                up_ol.Text = "";
                oqty_ol.Text = "";
                dis1_ol.Text = "";
                multidis_ol.Text = "";

                taxListView_ol.ItemsSource = null;
                taxStackLayout_ol.IsVisible = false;
              //  taxlistviewGrid_ol.IsVisible = false;

                add_new_orderline = true;

                taxlistviewGrid_ol.BackgroundColor = Color.FromHex("#F0EEEF");
                taxStackLayout_ol.BackgroundColor = Color.FromHex("#F0EEEF");
                taxListView_ol.BackgroundColor = Color.FromHex("#F0EEEF");

                taxlistviewGrid_ol.Padding = new Thickness(15, 0, 0, 0);
                taxStackLayout_ol.Padding = new Thickness(15, 0, 0, 0);

            };
            Add_OrderLineBtn.GestureRecognizers.Add(addbtn_orderlineRecognizer);


            var Addtax_lineImgRecognizer = new TapGestureRecognizer();
            Addtax_lineImgRecognizer.Tapped += (s, e) => {

                // addtaxGrid.IsVisible = true;
                Addtax_line.IsVisible = false;

                Navigation.PushPopupAsync(new TaxSelectionPage());

            };
            Addtax_line.GestureRecognizers.Add(Addtax_lineImgRecognizer);

            var Addtax_lineImgRecognizer_ol = new TapGestureRecognizer();
            Addtax_lineImgRecognizer_ol.Tapped += (s, e) => {

                // addtaxGrid.IsVisible = true;
                Addtax_line.IsVisible = false;

                Navigation.PushPopupAsync(new TaxSelectionPage("tax_new"));

            };
            Addtax_line_ol.GestureRecognizers.Add(Addtax_lineImgRecognizer_ol);

        }

        //public SalesQuotationsListviewDetail(SalesQuotationDB item)
        //{
        //    InitializeComponent();
        //    NavigationPage.SetHasNavigationBar(this, false);

        //    //    App.orderLineList = item.order_line;


        //    dbobj = item;

        //    Cus.Text = item.customer;
        //    // CD.Text = item.DateOrder;
        //  //  CD.Text = item.date_Order;
        //    PT.Text = item.payment_term.ToString();
        //    SP.Text = item.sales_person;
        //    ST.Text = item.sales_team;
        //    CR.Text = item.customer_reference;
        //    FP.Text = item.fiscal_position;
        //    saleoder_id = item.id;

        //    List<OrderLine> or_linelistdb = new List<OrderLine>();



        //            var json_orderline = JsonConvert.SerializeObject(item.order_line);

        //            String convertstring = json_orderline.ToString();

        //            //  "\"[{\\\"customer_lead\\\":\\\"0\\\",\\\"price_unit\\\":\\\"10000\\\",\\\"product_uom_qty\\\":\\\"10\\\",\\\"price_subtotal\\\":\\\"100000\\\",\\\"taxes\\\":[],\\\"product_name\\\":\\\"Floordeck 1000x060 MM\\\"},{\\\"customer_lead\\\":\\\"0\\\",\\\"price_unit\\\":\\\"1\\\",\\\"product_uom_qty\\\":\\\"1\\\",\\\"price_subtotal\\\":\\\"1\\\",\\\"taxes\\\":[\\\"Sales Tax N/A SRCA-S\\\"],\\\"product_name\\\":\\\"Floordeck 1000x060 MM\\\"}]\""

        //            String finstring = convertstring.Replace("\\", "");

        //            finstring = finstring.Substring(1);

        //            finstring = finstring.Remove(finstring.Length - 1);

        //            JArray stringres = JsonConvert.DeserializeObject<JArray>(finstring);

        //            //  or_linelistdb = JsonConvert.DeserializeObject<List<OrderLine>>(finstring);


        //            //  OrderLine stringres = JsonConvert.DeserializeObject<OrderLine>(json_orderline)

        //            int cus_lead = 0;
        //            string prod_name = "";


        //        foreach (JObject obj in stringres)
        //        {
        //            OrderLine or_line = new OrderLine();
        //           object[] tax_id = new object[2];

        //            or_line.product_name = obj["product_name"].ToString();
        //            or_line.product_uom_qty = obj["product_uom_qty"].ToString();
        //            or_line.price_subtotal = obj["price_subtotal"].ToString();
        //            or_line.taxes = tax_id;

        //        try
        //        {
        //            or_line.tax_names = obj["tax_names"].ToString();
        //        }
        //        catch
        //        {
        //            or_line.tax_names = "";
        //        }

        //        try
        //        {
        //            or_line.discount = obj["discount"].ToString();
        //        }

        //        catch
        //        {
        //            or_line.discount = "";
        //        }


        //            or_linelistdb.Add(or_line);
        //        }


        //    orderListview.ItemsSource = or_linelistdb;


        //    var sq_editRecognizer = new TapGestureRecognizer();
        //    sq_editRecognizer.Tapped += async (s, e) =>
        //    {
        //        await DisplayAlert("Alert", "Need Internet Connection", "Ok");
        //    };
        //    sq_editbtn.GestureRecognizers.Add(sq_editRecognizer);

        //    //orderListview.ItemsSource = item.order_line;


        //}



        protected override void OnAppearing()
        {
            base.OnAppearing();


            MessagingCenter.Subscribe<string, int>("MyApp", "PickerMsg", (sender, arg) =>
            {
                // HideLbl.Text = "New Quotation Creation";

                if (App.productList.Count != 0)
                {
                    var productlis = from pro in App.productList
                                     where pro.Id == arg
                                     select pro;

                    foreach (var prodresults in productlis)
                    {
                        searchprod.Text = prodresults.Name;
                        orderline_des.Text = prodresults.Name;
                        up.Text = prodresults.list_price;

                        searchprod_ol.Text = prodresults.Name;
                        orderline_des_ol.Text = prodresults.Name;
                        up_ol.Text = prodresults.list_price;
                    }
                }

                else
                {
                    var productlis = from pro in App.ProductListDb
                                     where pro.Id == arg
                                     select pro;

                    foreach (var prodresults in productlis)
                    {
                        searchprod.Text = prodresults.Name;
                        orderline_des.Text = prodresults.Name;
                        up.Text = prodresults.list_price;
                    }
                }




                int i = 0;

            });




            MessagingCenter.Subscribe<string, string>("MyApp", "taxPickerMsg", (sender, arg) =>
            {

                taxListView.ItemsSource = null;

                taxList_edit.Add(new taxes(arg));

                taxList_edit = taxList_edit.GroupBy(i => i.Name).Select(g => g.First()).ToList();
                // taxpicker.IsVisible = false;
                taxStackLayout.IsVisible = true;
                taxListView.ItemsSource = taxList_edit;
                taxListView.RowHeight = 30;
                taxListView.HeightRequest = 30 * taxList_edit.Count;

                taxStackLayout.BackgroundColor = Color.FromHex("#363E4B");
                taxListView.BackgroundColor = Color.FromHex("#363E4B");
                taxStackLayout.CornerRadius = 20;

               taxStackLayout.Padding = new Thickness(5);


                // taxpickstringList.Add(taxpicker.SelectedItem.ToString());
                var taxesid =
               (
               from i in App.taxList
               where i.Name == arg

               select new
               {
                   i.Id,
               }
               ).ToList();

                foreach (var person in taxesid)
                {
                    int selecttaxid = person.Id;
                    taxidList.Add(selecttaxid);
                    taxidList = taxidList.GroupBy(i => i).Select(g => g.First()).ToList();
                }

                Addtax_line.IsVisible = true;

            });


            MessagingCenter.Subscribe<string, string>("MyApp", "taxnewPickerMsg", (sender, arg) =>
            {

                taxListView_ol.ItemsSource = null;

                taxList_edit.Add(new taxes(arg));

                taxList_edit = taxList_edit.GroupBy(i => i.Name).Select(g => g.First()).ToList();
                // taxpicker.IsVisible = false;
                taxStackLayout_ol.IsVisible = true;
                taxListView_ol.ItemsSource = taxList_edit;
                taxListView_ol.RowHeight = 35;
                taxListView_ol.HeightRequest = 35 * taxList_edit.Count;

                taxStackLayout_ol.BackgroundColor = Color.FromHex("#363E4B");
                taxListView_ol.BackgroundColor = Color.FromHex("#363E4B");
                taxStackLayout_ol.CornerRadius = 20;

                taxStackLayout_ol.Padding = new Thickness(5);

                // taxpickstringList.Add(taxpicker.SelectedItem.ToString());
                var taxesid =
               (
               from i in App.taxList
               where i.Name == arg

               select new
               {
                   i.Id,
               }
               ).ToList();

                foreach (var person in taxesid)
                {
                    int selecttaxid = person.Id;
                    taxidList.Add(selecttaxid);
                    taxidList = taxidList.GroupBy(i => i).Select(g => g.First()).ToList();
                }

                Addtax_line_ol.IsVisible = true;

            });

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




        private void prodpicker_Focused(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new PickerSelectionPage());

            searchprod.Unfocus();


        }

        private void ViewCelltax_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#102b1e");
            //  m_title.TextColor = Color.Red;
        }


        public void ListviewcloseClicked(object sender, EventArgs e1)
        {
            try
            {
                var args = (TappedEventArgs)e1;
                taxes t2 = args.Parameter as taxes;

                //  var itemToRemove = App.taxListRemove.Single(r => r.Name == t2.Name);

                var itemToRemove = taxList_edit.Single(r => r.Name == t2.Name);

                taxList_edit.Remove(itemToRemove);


                taxListView.ItemsSource = null;


                //  taxStackLayout.Padding = 0;

                taxListView.ItemsSource = taxList_edit;
                taxListView.RowHeight = 35;
                taxListView.HeightRequest = 35 * taxList_edit.Count;



                if (taxList_edit.Count == 0)
                {
                    taxStackLayout.BackgroundColor = Color.White;
                    taxListView.BackgroundColor = Color.White;
                    taxStackLayout.CornerRadius = 0;
                    taxStackLayout.Padding = 0;
                }



            }
            catch (Exception e)
            {


                System.Diagnostics.Debug.WriteLine(e.Message);

            }

        }




        public void Listview_ol_closeClicked(object sender, EventArgs e1)
        {
            try
            {
                var args = (TappedEventArgs)e1;
                taxes t2 = args.Parameter as taxes;

                //  var itemToRemove = App.taxListRemove.Single(r => r.Name == t2.Name);

                var itemToRemove = taxList_edit.Single(r => r.Name == t2.Name);

                taxList_edit.Remove(itemToRemove);


                taxListView_ol.ItemsSource = null;


                //  taxStackLayout.Padding = 0;

                taxListView_ol.ItemsSource = taxList_edit;
                taxListView_ol.RowHeight = 35;
                taxListView_ol.HeightRequest = 35 * taxList_edit.Count;



                if (taxList_edit.Count == 0)
                {
                    taxStackLayout_ol.BackgroundColor = Color.White;
                    taxListView_ol.BackgroundColor = Color.White;
                    taxStackLayout_ol.CornerRadius = 0;
                    taxStackLayout_ol.Padding = 0;
                }



            }
            catch (Exception e)
            {


                System.Diagnostics.Debug.WriteLine(e.Message);

            }

        }


        private async void attachStackClicked(object sender, EventArgs e)
        {            
            await  Navigation.PushPopupAsync(new AttachmentPopupPage(attachres,saleoder_id));                      
        }

        private void OnMenuItemTapped(object sender, ItemTappedEventArgs ea)
        {
            orderLineGrid_ol.IsVisible = false;
            discount_grid_ol.IsVisible = false;
            taxlistviewGrid_ol.IsVisible = false;
            addbtn_orderline.IsVisible = false;

            taxList_edit.Clear();


            if (editbtn_clicked == true)
            {
                updatebtn.IsVisible = false;

                OrderLine masterItemObj = (OrderLine)ea.Item;

                listview_editlayout.IsVisible = true;
                discount_grid.IsVisible = true;
                taxlistviewGrid.IsVisible = true;
                OrderLineList1.IsVisible = false;

                searchprod.Text = masterItemObj.product_name;
                orderline_des.Text = masterItemObj.product_name;
                oqty.Text = masterItemObj.product_uom_qty;
                up.Text = masterItemObj.price_unit;
                dis1.Text = masterItemObj.discount;
                multidis.Text = masterItemObj.multi_discount;
                orderline_id = masterItemObj.id;


                taxlistviewGrid.IsVisible = true;

                taxes taxname = new taxes("");


                for (int i = 0; i < masterItemObj.taxes_id.Count(); i++)
                {

                    string taname = masterItemObj.taxes_id[i].ToString();
                    int taid = Int32.Parse(taname);

                    //  var name = App.taxList.Select(x => x.Id == 4 );

                    var tax_list = from tx in App.taxList
                                   where tx.Id == taid
                                   select tx;



                    string name = "";
                    foreach (var tax in tax_list)
                    {
                        name = tax.Name;
                    }

                    //  var taxesfinname = final_listview[i].taxes;

                    if (masterItemObj.taxes_id[i] != null)
                    {

                        taxList_edit.Add(new taxes(name));
                    }

                }



                taxListView.ItemsSource = taxList_edit;
                taxListView.RowHeight = 35;
                taxListView.HeightRequest = 35 * taxList_edit.Count;
                taxStackLayout.BackgroundColor = Color.FromHex("#363E4B");
                taxListView.BackgroundColor = Color.FromHex("#363E4B");

                if (taxList_edit.Count == 0)
                {
                    taxStackLayout.BackgroundColor = Color.FromHex("#F0EEEF");
                    taxListView.BackgroundColor = Color.FromHex("#F0EEEF");
                    taxlistviewGrid.Padding = new Thickness(15, 0, 0, 0);
                    taxStackLayout.Padding = new Thickness(0, 0, 0, 0);
                }

            }

            savebtn_layout.IsVisible = true;
        }


        private async void send_byemail_ClickedAsync(object sender, EventArgs ea)
        {
            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);


            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(obj.email);

            //  

            if (match.Success)
            {
                string emailres = Controller.InstanceCreation().SaleOrderConfirm("sale.order", "send_by_mail_from_app", saleoder_id);

                if (emailres == "true")
                {
                    await DisplayAlert("Alert", "Email Sent Successfully", "Ok");
                }

                else
                {
                    await DisplayAlert("Alert", "Please Try again", "Ok");
                }
            }

            else
            {
                await DisplayAlert("Alert", "Customer Email Invalid", "Ok");

            }

            await PopupNavigation.PopAsync();
        }


        private void ConfirmSOClicked(object sender, EventArgs ea)
        {
            String res = "";
            try
            {
                res = Controller.InstanceCreation().SaleOrderConfirm("sale.order", "confirm_sale_quotation", saleoder_id);
            }

            catch
            {
                int j = 0;
            }
            //  String res = "Stock not available for this products : \n Down payment";

            if (App.NetAvailable == true)
            {
                //  var data = await DisplayAlert("Logout Alert", "Are you sure you want to log out?", "OK", "Cancel");
                if (res.Contains("\n"))
                {
                    string[] stringSeparators = new string[] { "\n" };
                    string[] results = res.Split(stringSeparators, StringSplitOptions.None);

                    String first = results[0];
                    String second = results[1];

                    DisplayAlert(first, second, "Ok");
                }

                else
                {

                    if (res == "Success")
                    {
                        DisplayAlert("Alert", "Sale Order Confirmed Successfully", "Ok");
                        Application.Current.MainPage = new MasterPage(new CrmTabbedPage());
                    }
                    else
                    {
                        DisplayAlert("Alert", res, "Ok");

                        //  orderListview.ItemsSource = null;
                        // Loadingalertcall();
                    }
                }
            }
            else
            {
                DisplayAlert("Alert", "Need Internet Connection", "Ok");
            }

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

            tap2_clicked = false;

            if (editbtn_clicked == true)
            {
                addbtn_orderline.IsVisible = true;
                total_cal.IsVisible = false;
            }
            //  savebtn_layout.IsVisible = true;

            tab1.TextColor = Color.White;
            tab2.TextColor = Color.Black;

            Add_OrderLineBtn.IsVisible = true;
            Addtax_line_ol.IsVisible = true;
        }

        private void Tab2Clicked(object sender, EventArgs ea)
        {
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

            listview_editlayout.IsVisible = false;
            discount_grid.IsVisible = false;
            taxlistviewGrid.IsVisible = false;
            addbtn_orderline.IsVisible = false;

            tab1.TextColor = Color.Black;
            tab2.TextColor = Color.White;

            tap2_clicked = true;

            orderLineGrid_ol.IsVisible = false;
            Addtax_line_ol.IsVisible = false;
            //    airconImg1.IsVisible = false;

            discount_grid_ol.IsVisible = false;
            taxlistviewGrid_ol.IsVisible = false;



            //  savebtn_layout.IsVisible = false;

        }

    

     // New Save method

        private async void save_clickedAsync(object sender, EventArgs ea)
        {
            // final_listview.Clear();
            //   addbtn_orderline.IsVisible = true;
            updatebtn.IsVisible = true;
            sq_editbtn.IsVisible = false;
            orderLineGrid_ol.IsVisible = false;
            discount_grid_ol.IsVisible = false;
            taxlistviewGrid_ol.IsVisible = false;
            addbtn_orderline.IsVisible = true;

            savebtn_layout.IsVisible = false;

            String taxname_full = "";

            OrderLine orderLine = new OrderLine();

            if (add_new_orderline == true)
            {

                OrderLine orderLinenew = new OrderLine();

                var productlis = from pro in App.productList
                                 where pro.Name == searchprod_ol.Text
                                 select pro;

                int prod_id = 0;

                foreach (var pro in productlis)
                {
                    prod_id = pro.Id;
                }

                //  orderLine.product_id = prod_id;

                int newodid = 0;

                foreach (var ids in obj.order_line)
                {
                    newodid = ids.id + 11;
                    //  newodid = newodid;
                }

                orderLinenew.id = newodid;
                orderLinenew.product_name = searchprod_ol.Text;
                orderLinenew.product_id = prod_id;
                orderLinenew.product_uom_qty = oqty_ol.Text;
                orderLinenew.price_unit = up_ol.Text;
                orderLinenew.discount = dis1_ol.Text;
                orderLinenew.multi_discount = multidis_ol.Text;


                //  orderLinenew.taxes = tax_id;
                //try
                //{
                //    orderLinenew.price_subtotal = (Convert.ToInt32(oqty_ol.Text) * Convert.ToInt32(up_ol.Text)).ToString();

                //    Double tot = Double.Parse(orderLinenew.price_subtotal) * (Double.Parse(dis1_ol.Text) / 100);

                //    orderLinenew.price_subtotal = (Double.Parse(orderLinenew.price_subtotal) - tot).ToString();
                //}

                //catch
                //{
                //    orderLinenew.price_subtotal = "";
                //}

              

                //Object[] tax_id = new object[taxList_edit.Count()];

                //List<int> taxids = new List<int>();

                //for (int i = 0; i < taxList_edit.Count(); i++)
                //{
                //    var tax_list = from tx in App.taxList
                //                   where tx.Name == taxList_edit[i].Name.ToString()
                //                   select tx;

                //    int tax_list_id = 0;
                //    foreach (var tax in tax_list)
                //    {
                //        tax_list_id = tax.Id;
                //        taxids.Add(tax.Id);
                //    }

                //    //var tax_list_id = from x in App.taxList
                //    //         where x.Name == newobj.taxes[i].ToString()
                //    //select x.Id;

                //    tax_id[i] = tax_list_id;

                //    orderLinenew.tax_id = tax_id;

                //    taxname_full = taxname_full + "  " + taxList_edit[i].Name.ToString();

                //}

                var newtax_id = new object[taxList_edit.Count];
                var newtaxnameids = new List<int>();
                for (int i = 0; i < taxList_edit.Count(); i++)
                {

                    int taxname_id = App.taxList.FirstOrDefault(x => x.Name == taxList_edit[i].Name.ToString()).Id;

                    newtax_id[i] = taxname_id;
                    newtaxnameids.Add(taxname_id);

                    taxname_full = taxname_full + "  " + taxList_edit[i].Name.ToString();

                }

                orderLinenew.tax_id = newtax_id;
                orderLinenew.taxes_id = newtax_id;

                orderLinenew.tax_names = taxname_full;

                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);

                if (dis1.Text == "")
                {
                    dis1.Text = "0";
                }
                float subtotal = Controller.InstanceCreation().getsubtotal("sale.order", "get_sub_total", float.Parse(up_ol.Text), float.Parse(oqty_ol.Text), newtaxnameids, float.Parse(dis1_ol.Text));

                int subtotal1 = (int)subtotal;

                orderLinenew.price_subtotal = subtotal1.ToString();

                await PopupNavigation.PopAsync();

                obj.order_line.Add(orderLinenew);

            }



            foreach (var newobj in obj.order_line)
            {




                if (newobj.id == orderline_id && newobj.id != 0)
                {
                    // orderLine.id = newobj.id;
                    orderLine.product_name = searchprod.Text;


                    var productlis = from pro in App.productList
                                     where pro.Name == newobj.product_name
                                     select pro;

                    int prod_id = 0;

                    foreach (var pro in productlis)
                    {
                        prod_id = pro.Id;
                    }

                    orderLine.product_id = prod_id;
                    orderLine.product_uom_qty = oqty.Text;
                    orderLine.price_unit = up.Text;
                    orderLine.taxes = newobj.taxes;
                    orderLine.discount = dis1.Text;
                    orderLine.multi_discount = multidis.Text;
                    orderLine.id = newobj.id;
                    orderline_id = newobj.id;


                    var taxnew_id = new object[taxList_edit.Count];
                    var taxnameids = new List<int>();
                    for (int i = 0; i < taxList_edit.Count(); i++)
                    {
                      
                        int taxname_id = App.taxList.FirstOrDefault(x => x.Name == taxList_edit[i].Name.ToString()).Id;
                      
                        taxnew_id[i] = taxname_id;
                        taxnameids.Add(taxname_id);

                        taxname_full = taxname_full + "  " + taxList_edit[i].Name.ToString();

                    }

                    orderLine.tax_id = taxnew_id;
                    orderLine.taxes_id = taxnew_id;
                    orderLine.tax_names = taxname_full;

                    //  orderLine.tax_namecut = newobj.tax_namecut;
                    //try
                    //{

                    //    orderLine.price_subtotal = (Convert.ToInt32(oqty.Text) * Convert.ToInt32(up.Text)).ToString();
                    //    orderLine.customer_lead = newobj.customer_lead;

                    //    Double tot = Double.Parse(orderLine.price_subtotal) * (Double.Parse(dis1.Text) / 100);

                    //    orderLine.price_subtotal = (Double.Parse(orderLine.price_subtotal) - tot).ToString();
                    //}

                    //catch
                    //{

                    //    DisplayAlert("Alert", "Try again", "Ok");
                    //}

                    //  orderLine.price_subtotal = tot.ToString();

                    var currentpage = new LoadingAlert();
                    await PopupNavigation.PushAsync(currentpage);

                    if (dis1.Text == "")
                    {
                        dis1.Text = "0";
                    }
                    float subtotal = Controller.InstanceCreation().getsubtotal("sale.order", "get_sub_total", float.Parse(up.Text), float.Parse(oqty.Text), taxnameids, float.Parse(dis1.Text));

                    int subtotal1 = (int)subtotal;

                    orderLine.price_subtotal = subtotal1.ToString();

                    await PopupNavigation.PopAsync();


                }

                //  orderLine.tax_id = tax_id;

            }

            if (add_new_orderline == false)
            {

                int index = final_listview.FindIndex(m => m.id == orderline_id);
                if (index >= 0)
                    final_listview[index] = orderLine;

            }
            orderLinelist.Clear();

            foreach (var newobj in final_listview)
            {
                OrderLinesListForUpdate orderLineupdate = new OrderLinesListForUpdate();
                // orderLineupdate.id = newobj.id;

                var productlis = from pro in App.productList
                                 where pro.Name == newobj.product_name
                                 select pro;

                int prod_id = 0;
                foreach (var pro in productlis)
                {
                    prod_id = pro.Id;
                }


                //  orderLineupdate.product_id = newobj.product_id;
                orderLineupdate.product_id = prod_id;
                orderLineupdate.product = newobj.product_name;
                orderLineupdate.ordered_qty = newobj.product_uom_qty;
                orderLineupdate.unit_price = newobj.price_unit;
                orderLineupdate.tax_names = newobj.tax_names;
                orderLineupdate.tax_id = newobj.taxes_id;
                orderLineupdate.discount = newobj.discount;
                orderLineupdate.multi_discount = newobj.multi_discount;
                //  orderLineupdate.tax_id = newobj.taxes;



                orderLinelist.Add(orderLineupdate);
            }

            //   orderListview.ClearValue();

            OrderLineList1.IsVisible = true;
            orderListview.ItemsSource = final_listviewnew;
            final_listviewnew.Clear();
            orderListview.ItemsSource = final_listview;

            orderListview.HeightRequest = final_listview.Count * 55;

            listview_editlayout.IsVisible = false;
            discount_grid.IsVisible = false;
            taxlistviewGrid.IsVisible = false;

            Cus.Text = cuspicker1.SelectedItem.ToString();
            // CD.Text = item.DateOrder;
            //  CD.Text = cd_Picker.Date.ToString();
            PT.Text = ptpicker.SelectedItem.ToString();
            CG.Text = comgroup_picker.SelectedItem.ToString();
            SP.Text = salesperson_picker.SelectedItem.ToString();

            if (salesteam_picker.SelectedItem != null)
            {
                ST.Text = salesteam_picker.SelectedItem.ToString();
            }
            CR.Text = cr_entry.Text;
            FP.Text = fp_entry.Text;

            add_new_orderline = false;


            cus_edit.IsVisible = true;
            //  con_dateedit.IsVisible = true;
            ptpicker_edit.IsVisible = true;
            commissionpicker_edit.IsVisible = true;
            sales_teamedit.IsVisible = true;
            sales_personsedit.IsVisible = true;

            cr_edit.IsVisible = true;
            fp_edit.IsVisible = true;
            is_edit.IsVisible = true;


            branch_edit.IsVisible = true;
            analytic_edit.IsVisible = true;
            wh_edit.IsVisible = true;
            sp_edit.IsVisible = true;

            cus_noedit.IsVisible = false;
            //  con_datenoedit.IsVisible = false;
            ptpicker_noedit.IsVisible = false;
            commissionpicker_noedit.IsVisible = false;
            sales_teamnoedit.IsVisible = false;
            sales_personsnoedit.IsVisible = false;

            cr_noedit.IsVisible = false;
            fp_noedit.IsVisible = false;
            is_noedit.IsVisible = false;

            branch_noedit.IsVisible = false;
            analytic_noedit.IsVisible = false;
            wh_noedit.IsVisible = false;
            sp_noedit.IsVisible = false;

            OtherInfoStack1.IsVisible = false;
            OtherInfoStack2.IsVisible = false;
            OtherInfoStack3.IsVisible = false;

            // }
        }



        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#FFFFFF");
        }

        private void cancel_clicked(object sender, EventArgs ea)
        {

            savebtn_layout.IsVisible = false;
            orderLineGrid_ol.IsVisible = false;
            discount_grid_ol.IsVisible = false;
            taxlistviewGrid_ol.IsVisible = false;


            OtherInfoStack1.IsVisible = false;

            OrderLineList1.IsVisible = true;
            orderListview.ItemsSource = final_listviewnew;
            final_listviewnew.Clear();
            orderListview.ItemsSource = final_listview;


            // editbtn_clicked = false;

            cus_edit.IsVisible = false;
            // con_dateedit.IsVisible = false;
            ptpicker_edit.IsVisible = false;
            sales_teamedit.IsVisible = false;
            sales_personsedit.IsVisible = false;

            cr_edit.IsVisible = false;
            fp_edit.IsVisible = false;
            is_edit.IsVisible = false;

            branch_edit.IsVisible = false;
            analytic_edit.IsVisible = false;
            wh_edit.IsVisible = false;
            sp_edit.IsVisible = false;

            cus_noedit.IsVisible = true;
            //  con_datenoedit.IsVisible = true;
            ptpicker_noedit.IsVisible = true;
            sales_teamnoedit.IsVisible = true;
            sales_personsnoedit.IsVisible = true;

            branch_noedit.IsVisible = true;
            analytic_noedit.IsVisible = true;
            wh_noedit.IsVisible = true;
            sp_noedit.IsVisible = true;

            cr_noedit.IsVisible = true;
            fp_noedit.IsVisible = true;
            is_noedit.IsVisible = true;

            sq_editbtn.IsVisible = true;

            listview_editlayout.IsVisible = false;
            discount_grid.IsVisible = false;
            taxlistviewGrid.IsVisible = false;

            updatebtn.IsVisible = true;
            addbtn_orderline.IsVisible = false;

            OtherInfoStack2.IsVisible = false;
            OtherInfoStack3.IsVisible = false;
            commissionpicker_edit.IsVisible = false;
            commissionpicker_noedit.IsVisible = true;
        }

        void multidiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool entry = false;

            //  bool res = Regex.IsMatch(dis1.Text, @"^-?\d+$");

            //    bool res1 = Regex.IsMatch(multidis.Text, @"^\d+$");

            bool res = Regex.IsMatch(multidis.Text, @"\d");


            if (multidis.Text == "+")
            {
                entry = true;
            }

            else if (multidis.Text == "+" || multidis.Text == "." || res == true)
            {
                entry = true;
            }

            else
            {
                entry = false;
            }

            if (entry && res)
            {

                var foos = multidis.Text;
                var fooArray = foos.Split('+');

                double discount_per = 100;

                //string[] stringArray = foos.Split(',').ToArray();

                //foos = String.Join(",", fooArray).toa;


                for (int i = 0; i < fooArray.Length; i++)
                {
                    if (fooArray[i] != "")
                    {
                        double val = Convert.ToDouble(fooArray[i]);
                        discount_per = discount_per - ((val / 100) * discount_per);
                    }
                }


                double overper = 100 - discount_per;

                overper = Convert.ToDouble(overper.ToString("n2"));

                overper_string = overper.ToString();

                dis1.Text = overper.ToString();

            }


            else
            {
                //  DisplayAlert("Alert", "Please give valid input", "Ok");

            }

            //if(e.NewTextValue != "+" && !="")
            //{

            //}

            //else

        }


        void multidiscount_ol__TextChanged(object sender, TextChangedEventArgs e)
        {
            bool entry = false;

            //  bool res = Regex.IsMatch(dis1.Text, @"^-?\d+$");

            //    bool res1 = Regex.IsMatch(multidis.Text, @"^\d+$");

            bool res = Regex.IsMatch(multidis_ol.Text, @"\d");


            if (multidis_ol.Text == "+")
            {
                entry = true;
            }

            else if (multidis_ol.Text == "+" || multidis_ol.Text == "." || res == true)
            {
                entry = true;
            }

            else
            {
                entry = false;
            }

            if (entry && res)
            {

                var foos = multidis_ol.Text;
                var fooArray = foos.Split('+');

                double discount_per = 100;

                //string[] stringArray = foos.Split(',').ToArray();

                //foos = String.Join(",", fooArray).toa;


                for (int i = 0; i < fooArray.Length; i++)
                {
                    if (fooArray[i] != "")
                    {
                        double val = Convert.ToDouble(fooArray[i]);
                        discount_per = discount_per - ((val / 100) * discount_per);
                    }
                }


                double overper = 100 - discount_per;

                overper = Convert.ToDouble(overper.ToString("n2"));

                overper_string = overper.ToString();

                dis1_ol.Text = overper.ToString();

            }


            else
            {
                //  DisplayAlert("Alert", "Please give valid input", "Ok");

            }

            //if(e.NewTextValue != "+" && !="")
            //{

            //}

            //else

        }

        // ******************************* Update Clicks*****************************

        private async void update_clickedAsync(object sender, EventArgs ea)
        {
            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
            int selectpaytermid = 0;
            int selectcomgroupid = 0;

          //  List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

            if (App.NetAvailable == true)
            {
                try
                {

                    if (ptpicker.SelectedItem == null)
                    {
                        vals["payment_terms"] = false;
                    }

                    else
                    {
                        var paytermid =
                        (
                                from i in App.paytermList
                                where i.name == ptpicker.SelectedItem.ToString()
                                select new
                                {
                                    i.id,
                                }
                   ).ToList();

                        foreach (var person in paytermid)
                        {
                            selectpaytermid = person.id;
                        }

                        vals["payment_terms"] = selectpaytermid;
                    }

                    if (comgroup_picker.SelectedItem == null)
                    {
                        vals["commission_group"] = false;
                    }

                    else
                    {
                        var comgroupid =
                        (
                                from i in App.commisiongroupList
                                where i.name == comgroup_picker.SelectedItem.ToString()
                                select new
                                {
                                    i.id,
                                }
                   ).ToList();

                        foreach (var comgroup in comgroupid)
                        {
                            selectcomgroupid = comgroup.id;
                        }

                        vals["commission_group"] = selectcomgroupid;
                    }

                }

                catch (Exception ex)
                {
                    await DisplayAlert("Alert-1", "Test No 1.", "Ok");
                }

                try
                {



                    String order_date_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", od_Picker.Date);

                    String exp_date_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", expir_Picker.Date);

                    String po_date_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", podate_Picker.Date);

                    String deldead_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", delidead_Picker.Date);


                    if (taxterm_Picker.SelectedIndex == 0)
                    {
                        vals["tax_term"] = "free";
                    }

                    else if (taxterm_Picker.SelectedIndex == 1)

                    {
                        vals["tax_term"] = "company";
                    }

                    else
                    {
                        vals["tax_term"] = "customer";
                    }


                    vals["user_id"] = App.userid;

                    var cusid = App.cusList.FirstOrDefault(x => x.name == cuspicker1.SelectedItem.ToString()).id;
                    vals["customer"] = cusid;

                    vals["delivery_deadline"] = deldead_string;


                    vals["order_date"] = order_date_string;

                    vals["expiration_date"] = exp_date_string;


                    try
                    {
                        string ids = App.product_PriceList.First(c => c.name == pricelist_Picker.SelectedItem.ToString()).id;

                        vals["pricelist_id"] = Int32.Parse(ids);
                    }

                    catch
                    {
                        vals["pricelist_id"] = false;
                    }

                    vals["po_number_reference"] = ponumref_entry.Text;


                    vals["po_date"] = po_date_string;

                    vals["po_file"] = file_uploadstring;
                    vals["file_name"] = file_uploadname;

                    vals["franco"] = franco_entry.Text;

                    vals["special_notes"] = comments.Text;


                    try
                    {
                        string ids = App.product_PriceList.First(c => c.name == pricelist_Picker.SelectedItem.ToString()).id;

                        vals["pricelist_id"] = Int32.Parse(ids);
                    }

                    catch
                    {
                        vals["pricelist_id"] = false;
                    }

                    try
                    {

                        var inv_id = cus_selectdict.FirstOrDefault(x => x.Value == invaddr_Picker.SelectedItem.ToString()).Key;
                        vals["partner_invoice_id"] = inv_id;

                        if (inv_id == 0)
                        {
                            vals["partner_invoice_id"] = cusid;
                        }
                    }

                    catch
                    {
                        vals["partner_invoice_id"] = false;
                    }

                    try
                    {
                        var delvaddr_id = cus_selectdict.FirstOrDefault(x => x.Value == deladdr_Picker.SelectedItem.ToString()).Key;
                        vals["partner_shipping_id"] = delvaddr_id;

                        if (delvaddr_id == 0)
                        {
                            vals["partner_shipping_id"] = cusid;
                        }

                    }

                    catch
                    {
                        vals["partner_shipping_id"] = false;
                    }


                    try
                    {
                        var conperson_id = cus_selectdict.FirstOrDefault(x => x.Value == contperson_Picker.SelectedItem.ToString()).Key;
                        vals["contact_person_id"] = conperson_id;
                    }

                    catch
                    {
                        vals["contact_person_id"] = false;
                    }

                    try

                    {

                        var delmethod_id = App.all_delivery_method.FirstOrDefault(x => x.name == delmethod_Picker.SelectedItem.ToString()).id;
                        vals["carrier_id"] = Int32.Parse(delmethod_id);
                    }

                    catch
                    {
                        vals["carrier_id"] = false;
                    }
                    // vals["order_lines"] = final_listview;
                    vals["order_lines"] = orderLinelist;

                    vals["state"] = "x_draft";

                    if (shipping_picker.SelectedIndex == 0)
                    {

                        vals["picking_policy"] = "direct";
                    }

                    else if (shipping_picker.SelectedIndex == 1)
                    {
                        vals["picking_policy"] = "one";
                    }

                    else
                    {
                        vals["picking_policy"] = false;
                    }


                    vals["client_order_ref"] = cr_entry.Text;
                    //  vals["discount"] = dis1.Text;
                    // vals["multi_discount"] = overper_string;


                    try
                    {
                        var warehouse_id = App.warehousList.FirstOrDefault(x => x.name == warehouse_picker.SelectedItem.ToString()).id;
                        vals["warehouse_id"] = warehouse_id;

                        if (warehouse_id == 0)
                        {
                            vals["warehouse_id"] = false;
                        }
                    }

                    catch
                    {
                        vals["warehouse_id"] = false;
                    }


                    try
                    {
                        var team_id = App.salesteam.FirstOrDefault(x => x.Value == salesteam_picker.SelectedItem.ToString()).Key;
                        vals["team_id"] = team_id;

                        if (team_id == 0)
                        {
                            vals["team_id"] = false;
                        }
                    }

                    catch
                    {
                        vals["team_id"] = false;
                    }

                    try
                    {
                        var analaytic_id = App.analayticList.FirstOrDefault(x => x.name == analytic_picker.SelectedItem.ToString()).id;
                        vals["project_id"] = analaytic_id;

                        if (analaytic_id == 0)
                        {
                            vals["project_id"] = false;
                        }
                    }

                    catch
                    {
                        vals["project_id"] = false;
                    }


                    try
                    {
                        var branch_id = App.branchList.FirstOrDefault(x => x.name == branch_picker.SelectedItem.ToString()).id;
                        vals["branch_id"] = branch_id;

                        if (branch_id == 0)
                        {
                            vals["branch_id"] = false;
                        }
                    }

                    catch
                    {
                        vals["branch_id"] = false;
                    }

                }

                catch
                {
                    await DisplayAlert("Alert-2", "Test No 2.", "Ok");
                }

                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);

                try
                {

                    bool updated = Controller.InstanceCreation().UpdateSaleOrder("sale.order", "update_sale_quotation", saleoder_id, vals);

                    if (updated == true)
                    {
                        App.draftquot_rpc = true;
                        App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab3"));
                        await Navigation.PopAllPopupAsync();
                    }

                    else
                    {
                        await DisplayAlert("Alert", "Please try again", "Ok");
                        await Navigation.PopAllPopupAsync();
                    }
                }

                catch
                {
                    await DisplayAlert("Alert-3", "Test No 3.", "Ok");
                    await Navigation.PopAllPopupAsync();
                }


            }

            else
            {
                await DisplayAlert("Alert", "Need Internet Connection", "Ok");
                await Navigation.PopAllPopupAsync();
                //  await Navigation.PopAllPopupAsync();
            }
        }

        private async void updatecancel_clickedAsync(object sender, EventArgs ea)
        {
            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);

            App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab3"));

            await Navigation.PopAllPopupAsync();
        }


        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    MessagingCenter.Subscribe<string, List<Attachments>>("MyApp", "attachUpdated", (sender, arg) =>
        //    {
        //        // List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

        //     //   List<Attachments> attch = new List<Attachments>();
        //        //  crmLeadListView.ItemsSource = App.crmList;

        //        attachres = arg;

        //        if (attachres.Count == 0)
        //        {
        //            attach_name.Text = "Attachment(s)";
        //        }

        //        else if (attachres.Count > 0)
        //        {
        //            attach_name.Text = attachres.Count + " " + "Attachment(s)";
        //        }

        //    //    attach_name.Text = attachres.Count + " " + "Attachment(s)";


        //    });
        //}

        private async void uploadClicked(object sender, EventArgs e)
        {

            FileData fileData = new FileData();
            FileData filedata = null;
            try
            {
                filedata = await CrossFilePicker.Current.PickFile();

                if (filedata.FileName.Contains(".pdf") || filedata.FileName.Contains(".doc") ||
                   filedata.FileName.Contains(".docx") || filedata.FileName.Contains(".txt"))
                {
                    if (!string.IsNullOrEmpty(filedata.FileName)) //Just the file name, it doesn't has the path
                    {
                        byte[] bydata = filedata.DataArray;
                        //  String UploadData = Convert.ToBase64String(bydata);
                        file_uploadstring = Convert.ToBase64String(bydata);
                        file_uploadname = filedata.FileName;
                        //  topimg = UploadData;
                        // var stream = new MemoryStream(bydata);
                        // user_img.Source = ImageSource.FromStream(() => stream);
                    }
                    file_frame.IsVisible = true;
                    file_grid.IsVisible = true;
                    pofile_noedit.IsVisible = false;
                    fileframname_lbl.Text = filedata.FileName;
                }

                else
                {
                    await DisplayAlert("Alert", "Please upload valid file name", "Ok");
                }
            }
            catch (Exception ex)
            {
                filedata = null;
                System.Diagnostics.Debug.WriteLine("Warning Exception :  " + ex.Message);
            }

        }

    }
}