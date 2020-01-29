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
using Android.Util;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Firebase.Firestore;
using Firebase.Storage;
using Java.Lang;
using Newtonsoft.Json;
using Project_OCS_Second.Adapters;
using Project_OCS_Second.DataModels;
using Project_OCS_Second.Services;

namespace Project_OCS_Second.Fragments
{
    public class HomePageFragment : Android.Support.V4.App.Fragment, IOnSuccessListener, IEventListener
    {

        TextView secfname;
        TextView secemail;
        TextView secpass;
        TextView secid;

        Button searchbtn;
        Button openmenubtn;

        ImageView secprofimg;

        RecyclerView recyclerView;
        DataAdapter dataAdapter;

        List<Product> displayproducts = new List<Product>();

        FirebaseFirestore database;

        //FirebaseStorage storage;


        public static User loggedinuser = new User();

        DatabaseServices databaseservices = new DatabaseServices();

        string imgurl;

        //Bundle zbundle;



        public HomePageFragment(User user)
        {
            loggedinuser = user;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            var view = inflater.Inflate(Resource.Layout.SecondPageActivity, container, false);

            secfname = (TextView)view.FindViewById(Resource.Id.secondpagefname);
            secemail = (TextView)view.FindViewById(Resource.Id.secondpageemail);
            secpass = (TextView)view.FindViewById(Resource.Id.secondpagepassword);
            secid = (TextView)view.FindViewById(Resource.Id.secondid);
            searchbtn = (Button)view.FindViewById(Resource.Id.searcharea);
            openmenubtn = (Button)view.FindViewById(Resource.Id.openmenubar);

            secprofimg = (ImageView)view.FindViewById(Resource.Id.secondprofimg);

            recyclerView = (RecyclerView)view.FindViewById(Resource.Id.secondpagerecycler);


            searchbtn.Click += (sender, e) =>
            {
                Intent searchpage = new Intent(Activity, typeof(SearchActivity));
                //productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
                StartActivity(searchpage);

            };

            openmenubtn.Click += (sender, e) =>
            {
                Intent menupage = new Intent(Activity, typeof(LandingPageActivity));
                //productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
                StartActivity(menupage);

            };

            database = databaseservices.GetDataBase();




            secfname.Text = loggedinuser.Fullname;
            secemail.Text = loggedinuser.Email;
            secpass.Text = loggedinuser.password;
            secid.Text = loggedinuser.ID;

            StorageReference ppref = FirebaseStorage.Instance.GetReference($"users/{loggedinuser.Fullname}_image");

            //StorageReference pathref = ppref.Child($"users/{loggedinuser.Fullname}_image");

            //StorageReference islandRef = storageRef.child("images/island.jpg");

            //final long ONE_MEGABYTE = 1024 * 1024;
            //

            ppref.GetDownloadUrl().AddOnSuccessListener(Activity, this);

            //imgurl = imgurl.ToString();

            Console.WriteLine("imgurl = " + imgurl);




            FetchandListen();
            SetuprecyclerView();




            return view;

        }



        void FetchandListen()
        {
            //MainActivity service = new MainActivity();
            database.Collection("products").AddSnapshotListener(this);
        }


        void SetuprecyclerView()
        {
            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            dataAdapter = new DataAdapter(displayproducts);
            dataAdapter.ItemClick += DataAdapter_ItemClick;
            recyclerView.SetAdapter(dataAdapter);
        }

        void DataAdapter_ItemClick(object sender, DataAdapterClickEventArgs e)
        {
            Product selectedproduct = new Product();
            selectedproduct = displayproducts[e.Position];

            Intent productpage = new Intent(Activity, typeof(ProductActivity));
            productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
            StartActivity(productpage);

        }
            public void OnSuccess(Java.Lang.Object result)
        {
            imgurl = result.ToString();

            Console.WriteLine("imgurl = " + imgurl);


            ImageService.Instance.LoadUrl(imgurl)
                .Retry(3, 200)
                .DownSample(100, 100)
                .Into(secprofimg);
        }

        public void OnEvent(Java.Lang.Object obj, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)obj;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                displayproducts.Clear();

                foreach (DocumentSnapshot item in documents)
                {
                    Product product = new Product();
                    product.ID = item.Id;
                    product.productname = item.Get("productname").ToString();
                    product.category = item.Get("category") != null ? item.Get("category").ToString() : "";
                    product.amount = item.GetLong("amount").IntValue() != 0 ? item.GetLong("amount").IntValue() : 1;
                    //item.Get("amount") != 0 ? item.Get("amount") : 1;
                    //user.ProfileImage = item.Get("profileimage") != null ? item.Get("profileimage").ToString() : "";

                    displayproducts.Add(product);
                }

                if (dataAdapter != null)
                {
                    dataAdapter.NotifyDataSetChanged();
                }
            }
        }
    }
}