using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.models;
using SalesApp.views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SalesApp.models.CRMModel;


//24/01/2019 00:00:00
namespace SalesApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPopupPage : PopupPage
    {
               
        Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

        Dictionary<string, dynamic> vals_update = new Dictionary<string, dynamic>();

        List<AttendeesList> attList = new List<AttendeesList>();

        List<all_events> attlistviewlist = new List<all_events>();

        List<AttendeesList> attList1 = new List<AttendeesList>();
        List<int> atnIdsList = new List<int>();

        List<TagsList> tagsList = new List<TagsList>();
        List<TagsList> tagsList1 = new List<TagsList>();
        List<int> tagIdsList = new List<int>();

        DateTime start_dateTime = new DateTime();
        DateTime stop_dateTime = new DateTime();

        string start_fullTime = "";
        string stop_fullTime = "";

        int updateId = 0;

        public CalendarPopupPage()
        {
            InitializeComponent();

            update_stack.IsVisible = false;
            create_stack.IsVisible = true;

            attspicker1.ItemsSource = App.cusList.Select(x => x.name).ToList();
            attspicker1.SelectedIndex = -1;
            attspicker1.Title = "Select";

            tagspicker1.ItemsSource = App.tagsDict.Select(x => x.Value).ToList();
            tagspicker1.SelectedIndex = -1;
            tagspicker1.Title = "Select";

            if (App.calselecteddate == "")
            {
                var dateselected = DateTime.Now.ToString("yyyy-MM-dd");
                App.calselecteddate = dateselected;
            }

            DateTime caldate= DateTime.ParseExact(App.calselecteddate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            st_date.Date = caldate;

            atnIdsList.Add(App.partner_id);

            string value = App.cusList.Single(x => x.id == App.partner_id).name;
            //string value = keyValuePair.Value;
            attList.Add(new AttendeesList(value));
            attnListView.ItemsSource = attList;

            attnListView.ItemsSource = attList;
            attnListView.RowHeight = 40;
            attnListView.HeightRequest = 40 * attList.Count;
            mainstack.HeightRequest = mainstack.HeightRequest + 40;
             pickergrid.IsVisible = false;
            Addbtn.IsVisible = true;

            var AirConImgRecognizer = new TapGestureRecognizer();
            AirConImgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                attnListView.IsVisible = true;
                Addbtn.IsVisible = false;
                pickergrid.IsVisible = true;
            };
            AddAirCon1.GestureRecognizers.Add(AirConImgRecognizer);

            var AirConImgRecognizer1 = new TapGestureRecognizer();
            AirConImgRecognizer1.Tapped += (s, e) => {
                // handle the tap              
                tagsListView.IsVisible = true;
                Addbtn1.IsVisible = false;
                tagsgrid.IsVisible = true;
            };
            AddAirCon2.GestureRecognizers.Add(AirConImgRecognizer1);


            var overallcloseImgRecognizer = new TapGestureRecognizer();
            overallcloseImgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                Navigation.PopAllPopupAsync();
            };
            overall_close.GestureRecognizers.Add(overallcloseImgRecognizer);

        }

        public CalendarPopupPage(all_events obj)
        {
            InitializeComponent();

            update_stack.IsVisible = true;
            create_stack.IsVisible = false;

            attnListView.IsVisible = true;
          //  attList = obj.attendees;
                       
            foreach(var attobj in obj.attendees)
            {
                attList.Add(new AttendeesList(attobj.name));
                int cusid = App.cusList.FirstOrDefault(x => x.name == attobj.name).id;
                atnIdsList.Add(cusid);
            } 

            meeting_subject.Text = obj.meeting_subject;
            loc.Text = obj.location;
            dur.Text = obj.duration;

            updateId = obj.id;

            start_dateTime = DateTime.ParseExact(obj.start, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string start_datestring = start_dateTime.ToString("yyyy-MM-dd");
            string start_timestring = start_dateTime.ToLocalTime().ToString("HH:mm");
            start_fullTime = start_datestring + " " + start_timestring;
            TimeSpan start_timespan = TimeSpan.Parse(start_timestring);
            st_date.Date = start_dateTime;
            st_poptime.Time = start_timespan;

            if (obj.allday == true)
            {
                check1.Checked = true;

                stoplabelStack.IsVisible = true;
                stoplabelStack1.IsVisible = true;
                durStack.IsVisible = false;
                //DateTime startDate1 = start_dateTime.AddHours(Convert.ToDouble(obj.duration));
                //string startstring1 = startDate1.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            }

            stop_dateTime = DateTime.ParseExact(obj.stop, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string stop_datestring = stop_dateTime.ToString("yyyy-MM-dd");
            string stop_timestring = stop_dateTime.ToLocalTime().ToString("HH:mm");
            stop_fullTime = stop_datestring + " " + stop_timestring;
            TimeSpan stop_timespan = TimeSpan.Parse(stop_timestring);


            stop_date.Date = stop_dateTime;
            stop_poptime.Time = stop_timespan;

            //stop_date.Date = start_dateTime;
            //stop_poptime.Time = start_timespan;

            attnListView.ItemsSource = attList;
            attnListView.RowHeight = 40;
            attnListView.HeightRequest = 40 * attList.Count;
            mainstack.HeightRequest = mainstack.HeightRequest + attnListView.HeightRequest;

            foreach(var attobj in obj.tags)
            {
                tagsList.Add(new TagsList(attobj.name));
                int cusid = App.tagsDict.FirstOrDefault(x => x.Value == attobj.name).Key;
                tagIdsList.Add(cusid);
            } 

            tagsListView.ItemsSource = tagsList;
            tagsListView.RowHeight = 40;
            tagsListView.HeightRequest = 40 * tagsList.Count;

            mainstack.HeightRequest = mainstack.HeightRequest + tagsListView.HeightRequest;

            attspicker1.ItemsSource = App.cusList.Select(x => x.name).ToList();
            attspicker1.SelectedIndex = -1;
            attspicker1.Title = "Select";

            tagspicker1.ItemsSource = App.tagsDict.Select(x => x.Value).ToList();
            attspicker1.SelectedIndex = -1;
            attspicker1.Title = "Select";

            var AirConImgRecognizer = new TapGestureRecognizer();
            AirConImgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                attnListView.IsVisible = true;
                Addbtn.IsVisible = false;
                pickergrid.IsVisible = true;
            };
            AddAirCon1.GestureRecognizers.Add(AirConImgRecognizer);

            var AirConImgRecognizer1 = new TapGestureRecognizer();
            AirConImgRecognizer1.Tapped += (s, e) => {
                // handle the tap              
                tagsListView.IsVisible = true;
                Addbtn1.IsVisible = false;
                tagsgrid.IsVisible = true;
            };
            AddAirCon2.GestureRecognizers.Add(AirConImgRecognizer1);

            var overallcloseImgRecognizer = new TapGestureRecognizer();
            overallcloseImgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                Navigation.PopAllPopupAsync();
            };
            overall_close.GestureRecognizers.Add(overallcloseImgRecognizer);

        }

        public void myPickerSelectedIndexChanged(object sender, EventArgs e)
        {            
            int cusid = App.cusList.FirstOrDefault(x => x.name == attspicker1.SelectedItem.ToString()).id;
            atnIdsList.Add(cusid);

            attList.Add(new AttendeesList(attspicker1.SelectedItem.ToString()));

            atnIdsList = atnIdsList.GroupBy(i => i).Select(g => g.First()).ToList();
            attList = attList.GroupBy(i => i.name).Select(g => g.First()).ToList();

            attnListView.ItemsSource = attList;
            attnListView.RowHeight = 40;
            attnListView.HeightRequest = 40 * attList.Count;

            mainstack.HeightRequest = mainstack.HeightRequest + 40;

            pickergrid.IsVisible = false;
            Addbtn.IsVisible = true;
        }

        public void tagsPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            int cusid = App.tagsDict.FirstOrDefault(x => x.Value == tagspicker1.SelectedItem.ToString()).Key;
            tagIdsList.Add(cusid);

            tagsList.Add(new TagsList(tagspicker1.SelectedItem.ToString()));

            tagIdsList = tagIdsList.GroupBy(i => i).Select(g => g.First()).ToList();
            tagsList = tagsList.GroupBy(i => i.name).Select(g => g.First()).ToList();

            tagsListView.ItemsSource = tagsList;
            tagsListView.RowHeight = 40;
            tagsListView.HeightRequest = 40 * tagsList.Count;

            mainstack.HeightRequest = mainstack.HeightRequest + 40;

            tagsgrid.IsVisible = false;
            Addbtn1.IsVisible = true;
        }

        async void ListviewcloseClicked(object sender, EventArgs e1)
        {
            attnListView.ItemsSource = attList1;
            var args = (TappedEventArgs)e1;
            AttendeesList t2 = args.Parameter as AttendeesList;

            var itemToRemove = attList.Single(r => r.name == t2.name);
            attList.Remove(itemToRemove);

            int cusid = App.cusList.FirstOrDefault(x => x.name == t2.name.ToString()).id;
            atnIdsList.Remove(cusid);

            attnListView.ItemsSource = attList;
            attnListView.RowHeight = 40;
            attnListView.HeightRequest = 40 * attList.Count;

            mainstack.HeightRequest = mainstack.HeightRequest - 40;
            // taxes obj = (taxes)args;
         }

        async void tagsListviewcloseClicked(object sender, EventArgs e1)
        {
            tagsListView.ItemsSource = tagsList1;
           tagsListView.ItemsSource = null;
            var args = (TappedEventArgs)e1;
            TagsList t2 = args.Parameter as TagsList;
            var itemToRemove = tagsList.Single(r => r.name == t2.name);
            tagsList.Remove(itemToRemove);

            int cusid = App.tagsDict.FirstOrDefault(x => x.Value == t2.name.ToString()).Key;
            tagIdsList.Remove(cusid);

            tagsListView.ItemsSource = tagsList;
            tagsListView.RowHeight = 40;
            tagsListView.HeightRequest = 40 * tagsList.Count;
            mainstack.HeightRequest = mainstack.HeightRequest - 40;             
        }

        private void check1_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (check1.Checked == true)
            {
                stoplabelStack.IsVisible = true;
                stoplabelStack1.IsVisible = true;
                durStack.IsVisible = false;
                //stop_date.Date =  st_date.Date;
                //stop_poptime.Time = st_poptime.Time;

                //DateTime startDate1 = startDate.AddHours(Convert.ToDouble(dur.Text));
                //string startstring1 = startDate1.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

            }

            else
            {
                stoplabelStack.IsVisible = false;
                stoplabelStack1.IsVisible = false;
                durStack.IsVisible = true;
             //   mainstack.HeightRequest = mainstack.HeightRequest - 40; 
            }

        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {

            try
            {

                Contract.Ensures(Contract.Result<Task>() != null);
                date_alert.IsVisible = false;
                date_box.IsVisible = false;
                meeting_alert.IsVisible = false;
                meeting_box.IsVisible = false;

                DateTime startdateTime;
                DateTime stopdateTime;




               


                // DateTime startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

             //  startdateTime = DateTime.ParseExact("24/01/2019 00:00:00", "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);

                //try
                //{
                //    startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                //}

                //catch (Exception exe)
                //{
                //    try
                //    {
                //        startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                //    }

                //    catch (Exception ea)
                //    {
                //        try
                //        {
                //            startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //        }
                //        catch (Exception ea1)
                //        {

                //            try
                //            {
                //                startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //            }
                //            catch
                //            {

                //                try
                //                {
                //                    startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                //                }
                //                catch
                //                {
                //                    try
                //                    {
                //                        startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //                    }
                //                    catch
                //                    {

                //                        try
                //                        {
                //                            startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //                        }
                //                        catch
                //                        {
                //                            startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                String convertstartdate = st_date.Date.ToString("yyyy-MM-dd");


             //   string convertstartdate = startdateTime.ToString("yyyy-MM-dd");
                var dt = st_poptime.Time.ToString();

                convertstartdate = convertstartdate + " " + dt.ToString();
                //  string convertstarttime1 = startdateTime;

                //  string convertstarttime = dt.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

                string convertstarttime = convertstartdate;

                DateTime startDate = DateTime.ParseExact(convertstarttime, "yyyy-MM-dd HH:mm:ss", null);

            //    convertstarttime = startDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

                convertstarttime = startDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");


                //try
                //{
                //    stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                //}

                //catch (Exception exc)
                //{
                //    try
                //    {
                //        stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);

                //    }
                //    catch (Exception a)
                //    {

                //        try
                //        {
                //            stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //        }

                //        catch
                //        {

                //            try
                //            {
                //                stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //            }
                //            catch
                //            {

                //                try
                //                {
                //                    stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                //                }

                //                catch
                //                {                                 
                //                        try
                //                        {
                //                            stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //                        }

                //                    catch
                //                    {
                //                        try
                //                        {
                //                            stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //                        }
                //                        catch
                //                        {
                //                            try
                //                            {
                //                                stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "dd/MM/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                //                            }

                //                            catch
                //                            {
                //                                try
                //                                {
                //                                    stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "M/d/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                //                                }
                //                                catch
                //                                {
                //                                    stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //                                }
                //                            }


                //                        }

                //                    }
                //                }


                //            }
                //        }
                //    }
                //}




                //string convertstopdate = stopdateTime.ToString("yyyy-MM-dd");

                string convertstopdate = stop_date.Date.ToString("yyyy-MM-dd");


                var dt1 = stop_poptime.Time.ToString();

                // string convertstoptime = dt1.ToUniversalTime().ToString("HH:mm:ss");

                string convertstoptime = convertstopdate + " " + dt1.ToString();
                  
                DateTime stopDate = DateTime.ParseExact(convertstoptime, "yyyy-MM-dd HH:mm:ss", null);

                convertstoptime = stopDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

                vals["location"] = loc.Text.ToString();
                vals["attendees"] = atnIdsList;
                vals["tags"] = tagIdsList;
                vals["meeting_subject"] = meeting_subject.Text;

                vals["start_datetime"] = convertstarttime;
                vals["stop_datetime"] = convertstoptime;
                vals["start"] = convertstarttime;
                vals["stop"] = convertstoptime;
                vals["description"] = des.Text;

                vals["start_date"] = startDate.ToString("yyyy-MM-dd");

                if (check1.Checked == true)
                {
                    vals["all_day"] = true;
                    vals["duration"] = 0;
                 //   vals["start_date"] = convertstarttime;
                    vals["start_date"] = startDate.ToString("yyyy-MM-dd");
                   // vals["stop_date"] = convertstoptime;
                    vals["stop_date"] = stopDate.ToString("yyyy-MM-dd");

                    vals_update["start"] = convertstarttime;
                    vals_update["stop"] = convertstoptime;

                    if (stopDate.Date <= startDate.Date)
                    {
                        date_alert.IsVisible = true;
                        date_box.IsVisible = true;
                        // DisplayAlert("Alert", "Please fill meeting subject", "Ok");
                    }
                }

                else
                {
                    vals["all_day"] = false;

                    try
                    {
                        vals["duration"] = Convert.ToDouble(dur.Text);

                        DateTime startDate1 = startDate.ToUniversalTime();

                        startDate1 = startDate1.AddHours(Convert.ToDouble(dur.Text));

                        string startstring1 = startDate1.ToString("yyyy-MM-dd HH:mm:ss");
                        vals["stop_date"] = stopDate.ToString("yyyy-MM-dd");
                        vals["stop_datetime"] = startstring1;
                        vals["stop"] = startstring1;
                    }
                    catch
                    {
                        vals["duration"] = 0;

                        //vals["stop_date"] = stopDate.ToString("yyyy-MM-dd");
                        //vals["stop_datetime"] = convertstarttime;
                        //vals["stop"] = convertstarttime;

                    }
                    //  vals["duration"] = 0;
                }



                if (meeting_subject.Text == "")
                {
                    meeting_alert.IsVisible = true;
                    meeting_box.IsVisible = true;
                    // DisplayAlert("Alert", "Please check your stop date ", "Ok");
                }

                else if (check1.Checked == false && dur.Text=="")
                {
                    await DisplayAlert("Alert", "Please fill All day / Duration", "Ok");
                }

                else
                {
                    var updated = Controller.InstanceCreation().UpdateCRMOpporData("calendar.event", "create_calendar_event_app", vals);
                    if (updated == "true")
                    {
                        date_alert.IsVisible = false;
                        var currentpage = new LoadingAlert();
                        await PopupNavigation.PushAsync(currentpage);
                        App.Current.MainPage = new MasterPage(new CalendarPage());
                        await  DisplayAlert("Alert", "Created Successfull", "Ok");
                        Loadingalertcall();
                        //   Navigation.PopAllPopupAsync();
                    }
                    else
                    {
                        await DisplayAlert("Alert", "Please try again", "Ok");
                    }
                }
            }

            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>"+ex.Message);
            }
          
        }


        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        void cancel_Clicked(object sender, System.EventArgs e)
        {
            Loadingalertcall();
            Navigation.PopAllPopupAsync();
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#363E4B");
        }

        void update_cancel_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopAllPopupAsync();
        }

        void Handle_Update_Clicked(object sender, System.EventArgs e)
        {
            date_alert.IsVisible = false;
            date_box.IsVisible = false;
            meeting_alert.IsVisible = false;
            meeting_box.IsVisible = false;

            vals_update["attendees"] = atnIdsList;
            vals_update["tags"] = tagIdsList;
            vals_update["meeting_subject"] = meeting_subject.Text;

          //  DateTime startdateTime = DateTime.ParseExact(st_date.Date.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            String convertstartdate = st_date.Date.ToString("yyyy-MM-dd");

         //   string convertstartdate = startdateTime.ToString("yyyy-MM-dd");


            var dt = Convert.ToDateTime(st_poptime.Time.ToString());
            string convertstarttime = dt.ToUniversalTime().ToString("HH:mm:ss");
            TimeSpan start_timespan = TimeSpan.Parse(convertstarttime);
            string startstring = convertstartdate + " " + start_timespan;
            DateTime startDate = DateTime.ParseExact(startstring, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

           // DateTime stopdateTime = DateTime.ParseExact(stop_date.Date.ToString(), "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
           
            string convertstopdate = stop_date.Date.ToString("yyyy-MM-dd");

          //  string convertstopdate = stopdateTime.ToString("yyyy-MM-dd");

            var dt1 = Convert.ToDateTime(stop_poptime.Time.ToString());
            string convertstoptime = dt1.ToUniversalTime().ToString("HH:mm:ss");
            TimeSpan stop_timesspan = TimeSpan.Parse(convertstoptime);
            string stopstring = convertstopdate + " " + stop_timesspan;
            DateTime stopDate = DateTime.ParseExact(stopstring, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            vals_update["start_datetime"] = startstring;
            vals_update["stop_datetime"] = stopstring;
            vals_update["start"] = startstring;
            vals_update["stop"] = stopstring;
            vals["start_date"] = startDate.ToString("yyyy-MM-dd");

            vals_update["location"] = loc.Text.ToString();
            vals_update["description"] = des.Text;

            if (check1.Checked == true)
            {
                vals_update["allday"] = true;
                vals_update["duration"] = 0;

                vals_update["start_date"] = startstring;
                vals_update["stop_date"] = stopstring;
                vals_update["start"] = startstring;
                vals_update["stop"] = stopstring;
                vals["start_date"] = startDate.ToString("yyyy-MM-dd");

                if (stopDate.Date <= startDate.Date)
                {
                    date_alert.IsVisible = true;
                    date_box.IsVisible = true;
                    // DisplayAlert("Alert", "Please fill meeting subject", "Ok");
                }
            }

            else
            {
                vals_update["allday"] = false;
                try
                {
                    vals_update["duration"] = Convert.ToDouble(dur.Text);
                    DateTime startDate1 = startDate.AddHours(Convert.ToDouble(dur.Text));
                    string startstring1 = startDate1.ToString("yyyy-MM-dd HH:mm:ss");

                    vals_update["stop_datetime"] = startstring1;
                    vals_update["stop"] = startstring1;

                }
                catch
                {
                    vals_update["duration"] = 0;
                }
                //  vals["duration"] = 0;
            }


            if (meeting_subject.Text == "")
            {
                meeting_alert.IsVisible = true;
                meeting_box.IsVisible = true;
            }
            else
            {
                  var updated = Controller.InstanceCreation().UpdateCRMOpporData1("calendar.event", "update_calendar_event", updateId, vals_update);

                    if (updated == "true")
                    {

                      date_alert.IsVisible = false;
                        // App.Current.MainPage = new MasterPage(new CrmTabbedPage());
                        // Navigation.PushPopupAsync(new MasterPage(  );
                        DisplayAlert("Alert", "Created Successfull", "Ok");
                        Navigation.PopAllPopupAsync();
                        App.Current.MainPage = new MasterPage(new CalendarPage());
                    }

       
                    else
                    {
                        DisplayAlert("Alert", "Please try again", "Ok");
                    }

            }
        }


    }
}