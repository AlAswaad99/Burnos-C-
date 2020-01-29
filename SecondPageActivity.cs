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
using Project_OCS_Second.DataModels;
using Newtonsoft.Json;
using Android.Support.V7.Widget;
using Project_OCS_Second.Adapters;
using Firebase.Firestore;
using Java.Lang;
using Firebase;
using Project_OCS_Second;
using FFImageLoading;
using Firebase.Storage;
using Android.Gms.Tasks;
using Project_OCS_Second.Services;

namespace Project_OCS_Second
{
    [Activity(Label = "SecondPageActivity")]
    public class SecondPageActivity : Activity, Firebase.Firestore.IEventListener, IOnSuccessListener
    {


        TextView secfname;
        TextView secemail;
        TextView secpass;
        TextView secid;

        Button searchbtn;
        Button openmenubtn;
        Button gotocartbtn;

        ImageView secprofimg;

        RecyclerView recyclerView;
        DataAdapter dataAdapter;

        List<Product> displayproducts = new List<Product>();

        FirebaseFirestore database;

        //FirebaseStorage storage;


        public static User loggedinuser = new User();

        DatabaseServices databaseservices = new DatabaseServices();

        //string imgurl;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SecondPageActivity);

            secfname = (TextView)FindViewById(Resource.Id.secondpagefname);
            secemail = (TextView)FindViewById(Resource.Id.secondpageemail);
            secpass = (TextView)FindViewById(Resource.Id.secondpagepassword);
            secid = (TextView)FindViewById(Resource.Id.secondid);
            searchbtn = (Button)FindViewById(Resource.Id.searcharea);
            openmenubtn = (Button)FindViewById(Resource.Id.openmenubar);
            gotocartbtn = (Button)FindViewById(Resource.Id.gotocart);

            secprofimg = (ImageView)FindViewById(Resource.Id.secondprofimg);

            recyclerView = (RecyclerView)FindViewById(Resource.Id.secondpagerecycler);

            loggedinuser = JsonConvert.DeserializeObject<User>(Intent.GetStringExtra("accepteduser"));


            searchbtn.Click += (sender, e) =>
            {
                Intent searchpage = new Intent(this, typeof(SearchActivity));
                //productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
                StartActivity(searchpage);

            };

            openmenubtn.Click += (sender, e) =>
            {
                Intent menupage = new Intent(this, typeof(LandingPageActivity));
                menupage.PutExtra("accepteduser", JsonConvert.SerializeObject(loggedinuser));
                StartActivity(menupage);

            };

            gotocartbtn.Click += (sender, e) =>
            {
                Intent cartpage = new Intent(this, typeof(CartActivity));
                //menupage.PutExtra("accepteduser", JsonConvert.SerializeObject(loggedinuser));
                StartActivity(cartpage);

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

            ppref.GetDownloadUrl().AddOnSuccessListener(this, this);

            //imgurl = imgurl.ToString();

            //Console.WriteLine("imgurl = " + imgurl);


            //Console.WriteLine("imgurl = " + imgurl);


            


            FetchandListen();
            SetuprecyclerView();


            // Create your application here
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

            Intent productpage = new Intent(this, typeof(ProductActivity));
            productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
            StartActivity(productpage);





            //string userID = displayproducts[e.Position].ID;


            //Android.Support.V7.App.AlertDialog.Builder deletedataAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
            //deletedataAlert.SetTitle("DELETE USER?");
            //deletedataAlert.SetMessage("Are you sure?");
            //deletedataAlert.SetPositiveButton("Continue", (senderAlert, args) =>
            //{
            //    //services = new MainActivity();
            //    //FirebaseFirestore database = services.database;
            //    //services.deleteUser(userID);

            //    DocumentReference docRef = database.Collection("users").Document(userID);
            //    docRef.Delete();



            //    deletedataAlert.Dispose();

            //    Toast.MakeText(this, "User Successfully Deleted", ToastLength.Long);

            //});
            //deletedataAlert.SetNegativeButton("Cancel", (senderAlert, args) =>
            //{
            //    deletedataAlert.Dispose();
            //});

            //deletedataAlert.Show();


        }


        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)value;

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

        public void OnSuccess(Java.Lang.Object result)
        {
            ImageService.Instance.LoadUrl(result.ToString())
                .Retry(3, 200)
                .DownSample(100, 100)
                .Into(secprofimg);
        }
    }
}