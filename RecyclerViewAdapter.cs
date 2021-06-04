using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;

namespace QuoteSaver
{
    public class RecyclerViewAdapter : RecyclerView.Adapter
    {
        public List<Quote> Items = new List<Quote>();
        public event EventHandler<ClickEventArgs> ItemClick;

        public RecyclerViewAdapter(String buttonMessage)
        {
            ButtonMessage = buttonMessage;
        }

        public string ButtonMessage { get; set; }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            QuoteViewHolder mViewHolder = holder as QuoteViewHolder;
            mViewHolder.TvQuote.Text = Items[position].QuoteText;
            mViewHolder.BtnSaveQuote.Text = ButtonMessage;
        }

        public void SetItems(List<Quote> items)
        {
            Items = items.ToList();
            NotifyDataSetChanged();
        }

        public void RemoveItem(int position)
        {
            Items.RemoveAt(position);
            NotifyItemRemoved(position);
            NotifyItemRangeChanged(position, ItemCount - position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.list_item_quote, parent, false);

            QuoteViewHolder mViewHolder = new QuoteViewHolder(itemView, OnClick);
            return mViewHolder;
        }

        public override int ItemCount => Items.Count;
        private void OnClick(ClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        
    }
}