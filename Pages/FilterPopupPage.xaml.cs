using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.models;
using SalesApp.views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SalesApp.models.CRMModel;

namespace SalesApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopupPage : PopupPage
    {
       // String filterstring = "Month";
        DateTime start_dateTime = new DateTime();
        DateTime end_dateTime = new DateTime();

        String tab_string = "";

       // Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

        void cancel_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopAllPopupAsync();
        }

        async void save_ClickedAsync(object sender, System.EventArgs e)
        {           

           

            DateTime startdateTime;
            DateTime stopdateTime;

           // DisplayAlert("Alert", start_datepicker.Date.ToString(), "Ok");

            String startdatestring =   String.Format("{0:MM-dd-yyyy HH:mm:ss}", start_datepicker.Date);

            startdateTime = DateTime.ParseExact(startdatestring, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

           
            string start_datestring = startdateTime.ToString("yyyy-MM-dd");


            String stopdatestring = String.Format("{0:MM-dd-yyyy HH:mm:ss}", end_datepicker.Date);

            stopdateTime = DateTime.ParseExact(stopdatestring, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

           
  


            string end_datestring = stopdateTime.ToString("yyyy-MM-dd");


            string combinestring = start_datestring + " " + end_datestring;

            DateTime startDate = DateTime.ParseExact(start_datestring, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(end_datestring, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            //if (endDate.Date <= startDate.Date)
            //{
            //    date_alert.IsVisible = true;
            //    date_box.IsVisible = true;
            //    // DisplayAlert("Alert", "Please fill meeting subject", "Ok");
            //    mainlayout.HeightRequest = 380;
            //}

            if (App.filterstring != "Days" && App.filterstring != "Month")
            {
                App.filterdict["range"] = combinestring;
                App.filterdict["days"] = "False";
                App.filterdict["month"] = "False";

                if (endDate.Date <= startDate.Date)
                {
                    date_alert.IsVisible = true;
                    date_box.IsVisible = true;
                    // DisplayAlert("Alert", "Please fill meeting subject", "Ok");
                    mainlayout.HeightRequest = 380;
                  //  await Navigation.PopAsync();

                }
                else
                {
                    var currentpage = new LoadingAlert();
                    await PopupNavigation.PushAsync(currentpage);

                    if (tab_string == "tab1")
                    {
                        App.lead_filter = true;
                        App.lead_rpc = true;
                    }
                    else if (tab_string == "tab2")
                    {
                        App.opp_filter = true;
                        App.oppo_rpc = true;
                    }

                    else if (tab_string == "tab3")
                    {
                        App.dq_filter = true;
                        App.draftquot_rpc = true;
                    }

                    else if(tab_string == "tab4")
                    {
                        App.sq_filter = true;
                        App.salequot_rpc = true;
                    }

                    else 
                    {
                        App.so_filter = true;
                        App.so_rpc = true;

                    }

                    App.Current.MainPage = new MasterPage(new CrmTabbedPage(tab_string));

                   await Navigation.PopAllPopupAsync();
                }
             }

            else
            {
                if (App.filterstring == "Days")
                {
                    App.filterdict["range"] = "False";
                    App.filterdict["days"] = "True";
                    App.filterdict["month"] = "False";
                }

                else
                {
                    App.filterdict["range"] = "False";
                    App.filterdict["days"] = "False";
                    App.filterdict["month"] = "True";
                }

                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);

                if (tab_string == "tab1")
                {
                    App.lead_filter = true;
                    App.lead_rpc = true;
                }
                else if (tab_string == "tab2")
                {
                    App.opp_filter = true;
                    App.oppo_rpc = true;
                }

                else if (tab_string == "tab3")
                {
                    App.dq_filter = true;
                    App.draftquot_rpc = true;
                }

                else if (tab_string == "tab4")
                {
                    App.sq_filter = true;
                    App.salequot_rpc = true;
                }

                else
                {
                    App.so_filter = true;
                    App.so_rpc = true;

                }

                App.Current.MainPage = new MasterPage(new CrmTabbedPage(tab_string));


                Loadingalertcall();

            }
   
            async void Loadingalertcall()
            {
                await PopupNavigation.PopAllAsync();
            }
        }

        public FilterPopupPage(string tab)
        {
            InitializeComponent();

            tab_string = tab;

            if(App.filterstring == "Month")
            {
                monthfillimg.IsVisible = true;
                monthempimg.IsVisible = false;
                daysfillimg.IsVisible = false;
                datefillimg.IsVisible = false;
            }

            else if (App.filterstring == "Days")
            {
                monthfillimg.IsVisible = false;
                daysfillimg.IsVisible = true;
                daysempimg.IsVisible = false;
                datefillimg.IsVisible = false;
            }

            else if (App.filterstring == "Range")
            {
                monthfillimg.IsVisible = false;
                daysfillimg.IsVisible = false;
                dateempimg.IsVisible = false;
                datefillimg.IsVisible = true;

                startdatepickerGrid.IsVisible = true;
                enddatepickerGrid.IsVisible = true;
                mainlayout.HeightRequest = 350;
            }
                                               
            var daysfillimgRecognizer = new TapGestureRecognizer();
            daysfillimgRecognizer.Tapped += (s, e) => {
                monthfillimg.IsVisible = false;
                datefillimg.IsVisible = false;
                daysfillimg.IsVisible = true;
                daysempimg.IsVisible = false;
                mainlayout.HeightRequest = 190;
                startdatepickerGrid.IsVisible = false;
                enddatepickerGrid.IsVisible = false;
                monthempimg.IsVisible = true;
                dateempimg.IsVisible = true;
                App.filterstring = "Days";

                date_alert.IsVisible = false;
                date_box.IsVisible = false;
              //  MessagingCenter.Send<string, string>("MyApp", "NotifyMsg", filterstring);

            };
            daysempimg.GestureRecognizers.Add(daysfillimgRecognizer);

            var monthfillimgRecognizer = new TapGestureRecognizer();
            monthfillimgRecognizer.Tapped += (s, e) => {
                daysfillimg.IsVisible = false;
                datefillimg.IsVisible = false;
                monthfillimg.IsVisible = true;
                monthempimg.IsVisible = false;
                mainlayout.HeightRequest = 190;
                startdatepickerGrid.IsVisible = false;
                enddatepickerGrid.IsVisible = false;
                daysempimg.IsVisible = true;
                dateempimg.IsVisible = true;

                App.filterstring = "Month";

                date_alert.IsVisible = false;
                date_box.IsVisible = false;

              //  MessagingCenter.Send<string, string>("MyApp", "NotifyMsg", filterstring);

            };
            monthempimg.GestureRecognizers.Add(monthfillimgRecognizer);

            var datefillimgRecognizer = new TapGestureRecognizer();
            datefillimgRecognizer.Tapped += (s, e) => {
                daysfillimg.IsVisible = false;
                monthfillimg.IsVisible = false;
                datefillimg.IsVisible = true;
                dateempimg.IsVisible = false;
                startdatepickerGrid.IsVisible = true;
                enddatepickerGrid.IsVisible = true;
                mainlayout.HeightRequest = 350;
                daysempimg.IsVisible = true;
                monthempimg.IsVisible = true;

               App.filterstring = "Range";
                date_alert.IsVisible = false;
                date_box.IsVisible = false;
              
            //    MessagingCenter.Send<string, string>("MyApp", "NotifyMsg", filterstring);
            };
            dateempimg.GestureRecognizers.Add(datefillimgRecognizer);

        }
    }
}