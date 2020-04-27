using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace SalesApp.views
{
    public partial class ConvertToOpportunitiesPage : PopupPage
    {
        public ConvertToOpportunitiesPage()
        {
            InitializeComponent();

            //sales_picker.Items.Add("Administrator");
            //sales_picker.Items.Add("Demo User");
            //sales_picker.Items.Add("Tester");

            salesperson_picker.ItemsSource = App.salesteam.Select(x => x.Value).ToList();
            salesperson_picker.SelectedIndex = 0;

            salesteam_picker.ItemsSource = App.salespersons.Select(x => x.Value).ToList();
            salesteam_picker.SelectedIndex = 0;

            cus_picker.ItemsSource = App.cusList.Select(x => x.name).ToList();
            cus_picker.SelectedIndex = 0;

            var convertempimgRecognizer = new TapGestureRecognizer();
            convertempimgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                convertempimg.IsVisible = false;
                convertfillimg.IsVisible = true;
                mergefillimg.IsVisible = false;
                mergeempimg.IsVisible = true;
            };
            convertempimg.GestureRecognizers.Add(convertempimgRecognizer);

            var actionempimgRecognizer = new TapGestureRecognizer();
            actionempimgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                mergeempimg.IsVisible = false;
                mergefillimg.IsVisible = true;
                convertfillimg.IsVisible = false;
                convertempimg.IsVisible = true;
            };
            mergeempimg.GestureRecognizers.Add(actionempimgRecognizer);

            var linkempimgRecognizer = new TapGestureRecognizer();
            linkempimgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                linkfillimg.IsVisible = true;
                createcusfillimg.IsVisible = false;
                donotlinkfillimg.IsVisible = false;

                linkempimg.IsVisible = false;
                createcusempimg.IsVisible = true;
                donotlinkempimg.IsVisible = true;

                cusGrid.IsVisible = true;

            };
            linkempimg.GestureRecognizers.Add(linkempimgRecognizer);

            var createcusempimgRecognizer = new TapGestureRecognizer();
            createcusempimgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                linkfillimg.IsVisible = false;
                createcusfillimg.IsVisible = true;
                donotlinkfillimg.IsVisible = false;

                linkempimg.IsVisible = true;
                createcusempimg.IsVisible = false;
                donotlinkempimg.IsVisible = true;

                cusGrid.IsVisible = false;
            };
            createcusempimg.GestureRecognizers.Add(createcusempimgRecognizer);


            var donotlinkempimgRecognizer = new TapGestureRecognizer();
            donotlinkempimgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                linkfillimg.IsVisible = false;
                createcusfillimg.IsVisible = false;
                donotlinkfillimg.IsVisible = true;

                linkempimg.IsVisible = true;
                createcusempimg.IsVisible = true;
                donotlinkempimg.IsVisible = false;

                cusGrid.IsVisible = false;
            };
            donotlinkempimg.GestureRecognizers.Add(donotlinkempimgRecognizer);

        }

        void update_cancel_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        void Handle_Update_Clicked(object sender, System.EventArgs e)
        { 
            
        }
    }
}
