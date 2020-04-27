using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
    public partial class LeadCreationPage : PopupPage
    {
        string count = "0";
        Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
        Dictionary<string, dynamic> vals1 = new Dictionary<string, dynamic>();
        List<AttendeesList> attList = new List<AttendeesList>();
        List<AttendeesList> attList1 = new List<AttendeesList>();

        List<int> atnIdsList = new List<int>();

        List<MeetingLinesList> meetingLineList = new List<MeetingLinesList>();

        double duration = 0;

        Dictionary<int, object> salespersdict = new Dictionary<int, object>();
        Dictionary<int, object> salesteamdict = new Dictionary<int, object>();
        Dictionary<int, object> customerdict = new Dictionary<int, object>();
        Dictionary<int, object> tagsdict = new Dictionary<int, object>();
        Dictionary<int, object> statedict = new Dictionary<int, object>();
        Dictionary<int, object> countrydict = new Dictionary<int, object>();

        List<next_activity> nextact_List = new List<next_activity>();

        int next_activity_id = 0;

        public LeadCreationPage()
        {
            InitializeComponent();
                                 
            var overallcloseImgRecognizer = new TapGestureRecognizer();
            overallcloseImgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                Navigation.PopAllPopupAsync();
            };
            overall_close.GestureRecognizers.Add(overallcloseImgRecognizer);

           
            if (App.NetAvailable == true)
            {

                salesperson_Picker.ItemsSource = App.salespersons.Select(x => x.Value).ToList();
                salesperson_Picker.SelectedIndex = 0;

                salesteam_Picker.ItemsSource = App.salesteam.Select(x => x.Value).ToList();
                salesteam_Picker.SelectedIndex = 0;

                //tagsPicker.ItemsSource = App.crmleadtags.Select(x => x.Value).ToList();
                //tagsPicker.SelectedIndex = 0;


                //customer_Picker.ItemsSource = App.cusdict.Select(x => x.Value).ToList();
                //customer_Picker.SelectedIndex = 0;

                //nextact_Picker.ItemsSource = App.nextActivityList.Select(x => x.name).ToList();
                //nextact_Picker.SelectedIndex = 0;

              //  state_picker.ItemsSource = App.statedict.Select(x => x.Value).ToList();
                // state_picker.SelectedIndex = -1;

              //  country_picker.ItemsSource = App.countrydict.Select(x => x.Value).ToList();
                // country_picker.SelectedIndex = -1;


                var keyValuePair1 = App.salespersons.Single(x => x.Key == App.userid);
                string value1 = keyValuePair1.Value;

                salesperson_Picker.SelectedItem = value1;

                int countdic = App.cusList.Count;

                int p_id = App.partner_id;

            }

            if (App.NetAvailable == false)
            {
              

                int user_iddb = 0;
                int partner_iddb = 0;


                foreach(var res in App.UserListDb)
                {

                    salespersdict = JsonConvert.DeserializeObject<Dictionary<int, object>>(res.sales_persons);
                    salesteamdict = JsonConvert.DeserializeObject<Dictionary<int, object>>(res.sales_team);
                    tagsdict = JsonConvert.DeserializeObject<Dictionary<int, object>>(res.tagsPicker);
                    statedict = JsonConvert.DeserializeObject<Dictionary<int, object>>(res.state_list);
                    countrydict = JsonConvert.DeserializeObject<Dictionary<int, object>>(res.country_list);
                    customerdict = JsonConvert.DeserializeObject<Dictionary<int, object>>(res.customers_list);
                    nextact_List = JsonConvert.DeserializeObject<List<next_activity>>(res.next_activity);
                    user_iddb = res.userid;
                    partner_iddb = res.partnerid;
                }

                salesperson_Picker.ItemsSource = salespersdict.Select(x => x.Value).ToList();
                salesperson_Picker.SelectedIndex = 0;

                //customer_Picker.ItemsSource = customerdict.Select(x => x.Value).ToList();
                //customer_Picker.SelectedIndex = 0;

                salesteam_Picker.ItemsSource = salesteamdict.Select(x => x.Value).ToList();
                salesteam_Picker.SelectedIndex = 0;

                //tagsPicker.ItemsSource = tagsdict.Select(x => x.Value).ToList();
                //tagsPicker.SelectedIndex = 0;

                //nextact_Picker.ItemsSource = nextact_List.Select(x => x.name).ToList();
                //nextact_Picker.SelectedIndex = 0;

              //  state_picker.ItemsSource = statedict.Select(x => x.Value).ToList();
                // state_picker.SelectedIndex = -1;

              //  country_picker.ItemsSource = countrydict.Select(x => x.Value).ToList();
                // country_picker.SelectedIndex = -1;


                var keyValuePair1 = salespersdict.Single(x => x.Key == user_iddb);
                string value1 = keyValuePair1.Value.ToString();

                salesperson_Picker.SelectedItem = value1;

                //   int countdic = App.cusdict.Count;

                int p_id = partner_iddb;

            }


            //atnIdsList.Add(App.partner_id);
            //var keyValuePair = App.cusdict.Single(x => x.Key == App.partner_id);
            //string value = keyValuePair.Value;
            //attList.Add(new AttendeesList(value));


            var empstar1Recognizer = new TapGestureRecognizer();
            empstar1Recognizer.Tapped += (s, e) => {

                str1.IsVisible = true;
                em1.IsVisible = false;
                str2.IsVisible = false;
                str3.IsVisible = false;
                em2.IsVisible = true;
                em3.IsVisible = true;

                count = "1";
            };
            em1.GestureRecognizers.Add(empstar1Recognizer);

            var empstar2Recognizer = new TapGestureRecognizer();
            empstar2Recognizer.Tapped += (s, e) => {
                str1.IsVisible = true;
                str2.IsVisible = true;
                em2.IsVisible = false;
                em1.IsVisible = false;
                str3.IsVisible = false;
                em3.IsVisible = true;
                count = "2";
            };
            em2.GestureRecognizers.Add(empstar2Recognizer);

            var empstar3Recognizer = new TapGestureRecognizer();
            empstar3Recognizer.Tapped += (s, e) => {
                str1.IsVisible = true;
                str2.IsVisible = true;
                str3.IsVisible = true;
                em3.IsVisible = false;
                em2.IsVisible = false;
                em1.IsVisible = false;
                count = "3";
            };
            em3.GestureRecognizers.Add(empstar3Recognizer);

            var str1Recognizer = new TapGestureRecognizer();
            str1Recognizer.Tapped += (s, e) => {

                str1.IsVisible = true;
                em2.IsVisible = true;
                em3.IsVisible = true;
                str2.IsVisible = false;
                str3.IsVisible = false;

                count = "1";

            };
            str1.GestureRecognizers.Add(str1Recognizer);

            var str2Recognizer = new TapGestureRecognizer();
            str2Recognizer.Tapped += (s, e) => {

                str1.IsVisible = true;
                str2.IsVisible = true;
                str3.IsVisible = false;
                em1.IsVisible = false;
                em2.IsVisible = false;
                em3.IsVisible = true;

                count = "2";

            };
            str2.GestureRecognizers.Add(str2Recognizer);

            var str3Recognizer = new TapGestureRecognizer();
            str3Recognizer.Tapped += (s, e) => {

                str1.IsVisible = true;
                str2.IsVisible = true;
                str3.IsVisible = false;
                em1.IsVisible = false;
                em2.IsVisible = false;
                em3.IsVisible = true;

                count = "3";

            };
            str3.GestureRecognizers.Add(str3Recognizer);

        }

       
        private async void discardClickedAsync(object sender, System.EventArgs e)
        {           
            await  Navigation.PopAllPopupAsync();
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            // return base.OnBackButtonPressed();

            Navigation.PopAllPopupAsync();
            return true;
        }

        public void lead_TextChanged(object sender, EventArgs e)
        {
            cus_name.Text = leadTitle.Text;
        }
               
        private async void createClickedAsync(object sender, EventArgs ea)
        {

            if (leadTitle.Text == "")
            {
                leadtitle_alert.IsVisible = true;
            }

            //else if (Convert.ToInt32(prob_entry.Text) > 100) 
            //{
            //    leadtitle_alert.IsVisible = false;
            //    prob_alert.IsVisible = true;
            //}

            else
            {
                //var cusid = App.cusdict.FirstOrDefault(x => x.Value == customer_Picker.SelectedItem.ToString()).Key;
                //vals["partner_id"] = cusid;

                var currentpage = new LoadingAlert();

                await PopupNavigation.PushAsync(currentpage);


                int stateid = 0;
                int countryid = 0;

                vals["contact_name"] = contName.Text;

                vals["name"] = leadTitle.Text;

                vals["email_from"] = eMail.Text;
                vals["phone"] = phone.Text;
                vals["function"] = jobPos.Text;
                //vals["street"] = street.Text;
                //vals["street2"] = street2.Text;
                //vals["zip"] = zip.Text;

                var salespersonid = App.salespersons.FirstOrDefault(x => x.Value == salesperson_Picker.SelectedItem.ToString()).Key;
                vals["user_id"] = salespersonid;

                var salesteamid = App.salesteam.FirstOrDefault(x => x.Value == salesteam_Picker.SelectedItem.ToString()).Key;
                vals["team_id"] = salesteamid;

                //var customerid = App.cusdict.FirstOrDefault(x => x.Value == customer_Picker.SelectedItem.ToString()).Key;
                //vals["partner_id"] = customerid;

                //vals["partner_name"] = customer_Picker.SelectedItem.ToString();

               // var nextactivityid =
               //     (
               //         from i in App.nextActivityList
               //         where i.name == nextact_Picker.SelectedItem.ToString()
               //             select new
               //             {
               //                 i.id,
               //             }
               //).ToList();

                //foreach (var comgroup in nextactivityid)
                //{
                //    next_activity_id = comgroup.id;
                //}

               // vals["next_activity_id"] = next_activity_id;

                //String date_deadline_string = String.Format("{0:MM-dd-yyyy HH:mm:ss}", expected_closing_Picker.Date);

                //vals["date_deadline"] = date_deadline_string;

              //  vals["planned_revenue"] = exrev_entry.Text;

               // vals["probability"] = prob_entry.Text;

                //if (state_picker.SelectedItem == null)
                //{
                //    vals["state_id"] = false;
                //}
                //else
                //{
                //     stateid = App.statedict.FirstOrDefault(x => x.Value == state_picker.SelectedItem.ToString()).Key;
                //    vals["state_id"] = stateid;
                //}

                //if (country_picker.SelectedItem == null)
                //{
                //    vals["country_id"] = false;
                //}
                //else
                //{
                //     countryid = App.countrydict.FirstOrDefault(x => x.Value == country_picker.SelectedItem.ToString()).Key;
                //    vals["country_id"] = countryid;
                //}


                vals["description"] = comments.Text;

                vals["type"] = "lead";

                vals["priority"] = count;


                //bool updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_crm_quotations", vals);

                if (App.NetAvailable == true)
                {

                    string   updated = Controller.InstanceCreation().UpdateLeadCreationData("crm.lead", "create", vals);

                    if (updated == "Odoo Error")
                    {                       
                        await  DisplayAlert("Alert", "Please try again", "Ok");
                        PopupNavigation.PopAsync();
                    }
                    else
                    {
                        var currentpage1 = new LoadingAlert();
                        await PopupNavigation.PushAsync(currentpage1);

                      //  await DisplayAlert("Alert", "Created Successfull", "Ok");

                    //    List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

                        App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab1"));

                        Loadingalertcall();

                        salesteam_Picker.Unfocus();

                    }
                }

                else if (App.NetAvailable == false)
                {
                  
                    int stateidoffline = 0;
                    int countryidoffline =0;

                    var salespersonidoffline = salespersdict.FirstOrDefault(x => x.Value == salesperson_Picker.SelectedItem.ToString()).Key;
                    vals["user_id"] = salespersonidoffline;

                    var salesteamidoffline = salesteamdict.FirstOrDefault(x => x.Value == salesteam_Picker.SelectedItem.ToString()).Key;
                    vals["team_id"] = salesteamidoffline;


                    //var customeridoffline = customerdict.FirstOrDefault(x => x.Value == customer_Picker.SelectedItem.ToString()).Key;
                    //vals["partner_id"] = customeridoffline;

                    //var nextactidoffline = nextact_List.FirstOrDefault(x => x.name == nextact_Picker.SelectedItem.ToString());
                    //vals["next_activity_id"] = nextactidoffline;

                   // var nextactidoffline =
                   //(
                   //         from i in nextact_List
                   //    where i.name == nextact_Picker.SelectedItem.ToString()
                   //    select new
                   //    {
                   //        i.id,
                   //    }
                   //).ToList();

                    //foreach (var comgroup in nextactivityid)
                    //{
                    //    next_activity_id = comgroup.id;
                    //}

                    //String ex_closing_string = String.Format("{0:MM-dd-yyyy HH:mm:ss}", expected_closing_Picker.Date);


                    //if (state_picker.SelectedItem == null)
                    //{
                    //  //  vals["state_id"] = false;
                    //    stateidoffline = 0;
                    //}
                    //else
                    //{
                    //    stateidoffline = statedict.FirstOrDefault(x => x.Value == state_picker.SelectedItem.ToString()).Key;
                    //    vals["state_id"] = stateidoffline;
                    //}

                    //if (country_picker.SelectedItem == null)
                    //{
                    //    // vals["country_id"] = false;

                    //    countryidoffline = 0;
                    //}
                    //else
                    //{
                    //    countryidoffline = countrydict.FirstOrDefault(x => x.Value == country_picker.SelectedItem.ToString()).Key;
                    //    vals["country_id"] = countryidoffline;
                    //}


                    var sample = new CRMLeadDB
                    {
                        //  id = item.id,
                        customer = leadTitle.Text,
                        // next_activity = item.next_activity,

                        name = leadTitle.Text,

                        //   probability = item.probability,
                        //  phone = item.phone,
                        //  title_action = item.title_action,
                        //  expected_revenue = item.expected_revenue,

                        //   team_name = salesteam_Picker.SelectedItem.ToString(),
                        priority = Int32.Parse(count),
                        //  state = state_picker.SelectedItem.ToString(),
                        //street = street.Text,
                        //street2 = street2.Text,
                        //city = city.Text,
                        //   country = country_picker.SelectedItem.ToString(),
                        contact_name = contName.Text,
                        mobile = phone.Text,
                        email = eMail.Text,

                        function = jobPos.Text,
                        user_id = salespersonid,
                        team_id = salesteamid,
                        state_id = stateid,
                        country_id = countryid,
                        description = comments.Text,
                        type = "lead",
                        nextact_id = next_activity_id,
                      //  expected_closing = ex_closing_string,
                      //  zip= zip.Text,
                       // pipe_date = item.pipe_date,
                     //   pipe_date1 = item.pipe_date1,
                      //  conDate = item.conDate,
                       // conDate1 = item.conDate1,
                      //  DateOrder = item.DateOrder,
                        FullState = "  New  ",
                        yellowimg_string = "yellowcircle.png",
                        state_colour = "#3498db",
                      //  server_color = "#fafb80",
                        serverupdate_string = "Local"

                        //order_line = item.order_line[0].ToString()
                    };
                    App._connection.Insert(sample);


                    App._connection.CreateTable<CRMLeadDB>();
                    try
                    {
                        var details = (from y in App._connection.Table<CRMLeadDB>() select y).ToList();
                        App.crmListDb = details;
                    }
                    catch (Exception ex)
                    {
                        int i = 0;
                    }

                 //   await DisplayAlert("Alert", "Created Successfull", "Ok");
                   // await Navigation.PopAllPopupAsync();
                    App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab1"));

                    Loadingalertcall();

                }
                            
            }

        }

    }
}
