using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SalesApp.Pages;
using Rg.Plugins.Popup.Extensions;
using SalesApp.models;
using Newtonsoft.Json.Linq;
using static SalesApp.models.CRMModel;
using Rg.Plugins.Popup.Services;
using System.Globalization;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        List<all_events> calresult = new List<all_events>();

        List<all_events> temp_result = new List<all_events>();

        MonthViewSettings monthViewSettings = new MonthViewSettings();

        public CalendarPage()
        {
            InitializeComponent();


            calendar.MonthViewSettings = monthViewSettings;

            calendar.HeightRequest = 300;

            monthViewSettings.DateSelectionColor = Color.Gray;

                monthViewSettings.SelectedDayTextColor = Color.FromHex("#363E4B");
            monthViewSettings.BlackoutColor = Color.Gray;
            monthViewSettings.CurrentMonthTextColor = Color.FromHex("#ffffff");
            monthViewSettings.PreviousMonthTextColor = Color.Black;
          //  monthViewSettings.CurrentMonthTextColor = Color.FromHex("#128197");
            monthViewSettings.CurrentMonthBackgroundColor = Color.FromHex("#128197");
         //   monthViewSettings.PreviousMonthBackgroundColor = Color.White;

            monthViewSettings.PreviousMonthBackgroundColor = Color.FromHex("#363E4B");


            monthViewSettings.SelectedDayTextColor = Color.GreenYellow;
            monthViewSettings.DayLabelTextAlignment = DayLabelTextAlignment.Center;

            calendar.MonthViewSettings = monthViewSettings;

            calresult = Controller.InstanceCreation().GetCalenderData(DateTime.Now.Month, DateTime.Now.Year);

            temp_result = calresult;

            CalendarEventCollection appointments = new CalendarEventCollection();

            List<string> show_meeting_days1 = new List<string>();
            DateTime dateTime = new DateTime();

            foreach (all_events cal in calresult)
            {

                //for (int i = 0; i < cal.meeting_days.Count; i++)                 //{                  //    string md = cal.meeting_days[i];                 //    //   DateTime dateTime = DateTime.ParseExact(md, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);                  //      string mdfin = Convert.ToDateTime(md).ToLocalTime().ToString();


                //    try
                //    {
                //        dateTime = DateTime.ParseExact(mdfin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //    }

                //    catch
                //    {
                //        try
                //        {
                //            dateTime = DateTime.ParseExact(mdfin, "M/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //        }

                //        catch
                //        {
                //            try
                //            {
                //                dateTime = DateTime.ParseExact(mdfin, "MM/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                //            }

                //            catch
                //            {
                //                try
                //                {
                //                    dateTime = DateTime.ParseExact(mdfin, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                //                }

                //                catch
                //                {
                //                    dateTime = DateTime.ParseExact(mdfin, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                //                }

                //            }
                //        }
                //    }

                //        string convertstartdate = dateTime.ToString("yyyy-MM-dd");

                //        show_meeting_days1.Add(mdfin);
                                     //} 
          
                if (cal.start == "")
                {
                    continue;
                }

                try
                {
                    if (cal.allday)
                    {
                        string startTime = cal.start;
                        string stopTime = cal.stop;

                        foreach (string dt in cal.meeting_days)
                        {

                            appointments.Add(new CalendarInlineEvent()
                            {
                                StartTime = Convert.ToDateTime(dt).ToLocalTime(),
                                EndTime = Convert.ToDateTime(dt).ToLocalTime(),
                                Subject = cal.meeting_subject.ToString(),
                                // Color = Color.Blue,
                                Color = Color.FromHex("#FDA701"),
                            });
                        }

                    }

                    else
                    {
                        appointments.Add(new CalendarInlineEvent()
                        {
                            StartTime = Convert.ToDateTime(cal.start.ToString()).ToLocalTime(),
                            EndTime = Convert.ToDateTime(cal.start.ToString()).ToLocalTime(),
                            Subject = cal.meeting_subject.ToString(),
                            Color = Color.White,
                        });

                        string StartTime2 = Convert.ToDateTime(cal.start.ToString()).ToLocalTime().ToString();
                    }
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine("" + exc.Message);
                }
            }

            List<all_events> selectedCalDate = calresult.Where((cal) =>
            {
               // return cal.start.Contains(DateTime.Today.ToString("yyyy-MM-dd"));
                return cal.StartAt.Contains(DateTime.Today.ToString("yyyy-MM-dd"));
            }
            ).ToList<all_events>();

            CalendarListView.ItemsSource = selectedCalDate;

            calendar.DataSource = appointments;
            CalendarListView.ItemsSource = calresult;

        }

        private void Toolbar_Filter_Activated(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new CalendarPopupPage());
        }

        private void Toolbar_Search_Activated(object sender, EventArgs e)
        {

        }

        private void ViewCellPending_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.White;
        }

        private void CalendarListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            all_events modelObj = e.Item as all_events;
          //  Navigation.PushAsync(new CalendarDetailPage(modelObj));
            Navigation.PushPopupAsync(new CalendarDetailPage(modelObj));
        }

        private void Calendar_OnCalendarTapped(object sender, CalendarTappedEventArgs args)
        {

            var dateselected = calendar.SelectedDate.Date.ToString("yyyy-MM-dd");                 App.calselecteddate = dateselected; 
            try
            {

                List<all_events> selectedCalDate = calresult.Where((cal) =>
                {
                    return cal.show_meeting_days.Contains(dateselected);
                //try
                //{

                //    return cal.ContainsLoop(cal.meeting_days, dateselected);
                //}
                //catch(Exception ex)
                //{
                //    System.Diagnostics.Debug.WriteLine(">>>>>>>" + ex.Message);
                //}

                //return cal.ContainsLoop(cal.meeting_days, dateselected);

            }).ToList<all_events>();

                CalendarListView.ItemsSource = selectedCalDate;

                SelectDate.Text = calendar.SelectedDate.Date.ToString("dd MMM");

            }

            catch( Exception e)
            {
                int j = 0;
                   
            }             
        }

        private async void calendar_MonthChangedAsync(object sender, MonthChangedEventArgs args)
        {

            var currentpage = new LoadingAlert();

            await PopupNavigation.PushAsync(currentpage);

            DateTime dt = args.args.CurrentValue;

            calresult = new List<all_events>();

            calresult = Controller.InstanceCreation().GetCalenderData(dt.Month, dt.Year);

            CalendarEventCollection appointments = new CalendarEventCollection();

            foreach (all_events cal in calresult)
            {
                if (cal.start == "")
                {
                    continue;
                }

                try
                {
                    if (cal.allday)
                    {
                        string startTime = cal.start;
                        string stopTime = cal.stop;

                        foreach (string data in cal.meeting_days)
                        {
                            appointments.Add(new CalendarInlineEvent()
                            {
                                StartTime = Convert.ToDateTime(data).ToLocalTime(),
                                EndTime = Convert.ToDateTime(data).ToLocalTime(),
                                Subject = cal.meeting_subject.ToString(),
                                Color = Color.FromHex("#FDA701"),
                            });
                        }
                    }

                    else
                    {
                        appointments.Add(new CalendarInlineEvent()
                        {
                            StartTime = Convert.ToDateTime(cal.start.ToString()).ToLocalTime(),
                            EndTime = Convert.ToDateTime(cal.start.ToString()).ToLocalTime(),
                            Subject = cal.meeting_subject.ToString(),
                            Color = Color.White,
                        });
                    }
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine("" + exc.Message);
                }

            }

            List<all_events> selectedCalDate = calresult.Where((cal) =>
            {
                return cal.start.Contains(DateTime.Today.ToString("yyyy-MM-dd"));
            }
            ).ToList<all_events>();

            CalendarListView.ItemsSource = selectedCalDate;

            calendar.DataSource = appointments;
            CalendarListView.ItemsSource = calresult;

            Loadingalertcall();
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }
    }
}
