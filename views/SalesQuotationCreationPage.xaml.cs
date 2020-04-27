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
using SalesApp.Persistance;
using SalesApp.wizard;
using Xamarin.Forms;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    public partial class SalesQuotationCreationPage : PopupPage
    {

        string taxname = "";
        string overper_string = "";
        String file_uploadstring = "";
        String file_uploadname = "";

        int cus_id = 0;

        double subtotal = 0;

        bool add_orderline_btn_clicked = false;

        Dictionary<int, string> cus_selectdict = new Dictionary<int, string>();

        void Handle_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            throw new NotImplementedException();
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

            if( entry && res)
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

                overper =  Convert.ToDouble(overper.ToString("n2"));

                overper_string = overper.ToString();

                dis1.Text = overper.ToString();
             
            }


            else
            {
                DisplayAlert("Alert", "Please give valid input", "Ok");

            }

            //if(e.NewTextValue != "+" && !="")
            //{
                
            //}

            //else

        }

        List<KeyValuePair<string, dynamic>> order_lines = new List<KeyValuePair<string, dynamic>>();
        List<int> taxidList = new List<int>();

        List<Dictionary<string, dynamic>> abc = new List<Dictionary<string, dynamic>>();
        List<OrderLinesList> orderLineList1 = new List<OrderLinesList>();
        List<OrderLinesList> orderLineList2 = new List<OrderLinesList>();

      //  Dictionary<int, string> cusdictdb = new Dictionary<int, string>();
        List<taxes> taxListdb = new List<taxes>();
        List<paytermList> payment_termsdb = new List<paytermList>();
        List<commisiongroupList> commission_groupdb = new List<commisiongroupList>();

     //   Dictionary<int, object> salesteamdict = new Dictionary<int, object>(


        void exdateentry_focused(object sender, FocusEventArgs e)
        {
            expdate_entry_stack.IsVisible = false;
            expirdate_picker_stack.IsVisible = true;
            exDatePicker.Focus();
            exdateentry.Text = exDatePicker.Date.ToString();

        }

        void deldeaddateentry_focused(object sender, FocusEventArgs e)
        {
            deldead_entry_stack.IsVisible = false;
            deldeaddate_picker_stack.IsVisible = true;
            deldead_picker.Focus();
            deldateentry.Text = deldead_picker.Date.ToString();
        }


        void podateentry_focused(object sender, FocusEventArgs e)
        {
            podate_entry_stack.IsVisible = false;
            podate_picker_stack.IsVisible = true;
            PoDatePicker.Focus();
            podateentry.Text = PoDatePicker.Date.ToString();
        }


       
        public SalesQuotationCreationPage()
        {
            InitializeComponent();
            orderListview.HeightRequest = 0;

            App.cusList = App.cusList;

           // exDatePicker.Date.ToString()
            void exdateentry_focused(object sender, FocusEventArgs e)
            {
                expdate_entry_stack.IsVisible = false;
                expirdate_picker_stack.IsVisible = true;
                exDatePicker.Focus();
                exdateentry.Text = exDatePicker.Date.ToString();

            }

            void deldeaddateentry_focused(object sender, FocusEventArgs e)
            {
                deldead_entry_stack.IsVisible = false;
                deldeaddate_picker_stack.IsVisible = true;
                deldead_picker.Focus();
                deldateentry.Text = deldead_picker.Date.ToString();
            }


            void podateentry_focused(object sender, FocusEventArgs e)
            {
                podate_entry_stack.IsVisible = false;
                podate_picker_stack.IsVisible = true;
                PoDatePicker.Focus();
                podateentry.Text = PoDatePicker.Date.ToString();
            }
            if (App.NetAvailable == true)
            {

               // cuspicker1.ItemsSource = App.cusdict.Select(x => x.Value).ToList();
               // cuspicker1.SelectedIndex = -1;

                pricelist_picker.ItemsSource = App.product_PriceList.Select(x => x.name).ToList();
                pricelist_picker.SelectedIndex = -1;

                // var num = App.product_PriceList.Where(x => x.name == pricelist_picker.SelectedItem.ToString()).Select(x => x.id);

                // string  ids = num[0];

                tax_termPicker.Items.Add("Free tax");
                tax_termPicker.Items.Add("Pay via PT. LABORINDO SARANA");
                tax_termPicker.Items.Add("Pay via Customer");

                tax_termPicker.SelectedIndex = 0;



                var dropdownImgRecognizer = new TapGestureRecognizer();
                dropdownImgRecognizer.Tapped += (s, e) =>
                {
                    Navigation.PushPopupAsync(new PickerSelectionPage());

                    searchprod.IsVisible = true;
                };
                pickerdropimg.GestureRecognizers.Add(dropdownImgRecognizer);



                //taxpicker.ItemsSource = App.taxList.Select(x => x.Name).ToList();
                //taxpicker.SelectedIndex = 0;

                ptpicker.ItemsSource = App.paytermList.Select(x => x.name).ToList();
                ptpicker.SelectedIndex = -1;

                comgroup_picker.ItemsSource = App.commisiongroupList.Select(x => x.name).ToList();
                comgroup_picker.SelectedIndex = -1;

                delmethod_picker.ItemsSource = App.all_delivery_method.Select(x => x.name).ToList();
                delmethod_picker.SelectedIndex = -1;

            }

            else if (App.NetAvailable == false)
            {
                int user_iddb = 0;
                int partner_iddb = 0;

                JArray taxtlist_array = new JArray();

                foreach (var res in App.UserListDb)
                {

                  //  cusdictdb = JsonConvert.DeserializeObject<Dictionary<int, string>>(res.customers_list);
                    taxtlist_array = JsonConvert.DeserializeObject<JArray>(res.tax_list);
                    payment_termsdb = JsonConvert.DeserializeObject<List<paytermList>>(res.payment_terms);
                    commission_groupdb = JsonConvert.DeserializeObject<List<commisiongroupList>>(res.commission_group);

                    user_iddb = res.userid;
                    partner_iddb = res.partnerid;

                }

                //foreach (JObject obj in taxtlist_array)
                //{
                //    taxes taxesdb = new taxes("");
                //    taxesdb.Id = obj["id"].ToObject<int>();
                //    taxesdb.Name = obj["name"].ToString();
                //    taxListdb.Add(taxesdb);
                //}


                App.taxListdb = taxListdb;

            //    cuspicker1.ItemsSource = cusdictdb.Select(x => x.Value).ToList();
              //  cuspicker1.SelectedIndex = 0;

                var dropdownImgRecognizer = new TapGestureRecognizer();
                dropdownImgRecognizer.Tapped += (s, e) =>
                {
                    Navigation.PushPopupAsync(new PickerSelectionPage());
                };
                pickerdropimg.GestureRecognizers.Add(dropdownImgRecognizer);

                //taxpicker.ItemsSource = taxListdb.Select(x => x.Name).ToList();
                //taxpicker.SelectedIndex = 0;

                ptpicker.ItemsSource = payment_termsdb.Select(x => x.name).ToList();
                ptpicker.SelectedIndex = 0;

                comgroup_picker.ItemsSource = commission_groupdb.Select(x => x.name).ToList();
                comgroup_picker.SelectedIndex = 0;
            }

            var AirConImgRecognizer = new TapGestureRecognizer();
            AirConImgRecognizer.Tapped += (s, e) => {
                // handle the tap 
                //pd.ItemsSource = App.productList.Select(x => x.Name).ToList();
                //pd.SelectedIndex = 0;
                airconImg1.IsVisible = true;
                AddAirCon.IsVisible = false;
                orderLineGrid.IsVisible = true;
                discount_grid.IsVisible = true;
                //    orderLineGrid_1.IsVisible = true;

                taxname = "";

                taxlistviewGrid.IsVisible = true;

                Addtax_line.IsVisible = true;

                taxStackLayout.IsVisible = false;

                dis1.Text = "";
                multidis.Text = "";

                //  addtaxGrid.IsVisible = true;

                //  taxpicker.SelectedIndex = 0;
                taxListView.ItemsSource = null;
                orderListview.ItemsSource = orderLineList1;
            };
            AddAirCon.GestureRecognizers.Add(AirConImgRecognizer);



            var Addtax_lineImgRecognizer = new TapGestureRecognizer();
            Addtax_lineImgRecognizer.Tapped += (s, e) => {

                // addtaxGrid.IsVisible = true;
                Addtax_line.IsVisible = false;

                Navigation.PushPopupAsync(new TaxSelectionPage());

            };
            Addtax_line.GestureRecognizers.Add(Addtax_lineImgRecognizer);



            var overallcloseImgRecognizer = new TapGestureRecognizer();
            overallcloseImgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                Navigation.PopAllPopupAsync();
            };
            overall_close.GestureRecognizers.Add(overallcloseImgRecognizer);
        }

        private void Tab1Clicked(object sender, EventArgs ea)
        {
            tab1stack.BackgroundColor = Color.FromHex("#363E4B");
            tab1.BackgroundColor = Color.FromHex("#363E4B");
            tab2stack.BackgroundColor = Color.White;
            tab2.BackgroundColor = Color.White;
            tab2frame.BackgroundColor = Color.FromHex("#363E4B");
            tab2borderstack.BackgroundColor = Color.White;
            orderLineList.IsVisible = true;
            OtherInfoStack1.IsVisible = false;
            //  OtherInfoStack2.IsVisible = false;
            tab1frame.BackgroundColor = Color.FromHex("#363E4B");
            tab1borderstack.BackgroundColor = Color.FromHex("#363E4B");
            OrderLineList1.IsVisible = true;

            // tap2_clicked = false;

            //if (editbtn_clicked == true)
            //{
            //    addbtn_orderline.IsVisible = true;
            //}
            //savebtn_layout.IsVisible = true;

            tab1.TextColor = Color.White;
            tab2.TextColor = Color.Black;

            airconImg.IsVisible = true;
            AddAirCon.IsVisible = true;

        
         //   airconImg1.IsVisible = true;
        }

        private void Tab2Clicked(object sender, EventArgs ea)
        {
            tab2stack.BackgroundColor = Color.FromHex("#363E4B");
            tab2.BackgroundColor = Color.FromHex("#363E4B");
            tab1stack.BackgroundColor = Color.White;
            tab1.BackgroundColor = Color.White;
            tab2borderstack.BackgroundColor = Color.FromHex("#363E4B");
            tab2frame.BackgroundColor = Color.FromHex("#363E4B");
            orderLineList.IsVisible = false;
            OtherInfoStack1.IsVisible = true;
            //  OtherInfoStack2.IsVisible = true;
            tab1frame.BackgroundColor = Color.FromHex("#363E4B");
            tab1borderstack.BackgroundColor = Color.White;
            OrderLineList1.IsVisible = false;

            //  listview_editlayout.IsVisible = false;
            orderLineGrid.IsVisible = false;
            Addtax_line.IsVisible = false;
            airconImg1.IsVisible = false;

            discount_grid.IsVisible = false;
            taxlistviewGrid.IsVisible = false;
            //   addbtn_orderline.IsVisible = false;

            tab1.TextColor = Color.Black;
            tab2.TextColor = Color.White;

            if (salesperson_picker.SelectedItem == null)
            {
                salesperson_picker.ItemsSource = App.salespersons.Select(x => x.Value).ToList();
            }


                shipping_picker.Items.Clear();

                 shipping_picker.Items.Add("Deliver each product when available");
                shipping_picker.Items.Add("Deliver all products at once");
                shipping_picker.SelectedIndex = -1;


            if(warehouse_picker.SelectedItem == null)
            {
                warehouse_picker.ItemsSource = App.warehousList.Select(x => x.name).ToList();
            }
            if (analytic_picker.SelectedItem == null)
            {
                analytic_picker.ItemsSource = App.analayticList.Select(x => x.name).ToList();
            }
            if (branch_picker.SelectedItem == null)
            {
                branch_picker.ItemsSource = App.branchList.Select(x => x.name).ToList();
            }

            if (salesteam_picker.SelectedItem == null)
            {
                salesteam_picker.ItemsSource = App.salesteam.Select(x => x.Value).ToList();
            }
            //   tap2_clicked = true;

            airconImg.IsVisible = false;

            //  savebtn_layout.IsVisible = false;

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string, string>("MyApp", "NotifyMsg", (sender, arg) =>
            {
                // HideLbl.Text = "New Quotation Creation";

            });


            MessagingCenter.Subscribe<string, int>("MyApp", "CusPickerMsg", (sender, arg) =>
            {
                // HideLbl.Text = "New Quotation Creation";

                if (App.cusList.Count != 0)
                {
                    var productlis = from pro in App.cusList
                                     where pro.id == arg
                                     select pro;

                    foreach (var prodresults in productlis)
                    {
                        searchcus.Text = prodresults.name;
                        cus_id = prodresults.id;
                    }
                }

                else
                {
                    var productlis = from pro in App.cusList
                                     where pro.id == arg
                                     select pro;

                    foreach (var prodresults in productlis)
                    {
                        searchcus.Text = prodresults.name;
                        cus_id = prodresults.id;
                    }
                }

               // int i = 0;


               // var cusid = App.cusdict.FirstOrDefault(x => x.Value == cuspicker1.SelectedItem.ToString()).Key;

                JObject con_dict = App.cus_address.FirstOrDefault(x => x.Key == cus_id.ToString()).Value;

                cus_selectdict = con_dict.ToObject<Dictionary<int, string>>();

                if (cus_selectdict.Count == 0)
                {
                    //  invaddr_picker.Items.Add(cuspicker1.SelectedItem.ToString());
                    invaddr_picker.Items.Clear();
                    invaddr_picker.Items.Add(searchcus.Text);
                    invaddr_picker.SelectedItem = searchcus.Text.ToString();
                    invaddr_picker.SelectedIndex = 0;
                }

                else
                {
                    invaddr_picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
                    invaddr_picker.SelectedIndex = 0;
                }

                if (cus_selectdict.Count == 0)
                {
                    deladdr_picker.Items.Clear();
                    deladdr_picker.Items.Add(searchcus.Text);
                    deladdr_picker.SelectedItem = searchcus.Text.ToString();
                    deladdr_picker.SelectedIndex = 0;
                   // deladdr_picker.SelectedItem = cuspicker1.SelectedItem.ToString();
                }

                else
                {
                    deladdr_picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
                    deladdr_picker.SelectedIndex = 0;
                }

                contperson_picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
                contperson_picker.SelectedIndex = 0;


            });



            MessagingCenter.Subscribe<string, string>("MyApp", "taxPickerMsg", (sender, arg) =>
            {

                taxListView.ItemsSource = null;

                App.taxListRemove.Add(new taxes(arg));

                App.taxListRemove = App.taxListRemove.GroupBy(i => i.Name).Select(g => g.First()).ToList();
                // taxpicker.IsVisible = false;
                taxStackLayout.IsVisible = true;
                taxListView.ItemsSource = App.taxListRemove;

                taxListView.RowHeight = 35;
                taxListView.HeightRequest = 35 * App.taxListRemove.Count;

                taxStackLayout.BackgroundColor = Color.FromHex("#363E4B");
                taxListView.BackgroundColor = Color.FromHex("#363E4B");
                taxStackLayout.CornerRadius = 20;

                taxStackLayout.Padding = 5;

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
        }


        private void Date_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            //  SelectedDate = e.NewDate;
            //  TextEntry.Text = String.IsNullOrEmpty(Format) ? e.NewDate.ToString(Format) : e.NewDate.ToString();
        }

        private void Button_Back_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAllPopupAsync();
        }

        //private void cus_indexChanged(object sender, EventArgs e)
        //{

        //    var cusid = App.cusdict.FirstOrDefault(x => x.Value == cuspicker1.SelectedItem.ToString()).Key;

        //    JObject con_dict = App.cus_address.FirstOrDefault(x => x.Key == cusid.ToString()).Value;

        //    cus_selectdict = con_dict.ToObject<Dictionary<int, string>>();

        //    if (cus_selectdict.Count == 0)
        //    {
        //        //  invaddr_picker.Items.Add(cuspicker1.SelectedItem.ToString());
        //        invaddr_picker.Items.Clear();
        //        invaddr_picker.Items.Add(cuspicker1.SelectedItem.ToString());
        //        invaddr_picker.SelectedItem = cuspicker1.SelectedItem.ToString();
        //        invaddr_picker.SelectedIndex = 0;
        //    }

        //    else
        //    {
        //        invaddr_picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
        //        invaddr_picker.SelectedIndex = 0;
        //    }

        //    if (cus_selectdict.Count == 0)
        //    {
        //        deladdr_picker.Items.Clear();
        //        deladdr_picker.Items.Add(cuspicker1.SelectedItem.ToString());
        //        deladdr_picker.SelectedItem = cuspicker1.SelectedItem.ToString();
        //        deladdr_picker.SelectedIndex = 0;
        //        deladdr_picker.SelectedItem = cuspicker1.SelectedItem.ToString();
        //    }

        //    else
        //    {
        //        deladdr_picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
        //        deladdr_picker.SelectedIndex = 0;
        //    }

        //    contperson_picker.ItemsSource = cus_selectdict.Select(x => x.Value).ToList();
        //    contperson_picker.SelectedIndex = 0;


        //}

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            // return base.OnBackButtonPressed();

            Navigation.PopAllPopupAsync();
            return true;
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        protected async void Button_Create_ClickedAsync(object sender, EventArgs e)
        {
            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);

            int selectpaytermid = 0;
            int selectcomgroupid = 0;
            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();


            String order_date_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", orDatePicker.Date);

            String exp_date_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", exDatePicker.Date);

            String po_date_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", PoDatePicker.Date);

            String deldead_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", deldead_picker.Date);



            if (tax_termPicker.SelectedIndex == 0)
            {
                vals["tax_term"] = "free";
            }

            else if (tax_termPicker.SelectedIndex == 1)

            {
                vals["tax_term"] = "company";
            }

            else
            {
                vals["tax_term"] = "customer";
            }



            //try
            //{
            //    List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

            //}

            //catch (Exception ea)
            //{
            //   if (ea.Message.Contains("(Network is unreachable)") || ea.Message.Contains("NameResolutionFailure"))
            //   {
            //       App.NetAvailable = false;
            //   }

            //   else if (ea.Message.Contains("(503) Service Unavailable"))
            //   {
            //       App.responseState = false;
            //   }

            //}

            if (orderLineList1.Count == 0)
            {
                await DisplayAlert("Alert", "Please fill orderlines atleast one", "Ok");

                // await Navigation.PopAsync();

                await PopupNavigation.PopAsync();

                // Loadingalertcall();
            }



           // else if (cuspicker1.SelectedIndex == -1)
            else if (searchcus.Text == "")
            {
                cus_alert.IsVisible = true;
                invaddr_alert.IsVisible = false;
                delvaddr_alert.IsVisible = false;
                deldead_alert.IsVisible = false;
                pricelist_alert.IsVisible = false;
                paymentterms_alert.IsVisible = false;
                delmethod_alert.IsVisible = false;

                await PopupNavigation.PopAsync();
            }

            else if (invaddr_picker.SelectedIndex == -1)
            {

                cus_alert.IsVisible = false;

                delvaddr_alert.IsVisible = false;
                deldead_alert.IsVisible = false;
                pricelist_alert.IsVisible = false;
                paymentterms_alert.IsVisible = false;
                delmethod_alert.IsVisible = false;

                invaddr_alert.IsVisible = true;
                await PopupNavigation.PopAsync();
            }


            else if (deladdr_picker.SelectedIndex == -1)
            {
                cus_alert.IsVisible = false;
                invaddr_alert.IsVisible = false;

                deldead_alert.IsVisible = false;
                pricelist_alert.IsVisible = false;
                paymentterms_alert.IsVisible = false;
                delmethod_alert.IsVisible = false;

                delvaddr_alert.IsVisible = true;
                await PopupNavigation.PopAsync();
            }


            else if (deldateentry.Text == "")
            {
                cus_alert.IsVisible = false;
                invaddr_alert.IsVisible = false;
                delvaddr_alert.IsVisible = false;

                pricelist_alert.IsVisible = false;
                paymentterms_alert.IsVisible = false;
                delmethod_alert.IsVisible = false;

                deldead_alert.IsVisible = true;
                await PopupNavigation.PopAsync();
            }

            else if (pricelist_picker.SelectedIndex == -1)
            {
                cus_alert.IsVisible = false;
                invaddr_alert.IsVisible = false;
                delvaddr_alert.IsVisible = false;
                deldead_alert.IsVisible = false;

                paymentterms_alert.IsVisible = false;
                delmethod_alert.IsVisible = false;

                pricelist_alert.IsVisible = true;
                await PopupNavigation.PopAsync();
            }


            else if (ptpicker.SelectedIndex == -1)
            {
                cus_alert.IsVisible = false;
                invaddr_alert.IsVisible = false;
                delvaddr_alert.IsVisible = false;
                deldead_alert.IsVisible = false;
                pricelist_alert.IsVisible = false;

                delmethod_alert.IsVisible = false;

                paymentterms_alert.IsVisible = true;
                await PopupNavigation.PopAsync();
            }

            else if (delmethod_picker.SelectedIndex == -1)
            {
                cus_alert.IsVisible = false;
                invaddr_alert.IsVisible = false;
                delvaddr_alert.IsVisible = false;
                deldead_alert.IsVisible = false;
                pricelist_alert.IsVisible = false;
                paymentterms_alert.IsVisible = false;


                delmethod_alert.IsVisible = true;
                await PopupNavigation.PopAsync();
            }

            else
            {
                // vals["customer"] = cuspicker1.SelectedItem;

              //  vals["quotation_reference"] = quotref.Text;

                vals["order_date"] = order_date_string;

                vals["expiration_date"] = exp_date_string;

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


                vals["user_id"] = App.userid;

                vals["delivery_deadline"] = deldead_string;

                try
                {
                    string ids = App.product_PriceList.First(c => c.name == pricelist_picker.SelectedItem.ToString()).id;

                    vals["pricelist_id"] = Int32.Parse(ids);
                }

                catch
                {
                    vals["pricelist_id"] = null;
                }

                //  vals["payment_term_id"] = "";


                vals["po_number_reference"] = ponumref.Text;


                vals["po_date"] = po_date_string;

                //if(podateentry.Text == "")
                //{
                //    vals["po_date"] = podateentry.Text;
                //}

                //else
                //{
                //    vals["po_date"] = po_date_string;
                //}


                vals["po_file"] = file_uploadstring;
                vals["file_name"] = file_uploadname;

                vals["franco"] = franco.Text;

                vals["special_notes"] = comments.Text;

               // var cusid = App.cusdict.FirstOrDefault(x => x.Value == cuspicker1.SelectedItem.ToString()).Key;
                var cusid = App.cusList.FirstOrDefault(x => x.name == searchcus.Text.ToString()).id;
                vals["customer"] = cusid;

                try
                {
                    string ids = App.product_PriceList.First(c => c.name == pricelist_picker.SelectedItem.ToString()).id;

                    vals["pricelist_id"] = Int32.Parse(ids);
                }

                catch
                {
                    vals["pricelist_id"] = null;
                }

                try
                {

                    var inv_id = cus_selectdict.FirstOrDefault(x => x.Value == invaddr_picker.SelectedItem.ToString()).Key;
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
                    var delvaddr_id = cus_selectdict.FirstOrDefault(x => x.Value == deladdr_picker.SelectedItem.ToString()).Key;
                    vals["partner_shipping_id"] = delvaddr_id;

                    if (delvaddr_id == 0)
                    {
                        vals["partner_shipping_id"] = cusid;
                    }
                }

                catch
                {
                    vals["partner_shipping_id"] = 0;
                }


                try
                {
                    var conperson_id = cus_selectdict.FirstOrDefault(x => x.Value == contperson_picker.SelectedItem.ToString()).Key;
                    vals["contact_person_id"] = conperson_id;

                    if (conperson_id == 0)
                    {
                        vals["contact_person_id"] = false;
                    }
                }

                catch
                {
                    vals["contact_person_id"] = false;
                }

                var delmethod_id = App.all_delivery_method.FirstOrDefault(x => x.name == delmethod_picker.SelectedItem.ToString()).id;
                vals["carrier_id"] = Int32.Parse(delmethod_id);

                vals["order_lines"] = orderLineList1;
                vals["state"] = "x_draft";

                // vals["state"] = "quotation";

                vals["is_direct_so"] = false;

                if (shipping_picker.SelectedIndex == 0)
                {

                    vals["picking_policy"] = "direct";
                }

                else if(shipping_picker.SelectedIndex == 1)
                {
                    vals["picking_policy"] = "one";
                }

                else
                {
                    vals["picking_policy"] = false;
                }


                vals["client_order_ref"] = cus_reference.Text;
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


                if (App.NetAvailable == true)
                {

                    string updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_sale_quotation", vals);

                    if (updated == "true")
                    {
                        // App.Current.MainPage = new MasterPage(new CrmTabbedPage());
                        // Navigation.PushPopupAsync(new MasterPage(  );
                        // await  DisplayAlert("Alert", "Successfully created", "Ok");
                        //  await Navigation.PopAllPopupAsync();

                        App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab3"));

                        Loadingalertcall();
                    }

                    else
                    {
                        await DisplayAlert("Alert", "Please try again", "Ok");
                        Loadingalertcall();
                    }
                }

                else if (App.NetAvailable == false)
                {

                    string ptpickerstring = ptpicker.SelectedItem.ToString();
                    string cgpickerstring = comgroup_picker.SelectedItem.ToString();

                    // ptpickerstring = ptpicker.SelectedItem.ToString();

                    if (ptpicker.SelectedItem == null)
                    {
                        //  vals["payment_terms"] = false;

                        ptpickerstring = "";
                    }
                    else
                    {

                        var paytermid =
                        (
                                from i in payment_termsdb
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

                    }

                    vals["payment_terms"] = selectpaytermid;


                    if (comgroup_picker.SelectedItem == null)
                    {
                        //vals["commission_group"] = false;
                        cgpickerstring = "";
                    }

                    else
                    {
                        var comgroupid =
                        (
                                from i in commission_groupdb
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

                 //   var cusiddb = App.cusdictDb.FirstOrDefault(x => x.Value == cuspicker1.SelectedItem.ToString()).Key;
                    //var cusiddb = App.cusdict.FirstOrDefault(x => x.Value == searchcus.Text.ToString()).Key;
                    //vals["customer"] = cusiddb;

                    List<OrderLine> or_linelistdb = new List<OrderLine>();


                    foreach (var obj in orderLineList1)
                    {
                        OrderLine or_line = new OrderLine();

                        List<int> tax_id = new List<int>();

                        or_line.product_name = obj.product.ToString();
                        or_line.product_uom_qty = obj.ordered_qty.ToString();
                        or_line.price_subtotal = obj.ordered_qty.ToString();
                        or_line.discount = obj.discount;
                        or_line.multi_discount = obj.multi_discount;

                        or_line.taxesid = obj.tax_id;

                        // or_line.taxes = tax_id;

                        or_linelistdb.Add(or_line);
                    }

                    var orderLineListnew = JsonConvert.SerializeObject(or_linelistdb);
                    string order_date = orDatePicker.Date.ToString();
                    //  DateTime oDate = DateTime.ParseExact(order_date, "yyyy-MM-dd HH:mm tt", null);

                    //   String order_date_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", orDatePicker.Date);
                    String expiration_date_string = String.Format("{0:yyyy-MM-dd HH:mm:ss}", exDatePicker.Date);


                    var sample = new SalesQuotationDB
                    {
                        //  id = item.id,

                        order_date = order_date_string,
                        expiration_date = expiration_date_string,
                        payment_term = ptpickerstring,
                        commission_group = cgpickerstring,
                        payment_term_id = selectpaytermid,
                        commission_group_id = selectcomgroupid,
                        user_id = App.userid_db,
                      //  customer_id = cusiddb,
                        order_line = orderLineListnew,
                        customer = searchcus.Text.ToString(),
                        date_Order = orDatePicker.Date.ToString(),
                        name = "LocalSO",
                        FullState = "draft",
                        state = "draft",
                        state_colour = "#3498db",

                        yellowimg_string = "yellowcircle.png",

                        //   ColorCode  = "#3498db"

                        //order_line = item.order_line[0].ToString()
                    };
                    App._connection.Insert(sample);


                    App._connection.CreateTable<SalesQuotationDB>();
                    try
                    {
                        var details = (from y in App._connection.Table<SalesQuotationDB>() select y).ToList();
                        App.SalesQuotationListDb = details;
                    }
                    catch (Exception ex)
                    {
                        int i = 0;
                    }


                    App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab3"));

                    // await DisplayAlert("Alert", "Created Successfull", "Ok");
                    // await Navigation.PopAllPopupAsync();

                    Loadingalertcall();

                }

            }
        }

        void productentry(object sender, EventArgs args)
        {
            Navigation.PushPopupAsync(new PickerSelectionPage());
        }







        private async void ol_clickedAsync(object sender, EventArgs e)
        {
     
            airconImg1.IsVisible = false;
            taxlistviewGrid.IsVisible = false;
            //  addtaxGrid.IsVisible = false;

            orderListview.ItemsSource = orderLineList2;
            Dictionary<string, dynamic> xyz = new Dictionary<string, dynamic>();

            if (up.Text == "" || oqty.Text == null || oqty.Text == "" || searchprod.Text == "")
            {

                await  DisplayAlert("Alert", "Please fill all the fields", "Ok");

                orderLineGrid.IsVisible = false;
                discount_grid.IsVisible = false;
                // orderLineGrid_1.IsVisible = false;
                airconImg.IsVisible = true;
                AddAirCon.IsVisible = true;
                Addtax_line.IsVisible = false;

                orderListview.ItemsSource = orderLineList1;

            }
            else
            {

                //  xyz.Add("product", pd.SelectedItem.ToString());
                xyz.Add("product", searchprod.Text);

                try
                {
                    xyz.Add("ordered_qty", Convert.ToDouble(oqty.Text));
                }

                catch
                {
                    await DisplayAlert("Alert", "Please fill all the fields", "Ok");
                }


                try
                {
                    xyz.Add("unit_price", Convert.ToDouble(up.Text));
                }
                catch
                {
                    await DisplayAlert("Alert", "Please fill all the fields", "Ok");
                }

                xyz.Add("description", orderline_des.Text);



                if (taxidList.Count > 0)
                {


                    foreach (var tax_string in taxidList)
                    {
                        string namenew = App.taxList.Where(item => item.Id == tax_string).Select(x => x.Name).SingleOrDefault();

                        taxname = taxname + "," + namenew;
                    }


                    taxname = taxname.Substring(2);
                }


                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);

                if(dis1.Text =="")
                {
                    dis1.Text = "0";
                }
                subtotal = Controller.InstanceCreation().getsubtotal("sale.order", "get_sub_total", float.Parse(up.Text),float.Parse(oqty.Text),taxidList,float.Parse( dis1.Text));
                int subtotal1 = (int)subtotal;
                orderLineList1.Add(new OrderLinesList(searchprod.Text, Convert.ToDouble(oqty.Text), Convert.ToDouble(up.Text), taxidList, orderline_des.Text, taxname, dis1.Text, multidis.Text, subtotal1));

                orderListview.ItemsSource = orderLineList1;
                orderListview.RowHeight = 40;

                await PopupNavigation.PopAsync();

                orderLineGrid.IsVisible = false;
                discount_grid.IsVisible = false;
                // orderLineGrid_1.IsVisible = false;
                airconImg.IsVisible = true;
                AddAirCon.IsVisible = true;

                orderListview.HeightRequest = 40 * orderLineList1.Count;

                searchprod.Text = "";
                oqty.Text = "";
                up.Text = "";
                orderline_des.Text = "";

                Addtax_line.IsVisible = false;

                taxidList = new List<int>();
            }
        }


        void customerentry(object sender, EventArgs args)
        {
            Navigation.PushPopupAsync(new CustomerSelectionPage());
        }


        public void ListviewcloseClicked(object sender, EventArgs e1)
        {
            try
            {
                var args = (TappedEventArgs)e1;
                taxes t2 = args.Parameter as taxes;

                var itemToRemove = App.taxListRemove.Single(r => r.Name == t2.Name);

                App.taxListRemove.Remove(itemToRemove);


                taxListView.ItemsSource = null;


                //  taxStackLayout.Padding = 0;

                taxListView.ItemsSource = App.taxListRemove;
                taxListView.RowHeight = 35;
                taxListView.HeightRequest = 35 * App.taxListRemove.Count;



                if (App.taxListRemove.Count == 0)
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
                        file_uploadstring = Convert.ToBase64String(bydata);
                        file_uploadname = filedata.FileName;
                        //  topimg = UploadData;
                        // var stream = new MemoryStream(bydata);
                        // user_img.Source = ImageSource.FromStream(() => stream);
                    }
                    file_frame.IsVisible = true;
                    filename_lbl.Text = filedata.FileName;
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



        private void ViewCelltax_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#102b1e");
            //  m_title.TextColor = Color.Red;
        }

    }
}
