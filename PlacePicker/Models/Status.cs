using System;
namespace PlacePicker.Models
{
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
