using System;
using Xamarin.Essentials;

namespace PlacePicker.Models
{
    public class Place
    {
        public Location Location { get; set; }
        public Placemark Placemark { get; set; }
        public string LocationAddress { get; set; }
    }
}
