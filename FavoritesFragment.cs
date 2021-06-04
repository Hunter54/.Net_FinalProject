using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.OS.Strictmode;
using Android.Support.V4.Content.Res;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using Fragment = Android.Support.V4.App.Fragment;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace QuoteSaver
{
    public class FavoritesFragment : Fragment , SearchView.IOnQueryTextListener
    {
        private static FavoritesFragment _instance;
        private RecyclerViewAdapter _mRecyclerViewAdapter;
        private RecyclerView _mRecyclerView;

        public static FavoritesFragment NewInstance()
        {
            return _instance ??= new FavoritesFragment();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            _mRecyclerViewAdapter = new RecyclerViewAdapter(GetString(Resource.String.delete_button));
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var root = inflater.Inflate(Resource.Layout.favorites_fragment, container, false);

            _mRecyclerView = root?.FindViewById<RecyclerView>(Resource.Id.rvSavedQuotes);
            _mRecyclerView.HasFixedSize = false;
            _mRecyclerView?.SetAdapter(_mRecyclerViewAdapter);
            _mRecyclerViewAdapter.ItemClick += DeleteClicked;
            return root;
        }

        public override void OnResume()
        {
            var sharedPref = Activity.GetPreferences(FileCreationMode.Private);
            var jsonString = sharedPref.GetString(Constants.SavedQuotesKey, "");
            List<Quote> items = JsonConvert.DeserializeObject<List<Quote>>(jsonString) ?? new List<Quote>();

            _mRecyclerViewAdapter.SetItems(items);
            base.OnResume();
        }

        private void DeleteClicked(object sender, ClickEventArgs e)
        {
            if (Activity == null)
            {
                return;
            }

            var itemToRemove = _mRecyclerViewAdapter.Items[e.Position];
            var sharedPref = Activity.GetPreferences(FileCreationMode.Private);
            var jsonString = sharedPref.GetString(Constants.SavedQuotesKey, "");
            List<Quote> items = JsonConvert.DeserializeObject<List<Quote>>(jsonString) ?? new List<Quote>();
            var itemToRemoveFromMemory = items.Find(x => x.QuoteText.Equals(itemToRemove.QuoteText));
            items.Remove(itemToRemoveFromMemory);

            jsonString = JsonConvert.SerializeObject(items);
            var editor = sharedPref.Edit();
            editor?.PutString(Constants.SavedQuotesKey, jsonString.ToString());
            editor?.Apply();
            _mRecyclerViewAdapter.RemoveItem(e.Position);
            Toast.MakeText(Context, GetString(Resource.String.delete_message), ToastLength.Long).Show();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
            var actionMenuItem = menu.FindItem(Resource.Id.action_search);
            SearchView searchView = (SearchView)actionMenuItem.ActionView;
            searchView.SetOnQueryTextListener(this);
        }

        public bool OnQueryTextChange(string newText)
        {
            return false;
        }

        public bool OnQueryTextSubmit(string query)
        {
            if (Context != null)
            {
                Toast.MakeText(Context, "FavoritesQueryTextSubmit", ToastLength.Long).Show();
            }

            return false;
        }
    }
}