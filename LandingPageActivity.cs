using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Project_OCS_Second.DataModels;
using Project_OCS_Second.Fragments;

namespace Project_OCS_Second
{
    [Activity(Label = "LandingPageActivity")]
    public class LandingPageActivity : FragmentActivity
    {


        ViewPager viewpager; 

        BottomNavigationView bottommenubar;
        Android.Support.V4.App.Fragment[] fragments;

        public static User loggedinuser = new User();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.LandingPageActivity);

            loggedinuser = JsonConvert.DeserializeObject<User>(Intent.GetStringExtra("accepteduser"));


            InitializeTabs();

            bottommenubar = (BottomNavigationView)FindViewById(Resource.Id.bottom_navigation);
            viewpager = (ViewPager)FindViewById(Resource.Id.viewpager);
            viewpager.PageSelected += Viewpager_PageSelected;

            viewpager.Adapter = new LandingPageAdapter(SupportFragmentManager, fragments);


            RemoveShiftMode(bottommenubar);
            bottommenubar.NavigationItemSelected += Bottommenubar_NavigationItemSelected;


           

        }

        private void Bottommenubar_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            viewpager.SetCurrentItem(e.Item.Order, true);
        }

        private void Viewpager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            var item = bottommenubar.Menu.GetItem(e.Position);
            bottommenubar.SelectedItemId = item.ItemId;

        }

        void InitializeTabs()
        {
            
           


            fragments = new Android.Support.V4.App.Fragment[] {
                new HomePageFragment(loggedinuser),
                MenuBarFragment.NewInstance("Titles"),
                MenuBarFragment.NewInstance("Stream"),
                MenuBarFragment.NewInstance("Showtimes") };

        }

        private void RemoveShiftMode(BottomNavigationView bottommenubar)
        {
            var menuView = (BottomNavigationMenuView)bottommenubar.GetChildAt(0);
            try
            {
                var shiftingMode = menuView.Class.GetDeclaredField("mShiftingMode");
                shiftingMode.Accessible = true;
                shiftingMode.SetBoolean(menuView, false);
                shiftingMode.Accessible = false;

                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    var item = (BottomNavigationItemView)menuView.GetChildAt(i);
                    item.SetShifting(false);
                    // set checked value, so view will be updated
                    item.SetChecked(item.ItemData.IsChecked);
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine((ex.InnerException ?? ex).Message);
            }
        }
    }
}