using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

namespace Managers
{
    public class XMLManager: MonoBehaviour
    {
        public static XMLManager Instance { get; private set; }
        public RoutesCollection routesCollection;
        public ScoresCollection scoresCollection;
    
        private void Awake()
        {
            // Singleton
            Instance = this;
            
            if (!Directory.Exists(Application.persistentDataPath + "/Routes/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Routes/");
            }
            if (!Directory.Exists(Application.persistentDataPath + "/Scores/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Scores/");
            }
        }
        
        public void SaveRoutes(List<MapMatrixObject> matrixListToSave)
        {
            DatabaseManager.Instance.CreateRoutes(matrixListToSave);
            /*routesCollection.list = matrixListToSave; 
            XmlSerializer serializer = new XmlSerializer(typeof(RoutesCollection));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Routes/routes.xml", FileMode.Create);
            serializer.Serialize(stream, routesCollection);
            stream.Close();*/
        }

        public void LoadRoutes(Action<List<MapMatrixObject>> OnRoutesFetched)
        {
            StartCoroutine(DatabaseManager.Instance.GetRoutes(OnRoutesFetched));
            /*if (File.Exists(Application.persistentDataPath + "/Routes/routes.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RoutesCollection));
                FileStream stream = new FileStream(Application.persistentDataPath + "/Routes/routes.xml", FileMode.Open);
                routesCollection = serializer.Deserialize(stream) as RoutesCollection;
                stream.Close();
            }

            return routesCollection.list;
            */
        }
        
        public void SaveScores(List<ScoresObject> scoresObjectListToSave)
        {
            scoresCollection.list = scoresObjectListToSave; 
            XmlSerializer serializer = new XmlSerializer(typeof(ScoresCollection));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Scores/scores.xml", FileMode.Create);
            serializer.Serialize(stream, scoresCollection);
            stream.Close();
        }

        public List<ScoresObject> LoadScores()
        {
            if (File.Exists(Application.persistentDataPath + "/Scores/scores.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ScoresCollection));
                FileStream stream = new FileStream(Application.persistentDataPath + "/Scores/scores.xml", FileMode.Open);
                scoresCollection = serializer.Deserialize(stream) as ScoresCollection;
                stream.Close();
            }

            return scoresCollection.list;
        }

        /*private void OnRoutesFetched(List<MapMatrixObject> routes)
        {
            // Handle the fetched data
            foreach (var route in routes)
            {
                Debug.Log($"Matrix Name: {route.name}");
                foreach (var cell in route.mapCellObjectsList)
                {
                    Debug.Log($"Cell at ({cell.row}, {cell.col}) with component number: {cell.componentObject.componentNumber}");
                }
            }
        }*/
    }
}

[System.Serializable]
public class RoutesCollection
{
    [XmlElement("MatrixObject")]
    public List<MapMatrixObject> list = new List<MapMatrixObject>();

    public RoutesCollection()
    {
    }
}

[System.Serializable]
public class ScoresCollection
{
    [XmlElement("ScoresObjectList")]
    public List<ScoresObject> list = new List<ScoresObject>();

    public ScoresCollection()
    {
    }
}
