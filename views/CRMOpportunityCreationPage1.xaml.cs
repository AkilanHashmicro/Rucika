using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.DBModel;
using SalesApp.models;
using SalesApp.Pages;
using SalesApp.wizard;
using Xamarin.Forms;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    public partial class CRMOpportunityCreationPage1 : PopupPage
    {
        string count = "0";
        Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
        Dictionary<string, dynamic> vals1 = new Dictionary<string, dynamic>();
        List<AttendeesList> attList = new List<AttendeesList>();
        List<AttendeesList> attList1 = new List<AttendeesList>();

        Dictionary<int, object> salespersdict = new Dictionary<int, object>();
        Dictionary<int, object> salesteamdict = new Dictionary<int, object>();

        List<int> atnIdsList = new List<int>();

        List<MeetingLinesList> meetingLineList = new List<MeetingLinesList>();

        double duration = 0;

         Dictionary<int, string> cusdict = new Dictionary<int, string>();
        List<stages> stageListDB = new List<stages>();
      //  Dictionary<int, object> nextactivitydict = new Dictionary<int, object>();
        List<next_activity> nextActivityListDB = new List<next_activity>();

       // List<next_activity> nextact_List = new List<next_activity>();

        Customers cus = new Customers();
        stages stagesmodel = new stages();

        next_activity nxt_activity = new next_activity();


        public CRMOpportunityCreationPage1()
        {
            InitializeComponent();

            int product_id = 0;

       
        
            if (App.NetAvailable == true)
            {

                cuspicker1.ItemsSource = App.cusList.Select(d => d.name).ToList();
                cuspicker1.SelectedIndex = 0;

                stagePicker.ItemsSource = App.stageList.Select(x => x.name).ToList();
                stagePicker.SelectedIndex = 0;

                nextActPicker.ItemsSource = App.nextActivityList.Select(x => x.name).ToList();
                nextActPicker.SelectedIndex = 0;

                salesperson_Picker.ItemsSource = App.salespersons.Select(x => x.Value).ToList();
                salesperson_Picker.SelectedIndex = 0;

                salesteam_Picker.ItemsSource = App.salesteam.Select(x => x.Value).ToList();
                salesteam_Picker.SelectedIndex = 0;

                attnListView.HeightRequest = 0;
                meetingsListview.HeightRequest = 0;

                attnPicker.ItemsSource = App.cusList.Select(d => d.name).ToList();
                attnPicker.SelectedIndex = -1;

                atnIdsList.Add(App.partner_id);
                string value = App.cusList.Single(x => x.id == App.partner_id).name;
               // string value = keyValuePair.Value;
                attList.Add(new AttendeesList(value));
                attnListView.ItemsSource = attList;

                attnListView.ItemsSource = attList;
                attnListView.RowHeight = 30;
                attnListView.HeightRequest = 30 * attList.Count;

            }

            if (App.NetAvailable == false)
            {
                //int user_iddb = 0;
                //int partner_iddb = 0;

                //JArray nxt_activities = new JArray();

               
            }


            var empstar1Recognizer = new TapGestureRecognizer();
            empstar1Recognizer.Tapped += (s, e) => {

                str1.IsVisible = true;
                em1.IsVisible = false;
                str2.IsVisible = false;
                str3.IsVisible = false;
                em2.IsVisible = true;
                em3.IsVisible = true;
                //  RatingLayout1.IsVisible = true;
                //RatingLayout2.IsVisible = false;
                //RatingLayout3.IsVisible = false;
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
                count ="2";
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

            var AirConImgRecognizer = new TapGestureRecognizer();
            AirConImgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                addMeetings.IsVisible = true;
                attnListView.IsVisible = true;
                AddAirCon.IsVisible = false;
                addbtnstack.IsVisible = true;
                atnstack.IsVisible = true;
                AddAirCon1.IsVisible = true;
             //   Navigation.PushPopupAsync(new AddCrmOppMeetingWizard());
               // Navigation.PushPopupAsync(new AddOppMeetingWizard());
            };
            AddAirCon.GestureRecognizers.Add(AirConImgRecognizer);


            var AirConImgRecognizer1 = new TapGestureRecognizer();
            AirConImgRecognizer1.Tapped += (s, e) => {
                attnPicker.IsVisible = true;
                attnListView.ItemsSource = attList;
                attnListView.RowHeight = 30;
                attnListView.HeightRequest = 30 * attList.Count;
            };
            AddAirCon1.GestureRecognizers.Add(AirConImgRecognizer1);


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
            tab2frame.BackgroundColor = Color.Gray;
            tab2borderstack.BackgroundColor = Color.White;
            notesList.IsVisible = true;
            meetingsList.IsVisible = false;
            // OtherInfoStack2.IsVisible = false;
            tab1frame.BackgroundColor = Color.FromHex("#363E4B");
            tab1borderstack.BackgroundColor = Color.FromHex("#363E4B");
            //OrderLineList1.IsVisible = true;
            atnstack.IsVisible = false;
            addMeetings.IsVisible = false;
            addbtnstack.IsVisible = false;

            tab1.TextColor = Color.White;
            tab2.TextColor = Color.Black;
        }

        private void Tab2Clicked(object sender, EventArgs ea)
        {
            tab2stack.BackgroundColor = Color.FromHex("#363E4B");
            tab2.BackgroundColor = Color.FromHex("#363E4B");
            tab1stack.BackgroundColor = Color.White;
            tab1.BackgroundColor = Color.White;
            tab2borderstack.BackgroundColor = Color.FromHex("#363E4B");
            tab2frame.BackgroundColor = Color.FromHex("#363E4B");
            notesList.IsVisible = false;
            meetingsList.IsVisible = true;
            tab1frame.BackgroundColor = Color.Gray;
            tab1borderstack.BackgroundColor = Color.White;

            tab2.TextColor = Color.White;
            tab1.TextColor = Color.Black;

            AddAirCon.IsVisible = true;

            atnstack.IsVisible = false;
            addMeetings.IsVisible = false;
            addbtnstack.IsVisible = false;

        }

        public void myPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (App.NetAvailable == true)
            {

                attnListView.ItemsSource = attList1;
                attnPicker.IsVisible = false;
                attList.Add(new AttendeesList(attnPicker.SelectedItem.ToString()));

                int cusid = App.cusList.FirstOrDefault(x => x.name == attnPicker.SelectedItem.ToString()).id;
                atnIdsList.Add(cusid);

                atnIdsList = atnIdsList.GroupBy(i => i).Select(g => g.First()).ToList();
                attList = attList.GroupBy(i => i.name).Select(g => g.First()).ToList();

                attnListView.ItemsSource = attList;
                attnListView.RowHeight = 30;
                attnListView.HeightRequest = 30 * attList.Count;

                return;

            }

            else if (App.NetAvailable == false)
            {

                //attnListView.ItemsSource = attList1;
                //attnPicker.IsVisible = false;
                //attList.Add(new AttendeesList(attnPicker.SelectedItem.ToString()));

                //int cusid = cusdict.FirstOrDefault(x => x.Value == attnPicker.SelectedItem.ToString()).Key;
                //atnIdsList.Add(cusid);

                //atnIdsList = atnIdsList.GroupBy(i => i).Select(g => g.First()).ToList();
                //attList = attList.GroupBy(i => i.name).Select(g => g.First()).ToList();

                //attnListView.ItemsSource = attList;
                //attnListView.RowHeight = 30;
                //attnListView.HeightRequest = 30 * attList.Count;

                //return;

            }

        }

        private void Button_Add_Clicked1(object sender, EventArgs e)
        {
            meetingsListview.ItemsSource = null;
            AddAirCon.IsVisible = true;
            addbtnstack.IsVisible = false;
            addMeetings.IsVisible = false;

            atnstack.IsVisible = false;
            AddAirCon1.IsVisible = false;

            try
            {
                 duration = Convert.ToDouble(Dur1.Text);
            }

            catch
            {
                 duration = 0;
            }


            DateTime startdateTime;
            DateTime stopdateTime;;

            try
            {
                startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }

            catch (Exception exe)
            {
                try
                {
                    startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                }

                catch (Exception exa)
                {
                    try
                    {
                        startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ea1)
                    {

                        try
                        {
                            startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                            try
                            {
                                startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                try
                                {
                                    startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                    try
                                    {
                                        startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {

                                        try
                                        {
                                            startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                                        }

                                        catch
                                        {

                                            startdateTime = DateTime.ParseExact(sDate.Date.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string start_date = startdateTime.ToString("yyyy-MM-dd HH:mm:ss");


            try
            {
                stopdateTime = DateTime.ParseExact(sDate.Date.ToString(), "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }

            catch (Exception exe)
            {
                try
                {
                    stopdateTime = DateTime.ParseExact(eDate.Date.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                }

                catch (Exception exa)
                {
                    try
                    {
                        stopdateTime = DateTime.ParseExact(eDate.Date.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ea1)
                    {

                        try
                        {
                            stopdateTime = DateTime.ParseExact(eDate.Date.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                            try
                            {
                                stopdateTime = DateTime.ParseExact(eDate.Date.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                try
                                {
                                    stopdateTime = DateTime.ParseExact(eDate.Date.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                    try
                                    {
                                        stopdateTime = DateTime.ParseExact(eDate.Date.ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {

                                        try
                                        {
                                            stopdateTime = DateTime.ParseExact(eDate.Date.ToString(), "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                                        }

                                        catch
                                        {

                                            stopdateTime = DateTime.ParseExact(eDate.Date.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            string end_date = stopdateTime.ToString("yyyy-MM-dd HH:mm:ss");

            meetingLineList.Add(new MeetingLinesList(sub1.Text, start_date, end_date, loc1.Text, duration, atnIdsList));

            meetingsListview.ItemsSource = meetingLineList;
            meetingsListview.RowHeight = 40;
            meetingsListview.HeightRequest = 40 * meetingLineList.Count;

            if(meetingLineList.Count > 0)
            {
                createStackLayout.IsVisible = true;
            }
                                        
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        private async void createClickedAsync(object sender, EventArgs ea)
        {   


            var currentpage = new LoadingAlert();

            await PopupNavigation.PushAsync(currentpage);

            var cusid = App.cusList.FirstOrDefault(x => x.name == cuspicker1.SelectedItem.ToString()).id;
            vals["customer"] = cusid;
            vals["oppurtunity_title"] = oppTitle.Text;
            vals["email"] = eMail.Text;
            vals["phone"] = phone.Text;

          //  String picker_date = nextActDatePicker.Date.ToString();
            DateTime startdateTime;


            try
            {
                startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }

            catch (Exception exe)
            {
                try
                {
                    startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                }

                catch (Exception exa)
                {
                    try
                    {
                        startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ea1)
                    {

                        try
                        {
                            startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                            try
                            {
                                startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                try
                                {
                                    startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                    try
                                    {
                                        startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {

                                        try
                                        {
                                            startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                                        }

                                        catch
                                        {

                                            startdateTime = DateTime.ParseExact(nextActDatePicker.Date.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string nextactdatepicker_date = startdateTime.ToString("yyyy-MM-dd");

           // vals["next_activity_date"] = nextActDatePicker.Date.ToString();

            vals["next_activity_date"] = nextactdatepicker_date;
            vals["next_activity"] = nextActPicker.SelectedItem.ToString();
            vals["next_activity_summary"] = nextActSum.Text;

            if(exRev.Text=="")
            {
                exRev.Text = "0";
            }
            vals["expected_revenue"] = Convert.ToDouble( exRev.Text);
            vals["rating"] = count;
            vals["stage"] = stagePicker.SelectedItem;
            vals["internal_notes"] = comments.Text;
            vals["meetings"] = meetingLineList;
            vals["state"] = "new";


            var salespersonid = App.salespersons.FirstOrDefault(x => x.Value == salesperson_Picker.SelectedItem.ToString()).Key;
            vals["user_id"] = salespersonid;

            var salesteamid = App.salesteam.FirstOrDefault(x => x.Value == salesteam_Picker.SelectedItem.ToString()).Key;
            vals["team_id"] = salesteamid;

            var customerid = App.cusList.FirstOrDefault(x => x.name == cuspicker1.SelectedItem.ToString()).id;
            vals["partner_id"] = customerid;

            vals["partner_name"] = cuspicker1.SelectedItem.ToString();

            int next_activity_id = 0;

            var nextactivityid =
                (
                    from i in App.nextActivityList
                    where i.name == nextActPicker.SelectedItem.ToString()
                    select new
                    {
                        i.id,
                    }
           ).ToList();

            foreach (var comgroup in nextactivityid)
            {
                next_activity_id = comgroup.id;
            }

            vals["next_activity_id"] = next_activity_id;


            if(cuspicker1.SelectedItem.ToString() == "")           
            {
                var alert = DisplayAlert("Alert", "Please select the customer", "Ok");
            }


            String date_deadline_string = String.Format("{0:yyy-MM-dd HH:mm:ss}", expected_closing_Picker.Date);

            vals["date_deadline"] = date_deadline_string;

            //   List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

            if (App.NetAvailable == true)
            {

                if (Convert.ToInt32(prob_entry.Text) > 100)
                {
                    // leadtitle_alert.IsVisible = false;
                    prob_alert.IsVisible = true;
                }

                else

                {

                    //bool updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_crm_quotations", vals);
                    string updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_crm_quotations", vals);

                    if (updated == "true")
                    {
                        // App.Current.MainPage = new MasterPage(new CrmTabbedPage());
                        // Navigation.PushPopupAsync(new MasterPage(  );
                        // await DisplayAlert("Alert", "Created Successfull", "Ok");
                        //  await   Navigation.PopAllPopupAsync();
                        App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab2"));
                        await Navigation.PopAllPopupAsync();

                    }
                    else
                    {
                        await DisplayAlert("Alert", "Please try again", "Ok");
                        await PopupNavigation.PopAsync();
                    }
                }
            }

            else if (App.NetAvailable == false)
            {

                //    var cusiddb = cusdict.FirstOrDefault(x => x.Value == cuspicker1.SelectedItem.ToString()).Key;

                //    var json_meetingLineList = JsonConvert.SerializeObject(meetingLineList);


                //    int next_activity_offline_id = 0;

                //    var nextactivityids =
                //        (
                //            from i in App.nextActivityList
                //            where i.name == nextActPicker.SelectedItem.ToString()
                //            select new
                //            {
                //                i.id,
                //            }
                //   ).ToList();

                //    foreach (var comgroup in nextactivityids)
                //    {
                //        next_activity_offline_id = comgroup.id;
                //    }




                //    String ex_closing_string = String.Format("{0:MM-dd-yyyy HH:mm:ss}", expected_closing_Picker.Date);

                //    var sample = new CRMOpportunitiesDB
                //    {
                //        //  id = item.id,
                //        cusid = cusiddb,
                //        customer = cuspicker1.SelectedItem.ToString(),
                //        next_activity = nextActPicker.SelectedItem.ToString(),
                //        name =oppTitle.Text,
                //        email = eMail.Text,
                //        phone = phone.Text,
                //        next_activity_date = nextactdatepicker_date.ToString(),
                //        expected_revenue = Convert.ToDouble(exRev.Text).ToString(),
                //        rating = count,
                //        stage = stagePicker.SelectedItem.ToString(),
                //        internal_notes = comments.Text,
                //        meetings = json_meetingLineList,
                //        FullState = "  New  ",
                //        state = "  New  ",
                //        state_colour = "#3498db",
                //        nextact_id = next_activity_offline_id,
                //        oppurtunity_title = oppTitle.Text,
                //        next_activity_summary = nextActSum.Text,
                //        expected_closing = date_deadline_string,
                //        yellowimg_string = "yellowcircle.png",

                //        //order_line = item.order_line[0].ToString()
                //    };
                //    App._connection.Insert(sample);

                //    App._connection.CreateTable<CRMOpportunitiesDB>();
                //    try
                //    {
                //        var details = (from y in App._connection.Table<CRMOpportunitiesDB>() select y).ToList();
                //        App.CRMOpportunitiesListDb = details;
                //    }
                //    catch (Exception ex)
                //    {
                //        int i = 0;
                //    }

                // // await  DisplayAlert("Alert", "Created Successfull", "Ok");

                //    App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab2"));

                //    await Navigation.PopAllPopupAsync();

                //}
            }
        }

        async void ListviewcloseClicked(object sender, EventArgs e1)
        {
            var args = (TappedEventArgs)e1;
            AttendeesList t2 = args.Parameter as AttendeesList;

            var itemToRemove = attList.Single(r => r.name == t2.name);

            attList.Remove(itemToRemove);
            attnListView.ItemsSource = attList;
            attnListView.RowHeight = 30;
            attnListView.HeightRequest = 30 * attList.Count;
            // taxes obj = (taxes)args;
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAllPopupAsync();
            return true;
        }
    }
}
