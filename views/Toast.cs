using System;

using Xamarin.Forms;

namespace SalesApp.views
{
    public class Toast : ContentPage
    {
        public Toast()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

