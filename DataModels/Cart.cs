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
    class Cart
    {
        public string ID { get; set; }
        public string productname { get; set; }
        public string buyername { get; set; }
        public string productimgurl { get; set; }
        public int unitprice { get; set; }
        public int productamount { get; set; }

    }
}