using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Firebase.Storage;
using Java.Lang;
using Newtonsoft.Json;
using Project_OCS_Second.Adapters;
using Project_OCS_Second.DataModels;
using Project_OCS_Second.Services;

namespace Project_OCS_Second
{
    [Activity(Label = "SearchActivity")]
    public class SearchActivity : Activity, IEventListener,IOnSuccessListener
    {

        EditText searchtext;
        RecyclerView searchrv;


        List<Product> searchproducts = new List<Product>();
        List<Product> SearchResult = new List<Product>();
        //List<string> imagelist = new List<string>();
        //List<string> searchresultimagelist = new List<string>();

        //string imgurl;

        FirebaseFirestore database;

        DatabaseServices databaseservices = new DatabaseServices();


        SearchAdapter searchadapter;

        User buyinguser = new User();

        string username;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SearchActivity);

            searchtext = (EditText)FindViewById(Resource.Id.searchtext);
            searchrv = (RecyclerView)FindViewById(Resource.Id.searchrecyclerview);

            searchtext.Text = "";



            database = databaseservices.GetDataBase();

            FetchandListen();
            SetuprecyclerView();


            

            
            // Create your application here
        }

        public void OnEvent(Java.Lang.Object obj, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)obj;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                searchproducts.Clear();

                foreach (DocumentSnapshot item in documents)
                {
                    Product product = new Product();
                    product.ID = item.Id;
                    product.productname = item.Get("productname").ToString();
                    product.category = item.Get("category") != null ? item.Get("category").ToString() : "";
                    product.imageurl = item.Get("imageurl") != null ? item.Get("imageurl").ToString() : "";
                    product.amount = item.GetLong("amount").IntValue() != 0 ? item.GetLong("amount").IntValue() : 1;


                    //item.Get("amount") != 0 ? item.Get("amount") : 1;
                    //user.ProfileImage = item.Get("profileimage") != null ? item.Get("profileimage").ToString() : "";

                    searchproducts.Add(product);

                    buyinguser = SecondPageActivity.loggedinuser;

                    username = buyinguser.Fullname;


                    //imagelist.Add($"{product.productname}_image.jpg");

                    //StorageReference ppref = FirebaseStorage.Instance.GetReference($"users/{username}_image");

                    //ppref.GetDownloadUrl().AddOnSuccessListener(this, this);
                }

                if (searchadapter != null)
                {
                    searchadapter.NotifyDataSetChanged();
                }
            }
        }

        void FetchandListen()
        {
            //MainActivity service = new MainActivity();
            database.Collection("products").AddSnapshotListener(this);

            

        }



        void SetuprecyclerView()
        {
            //imagelist.Clear();

            //buyinguser = SecondPageActivity.loggedinuser;

            //username = buyinguser.Fullname;
            //foreach (var item in searchproducts)
            //{
            //    imagelist.Add($"{username}_image");

            //}
            searchrv.SetLayoutManager(new LinearLayoutManager(searchrv.Context));
            searchadapter = new SearchAdapter(searchproducts);
            searchadapter.ItemClick += Searchadapter_ItemClick;
            searchrv.SetAdapter(searchadapter);

            searchrv.Visibility = ViewStates.Invisible;

            searchtext.TextChanged += (sender, e) =>
            {
                SearchResult =
                (from product in searchproducts
                 where product.productname.ToLower().Contains(searchtext.Text.ToLower()) || product.category.ToLower().Contains(searchtext.Text.ToLower())
                 select product).ToList();

                //foreach(var item in SearchResult)
                //{
                //    StorageReference ppref = FirebaseStorage.Instance.GetReference($"products/{item.productname}_image");

                //    ppref.GetDownloadUrl().AddOnSuccessListener(this, this);
                //    searchresultimagelist.Add(
                //}
                //searchresultimagelist.Clear();
                //buyinguser = SecondPageActivity.loggedinuser;

                //username = buyinguser.Fullname;
                //foreach (var item in SearchResult)
                //{
                //    searchresultimagelist.Add($"{item.productname}_image.jpg");
                //}

                searchadapter = new SearchAdapter(SearchResult);
                searchadapter.ItemClick += SearchResult_ItemClick;
                searchrv.SetAdapter(searchadapter);

                if (searchtext.Text == "")
                {
                    searchrv.Visibility = ViewStates.Invisible;

                }
                searchrv.Visibility = ViewStates.Visible;


            };

        }

        private void Searchadapter_ItemClick(object sender, SearchAdapterClickEventArgs e)
        {
            Product selectedproduct = new Product();
            selectedproduct = searchproducts[e.Position];

            Intent productpage = new Intent(this, typeof(ProductActivity));
            productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
            StartActivity(productpage);
        }

        private void SearchResult_ItemClick(object sender, SearchAdapterClickEventArgs e)
        {
            Product selectedproduct = new Product();
            selectedproduct = SearchResult[e.Position];

            Intent productpage = new Intent(this, typeof(ProductActivity));
            productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
            StartActivity(productpage);
        }


        

        public void OnSuccess(Java.Lang.Object result)
        {
            //imgurl = result.ToString();

            //imagelist.Add(imgurl);
        }
    }
}