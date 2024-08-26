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
        public ScoresCollection scoresCollection;
    
        private void Awake()
        {
            // Singleton
            Instance = this;
            if (!Directory.Exists(Application.persistentDataPath + "/Scores/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Scores/");
            }
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
