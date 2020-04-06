
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace demo_app
{
    
    [Activity(Name= "com.mappp.MappAndroidSDKTest.DeepLinkActivity", Label = "DeepLinkActivity")]
    public class DeepLinkActivity : Activity
    {

        private const String DPL = "com.appoxee.VIEW_DEEPLINK";
        private String link = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_deeplink);
            // Create your application here

            Log.Info("DeeplinkActiviti-Xamarin", this.Intent.Action);

            Button openDeepLink = FindViewById<Button>(Resource.Id.open_link);
            openDeepLink.Click += openLink;

            Android.Net.Uri uri;
            if (this.Intent != null)
            {
                if ("com.appoxee.VIEW_DEEPLINK".Equals(this.Intent.Action))
                {
                    uri = this.Intent.Data;
                    //Data supplied from the front-end.
                    link = uri.GetQueryParameter("link");
                    ////This is the messageId
                    var messageId = uri.GetQueryParameter("message_id");
                    ////This is the eventTrigger only for version 5.0.7 and higher
                    var eventTrigger = uri.GetQueryParameter("event_trigger");
     
                }
            }

        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            Android.Net.Uri uri;
            if (intent != null)
            {
                if ("com.appoxee.VIEW_DEEPLINK".Equals(intent.Action))
                {
                    uri = intent.Data;
                    //Data supplied from the front-end.
                    link = uri.GetQueryParameter("link");
                    ////This is the messageId
                    String messageId = uri.GetQueryParameter("message_id");
                    ////This is the eventTrigger only for version 5.0.7 and higher
                    String eventTrigger = uri.GetQueryParameter("event_trigger");          

                }
            }
        }

        private void openLink(object sender, EventArgs e)
        {
            if (!link.Equals(""))
            {
                Intent newActivity = new Intent(Intent.ActionView);
                newActivity.SetData(Android.Net.Uri.Parse(link));
                StartActivity(newActivity);
            }
            
        }
    }
}
