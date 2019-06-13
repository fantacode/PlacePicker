using System;
using System.Linq;
using System.Threading.Tasks;
using PlacePicker.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;

namespace PlacePicker.Helpers
{
    public class GetUserLocation
    {
        public async Task<BaseModel<Location>> GetPosition()
        {
            var result = new BaseModel<Location>();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                    if (status == PermissionStatus.Granted)
                    {
                        result = await GetLocation();
                    }
                    else
                    {
                        try
                        {
                            var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                            //Best practice to always check that the key exists
                            if (results.ContainsKey(Permission.Location))
                                status = results[Permission.Location];
                            if (status == PermissionStatus.Granted)
                            {
                                result = await GetLocation();
                            }
                            else if (status == PermissionStatus.Denied)
                            {
                                result.Data = null;
                                result.Status = Status.Denied;
                            }
                            else if (status == PermissionStatus.Disabled)
                            {
                                result.Data = null;
                                result.Status = Status.Disabled;
                            }
                            else if (status != PermissionStatus.Granted)
                            {
                                var stat = await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location);
                                if (stat)
                                {
                                    result.Data = null;
                                    result.Status = Status.Unknown;
                                }
                                else
                                {
                                    result.Data = null;
                                    result.Status = Status.Unknown;
                                }
                            }
                            else
                            {
                                result.Data = null;
                                result.Status = Status.Unknown;
                            }
                        }
                        catch (Exception e)
                        {
                            result.Data = null;
                            result.Status = Status.Unknown;
                        }
                    }
                }
                catch (FeatureNotEnabledException)
                {
                    result.Data = null;
                    result.Status = Status.FeatureNotEnabled;
                }
                catch (Exception)
                {
                    result.Data = null;
                    result.Status = Status.Failed;
                }
            }
            else
            {
                result.Data = null;
                result.Status = Status.NoInternet;
            }
            return result;
        }

        async Task<BaseModel<Location>> GetLocation()
        {
            var result = new BaseModel<Location>();
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var loc = Geolocation.GetLastKnownLocationAsync();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (await Task.WhenAny(loc, Task.Delay(5000)) == loc)
                {
                    if (loc?.Result != null)
                    {
                        result.Data = loc.Result;
                        result.Status = Status.Success;
                    }
                    else
                    {
                        var location = await Geolocation.GetLocationAsync(request);
                        if (location != null)
                        {
                            result.Data = location;
                            result.Status = Status.Success;
                        }
                        else
                        {
                            result.Data = null;
                            result.Status = Status.Timeout;
                        }
                    }
                }
                else
                {
                    var location = await Geolocation.GetLocationAsync(request);
                    if (location != null)
                    {
                        result.Data = location;
                        result.Status = Status.Success;
                    }
                    else
                    {
                        result.Data = null;
                        result.Status = Status.Timeout;
                    }
                }
            }
            else
            {
                result.Data = null;
                result.Status = Status.NoInternet;
            }
            return result;
        }

        public async Task<BaseModel<Placemark>> GetAddress(Location location)
        {
            var result = new BaseModel<Placemark>();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location);
                    if (placemarks != null)
                    {
                        var placemark = placemarks?.FirstOrDefault();
                        if (placemark != null)
                        {
                            result.Data = placemark;
                            result.Status = Status.Success;
                        }
                    }
                }
                catch (FeatureNotSupportedException)
                {
                    result.Data = null;
                    result.Status = Status.FeatureNotSupported;
                }
                catch (Exception)
                {
                    result.Data = null;
                    result.Status = Status.Failed;
                }
            }
            else
            {
                result.Data = null;
                result.Status = Status.NoInternet;
            }
            return result;
        }

        public void Dispose()
        {
            Geolocation.GetLocationAsync().Dispose();
        }
    }
}
