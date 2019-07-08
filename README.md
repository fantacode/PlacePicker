# PlacePicker for Xamarin Forms

Place Picker is a simple cross-platform plugin which allows to pick a location and get location address from Google Maps with the help of Xamarin Essentials Geolocation & Geocoding.

![Image](https://github.com/fantacode/PlacePicker/blob/master/nuget/view.PNG)

- Location Entry shows the location address. (Cannot be edited)
- **Confirm Location** -> returns the location response.
- **Cancel** -> stops the operation and pops the view.

## Supported Platforms

Follow Minimum support versions required for Xamarin Essentials & Permissions Plugin.

- iOS
- Android

## Installation

- Available on Nuget : https://www.nuget.org/packages/PlacePicker/
- Install into your PCL Projects & Client Projects.

## Permission & Setup Information

These steps must be implemented for usage of Xamarin Forms Google Maps & Xamarin Essentials Geolocation.

### Android

- Open the **AssemblyInfo.cs** file under the **Properties** and add :

 ```
[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]
 ```
- Open **MainActivity.cs** file, initialise the following inside the **OnCreate** method :

```
   Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
   Xamarin.FormsGoogleMaps.Init(this, bundle);
   Xamarin.Essentials.Platform.Init(this, bundle);
```

- To use the Google Maps on Android you must generate an API key and add it to your Android project. 
  Open **AndroidManifest.xml** file, and update the following in `application` tag
  
  ```
  <application>
	<meta-data android:name="com.google.android.geo.API_KEY" android:value="YOUR-API-KEY-HERE" />
	<uses-library android:name="org.apache.http.legacy" android:required="false" />
  </application>
  ```

### iOS

- Your app's **Info.plist** must contain the `NSLocationWhenInUseUsageDescription` key in order to access the device’s location.

Open the plist editor and add the **Privacy - Location When In Use Usage Description** property and fill in a value to display to the user.

Or manually edit the file and add the following :

```
<key>NSLocationAlwaysUsageDescription</key>
<string>Can we use your location at all times?</string>
<key>NSLocationWhenInUseUsageDescription</key>
<string>Can we use your location when your app is being used?</string>
<key>NSLocationAlwaysAndWhenInUseUsageDescription</key>
<string>Can we use your location at all times?</string>
```
- Open **AppDelegate.cs** file, and initialise :

```
Xamarin.FormsGoogleMaps.Init("YOUR-API-KEY-HERE");
```

## Usage

- You can set the map pin icon as follows :

```
LocationPicker.PinImage = "pin.png";
```

Make sure to add the required image resource file to corresponsing iOS & Android Projects.

- Get Location call

```
var place = await LocationPicker.SelectPlace();
if (place?.Data != null)
{
   // Do the necessary code
}
```
The **Result** class :
```
public class BaseModel<Place>
{
    public Place Data { get; set; }

    public Status Status { get; set; }
}
```
Place model :
```
public class Place
{
    public Location Location { get; set; }
    public Placemark Placemark { get; set; }
    public string LocationAddress { get; set; }
}
```

Status model :
```
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
```
Note : - The Response Status is based on location permissions & exceptions returned by the essentials plugin, you may do the action or show alerts as required.

**The Response Status gives Success and returns the location data, only if locations permissions are enabled and can be accessed by the app.**

## DISCLAIMER

This is a prerelease, which has been made according to our requirements. I don't guarantee the performance and code quality of this library, this is an experimentation and all are welcome to send PR for improving performance or finding more simplified ways.



