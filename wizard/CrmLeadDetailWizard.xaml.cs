using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.DBModel;
using SalesApp.models;
using SalesApp.Pages;
using SalesApp.Persistance;
using SalesApp.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs;
using static SalesApp.models.CRMModel;

namespace SalesApp.wizard
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrmLeadDetailWizard : PopupPage
	{
       
        Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
        Dictionary<string, dynamic> vals_update = new Dictionary<string, dynamic>();
        CRMLead obj = new CRMLead();
        CRMLeadDB dbobj = new CRMLeadDB();

        String count = "";
        int priorityCnt = 0;
        int crm_lead_id = 0;

        public CrmLeadDetailWizard (CRMLead modelObj,string page)
		{
			InitializeComponent ();

            obj = modelObj;              
           // CustomerNameValue.Text = modelObj.customer;
            CrmNameValue.Text = modelObj.name;
            //ExpectedRevenueValue.Text = modelObj.expected_revenue + "$";
            //ProbabilityValue.Text = modelObj.probability + "%";
            EmailValue.Text = modelObj.email;
            PhoneValue.Text = modelObj.phone;
            TeamValue.Text = modelObj.team_name;
            ContactNameValue.Text = modelObj.contact_name;
            SalesPersonValue.Text = modelObj.sales_person;

            //NextActValue.Text = modelObj.next_activity;
            //expectedClosingValue.Text = modelObj.expected_closing;

          // SalesPersonValue.Text = modelObj.
          //  SalesPersonValue.Text = modelObj.next_activity;
           // NextActivityDateValue.Text = modelObj.next_activity;
            //ActivityDescritionValue.Text = modelObj.title_action;
          //  expectedClosingValue.Text = "";
            //StreetValue.Text = modelObj.street;
            //Street2Value.Text = modelObj.street2;
            //CityValue.Text = modelObj.city;
            //CountryValue.Text = modelObj.country;
            //ContactNameValue.Text = modelObj.contact_name;
            //ContactMobileValue.Text = modelObj.mobile;

            crm_lead_id = obj.id;
            int priorityCnt = Convert.ToInt32(modelObj.priority);

            if(page =="Lead")
            {
               // btnStack.IsVisible = false;
                Convertbtn.IsVisible = true;
            }

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
                    
                //ex_rev_entry.IsVisible = true;
                //ex_rev_entry.Text = obj.expected_revenue;
                //ExpectedRevenueValue.IsVisible = false;

                //prob_entry.IsVisible = true;
                //prob_entry.Text = obj.probability.ToString();
                //ProbabilityValue.IsVisible = false;


                contactname_entry.IsVisible = true;
                contactname_entry.Text = ContactNameValue.Text;
                ContactNameValue.IsVisible = false;

                sales_person_picker.IsVisible = true;               
                sales_person_picker.ItemsSource = App.salespersons.Select(x => x.Value).ToList();
                sales_person_picker.SelectedIndex = 0;
                sales_person_picker.SelectedItem = SalesPersonValue.Text;
                SalesPersonValue.IsVisible = false;

                //cus_picker.IsVisible = true;
                //cus_picker.ItemsSource = App.cusdict.Select(x => x.Value).ToList();

                // int index =   App.cusdict.Values.ToList().IndexOf(CustomerNameValue.Text);


                //int index = 0;
                //for (int i = 0; i < cus_picker.ItemsSource.Count;i++)
                //{

                //    System.Diagnostics.Debug.WriteLine("IDDDDD>>>>" + cus_picker.ItemsSource[i]);
                //    if(CustomerNameValue.Text == cus_picker.ItemsSource[i])
                //    {

                //        index = i;
                //    }
                //}

               // cus_picker.SelectedIndex = 0;
               ////  cus_picker.SelectedIndex = index;
               //cus_picker.SelectedItem = CustomerNameValue.Text;
                //CustomerNameValue.IsVisible = false;

                //nextact_picker.IsVisible = true;
                //nextact_picker.ItemsSource = App.nextActivityList.Select(x => x.name).ToList();               
                //nextact_picker.SelectedItem = NextActValue.Text;
                //NextActValue.IsVisible = false;

                //expe_closing_picker.IsVisible = true;

                //try
                //{
                //    DateTime expclosingDate = DateTime.ParseExact(modelObj.expected_closing, "yyyy-MM-dd", null);
                //    expectedClosingValue.IsVisible = false;
                //    expe_closing_picker.Date = expclosingDate;
                //}

                //catch
                //{
                //    expectedClosingValue.IsVisible = false;
                //    expe_closing_picker.Date = DateTime.Today;
                //}
                //// expe_closing_picker.ItemsSource = App.nextActivityList.Select(x => x.name).ToList();

                //NextActValue.IsVisible = false;

                email_entry.IsVisible = true;
                email_entry.Text = EmailValue.Text;
                EmailValue.IsVisible = false;

                phone_entry.IsVisible = true;
                phone_entry.Text = PhoneValue.Text;
                PhoneValue.IsVisible = false;

                team_picker.IsVisible = true;
                team_picker.ItemsSource =  App.salesteam.Select(x => x.Value).ToList();
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


        public CrmLeadDetailWizard(CRMLeadDB modelObj, string page)
        {
            InitializeComponent();
           // CRMLeadDB obj = new CRMLeadDB();

            //if(modelObj.serverupdate_string!=null)
            //{
            //    synclayout.IsVisible = true;
            //}

            dbobj = modelObj;
         //   CustomerNameValue.Text = modelObj.name;
            CrmNameValue.Text = modelObj.customer;
            //ExpectedRevenueValue.Text = modelObj.expected_revenue + "$";
            //ProbabilityValue.Text = modelObj.probability + "%";
            EmailValue.Text = modelObj.email;
            PhoneValue.Text = modelObj.phone;
            TeamValue.Text = modelObj.team_name;
            //NextActivityValue.Text = modelObj.next_activity;
            //NextActivityDateValue.Text = modelObj.next_activity;
          //  ActivityDescritionValue.Text = modelObj.title_action;
          //  expectedClosingValue.Text = "";
            //StreetValue.Text = modelObj.street;
            //Street2Value.Text = modelObj.street2;
            //CityValue.Text = modelObj.city;
            //CountryValue.Text = modelObj.country;
            //ContactNameValue.Text = modelObj.contact_name;
            //ContactMobileValue.Text = modelObj.mobile;
            priorityCnt = Convert.ToInt32(modelObj.priority);

            //if(modelObj.yellowimg_string=="")
            //{
            //    synclayout.IsVisible = false;
            //}
            crm_lead_id = dbobj.id;

            if (page == "Lead")
            {
              //  btnStack.IsVisible = false;
                Convertbtn.IsVisible = true;
            }

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

        private void Convertbtn_Clicked(object sender, EventArgs e)
        {
            
                vals["type"] = "opportunity";
                string updated = Controller.InstanceCreation().UpdateLeadCreationConvertData("crm.lead", "write", obj.id, vals);

                if (App.NetAvailable == false)
                {
                    DisplayAlert("Alert", "Need Internet Connection", "Ok");
                }

                else
                {
                   if (updated == "Odoo Error")
                    {
                        // App.Current.MainPage = new MasterPage(new CrmTabbedPage());
                        // Navigation.PushPopupAsync(new MasterPage(  );
                        DisplayAlert("Alert", "Please try again", "Ok");
                    }
                    else
                    {

                        DisplayAlert("Alert", "Created Successfull", "Ok");
                        Navigation.PopAllPopupAsync();

                    }
                }
           

          //  Navigation.PushPopupAsync(new ConvertToOpportunitiesPage());
        }

        private async Task ButtonMarkWon_ClickedAsync(object sender, EventArgs e)
        {
            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);

            bool updated = Controller.InstanceCreation().UpdateMarkWon(obj.id);

            if (updated)
            {      
               await DisplayAlert("Alert", "State successfully moved", "Ok");

                //   List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();
                App.lead_rpc = true;
               App.Current.MainPage = new MasterPage(new CrmTabbedPage("tab1"));
               Loadingalertcall();               
            }

            else
            {
                await  DisplayAlert("Alert", "Please try again", "Ok");

                Loadingalertcall();
            }

        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        private void ButtonMarkLost_Clicked(object sender, EventArgs e)
        {
           // Navigation.PushPopupAsync(new MarkLostPopupPage());
         //   List<CRMLead> crmLeadData = Controller.InstanceCreation().crmLeadData();
            PopupNavigation.PushAsync(new MarkLostPopupPage(obj.id));
        }

        private void ButtonNewQuotation_Clicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new SalesQuotationCreationPage());
            MessagingCenter.Send<string, string>("MyApp", "NotifyMsg", "From CRM Lead");
        }


        private async void updateClickedAsync(object sender, EventArgs e)
        {


            //int stateid = 0;
            //int countryid = 0;

            //vals["planned_revenue"] = ex_rev_entry.Text;
            //vals["probability"] = prob_entry.Text;
            vals["name"] = crm_name_entry.Text;

            vals["contact_name"] = contactname_entry.Text;

            vals["email_from"] = email_entry.Text;

            vals["phone"] = phone_entry.Text;

            //vals["function"] = jobPos.Text;
            //vals["street"] = street.Text;
            //vals["street2"] = street2.Text;
            //vals["zip"] = zip.Text;

            //var salespersonid = App.salespersons.FirstOrDefault(x => x.Value == salesperson_Picker.SelectedItem.ToString()).Key;
            //vals["user_id"] = salespersonid;

            var salesteamid = App.salesteam.FirstOrDefault(x => x.Value == team_picker.SelectedItem.ToString()).Key;
            vals["team_id"] = salesteamid;

            var salespersonid = App.salespersons.FirstOrDefault(x => x.Value == sales_person_picker.SelectedItem.ToString()).Key;
            vals["user_id"] = salespersonid;

            //var customerid = App.cusdict.FirstOrDefault(x => x.Value == cus_picker.SelectedItem.ToString()).Key;
            //vals["partner_id"] = customerid;

          //  vals["partner_name"] = cus_picker.SelectedItem.ToString();

            //int next_activity_id = 0;

            //var nextactivityid =
            //        (
            //            from i in App.nextActivityList
            //        where i.name == nextact_picker.SelectedItem.ToString()
            //            select new
            //            {
            //                i.id,
            //            }
            //   ).ToList();

            //foreach (var comgroup in nextactivityid)
            //{
            //    next_activity_id = comgroup.id;
            //}

            //vals["next_activity_id"] = next_activity_id;

            //String date_deadline_string = String.Format("{0:MM-dd-yyyy HH:mm:ss}", expe_closing_picker.Date);

            //vals["date_deadline"] = date_deadline_string;

            //vals["planned_revenue"] = ex_rev_entry.Text;

            //vals["probability"] = prob_entry.Text;

           // vals["description"] = activty_des.Text;

            vals["type"] = "lead";

            vals["priority"] = count;

            //if (CrmNameValue.Text == "")
            //{
            //    leadtitle_alert.IsVisible = true;
            //}

           //if (Convert.ToInt32(prob_entry.Text) > 100)
            //{               
            //    prob_alert.IsVisible = true;
            //}

            if (CrmNameValue.Text == "")
            {
                //  leadtitle_alert.IsVisible = true;
                await DisplayAlert("Alert", "Please Fill Name", "Ok");
            }

            else
            {
                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);
                //  string updated = Controller.InstanceCreation().UpdateLeadCreationData("crm.lead", "create", vals_update);

                bool updated = Controller.InstanceCreation().UpdateSaleOrder("crm.lead", "write", crm_lead_id, vals);

                if (updated == true)
                {
                    App.Current.MainPage = new MasterPage(new CrmTabbedPage());
                         
                }

                else
                {
                  //  Loadingalertcall(); 
                    await  DisplayAlert("Alert", "Please try again", "Ok");

                }

               

            }

            Loadingalertcall();  

        }

        private void cancelClickedAsync(object sender, EventArgs e)
        {
            crm_name_layout.IsVisible = false;
            CrmNameValue.IsVisible = true;

            //ExpectedRevenueValue.IsVisible = true;
            //ex_rev_entry.IsVisible = false;

            //ProbabilityValue.IsVisible = true;
            //prob_entry.IsVisible = false;

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

            //NextActValue.IsVisible = true;
            //nextact_picker.IsVisible = false;

            //expectedClosingValue.IsVisible = true;
            //expe_closing_picker.IsVisible = false;



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



    }
}