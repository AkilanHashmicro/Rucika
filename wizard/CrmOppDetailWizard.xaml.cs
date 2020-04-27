using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.DBModel;
using SalesApp.models;
using SalesApp.OdooRpc;
using SalesApp.Pages;
using SalesApp.Persistance;
using SalesApp.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SalesApp.models.CRMModel;

namespace SalesApp.wizard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrmOppDetailWizard : PopupPage
    {
        CRMOpportunities obj = new CRMOpportunities();
        Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
        private OdooRPC odooConnector;


        String count = "";
        int priorityCnt = 0;
        int crm_opp_id = 0;

        CRMOpportunitiesDB dbobj = new CRMOpportunitiesDB();

        public CrmOppDetailWizard (CRMOpportunities modelObj)
		{
			InitializeComponent ();

            obj = modelObj;
            CustomerNameValue.Text = modelObj.customer;
            CrmNameValue.Text = modelObj.name;
            ExpectedRevenueValue.Text = modelObj.expected_revenue + "$";
            ProbabilityValue.Text = modelObj.probability + "%";
            EmailValue.Text = modelObj.email;
            PhoneValue.Text = modelObj.phone;
            TeamValue.Text = modelObj.team_name;
            ContactNameValue.Text = modelObj.contact_name;
            SalesPersonValue.Text = modelObj.sales_person;
            NextActValue.Text = modelObj.next_activity;
            expectedClosingValue.Text = modelObj.expected_closing;

            crm_opp_id = obj.id;

            //StreetValue.Text = modelObj.street;
            //Street2Value.Text = modelObj.street2;
            //CityValue.Text = modelObj.city;
            //CountryValue.Text = modelObj.country;
            //ContactNameValue.Text = modelObj.contact_name;
            //ContactMobileValue.Text = modelObj.mobile;
            int priorityCnt = Convert.ToInt32(modelObj.priority);

            if (priorityCnt == 3)
            { RatingLayout3.IsVisible = true; }
            else if (priorityCnt == 2)
            { RatingLayout2.IsVisible = true; }
            else if (priorityCnt == 1)
            {
                RatingLayout1.IsVisible = true;
            }
            else { }


            var lead_editbtnRecognizer = new TapGestureRecognizer();
            lead_editbtnRecognizer.Tapped += (s, e) =>
            {
                updateStackLayout.IsVisible = true;
                lead_editbtn.IsVisible = false;

                crm_name_layout.IsVisible = true;
                CrmNameValue.IsVisible = false;
                crm_name_entry.Text = CrmNameValue.Text;

                ex_rev_entry.IsVisible = true;
                ex_rev_entry.Text = obj.expected_revenue;
                ExpectedRevenueValue.IsVisible = false;

                prob_entry.IsVisible = true;
                prob_entry.Text = obj.probability.ToString();
                ProbabilityValue.IsVisible = false;

                contactname_entry.IsVisible = true;
                contactname_entry.Text = ContactNameValue.Text;
                ContactNameValue.IsVisible = false;

                sales_person_picker.IsVisible = true;
                sales_person_picker.ItemsSource = App.salespersons.Select(x => x.Value).ToList();
                sales_person_picker.SelectedIndex = 0;
                sales_person_picker.SelectedItem = SalesPersonValue.Text;
                SalesPersonValue.IsVisible = false;

                cus_picker.IsVisible = true;
                cus_picker.ItemsSource = App.cusList.Select(x => x.name).ToList();
                cus_picker.SelectedIndex = 0;
              //  cus_picker.SelectedItem = CustomerNameValue.Text;
                CustomerNameValue.IsVisible = false;

                nextact_picker.IsVisible = true;
                nextact_picker.ItemsSource = App.nextActivityList.Select(x => x.name).ToList();
                nextact_picker.SelectedItem = NextActValue.Text;
                NextActValue.IsVisible = false;

                expe_closing_picker.IsVisible = true;

                try
                {
                    DateTime expclosingDate = DateTime.ParseExact(modelObj.expected_closing, "yyyy-MM-dd", null);
                    expectedClosingValue.IsVisible = false;
                    expe_closing_picker.Date = expclosingDate;
                }

                catch
                {
                    expectedClosingValue.IsVisible = false;
                    expe_closing_picker.Date = DateTime.Today;
                }
                // expe_closing_picker.ItemsSource = App.nextActivityList.Select(x => x.name).ToList();

                NextActValue.IsVisible = false;

                email_entry.IsVisible = true;
                email_entry.Text = EmailValue.Text;
                EmailValue.IsVisible = false;

                phone_entry.IsVisible = true;
                phone_entry.Text = PhoneValue.Text;
                PhoneValue.IsVisible = false;

                team_picker.IsVisible = true;
                team_picker.ItemsSource = App.salesteam.Select(x => x.Value).ToList();
                team_picker.SelectedIndex = 0;
                team_picker.SelectedItem = TeamValue.Text;
                TeamValue.IsVisible = false;


                RatingLayout3.IsVisible = false;
                RatingLayout2.IsVisible = false;
                RatingLayout1.IsVisible = false;

                emptyRatingLayout3.IsVisible = true;


            };
            lead_editbtn.GestureRecognizers.Add(lead_editbtnRecognizer);



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


        public CrmOppDetailWizard(CRMOpportunitiesDB modelObj)
        {
            InitializeComponent();
           
            dbobj = modelObj;

            //if (modelObj.yellowimg_string == "yellowcircle.png")
            //{
            //    synclayout.IsVisible = true;
            //}


            CustomerNameValue.Text = modelObj.name;
            CrmNameValue.Text = modelObj.customer;
            ExpectedRevenueValue.Text = modelObj.expected_revenue + "$";
            ProbabilityValue.Text = modelObj.probability + "%";
            EmailValue.Text = modelObj.email;
            PhoneValue.Text = modelObj.phone;
            TeamValue.Text = modelObj.team_name;
            NextActValue.Text = modelObj.next_activity;
            NextActValue.Text = modelObj.next_activity;
            NextActValue.Text = modelObj.title_action;
            expectedClosingValue.Text = "";
            //StreetValue.Text = modelObj.street;
            //Street2Value.Text = modelObj.street2;
            //CityValue.Text = modelObj.city;
            //CountryValue.Text = modelObj.country;
            //ContactNameValue.Text = modelObj.contact_name;
            //ContactMobileValue.Text = modelObj.mobile;
            int priorityCnt = Convert.ToInt32(modelObj.priority);

            if (priorityCnt == 3)
            { RatingLayout3.IsVisible = true; }
            else if (priorityCnt == 2)
            { RatingLayout2.IsVisible = true; }
            else if (priorityCnt == 1)
            {
                RatingLayout1.IsVisible = true;
            }
            else { }
        }

        private async void ButtonMarkWon_ClickedAsync(object sender, EventArgs e)
        {
           // List<CRMLead> crmLeadData1 = Controller.InstanceCreation().crmLeadData();
                
            if (App.NetAvailable == true)
            {
                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);

                bool updated = Controller.InstanceCreation().UpdateMarkWon(obj.id);

                if (updated)
                {
                    await DisplayAlert("Alert", "State successfully moved", "Ok");

                    //  List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();
                    App.oppo_rpc = true;
                    App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab2"));
                    Loadingalertcall();
                }

                else
                {
                    await DisplayAlert("Alert", "Please try again", "Ok");
                    Loadingalertcall();
                }

            }
            else
            {
                await DisplayAlert("Alert", "Internet Connection Needed", "Ok");
            }
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        private async void ButtonMarkLost_ClickedAsync(object sender, EventArgs e)
        {
            // Navigation.PushPopupAsync(new MarkLostPopupPage());
           // List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();

            if (App.NetAvailable == true)
            {
                await PopupNavigation.PushAsync(new MarkLostPopupPage(obj.id));
            }

            else
            {
                await DisplayAlert("Alert", "Internet Connection Needed", "Ok");
            }
        }

        private async void ButtonNewQuotation_ClickedAsync(object sender, EventArgs e)
        {
           // List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();


            if (App.NetAvailable == true)
            {
               await Navigation.PushPopupAsync(new SalesQuotationCreationPage());
                MessagingCenter.Send<string, string>("MyApp", "NotifyMsg", "From CRM Opp");
            }

            else
            {
                await DisplayAlert("Alert", "Internet Connection Needed", "Ok");
            }
           
        }


        private async void updateClickedAsync(object sender, EventArgs e)
        {
            vals["planned_revenue"] = ex_rev_entry.Text;
            vals["probability"] = prob_entry.Text;
            vals["name"] = crm_name_entry.Text;

            vals["contact_name"] = contactname_entry.Text;

            vals["email_from"] = email_entry.Text;

            vals["phone"] = phone_entry.Text;


            var salesteamid = App.salesteam.FirstOrDefault(x => x.Value == team_picker.SelectedItem.ToString()).Key;
            vals["team_id"] = salesteamid;


            try
            {
                var salespersonid = App.salespersons.FirstOrDefault(x => x.Value == sales_person_picker.SelectedItem.ToString()).Key;
                vals["partner_id"] = salespersonid;
            }
            catch
            {
                vals["partner_id"] = false;
            }

            var customerid = App.cusList.FirstOrDefault(x => x.name == cus_picker.SelectedItem.ToString()).id;

            vals["partner_id"] = customerid;

            vals["partner_name"] = cus_picker.SelectedItem.ToString();

            int next_activity_id = 0;

            var nextactivityid =
                    (
                        from i in App.nextActivityList
                    where i.name == nextact_picker.SelectedItem.ToString()
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

            String date_deadline_string = String.Format("{0:MM-dd-yyyy HH:mm:ss}", expe_closing_picker.Date);

            vals["date_deadline"] = date_deadline_string;

            vals["planned_revenue"] = ex_rev_entry.Text;

            vals["probability"] = prob_entry.Text;

           // vals["description"] = activty_des.Text;

            vals["type"] = "opportunity";

            vals["priority"] = count;

            //if (CrmNameValue.Text == "")
            //{
            //    leadtitle_alert.IsVisible = true;
            //}

           if (Convert.ToInt32(prob_entry.Text) > 100)
            {               
                prob_alert.IsVisible = true;
            }

            else
            {
                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);
                //  string updated = Controller.InstanceCreation().UpdateLeadCreationData("crm.lead", "create", vals_update);

                bool updated = Controller.InstanceCreation().UpdateSaleOrder("crm.lead", "write", crm_opp_id, vals);

                if (updated == true)
                {
                    App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab2"));
                         
                }

                else
                {
                  //  Loadingalertcall(); 
                    await DisplayAlert("Alert", "Please try again", "Ok");
                }
                             
            }

            Loadingalertcall();  


        }

        private async void cancelClickedAsync(object sender, EventArgs e)
        {
            crm_name_layout.IsVisible = false;
            CrmNameValue.IsVisible = true;

            ExpectedRevenueValue.IsVisible = true;
            ex_rev_entry.IsVisible = false;

            ProbabilityValue.IsVisible = true;
            prob_entry.IsVisible = false;

            //CustomerNameValue.IsVisible = true;
            //cus_picker.IsVisible = false;

            EmailValue.IsVisible = true;
            email_entry.IsVisible = false;

            PhoneValue.IsVisible = true;
            phone_entry.IsVisible = false;

            TeamValue.IsVisible = true;
            team_picker.IsVisible = false;

            SalesPersonValue.IsVisible = true;
            sales_person_picker.IsVisible = false;

            ContactNameValue.IsVisible = true;
            contactname_entry.IsVisible = false;

            NextActValue.IsVisible = true;
            nextact_picker.IsVisible = false;

            expectedClosingValue.IsVisible = true;
            expe_closing_picker.IsVisible = false;



            //NextActivityValue.IsVisible = true;
            //next_activity_picker.IsVisible = false;

            //NextActivityDateValue.IsVisible = true;
            //next_activitydate_picker.IsVisible = false;

            //ActivityDescritionValue.IsVisible = true;
            //activty_des.IsVisible = false;

            //expectedClosingValue.IsVisible = true;
            //expe_closing_picker.IsVisible = false;

            emptyRatingLayout3.IsVisible = false;

            priorityCnt = Convert.ToInt32(obj.priority);

            if (priorityCnt == 3)
            { RatingLayout3.IsVisible = true; }
            else if (priorityCnt == 2)
            { RatingLayout2.IsVisible = true; }
            else if (priorityCnt == 1)
            {
                RatingLayout1.IsVisible = true;
            }
            else { }

            updateStackLayout.IsVisible = false;
            lead_editbtn.IsVisible = true;

        }

        //private void syncbtn_Clicked(object sender, EventArgs e)
        //{


        //    try
        //    {
        //        List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();
        //    }


        //    catch (Exception ea)
        //    {
        //        if (ea.Message.Contains("(Network is unreachable)") || ea.Message.Contains("NameResolutionFailure"))
        //        {
        //            App.NetAvailable = false;
        //        }

        //        else if (ea.Message.Contains("(503) Service Unavailable"))
        //        {
        //            App.responseState = false;
        //        }
        //    }

        // //   var cusid = App.cusList.FirstOrDefault(x => x.Value == cuspicker1.SelectedItem.ToString()).Key;
        //    vals["customer"] = dbobj.cusid;
        //    vals["oppurtunity_title"] = dbobj.oppurtunity_title;
        //    vals["email"] = dbobj.email;
        //    vals["phone"] = dbobj.phone;


        //    // string nextactdatepicker_date = startdateTime.ToString("yyyy-MM-dd");

        //    // vals["next_activity_date"] = nextActDatePicker.Date.ToString();

        //    vals["next_activity_date"] = dbobj.next_activity_date;
        //    vals["next_activity"] = dbobj.next_activity;
        //    vals["next_activity_summary"] = dbobj.next_activity_summary;

        //    //if (exRev.Text == "")
        //    //{
        //    //    exRev.Text = "0";
        //    //}
        //    vals["expected_revenue"] = dbobj.expected_revenue;

        //    vals["rating"] = dbobj.rating;
        //    vals["stage"] = dbobj.stage;
        //    vals["internal_notes"] = dbobj.internal_notes;

        //    vals["state"] = "new";


        //    List<MeetingLinesList> meetingLineList = new List<MeetingLinesList>();

        //    meetingLineList=   JsonConvert.DeserializeObject<List<MeetingLinesList>>(dbobj.meetings);

        //    vals["meetings"] = meetingLineList;

        //    if (App.NetAvailable == true)
        //    {

        //        //bool updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_crm_quotations", vals);
        //        string updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_crm_quotations", vals);

        //        if (updated == "true")
        //        {
        //            // App.Current.MainPage = new MasterPage(new CrmTabbedPage());
        //            // Navigation.PushPopupAsync(new MasterPage(  );
                  


        //            App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();

        //            try
        //            {
        //                App._connection.Query<CRMOpportunitiesDB>("UPDATE CRMOpportunitiesDB set yellowimg_string=? Where Dbid=?", "", dbobj.Dbid);

        //                App._connection.CreateTable<CRMOpportunitiesDB>();
        //                var details = (from y in App._connection.Table<CRMOpportunitiesDB>() select y).ToList();
        //                App.CRMOpportunitiesListDb = details;


        //            DisplayAlert("Alert", "Created Successfull", "Ok");
        //            Navigation.PopAllPopupAsync();
        //            App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab2"));
                

        //            }

        //            catch (Exception ex)
        //            {

        //            }


        //        }
        //        else
        //        {
        //            DisplayAlert("Alert", "Please try again", "Ok");
        //        }

        //    }

        //    else
        //    {
        //        DisplayAlert("Alert", "Check your internet connection", "Ok");
        //    }


        //}

    }
}