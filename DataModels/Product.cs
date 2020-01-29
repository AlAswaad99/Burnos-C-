using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Project_OCS_Second.DataModels
{
    class Product
    {
        public string ID { get; set; }
        public string productname { get; set; }
        public string category { get; set; }
        public string imageurl { get; set; }
        public int amount { get; set; }
    }
}