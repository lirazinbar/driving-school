using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using Firebase.Database;
using UnityEngine;
using Firebase.Extensions;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;


/*Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
    var dependencyStatus = task.Result;
    if (dependencyStatus == Firebase.DependencyStatus.Available) {
        // Create and hold a reference to your FirebaseApp,
        // where app is a Firebase.FirebaseApp property of your application class.
        app = Firebase.FirebaseApp.DefaultInstance;

        // Set a flag here to indicate whether Firebase is ready to use by your app.
    } else {
        UnityEngine.Debug.LogError(System.String.Format(
            "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        // Firebase Unity SDK is not safe to use here.
    }
});*/

public class DatabaseManager : MonoBehaviour
{
    private FirebaseApp app;
    public static DatabaseManager Instance { get; private set; }
    private string userId;
    private DatabaseReference dbReference;

    
    void Awake()
    {
        // If an instance of FirebaseManager already exists, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the singleton instance
        Instance = this;

        DontDestroyOnLoad(gameObject);

        // Initialize Firebase
        InitializeFirebase();
    }
    
    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }
    void Start()
    {
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            app = FirebaseApp.DefaultInstance;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
        
        userId = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    
    public void CreateRoutes(List<MapMatrixObject> matrixListToSave)
    {
        string json =  JsonConvert.SerializeObject(matrixListToSave, Formatting.Indented);
        
        Debug.Log("json: "+ json);
        Debug.Log("dbReference: "+ dbReference);
        
        dbReference.Child("Routes").Child(userId).SetRawJsonValueAsync(json);
    }

    public IEnumerator GetRoutes(Action<List<MapMatrixObject>> onCallBack)
    {
        Debug.Log("dbReference: "+ dbReference);
        Debug.Log("userId: "+ userId);

        var routesDate = dbReference.Child("Routes").Child(userId).GetValueAsync();

        yield return new WaitUntil(predicate: () => routesDate.IsCompleted);

        if (routesDate != null)
        {
            DataSnapshot snapshot = routesDate.Result;
            string json = snapshot.GetRawJsonValue();
            List<MapMatrixObject> matrixList = JsonConvert.DeserializeObject<List<MapMatrixObject>>(json);

            onCallBack.Invoke(matrixList);
        }

    }
}
