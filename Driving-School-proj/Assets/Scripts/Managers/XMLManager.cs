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
            string filePath = Path.Combine(Application.persistentDataPath, "Scores/scores.xml");
            if (File.Exists(filePath))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ScoresCollection));
                    using (FileStream stream = new FileStream(filePath, FileMode.Open))
                    {
                        scoresCollection = serializer.Deserialize(stream) as ScoresCollection;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load scores from {filePath}: {ex.Message}");
                    // Optionally, handle the exception (e.g., return an empty list or rethrow)
                    return new List<ScoresObject>();
                }
            }
            else
            {
                Debug.LogWarning($"Scores file not found at {filePath}");
                return new List<ScoresObject>();
            }
            
            return scoresCollection?.list ?? new List<ScoresObject>();
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
