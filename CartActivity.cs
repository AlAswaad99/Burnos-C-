using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Newtonsoft.Json;
using Project_OCS_Second.Adapters;
using Project_OCS_Second.DataModels;
using Project_OCS_Second.Services;

namespace Project_OCS_Second
{
    [Activity(Label = "CartActivity")]
    public class CartActivity : Activity, IEventListener
    {

        TextView mycart;
        RecyclerView cartrv;

        CartAdapter cartadapter;

        FirebaseFirestore database;

        DatabaseServices databaseservices = new DatabaseServices();

        List<Cart> cartresult = new List<Cart>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CartActivity);

            mycart = (TextView)FindViewById(Resource.Id.mycarttv);
            cartrv = (RecyclerView)FindViewById(Resource.Id.cartrecyclerview);
            //cartproductdelete = (ImageView)itemView.FindViewById(Resource.Id.cartdeleteiv);


            database = databaseservices.GetDataBase();

            FetchandListen();
            SetuprecyclerView();


            // Create your application here
        }



        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)value;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                cartresult.Clear();

                foreach (DocumentSnapshot item in documents)
                {
                    Cart product = new Cart();
                    product.ID = item.Id;
                    product.productname = item.Get("productname").ToString();
                    product.productimgurl = item.Get("imageurl") != null ? item.Get("imageurl").ToString() : "";
                    product.unitprice = item.GetLong("unitprice").IntValue() != 0 ? item.GetLong("unitprice").IntValue() : 1;
                    //item.Get("amount") != 0 ? item.Get("amount") : 1;
                    //user.ProfileImage = item.Get("profileimage") != null ? item.Get("profileimage").ToString() : "";

                    cartresult.Add(product);
                }

                if (cartadapter != null)
                {
                    cartadapter.NotifyDataSetChanged();
                }
            }

        }



        void FetchandListen()
        {
            //MainActivity service = new MainActivity();
            database.Collection("carts").AddSnapshotListener(this);
        }


        void SetuprecyclerView()
        {
            cartrv.SetLayoutManager(new LinearLayoutManager(cartrv.Context));
            cartadapter = new CartAdapter(cartresult);
            cartadapter.ItemClick += Cartadapter_ItemClick;
            cartadapter.DeleteItemClick += Cartadapter_DeleteItemClick;
            cartrv.SetAdapter(cartadapter);



        }

        private void Cartadapter_DeleteItemClick(object sender, CartAdapterClickEventArgs e)
        {
            var key = cartresult[e.Position].ID;


            Android.Support.V7.App.AlertDialog.Builder deletedataAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
            deletedataAlert.SetTitle("REMOVE FROM CART?");
            deletedataAlert.SetMessage("Are you sure?");
            deletedataAlert.SetPositiveButton("Continue", (senderAlert, args) =>
            {
                //services = new MainActivity();
                //FirebaseFirestore database = services.database;
                //services.deleteUser(userID);

                DocumentReference docRef = database.Collection("carts").Document(key);
                docRef.Delete();



                deletedataAlert.Dispose();

                Toast.MakeText(this, "Product Removed Successfully", ToastLength.Long);

            });
            deletedataAlert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                deletedataAlert.Dispose();
            });

            deletedataAlert.Show();

            //DocumentReference docref = database.Collection("carts").Document(key);
            //docref.Delete();
        }

        private void Cartadapter_ItemClick(object sender, CartAdapterClickEventArgs e)
        {
            Product selectedproduct = new Product();
            Cart selectedcart = new Cart();
            selectedcart = cartresult[e.Position];

            selectedproduct.productname = selectedcart.productname;
            selectedproduct.imageurl = selectedcart.productimgurl;

            Intent productpage = new Intent(this, typeof(ProductActivity));
            productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
            StartActivity(productpage);
        }

        

        


    }
}