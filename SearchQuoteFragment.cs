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
using Android.Arch.Lifecycle;
using Android.Support.V7.Widget;
using Newtonsoft.Json;
using SearchView = AndroidX.AppCompat.Widget.SearchView;
using Fragment = Android.Support.V4.App.Fragment;

namespace QuoteSaver
{
    public class SearchQuoteFragment : Fragment, SearchView.IOnQueryTextListener
    {
        private static SearchQuoteFragment _instance;
        private readonly PaperQuotesService _apiService = new PaperQuotesService();
        private RecyclerViewAdapter _mRecyclerViewAdapter;
        private RecyclerView _mRecyclerView;

        public static SearchQuoteFragment NewInstance()
        {
            return _instance ??= new SearchQuoteFragment();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            _mRecyclerViewAdapter = new RecyclerViewAdapter(GetString(Resource.String.save_button));
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var root = inflater.Inflate(Resource.Layout.search_quote_fragment, container, false);
            _mRecyclerView = root?.FindViewById<RecyclerView>(Resource.Id.rvSearchedQuotes);
            _mRecyclerView?.SetAdapter(_mRecyclerViewAdapter);
            _mRecyclerViewAdapter.ItemClick += SaveQuote;
            return root;
        }

        private void SaveQuote(object sender, ClickEventArgs e)
        {
            if (Activity == null)
            {
                return;
            }
            var sharedPref = Activity.GetPreferences(FileCreationMode.Private);
            var jsonString = sharedPref.GetString(Constants.SavedQuotesKey, "");
            List<Quote> items = JsonConvert.DeserializeObject<List<Quote>>(jsonString) ?? new List<Quote>();
            items.Add(_mRecyclerViewAdapter.Items[e.Position]);

            jsonString = JsonConvert.SerializeObject(items);
            var editor = sharedPref.Edit();
            editor?.PutString(Constants.SavedQuotesKey, jsonString.ToString());
            editor?.Apply();
            Toast.MakeText(Context, GetString(Resource.String.saved_message), ToastLength.Long).Show();
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            GetQuotes();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
            var actionMenuItem = menu.FindItem(Resource.Id.action_search);
            SearchView searchView = (SearchView) actionMenuItem.ActionView;
            searchView.SetOnQueryTextListener(this);
        }

        public bool OnQueryTextChange(string newText)
        {
            return false;
        }

        private async void GetSearchedQuotes(String searchText)
        {
            String[] tags = searchText.Split(' ');
            var items = await _apiService.GetSearchedQuoteByTagAsync(tags);
            if (items == null)
            {
                Toast.MakeText(Context,"Invalid Response", ToastLength.Short).Show();
            }
            else
            {
                _mRecyclerViewAdapter.SetItems(items);
            }
        }

        private async void GetQuotes()
        {
            var items = await _apiService.GetRandomCuratedQuotesAsync();
            if (items == null)
            {
                Toast.MakeText(Context, "Invalid Response", ToastLength.Short).Show();
            }
            else
            {
                _mRecyclerViewAdapter.SetItems(items);
            }
        }

        public bool OnQueryTextSubmit(string query)
        {
            if (Context != null)
            {
                GetSearchedQuotes(query);
            }

            return false;
        }
    }
}