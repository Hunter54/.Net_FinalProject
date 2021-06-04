using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;

namespace QuoteSaver
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, WindowSoftInputMode = Android.Views.SoftInput.AdjustPan)]
    [MetaData("android.app.searchable", Resource = "@xml/searchable")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private BottomNavigationView _navigation;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            _navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            _navigation.SetOnNavigationItemSelectedListener(this);
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.mainFragmentLayout, SearchQuoteFragment.NewInstance());
            transaction.Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    OpenFragment(SearchQuoteFragment.NewInstance());
                    return true;
                case Resource.Id.navigation_favorites:
                    OpenFragment(FavoritesFragment.NewInstance());
                    return true;
            }
            return false;
        }

        

        private void OpenFragment(Fragment fragment)
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.mainFragmentLayout, fragment);
            transaction.Commit();

        }
    }
}

