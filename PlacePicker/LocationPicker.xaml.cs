using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using PlacePicker.Helpers;
using PlacePicker.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace PlacePicker
{
    public partial class LocationPicker : ContentPage, INotifyPropertyChanged
    {
        public static string PinImage { get; set; }

        public GetUserLocation locator = new GetUserLocation();
        public LocationAddress address = new LocationAddress();

        private TaskCompletionSource<Place> _taskCompletionSource;

        private static Place place;
        public static Place Place
        {
            get { return place; }
            set
            {
                place = value;
            }
        }

        public LocationPicker()
        {
            InitializeComponent();
            Place = new Place();
            LocationMap.UiSettings.ZoomGesturesEnabled = true;
            LocationMap.MyLocationEnabled = true;
            LocationMap.UiSettings.MyLocationButtonEnabled = true;
            LocationMap.UiSettings.ScrollGesturesEnabled = true;
            pin.Source = PinImage;
        }

        public async Task<BaseModel<Location>> GetCurrentLocation()
        {
            var result = await locator.GetPosition();
            if (result?.Data != null)
            {
                var loc = result.Data;
                var position = new Position(loc.Latitude, loc.Longitude);
                UpdateLocationOnMap(position.Latitude, position.Longitude, true);
            }
            return result;
        }

        public async void Handle_ConfirmClicked(object sender, EventArgs e)
        {
            if (_taskCompletionSource != null)
            {
                _taskCompletionSource.SetResult(place);
                _taskCompletionSource = null;
                await Navigation.PopModalAsync();
            }
        }

        public async void Handle_CancelTapped(object sender, EventArgs e)
        {
            if (_taskCompletionSource != null)
            {
                _taskCompletionSource.SetResult(null);
                _taskCompletionSource = null;
                await Navigation.PopModalAsync();
            }
        }

        public void Handle_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            var position = new Position(e.Position.Target.Latitude, e.Position.Target.Longitude);
            UpdateLocationOnMap(position.Latitude, position.Longitude, false);
        }

        public async void UpdateLocationOnMap(double? mapLatitude, double? mapLongitude, bool DefaultPosition = false)
        {
            var position = new Position(mapLatitude.Value, mapLongitude.Value);
            var loc = new Location(mapLatitude.Value, mapLongitude.Value);
            var x = await locator.GetAddress(loc);
            if (x?.Data != null)
            {
                Place.Location = loc;
                Place.Placemark = x.Data;
                Place.LocationAddress = address.GetLocationString(x.Data);
                location.Text = address.GetLocationString(x.Data);
            }

            if (DefaultPosition)
            {
                await LocationMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(
                new CameraPosition(position, 15d)), TimeSpan.FromSeconds(0.5));
            }
        }

        private Task<Place> GetPlace()
        {
            _taskCompletionSource = new TaskCompletionSource<Place>();
            return _taskCompletionSource.Task;
        }

        async public static Task<BaseModel<Place>> SelectPlace()
        {
            var currentNav = Application.Current.MainPage.Navigation;
            if (currentNav != null)
            {
                var result = new BaseModel<Place>();
                var placePicker = new LocationPicker();
                var loc = await placePicker.GetCurrentLocation();
                if (loc.Data != null)
                {
                    await currentNav.PushModalAsync(placePicker);
                    var selectedPlace = await placePicker.GetPlace();
                    result.Data = selectedPlace;
                    result.Status = Status.Success;
                }
                else
                {
                    result.Status = loc.Status;
                }
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
