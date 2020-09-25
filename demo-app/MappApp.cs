using System;

using Android.App;
//using Android.Content;
//using Android.OS;
using Android.Runtime;
using Appoxee;
using Android.Gms.Common;
using Appoxee.Push;

namespace demo_app
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [Application]
    public class MappApp : Application
    {
        const string TAG = "MyFirebaseIIDService";
        public MappApp(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            AppoxeeOptions opt = new AppoxeeOptions();
            opt.SdkKey = "174bfe22eaf5f6.12942055";
            opt.GoogleProjectId = "1028993954364";
            opt.CepURL = "https://jamie-test.shortest-route.com";
            opt.AppID = "1585031493383";
            opt.TenantID = "33";
            opt.NotificationMode = NotificationModes.BackgroundAndForeground;

            opt.Server = AppoxeeOptions.ServerForUsing.Test;
            opt.LogLevel = AppoxeeOptions.LogLevels.ClientDebug;
            Appoxee.EngageApoxee.Engage(this, opt);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Android.Util.Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Android.Util.Log.Debug(TAG, "This device is not supported");
                }
                return false;
            }
            else
            {
                Android.Util.Log.Debug(TAG, "Google Play Services is available.");
                return true;
            }
        }
    }
}
