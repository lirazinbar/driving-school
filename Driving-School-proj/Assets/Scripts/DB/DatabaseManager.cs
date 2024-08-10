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

    [SerializeField] private TMP_InputField Name;
    [SerializeField] private TMP_InputField Gold;
    
    void Awake()
    {
        // Singleton
        Instance = this;
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

    public void CreateUser()
    {
        User newUser = new User(Name.text, 999);
        // User newUser = new User("111", 22);

        string json = JsonUtility.ToJson(newUser);
        
        Debug.Log("json: "+ json);
        Debug.Log("dbReference: "+ dbReference);
        
        dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
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
