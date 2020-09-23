using Android.App;
using Android.Widget;
using Android.OS;

using Appoxee;
using System;
using Android.Util;
using Java.Util;
using Android.Content;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android;
using Android.Content.PM;

namespace demo_app
{

    

    [Activity(Label = "@string/app_name", Name = "com.mappp.MappAndroidSDKTest.MainActivity", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity, EngageApoxee.IOnInitCompletedListener
    {
        private Switch pushEnable;
        private EngageApoxee appoxeeInstance;
        private Date DATE_FIELD = Calendar.Instance.Time;
        const int MY_PERMISSIONS_ACCESS_FINE_LOCATION = 1 << 3;
        const int MY_PERMISSIONS_ACCESS_FINE_AND_BACKGROUND_LOCATION = 1 << 4;

        public void OnInitCompleted(bool p0, Java.Lang.Exception p1)
        {
            Log.Info("APX", "init completed listener - MainActivity");
            RunOnUiThread(() => {


                pushEnable.Checked = appoxeeInstance.IsPushEnabled; ;
            });


        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            EngageApoxee.HandleRichPush(this, intent);

            Toast.MakeText(this, intent.Action, ToastLength.Long).Show();
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            EngageApoxee.HandleRichPush(this, this.Intent);

            TextView dateValue = FindViewById<TextView>(Resource.Id.date_value);
            dateValue.Text = DATE_FIELD + "";


            appoxeeInstance = EngageApoxee.Instance();
            appoxeeInstance.AddInitListener(this);

            pushEnable = FindViewById<Switch>(Resource.Id.push_enabled);
            pushEnable.Click += pushClick;

            Button setAlias = FindViewById<Button>(Resource.Id.set_alias);
            setAlias.Click += setDeviceAlias;

            Button getAlias = FindViewById<Button>(Resource.Id.get_alias);
            getAlias.Click += getDeviceAlias;

            Button setTag = FindViewById<Button>(Resource.Id.set_tag);
            setTag.Click += setDeviceTag;

            Button getTag = FindViewById<Button>(Resource.Id.get_tag);
            getTag.Click += getDeviceTag;

            Button removeTag = FindViewById<Button>(Resource.Id.remove_tag);
            removeTag.Click += removeDeviceTag;

            Button setNumericAttrBtn = FindViewById<Button>(Resource.Id.set_numeric_attr);
            setNumericAttrBtn.Click += setNumericAttr;

            Button setStringAttrBtn = FindViewById<Button>(Resource.Id.set_string_attr);
            setStringAttrBtn.Click += setStringAttr;

            Button setDateAttrBtn = FindViewById<Button>(Resource.Id.set_date_attr);
            setDateAttrBtn.Click += setDateAttr;

            Button getCustomAttrBtn = FindViewById<Button>(Resource.Id.get_attr);
            getCustomAttrBtn.Click += getCustomAttr;

            Button deviceInfoBtn = FindViewById<Button>(Resource.Id.device_info);
            deviceInfoBtn.Click += deviceInfo;

            Button inAppMessageBtn = FindViewById<Button>(Resource.Id.in_app_message);
            inAppMessageBtn.Click += openInApp;

            Button geoButton = FindViewById<Button>(Resource.Id.geo_start);
            inAppMessageBtn.Click += startGeo;

            
        }

        private void deviceInfo(object sender, EventArgs e)
        {
            DeviceInfo info = appoxeeInstance.DeviceInfo;

            TextView deviceInfoTextView = FindViewById<TextView>(Resource.Id.device_info_text);
            string value = "app Version:" + info.AppVersion + "device model:" + info.DeviceModel + "Manufacturer:" + info.Manufacturer
                + "os version:" + info.OsVersion + "resolution:" + info.Resolution + "sdk version:" + info.SdkVersion + "timeZone:" + info.Timezone + "" + info.Locale;
            deviceInfoTextView.Text = value;

        }

        private void openInApp(object sender, EventArgs e)
        {
            appoxeeInstance.TriggerDMCCallInApp(this, "app_open");
        }

        private void getCustomAttr(object sender, EventArgs e)
        {
            EditText key = FindViewById<EditText>(Resource.Id.key);
            if (key.Text.Length > 0)
            {
                string value = appoxeeInstance.GetAttributeStringValue(key.Text);
                TextView valueTextView = FindViewById<TextView>(Resource.Id.value);
                valueTextView.Text = value;
            }
        }

        private void setDateAttr(object sender, EventArgs e)
        {
            EditText dateKey = FindViewById<EditText>(Resource.Id.date_key);

            if (dateKey.Text.Length > 0)
            {
                appoxeeInstance.SetAttribute(dateKey.Text, DATE_FIELD);
            }
        }

        private void setStringAttr(object sender, EventArgs e)
        {
            EditText stringKey = FindViewById<EditText>(Resource.Id.string_key);
            EditText stringValue = FindViewById<EditText>(Resource.Id.string_value);
            if (stringKey.Text.Length > 0 && stringValue.Text.Length > 0)
            {

                appoxeeInstance.SetAttribute(stringKey.Text, stringValue.Text);
            }
        }

        private void setNumericAttr(object sender, EventArgs e)
        {
            EditText numericKey = FindViewById<EditText>(Resource.Id.numeric_key);
            EditText numericValue = FindViewById<EditText>(Resource.Id.numeric_value);
            if (numericKey.Text.Length > 0 && numericValue.Text.Length > 0)
            {
                int v = Convert.ToInt32(Convert.ToDecimal(numericValue.Text));

                Java.Lang.Integer intObj = new Java.Lang.Integer(v);
                Java.Lang.Number numObj = (Java.Lang.Number)intObj;
                appoxeeInstance.SetAttribute(numericKey.Text, numObj);
            }
        }
        
        private void removeDeviceTag(object sender, EventArgs e)
        {
            EditText removeTagValue = FindViewById<EditText>(Resource.Id.remove_tag_text);
            if (removeTagValue.Text.Length > 0)
            {
                appoxeeInstance.RemoveTag(removeTagValue.Text);
            }
        }

        private void getDeviceTag(object sender, EventArgs e)
        {
            TextView tagsTextView = FindViewById<TextView>(Resource.Id.tags_text);
            System.Collections.Generic.ICollection<string> tags = appoxeeInstance.Tags;
            string[] strArray = new string[tags.Count];
            tags.CopyTo(strArray, 0);
            string tagValues = string.Join(",", strArray);
            tagsTextView.Text = tagValues;

        }

        private void setDeviceTag(object sender, EventArgs e)
        {
            EditText tagEditText = FindViewById<EditText>(Resource.Id.enter_tag);
            if (tagEditText.Text.Length > 0)
            {
                appoxeeInstance.AddTag(tagEditText.Text);
            }
        }

        private void getDeviceAlias(object sender, EventArgs e)
        {

            string alias = appoxeeInstance.Alias;
            TextView aliasView = FindViewById<TextView>(Resource.Id.alias_text);
            aliasView.Text = alias;
        }

        private void setDeviceAlias(object sender, EventArgs e)
        {
            EditText aliasTextView = FindViewById<EditText>(Resource.Id.enter_alias);
            if (aliasTextView.Text.Length > 0)
            {
                appoxeeInstance.SetAlias(aliasTextView.Text);
            }
        }

        private void pushClick(object sender, EventArgs e)
        {
            appoxeeInstance.SetPushEnabled(pushEnable.Checked);
        }

        private void startGeo(object sender, EventArgs e)
        {

            const string permission = Manifest.Permission.AccessFineLocation;
          

                if (ContextCompat.CheckSelfPermission(this, permission) == Permission.Granted)
                {
                    appoxeeInstance.StartGeoFencing();
                }
                else
                {
                var requiredPermissions = new String[] { Manifest.Permission.AccessFineLocation };
                ActivityCompat.RequestPermissions(this, requiredPermissions, MY_PERMISSIONS_ACCESS_FINE_LOCATION);           
              
            }
           
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);


            if (requestCode == MY_PERMISSIONS_ACCESS_FINE_LOCATION)
            {
                if ((grantResults.Length >= 1) && (grantResults[0] == Permission.Granted))
                {
                    appoxeeInstance.StartGeoFencing();
                }
            }
        }
    }
}