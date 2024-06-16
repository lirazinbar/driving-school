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
    
        private void Awake()
        {
            // Singleton
            Instance = this;
            
            if (!Directory.Exists(Application.persistentDataPath + "/Routes/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Routes/");
            }
        }
        
        public void Save(List<MapMatrixObject> matrixListToSave)
        {
            routesCollection.list = matrixListToSave; 
            XmlSerializer serializer = new XmlSerializer(typeof(RoutesCollection));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Routes/routes.xml", FileMode.Create);
            serializer.Serialize(stream, routesCollection);
            stream.Close();
        }

        public List<MapMatrixObject> Load()
        {
            if (File.Exists(Application.persistentDataPath + "/Routes/routes.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RoutesCollection));
                FileStream stream = new FileStream(Application.persistentDataPath + "/Routes/routes.xml", FileMode.Open);
                routesCollection = serializer.Deserialize(stream) as RoutesCollection;
                stream.Close();
            }

            return routesCollection.list;
        }
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

