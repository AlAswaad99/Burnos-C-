using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Util;
using Newtonsoft.Json;
using Plugin.Messaging;
using Project_OCS_Second.DataModels;
using Project_OCS_Second.Fragments;
using Project_OCS_Second.Services;
using Random = System.Random;

namespace Project_OCS_Second
{
    [Activity(Label = "ProductActivity")]
    public class ProductActivity : AppCompatActivity, IOnSuccessListener
    {

        TextView prodetname;
        TextView prodetcat;
        TextView prodetamount;

        //ImageView prodetimage;

        public FirebaseFirestore database;

        DatabaseServices databaseservice = new DatabaseServices();

        List<Cart> cartitems = new List<Cart>();


        Product selectedProduct;

        Button buy;
        Button cart;

        User buyinguser = new User();

        string username;

        bool productadded;






        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ProductAtivity);


            prodetname = (TextView)FindViewById(Resource.Id.productdetailname);
            prodetcat = (TextView)FindViewById(Resource.Id.productdetailcategory);
            prodetamount = (TextView)FindViewById(Resource.Id.productdetailamount);
            //prodetimage = (ImageView)FindViewById(Resource.Id.productdetailiv);
            
            buy = (Button)FindViewById(Resource.Id.buybtn);
            cart = (Button)FindViewById(Resource.Id.cartbtn);

            selectedProduct = JsonConvert.DeserializeObject<Product>(Intent.GetStringExtra("productdetail"));

            prodetname.Text = selectedProduct.productname.ToUpper().ToString();
            prodetcat.Text = selectedProduct.category.ToUpper().ToString();
            prodetamount.Text = selectedProduct.amount.ToString();

            database = databaseservice.GetDataBase();

            buyinguser = SecondPageActivity.loggedinuser;

            username = buyinguser.Fullname;

            FetchCartData();

            productadded = CheckIfProductExistsInCart();

            if (productadded)
            {
                cart.Text = "Go to cart";
            }



            buy.Click += Buybtn_Click;

            cart.Click += Cart_Click;



            // Create your application here
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;

            //while (!snapshot.IsEmpty)
            //{
            //    snapshot = (QuerySnapshot)result;
            //}

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                cartitems.Clear();

                foreach (DocumentSnapshot item in documents)
                {
                    Cart cartitem = new Cart();
                    cartitem.ID = item.Id;
                    cartitem.buyername = item.Get("buyername").ToString();
                    cartitem.productname = item.Get("productname") != null ? item.Get("productname").ToString() : "";
                    cartitem.productamount = item.GetLong("productamount").IntValue() != 0 ? item.GetLong("productamount").IntValue() : 1;
                    cartitem.unitprice = item.GetLong("unitprice").IntValue() != 0 ? item.GetLong("unitprice").IntValue() : 1;
                    //cartitem.productimgurl 
                    //user.ProfileImage = item.Get("profileimage") != null ? item.Get("profileimage").ToString() : "";

                    cartitems.Add(cartitem);
                }
            }
        }

        private void Cart_Click(object sender, EventArgs e)
        {

            if (productadded)
            {
                Intent cartpage = new Intent(this, typeof(CartActivity));
                //productpage.PutExtra("productdetail", JsonConvert.SerializeObject(selectedproduct));
                StartActivity(cartpage);
            }
            else
            {
                DocumentReference docRef = database.Collection("carts").Document();

                HashMap map = new HashMap();
                map.Put("buyername", username);
                map.Put("productname", selectedProduct.productname);
                map.Put("imageurl", selectedProduct.imageurl);
                map.Put("unitprice", 250);
                //map.Put("productimgurl", selectedProduct.);

                docRef.Set(map);

                Toast.MakeText(this, "Product Added to Cart", ToastLength.Long).Show();

                cart.Text = "Go To Cart";
            }
            //database = databaseservice.GetDataBase();

            //User buyinguser = new User();

            //buyinguser = SecondPageActivity.loggedinuser;

            //username = buyinguser.Fullname;

            

        }


        private void Buybtn_Click(object sender, EventArgs e)
        {
            selectedProduct.amount -= 1;

            database=databaseservice.GetDataBase();

            DocumentReference docRef = database.Collection("products").Document(selectedProduct.ID);

            docRef.Update("amount", selectedProduct.amount);

            //SecondPageActivity loggedinuser = new SecondPageActivity();

           

            var chars = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890";

            var stringchars = new char[5];

            var rand = new Random();

            for(int i = 0; i<stringchars.Length; i++)
            {
                stringchars[i] = chars[rand.Next(chars.Length)];
            }

            var orderidstring = new String(stringchars);


            HashMap map = new HashMap();
            map.Put("orderid", orderidstring);
            map.Put("buyername", username);
            map.Put("productname",selectedProduct.productname);
            map.Put("productamount", 1);
            map.Put("productprice", 250);
            map.Put("buyerlocation", "Debere Abay St.");
            map.Put("buyerphone", "0944194561");
            //map.Put("profileimage", Profileimg);

            docRef = database.Collection("orders").Document();

            docRef.Set(map);


            var trans = SupportFragmentManager.BeginTransaction();
            var buyproductfragment = new BuyFragment();

            buyproductfragment.Show(trans, "buyproduct");

            var SMSMessenger = CrossMessaging.Current.SmsMessenger;
            if (SMSMessenger.CanSendSmsInBackground)
            {
                SMSMessenger.SendSmsInBackground("251944194561", $"This is from Burnos Clothes.\n\nYour Order number is {orderidstring.ToUpper()}.");
            }
            

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("burnosc@gmail.com");
                mail.To.Add("dannyboy9917@gmail.com");
                mail.Subject = "Burnos Clothes Order Successful";
                mail.Body = $"Well hello there {username}. This is from Selvaj clothes.\nThis is ur Order ID:\n\n{orderidstring}\n\nUse this id to recieve ur package from our store when ur products arrives";

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential("burnosc@gmail.com", "burnos123");

                ServicePointManager.ServerCertificateValidationCallback = delegate (object zsender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslpolicyerrors)
                {
                    return true;
                };

                SmtpServer.Send(mail);


            }
            catch(Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long);
            }



            //var trans = SupportFragmentManager.BeginTransaction();
            //var buyproductfragment = new BuyFragment();

            //buyproductfragment.Show(trans, "buyproduct");






        }


        

        public bool CheckIfProductExistsInCart()
        {
            foreach (var item in cartitems)
            {
                if (selectedProduct.productname.Equals(item.productname) && username.Equals(item.buyername))
                {
                    
                    return true;
                }

            }

            return false;
        }

        public void FetchCartData()
        {
            database.Collection("carts").Get()
                .AddOnSuccessListener(this);

        }
    }
}