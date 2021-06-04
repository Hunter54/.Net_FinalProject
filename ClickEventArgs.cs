using System;
using Android.Views;

namespace QuoteSaver
{
    public class ClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}