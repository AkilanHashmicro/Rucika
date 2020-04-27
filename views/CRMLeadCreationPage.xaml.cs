using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;

using System.IO;
using SalesApp.models;
using static SalesApp.models.CRMModel;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;

namespace SalesApp.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CRMLeadCreationPage : PopupPage
    {
        string selectedImage = "";
        string topimg = "";
        string companyType = "person";

        Dictionary<int, string> countrydict = new Dictionary<int, string>();
        Dictionary<int, string> custagsdict = new Dictionary<int, string>();
        Dictionary<int, string> companydict = new Dictionary<int, string>();
        Dictionary<int, string> statedict = new Dictionary<int, string>();

        List<TagsList> tagsList = new List<TagsList>();
        List<TagsList> tagsList1 = new List<TagsList>();
        List<int> tagIdsList = new List<int>();
        Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
        Dictionary<string, dynamic> vals1 = new Dictionary<string, dynamic>();

        public CRMLeadCreationPage ()
        {
            InitializeComponent ();

            ContactListView.HeightRequest = 0;

            App.concreationList.Clear();

            NavigationPage.SetHasNavigationBar(this, false);

            JObject customerdata = Controller.InstanceCreation().GetCustomerCreationData();

            countrydict = customerdata["country_list"].ToObject<Dictionary<int, string>>();
            custagsdict = customerdata["tags_list"].ToObject<Dictionary<int, string>>();
            companydict = customerdata["company_list"].ToObject<Dictionary<int, string>>();
            statedict = customerdata["state_list"].ToObject<Dictionary<int, string>>();

            indipicker.ItemsSource = companydict.Select(d => d.Value).ToList();           
            indipicker.SelectedIndex = -1;
            indipicker.Title = "Select";

            tagspicker.ItemsSource = custagsdict.Select(v => v.Value).ToList();
            tagspicker.SelectedIndex = -1;
            tagspicker.Title = "Select";

            statePicker.ItemsSource = statedict.Select(d => d.Value).ToList();
            statePicker.SelectedIndex = 0;

            countryPicker.ItemsSource = countrydict.Select(d => d.Value).ToList();
            countryPicker.SelectedIndex = 0;


            var backImgRecognizer = new TapGestureRecognizer();
            backImgRecognizer.Tapped += (s, e) => {
                // handle the tap    

                Navigation.PopAllPopupAsync();

            };
            backImg.GestureRecognizers.Add(backImgRecognizer);

            var rbtnRecognizer = new TapGestureRecognizer();
            rbtnRecognizer.Tapped += (s, e) => {
                fillimg1.IsVisible = true;
                fillimg2.IsVisible = false;
                indiPickerStack.IsVisible = true;

            };
            empimg1.GestureRecognizers.Add(rbtnRecognizer);

            var rbtnRecognizer1 = new TapGestureRecognizer();
            rbtnRecognizer1.Tapped += (s, e) => {
                fillimg2.IsVisible = true;
                fillimg1.IsVisible = false;
                indiPickerStack.IsVisible = false;
                companyType = "company";
            };
            empimg2.GestureRecognizers.Add(rbtnRecognizer1);

            var AirConImgRecognizer1 = new TapGestureRecognizer();
            AirConImgRecognizer1.Tapped += (s, e) => {
                // handle the tap              
                tagsListView.IsVisible = true;
                Addbtn1.IsVisible = false;
                tagsgrid.IsVisible = true;
            };
            AddAirCon2.GestureRecognizers.Add(AirConImgRecognizer1);


            var imgEditRecognizer = new TapGestureRecognizer();
            imgEditRecognizer.Tapped += async (s, e) => {

                FileData fileData = new FileData();
                FileData filedata = null;
                try
                {
                    filedata = await CrossFilePicker.Current.PickFile();
                    if (!string.IsNullOrEmpty(filedata.FileName)) //Just the file name, it doesn't has the path
                    {
                        byte[] bydata = filedata.DataArray;
                        String UploadData = Convert.ToBase64String(bydata);
                        topimg = UploadData;
                        var stream = new MemoryStream(bydata);
                        user_img.Source = ImageSource.FromStream(() => stream);
                    }
                }

                catch (Exception ex)
                {
                    filedata = null;
                    System.Diagnostics.Debug.WriteLine("Warning Exception :  " + ex.Message);
                }

            };
            imgEdit.GestureRecognizers.Add(imgEditRecognizer);

            var imgDeleteRecognizer = new TapGestureRecognizer();
            imgDeleteRecognizer.Tapped += async (s, e) => {

                user_img.Source = "unknown.png";

            };
            imgDelete.GestureRecognizers.Add(imgDeleteRecognizer);
        }
               
        private void CRMCreateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new CustomerContactCreationPage());
           // App.Current.MainPage = new MasterPage(new CRMCreationPopupPage());
        }

        private void ViewCellPending_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.White;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string, string>("MyApp", "NotifyMsg", (sender, arg) =>
            {
                ContactListView.ItemsSource = null;
                ContactListView.ItemsSource = App.concreationList;
                ContactListView.HeightRequest = 100 * App.concreationList.Count;

            });
        }

        public void tagsPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            int cusid = custagsdict.FirstOrDefault(x => x.Value == tagspicker.SelectedItem.ToString()).Key;
            tagIdsList.Add(cusid);

            tagsList.Add(new TagsList(tagspicker.SelectedItem.ToString()));

            tagIdsList = tagIdsList.GroupBy(i => i).Select(g => g.First()).ToList();
            tagsList = tagsList.GroupBy(i => i.name).Select(g => g.First()).ToList();

            tagsListView.ItemsSource = tagsList;
            tagsListView.RowHeight = 40;
            tagsListView.HeightRequest = 40 * tagsList.Count;

            //mainstack.HeightRequest = mainstack.HeightRequest + 40;

            tagsgrid.IsVisible = false;
            Addbtn1.IsVisible = true;
        }

        void tagsListviewcloseClicked(object sender, EventArgs e1)
        {
            tagsListView.ItemsSource = tagsList;
            var args = (TappedEventArgs)e1;
            TagsList t2 = args.Parameter as TagsList;

            var itemToRemove = tagsList.Single(r => r.name == t2.name);
            tagsList.Remove(itemToRemove);

            int cusid = custagsdict.FirstOrDefault(x => x.Value == t2.name.ToString()).Key;
            tagIdsList.Remove(cusid);

            tagsListView.ItemsSource = tagsList;
            tagsListView.RowHeight = 40;
            tagsListView.HeightRequest = 40 * tagsList.Count;

            //  mainstack.HeightRequest = mainstack.HeightRequest - 40;
            // taxes obj = (taxes)args;
        }


        private async void save_ClickedAsync(object sender, EventArgs e)
        {

            List<ContactsCreationList1> meetingLineList = new List<ContactsCreationList1>();

            foreach (var obj in App.concreationList)
            {
                meetingLineList.Add(new ContactsCreationList1(obj.Name, obj.Mobile, obj.Email, obj.ImageString));
            }

            vals["name"]  = name_entry.Text;
            vals["company_type"] = companyType;
                       
            if (companyType == "company")
            {
                vals["parent_id"] = false;
            }

            else
            {
                if(indipicker.SelectedItem ==null)
                {
                    vals["parent_id"] = false;
                }
                else
                {                                   
                int companyid = companydict.FirstOrDefault(x => x.Value == indipicker.SelectedItem.ToString()).Key;
                    vals["parent_id"] = companyid;
                }
            }
                       
            vals["street"]= street1Entry.Text;
            vals["street2"] = street2Entry.Text;
            vals["city"] = cityEntry.Text;

            int stateid = statedict.FirstOrDefault(x => x.Value == statePicker.SelectedItem.ToString()).Key;
            vals["state_id"] = stateid;

            vals["zip"] = zipEntry.Text;

            int countryid = countrydict.FirstOrDefault(x => x.Value == countryPicker.SelectedItem.ToString()).Key;
            vals["country_id"] = countryid;

            vals["website"] = webEntry.Text;

            vals["category_id"] = tagIdsList;
            vals["function"] = jobposttion.Text;
            vals["phone"] = phoneEntry.Text;

            vals["mobile"] = mobileEntry.Text;
            vals["email"] = mailEntry.Text;


            vals["image"] = topimg;
            vals["contact_list"] = meetingLineList;

            if (name_entry.Text == "")
            {
                name_alert.IsVisible = true;
            }

            else
            {
                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);
                              
                var updated = Controller.InstanceCreation().UpdateCRMOpporData("res.partner", "create_customer_app", vals);

                if (updated == "true")
                {
                 //   await  DisplayAlert("Alert", "Created Successfull", "Ok");
                    App.Current.MainPage= new MasterPage(new CustomersPage());
                }
                else
                {
                    await DisplayAlert("Alert", "Please try again", "Ok");
                }

                Loadingalertcall();

            }
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        private void cancel_Clicked(object sender, EventArgs e)
        {
            //  Navigation.PushPopupAsync(new CustomerContactCreationPage());

         //   PopupNavigation.PopAsync();

          //  App.Current.MainPage = new MasterPage(new CustomersPage());

            Navigation.PopAllPopupAsync();
        }

        private async void imgStackClicked(object sender, EventArgs e)
        {
            FileData fileData = new FileData();
            FileData filedata = null;
            try
            {
                filedata = await CrossFilePicker.Current.PickFile();
                if (!string.IsNullOrEmpty(filedata.FileName)) //Just the file name, it doesn't has the path
                {
                    byte[] bydata = filedata.DataArray;
                    String UploadData = Convert.ToBase64String(bydata);
                    topimg = UploadData;
                    var stream = new MemoryStream(bydata);
                    user_img.Source = ImageSource.FromStream(() => stream);
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