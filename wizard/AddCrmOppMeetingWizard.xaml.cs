using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SalesApp.models;
using SalesApp.views;
using Xamarin.Forms;
using static SalesApp.models.CRMModel;

namespace SalesApp.wizard
{
    public partial class AddCrmOppMeetingWizard : PopupPage
    {
        List<AttendeesList> attList = new List<AttendeesList>();
        List<AttendeesList> attList1 = new List<AttendeesList>();

        public AddCrmOppMeetingWizard()
        {
            InitializeComponent();

           // List<AttendeesList> atts = new List<AttendeesList>();
          
            attnListView.HeightRequest = 0;

            attnPicker.ItemsSource = App.cusList.Select(d => d.name).ToList();;
            attnPicker.SelectedIndex = -1;
           // Attn.ItemsSource = App.cusdict;

            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

            //vals["subject"] = sub.SelectedItem.ToString();
            //vals["start_date"] = sDate.Date;
            //vals["end_date"] = eDate.Date;
            //vals["attendees"] = Attn.Text;
            //vals["location"] = loc.Text.ToString();        
            //vals["duration"] = Dur.Text.ToString();

            string updated = Controller.InstanceCreation().UpdateCRMOpporData("sale.crm", "create_crm_quotations", vals);

            var backRecognizer = new TapGestureRecognizer();
            backRecognizer.Tapped += (s, e) => {

                Navigation.PushPopupAsync(new CRMOpportunityCreationPage1());

            };
            backImg.GestureRecognizers.Add(backRecognizer);

            var AirConImgRecognizer = new TapGestureRecognizer();
            AirConImgRecognizer.Tapped += (s, e) => {                
                attnPicker.IsVisible = true;
                                                                         
            };
            AddAirCon.GestureRecognizers.Add(AirConImgRecognizer);
        }

        public void myPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            attnListView.ItemsSource = attList1;
            attnPicker.IsVisible = false;
            attList.Add(new AttendeesList(attnPicker.SelectedItem.ToString()));

            attList = attList.GroupBy(i => i.name).Select(g => g.First()).ToList();

           // attnListView.ItemsSource = attList;

            attnListView.ItemsSource = attList;
            attnListView.RowHeight = 40;
            attnListView.HeightRequest = 40 * attList.Count;

            return;
          
              //  Navigation.PushPopupAsync(new AddCrmOppMeetingWizard());           
        }

        private void Button_Back_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Button_Add_Clicked(object sender, EventArgs e)
        {
            
            
        }
    }
}
