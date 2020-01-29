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
using FFImageLoading;
using Project_OCS_Second.DataModels;

namespace Project_OCS_Second.Adapters
{
    class CartAdapter : RecyclerView.Adapter
    {
        public event EventHandler<CartAdapterClickEventArgs> ItemClick;
        public event EventHandler<CartAdapterClickEventArgs> DeleteItemClick;

        List<Cart> cartlist;


        public CartAdapter(List<Cart> data)
        {
            cartlist = data;


        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var product = cartlist[position];
            //var url = imageurls[position];


            // Replace the contents of the view with that element
            var rowholder = holder as CartAdapterViewHolder;

            //ImageService.Instance.LoadUrl(imageurls[position])
            //    .Retry(3, 200)
            //    .DownSample(100, 100)
            //    .Into(holder.productimage);



            rowholder.cartproductname.Text = product.productname;
            rowholder.cartunitprice.Text = 250.ToString();

            ImageService.Instance.LoadUrl(product.productimgurl)
               .Retry(3, 200)
               .DownSample(100, 100)
               .Into(rowholder.cartproductimage);


        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = null;

            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.CartRow, parent, false);
            var vh = new CartAdapterViewHolder(itemView, OnClick, OnDeleteClick);
            return vh;
        }

        public override int ItemCount => cartlist.Count;

        void OnClick(CartAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnDeleteClick(CartAdapterClickEventArgs args) => DeleteItemClick?.Invoke(this, args);


        //void OnLongClick(DataAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class CartAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }

        public TextView cartproductname { get; set; }
        public TextView cartunitprice { get; set; }
        public ImageView cartproductimage { get; set; }
        public ImageView cartproductdelete { get; set; }
        //public ImageView productpicurl { get; set; }




        public CartAdapterViewHolder(View itemView, Action<CartAdapterClickEventArgs> clickListener, Action<CartAdapterClickEventArgs> deleteclicklistener) : base(itemView)
        {
            //TextView = v;
            cartproductname = (TextView)itemView.FindViewById(Resource.Id.cartresultname);
            cartunitprice = (TextView)itemView.FindViewById(Resource.Id.cartresultprice);
            cartproductimage = (ImageView)itemView.FindViewById(Resource.Id.cartresultiv);
            cartproductdelete = (ImageView)itemView.FindViewById(Resource.Id.cartdeleteiv);


            //proidtext = (TextView)itemView.FindViewById(Resource.Id.idtext);
            // productpicurl = (ImageView)itemView.FindViewById(Resource.Id.productlistiv);

            itemView.Click += (sender, e) => clickListener(new CartAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            cartproductdelete.Click += (sender, e) => deleteclicklistener(new CartAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            //itemView.LongClick += (sender, e) => longClickListener(new DataAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class CartAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }

}

