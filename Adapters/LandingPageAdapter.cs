using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace Project_OCS_Second.Fragments
{
    class LandingPageAdapter : FragmentPagerAdapter
    {
        // find the view

        Fragment[] zfragments;

        public LandingPageAdapter(FragmentManager fm, Fragment[] fragments) : base(fm)
        {
            zfragments = fragments;
        }

        public override int Count => zfragments.Length;

        public override Fragment GetItem(int position) => zfragments[position];



    }

   

}