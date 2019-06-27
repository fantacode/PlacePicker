using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlacePicker;
using PlacePicker.Models;
using Xamarin.Forms;

namespace DemoApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private Place selectedPlace;
        public Place SelectedPlace
        {
            get { return selectedPlace; }
            set
            {
                selectedPlace = value;
                OnPropertyChanged();
            }
        }

        public async void Handle_Clicked(object sender, EventArgs e)
        {
            LocationPicker.PinImage = "pin.png";
            var place = await LocationPicker.SelectPlace();
            if (place?.Data != null)
            {
                SelectedPlace = place.Data;
                location.Text = place.Data.LocationAddress;
            }
            else
            {
                if(place != null)
                await DisplayAlert("Oops", place.Status.ToString(), "OK");
            }
        }
    }
}
