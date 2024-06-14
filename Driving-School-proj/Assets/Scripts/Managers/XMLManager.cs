using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using Managers;

namespace Managers
{
    public class XMLManager: MonoBehaviour
    {
        public static XMLManager Instance { get; private set; }
        public RoutesCollection routesCollection;
    
        private void Awake()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            if (!Directory.Exists(Application.persistentDataPath + "/Routes/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Routes/");
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Save(List<ComponentObject[,]> scoresToSave)
        {
            routesCollection.list = scoresToSave;
            XmlSerializer serializer = new XmlSerializer(typeof(RoutesCollection));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Routes/routes.xml", FileMode.Create);
            serializer.Serialize(stream, routesCollection);
            stream.Close();
        }

        public List<ComponentObject[,]> Load()
        {
            if (File.Exists(Application.persistentDataPath + "/Routes/routes.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RoutesCollection));
                FileStream stream = new FileStream(Application.persistentDataPath + "/Routes/routes.xml", FileMode.Open);
                routesCollection = serializer.Deserialize(stream) as RoutesCollection;
            }

            return routesCollection.list;
        }
    }
}

[System.Serializable]
public class RoutesCollection
{
    public List<ComponentObject[,]> list = new List<ComponentObject[,]>();
}

