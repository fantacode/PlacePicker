using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace PlacePicker.Helpers
{
    [Preserve(AllMembers = true)]
    public class LocationAddress
    {
        public static string GetLocationString(Placemark placemark)
        {
            string address;
            if (placemark != null)
            {
                var array = new[] { placemark.FeatureName, placemark.Locality, placemark.AdminArea, placemark.PostalCode, placemark.CountryName };
                if (!placemark.FeatureName.Any(char.IsDigit))
                {
                    if (CheckSimilar(array, 0))
                    {
                        array = array.Skip(1).ToArray();
                        address = string.Join(", ", array.Where(s => !string.IsNullOrWhiteSpace(s)));
                    }
                    else
                        address = string.Join(", ", array.Where(s => !string.IsNullOrWhiteSpace(s)));
                }
                else
                {
                    if (CheckSimilar(array, 1))
                    {
                        array = array.Skip(2).ToArray();
                        address = string.Join(", ", array.Where(s => !string.IsNullOrWhiteSpace(s)));
                    }
                    else
                    {
                        array = array.Skip(1).ToArray();
                        address = string.Join(", ", array.Where(s => !string.IsNullOrWhiteSpace(s)));
                    }
                }
                
                return address;
            }
            return string.Empty;
        }

        private static bool CheckSimilar(string[] array, int start)
        {
            string item = array[start];
            bool allEqual = array.Skip(start + 1)
                .Any(s => string.Equals(item, s, StringComparison.InvariantCultureIgnoreCase));
            return allEqual;
        }
    }
}
