using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Project_OCS_Second.DataModels;
using Firebase.Storage;
using Android.App;

namespace Project_OCS_Second.Adapters
{
    class DataAdapter : RecyclerView.Adapter
    {
        public event EventHandler<DataAdapterClickEventArgs> ItemClick;
        public event EventHandler<DataAdapterClickEventArgs> ItemLongClick;
        List<Product> productlist;

        public DataAdapter(List<Product> data)
        {
            productlist = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
           
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.row, parent, false);
            var vh = new DataAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var product = productlist[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as DataAdapterViewHolder;
            holder.productname.Text = product.productname;
            holder.category.Text = product.category;
            holder.amount.Text = product.amount.ToString();
            holder.proidtext.Text = product.ID;


            //StorageReference ppref = FirebaseStorage.Instance.GetReference($"products/{product.category}/{product.productname}_image");

            //ppref.GetDownloadUrl().AddOnSuccessListener(Application.Context, this);

            //holder.ppicurl.Text = user.ProfileImage != null ? user.ProfileImage.ToString() : "No URL";

        }

        public override int ItemCount => productlist.Count;

        void OnClick(DataAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(DataAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class DataAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }

        public TextView productname { get; set; }
        public TextView category { get; set; }
        public TextView amount { get; set; }
        public TextView proidtext { get; set; }
        //public ImageView productpicurl { get; set; }




        public DataAdapterViewHolder(View itemView, Action<DataAdapterClickEventArgs> clickListener,
                            Action<DataAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            productname = (TextView)itemView.FindViewById(Resource.Id.fullnameText);
            category = (TextView)itemView.FindViewById(Resource.Id.emailText);
            amount = (TextView)itemView.FindViewById(Resource.Id.passwordtext);
            proidtext = (TextView)itemView.FindViewById(Resource.Id.idtext);
           // productpicurl = (ImageView)itemView.FindViewById(Resource.Id.productlistiv);

            itemView.Click += (sender, e) => clickListener(new DataAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new DataAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class DataAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}