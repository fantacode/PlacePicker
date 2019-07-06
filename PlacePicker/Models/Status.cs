using System;
using Xamarin.Forms.Internals;

namespace PlacePicker.Models
{
    [Preserve(AllMembers = true)]
    public enum Status
    {
        Success,
        Failed,
        Denied,
        Disabled,
        FeatureNotEnabled,
        NoInternet,
        Timeout,
        FeatureNotSupported,
        PermissionException,
        NSErrorException,
        Unknown
    }
}
