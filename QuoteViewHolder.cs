using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace QuoteSaver
{
    public class QuoteViewHolder : RecyclerView.ViewHolder
    {
        public TextView TvQuote { get; private set; }
        public Button BtnSaveQuote { get; private set; }
        public QuoteViewHolder(View itemView, Action<ClickEventArgs> clickListener) : base(itemView)
        {
            TvQuote = itemView.FindViewById<TextView>(Resource.Id.tvQuote);
            BtnSaveQuote = itemView.FindViewById<Button>(Resource.Id.btnSaveQuote);

            BtnSaveQuote.Click += (sender, e) => clickListener(
                new ClickEventArgs { View = itemView, Position = AdapterPosition});
        }
    }
}