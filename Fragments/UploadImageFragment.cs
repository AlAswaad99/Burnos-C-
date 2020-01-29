using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.Media;

namespace Project_OCS_Second.Fragments
{
    public class UploadImageFragment : Android.Support.V4.App.DialogFragment
    {

        TextView fromgallery;
        TextView fromcamera;

        //MainActivity imgservices = new MainActivity();
        private byte[] imageArray;
        private Bitmap bitmap;

        public delegate void onSetImage(Bitmap bitmap, byte[] imgary);
        public event onSetImage uploadImage;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.ProfilePicMethod, container, false);

            fromgallery = (TextView)view.FindViewById(Resource.Id.galleryinput);
            fromcamera = (TextView)view.FindViewById(Resource.Id.camerainput);

            fromcamera.Click += Fromcamera_Click;

            fromgallery.Click += Fromgallery_Click;

            return view;
        }

         public async void Fromgallery_Click(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();



            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 40

            });

            // Convert file to byre array, to bitmap and set it to our ImageView

            imageArray = System.IO.File.ReadAllBytes(file.Path);
            bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            //profimg.SetImageBitmap(bitmap);
            uploadImage(bitmap, imageArray);


        }

        private async void Fromcamera_Click(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "myimage.jpg",
                Directory = "sample"

            });

            if (file == null)
            {
                return;
            }

            // Convert file to byte array and set the resulting bitmap to imageview
            imageArray = System.IO.File.ReadAllBytes(file.Path);
            bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            //profimg.SetImageBitmap(bitmap);
            uploadImage(bitmap, imageArray);


            //uploadimg.Dismiss();
        }



        //public async void UploadPhoto()
        //{

        //    await CrossMedia.Current.Initialize();



        //    var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
        //    {
        //        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
        //        CompressionQuality = 40

        //    });

        //    // Convert file to byre array, to bitmap and set it to our ImageView

        //    imageArray = System.IO.File.ReadAllBytes(file.Path);
        //    bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
        //    //profimg.SetImageBitmap(bitmap);
        //}

    }
}