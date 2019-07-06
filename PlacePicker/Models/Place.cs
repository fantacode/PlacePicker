using System;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace PlacePicker.Models
{
    [Preserve(AllMembers = true)]
    public class Place
    {
        public Location Location { get; set; }
        public Placemark Placemark { get; set; }
        public string LocationAddress { get; set; }
    }
}
