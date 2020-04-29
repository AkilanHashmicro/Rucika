using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SalesApp.models;
using SalesApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XLabs.Forms.Controls;
using static SalesApp.models.CRMModel;

namespace SalesApp.views
{
    public partial class CalendarDetailPage : PopupPage
    {
        List<all_events> calresult = new List<all_events>();
        all_events editobj = new all_events();
        public CalendarDetailPage(all_events obj)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            editobj = obj;

            var backRecognizer = new TapGestureRecognizer();
            backRecognizer.Tapped += async (s, e) => {


                App.Current.MainPage = new MasterPage(new CalendarPage());

                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);
                Loadingalertcall();
               // PopupNavigation.PopAsync();
           //    Navigation.PopAllPopupAsync();
                //   Navigation.PushAsync(new CalendarPage());
            //  App.Current.MainPage = new MasterPage(new CalendarPage());


            };
            backImg.GestureRecognizers.Add(backRecognizer);


            var toolRecognizer = new TapGestureRecognizer();
            toolRecognizer.Tapped += (s, e) => {

                Navigation.PushPopupAsync(new CalendarPopupPage(editobj));

            };
            toolImg.GestureRecognizers.Add(toolRecognizer);


            if (editobj.sign_in == true)
            {

                signin_loc.IsVisible = false;
                cancel_signin.IsVisible = true;

                //  sign_time.Text = "25/06/2019 15:45";

                //  DateTime gt =    DateTime.ParseExact(obj.sign_in_time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                //   DateTime lt = gt.ToUniversalTime();

                sign_time.Text = obj.sign_in_time; 

                MyMap.MoveToRegion(
          MapSpan.FromCenterAndRadius(
                        new Position(editobj.sign_in_lat, editobj.sign_in_long), Distance.FromMiles(0.05)));

                var position1 = new Position(editobj.sign_in_lat, editobj.sign_in_long);

                var pin1 = new Pin
                {
                    Type = PinType.Place,
                    Position = position1,
                    Label = "Your Address",
                    Address = "",
                };

                MyMap.Pins.Add(pin1);

            }


            if (editobj.sign_out == true)
            {
                signout_loc.IsVisible = false;
                cancel_signout.IsVisible = true;

                signout_time.Text = obj.sign_out_time; 

                MyMap1.MoveToRegion(
              MapSpan.FromCenterAndRadius(
                        new Position(editobj.sign_out_lat, editobj.sign_out_long), Distance.FromMiles(0.05)));

                var position2 = new Position(editobj.sign_out_lat, editobj.sign_out_long);

                var pin2 = new Pin
                {
                    Type = PinType.Place,
                    Position = position2,
                    Label = "Your Address",
                    Address = "",
                };

                MyMap1.Pins.Add(pin2);

            }

            List<EventsAtts> test = new List<EventsAtts>();

            List<TagsList> tagsstring = new List<TagsList>();

            metsub.Text = obj.meeting_subject;
            try
            {
                DateTime dateTime = DateTime.ParseExact(obj.starting_at, "yyyy-MM-dd HH:mm:ss",
                                                        CultureInfo.InvariantCulture);
                sat.Text = dateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
            }

            catch (Exception ea)
            {
                sat.Text = "";
            }

            dur.Text = obj.duration;
            loc.Text = obj.location;
            des.Text = obj.description;
            test = obj.attendees;
            tagsstring = obj.tags;

            int cnt = 0;
            try
            {
                foreach (var attobj in test)
                {
                    for (int row = 0; row < test.Count; row++)
                    {
                        attGrid.IsVisible = true;
                        if (cnt == test.Count) { break; }

                        if (test.Count == 1)
                        {
                            attGrid.VerticalOptions = LayoutOptions.Center;
                            attGrid.VerticalOptions = LayoutOptions.Center;
                            attGrid.HorizontalOptions = LayoutOptions.Center;
                            attGrid.HorizontalOptions = LayoutOptions.Center;
                        }

                        for (int col = 0; col < 3; col++)
                        {
                            if (cnt == test.Count) { break; }

                            EventsAtts tmpObj = test.ElementAt(cnt++);

                            StackLayout nestedStackLayout = new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                VerticalOptions = LayoutOptions.Center,

                            };

                            var Pic = new CircleImage()
                            {
                                WidthRequest = 50,
                                HeightRequest = 50,
                                //CacheDuration = TimeSpan.FromDays(1),
                                //DownsampleToViewSize = true,
                                //BitmapOptimizations = false,
                                Aspect = Aspect.AspectFit,
                            };

                            Label imageLabel = new Label
                            {
                                Margin = new Thickness(0, 0, 0, 0),
                                Text = tmpObj.name,
                                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                                FontAttributes = FontAttributes.Bold,
                                //TextColor = Color.FromHex("#9EA09F"),
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalTextAlignment = TextAlignment.Center,
                                TextColor = Color.Black,

                            };

                            byte[] imageAsBytes = Encoding.UTF8.GetBytes(tmpObj.image_small);

                            byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
                            var stream = new MemoryStream(decodedByteArray);
                            Pic.Source = ImageSource.FromStream(() => stream);

                            nestedStackLayout.Children.Add(Pic);
                            nestedStackLayout.Children.Add(imageLabel);

                            attGrid.Children.Add(nestedStackLayout, col, row);
                        }
                    }

                }
            }

            catch (Exception ea)
            { System.Diagnostics.Debug.WriteLine("Warning Message : " + ea.Message); }

            //tags strted 

            try
            {
                int cnt1 = 0;

                for (int row = 0; row < tagsstring.Count; row++)
                {
                    tagsGrid.IsVisible = true;
                    if (cnt1 == tagsstring.Count) { break; }

                    if (test.Count == 1)
                    {
                        attGrid.VerticalOptions = LayoutOptions.Center;
                        attGrid.VerticalOptions = LayoutOptions.Center;
                        attGrid.HorizontalOptions = LayoutOptions.Center;
                        attGrid.HorizontalOptions = LayoutOptions.Center;
                    }

                    for (int col = 0; col < 3; col++)
                    {
                        if (cnt1 == tagsstring.Count) { break; }

                        TagsList modelobj = tagsstring.ElementAt(cnt1++);
                        //  cnt = cnt + 1;

                        StackLayout nestedStackLayout = new StackLayout
                        {
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            //  Padding = 10
                            // HeightRequest = 20
                        };

                        Label tagsLabel = new Label
                        {
                            //  Margin = new Thickness(5, 5, 5, 5),
                            Text = modelobj.name,
                            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                            FontAttributes = FontAttributes.Bold,
                            //TextColor = Color.FromHex("#9EA09F"),
                            HorizontalOptions = LayoutOptions.Start,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Color.Black,
                            VerticalOptions = LayoutOptions.Start,
                            VerticalTextAlignment = TextAlignment.Center,
                            //HeightRequest =10,
                        };

                        //TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer
                        //{
                        //    Command = new Command(OnSquareTapped),
                        //    CommandParameter = tmpObj
                        //};
                        //tagsLabel.GestureRecognizers.Add(tapGestureRecognizer);

                        Frame lblframe = new Frame()
                        {
                            CornerRadius = 6,
                            BackgroundColor = Color.FromHex(modelobj.colour.Replace(" ", "")),
                            Padding = new Thickness(5),
                            HeightRequest = 15,
                            Margin = new Thickness(3, 3, 3, 3),
                        };

                        lblframe.Content = tagsLabel;

                        nestedStackLayout.Children.Add(lblframe);

                        tagsGrid.Children.Add(nestedStackLayout, col, row);
                    }
                }

            }

            catch (Exception ea)
            { System.Diagnostics.Debug.WriteLine("Warning Message : " + ea.Message); }

        }

        private async void SignIn_ClickedAsync(object sender, EventArgs e)
        {
            // await DisplayAlert("Alert", "Make Sure your GPS is Enabled", "Ok");
            await RetrieveLocation();
        }

        private async void CancelSignIn_ClickedAsync(object sender, EventArgs e)
        {
            var updated = Controller.InstanceCreation().clearGPSData("calendar.event", "cancel_sign_in", editobj.id);


            MyMap.MoveToRegion(
              MapSpan.FromCenterAndRadius(
                        new Position(0, 0), Distance.FromMiles(0.05)));

            var position1 = new Position(0, 0);

            var pin1 = new Pin
            {
                Type = PinType.Place,
                Position = position1,
                Label = "Your Address",
                Address = "",
            };

            MyMap.Pins.Add(pin1);

            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);
            await DisplayAlert("Alert", "Location Successfully Updated", "Ok");

            // App.Current.MainPage = new MasterPage(new CalendarPage());



         await PopupNavigation.PopAsync();

          //    Loadingalertcall();

            sign_time.Text = "";
            signin_loc.IsVisible = true;
            cancel_signin.IsVisible = false;

            //    await RetrieveLocationForSignOut();

        }


        private async Task RetrieveLocation()
        {

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 20;
                var position = await locator.GetPositionAsync(timeout: new TimeSpan(20000));

                String dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //DateTime fd =   DateTime.ParseExact(dt, "MM-dd-yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //dt = fd.ToString("yyyy-MM-dd HH:mm:ss");

                var updated = Controller.InstanceCreation().UpdateGPSData("calendar.event", "set_sign_in_location",editobj.id, (float)position.Latitude, (float)position.Longitude,dt);

                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);
                //    App.Current.MainPage = new MasterPage(new CalendarPage());
                await DisplayAlert("Alert", "Location Successfully Updated", "Ok");
                //App.Current.MainPage = new MasterPage(new CalendarPage());

                //Loadingalertcall();

                await PopupNavigation.PopAsync();

                signin_loc.IsVisible = false;
                cancel_signin.IsVisible = true;


                sign_time.Text = dt;

                MyMap.MoveToRegion(
      MapSpan.FromCenterAndRadius(
                    new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.05)));

                var position1 = new Position(position.Latitude, position.Longitude);

                var pin1 = new Pin
                {
                    Type = PinType.Place,
                    Position = position1,
                    Label = "Your Address",
                    Address = "",
                };

                MyMap.Pins.Add(pin1);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Alert", "Please Check GPS Connection", "Ok");
            }
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.PopAllAsync();
        }

        private async void Signout_ClickedAsync(object sender, EventArgs e)
        {
            await RetrieveLocationForSignOut();
        }

        private async void CancelSignout_ClickedAsync(object sender, EventArgs e)
        {

            var updated = Controller.InstanceCreation().clearGPSData("calendar.event", "cancel_sign_out", editobj.id);
            // await RetrieveLocationForSignOut();

            MyMap1.MoveToRegion(
          MapSpan.FromCenterAndRadius(
                        new Position(0, 0), Distance.FromMiles(0.05)));

            var position1 = new Position(0, 0);

            var pin1 = new Pin
            {
                Type = PinType.Place,
                Position = position1,
                Label = "Your Address",
                Address = "",
            };

            MyMap1.Pins.Add(pin1);

            var currentpage = new LoadingAlert();
            await PopupNavigation.PushAsync(currentpage);
            await DisplayAlert("Alert", "Location Successfully Updated", "Ok");

            signout_time.Text = "";

          //  App.Current.MainPage = new MasterPage(new CalendarPage());

          //  Loadingalertcall();

            await PopupNavigation.PopAsync();

            signout_loc.IsVisible = true;
            cancel_signout.IsVisible = false;

            //   await RetrieveLocationForSignOut();


        }

        private async Task RetrieveLocationForSignOut()
        {
            try
            {

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 20;
                var position = await locator.GetPositionAsync(timeout: new TimeSpan(20000));
                String dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //DateTime fd = DateTime.ParseExact(dt, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //dt = fd.ToString("yyyy-MM-dd HH:mm:ss");

                var updated = Controller.InstanceCreation().UpdateGPSData("calendar.event", "set_sign_out_location",editobj.id, (float)position.Latitude, (float)position.Longitude, dt);

                calresult = Controller.InstanceCreation().GetCalenderData(DateTime.Now.Month, DateTime.Now.Year);

                //if (updated == "true")
                //{
                var currentpage = new LoadingAlert();
                await PopupNavigation.PushAsync(currentpage);
                // App.Current.MainPage = new MasterPage(new CalendarPage());
                await DisplayAlert("Alert", "Location Successfully Updated", "Ok");

               //   App.Current.MainPage = new MasterPage(new CalendarPage());

                //   Loadingalertcall();

                await PopupNavigation.PopAsync();

                signout_time.Text = dt;

                cancel_signout.IsVisible = true;
                signout_loc.IsVisible = false;

                MyMap1.MoveToRegion(
          MapSpan.FromCenterAndRadius(
                        new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.05)));

                var position1 = new Position(position.Latitude, position.Longitude);

                var pin1 = new Pin
                {
                    Type = PinType.Place,
                    Position = position1,
                    Label = "Your Address",
                    Address = "",
                };

                MyMap1.Pins.Add(pin1);
            }

            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Please Check GPS Connection", "Ok");
            }
            //}

            //else
            //{
            //    await DisplayAlert("Alert", "Please try again", "Ok");
            //}

        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            // return base.OnBackButtonPressed();

            //  Navigation.PopAllPopupAsync();

            App.Current.MainPage = new MasterPage(new CalendarPage());

            //var currentpage = new LoadingAlert();
             //PopupNavigation.PushAsync(currentpage);
         //   Loadingalertcall();
            PopupNavigation.PopAllAsync();
            return true;
        }

        private void Toolbar_edit_Activated(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new CalendarPopupPage(editobj));
        }
    }
}
