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
    public class MenuBarFragment : Android.Support.V4.App.Fragment
    {
        string title;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if(Arguments != null)
            {
                if (Arguments.ContainsKey("title"))
                {
                    title = (string)Arguments.Get("title");
                }
            }

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            var view = inflater.Inflate(Resource.Layout.LandingPageFragmentLayout, container, false);

            var contenttitle = (TextView)view.FindViewById(Resource.Id.landingpagetitle);

            contenttitle.SetText(title, null);

            return view;

        }

        public static MenuBarFragment NewInstance(string title)
        {
            var menuitem = new MenuBarFragment();
            menuitem.Arguments = new Bundle();
            menuitem.Arguments.PutString("title", title);

            return menuitem;
        }
    }
}