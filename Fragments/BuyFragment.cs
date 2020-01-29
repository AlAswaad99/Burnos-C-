using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Project_OCS_Second.Fragments
{
    public class BuyFragment : Android.Support.V4.App.DialogFragment
    {

        TextView transtext;
        TextView transdetail;
        //TextView buyername;



        
        // Button backbtn;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.TransactionFragment, container, false);
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            transtext = (TextView)view.FindViewById(Resource.Id.buysuccesstext);
            transdetail = (TextView)view.FindViewById(Resource.Id.buysuccessdetail);
           // buyername = (TextView)view.FindViewById(Resource.Id.buyerdetail);
            //backbtn = (Button)view.FindViewById(Resource.Id.returnbtn);

            //buyername.Text = username.ToUpper();

            //backbtn.Click += (sender, e) => { 

           

            //};

            return view;
        }
    }
}