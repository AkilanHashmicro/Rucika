using System.Collections;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.models;
using SalesApp.Pages;
using SalesApp.wizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using static SalesApp.models.CRMModel;
//using Java.Util;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;

namespace SalesApp.views
{
    public partial class CustomerListviewDetailPage : PopupPage
    {
        CustomersModel cusobj = new CustomersModel();
        string selectedimage = "";
        string image = "";
        List<ContactsList> cusnewList = new List<ContactsList>();

        List<ContactsList> final_listview = new List<ContactsList>();

        //save previous contactlist
        //ContactsListPrevious data;
        //List<ContactsListPrevious> cusnewListPrev = new List<ContactsListPrevious>();

        int customer_id = 0;

        public CustomerListviewDetailPage(int cus_id)
        {
            InitializeComponent();

           // cusobj = obj;

            customer_id = cus_id;

            JObject obj = Controller.InstanceCreation().GetCustomerDetailData(customer_id);

            //CusListView.IsEnabled = false;
            //cus_image.IsEnabled = false;

            //List<string> tagsList = new List<string>();

            //List<TagsList> custagsList = new List<TagsList>();

            ////List<ContactsList> cusnewList = new List<ContactsList>();

            //byte[] imageAsBytes = Encoding.UTF8.GetBytes(obj.image_small);

            //byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
            //var stream = new MemoryStream(decodedByteArray);
            //cus_image.Source = ImageSource.FromStream(() => stream);

            //name.Text = obj.name;
            //email.Text = obj.email;

            //mobile_No.Text = obj.mobile;
            //street.Text = obj.street;
            //city.Text = obj.city;
            //zip.Text = obj.zip;
            //web_text.Text = obj.website;
            //oppo_text.Text = obj.crm_count.ToString();
            //sales_text.Text = obj.sales_count.ToString();
            //meeting_text.Text = obj.meeting_count.ToString();

            //image = obj.image_small;

            //cusnewList = obj.contacts;
            //CusListView.HeightRequest = 80 * cusnewList.Count;
            //CusListView.ItemsSource = cusnewList;





            List<ContactsList> cusnewList = new List<ContactsList>();

            string cus_name = obj["name"].ToObject<string>();
            string cus_email = obj["email"].ToObject<string>();
            string cus_mobile = obj["mobile"].ToObject<string>();
            string cus_street = obj["street"].ToObject<string>();
            string cus_city = obj["city"].ToObject<string>();
            string cus_zip = obj["zip"].ToObject<string>();
            string cus_website = obj["website"].ToObject<string>();
            int crm_count = obj["crm_count"].ToObject<int>();
            int sales_count = obj["sales_count"].ToObject<int>();
            int meeting_count = obj["meeting_count"].ToObject<int>();
            string image_small = obj["image_small"].ToObject<string>();
            cusnewList = obj["contacts"].ToObject<List<ContactsList>>();

            Dictionary<int, string> tags = obj["tags"].ToObject<Dictionary<int, string>>();

            CusListView.IsEnabled = false;
            cus_image.IsEnabled = false;

            List<string> tagsList = new List<string>();

            List<TagsList> custagsList = new List<TagsList>();



            byte[] imageAsBytes = Encoding.UTF8.GetBytes(image_small);

            byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
            var stream = new MemoryStream(decodedByteArray);
            cus_image.Source = ImageSource.FromStream(() => stream);

            name.Text = cus_name;
            email.Text = cus_email;
            mobile_No.Text = cus_mobile;
            street.Text = cus_street;
            city.Text = cus_city;
            zip.Text = cus_zip;
            web_text.Text = cus_website;
            oppo_text.Text = crm_count.ToString();
            sales_text.Text = sales_count.ToString();
            meeting_text.Text = meeting_count.ToString();

            image = image_small;

            // cusnewList = obj.contacts;
            CusListView.HeightRequest = 80 * cusnewList.Count;
            CusListView.ItemsSource = cusnewList;

            final_listview = cusnewList;


         //   final_listview = obj.contacts;


            // saving prev contactlist
            //foreach (var res in obj.contacts)
            //{
            //    data = new ContactsListPrevious();
            //    data.email = res.email;
            //    data.id = res.id;
            //    data.image_small = res.image_small;
            //    data.mobile = res.mobile;
            //    data.name = res.name;
            //    data.phone = res.phone;
            //    data.PhoneNo = res.PhoneNo;
            //    data.position = res.position;
            //    cusnewListPrev.Add(data);
            //}

            //  CusListView.ItemsSource = cusmodel;
            foreach (var cusname in tags)
            {
                tagsList.Add(cusname.Value);
                custagsList.Add(new TagsList(cusname.Value));
            }

            //tags strted 

            try
            {
                //int cnt1 = 0;
                //int cnt = 0;

                //for (int row = 0; row < tagsList.Count; row++)
                //{
                //    tags_grid.IsVisible = true;
                //    if (cnt == tagsList.Count) { break; }

                //    if (tagsList.Count == 1)
                //    {
                //        tags_grid.VerticalOptions = LayoutOptions.Center;
                //        tags_grid.VerticalOptions = LayoutOptions.Center;
                //        tags_grid.HorizontalOptions = LayoutOptions.Center;
                //        tags_grid.HorizontalOptions = LayoutOptions.Center;
                //    }

                //    for (int col = 0; col < 2; col++)
                //    {
                //        if (cnt == tagsList.Count) { break; }

                //        TagsList modelobj = custagsList.ElementAt(cnt1++);
                //        //  cnt = cnt + 1;

                //        StackLayout nestedStackLayout = new StackLayout
                //        {
                //            Orientation = StackOrientation.Vertical,
                //            HorizontalOptions = LayoutOptions.CenterAndExpand,
                //            VerticalOptions = LayoutOptions.CenterAndExpand,
                //            //  Padding = 10
                //            // HeightRequest = 20
                //        };

                //        Label tagsLabel = new Label
                //        {
                //            //  Margin = new Thickness(5, 5, 5, 5),
                //            Text = modelobj.name,
                //            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                //            FontAttributes = FontAttributes.Bold,
                //            TextColor = Color.White,
                //            HorizontalOptions = LayoutOptions.Start,
                //            HorizontalTextAlignment = TextAlignment.Center,
                //            //  TextColor = Color.Black,
                //            VerticalOptions = LayoutOptions.Start,
                //            VerticalTextAlignment = TextAlignment.Center,
                //            //HeightRequest =10,
                //        };

                //        Frame lblframe = new Frame()
                //        {
                //            CornerRadius = 6,
                //            BackgroundColor = Color.Gray,
                //            Padding = new Thickness(5),
                //            HeightRequest = 15,
                //            Margin = new Thickness(3, 3, 3, 3),
                //        };

                //        lblframe.Content = tagsLabel;

                //        nestedStackLayout.Children.Add(lblframe);

                //        tags_grid.Children.Add(nestedStackLayout, col, row);
                //    }
                //}


                var backImgRecognizer = new TapGestureRecognizer();
                backImgRecognizer.Tapped += (s, e) => {
                    // handle the tap    

                    Navigation.PopAllPopupAsync();

                };
                backImg.GestureRecognizers.Add(backImgRecognizer);

            }
            catch (Exception ea)
            { System.Diagnostics.Debug.WriteLine("Warning Message : " + ea.Message); }



            var editbutton = new TapGestureRecognizer();
            editbutton.Tapped += (s, e) => {

                sq_editbtn.IsVisible = false;

                updatebtn.IsVisible = true;

                CusListView.IsEnabled = true;

                cus_image.IsEnabled = true;

                name.IsVisible = false;
                name_entry.Text = name.Text;
                name_label.IsVisible = true;
                name_entry.IsVisible = true;

                email.IsVisible = false;
                email_entry.Text = email.Text;
                email_label.IsVisible = true;
                email_entry.IsVisible = true;

                mobile_No.IsVisible = false;
                mobile_No_entry.Text = mobile_No.Text;
                mobile_No_label.IsVisible = true;
                mobile_No_entry.IsVisible = true;


                street.IsVisible = false;
                street_entry.Text = street.Text;
                street_label.IsVisible = true;
                street_entry.IsVisible = true;

                city.IsVisible = false;
                city_entry.Text = city.Text;
                city_label.IsVisible = true;
                city_entry.IsVisible = true;

                zip.IsVisible = false;
                zip_entry.Text = zip.Text;
                zip_label.IsVisible = true;
                zip_entry.IsVisible = true;

                web_text.IsVisible = false;
                web_text_entry.Text = web_text.Text;
                web_text_entry.IsVisible = true;

                AddContact_line.IsVisible = true;

            };
            sq_editbtn.GestureRecognizers.Add(editbutton);


            var imageRecognizer = new TapGestureRecognizer();
            imageRecognizer.Tapped += async (s, e) => {

                FileData fileData = new FileData();
                FileData filedata = null;
                try
                {
                    filedata = await CrossFilePicker.Current.PickFile();
                    if (!string.IsNullOrEmpty(filedata.FileName)) //Just the file name, it doesn't has the path
                    {
                        byte[] bydata = filedata.DataArray;
                        String UploadData = Convert.ToBase64String(bydata);
                        selectedimage = UploadData;
                        var str = new MemoryStream(bydata);
                        cus_image.Source = ImageSource.FromStream(() => str);
                    }
                }

                catch (Exception ex)
                {
                    filedata = null;
                    System.Diagnostics.Debug.WriteLine("Warning Exception :  " + ex.Message);
                }

            };
            cus_image.GestureRecognizers.Add(imageRecognizer);

            var contRecognizer = new TapGestureRecognizer();
            contRecognizer.Tapped += (s, e) => {

               // Navigation.PushPopupAsync(new ContractDetailWizard(obj.id));

                Navigation.PushPopupAsync(new ContractDetailWizard(customer_id));
            };
            AddContact_line.GestureRecognizers.Add(contRecognizer);


        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            // obj.View.BackgroundColor = Color.FromHex("#414141");
            obj.View.BackgroundColor = Color.White;
        }

        private void meetingStackClicked(object sender, EventArgs e)
        {
          //  Navigation.PushPopupAsync(new MeetingsListviewPage(cusobj.id));
            Navigation.PushPopupAsync(new MeetingsListviewPage(customer_id));
        }

        private void oppoClicked(object sender, EventArgs e)
        {
          //  Navigation.PushPopupAsync(new OppurtunityListviewPage(cusobj.id));
            Navigation.PushPopupAsync(new OppurtunityListviewPage(customer_id));
        }

        private void saleClicked(object sender, EventArgs e)
        {
           // Navigation.PushPopupAsync(new SaleListviewPage(cusobj.id));
            Navigation.PushPopupAsync(new SaleListviewPage(customer_id));
        }

        private void CusListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ContactsList obj = e.Item as ContactsList;
            Navigation.PushPopupAsync(new ContractDetailWizard(obj));
        }


        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAllPopupAsync();
            return true;
        }

        //private void save_clicked(object sender, EventArgs ea)
        //{
        //    updatebtn.IsVisible = true;

        //    // label fields
        //    name.IsVisible = true;
        //    email.IsVisible = true;
        //    mobile_No.IsVisible = true;
        //    street.IsVisible = true;
        //    city.IsVisible = true;
        //    zip.IsVisible = true;
        //    web_text.IsVisible = true;

        //    name_label.IsVisible = false;
        //    email_label.IsVisible = false;
        //    mobile_No_label.IsVisible = false;
        //    street_label.IsVisible = false;
        //    city_label.IsVisible = false;
        //    zip_label.IsVisible = false;

        //    // Entry fields
        //    name_entry.IsVisible = false;
        //    email_entry.IsVisible = false;
        //    mobile_No_entry.IsVisible = false;
        //    street_entry.IsVisible = false;
        //    city_entry.IsVisible = false;
        //    zip_entry.IsVisible = false;
        //    web_text_entry.IsVisible = false;

        //    name.Text = name_entry.Text;
        //    email.Text = email_entry.Text;
        //    mobile_No.Text = mobile_No_entry.Text;
        //    street.Text = street_entry.Text;
        //    city.Text = city_entry.Text;
        //    zip.Text = zip_entry.Text;
        //    web_text.Text = web_text_entry.Text;

        //}

        //private void cancel_clicked(object sender, EventArgs ea)
        //{

        //    sq_editbtn.IsVisible = true;
        //    savebtn_layout.IsVisible = false;

        //    // label fields
        //    name.IsVisible = true;
        //    email.IsVisible = true;
        //    mobile_No.IsVisible = true;
        //    street.IsVisible = true;
        //    city.IsVisible = true;
        //    zip.IsVisible = true;
        //    web_text.IsVisible = true;

        //    name_label.IsVisible = false;
        //    email_label.IsVisible = false;
        //    mobile_No_label.IsVisible = false;
        //    street_label.IsVisible = false;
        //    city_label.IsVisible = false;
        //    zip_label.IsVisible = false;

        //    // Entry fields
        //    name_entry.IsVisible = false;
        //    email_entry.IsVisible = false;
        //    mobile_No_entry.IsVisible = false;
        //    street_entry.IsVisible = false;
        //    city_entry.IsVisible = false;
        //    zip_entry.IsVisible = false;
        //    web_text_entry.IsVisible = false;
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ContactsList cont_list = null;

            MessagingCenter.Subscribe<string, ContactsList>("MyApp", "ContactMsg", (sender, arg) =>
            {
                int contact_id = arg.id;

                foreach (var data in cusobj.contacts)
                {
                    if (contact_id == data.id && data.id != 0)
                    {
                        cont_list = new ContactsList();

                        cont_list.name = arg.name;
                        cont_list.id = contact_id;
                        cont_list.mobile = arg.mobile;
                        cont_list.phone = arg.phone;
                        cont_list.position = arg.position;
                        cont_list.email = arg.email;
                        cont_list.image_small = arg.image_small;
                        //if (arg.image_small == "")
                        //{
                        //    cont_list.image_small = data.image_small;
                        //}
                        //else
                        //{
                        //    cont_list.image_small = arg.image_small;
                        //}
                    }
                }

                int index = final_listview.FindIndex(m => m.id == contact_id);
                if (index >= 0)
                    final_listview[index] = cont_list;

                CusListView.ItemsSource = null;

                CusListView.HeightRequest = 80 * final_listview.Count;
                CusListView.ItemsSource = final_listview;

            });

            MessagingCenter.Subscribe<string, ContactsList>("MyApp", "CreateMsg", (sender, arg) =>
            {
                final_listview.Add(arg);

                CusListView.ItemsSource = null;
                CusListView.HeightRequest = 80 * final_listview.Count;
                CusListView.ItemsSource = final_listview;
            });
        }

        private async void update_clickedAsync(object sender, EventArgs ea)
        {
            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

            //List<ContactsCreationList2> meetingLineList = new List<ContactsCreationList2>();

            //foreach (var obj in final_listview)
            //{
            //    meetingLineList.Add(new ContactsCreationList2(obj.name, obj.mobile, obj.email));
            //}


            //List<dynamic> contactList = new List<dynamic>();


            //foreach(var item in meetingLineList)
            //{
            //    Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();


            //    dict.Add("mobile", item.mobile);
            //    dict.Add("name", item.name);
            //    dict.Add("email", item.email);

            //    List<dynamic> flist = new List<dynamic>();
            //    flist.Add(0);
            //    flist.Add(0);
            //    flist.Add(dict);
            //    contactList.Add(flist);
            //}



            //System.Diagnostics.Debug.WriteLine("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq " + contactList);


            vals["name"] = name_entry.Text;
            vals["email"] = email_entry.Text;
            vals["mobile"] = mobile_No_entry.Text;
            vals["street"] = street_entry.Text;
            vals["city"] = city_entry.Text;
            vals["zip"] = zip_entry.Text;
            vals["website"] = web_text_entry.Text;

            //vals["child_ids"] = contactList;




            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);

            var updated = Controller.InstanceCreation().UpdateCustomerData("res.partner", "write", cusobj.id, vals);

            if (updated)
            {
                App.Current.MainPage = new MasterPage(new CustomersPage());
            }
            else
            {
                await DisplayAlert("Alert", "Please try again", "Ok");
            }

            Loadingalertcall();

        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        private void updatecancel_clickedAsync(object sender, EventArgs ea)
        {
            sq_editbtn.IsVisible = true;
            updatebtn.IsVisible = false;

            CusListView.IsEnabled = false;

            cus_image.IsEnabled = false;

            // label fields
            name.IsVisible = true;
            email.IsVisible = true;
            mobile_No.IsVisible = true;
            street.IsVisible = true;
            city.IsVisible = true;
            zip.IsVisible = true;
            web_text.IsVisible = true;

            name_label.IsVisible = false;
            email_label.IsVisible = false;
            mobile_No_label.IsVisible = false;
            street_label.IsVisible = false;
            city_label.IsVisible = false;
            zip_label.IsVisible = false;

            // Entry fields
            name_entry.IsVisible = false;
            email_entry.IsVisible = false;
            mobile_No_entry.IsVisible = false;
            street_entry.IsVisible = false;
            city_entry.IsVisible = false;
            zip_entry.IsVisible = false;
            web_text_entry.IsVisible = false;

            byte[] imageAsBytes = Encoding.UTF8.GetBytes(image);
            byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
            var stream = new MemoryStream(decodedByteArray);
            cus_image.Source = ImageSource.FromStream(() => stream);

            AddContact_line.IsVisible = false;


            //ContactsList cont_listNew;
            //final_listview.Clear();
            //foreach (var data in cusnewListPrev)
            //{
            //    cont_listNew = new ContactsList();
            //    cont_listNew.name = data.name;
            //    cont_listNew.id = data.id;
            //    cont_listNew.mobile = data.mobile;
            //    cont_listNew.phone = data.phone;
            //    cont_listNew.position = data.position;
            //    cont_listNew.email = data.email;
            //    cont_listNew.image_small = data.image_small;
            //    final_listview.Add(cont_listNew);
            //}

            //CusListView.ItemsSource = null;

            //CusListView.HeightRequest = 80 * final_listview.Count;
            //CusListView.ItemsSource = final_listview;

        }

    }
}