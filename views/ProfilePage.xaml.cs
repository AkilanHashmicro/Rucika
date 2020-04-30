using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SalesApp.models;
using SalesApp.Pages;
using Xamarin.Forms;

namespace SalesApp.views
{
    public partial class ProfilePage : ContentPage
    {
        string selectedImage = "";
        string topimg = "";

        Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

       bool edit_clicked = false;

        public ProfilePage()
        {
            InitializeComponent();

            name_value.Text = App.partner_name;
            email_value.Text = App.partner_email;

            try
            {
                byte[] imageAsBytes = Encoding.UTF8.GetBytes(App.partner_image);
                byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
                var stream = new MemoryStream(decodedByteArray);
                user_img.Source = ImageSource.FromStream(() => stream);

            }

            catch(Exception e)
            {
                user_img.Source = "";
            }


            var editimgRecognizer = new TapGestureRecognizer();
            editimgRecognizer.Tapped += (s, e) => {

                name_entry.IsVisible = true;
                name_value.IsVisible = false;
                email_entry.IsVisible = true;
                email_value.IsVisible = false;

                name_entry.Text = App.partner_name;
                email_entry.Text = App.partner_email;
              //  user_img.Source = App.partner_image;

                updateStackLayout.IsVisible = true;
                edit_img.IsVisible = false;
                file_img.IsEnabled = true;

                edit_clicked = true;

            };
            edit_img.GestureRecognizers.Add(editimgRecognizer);

        }
        private async void updateClickedAsync(object sender, EventArgs e)
        {
            vals["name"] = name_entry.Text;
            vals["login"] = email_entry.Text;
            vals["image"] = topimg;

          var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);
           

            bool updated = Controller.InstanceCreation().UpdateSaleOrder("res.users", "write", App.userid, vals);

            if (updated)
            {
                await DisplayAlert("Alert", "Profile updated successfully", "Ok");

                name_entry.IsVisible = false;
                name_value.IsVisible = true;
                email_entry.IsVisible = false;
                email_value.IsVisible = true;

                name_value.Text = name_entry.Text;
                email_value.Text = email_entry.Text;

                App.partner_name = name_value.Text;
                App.partner_image = topimg;
                App.partner_email = email_value.Text;

                edit_img.IsVisible = true;

                App.Current.MainPage = new MasterPage(new ProfilePage());

            }

            else
            {
                await DisplayAlert("Alert", "Try Again", "Ok");

            }
         

            await PopupNavigation.PopAllAsync();
        }

        private async void cancelClickedAsync(object sender, EventArgs e)
        {
            
                name_entry.IsVisible = false;
                name_value.IsVisible = true;
                email_entry.IsVisible = false;
                email_value.IsVisible = true;

                name_value.Text = App.partner_name;
                email_value.Text = App.partner_email;
                user_img.Source = topimg;

            try
            {
                byte[] imageAsBytes = Encoding.UTF8.GetBytes(App.partner_image);
                byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
                var stream = new MemoryStream(decodedByteArray);
                user_img.Source = ImageSource.FromStream(() => stream);
            }

            catch(Exception ex)
            {
                user_img.Source = "";
            }

                edit_img.IsVisible = true;
                edit_clicked = false;


        }

        private async void imgStackClicked(object sender, EventArgs e)
        {
            if (edit_clicked == true)
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

        private void changepasswordbtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new PasswordChangePopupPage());
        }

    }
}
