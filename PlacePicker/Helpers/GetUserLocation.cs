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
                    if (status != PermissionStatus.Granted)
                    {
                        if(await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                        {
                            result.Data = null;
                            result.Status = Status.Failed;
                        }
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                        if (results.ContainsKey(Permission.Location))
                            status = results[Permission.Location];
                    }
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
                    else
                    {
                        result.Data = null;
                        result.Status = Status.Unknown;
                    }
                }
                catch(Exception)
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
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
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
                        result.Status = Status.Failed;
                    }
                }
                catch (FeatureNotEnabledException)
                {
                    result.Data = null;
                    result.Status = Status.FeatureNotEnabled;
                }
                catch (FeatureNotSupportedException)
                {
                    result.Data = null;
                    result.Status = Status.FeatureNotSupported;
                }
                catch (PermissionException)
                {
                    result.Data = null;
                    result.Status = Status.PermissionException;
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
                    else
                    {
                        result.Data = null;
                        result.Status = Status.Failed;
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
