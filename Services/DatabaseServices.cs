using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Lang;
using Plugin.Connectivity;
using Project_OCS_Second.DataModels;

namespace Project_OCS_Second.Services
{
    class DatabaseServices
    {

        FirebaseFirestore database;

        //public IntPtr Handle => throw new NotImplementedException();

        //public IntPtr Handle => throw new NotImplementedException();

        public FirebaseFirestore GetDataBase()
        {

            var app = FirebaseApp.InitializeApp(Application.Context);

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                .SetProjectId("projectocs-f29de")
                .SetApplicationId("projectocs-f29de")
                .SetApiKey("AIzaSyB-qy3coTZrsGo_EWpiTsbGxUnXQK3CDqI")
                .SetDatabaseUrl("https://projectocs-f29de.firebaseio.com")
                .SetStorageBucket("projectocs-f29de.appspot.com")
                .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options);
            }




            database = FirebaseFirestore.GetInstance(app);

            return database;
        }

        public FirebaseAuth GetFirebaseAuth()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);

            FirebaseAuth mAuth;

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                .SetProjectId("projectocs-f29de")
                .SetApplicationId("projectocs-f29de")
                .SetApiKey("AIzaSyB-qy3coTZrsGo_EWpiTsbGxUnXQK3CDqI")
                .SetDatabaseUrl("https://projectocs-f29de.firebaseio.com")
                .SetStorageBucket("projectocs-f29de.appspot.com")
                .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options);
                mAuth = FirebaseAuth.Instance;

            }
            else
            {
                mAuth = FirebaseAuth.Instance;

            }


            return mAuth;

        }


        public void deleteItem(string documnet, string id)
        {
            DocumentReference docRef = database.Collection(documnet).Document(id);
            docRef.Delete();

        }

        public void OnSuccess(Java.Lang.Object result)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}